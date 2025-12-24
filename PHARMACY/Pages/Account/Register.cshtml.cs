using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PHARMACY.DAO;

namespace PHARMACY.Pages.Account
{
    public class RegisterModel : PageModel
    {
        UserDAO dao = new();

        [BindProperty]
        public RegisterInput Input { get; set; }

        public string Message { get; set; }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            //  Validation
            if (string.IsNullOrWhiteSpace(Input.Username) ||
                string.IsNullOrWhiteSpace(Input.Password) ||
                string.IsNullOrWhiteSpace(Input.Name))
            {
                Message = "Please fill all required fields";
                return Page();
            }

            if (Input.Password != Input.ConfirmPassword)
            {
                Message = "Passwords do not match!";
                return Page();
            }

            //  Check if username already exists
            if (dao.UsernameExists(Input.Username))
            {
                Message = "Username already exists. Please choose another one.";
                return Page();
            }


            //  Insert User (Customer by default)
            dao.InsertUser(
                Input.Username,
                Input.Password,
                Input.Name,
                "Customer",     
                Input.Email,
                Input.Phone,
                Input.Address
            );

            // Success message
            TempData["SuccessMessage"] = "Account created successfully 🎉";
            return Page();
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
