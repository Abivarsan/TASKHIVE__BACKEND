using System.ComponentModel.DataAnnotations;

namespace TASKHIVE.Models
{
    public class JobRole
    {

        [Key]
        public int JobRoleId { get; set; }
        [Required]
        public string JobRoleType { get; set; }

    }
}