//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using System.Linq;

//namespace PHARMACY.Pages.Customer.Orders
//{
//    public class AddOrderModel : PageModel
//    {
//        [BindProperty] public int MedicineID { get; set; }
//        [BindProperty] public string MedicineName { get; set; }
//        [BindProperty] public decimal UnitPrice { get; set; }
//        [BindProperty] public int Quantity { get; set; } = 1;

//        public IActionResult OnGet()
//        {

//            return Page();
//        }

//        public IActionResult OnPost()
//        {
//            if (!ModelState.IsValid) return Page();

//            int newId = IndexModel.OrdersData.Any() ? IndexModel.OrdersData.Max(o => o.OrderID) + 1 : 1;

//            IndexModel.OrdersData.Add(new Order
//            {
//                OrderID = newId,
//                MedicineName = MedicineName,
//                Quantity = Quantity,
//                Total = Quantity * UnitPrice,
//                Status = "Pending"
//            });

//            return RedirectToPage("/Customer/Orders/Index");
//        }
//    }
//}



using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PHARMACY.DAO;
using PHARMACY.Model;

namespace PHARMACY.Pages.Customer.Orders
{
    public class AddOrderModel : PageModel
    {
        private readonly OrderDAO orderDAO = new();

        [BindProperty] public string MedicineName { get; set; } = "";
        [BindProperty] public int Quantity { get; set; }
        [BindProperty] public decimal UnitPrice { get; set; }

        [BindProperty] public int MedicineID { get; set; }

        public IActionResult OnPost()
        {
            if (HttpContext.Session.GetString("Role") != "Customer")
                return RedirectToPage("/Account/Login");

            int? customerId = HttpContext.Session.GetInt32("UserID");
            if (customerId == null)
                return RedirectToPage("/Account/Login");

            Order order = new()
            {
                CustomerID = customerId.Value,
                PharmacistID = 3,
                Quantity = Quantity,
                Total = Quantity * UnitPrice,
                Status = "Pending"
            };

            orderDAO.AddOrder(order, MedicineID);

            return RedirectToPage("/Customer/Orders/Index");
        }

    }
}


