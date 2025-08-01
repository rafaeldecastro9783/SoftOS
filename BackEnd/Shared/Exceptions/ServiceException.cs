using System.Net;
using Microsoft.AspNetCore.Mvc;
using SoftOS.Shared.Enums;

namespace SoftOS.Shared.Exceptions;

public class ServiceException(
    HttpStatusCode statusCode,
    TemaModal tema = TemaModal.Ignore,
    string titulo = "",
    string texto = ""
) : Exception
{
    public static readonly ServiceException DadosRequisicaoInvalidos = new(
        HttpStatusCode.BadRequest,
        TemaModal.Erro,
        "Dados de requisição inválidos",
        "O conteúdo da requisição não está conforme o esperado"
    );

    public readonly HttpStatusCode StatusCode = statusCode;

    public readonly TemaModal Tema = tema;
    public readonly string Titulo = titulo;
    public readonly string Texto = texto;

    public IActionResult ToObjectResult() =>
        new ObjectResult(ToObject()) { StatusCode = (int)StatusCode };

    public override string ToString() => System.Text.Json.JsonSerializer.Serialize(ToObject());

    private object ToObject() =>
        new
        {
            Titulo,
            Texto,
            Tema
        };
}
