

namespace TASKHIVE.DTOs
{
    public class AdminGetProjectDetailsDTO
    {
        public int ProjectId { get; set; }  
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public string Technologies { get; set; }
        public DateTime P_StartDate { get; set; }
        public DateTime P_DueDate { get; set; }
        public int? Duration { get; set; }
        public string? TeamName { get; set; }
        public string? Objectives { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string ClientName { get; set; }
        public string ClientDescription { get; set; }
        public string ClientEmail { get; set; }
        public string ClientPhone { get; set; }

    }
}
