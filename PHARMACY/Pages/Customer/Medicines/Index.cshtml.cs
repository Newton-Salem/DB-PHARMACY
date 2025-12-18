using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PHARMACY.DAO;
using PHARMACY.Models;
using System.Collections.Generic;

namespace PHARMACY.Pages.Customer.Medicines
{
    public class IndexModel : PageModel
    {
        private readonly MedicineDAO _medicineDAO = new MedicineDAO();

        public List<Medicine> Medicines { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public void OnGet()
        {
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                Medicines = _medicineDAO.Search(SearchTerm);
            }
            else
            {
                Medicines = _medicineDAO.GetAll();
            }
        }
    }
}
