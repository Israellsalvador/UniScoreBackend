using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectEvaluationApi.Models
{
    [Table("CriterioAvaliacao")]
    public class CriterioAvaliacao
    {
        [Key]
        public int IdCriterio { get; set; }
        
        [Required]
        public int IdEvento { get; set; }
        
        [Required]
        [StringLength(255)]
        public string NomeCriterio { get; set; } = string.Empty;
        
        // Navigation properties
        [ForeignKey("IdEvento")]
        public virtual Evento? Evento { get; set; }
        
        public virtual ICollection<Nota> Notas { get; set; } = new List<Nota>();
    }
}
