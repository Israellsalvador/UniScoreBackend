using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectEvaluationApi.Models
{
    [Table("Evento")]
    public class Evento
    {
        [Key]
        public int IdEvento { get; set; }
        
        [Required]
        [StringLength(255)]
        public string NomeEvento { get; set; } = string.Empty;
        
        [Required]
        public string DescricaoEvento { get; set; } = string.Empty;
        
        [Required]
        [StringLength(255)]
        public string Local { get; set; } = string.Empty;
        
        [Required]
        public DateTime DataInicio { get; set; }
        
        public DateTime? DataFim { get; set; }
        
        // Navigation properties
        public virtual ICollection<CriterioAvaliacao> CriteriosAvaliacao { get; set; } = new List<CriterioAvaliacao>();
        public virtual ICollection<EventoProjeto> EventoProjetos { get; set; } = new List<EventoProjeto>();
    }
}
