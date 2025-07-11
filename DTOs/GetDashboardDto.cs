namespace TASKHIVE.DTOs
{
    public class GetDashboardDto
    {
        public int TotalAdmins { get; set; }
        public int TotalManagers { get; set; }
        public int TotalDevelopers { get; set; }
        public int TotalProjects { get; set; }
        public double TotalIncome { get; set; }
        public double TotalExpense { get; set; }
    }
}
