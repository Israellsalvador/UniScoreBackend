using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniScore.API.Models
{
    [Table("Evento")]
    public class Evento
    {
        [Key]
        public int IdEvento { get; set; }

        [Required]
        [StringLength(100)]
        public string NomeEvento { get; set; } = string.Empty;

        [StringLength(500)]
        public string? DescricaoEvento { get; set; }

        [StringLength(100)]
        public string? Local { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        public bool Ativo { get; set; } = true;

        public float NotaMinima { get; set; } = 0;

        public float NotaMaxima { get; set; } = 10;

        // Navigation properties
        public virtual ICollection<CriterioAvaliacao> CriteriosAvaliacao { get; set; } = new List<CriterioAvaliacao>();
        public virtual ICollection<EventoProjeto> EventosProjetos { get; set; } = new List<EventoProjeto>();
    }

    [Table("CriterioAvaliacao")]
    public class CriterioAvaliacao
    {
        [Key]
        public int IdCriterio { get; set; }

        [Required]
        public int IdEvento { get; set; }

        [Required]
        [StringLength(100)]
        public string NomeCriterio { get; set; } = string.Empty;

        // Navigation properties
        [ForeignKey("IdEvento")]
        public virtual Evento Evento { get; set; } = null!;
        public virtual ICollection<Nota> Notas { get; set; } = new List<Nota>();
    }

    [Table("Evento_Projeto")]
    public class EventoProjeto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdEvento { get; set; }

        [Required]
        public int IdProjeto { get; set; }

        // Navigation properties
        [ForeignKey("IdEvento")]
        public virtual Evento Evento { get; set; } = null!;

        [ForeignKey("IdProjeto")]
        public virtual Projeto Projeto { get; set; } = null!;
    }
}