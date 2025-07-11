using System;
using System.ComponentModel.DataAnnotations;

namespace TASKHIVE.Models
{
    public class TaskTime
    {

        [Key]
        public int TaskTimeId { get; set; }
        public DateTime TaskTimeStartTime { get; set; }
        public DateTime TaskTimeCompleteTime { get; set; }
        public int TotalTimeTaskTimeDuration { get; set; }



        public Task Task { get; set; }
        public int TaskId { get; set; }

        public Developer Developer { get; set; }
        public int DeveloperId { get; set; }




    }
}

