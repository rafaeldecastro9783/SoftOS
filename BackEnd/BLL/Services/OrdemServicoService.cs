using Microsoft.EntityFrameworkCore;
using SoftOS.BLL.Models;
using SoftOS.DAL.Context;
using SoftOS.Shared.Enums;
using SoftOS.Shared.Exceptions;
using System.Net;

namespace SoftOS.BLL.Services
{
    public interface IOrdemServicoService
    {
        Task<IEnumerable<OrdemServico>> ReadAllAsync();
        Task<OrdemServico> CreateAsync(OrdemServico model);
        Task<OrdemServico> ReadByIdAsync(int Id);
        Task<OrdemServico> UpdateById(int Id, OrdemServico model);
        Task DeleteByIdAsync(int Id);
    }

    public class OrdemServicoService(AppDbContext context) : IOrdemServicoService
    {
        private readonly AppDbContext _context = context;

        public async Task<IEnumerable<OrdemServico>> ReadAllAsync() =>
            await _context.OrdensServicos.ToArrayAsync();

        public async Task<OrdemServico> CreateAsync(OrdemServico model)
        {
            await _context.OrdensServicos.AddAsync(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<OrdemServico> ReadByIdAsync(int id)
        {
            if (await _context.OrdensServicos.FindAsync(id) is OrdemServico ordemServico)
                return ordemServico;

            throw new ServiceException(
                HttpStatusCode.NotFound,
                TemaModal.Aviso,
                "Ordem de Serviço não encontrada",
                "Não foi encontrada nenhuma Ordem de Serviço correspondente"
            );
        }

        public async Task<OrdemServico> UpdateById(int Id, OrdemServico model)
        {
            var ordemServico = await ReadByIdAsync(Id);
            _context.Entry(ordemServico).CurrentValues.SetValues(model);
            _context.SaveChanges();
            return model;
        }

        public async Task DeleteByIdAsync(int Id)
        {
            var ordemServico = await ReadByIdAsync(Id);
            _context.OrdensServicos.Remove(ordemServico);
            await _context.SaveChangesAsync();
        }
    }
}
