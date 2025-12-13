using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PHARMACY.Pages.Admin.Orders
{
    public class IndexModel : PageModel
    {
        // Simulate Orders list
        public static List<Order> OrdersList = new List<Order>
        {
            new Order { OrderID = 1, CustomerName = "Ahmed Ali", TotalAmount = 250, Status = "Pending" },
            new Order { OrderID = 2, CustomerName = "Sama Noor", TotalAmount = 500, Status = "Completed" },
            new Order { OrderID = 3, CustomerName = "Noor Salem", TotalAmount = 150, Status = "Pending" }
        };

        [BindProperty(SupportsGet = true)]
        public int? SearchID { get; set; }

        public List<Order> Orders { get; set; }

        public void OnGet()
        {
            Orders = SearchID.HasValue
                ? OrdersList.Where(o => o.OrderID == SearchID.Value).ToList()
                : OrdersList;
        }

        public IActionResult OnPostDelete(int orderId)
        {
            var order = OrdersList.FirstOrDefault(o => o.OrderID == orderId);
            if (order != null)
            {
                OrdersList.Remove(order);
            }

            return RedirectToPage();
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
