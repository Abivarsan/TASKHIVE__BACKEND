namespace TASKHIVE.DTOs
{
    public class TaskDetailsViewDTO
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; } 
        public int TaskStatus { get; set; }
        public int TimeDuration { get; set; }
        public string Technologies { get; set; }
        public string Dependancies { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set;}
        public int Priority { get; set; }
    }
}
