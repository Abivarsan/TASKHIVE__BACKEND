using System;
namespace TASKHIVE.DTOs
{
    public class ProjectDescriptionDTO
    {

        public string ProjectName { get; set; }

        public int ProjectId { get; set; }

        public string? ProjectDescription { get; set; }

        public string? Objectives { get; set; }



        public int ProjectManagerId { get; set; }

        public string? ProjectManagerName { get; set; }



        public DateTime P_StartDate { get; set; }

        public int? Duration { get; set; }

        public DateTime P_DueDate { get; set; }


    }
}

