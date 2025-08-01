using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SoftOS.BLL.Models;
using SoftOS.Shared.Enums;

namespace SoftOS.Shared.Utils
{
    public class JwtUtil
    {
        private const int _minutosRenovar = 30;
        private const string _secretKeyJwt =
            $@"
And all I am is a man
I want the world in my hands
I hate the beach
But I stand in California with my toes in the sand
Use the sleeves of my sweater
Let's have an adventure
Head in the clouds but my gravity centered
Touch my neck and I'll touch yours
You in those little high waisted shorts, oh";

        public static ClaimsPrincipal ExtrairClaimsPrincipal(string token)
        {
            byte[] key = Encoding.ASCII.GetBytes(_secretKeyJwt);
            var tokenHandler = new JwtSecurityTokenHandler();

            TokenValidationParameters validationParameters = new()
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateIssuer = false,
            };
            return tokenHandler.ValidateToken(token, validationParameters, out _);
        }

        public static bool TokenValido(HttpContext context)
        {
            if (
                context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last()
                is not string token
            )
                return false;

            if (TokenExpirando(token))
            {
                token = TokenRenovar(token);
                context.Request.Headers.Authorization = token;
                context.Response.Headers.Authorization = token;
            }

            if (TokenValidar(token) is JwtSecurityToken jwt)
            {
                ClaimsIdentity claimsIdentity = new(jwt.Claims, "padrao");
                context.User = new ClaimsPrincipal(claimsIdentity);
                return true;
            }

            return false;
        }

        public static string TokenGerar(ILoginModel model, int horasExpirar = 1)
        {
            if (model is Cliente cliente)
                return TokenGerar(cliente, horasExpirar);

            if (model is Profissional profissional)
                return TokenGerar(profissional, horasExpirar);

            throw new NotImplementedException($"O tipo implementado de {nameof(ILoginModel)} não pôde ser validado");
        }

        public static string TokenGerar(Cliente model, int horasExpirar = 1)
        {
            JwtSecurityTokenHandler tokenHandler = new();

            byte[] key = Encoding.ASCII.GetBytes(_secretKeyJwt);
            ICollection<Claim> claims =
            [
                new Claim(AuthorizationUtil.authId, model.Id.ToString(), ClaimValueTypes.Integer),
                new Claim(AuthorizationUtil.Nome, model.Nome, ClaimValueTypes.String),
            ];

            claims.Add(
                new(
                    AuthorizationUtil.Permissoes,
                    Convert
                        .ToString(
                            (int)JwtSoPermissao.Ticket,
                            2
                        )
                        .PadLeft(Enum.GetNames<JwtSoPermissao>().Length, '0')
                )
            );

            SecurityToken token = TokenCriar(tokenHandler, claims, key, horasExpirar);

            return tokenHandler.WriteToken(token);
        }

        public static string TokenGerar(Profissional model, int horasExpirar = 1)
        {
            JwtSecurityTokenHandler tokenHandler = new();

            byte[] key = Encoding.ASCII.GetBytes(_secretKeyJwt);
            ICollection<Claim> claims =
            [
                new Claim(AuthorizationUtil.authId, model.Id.ToString(), ClaimValueTypes.Integer),
                new Claim(AuthorizationUtil.Nome, model.Nome, ClaimValueTypes.String),
            ];

            if (model.Cargo == ProfissionalCargo.Administrador)
                claims.Add(
                    new(
                        AuthorizationUtil.Permissoes,
                        Convert
                            .ToString(
                                (int)(
                                    JwtSoPermissao.Profissional
                                    | JwtSoPermissao.Empresa
                                    | JwtSoPermissao.Ticket
                                    | JwtSoPermissao.OrdemServico
                                    | JwtSoPermissao.Cliente
                                ),
                                2
                            )
                            .PadLeft(Enum.GetNames<JwtSoPermissao>().Length, '0')
                    )
                );
            if (model.Cargo != ProfissionalCargo.Administrador)
                claims.Add(
                    new(
                        AuthorizationUtil.Permissoes,
                        Convert
                            .ToString(
                                (int)(
                                    JwtSoPermissao.Profissional
                                    | JwtSoPermissao.Ticket
                                    | JwtSoPermissao.OrdemServico
                                ),
                                2
                            )
                            .PadLeft(Enum.GetNames<JwtSoPermissao>().Length, '0')
                    )
                );
            else
                claims.Add(
                    new(
                        AuthorizationUtil.Permissoes,
                        Convert
                            .ToString((int)JwtSoPermissao.None, 2)
                            .PadLeft(Enum.GetNames<JwtSoPermissao>().Length, '0')
                    )
                );

            SecurityToken token = TokenCriar(tokenHandler, claims, key, horasExpirar);

            return tokenHandler.WriteToken(token);
        }

        private static SecurityToken TokenCriar(
            SecurityTokenHandler tokenHandler,
            ICollection<Claim> claims,
            byte[] key,
            int horasExpirar = 1
        )
        {
            SigningCredentials signingCredentials = new(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256
            );
            DateTime expires = DateTime.UtcNow.AddHours(horasExpirar);
            ClaimsIdentity subject = new(claims, "padrao");

            return tokenHandler.CreateToken(
                new SecurityTokenDescriptor
                {
                    SigningCredentials = signingCredentials,
                    Subject = subject,
                    Expires = expires,
                }
            );
        }

        private static bool TokenExpirando(string token)
        {
            JwtSecurityTokenHandler tokenHandler = new();

            if (tokenHandler.CanReadToken(token))
            {
                TimeSpan intervalo = tokenHandler
                    .ReadJwtToken(token)
                    .ValidTo.Subtract(DateTime.UtcNow);
                return intervalo < TimeSpan.FromMinutes(_minutosRenovar)
                    && intervalo > TimeSpan.Zero;
            }

            return false;
        }

        private static string TokenRenovar(string token)
        {
            byte[] key = Encoding.ASCII.GetBytes(_secretKeyJwt);
            JwtSecurityTokenHandler tokenHandler = new();
            ICollection<Claim> claims = [.. tokenHandler.ReadJwtToken(token).Claims];
            SecurityToken tokenRenovado = TokenCriar(tokenHandler, claims, key);
            return tokenHandler.WriteToken(tokenRenovado);
        }

        private static SecurityToken? TokenValidar(string token)
        {
            try
            {
                byte[] key = Encoding.ASCII.GetBytes(_secretKeyJwt);
                new JwtSecurityTokenHandler().ValidateToken(
                    token,
                    new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                    },
                    out SecurityToken tokenValidado
                );
                return tokenValidado;
            }
            catch
            {
                return null;
            }
        }
    }
}
