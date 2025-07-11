using System.ComponentModel.DataAnnotations;

namespace TASKHIVE.Models
{
    public class ViewInvoice
    {
        [Key]
        public int UserId { get; set; }

        public int InvoiceId { get; set; }
    }
}
