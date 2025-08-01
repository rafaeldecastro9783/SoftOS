using System.Net;
using Microsoft.EntityFrameworkCore;
using SoftOS.BLL.Models;
using SoftOS.DAL.Context;
using SoftOS.Shared.Enums;
using SoftOS.Shared.Exceptions;

namespace SoftOS.BLL.Services
{
    public interface ITicketService
    {
        Task<IEnumerable<Ticket>> ReadAllAsync();
        Task<Ticket> CreateAsync(Ticket model);
        Task<Ticket> ReadByIdAsync(int Id);
        Task<Ticket> UpdateById(int Id, Ticket model);
        Task DeleteByIdAsync(int Id);
    }

    public class TicketService(AppDbContext context) : ITicketService
    {
        private readonly AppDbContext _context = context;

        public async Task<IEnumerable<Ticket>> ReadAllAsync() =>
            await _context.Tickets.ToArrayAsync();

        public async Task<Ticket> CreateAsync(Ticket model)
        {
            await _context.Tickets.AddAsync(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<Ticket> ReadByIdAsync(int id)
        {
            if (await _context.Tickets.FindAsync(id) is Ticket cliente)
                return cliente;

            throw new ServiceException(
                HttpStatusCode.NotFound,
                TemaModal.Aviso,
                "Cliente não encontrado",
                "Não foi encontrado nenhum cliente correspondente"
            );
        }

        public async Task<Ticket> UpdateById(int Id, Ticket model)
        {
            var ticket = await ReadByIdAsync(Id);
            _context.Entry(ticket).CurrentValues.SetValues(model);
            _context.SaveChanges();
            return model;
        }

        public async Task DeleteByIdAsync(int Id)
        {
            var ticket = await ReadByIdAsync(Id);
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
        }
    }
}
