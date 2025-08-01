using System.Net;
using Microsoft.EntityFrameworkCore;
using SoftOS.BLL.Models;
using SoftOS.DAL.Context;
using SoftOS.Shared.Enums;
using SoftOS.Shared.Exceptions;

namespace SoftOS.BLL.Services
{
    public interface IProfissionalService
    {
        Task<IEnumerable<Profissional>> ReadAllAsync();
        Task<Profissional> CreateAsync(Profissional model);
        Task<Profissional> ReadByIdAsync(int Id);
        Task<Profissional> UpdateById(int Id, Profissional model);
        Task DeleteByIdAsync(int Id);
    }

    public class ProfissionalService(AppDbContext context) : IProfissionalService
    {
        private readonly AppDbContext _context = context;

        public async Task<IEnumerable<Profissional>> ReadAllAsync() =>
            await _context.Profissionais.ToArrayAsync();

        public async Task<Profissional> CreateAsync(Profissional model)
        {
            await _context.Profissionais.AddAsync(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<Profissional> ReadByIdAsync(int id)
        {
            if (await _context.Profissionais.FindAsync(id) is Profissional profissional)
                return profissional;

            throw new ServiceException(
                HttpStatusCode.NotFound,
                TemaModal.Aviso,
                "Profissional não encontrado",
                "Não foi encontrado nenhum Profissional correspondente"
            );
        }

        public async Task<Profissional> UpdateById(int Id, Profissional model)
        {
            var profissional = await ReadByIdAsync(Id);
            _context.Entry(profissional).CurrentValues.SetValues(model);
            _context.SaveChanges();
            return model;
        }

        public async Task DeleteByIdAsync(int Id)
        {
            var profissional = await ReadByIdAsync(Id);
            _context.Profissionais.Remove(profissional);
            await _context.SaveChangesAsync();
        }
    }
}
