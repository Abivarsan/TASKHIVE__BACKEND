using System.ComponentModel.DataAnnotations.Schema;

namespace TASKHIVE.Models
{
    public class ProjectManager
    {
        [ForeignKey("User")]
        public int ProjectManagerId { get; set; }
        public virtual User User { get; set; }
        public List<Project> Projects { get; set; }

    }
}
