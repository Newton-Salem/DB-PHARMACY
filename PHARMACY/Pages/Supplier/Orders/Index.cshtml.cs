using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PHARMACY.DAO;
using PHARMACY.Model;

namespace PHARMACY.Pages.Supplier.Orders
{
    public class CustomerOrdersModel : PageModel
    {
        private readonly SupplierRequestDAO dao = new();

        public List<SupplierRequest> Requests { get; set; } = new();

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Supplier")
                return RedirectToPage("/AccessDenied");

            var supplierId = HttpContext.Session.GetInt32("UserID");
            if (supplierId == null)
                return RedirectToPage("/Account/Login");

            Requests = dao.GetRequestsForSupplier(supplierId.Value);
            return Page();
        }

        public IActionResult OnPostApprove(int requestId)
        {
            dao.UpdateStatus(requestId, "Approved");
            TempData["Success"] = "Request approved successfully ✅";
            return RedirectToPage();
        }

        public IActionResult OnPostCancel(int requestId)
        {
            dao.UpdateStatus(requestId, "Cancelled");
            TempData["Success"] = "Request rejected successfully ❌";
            return RedirectToPage();
        }

    }
}
