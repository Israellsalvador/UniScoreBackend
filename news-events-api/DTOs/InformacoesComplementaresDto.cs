using System.ComponentModel.DataAnnotations;

namespace ProjectEvaluationApi.DTOs
{
    public class InformacoesComplementaresDto
    {
        public int IdComplemento { get; set; }
        public string InformacaoComplementar { get; set; } = string.Empty;
        public int IdProjeto { get; set; }
    }
    
    public class CreateInformacoesComplementaresDto
    {
        [Required]
        public string InformacaoComplementar { get; set; } = string.Empty;
        
        [Required]
        public int IdProjeto { get; set; }
    }
    
    public class UpdateInformacoesComplementaresDto
    {
        [Required]
        public string InformacaoComplementar { get; set; } = string.Empty;
    }
}
