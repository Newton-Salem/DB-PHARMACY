using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PHARMACY.Pages.Customer
{
    public class IndexModel : BasePageModel
    {
        public string CustomerName { get; set; } = "Ahmed Ali";
        public int OrdersCount { get; set; } = 5;
        public decimal TotalSpent { get; set; } = 500;

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Role") != "Customer")
            {
                return RedirectToPage("/Account/Login");
            }

            return Page();
        }

    }
}
