using System;


namespace TASKHIVE.DTOs
{
    public class UpdateProjectDTO
    {


        public string? ProjectName { get; set; }

        public string? ProjectDescription { get; set; }

        public string? ProjectObjectives { get; set; }

        public string? ProjectTechnologies { get; set; }

        public string? ProjectBudgetEstimation { get; set; }

        public DateTime ProjectStartDate { get; set; }

        public DateTime ProjectDueDate { get; set; }

        public int ProjectDuration { get; set; }

        public string? ProjectStatus { get; set; }

        public int ProjectManagerId { get; set; }

        public string? ProjectManagerName { get; set; }
    }
}

