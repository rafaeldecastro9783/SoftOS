using Microsoft.AspNetCore.Mvc;
using SoftOS.BLL.Áttributes;
using SoftOS.BLL.Models;
using SoftOS.BLL.Services;
using SoftOS.Shared.Enums;

namespace SoftOS.API.Controllers
{
    [Autorizar(JwtSoPermissao.Ticket)]
    public class TicketController(ITicketService service) : AjaxController
    {
        private readonly ITicketService _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAsync() =>
            await ExecuteAsync(async () => Ok(await _service.ReadAllAsync()));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id) =>
            await ExecuteAsync(async () => Ok(await _service.ReadByIdAsync(id)));

        [HttpPost]
        public async Task<IActionResult> PostAsync(Ticket model)
        {
            return await ExecuteAsync(async () =>
            {
                var ticket = await _service.CreateAsync(model);
                return Created($"/{ticket.Id}", ticket);
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutByIdAsync(int id, Ticket model) =>
            await ExecuteAsync(async () => Ok(await _service.UpdateById(id, model)));

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteByIdAsync(int id)
        {
            return await ExecuteAsync(async () =>
            {
                await _service.DeleteByIdAsync(id);
                return NoContent();
            });
        }
    }
}
