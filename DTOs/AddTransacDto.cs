using System.ComponentModel.DataAnnotations;

namespace TASKHIVE.DTOs
{
    public class AddTransacDto
    {
      
        public double Value { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public double Income { get; set; }
        public double Expence { get; set; }

        public DateTime Date { get; set; }
    
     }
}
