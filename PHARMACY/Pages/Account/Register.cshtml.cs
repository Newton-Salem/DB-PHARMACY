//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using PHARMACY.DAO;

//namespace PHARMACY.Pages.Account
//{
//    public class RegisterModel : PageModel
//    {
//        UserDAO dao = new();

//        [BindProperty]
//        public RegisterInput Input { get; set; }

//        public string Message { get; set; }

//        public void OnGet() { }

//        public IActionResult OnPost()
//        {
//            // 1️⃣ validation
//            if (string.IsNullOrEmpty(Input.Username) ||
//                string.IsNullOrEmpty(Input.Password))
//            {
//                Message = "Please fill all required fields";
//                return Page();
//            }

//            if (Input.Password != Input.ConfirmPassword)
//            {
//                Message = "Passwords do not match!";
//                return Page();
//            }

//            // 2️⃣ username exists?
//            if (dao.UsernameExists(Input.Username))
//            {
//                Message = "Username already exists!";
//                return Page();
//            }

//            // 3️⃣ register
//            dao.RegisterCustomer(
//                Input.Name,
//                Input.Username,
//                Input.Password,
//                Input.Email,
//                Input.Phone,
//                Input.Address
//            );

//            TempData["Success"] = "Account created successfully!";
//            return RedirectToPage("/Account/Login");
//        }
//    }

//    public class RegisterInput
//    {
//        public string Name { get; set; }
//        public string Username { get; set; }
//        public string Email { get; set; }
//        public string
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
            // 1️⃣ Validation
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

            // 2️⃣ Insert User (Customer by default)
            dao.InsertUser(
                Input.Username,
                Input.Password,
                Input.Name,
                "Customer",     // 👈 role ثابت
                Input.Email,
                Input.Phone,
                Input.Address
            );

            // 3️⃣ Success message
            TempData["SuccessMessage"] =
                "Account created successfully 🎉 Please login.";

            return RedirectToPage("/Account/Login");
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
