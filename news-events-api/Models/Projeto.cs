using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectEvaluationApi.Models
{
    [Table("Projeto")]
    public class Projeto
    {
        [Key]
        public int IdProjeto { get; set; }
        
        [Required]
        [StringLength(255)]
        public string NomeProjeto { get; set; } = string.Empty;
        
        [Required]
        [StringLength(500)]
        public string DescricaoProjeto { get; set; } = string.Empty;
        
        [Required]
        [StringLength(500)]
        public string Integrantes { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Turma { get; set; } = string.Empty;
        
        public float? NotaFinal { get; set; }
        
        // Navigation properties
        public virtual ICollection<InformacoesComplementares> InformacoesComplementares { get; set; } = new List<InformacoesComplementares>();
        public virtual ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();
        public virtual ICollection<EventoProjeto> EventoProjetos { get; set; } = new List<EventoProjeto>();
    }
}
