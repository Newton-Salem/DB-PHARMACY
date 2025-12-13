using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PHARMACY.Pages.Admin.Users
{
    public class IndexModel : PageModel
    {
        // Simulate Users list (combine Customers + Suppliers)
        public static List<User> Users = new List<User>
        {
            new User { Username = "user1", Name="Ahmed Ali", Role="Customer" },
            new User { Username = "user2", Name="Sama Noor", Role="Customer" },
            new User { Username = "user3", Name="Noor Salem", Role="Customer" },
            new User { Username = "sup1", Name="Pharma One", Role="Supplier" },
            new User { Username = "sup2", Name="Pharma Two", Role="Supplier" }
        };

        public List<User> UsersList { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public void OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role != "Admin")
            {
                HttpContext.Session.Clear();
                Response.Redirect("/Account/Login");
            }

            UsersList = string.IsNullOrEmpty(SearchTerm)
                ? Users
                : Users.Where(u => u.Username.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public IActionResult OnPostDelete(string username)
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role != "Admin")
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Account/Login");
            }

            var user = Users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                Users.Remove(user);
            }

            return RedirectToPage();
        }
    }

    public class User
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Role { get; set; } // "Customer" or "Supplier"
    }
}
