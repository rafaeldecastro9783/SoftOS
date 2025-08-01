using Microsoft.AspNetCore.Mvc;
using SoftOS.BLL.Models;
using SoftOS.BLL.Services;
using SoftOS.Shared.DTOs;

namespace SoftOS.API.Controllers;

public class AuthController(IAuthService service) : AjaxController
{
    private readonly IAuthService _service = service;

    [HttpPost("login/profissional")]
    public async Task<IActionResult> LoginProfissional(AuthRequestDTO model) =>
        await ExecuteAsync(async () =>
        {
            string token = await _service.CreateLoginAsync<Profissional>(model);
            return Ok(token);
        }
    );

    [HttpPost("login/cliente")]
    public async Task<IActionResult> LoginCliente(AuthRequestDTO model) =>
        await ExecuteAsync(async () =>
        {
            string token = await _service.CreateLoginAsync<Cliente>(model);
            return Ok(token);
        }
    );
}
