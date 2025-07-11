using System.ComponentModel.DataAnnotations.Schema;

namespace TASKHIVE.Models
{
    public class Developer
    {
        [ForeignKey("User")]
        public int DeveloperId { get; set; }

        public int FinanceReceiptId { get; set; }

        //DeveloperWorkingHoursCalculation columns
        public int TotalDeveloperWorkingHours { get; set; }



        public virtual User User { get; set; }
        public List<Task> Tasks { get; set; }

        public List<DeveloperProject> DeveloperProjects { get; set; }

        public List<TaskTime> TaskTimes { get; set; }
        public List<Payment> Payment { get; set; }


    }
}
