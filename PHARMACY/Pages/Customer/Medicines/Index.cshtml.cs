using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PHARMACY.DAO;
using PHARMACY.Models;
using System.Collections.Generic;

namespace PHARMACY.Pages.Customer.Medicines
{
    public class IndexModel : PageModel
    {
        private readonly MedicineDAO medicineDAO = new();
        private readonly CategoryDAO categoryDAO = new();

        // ===== DATA =====
        public List<Medicine> Medicines { get; set; } = new();
        public List<(int Id, string Name)> Categories { get; set; } = new();

        // ===== FILTERS =====
        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; }

        // ===== GET =====
        public void OnGet()
        {
            // Load categories always
            Categories = categoryDAO.GetAll();

            // Filter logic
            if (CategoryId.HasValue)
            {
                Medicines = medicineDAO.GetByCategory(CategoryId.Value);
            }
            else if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                Medicines = medicineDAO.Search(SearchTerm.Trim());
            }
            else
            {
                Medicines = medicineDAO.GetAll();
            }
        }
    }
}
