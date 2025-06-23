using System.ComponentModel.DataAnnotations;

namespace NewsEventsApi.DTOs
{
    public class StatusDto
    {
        public byte IdStatus { get; set; }
        
        [Required]
        [StringLength(50)]
        public string DescricaoStatus { get; set; } = string.Empty;
    }
    
    public class CreateStatusDto
    {
        [Required]
        [StringLength(50)]
        public string DescricaoStatus { get; set; } = string.Empty;
    }
    
    public class UpdateStatusDto
    {
        [Required]
        [StringLength(50)]
        public string DescricaoStatus { get; set; } = string.Empty;
    }
}
