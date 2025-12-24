using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PHARMACY.DAO;
using PHARMACY.Model;
using System.Collections.Generic;

namespace PHARMACY.Pages.Admin.SupplierOrders
{
    public class IndexModel : PageModel
    {
        SupplierRequestDAO dao = new();

        public List<SupplierRequest> Orders { get; set; } = new();

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin")
                return RedirectToPage("/Account/Login");

            Orders = dao.GetAll();
            return Page();
        }

        public IActionResult OnPostDelete(int requestId)
        {
            dao.Delete(requestId);   
            TempData["Message"] = "Supplier order deleted successfully ✅";
            return RedirectToPage();
        }
    }
}
