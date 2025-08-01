using Microsoft.AspNetCore.Mvc;
using SoftOS.BLL.Áttributes;
using SoftOS.BLL.Models;
using SoftOS.BLL.Services;
using SoftOS.Shared.Enums;

namespace SoftOS.API.Controllers
{
    [Autorizar(JwtSoPermissao.OrdemServico)]
    public class TipoServicoController(ITipoServicoService service) : AjaxController
    {
        private readonly ITipoServicoService _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAsync() =>
            await ExecuteAsync(async () => Ok(await _service.ReadAllAsync()));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id) =>
            await ExecuteAsync(async () => Ok(await _service.ReadByIdAsync(id)));

        [HttpPost]
        public async Task<IActionResult> PostAsync(TipoServico model) =>
            await ExecuteAsync(async () =>
            {
                var tipoServico = await _service.CreateAsync(model);
                return Created($"/{tipoServico.Id}", tipoServico);
            });

        [HttpPut("{id}")]
        public async Task<IActionResult> PutByIdAsync(int id, TipoServico model) =>
            await ExecuteAsync(async () => Ok(await _service.UpdateById(id, model)));

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteByIdAsync(int id) =>
            await ExecuteAsync(async () =>
            {
                await _service.DeleteByIdAsync(id);
                return NoContent();
            });
    }
}
