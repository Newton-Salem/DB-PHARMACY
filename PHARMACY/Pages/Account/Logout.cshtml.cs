using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PHARMACY.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // مسح كل السيشن
            HttpContext.Session.Clear();

            // رجوع لصفحة اللوجين
            return RedirectToPage("/Account/Login");
        }
    }
}
