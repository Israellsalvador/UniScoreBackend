using System.ComponentModel.DataAnnotations;

namespace ProjectEvaluationApi.DTOs
{
    public class ProjetoDto
    {
        public int IdProjeto { get; set; }
        public string NomeProjeto { get; set; } = string.Empty;
        public string DescricaoProjeto { get; set; } = string.Empty;
        public string Integrantes { get; set; } = string.Empty;
        public string Turma { get; set; } = string.Empty;
        public float? NotaFinal { get; set; }
        public List<InformacoesComplementaresDto> InformacoesComplementares { get; set; } = new List<InformacoesComplementaresDto>();
        public List<int> EventosIds { get; set; } = new List<int>();
    }
    
    public class CreateProjetoDto
    {
        [Required]
        [StringLength(255)]
        public string NomeProjeto { get; set; } = string.Empty;
        
        [Required]
        [StringLength(500)]
        public string DescricaoProjeto { get; set; } = string.Empty;
        
        [Required]
        [StringLength(500)]
        public string Integrantes { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Turma { get; set; } = string.Empty;
        
        public List<string> InformacoesComplementares { get; set; } = new List<string>();
    }
    
    public class UpdateProjetoDto
    {
        [Required]
        [StringLength(255)]
        public string NomeProjeto { get; set; } = string.Empty;
        
        [Required]
        [StringLength(500)]
        public string DescricaoProjeto { get; set; } = string.Empty;
        
        [Required]
        [StringLength(500)]
        public string Integrantes { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Turma { get; set; } = string.Empty;
        
        public float? NotaFinal { get; set; }
        
        public List<string> InformacoesComplementares { get; set; } = new List<string>();
    }
}
