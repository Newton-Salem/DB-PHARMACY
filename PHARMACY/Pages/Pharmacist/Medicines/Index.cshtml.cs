using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PHARMACY.DAO;
using PHARMACY.Models;

namespace PHARMACY.Pages.Pharmacist.Medicines
{
    public class IndexModel : PageModel
    {
        MedicineDAO dao = new();

        public List<Medicine> Medicines { get; set; } = new();

        [BindProperty] public string? SearchTerm { get; set; }
        [BindProperty] public Medicine NewMedicine { get; set; } = new();
        [BindProperty] public int UpdateMedicineId { get; set; }
        [BindProperty] public int? NewStock { get; set; }
        [BindProperty] public DateTime NewExpiryDate { get; set; }

        public void OnGet()
        {
            Medicines = dao.GetAll();
        }

        public IActionResult OnPostAdd()
        {
            dao.Add(NewMedicine);

            TempData["SuccessMessage"] = "✅ Medicine added successfully";

            return RedirectToPage();
        }

        public IActionResult OnPostUpdate()
        {
            int? stock = NewStock.HasValue ? NewStock : null;
            DateTime? expiry = NewExpiryDate == DateTime.MinValue ? null : NewExpiryDate;

            dao.Update(UpdateMedicineId, stock, expiry);

            TempData["SuccessMessage"] = "✏️ Medicine updated successfully";
            return RedirectToPage();

        }



        public void OnPostSearch()
        {
            Medicines = string.IsNullOrWhiteSpace(SearchTerm)
                ? dao.GetAll()
                : dao.Search(SearchTerm);
        }



        public IActionResult OnPostOutOfStock()
        {
            Medicines = dao.GetOutOfStock();
            return Page();
        }

    }
}
