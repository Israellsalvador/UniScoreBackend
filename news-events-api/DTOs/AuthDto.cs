using System.ComponentModel.DataAnnotations;

namespace ProjectEvaluationApi.DTOs
{
    public class LoginDto
    {
        [Required]
        public string Usuario { get; set; } = string.Empty;
        
        [Required]
        public string Senha { get; set; } = string.Empty;
    }
    
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public int IdPessoa { get; set; }
        public string NomePessoa { get; set; } = string.Empty;
        public List<string> Papeis { get; set; } = new List<string>();
    }
    
    public class RegisterDto
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
        
        [Required]
        public List<byte> TiposUsuario { get; set; } = new List<byte>();
    }
}
