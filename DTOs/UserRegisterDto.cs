namespace TASKHIVE.DTOs
{
    public class UserRegisterDto
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string NIC { get; set; }
        public DateTime DOB { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string UserCategoryType { get; set; }
        public string JobRoleType { get; set; }
        public string ProfileImageUrl { get; set; } // Firebase URL

    }
}
