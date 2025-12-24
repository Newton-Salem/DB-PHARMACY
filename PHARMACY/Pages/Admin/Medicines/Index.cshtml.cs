//using Microsoft.AspNetCore.Mvc.RazorPages;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace PHARMACY.Pages.Admin
//{
//    public class MedicinesModel : PageModel
//    {

//        public static List<Medicine> AllMedicines = new List<Medicine>
//        {
//            new Medicine { Id = 1, Name = "Panadol", Stock = 50, Price = 15.5M, Expiry = DateTime.Today.AddMonths(6) },
//            new Medicine { Id = 2, Name = "Vitamin C", Stock = 20, Price = 10M, Expiry = DateTime.Today.AddMonths(3) },
//            new Medicine { Id = 3, Name = "Augmentin", Stock = 30, Price = 25M, Expiry = DateTime.Today.AddMonths(12) }
//        };

//        public List<Medicine> Medicines { get; set; } = new List<Medicine>();

//        public string SearchName { get; set; }

//        public void OnGet(string searchName)
//        {
//            SearchName = searchName;

//            if (!string.IsNullOrWhiteSpace(searchName))
//            {
//                Medicines = AllMedicines
//                    .Where(m => m.Name.Contains(searchName, StringComparison.OrdinalIgnoreCase))
//                    .ToList();
//            }
//            else
//            {
//                Medicines = AllMedicines;
//            }
//        }
//    }

//    public class Medicine
//    {
//        public int Id { get; set; }
//        public string Name { get; set; }
//        public int Stock { get; set; }
//        public decimal Price { get; set; }
//        public DateTime Expiry { get; set; }
//    }
//}


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
