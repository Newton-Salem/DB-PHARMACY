namespace PHARMACY.Model
{
    public class SupplierOrder
    {
        public int RequestId { get; set; }
        public string SupplierName { get; set; }
        public string MedicineName { get; set; }
        public int Quantity { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
    }
}
