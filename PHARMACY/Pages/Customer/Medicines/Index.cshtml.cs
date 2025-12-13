using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace PHARMACY.Pages.Customer.Medicines
{
    public class IndexModel : PageModel
    {
        public List<Medicine> Medicines { get; set; }

        public void OnGet()
        {
            Medicines = new List<Medicine>
            {
                new Medicine{ Medicine_ID=1, Name="Paracetamol", Price=10 },
                new Medicine{ Medicine_ID=2, Name="Ibuprofen", Price=20 },
                new Medicine{ Medicine_ID=3, Name="Vitamin C", Price=15 }
            };
        }
    }

    public class Medicine
    {
        public int Medicine_ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
