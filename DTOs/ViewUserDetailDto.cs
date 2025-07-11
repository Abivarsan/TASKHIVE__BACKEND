namespace TASKHIVE.DTOs
{
    public class ViewUserDetailDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string NIC { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string ProfileImageName { get; set; }
        public string ImageSrc { get; set; }
        public bool IsActive { get; set; } = true;
        public string JobRoleType { get; set; }
        public string UserCategoryType { get; set; }
    }
}
