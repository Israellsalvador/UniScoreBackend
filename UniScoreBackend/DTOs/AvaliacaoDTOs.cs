using System.ComponentModel.DataAnnotations;

namespace UniScore.API.DTOs
{
    public class CreateAvaliacaoDto
    {
        [Required(ErrorMessage = "ID do projeto é obrigatório")]
        public int IdProjeto { get; set; }

        [Required(ErrorMessage = "Notas são obrigatórias")]
        public List<NotaCriterioDto> Notas { get; set; } = new List<NotaCriterioDto>();
    }

    public class NotaCriterioDto
    {
        [Required(ErrorMessage = "ID do critério é obrigatório")]
        public int IdCriterio { get; set; }

        [Required(ErrorMessage = "Nota é obrigatória")]
        [Range(0, 100, ErrorMessage = "Nota deve estar entre 0 e 100")]
        public float NotaProjeto { get; set; }
    }

    public class AvaliacaoDto
    {
        public int IdAvaliacao { get; set; }
        public int IdProjeto { get; set; }
        public string NomeProjeto { get; set; } = string.Empty;
        public string NomeAvaliador { get; set; } = string.Empty;
        public DateTime DataAvaliacao { get; set; }
        public bool Finalizada { get; set; }
        public List<NotaDetalheDto> Notas { get; set; } = new List<NotaDetalheDto>();
        public float NotaTotal { get; set; }
    }

    public class NotaDetalheDto
    {
        public int IdCriterio { get; set; }
        public string NomeCriterio { get; set; } = string.Empty;
        public float NotaProjeto { get; set; }
    }

    public class CriteriosEventoDto
    {
        public int IdEvento { get; set; }
        public string NomeEvento { get; set; } = string.Empty;
        public float NotaMinima { get; set; }
        public float NotaMaxima { get; set; }
        public List<CriterioDto> Criterios { get; set; } = new List<CriterioDto>();
    }
}