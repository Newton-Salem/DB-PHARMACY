using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PHARMACY.Pages.Account
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public RegisterInput Input { get; set; }

        public string Message { get; set; }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            // Dummy Register Logic (No Database)
            if (Input.Password != Input.ConfirmPassword)
            {
                Message = "Passwords do not match!";
                return;
            }

            Message = "Registration successful (Demo Mode)";
        }
    }

    public class RegisterInput
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
