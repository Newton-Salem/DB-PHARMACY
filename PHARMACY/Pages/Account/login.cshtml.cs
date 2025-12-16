using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PHARMACY.DAO;
using Microsoft.Data.SqlClient;

namespace PHARMACY.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginInput Input { get; set; }

        public string Message { get; set; }

        UserDAO Userdao = new UserDAO();

        public IActionResult OnGet()
        {
            HttpContext.Session.Clear();
            return Page();
        }

        public IActionResult OnPost()
        {
            SqlDataReader reader = Userdao.Login(Input.Username, Input.Password);

            if (reader.Read())
            {
                int userId = reader.GetInt32(0);
                string username = reader.GetString(1);
                string role = reader.GetString(2);

                reader.Close();

                HttpContext.Session.SetInt32("UserID", userId);
                HttpContext.Session.SetString("Username", username);
                HttpContext.Session.SetString("Role", role);

                // Redirect حسب الرول
                return role switch
                {
                    "Admin" => RedirectToPage("/Admin/Index"),
                    "Customer" => RedirectToPage("/Customer/Index"),
                    "Supplier" => RedirectToPage("/Supplier/Index"),
                    "Pharmacist" => RedirectToPage("/Pharmacist/Index"),
                    _ => Page()
                };
            }

            reader.Close();
            Message = "Invalid username or password";
            return Page();
        }
    }

    public class LoginInput
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
