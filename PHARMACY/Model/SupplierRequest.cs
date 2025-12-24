namespace PHARMACY.Model
{
    public class SupplierRequest
    {
        public int RequestId { get; set; }
        public string PharmacistName { get; set; } = "";
        public string MedicineName { get; set; } = "";
        public int Quantity { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; } = "";
    }
}
