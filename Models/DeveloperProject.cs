using System.ComponentModel.DataAnnotations;

namespace TASKHIVE.Models
{
    public class DeveloperProject
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        [Required]
        public int DeveloperId { get; set; }
        public Developer Developer { get; set; }
    }
}
