namespace PHARMACY.Model
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public int UserID { get; set; }
    }
}
