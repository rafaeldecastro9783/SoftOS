using SoftOS.Shared.Enums;

namespace SoftOS.BLL.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int OrdemServicoId { get; set; }
        public int EmpresaId { get; set; }
        public int? ProfissionalId { get; set; }
        public int ClienteId { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataConclusao { get; set; }
        public bool Ativo { get; set; } = true;
        public TicketSituacao Situacao { get; set; } = TicketSituacao.Aberto;
        public virtual OrdemServico? OrdemServico { get; set; } = null;
        public virtual Empresa? Empresa { get; set; } = null;
        public virtual Profissional? Profissional { get; set; } = null;
        public virtual Cliente? Cliente { get; set; } = null;
        // Additional properties can be added as needed
    }
}
