using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PHARMACY.Pages.Pharmacist
{
    public class IndexModel : PageModel
    {
        public string Username { get; set; }

        public IActionResult OnGet()
        {
            Username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(Username) || role != "Pharmacist")
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Account/Login");
            }

            return Page();
        }
    }
}
