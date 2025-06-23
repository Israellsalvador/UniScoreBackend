using System.ComponentModel.DataAnnotations;

namespace ProjectEvaluationApi.DTOs
{
    public class TipoUsuarioDto
    {
        public byte IdTipoUsuario { get; set; }
        public string PapelUsuario { get; set; } = string.Empty;
    }
    
    public class CreateTipoUsuarioDto
    {
        [Required]
        [StringLength(50)]
        public string PapelUsuario { get; set; } = string.Empty;
    }
    
    public class UpdateTipoUsuarioDto
    {
        [Required]
        [StringLength(50)]
        public string PapelUsuario { get; set; } = string.Empty;
    }
}
