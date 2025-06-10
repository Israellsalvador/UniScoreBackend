using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniScore.API.Models
{
    [Table("Projeto")]
    public class Projeto
    {
        [Key]
        public int IdProjeto { get; set; }

        [Required]
        [StringLength(100)]
        public string NomeProjeto { get; set; } = string.Empty;

        [StringLength(500)]
        public string? DescricaoProjeto { get; set; }

        [StringLength(200)]
        public string? Integrantes { get; set; }

        [StringLength(50)]
        public string? Turma { get; set; }

        public float? NotaFinal { get; set; }

        // Navigation properties
        public virtual ICollection<InformacoesComplementares> InformacoesComplementares { get; set; } = new List<InformacoesComplementares>();
        public virtual ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();
        public virtual ICollection<EventoProjeto> EventosProjetos { get; set; } = new List<EventoProjeto>();
    }

    [Table("InformacoesComplementares")]
    public class InformacoesComplementares
    {
        [Key]
        public int IdInformacao { get; set; }

        [Required]
        public int IdProjeto { get; set; }

        [Required]
        [StringLength(200)]
        public string InformacaoComplementar { get; set; } = string.Empty;

        // Navigation properties
        [ForeignKey("IdProjeto")]
        public virtual Projeto Projeto { get; set; } = null!;
    }
}