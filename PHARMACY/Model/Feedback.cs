namespace PHARMACY.Models
{
    public class Feedback
    {
        public int Feedback_ID { get; set; }
        public int CustomerID { get; set; }
        public int Order_ID { get; set; }
        public string Message { get; set; } = "";
        public DateTime Date { get; set; }
    }
}
