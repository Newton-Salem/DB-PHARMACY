

using Microsoft.AspNetCore.Mvc.RazorPages;
using PHARMACY.DAO;
using PHARMACY.Models;
using System.Collections.Generic;

namespace PHARMACY.Pages.Admin
{
    public class MedicinesModel : PageModel
    {
        MedicineDAO dao = new MedicineDAO();

        public List<Medicine> Medicines { get; set; } = new();
        public string SearchName { get; set; } = string.Empty;

        public void OnGet(string searchName)
        {
            SearchName = searchName;

            if (!string.IsNullOrWhiteSpace(searchName))
                Medicines = dao.Search(searchName);
            else
                Medicines = dao.GetAll();
        }
    }
}
