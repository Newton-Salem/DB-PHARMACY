using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PHARMACY.Pages.Pharmacist.Orders
{
    public class CreateModel : PageModel
    {
        
        public static List<SupplierOrder> OrdersData { get; set; } = new List<SupplierOrder>();

        public List<string> Suppliers { get; set; } = new()
        {
            "El Ezaby Supplier",
            "PharmaCare",
            "Medical United"
        };

        public List<string> Medicines { get; set; } = new()
        {
            "Panadol",
            "Vitamin C",
            "Augmentin"
        };

        [BindProperty]
        public SupplierOrder NewOrder { get; set; } = new();

        [BindProperty]
        public int CancelOrderId { get; set; } 

        public void OnGet()
        {
        }

        // أوردر جديد
        public IActionResult OnPost()
        {
            NewOrder.Id = OrdersData.Count + 1;
            NewOrder.OrderDate = DateTime.Now;
            NewOrder.Status = "Pending";

            OrdersData.Add(NewOrder);

            NewOrder = new SupplierOrder(); 
            return Page();
        }

        // الغاء أوردر موجود
        public IActionResult OnPostCancel()
        {
            var order = OrdersData.FirstOrDefault(o => o.Id == CancelOrderId);
            if (order != null && order.Status == "Pending")
            {
                order.Status = "Cancelled";
            }

            return Page();
        }
    }

    public class SupplierOrder
    {
        public int Id { get; set; }
        public string SupplierName { get; set; }
        public string MedicineName { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } 
    }
}
