using System.Net;
using Microsoft.EntityFrameworkCore;
using SoftOS.BLL.Models;
using SoftOS.DAL.Context;
using SoftOS.Shared.Enums;
using SoftOS.Shared.Exceptions;

namespace SoftOS.BLL.Services
{
    public interface ITipoServicoService
    {
        Task<IEnumerable<TipoServico>> ReadAllAsync();
        Task<TipoServico> CreateAsync(TipoServico model);
        Task<TipoServico> ReadByIdAsync(int Id);
        Task<TipoServico> UpdateById(int Id, TipoServico model);
        Task DeleteByIdAsync(int Id);
    }

    public class TipoServicoService(AppDbContext context) : ITipoServicoService
    {
        private readonly AppDbContext _context = context;

        public async Task<IEnumerable<TipoServico>> ReadAllAsync() =>
            await _context.TipoServicos.ToArrayAsync();

        public async Task<TipoServico> CreateAsync(TipoServico model)
        {
            await _context.TipoServicos.AddAsync(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<TipoServico> ReadByIdAsync(int id)
        {
            if (await _context.TipoServicos.FindAsync(id) is TipoServico tipoServico)
                return tipoServico;

            throw new ServiceException(
                HttpStatusCode.NotFound,
                TemaModal.Aviso,
                "Ordem de Serviço não encontrada",
                "Não foi encontrada nenhuma Ordem de Serviço correspondente"
            );
        }

        public async Task<TipoServico> UpdateById(int Id, TipoServico model)
        {
            var tipoServico = await ReadByIdAsync(Id);
            _context.Entry(tipoServico).CurrentValues.SetValues(model);
            _context.SaveChanges();
            return model;
        }

        public async Task DeleteByIdAsync(int Id)
        {
            var tipoServico = await ReadByIdAsync(Id);
            _context.TipoServicos.Remove(tipoServico);
            await _context.SaveChangesAsync();
        }
    }
}
