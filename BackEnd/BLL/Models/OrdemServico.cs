using System.ComponentModel.DataAnnotations;

namespace SoftOS.BLL.Models
{
    public class OrdemServico
    {
        public int Id { get; set; }
        public int TipoServicoId { get; set; }
        public virtual TipoServico? TipoServico { get; set; }
        public int ProfissionalId { get; set; }
        public virtual Profissional? Profissional { get; set; }
        public int EmpresaId { get; set; }
        public virtual Empresa? Empresa { get; set; }
        public virtual ICollection<Ticket>? Ticket { get; set; } = [];
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataConclusao { get; set; }
        public Dictionary<DateTime, string> Historico { get; set; } = [];
        public bool Ativo { get; set; }
        // Additional properties can be added as needed
    }
}
