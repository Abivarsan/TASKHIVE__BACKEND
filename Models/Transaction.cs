using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TASKHIVE.Models
{
    public class Transaction
    {

        [Key]
        public int TransacId { get; set; }

        
        public double Value { get; set; }

        
        public string Type { get; set; }

        
        public string Description { get; set; }
        public double Income { get; set; }
        public double Expence { get; set; }
        public DateTime Date { get; set; }

        [JsonIgnore]
        public Project Project { get; set; }
        public int ProjectId { get; set; }

    }
}
