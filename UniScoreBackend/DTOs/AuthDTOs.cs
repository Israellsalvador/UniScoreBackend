using System.ComponentModel.DataAnnotations;

namespace UniScore.API.DTOs
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Usuário é obrigatório")]
        public string Usuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        public string Senha { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string NomePessoa { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public List<string> Papeis { get; set; } = new List<string>();
        public DateTime ExpiresAt { get; set; }
    }

    public class CreateUserDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(255, ErrorMessage = "Nome deve ter no máximo 255 caracteres")]
        public string NomePessoa { get; set; } = string.Empty;

        [Required(ErrorMessage = "Usuário é obrigatório")]
        [StringLength(50, ErrorMessage = "Usuário deve ter no máximo 50 caracteres")]
        public string Usuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        [StringLength(128, MinimumLength = 6, ErrorMessage = "Senha deve ter entre 6 e 128 caracteres")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tipo de usuário é obrigatório")]
        public byte IdTipoUsuario { get; set; }
    }

    public class UserDto
    {
        public int IdPessoa { get; set; }
        public string NomePessoa { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public List<string> Papeis { get; set; } = new List<string>();
    }
}