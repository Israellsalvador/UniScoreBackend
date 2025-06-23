using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectEvaluationApi.Models
{
    [Table("Nota")]
    public class Nota
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int IdCriterio { get; set; }
        
        [Required]
        public int IdAvaliacao { get; set; }
        
        [Required]
        public float NotaProjeto { get; set; }
        
        // Navigation properties
        [ForeignKey("IdCriterio")]
        public virtual CriterioAvaliacao? CriterioAvaliacao { get; set; }
        
        [ForeignKey("IdAvaliacao")]
        public virtual Avaliacao? Avaliacao { get; set; }
    }
}
