using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectEvaluationApi.Models
{
    [Table("Pessoa_TipoUsuario")]
    public class PessoaTipoUsuario
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int IdPessoa { get; set; }
        
        [Required]
        public byte IdTipoUsuario { get; set; }
        
        // Navigation properties
        [ForeignKey("IdPessoa")]
        public virtual Pessoa? Pessoa { get; set; }
        
        [ForeignKey("IdTipoUsuario")]
        public virtual TipoUsuario? TipoUsuario { get; set; }
    }
}
