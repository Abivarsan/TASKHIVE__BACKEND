using System.ComponentModel.DataAnnotations;

namespace TASKHIVE.Models
{
    public class ViewReport
    {

        [Key]
        public int BudgetId { get; set; }

        public int UserId { get; set; }
    }
}
