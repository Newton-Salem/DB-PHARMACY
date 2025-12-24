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
        public List<Order> Orders { get; set; }

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

