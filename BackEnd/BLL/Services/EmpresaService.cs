using System.Net;
using Microsoft.EntityFrameworkCore;
using SoftOS.BLL.Models;
using SoftOS.DAL.Context;
using SoftOS.Shared.Enums;
using SoftOS.Shared.Exceptions;

namespace SoftOS.BLL.Services;

public interface IEmpresaService
{
    Task<IEnumerable<Empresa>> ReadAllAsync();
    Task<Empresa> CreateAsync(Empresa model);
    Task<Empresa> ReadByIdAsync(int id);
    Task<Empresa> UpdateAsync(int id, Empresa model);
    Task DeleteByIdAsync(int id);
}

public class EmpresaService(AppDbContext context) : IEmpresaService
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<Empresa>> ReadAllAsync() =>
        await _context.Empresas.ToArrayAsync();

    public async Task<Empresa> CreateAsync(Empresa model)
    {
        await _context.Empresas.AddAsync(model);
        await _context.SaveChangesAsync();
        return model;
    }

    public async Task<Empresa> ReadByIdAsync(int id)
    {
        if (await _context.Empresas.FindAsync(id) is Empresa empresa)
            return empresa;

        throw new ServiceException(
            HttpStatusCode.NotFound,
            TemaModal.Erro,
            "Empresa não encontrada",
            "Não foi encontrado nenhuma empresa correspondente"
        );
    }

    public async Task<Empresa> UpdateAsync(int id, Empresa model)
    {
        // if id != mode.Id
        var empresa = await ReadByIdAsync(id);
        _context.Entry(empresa).CurrentValues.SetValues(model);
        await _context.SaveChangesAsync();
        return model;
    }

    public async Task DeleteByIdAsync(int id)
    {
        var empresa = await ReadByIdAsync(id);
        _context.Empresas.Remove(empresa);
        await _context.SaveChangesAsync();
    }
}
