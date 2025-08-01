using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SoftOS.BLL.Models
{
    public class Empresa
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string? Cpf { get; set; }
        public string Endereco { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public string? Cnpj { get; set; }
        public bool Ativo { get; set; } = true;
        public virtual ICollection<Profissional>? Profissionais { get; set; }
        public virtual ICollection<Cliente>? Clientes { get; set; }
        public virtual ICollection<Ticket>? Tickets { get; set; }
        public virtual ICollection<OrdemServico>? OrdemServicos { get; set; }
    }
}
