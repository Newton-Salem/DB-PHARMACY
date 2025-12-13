using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PHARMACY.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");

            if (role == null)
            {
                return RedirectToPage("/Account/Login");
            }

            if (role == "Admin")
                return RedirectToPage("/Admin/Index");

            return RedirectToPage("/Customer/Index");
        }
    }
}
