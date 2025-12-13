using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PHARMACY.Pages.Admin.Medicines
{
    public class EditMedicineModel : PageModel
    {
        // Use the same in-memory list from MedicinesModel
        private static List<MedicinesModel.MedicineItem> _allMedicines => MedicinesModel._allMedicines;

        [BindProperty]
        public MedicinesModel.MedicineItem Medicine { get; set; }

        public IActionResult OnGet(int id)
        {
            Medicine = _allMedicines.FirstOrDefault(m => m.Id == id);

            if (Medicine == null)
            {
                return RedirectToPage("/Admin/Medicines/Medicines");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            var medicine = _allMedicines.FirstOrDefault(m => m.Id == Medicine.Id);
            if (medicine != null)
            {
                medicine.Name = Medicine.Name;
                medicine.Stock = Medicine.Stock;
                medicine.Price = Medicine.Price;
                medicine.Expiry = Medicine.Expiry;
            }

            return RedirectToPage("/Admin/Medicines/Medicines");
        }
    }
}
