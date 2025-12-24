using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PHARMACY.DAO;
using PHARMACY.Model;
using System.Collections.Generic;

namespace PHARMACY.Pages.Pharmacist.Orders
{
    public class CustomerOrdersModel : PageModel
    {
        private readonly OrderDAO orderDAO = new();

        public List<Order> Orders { get; set; } = new();

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role) || role != "Pharmacist")
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Account/Login");
            }

            Orders = orderDAO.GetPendingOrdersForPharmacist();
            return Page();
        }

        public IActionResult OnPostApprove(int orderId)
        {
            int pharmacistId = HttpContext.Session.GetInt32("UserID").Value;
            orderDAO.ApproveOrder(orderId, pharmacistId);

            return RedirectToPage();
        }

        public IActionResult OnPostCancel(int orderId, int pharmacistId)
        {
            orderDAO.CancelOrderPharmacist(orderId,pharmacistId);
            return RedirectToPage();
        }
    }
}
