using System.ComponentModel.DataAnnotations;

namespace TASKHIVE.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int MonthlyWorkedHours { get; set; }
        public double TotalMonthPayment { get; set; }
        public Developer Developer { get; set; }
        public int DeveloperId { get; set; }

    }
}
