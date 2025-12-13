using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PHARMACY.Pages.Pharmacist.Medicines
{
    public class IndexModel : PageModel
    {
        
        public static List<Medicine> MedicinesData = new List<Medicine>
        {
            new Medicine { Id = 1, Name = "Panadol", Stock = 10, ExpiryDate = DateTime.Today.AddMonths(6) },
            new Medicine { Id = 2, Name = "Vitamin C", Stock = 0, ExpiryDate = DateTime.Today.AddMonths(3) },
            new Medicine { Id = 3, Name = "Augmentin", Stock = 5, ExpiryDate = DateTime.Today.AddMonths(12) }
        };

       
        public List<Medicine> Medicines { get; set; } = new List<Medicine>();

        [BindProperty]
        public string SearchTerm { get; set; }

        [BindProperty]
        public NewMedicineInput NewMedicine { get; set; } = new NewMedicineInput();

        [BindProperty]
        public int UpdateMedicineId { get; set; }

        [BindProperty]
        public int NewStock { get; set; }

        [BindProperty]
        public DateTime? NewExpiryDate { get; set; }

        public void OnGet()
        {
            Medicines = MedicinesData;
        }

        public IActionResult OnPostAdd()
        {
            //  شوف لو الدواء موجود 
            var existingMedicine = MedicinesData
                .FirstOrDefault(m => m.Name.Equals(NewMedicine.Name, StringComparison.OrdinalIgnoreCase));

            if (existingMedicine != null)
            {
                // لو موجود زود الاستوك و حدث التاريخ
                existingMedicine.Stock += NewMedicine.Stock;
                existingMedicine.ExpiryDate = NewMedicine.ExpiryDate;
            }
            else
            {
                //  لو جديد
                int newId = MedicinesData.Any()
                    ? MedicinesData.Max(m => m.Id) + 1
                    : 1;

                MedicinesData.Add(new Medicine
                {
                    Id = newId,
                    Name = NewMedicine.Name,
                    Stock = NewMedicine.Stock,
                    ExpiryDate = NewMedicine.ExpiryDate
                });
            }

            //  تحديث الجدول
            Medicines = MedicinesData;

            //  تفريغ الفورم
            NewMedicine = new NewMedicineInput();

            return Page(); 
        }


        public IActionResult OnPostUpdate()
        {
            var med = MedicinesData.FirstOrDefault(m => m.Id == UpdateMedicineId);

            if (med != null)
            {
                if (NewStock >= 0)
                    med.Stock = NewStock;

                if (NewExpiryDate.HasValue)
                    med.ExpiryDate = NewExpiryDate.Value;
            }

            return RedirectToPage();
        }

        public IActionResult OnPostSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                Medicines = MedicinesData;
            }
            else
            {
                Medicines = MedicinesData
                    .Where(m => m.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return Page();
        }

       
        public IActionResult OnPostOutOfStock()
        {
            Medicines = MedicinesData
                .Where(m => m.Stock == 0)
                .ToList();

            return Page();
        }
    }

    public class Medicine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

    public class NewMedicineInput
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
