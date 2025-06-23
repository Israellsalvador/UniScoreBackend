using System.ComponentModel.DataAnnotations;

namespace ProjectEvaluationApi.DTOs
{
    public class EventoDto
    {
        public int IdEvento { get; set; }
        public string NomeEvento { get; set; } = string.Empty;
        public string DescricaoEvento { get; set; } = string.Empty;
        public string Local { get; set; } = string.Empty;
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public List<CriterioAvaliacaoDto> CriteriosAvaliacao { get; set; } = new List<CriterioAvaliacaoDto>();
        public List<int> ProjetosIds { get; set; } = new List<int>();
    }
    
    public class CreateEventoDto
    {
        [Required]
        [StringLength(255)]
        public string NomeEvento { get; set; } = string.Empty;
        
        [Required]
        public string DescricaoEvento { get; set; } = string.Empty;
        
        [Required]
        [StringLength(255)]
        public string Local { get; set; } = string.Empty;
        
        [Required]
        public DateTime DataInicio { get; set; }
        
        public DateTime? DataFim { get; set; }
        
        public List<string> CriteriosAvaliacao { get; set; } = new List<string>();
    }
    
    public class UpdateEventoDto
    {
        [Required]
        [StringLength(255)]
        public string NomeEvento { get; set; } = string.Empty;
        
        [Required]
        public string DescricaoEvento { get; set; } = string.Empty;
        
        [Required]
        [StringLength(255)]
        public string Local { get; set; } = string.Empty;
        
        [Required]
        public DateTime DataInicio { get; set; }
        
        public DateTime? DataFim { get; set; }
        
        public List<string> CriteriosAvaliacao { get; set; } = new List<string>();
    }
}
