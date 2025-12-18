using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PHARMACY.DAO;
using PHARMACY.Model;

namespace PHARMACY.Pages.Pharmacist.Orders
{
    public class CreateModel : PageModel
    {
        private readonly OrderDAO orderDAO = new();

        [BindProperty]
        public Order NewOrder { get; set; } = new();

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role) || role != "Pharmacist")
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Account/Login");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            NewOrder.Status = "Pending";
            orderDAO.AddSupplierOrder(NewOrder);

            return RedirectToPage("Index");
        }
    }
}
