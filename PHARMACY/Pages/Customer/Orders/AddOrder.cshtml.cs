using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace PHARMACY.Pages.Customer.Orders
{
    public class AddOrderModel : PageModel
    {
        [BindProperty] public int MedicineID { get; set; }
        [BindProperty] public string MedicineName { get; set; }
        [BindProperty] public decimal UnitPrice { get; set; }
        [BindProperty] public int Quantity { get; set; } = 1;

        public IActionResult OnGet()
        {
           
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            int newId = IndexModel.OrdersData.Any() ? IndexModel.OrdersData.Max(o => o.OrderID) + 1 : 1;

            IndexModel.OrdersData.Add(new Order
            {
                OrderID = newId,
                MedicineName = MedicineName,
                Quantity = Quantity,
                Total = Quantity * UnitPrice,
                Status = "Pending"
            });

            return RedirectToPage("/Customer/Orders/Index");
        }
    }
}
