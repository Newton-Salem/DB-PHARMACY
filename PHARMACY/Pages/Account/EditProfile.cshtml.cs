using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PHARMACY.Pages.Account
{
    public class EditProfileModel : PageModel
    {
        [BindProperty]
        public EditProfileInput Input { get; set; }

      
        public string Message { get; set; }

        public IActionResult OnGet()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToPage("/Account/Login");

            Input = new EditProfileInput
            {
                Name = HttpContext.Session.GetString("Name") ?? "",
                Email = HttpContext.Session.GetString("Email") ?? "",
                Phone = HttpContext.Session.GetString("Phone") ?? "",
                Address = HttpContext.Session.GetString("Address") ?? ""
            };

            return Page();
        }

        public IActionResult OnPost()
        {
            HttpContext.Session.SetString("Name", Input.Name);
            HttpContext.Session.SetString("Email", Input.Email);
            HttpContext.Session.SetString("Phone", Input.Phone);
            HttpContext.Session.SetString("Address", Input.Address);

            Message = "Profile updated successfully!";
            return Page();
        }
    }

    public class EditProfileInput
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
