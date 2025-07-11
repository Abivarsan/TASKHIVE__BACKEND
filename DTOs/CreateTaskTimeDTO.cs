using System;
namespace TASKHIVE.DTOs
{
    public class CreateTaskTimeDTO
    {

        public DateTime TaskTimeStartTime { get; set; }
        public DateTime TaskTimeCompleteTime { get; set; }
        public int TotalTimeTaskTimeDuration { get; set; }
        public int TaskId { get; set; }
        //public int DeveloperId { get; set; }


    }
}

