using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectEvaluationApi.Models
{
    [Table("InformacoesComplementares")]
    public class InformacoesComplementares
    {
        [Key]
        public int IdComplemento { get; set; }
        
        [Required]
        public string InformacaoComplementar { get; set; } = string.Empty;
        
        [Required]
        public int IdProjeto { get; set; }
        
        // Navigation properties
        [ForeignKey("IdProjeto")]
        public virtual Projeto? Projeto { get; set; }
    }
}
