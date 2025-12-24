using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PHARMACY.DAO;
using PHARMACY.Model;

namespace PHARMACY.Pages.Supplier.Order
{
    public class IndexModel : PageModel
    {
        private readonly SupplierRequestDAO dao = new();

        public List<Model.SupplierRequest> Orders { get; set; } = new();

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Pharmacist")
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Account/Login");
            }

            Orders = dao.GetAll();
            return Page();
        }
    }
}
