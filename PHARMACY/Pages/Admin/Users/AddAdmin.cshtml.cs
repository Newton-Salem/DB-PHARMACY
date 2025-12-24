using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PHARMACY.DAO;

namespace PHARMACY.Pages.Admin.Users
{
    public class AddAdminModel : PageModel
    {
        private readonly UserDAO userDAO = new();

        [BindProperty] public string Username { get; set; }
        [BindProperty] public string Password { get; set; }
        [BindProperty] public string Name { get; set; }
        [BindProperty] public string Email { get; set; }
        [BindProperty] public string Phone { get; set; }
        [BindProperty] public string Address { get; set; }
        [BindProperty] public decimal Salary { get; set; }

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin")
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Account/Login");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin")
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Account/Login");
            }

            userDAO.InsertAdmin(
                Username,
                Password,
                Name,
                Email,
                Phone,
                Address,
                Salary
            );

            return RedirectToPage("Index");
        }
    }
}