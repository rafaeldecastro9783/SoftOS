using Microsoft.AspNetCore.Mvc;
using SoftOS.BLL.Áttributes;
using SoftOS.BLL.Models;
using SoftOS.BLL.Services;
using SoftOS.Shared.Enums;

namespace SoftOS.API.Controllers;

[Autorizar(JwtSoPermissao.Cliente)]
public class ClienteController(IClienteService service) : AjaxController
{
    private readonly IClienteService _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAsync() =>
        await ExecuteAsync(async () => Ok(await _service.ReadAllAsync()));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id) =>
        await ExecuteAsync(async () => Ok(await _service.ReadByIdAsync(id)));

    [HttpPost]
    public async Task<IActionResult> PostAsync(Cliente model) =>
        await ExecuteAsync(async () =>
        {
            var cliente = await _service.CreateAsync(model);
            return Created($"/{cliente.Id}", cliente);
        });

    [HttpPut("{id}")]
    public async Task<IActionResult> PutByIdAsync(int id, Cliente model) =>
        await ExecuteAsync(async () => Ok(await _service.UpdateById(id, model)));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteByIdAsync(int id) =>
        await ExecuteAsync(async () =>
        {
            await _service.DeleteByIdAsync(id);
            return NoContent();
        });
}
