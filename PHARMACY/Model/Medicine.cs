namespace PHARMACY.Models
{
    public class Medicine
    {
        public int Medicine_ID { get; set; }
        public string Name { get; set; } = "";
        public int Stock_Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime Expiry_Date { get; set; }

        // 🔥 NEW (مش mandatory)
        public string? Category_Name { get; set; }
        public int? Category_ID { get; set; }
    }
}
