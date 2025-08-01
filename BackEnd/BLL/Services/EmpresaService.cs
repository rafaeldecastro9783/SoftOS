using System.Net;
using Microsoft.EntityFrameworkCore;
using SoftOS.BLL.Models;
using SoftOS.DAL.Context;
using SoftOS.Shared.Enums;
using SoftOS.Shared.Exceptions;

namespace SoftOS.BLL.Services;

public interface IUsuarioService
{
    Task<IEnumerable<Empresa>> ReadAllAsync();
    Task<Empresa> CreateAsync(Empresa model);
    Task<Empresa> ReadByIdAsync(int id);
    Task<Empresa> UpdateAsync(int id, Empresa model);
    Task DeleteByIdAsync(int id);
}

public class EmpresaService(AppDbContext context) : IUsuarioService
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<Empresa>> ReadAllAsync() =>
        await _context.Usuarios.ToArrayAsync();

    public async Task<Empresa> CreateAsync(Empresa model)
    {
        await _context.Usuarios.AddAsync(model);
        await _context.SaveChangesAsync();
        return model;
    }

    public async Task<Empresa> ReadByIdAsync(int id)
    {
        if (await _context.Usuarios.FindAsync(id) is Empresa usuario)
            return usuario;

        throw new ServiceException(
            HttpStatusCode.NotFound,
            TemaModal.Erro,
            "Usuário não encontrado",
            "Não foi encontrado nenhum usuário correspondente"
        );
    }

    public async Task<Empresa> UpdateAsync(int id, Empresa model)
    {
        // if id != mode.Id
        var usuario = await ReadByIdAsync(id);
        _context.Entry(usuario).CurrentValues.SetValues(model);
        await _context.SaveChangesAsync();
        return model;
    }

    public async Task DeleteByIdAsync(int id)
    {
        var usuario = await ReadByIdAsync(id);
        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
    }
}
