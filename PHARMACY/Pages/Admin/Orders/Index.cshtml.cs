using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PHARMACY.DAO;
using PHARMACY.Model;
using System.Collections.Generic;
using System.Linq;

namespace PHARMACY.Pages.Admin.Orders
{
    public class IndexModel : PageModel
    {
        private readonly OrderDAO orderDAO = new();

        public List<Order> Orders { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int? SearchID { get; set; }

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role != "Admin")
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Account/Login");
            }

            Orders = orderDAO.GetAllOrdersForAdmin();

            if (SearchID.HasValue)
            {
                Orders = Orders
                    .Where(o => o.OrderID == SearchID.Value)
                    .ToList();
            }

            return Page();
        }

        public IActionResult OnPostDelete(int orderId)
        {
            orderDAO.DeleteOrder(orderId);

            TempData["DeleteMessage"] = "Order deleted successfully ✅";

            return RedirectToPage();
        }
    }
}
