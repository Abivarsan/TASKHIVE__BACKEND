using System.ComponentModel.DataAnnotations.Schema;

namespace  TASKHIVE.Models
{
    public class Admin
    {
        [ForeignKey("User")]
        public int AdminId { get; set; }

        public virtual User User { get; set; }
        public List<Project> Projects { get; set; }

    }
}
