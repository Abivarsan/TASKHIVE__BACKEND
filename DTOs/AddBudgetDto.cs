using System.ComponentModel.DataAnnotations;

namespace TASKHIVE.DTOs
{
    public class AddBudgetDto
    {
        [Required]
        public string Objectives { get; set; } = string.Empty;
        public double SelectionprocessCost { get; set; }
        public double LicenseCost { get; set; }
        public double ServersCost { get; set; }
        public double HardwareCost { get; set; }
        public double ConnectionCost { get; set; }
        public double DeveloperCost { get; set; }
        public double OtherExpenses { get; set; }
        [Required]
        public double TotalCost { get; set; }

        public DateTime Date { get; set; }
    }
}
