using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace PHARMACY.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginInput Input { get; set; }

        public string Message { get; set; }

        // قائمة المستخدمين لكل رول
        public static List<User> Users = new List<User>
        {
            new User { Username = "admin", Password = "123", Role = "Admin" },
            new User { Username = "customer1", Password = "123", Role = "Customer" },
            new User { Username = "customer2", Password = "123", Role = "Customer" },
            new User { Username = "supplier1", Password = "123", Role = "Supplier" },
            new User { Username = "supplier2", Password = "123", Role = "Supplier" },
            new User { Username = "pharmacist1", Password = "123", Role = "Pharmacist" },
            new User { Username = "pharmacist2", Password = "123", Role = "Pharmacist" }
        };

        public IActionResult OnGet()
        {
            // أي حد يدخل اللوجين => نعمله لوج اوت
            HttpContext.Session.Clear();
            return Page();
        }

        public IActionResult OnPost()
        {
            // نبحث في اليوزرز
            var user = Users.FirstOrDefault(u => u.Username == Input.Username && u.Password == Input.Password);

            if (user != null)
            {
                HttpContext.Session.SetString("Role", user.Role);
                HttpContext.Session.SetString("Username", user.Username);

                // Redirect حسب الرول
                return user.Role switch
                {
                    "Admin" => RedirectToPage("/Admin/Index"),
                    "Customer" => RedirectToPage("/Customer/Index"),
                    "Supplier" => RedirectToPage("/Supplier/Index"),
                    "Pharmacist" => RedirectToPage("/Pharmacist/Index"),
                    _ => Page()
                };
            }

            Message = "Invalid username or password";
            return Page();
        }
    }

    public class LoginInput
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
