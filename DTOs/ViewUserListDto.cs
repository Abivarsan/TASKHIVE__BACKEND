namespace TASKHIVE.DTOs
{
    public class ViewUserListDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public required string Email { get; set; }
        public string UserCategoryType { get; set; }
        public bool IsActive { get; set; } = true;
        public string FirstName { get; set; }
        public string ProfileImageName { get; set; }
        public string ImageSrc { get; set; }
    }
}
