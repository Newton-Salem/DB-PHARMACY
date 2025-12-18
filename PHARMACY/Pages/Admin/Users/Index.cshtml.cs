using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PHARMACY.DAO;
using PHARMACY.Model;
using System.Collections.Generic;
using System.Linq;

namespace PHARMACY.Pages.Admin.Users
{
    public class IndexModel : PageModel
    {
        private readonly UserDAO userDAO = new();

        public List<User> UsersList { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role != "Admin")
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Account/Login");
            }

            UsersList = userDAO.GetAllUsers();

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                UsersList = UsersList
                    .Where(u => u.Username.Contains(SearchTerm, System.StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return Page();
        }

        public IActionResult OnPostDelete(string username)
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role != "Admin")
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Account/Login");
            }

            userDAO.DeleteUser(username);
            return RedirectToPage(new { SearchTerm });
        }
    }
}
