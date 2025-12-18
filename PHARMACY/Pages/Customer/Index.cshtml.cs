using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PHARMACY.DAO;
using System.Linq;

namespace PHARMACY.Pages.Customer
{
    public class IndexModel : PageModel
    {
        private readonly OrderDAO orderDAO = new();

        public string CustomerName { get; set; } = "";
        public int OrdersCount { get; set; }
        public decimal TotalSpent { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Role") != "Customer")
                return RedirectToPage("/Account/Login");

            int? customerId = HttpContext.Session.GetInt32("UserID");

            if (customerId == null)
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Account/Login");
            }

            var orders = orderDAO.GetOrdersByCustomer(customerId.Value);

            OrdersCount = orders.Count;

            TotalSpent = orders
                .Where(o => o.Status == "Completed")
                .Sum(o => o.Total);

            CustomerName = HttpContext.Session.GetString("Username") ?? "";

            return Page();
        }

    }
}
