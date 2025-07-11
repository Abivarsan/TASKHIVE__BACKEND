using System.ComponentModel.DataAnnotations;

namespace TASKHIVE.Models
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string NIC { get; set; }
        public string? ContactNumber { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? ClientDescription { get; set; }
        public double TotalPayment { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public int UserCategoryId { get; set; } = 4;
        public bool IsActive { get; set; } = true;
        public List<Project> Projects { get; set; }
    }
}