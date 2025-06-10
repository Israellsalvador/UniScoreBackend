using System.ComponentModel.DataAnnotations;

namespace UniScore.API.DTOs
{
    public class CreateEventoDto
    {
        [Required(ErrorMessage = "Nome do evento é obrigatório")]
        [StringLength(255, ErrorMessage = "Nome deve ter no máximo 255 caracteres")]
        public string NomeEvento { get; set; } = string.Empty;

        [Required(ErrorMessage = "Descrição é obrigatória")]
        public string DescricaoEvento { get; set; } = string.Empty;

        [Required(ErrorMessage = "Local é obrigatório")]
        [StringLength(255, ErrorMessage = "Local deve ter no máximo 255 caracteres")]
        public string Local { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data de início é obrigatória")]
        public DateTime DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        [Range(0, 100, ErrorMessage = "Nota mínima deve estar entre 0 e 100")]
        public float NotaMinima { get; set; } = 0;

        [Range(0, 100, ErrorMessage = "Nota máxima deve estar entre 0 e 100")]
        public float NotaMaxima { get; set; } = 10;

        public List<string> Criterios { get; set; } = new List<string>();
    }

    public class EventoDto
    {
        public int IdEvento { get; set; }
        public string NomeEvento { get; set; } = string.Empty;
        public string DescricaoEvento { get; set; } = string.Empty;
        public string Local { get; set; } = string.Empty;
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public bool Ativo { get; set; }
        public float NotaMinima { get; set; }
        public float NotaMaxima { get; set; }
        public List<CriterioDto> Criterios { get; set; } = new List<CriterioDto>();
        public int TotalProjetos { get; set; }
        public int TotalAvaliacoes { get; set; }
    }

    public class CriterioDto
    {
        public int IdCriterio { get; set; }
        public string NomeCriterio { get; set; } = string.Empty;
    }

    public class EncerrarEventoDto
    {
        [Required(ErrorMessage = "ID do evento é obrigatório")]
        public int IdEvento { get; set; }
    }
}