using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectEvaluationApi.Models
{
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
        public virtual Evento? Evento { get; set; }
        
        [ForeignKey("IdProjeto")]
        public virtual Projeto? Projeto { get; set; }
    }
}
