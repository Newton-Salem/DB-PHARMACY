using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PHARMACY.DAO;
using PHARMACY.Model;
using PHARMACY.Models;
using System.Collections.Generic;

namespace PHARMACY.Pages.Pharmacist.Orders
{
    public class CreateModel : PageModel
    {
        private readonly SupplierOrderDAO orderDAO = new();
        private readonly CommonDAO commonDAO = new();

        // Dropdown data
        public List<SupplierModel> Suppliers { get; set; } = new();
        public List<Medicine> Medicines { get; set; } = new();

        // Form fields
        [BindProperty] public int SupplierID { get; set; }
        [BindProperty] public int MedicineID { get; set; }
        [BindProperty] public int Quantity { get; set; }
        [BindProperty]
        public DateTime RequestDate { get; set; }


        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role != "Pharmacist")
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Account/Login");
            }

            Suppliers = commonDAO.GetSuppliers();
            Medicines = commonDAO.GetMedicines();

            return Page();
        }

        public IActionResult OnPost()
        {
            int? pharmacistId = HttpContext.Session.GetInt32("UserID");

            if (pharmacistId == null)
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Account/Login");
            }

            orderDAO.Add(SupplierID, pharmacistId.Value, MedicineID, Quantity, RequestDate);

            TempData["Success"] = "Supplier order sent successfully ✅";
            return RedirectToPage("../Index");
        }

    }
}
