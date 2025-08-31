//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace TASKHIVE.Models
//{
//    public class User
//    {

//        [Key]
//        public int UserId { get; set; }
//        [Required]
//        public string UserName { get; set; } = string.Empty;
//        [Required]
//        public string PasswordHash { get; set; } = string.Empty;
//        [Required]
//        public string FirstName { get; set; }
//        [Required]
//        public string LastName { get; set; }
//        [Required]
//        public string Email { get; set; }
//        public string ContactNumber { get; set; }
//        public string Address { get; set; }
//        [Required]
//        public string NIC { get; set; }
//        public DateTime DOB { get; set; }
//        public string Gender { get; set; }
//        public string ProfileImageName { get; set; }
//        public DateTime JoinDate { get; set; } = DateTime.Now;
//        public DateTime LastLoginDate { get; set; } 
//        public bool IsActive { get; set; } = true;
//        public int JobRoleId { get; set; }
//        public JobRole JobRole { get; set; }

//        public int UserCategoryId { get; set; }
//        public UserCategory UserCategory { get; set; }


//        public Developer Developer { get; set; }
//        public Admin Admin { get; set; }
//        public ProjectManager ProjectManager { get; set; }
//        [NotMapped]
//        public IFormFile ImageFile { get; set; }

//        [NotMapped]
//        public string ImageSrc { get; set; }


//    }
//}
// User.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TASKHIVE.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public string ContactNumber { get; set; }
        public string Address { get; set; }

        [Required]
        public string NIC { get; set; }

        public DateTime DOB { get; set; }
        public string Gender { get; set; }

        // Changed from ProfileImageName to ProfileImageUrl
        public string ProfileImageUrl { get; set; }

        public DateTime JoinDate { get; set; } = DateTime.Now;
        public DateTime LastLoginDate { get; set; }
        public bool IsActive { get; set; } = true;

        public int JobRoleId { get; set; }
        public JobRole JobRole { get; set; }

        public int UserCategoryId { get; set; }
        public UserCategory UserCategory { get; set; }

        public Developer Developer { get; set; }
        public Admin Admin { get; set; }
        public ProjectManager ProjectManager { get; set; }

        // Remove ImageFile and ImageSrc as they're not needed anymore
    }
}
