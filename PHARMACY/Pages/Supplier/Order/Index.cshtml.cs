//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using System.Collections.Generic;
//using System.Linq;
//using CustomerOrders = PHARMACY.Pages.Customer.Orders; 

//namespace PHARMACY.Pages.Supplier.Orders
//{
//    public class IndexModel : PageModel
//    {
       
//        public List<CustomerOrders.Order> Orders { get; set; }

//        public IActionResult OnGet()
//        {
          
//            var role = HttpContext.Session.GetString("Role");
//            if (string.IsNullOrEmpty(role) || role != "Supplier")
//            {
//                HttpContext.Session.Clear();
//                return RedirectToPage("/Account/Login");
//            }

         
//            Orders = CustomerOrders.IndexModel.OrdersData;

//            return Page();
//        }

      
//        public IActionResult OnPostComplete(int orderId)
//        {
//            var order = CustomerOrders.IndexModel.OrdersData.FirstOrDefault(o => o.OrderID == orderId);
//            if (order != null && order.Status == "Pending")
//            {
//                order.Status = "Completed";
//            }

//            return RedirectToPage();
//        }
//    }
//}
