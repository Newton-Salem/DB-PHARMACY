public class Order
{
    public int OrderID { get; set; }
    public int? CustomerID { get; set; }
    public int? PharmacistID { get; set; }

    public string CustomerName { get; set; }

    public string MedicineName { get; set; } = "";
    public int Quantity { get; set; }

    public decimal Total { get; set; }
    public string Status { get; set; } = "";
    public DateTime OrderDate { get; set; }
}
