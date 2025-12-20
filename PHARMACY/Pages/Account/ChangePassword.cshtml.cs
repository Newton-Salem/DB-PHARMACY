using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PHARMACY.DAO;

namespace PHARMACY.Pages.Account
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserDAO userDAO = new();

        [BindProperty]
        public ChangePasswordInput Input { get; set; } = new();

        public string Message { get; set; } = "";
        public bool IsSuccess { get; set; }

        public IActionResult OnGet()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToPage("/Account/Login");

            return Page();
        }

        public IActionResult OnPost()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToPage("/Account/Login");

            string? dbPassword = userDAO.GetPasswordByUsername(username);

            if (dbPassword == null)
            {
                Message = "User not found!";
                IsSuccess = false;
                return Page();
            }

            if (Input.CurrentPassword != dbPassword)
            {
                Message = "Current password is incorrect!";
                IsSuccess = false;
                return Page();
            }

            if (Input.NewPassword != Input.ConfirmPassword)
            {
                Message = "New passwords do not match!";
                IsSuccess = false;
                return Page();
            }

            userDAO.UpdatePassword(username, Input.NewPassword);

            Message = "Password changed successfully ✔";
            IsSuccess = true;

            return Page();
        }
    }

    public class ChangePasswordInput
    {
        public string CurrentPassword { get; set; } = "";
        public string NewPassword { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";
    }
}
