using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectEvaluationApi.Models
{
    [Table("TipoUsuario")]
    public class TipoUsuario
    {
        [Key]
        public byte IdTipoUsuario { get; set; }
        
        [Required]
        [StringLength(50)]
        public string PapelUsuario { get; set; } = string.Empty;
        
        // Navigation properties
        public virtual ICollection<PessoaTipoUsuario> PessoaTipoUsuarios { get; set; } = new List<PessoaTipoUsuario>();
    }
}
