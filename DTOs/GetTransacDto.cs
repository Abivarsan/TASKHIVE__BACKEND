namespace TASKHIVE.DTOs
{
    public class GetTransacDto
    {
        public int TransacId { get; set; }
        public double Value { get; set; }
        public string Description { get; set; }
        public double Income { get; set; }
        public double Expence { get; set; }
        public DateTime Date { get; set; }
    }
}
