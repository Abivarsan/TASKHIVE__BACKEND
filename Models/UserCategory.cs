using System.ComponentModel.DataAnnotations;

namespace TASKHIVE.Models
{
    public class UserCategory
    {

        [Key]
        public int UserCategoryId { get; set; }
        [Required]
        public string UserCategoryType { get; set; }
    }
}
