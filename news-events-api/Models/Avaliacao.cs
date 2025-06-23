using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectEvaluationApi.Models
{
    [Table("Avaliacao")]
    public class Avaliacao
    {
        [Key]
        public int IdAvaliacao { get; set; }
        
        [Required]
        public int IdPessoa { get; set; }
        
        [Required]
        public int IdProjeto { get; set; }
        
        // Navigation properties
        [ForeignKey("IdPessoa")]
        public virtual Pessoa? Pessoa { get; set; }
        
        [ForeignKey("IdProjeto")]
        public virtual Projeto? Projeto { get; set; }
        
        public virtual ICollection<Nota> Notas { get; set; } = new List<Nota>();
    }
}
