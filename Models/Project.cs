using System.ComponentModel.DataAnnotations;

namespace TASKHIVE.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string? ProjectDescription { get; set; }
        public string? Technologies { get; set; }
        public string? BudgetEstimation { get; set; }
        public DateTime P_StartDate { get; set; }
        public DateTime P_DueDate { get; set; }
        public int? Duration { get; set; }

        public string? ProjectStatus { get; set; }
        public string? TeamName { get; set; }
        public string? TimeLine { get; set; }
        public string? Objectives { get; set; }



        //ProjectTimeCalculation columns
        public int TotalProjectCompletedHours { get; set; }
        public int TotalProjectRemainingHours { get; set; }
        public int TotalProjectHours { get; set; }




        public Client Client { get; set; }
        public int ClientId { get; set; }

        public List<Task> Tasks { get; set; }





        public Admin Admin { get; set; }
        public int AdminId { get; set; }

        public ProjectManager ProjectManager { get; set; }
        public int ProjectManagerId { get; set; }

        public List<DeveloperProject> DeveloperProjects { get; set; }
        public List<FileResource> FileResources { get; set; }


        public Budget Budget { get; set; }

        public List<Transaction> Transactions { get; set; }

    }
}
