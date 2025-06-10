using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniScore.API.Models
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

        public DateTime DataAvaliacao { get; set; } = DateTime.Now;

        public bool Finalizada { get; set; } = false;

        // Navigation properties
        [ForeignKey("IdPessoa")]
        public virtual Pessoa Pessoa { get; set; } = null!;

        [ForeignKey("IdProjeto")]
        public virtual Projeto Projeto { get; set; } = null!;

        public virtual ICollection<Nota> Notas { get; set; } = new List<Nota>();
    }

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
        public virtual CriterioAvaliacao CriterioAvaliacao { get; set; } = null!;

        [ForeignKey("IdAvaliacao")]
        public virtual Avaliacao Avaliacao { get; set; } = null!;
    }
}