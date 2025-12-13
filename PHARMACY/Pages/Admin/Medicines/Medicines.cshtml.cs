using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PHARMACY.Pages.Admin
{
    public class MedicinesModel : PageModel
    {
        // In-memory list shared across all pages
        public static List<MedicineItem> _allMedicines = new List<MedicineItem>
        {
            new MedicineItem { Id = 1, Name = "Panadol", Stock = 200, Price = 50, Expiry = DateTime.Parse("2026-12-01") },
            new MedicineItem { Id = 2, Name = "Augmentin", Stock = 150, Price = 75, Expiry = DateTime.Parse("2025-06-10") },
            new MedicineItem { Id = 3, Name = "Vitamin C", Stock = 300, Price = 30, Expiry = DateTime.Parse("2027-04-18") },
            new MedicineItem { Id = 4, Name = "Cough Syrup", Stock = 250, Price = 20, Expiry = DateTime.Parse("2026-10-05") },
            new MedicineItem { Id = 5, Name = "Insulin", Stock = 180, Price = 60, Expiry = DateTime.Parse("2025-12-01") }
        };

        // This is what Razor Page uses
        public List<MedicineItem> Medicines { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; }

        public void OnGet()
        {
            Medicines = string.IsNullOrEmpty(SearchName)
                ? _allMedicines
                : _allMedicines.Where(m => m.Name.Contains(SearchName, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public IActionResult OnPostDelete(int id)
        {
            var medicine = _allMedicines.FirstOrDefault(m => m.Id == id);
            if (medicine != null)
            {
                _allMedicines.Remove(medicine);
            }
            return RedirectToPage();
        }

        public class MedicineItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Stock { get; set; }
            public decimal Price { get; set; }
            public DateTime Expiry { get; set; }
        }
    }
}
