using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectEvaluationApi.Models
{
    [Table("Pessoa")]
    public class Pessoa
    {
        [Key]
        public int IdPessoa { get; set; }
        
        [Required]
        [StringLength(255)]
        public string NomePessoa { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string Usuario { get; set; } = string.Empty;
        
        [Required]
        [StringLength(128)]
        public string Senha { get; set; } = string.Empty;
        
        // Navigation properties
        public virtual ICollection<PessoaTipoUsuario> PessoaTipoUsuarios { get; set; } = new List<PessoaTipoUsuario>();
        public virtual ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();
    }
}
