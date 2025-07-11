

namespace TASKHIVE.DTOs
{
    public class AdminViewDTO
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string? ProjectDescription { get; set; }
        public string? Technologies { get; set; }
        public string? BudgetEstimation { get; set; }
        public DateTime P_StartDate { get; set; }
        public DateTime P_DueDate { get; set; }
        public int? Duration { get; set; }
        public string? TeamName { get; set; }
        public string? TimeLine { get; set; }
        public string? Objectives { get; set; }
        // public string? ClientName { get; set; }
        public int ClientId { get; set; }
        public int ProjectManagerId { get; set; }
        public string ProjectManagerFName { get; set; }
        public string ProjectManagerLName { get; set; }

    }
}
