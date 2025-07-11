using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TASKHIVE.Models
{
    public class Budget
    {
        [Key]
        public int BudgetId { get; set; }

        [Required]
        public string Objectives { get; set; } = string.Empty;
        public double SelectionprocessCost { get; set; }
        public double LicenseCost { get; set; }
        public double ServersCost { get; set; }
        public double HardwareCost { get; set; }
        public double ConnectionCost { get; set; }
        public double DeveloperCost { get; set; }
        public double OtherExpenses { get; set; }
        public DateTime Date { get; set; }

        [Required]
        public double TotalCost { get; set; }



        [JsonIgnore]
        public Project Project { get; set; }
        public int ProjectId { get; set; } 
     
    }
}
