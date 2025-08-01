using Microsoft.AspNetCore.Mvc;
using SoftOS.BLL.Áttributes;
using SoftOS.BLL.Models;
using SoftOS.BLL.Services;
using SoftOS.Shared.Enums;

namespace SoftOS.API.Controllers;

[Autorizar(JwtSoPermissao.Empresa)]

public class UsuarioController(IUsuarioService service) : AjaxController
{
    private readonly IUsuarioService _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAsync() =>
        await ExecuteAsync(async () => Ok(await _service.ReadAllAsync()));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id) =>
        await ExecuteAsync(async () => Ok(await _service.ReadByIdAsync(id)));

    [HttpPost]
    public async Task<IActionResult> PostAsync(Empresa model) =>
        await ExecuteAsync(async () =>
        {
            var usuario = await _service.CreateAsync(model);
            return Created($"/{usuario.Id}", usuario);
        });

    [HttpPut("{id}")]
    public async Task<IActionResult> PutByIdAsync(int id, Empresa model) =>
        await ExecuteAsync(async () => Ok(await _service.UpdateAsync(id, model)));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteByIdAsync(int id) =>
        await ExecuteAsync(async () =>
        {
            await _service.DeleteByIdAsync(id);
            return NoContent();
        });
}
