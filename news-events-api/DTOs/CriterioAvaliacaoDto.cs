using System.ComponentModel.DataAnnotations;

namespace ProjectEvaluationApi.DTOs
{
    public class CriterioAvaliacaoDto
    {
        public int IdCriterio { get; set; }
        public int IdEvento { get; set; }
        public string NomeCriterio { get; set; } = string.Empty;
    }
    
    public class CreateCriterioAvaliacaoDto
    {
        [Required]
        public int IdEvento { get; set; }
        
        [Required]
        [StringLength(255)]
        public string NomeCriterio { get; set; } = string.Empty;
    }
    
    public class UpdateCriterioAvaliacaoDto
    {
        [Required]
        [StringLength(255)]
        public string NomeCriterio { get; set; } = string.Empty;
    }
}
