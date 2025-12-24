
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



            //int pharmacistId = orderDAO.GetAnyPharmacistId();

            Order order = new()
            {
                CustomerID = customerId.Value,
                Quantity = Quantity,
                Total = Quantity * UnitPrice,
                Status = "Pending"
            };

            orderDAO.AddOrder(order, MedicineID);

            return RedirectToPage("/Customer/Orders/Index");
        }

    }
}


