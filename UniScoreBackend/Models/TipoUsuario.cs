using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniScore.API.Models
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
        public virtual ICollection<PessoaTipoUsuario> PessoasTiposUsuario { get; set; } = new List<PessoaTipoUsuario>();
    }

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
        public virtual Pessoa Pessoa { get; set; } = null!;

        [ForeignKey("IdTipoUsuario")]
        public virtual TipoUsuario TipoUsuario { get; set; } = null!;
    }
}