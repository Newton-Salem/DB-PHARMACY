using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PHARMACY.Pages.Pharmacist.Orders;

namespace PHARMACY.Pages.Pharmacist
{
    public class IndexModel : PageModel
    {
        public string Username { get; set; }

        // 🌟 مجموع الأوردرات
        public int TotalOrders { get; set; }

        public IActionResult OnGet()
        {
            Username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(Username) || role != "Pharmacist")
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Account/Login");
            }

            // 🌟 إذا كانت صفحة Orders موجودة، نحسب عدد الأوردرات
            TotalOrders = CreateModel.OrdersData.Count;

            return Page();
        }
    }
}
