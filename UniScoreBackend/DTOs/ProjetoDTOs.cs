using System.ComponentModel.DataAnnotations;

namespace UniScore.API.DTOs
{
    public class CreateProjetoDto
    {
        [Required(ErrorMessage = "Nome do projeto é obrigatório")]
        [StringLength(255, ErrorMessage = "Nome deve ter no máximo 255 caracteres")]
        public string NomeProjeto { get; set; } = string.Empty;

        [Required(ErrorMessage = "Descrição é obrigatória")]
        [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
        public string DescricaoProjeto { get; set; } = string.Empty;

        [Required(ErrorMessage = "Integrantes são obrigatórios")]
        [StringLength(500, ErrorMessage = "Integrantes deve ter no máximo 500 caracteres")]
        public string Integrantes { get; set; } = string.Empty;

        [Required(ErrorMessage = "Turma é obrigatória")]
        [StringLength(100, ErrorMessage = "Turma deve ter no máximo 100 caracteres")]
        public string Turma { get; set; } = string.Empty;

        [Required(ErrorMessage = "ID do evento é obrigatório")]
        public int IdEvento { get; set; }

        public List<string> InformacoesComplementares { get; set; } = new List<string>();

        public List<int> AvaliadoresIds { get; set; } = new List<int>();
    }

    public class ProjetoDto
    {
        public int IdProjeto { get; set; }
        public string NomeProjeto { get; set; } = string.Empty;
        public string DescricaoProjeto { get; set; } = string.Empty;
        public string Integrantes { get; set; } = string.Empty;
        public string Turma { get; set; } = string.Empty;
        public float? NotaFinal { get; set; }
        public int IdEvento { get; set; }
        public string NomeEvento { get; set; } = string.Empty;
        public List<string> InformacoesComplementares { get; set; } = new List<string>();
        public List<AvaliacaoResumoDto> Avaliacoes { get; set; } = new List<AvaliacaoResumoDto>();
        public bool JaAvaliado { get; set; } = false;
    }

    public class ProjetoParaAvaliarDto
    {
        public int IdProjeto { get; set; }
        public string NomeProjeto { get; set; } = string.Empty;
        public string DescricaoProjeto { get; set; } = string.Empty;
        public string Integrantes { get; set; } = string.Empty;
        public string Turma { get; set; } = string.Empty;
        public string NomeEvento { get; set; } = string.Empty;
        public List<string> InformacoesComplementares { get; set; } = new List<string>();
        public bool JaAvaliado { get; set; } = false;
    }

    public class AvaliacaoResumoDto
    {
        public int IdAvaliacao { get; set; }
        public string NomeAvaliador { get; set; } = string.Empty;
        public DateTime DataAvaliacao { get; set; }
        public bool Finalizada { get; set; }
        public float? NotaTotal { get; set; }
    }
}