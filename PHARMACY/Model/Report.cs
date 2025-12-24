namespace PHARMACY.Model
{
    public class Report
    {
        public int ReportID { get; set; }
        public string Description { get; set; }
        public DateTime GeneratedDate { get; set; }
        public string ReportType { get; set; }
        public int AdminID { get; set; }

        // UI only
        public string AdminName { get; set; }
    }
}
