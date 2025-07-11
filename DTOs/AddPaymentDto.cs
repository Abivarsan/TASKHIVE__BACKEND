using TASKHIVE.Models;

namespace TASKHIVE.DTOs
{
    public class AddPaymentDto

    {
        public int PaymentId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int MonthlyWorkedHours { get; set; }
        public double TotalMonthPayment { get; set; }
        public int DeveloperId { get; set; }
    }
}
