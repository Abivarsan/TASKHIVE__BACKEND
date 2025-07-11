

namespace TASKHIVE.DTOs
{
    public class ClientRegisterDto
    {
        public string ClientName { get; set; }
        public string NIC { get; set; }
        public string? ContactNumber { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? ClientDescription { get; set; }
        public double TotalPayment { get; set; }
        public string UserName { get; set; }
        public int UserCategoryId { get; set; }
    }

}

