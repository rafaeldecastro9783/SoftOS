using System.Net;
using Microsoft.AspNetCore.Mvc;
using SoftOS.Shared.Utils;

namespace SoftOS.Shared.Exceptions
{
    public class ContextResultException(HttpStatusCode statusCode, string mensagem = "") : Exception
    {
        public readonly HttpStatusCode StatusCode = statusCode;
        public readonly string Mensagem = mensagem;

        public IActionResult ToObjectResult() =>
            new ObjectResult(new { mensagem = Mensagem }) { StatusCode = (int)StatusCode };

        public override string ToString() => JsonUtil.Serializar(ToObject());

        private object ToObject() => new { Mensagem };
    }
}
