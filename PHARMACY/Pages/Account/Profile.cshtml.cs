using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PHARMACY.Pages.Account
{
    public class ProfileModel : PageModel
    {
        public string Username { get; set; }
        public string Role { get; set; }

        public IActionResult OnGet()
        {
            Username = HttpContext.Session.GetString("Username");
            Role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(Username))
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Account/Login");
            }

            return Page();
        }
    }
}
