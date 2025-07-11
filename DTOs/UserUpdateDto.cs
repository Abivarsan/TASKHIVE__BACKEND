namespace TASKHIVE.DTOs
{
    public class UserUpdateDto
    {
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
