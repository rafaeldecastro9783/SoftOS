using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using SoftOS.Shared.DTOs;
using SoftOS.Shared.Enums;
using SoftOS.Shared.Exceptions;

namespace SoftOS.Shared.Utils
{
    public class AuthorizationUtil
    {
        private const string ExpirationTime = nameof(JwtSoDTO.Exp);
        private const string IssuedAtTime = nameof(JwtSoDTO.Iat);
        private const string NotBeforeTime = nameof(JwtSoDTO.Nbf);

        public const string Permissoes = nameof(JwtSoDTO.Pms);
        public const string authId = nameof(JwtSoDTO.Aid);
        public const string Nome = nameof(JwtSoDTO.Nom);

        public static void ValidarUsuario(ClaimsPrincipal usuario)
        {
            if (
                usuario.Identity is not IIdentity usuarioIdentity
                || !usuarioIdentity.IsAuthenticated
            )
                throw new ContextResultException(
                    HttpStatusCode.Unauthorized,
                    "Ação não autorizada"
                );
        }

        public static void ValidarClaims(ClaimsPrincipal usuario, JwtSoPermissao? permissao = null)
        {
            if (
                usuario.FindFirstValue(ExpirationTime) is not string exp
                || usuario.FindFirstValue(IssuedAtTime) is not string iat
                || usuario.FindFirstValue(NotBeforeTime) is not string nbf
                || usuario.FindFirstValue(Permissoes) is not string pms
                || usuario.FindFirstValue(authId) is not string aid
                || usuario.FindFirstValue(Nome) is not string nom
            )
            {
                throw new ContextResultException(HttpStatusCode.BadRequest, "Claims JWT inválidas");
            }
            else
            {
                JwtSoDTO jwtSo = new()
                {
                    Pms = (JwtSoPermissao)Convert.ToInt32(pms, 2),
                    Exp = int.Parse(exp),
                    Aid = int.Parse(aid),
                    Iat = int.Parse(iat),
                    Nbf = int.Parse(nbf),
                    Nom = nom,
                };
                if (jwtSo.Aid <= 0)
                    throw new ContextResultException(HttpStatusCode.BadRequest, "Usuário inválido");
                else if (permissao is not null && !jwtSo.Pms.HasFlag(permissao.Value))
                    throw new ContextResultException(
                        HttpStatusCode.Forbidden,
                        "Ação não permitida"
                    );
            }
        }
    }
}
