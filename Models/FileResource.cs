using System.ComponentModel.DataAnnotations;

namespace TASKHIVE.Models
{
    public class FileResource
    {
        [Key]
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string LocalStoragePath { get; set; }
        public DateTime UpdatedDate { get; set; }


        public Project Project { get; set; }
        public int ProjectId { get; set; }

        public Task Task { get; set; }
        public int? TaskId { get; set; }



    }
}
