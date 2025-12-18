//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using System.Collections.Generic;
//using System.Linq;

//namespace PHARMACY.Pages.Customer.Orders
//{
//    public class IndexModel : PageModel
//    {

//        public static List<Order> OrdersData = new List<Order>
//        {
//            new Order{ OrderID=1, MedicineName="Paracetamol", Quantity=2, Total=20, Status="Completed" },
//            new Order{ OrderID=2, MedicineName="Vitamin C", Quantity=1, Total=15, Status="Pending" }
//        };

//        public List<Order> Orders { get; set; }

//        [BindProperty]
//        public int OrderIdToCancel { get; set; }

//        public void OnGet()
//        {
//            Orders = OrdersData;
//        }


//        public IActionResult OnPostCancel(int orderId)
//        {
//            var order = OrdersData.FirstOrDefault(o => o.OrderID == orderId);
//            if (order != null && order.Status == "Pending")
//            {
//                order.Status = "Cancelled";
//            }
//            return RedirectToPage();
//        }
//    }

//    public class Order
//    {
//        public int OrderID { get; set; }
//        public string MedicineName { get; set; }
//        public int Quantity { get; set; }
//        public decimal Total { get; set; }
//        public string Status { get; set; }
//    }
//}



using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PHARMACY.DAO;
using PHARMACY.Model;
using System.Collections.Generic;

namespace PHARMACY.Pages.Customer.Orders
{
    public class IndexModel : PageModel
    {
        private readonly OrderDAO orderDAO = new();
        public List<Order> Orders { get; set; } = new();

        public IActionResult OnGet()
        {
            int? customerId = HttpContext.Session.GetInt32("UserID");
            if (customerId == null)
                return RedirectToPage("/Account/Login");

            Orders = orderDAO.GetOrdersByCustomer(customerId.Value);
            return Page();
        }

        public IActionResult OnPostCancel(int orderId)
        {
            orderDAO.CancelOrder(orderId);
            return RedirectToPage();
        }
    }
}

