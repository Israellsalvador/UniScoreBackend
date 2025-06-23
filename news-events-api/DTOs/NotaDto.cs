using System.ComponentModel.DataAnnotations;

namespace ProjectEvaluationApi.DTOs
{
    public class NotaDto
    {
        public int Id { get; set; }
        public int IdCriterio { get; set; }
        public string NomeCriterio { get; set; } = string.Empty;
        public int IdAvaliacao { get; set; }
        public float NotaProjeto { get; set; }
    }
    
    public class CreateNotaDto
    {
        [Required]
        public int IdCriterio { get; set; }
        
        [Required]
        [Range(0, 10)]
        public float NotaProjeto { get; set; }
    }
}
