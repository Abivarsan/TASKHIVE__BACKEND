using System.ComponentModel.DataAnnotations;

namespace TASKHIVE.DTOs
{
    public class AddRateDto
    {
        [Required]
        public DateTime UpdatedDate { get; set; }

        [Required]
        public double CurrentRate { get; set; }
    }
}
