using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SoftOS.BLL.Models;
using SoftOS.Shared.Utils;

namespace SoftOS.DAL.Context;

public partial class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Empresa> Usuarios { get; set; } = null!;
    public DbSet<OrdemServico> OrdensServicos { get; set; } = null!;
    public DbSet<TipoServico> TipoServicos { get; set; } = null!;
    public DbSet<Profissional> Profissionais { get; set; } = null!;
    public DbSet<Ticket> Tickets { get; set; } = null!;
    public DbSet<Cliente> Clientes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<OrdemServico>()
            .Property(o => o.DataCriacao)
            .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%f', 'now')");
        modelBuilder
            .Entity<Profissional>()
            .Property(p => p.DataCriacao)
            .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%f', 'now')");
        modelBuilder
            .Entity<Ticket>()
            .Property(t => t.DataCriacao)
            .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%f', 'now')");

        modelBuilder
            .Entity<OrdemServico>()
            .Property(o => o.Historico)
            .HasConversion(
                v => JsonUtil.Serializar(v),
                // GUARANTEE: tipos de prop. idêntico ao definido na classe em questão
                v => JsonUtil.Desserializar<Dictionary<DateTime, string>>(v)!
            );

        Cliente cliente = new()
        {
            Id = 1,
            Nome = "Greendoc",
            Email = "cliente@greendoc.com",
            Telefone = "81999999999",
            Login = "greendoc",
            Senha = HashUtil.ComputarPBKDF2("Gdoc123!@#"),
        };
        modelBuilder
            .Entity<Cliente>().HasData(cliente);

        Profissional profissional = new()
        {
            Id = 1,
            Nome = "Profissional Hum",
            Email = "profissional1@softdotpro.com",
            Guid = Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
            Telefone = "81984927181",
            Login = "profissional1",
            Senha = HashUtil.ComputarPBKDF2("Soft123!@#"),
        };

        modelBuilder
            .Entity<Profissional>().HasData(profissional);

        Empresa empresa = new()
        {
            Id = 1,
            Nome = "SoftDotPro",
            Email = "softdotpro@softdotpro.com",
            Telefone = "81999493640",
            Cpf = null,
            Endereco = "",
            Cidade = "Maceió",
            Estado = "AL",
            Cep = "",
            Cnpj = null,
        };
        modelBuilder.Entity<Empresa>().HasData(empresa);
    }
}
