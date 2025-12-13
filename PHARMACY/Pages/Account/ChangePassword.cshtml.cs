using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PHARMACY.Pages.Account
{
    public class ChangePasswordModel : PageModel
    {
        [BindProperty]
        public ChangePasswordInput Input { get; set; }

        public string Message { get; set; }

        public IActionResult OnGet()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToPage("/Account/Login");

            return Page();
        }

        public IActionResult OnPost()
        {
           
            var currentPassword = HttpContext.Session.GetString("Password") ?? "12345";

            if (Input.CurrentPassword != currentPassword)
            {
                Message = "Current password is incorrect!";
                return Page();
            }

            if (Input.NewPassword != Input.ConfirmPassword)
            {
                Message = "New passwords do not match!";
                return Page();
            }

    
            HttpContext.Session.SetString("Password", Input.NewPassword);
            Message = "Password changed successfully!";

            return Page();
        }
    }

    public class ChangePasswordInput
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
