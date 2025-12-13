using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PHARMACY.Pages.Pharmacist.Medicines
{
    public class IndexModel : PageModel
    {
        public List<string> Medicines { get; set; }

        public void OnGet()
        {
            Medicines = new List<string> { "Panadol", "Vitamin C", "Augmentin" };
        }
    }
}
