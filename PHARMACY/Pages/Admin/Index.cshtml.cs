using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PHARMACY.Pages.Admin
{
    public class IndexModel : BasePageModel
    {
        public int TotalCustomers { get; set; } = 10;
        public int TotalMedicines { get; set; } = 20;
        public int TotalOrders { get; set; } = 5;

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToPage("/Account/Login");
            }

            return Page();
        }

    }
}
