using System.ComponentModel.DataAnnotations;

namespace TASKHIVE.Models
{
    public class DeveloperFinancialRecipt
    {
        [Key]
        public int ReceiptId { get; set; }
        public int CurrentMonthWorkingHours { get; set; }
        public int PreviousMonthWorkingHours { get; set; }
        public double HourlyRate { get; set; }
        public int DeveloperId { get; set; }
    }
}
