using SoftOS.Shared.Enums;

namespace SoftOS.BLL.Models;

public class Cliente : ILoginModel
{
    public int Id { get; set; }
    public TipoCliente Tipo { get; set; } = TipoCliente.PessoaJuridica;
    public string Nome { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Endereco { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Cep { get; set; } = string.Empty;
    public string Pais { get; set; } = "Brasil";
    public string? Cnpj { get; set; }
    public bool IsActive { get; set; } = true;
    public int? EmpresaId { get; set; }
    public virtual Empresa? Empresa { get; set; }
    public int? OrdemServicolId { get; set; }
    public virtual ICollection<OrdemServico>? OrdemServico { get; set; }
    public int? Ticketid { get; set; }
    public virtual ICollection<Ticket>? Ticket { get; set; }
}
