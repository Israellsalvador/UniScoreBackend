using System.ComponentModel.DataAnnotations;

namespace ProjectEvaluationApi.DTOs
{
    public class AvaliacaoDto
    {
        public int IdAvaliacao { get; set; }
        public int IdPessoa { get; set; }
        public string NomePessoa { get; set; } = string.Empty;
        public int IdProjeto { get; set; }
        public string NomeProjeto { get; set; } = string.Empty;
        public List<NotaDto> Notas { get; set; } = new List<NotaDto>();
        public float? MediaFinal { get; set; }
    }
    
    public class CreateAvaliacaoDto
    {
        [Required]
        public int IdPessoa { get; set; }
        
        [Required]
        public int IdProjeto { get; set; }
        
        public List<CreateNotaDto> Notas { get; set; } = new List<CreateNotaDto>();
    }
    
    public class UpdateAvaliacaoDto
    {
        public List<CreateNotaDto> Notas { get; set; } = new List<CreateNotaDto>();
    }
}
