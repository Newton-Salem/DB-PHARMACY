using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PHARMACY.DAO;
using PHARMACY.Models;
using System;
using System.Collections.Generic;

namespace PHARMACY.Pages.Pharmacist.Medicines
{
    public class IndexModel : PageModel
    {
        // DAOs
        private readonly MedicineDAO dao = new();
        private readonly CategoryDAO categoryDao = new();

        // ===== DATA =====
        public List<Medicine> Medicines { get; set; } = new();

        public List<(int Id, string Name)> Categories { get; set; } = new();

        // ===== ADD =====\
        [BindProperty]
        public string? SearchTerm { get; set; }
        [BindProperty]
        public Medicine NewMedicine { get; set; } = new();

        [BindProperty]
        public int? SelectedCategoryId { get; set; }

        // ===== UPDATE =====
        [BindProperty]
        public int UpdateMedicineId { get; set; }

        [BindProperty]
        public int? NewStock { get; set; }

        [BindProperty]
        public DateTime? NewExpiryDate { get; set; }

        // ================== GET ==================
        public void OnGet()
        {
            Medicines = dao.GetAll();
            Categories = categoryDao.GetAll();
        }


        // ================== ADD ==================
        public IActionResult OnPostAdd()
        {
            if (!SelectedCategoryId.HasValue)
            {
                TempData["SuccessMessage"] = "❌ Please choose a category";
                return RedirectToPage();
            }

            dao.AddWithCategory(NewMedicine, SelectedCategoryId.Value);

            TempData["SuccessMessage"] = "✅ Medicine added successfully";
            return RedirectToPage();
        }


        // ================== UPDATE ==================
        public IActionResult OnPostUpdate()
        {
            dao.Update(UpdateMedicineId, NewStock, NewExpiryDate);

            TempData["SuccessMessage"] = "✏️ Medicine updated successfully";
            return RedirectToPage();
        }

    

        public IActionResult OnPostSearch()
        {
            
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                Medicines = dao.GetAll();
            }
            else
            {
                Medicines = dao.Search(SearchTerm.Trim());
            }

            Categories = categoryDao.GetAll();
            return Page();
        }



        // ================== OUT OF STOCK ==================
        public IActionResult OnPostOutOfStock()
        {
            Medicines = dao.GetOutOfStock();
            Categories = categoryDao.GetAll();
            return Page();
        }



        public void OnPostFilter()
        {
            if (SelectedCategoryId.HasValue)
                Medicines = dao.GetByCategory(SelectedCategoryId.Value);
            else
                Medicines = dao.GetAll();

            Categories = categoryDao.GetAll();
        }

    }
}
