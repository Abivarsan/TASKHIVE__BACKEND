using System.ComponentModel.DataAnnotations;

namespace TASKHIVE.Models
{
    public class DeveloperRate
    {
        [Key]
        public int Rateid { get; set; }

        public DateTime UpdatedDate { get; set; }

        [Required]
        public double CurrentRate { get; set; }
    }
}
