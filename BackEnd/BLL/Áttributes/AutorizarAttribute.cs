using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using SoftOS.Shared.Enums;
using SoftOS.Shared.Exceptions;
using SoftOS.Shared.Utils;

namespace SoftOS.BLL.Áttributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AutorizarAttribute(JwtSoPermissao permissao = JwtSoPermissao.None)
        : Attribute,
            IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                ClaimsPrincipal user;

                if (
                    context.HttpContext.Request.Headers.TryGetValue(
                        "Authorization",
                        out StringValues authHeader
                    )
                )
                    user = context.HttpContext.User;
                else
                {
                    string? token = context.HttpContext.Request.Cookies["token"];
                    if (string.IsNullOrWhiteSpace(token))
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }

                    user = JwtUtil.ExtrairClaimsPrincipal(token);
                }

                AuthorizationUtil.ValidarUsuario(user);
                AuthorizationUtil.ValidarClaims(user, permissao);

                if (context.HttpContext.User != user)
                    context.HttpContext.User = user;
            }
            catch (ContextResultException ex)
            {
                context.Result = ex.ToObjectResult();
            }
            catch
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
