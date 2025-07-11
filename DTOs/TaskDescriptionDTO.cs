using System;
namespace TASKHIVE.DTOs
{
    public class TaskDescriptionDTO
    {


        public string ProjectName { get; set; }

        public int ProjectId { get; set; }

        public string TaskName { get; set; }

        public int TaskId { get; set; }

        public int TaskStatus { get; set; }

        public string TaskDescription { get; set; }

        public int Priority { get; set; }

        public string? Technology { get; set; }

        public string? Dependancy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int TimeDuration { get; set; }

        public DateTime DueDate { get; set; }

    }
}

