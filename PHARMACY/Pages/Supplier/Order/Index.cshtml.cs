using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PHARMACY.Pages.Supplier.Orders
{
    public class IndexModel : PageModel
    {
        public List<Order> Orders { get; set; }

        public void OnGet()
        {
            Orders = new List<Order>
            {
                new Order { OrderID=1, CustomerName="Ahmed Ali", TotalAmount=200, Status="Pending" },
                new Order { OrderID=2, CustomerName="Sama Noor", TotalAmount=350, Status="Completed" }
            };
        }
    }

    public class Order
    {
        public int OrderID { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
    }
}
