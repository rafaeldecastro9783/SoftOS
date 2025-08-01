using SoftOS.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftOS.BLL.Models
{
    public class Profissional : ILoginModel
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public ProfissionalCargo Cargo { get; set; } = ProfissionalCargo.Tecnico;
        public bool Ativo { get; set; } = true;
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataDesativacao { get; set; }
        public virtual ICollection<Empresa>? Empresa { get; set; }
        public virtual ICollection<OrdemServico>? OrdensServico { get; set; }
        public virtual ICollection<Ticket>? Tickets { get; set; }
    }
}
