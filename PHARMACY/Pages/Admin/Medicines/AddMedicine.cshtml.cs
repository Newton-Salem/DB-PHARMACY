using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PHARMACY.Pages.Admin
{
    public class AddMedicineModel : PageModel
    {
        [BindProperty]
        public MedicinesModel.MedicineItem NewMedicine { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            // Assign new ID
            int newId = MedicinesModel._allMedicines.Max(m => m.Id) + 1;
            NewMedicine.Id = newId;

            // Add to the in-memory list
            MedicinesModel._allMedicines.Add(NewMedicine);

            return RedirectToPage("/Admin/Medicines/Medicines");
        }
    }
}
