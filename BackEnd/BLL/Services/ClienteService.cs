using System.Net;
using Microsoft.EntityFrameworkCore;
using SoftOS.BLL.Models;
using SoftOS.DAL.Context;
using SoftOS.Shared.Enums;
using SoftOS.Shared.Exceptions;

namespace SoftOS.BLL.Services
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> ReadAllAsync();
        Task<Cliente> CreateAsync(Cliente model);
        Task<Cliente> ReadByIdAsync(int Id);
        Task<Cliente> UpdateById(int Id, Cliente model);
        Task DeleteByIdAsync(int Id);
    }

    public class ClienteService(AppDbContext context) : IClienteService
    {
        private readonly AppDbContext _context = context;

        public async Task<IEnumerable<Cliente>> ReadAllAsync() =>
            await _context.Clientes.ToArrayAsync();

        public async Task<Cliente> CreateAsync(Cliente model)
        {
            await _context.Clientes.AddAsync(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<Cliente> ReadByIdAsync(int id)
        {
            if (await _context.Clientes.FindAsync(id) is Cliente cliente)
                return cliente;

            throw new ServiceException(
                HttpStatusCode.NotFound,
                TemaModal.Aviso,
                "Cliente não encontrado",
                "Não foi encontrado nenhum cliente correspondente"
            );
        }

        public async Task<Cliente> UpdateById(int Id, Cliente model)
        {
            var cliente = await ReadByIdAsync(Id);
            _context.Entry(cliente).CurrentValues.SetValues(model);
            _context.SaveChanges();
            return model;
        }

        public async Task DeleteByIdAsync(int Id)
        {
            var cliente = await ReadByIdAsync(Id);
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
        }
    }
}
