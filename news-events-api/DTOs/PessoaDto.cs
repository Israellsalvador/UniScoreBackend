using System.ComponentModel.DataAnnotations;

namespace ProjectEvaluationApi.DTOs
{
    public class PessoaDto
    {
        public int IdPessoa { get; set; }
        public string NomePessoa { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public List<TipoUsuarioDto> TiposUsuario { get; set; } = new List<TipoUsuarioDto>();
    }
    
    public class CreatePessoaDto
    {
        [Required]
        [StringLength(255)]
        public string NomePessoa { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string Usuario { get; set; } = string.Empty;
        
        [Required]
        [StringLength(128)]
        public string Senha { get; set; } = string.Empty;
        
        public List<byte> TiposUsuario { get; set; } = new List<byte>();
    }
    
    public class UpdatePessoaDto
    {
        [Required]
        [StringLength(255)]
        public string NomePessoa { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string Usuario { get; set; } = string.Empty;
        
        public string? Senha { get; set; }
        
        public List<byte> TiposUsuario { get; set; } = new List<byte>();
    }
}
