using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PHARMACY.DAO;

namespace PHARMACY.Pages.Account
{
    public class EditProfileModel : PageModel
    {
        private readonly UserDAO userDAO = new();

        [BindProperty]
        public EditProfileInput Input { get; set; } = new();

        public string Message { get; set; } = "";
        public bool IsSuccess { get; set; }

        public IActionResult OnGet()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToPage("/Account/Login");

            var user = userDAO.GetUserByUsername(username);
            if (user == null)
                return RedirectToPage("/Account/Login");

            Input.Name = user.Name;
            Input.Email = user.Email;
            Input.Phone = user.Phone;
            Input.Address = user.Address;

            return Page();
        }

        public IActionResult OnPost()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToPage("/Account/Login");

            userDAO.UpdateProfile(
                username,
                Input.Name,
                Input.Email,
                Input.Phone,
                Input.Address
            );

            // تحديث السيشن عشان باقي الصفحات
            HttpContext.Session.SetString("Name", Input.Name);
            HttpContext.Session.SetString("Email", Input.Email);
            HttpContext.Session.SetString("Phone", Input.Phone);
            HttpContext.Session.SetString("Address", Input.Address);

            Message = "Profile updated successfully ✔";
            IsSuccess = true;

            return Page();
        }
    }

    public class EditProfileInput
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
    }
}
