using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace PHARMACY.Pages.Pharmacist.CustomerFeedback
{
    public class IndexModel : PageModel
    {
      
        public List<FeedbackEntry> Feedbacks { get; set; }

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role != "Pharmacist")
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Account/Login");
            }

         
            Feedbacks = PHARMACY.Pages.Customer.Orders.FeedbackModel.Feedbacks
                .ConvertAll(f => new FeedbackEntry
                {
                    OrderID = f.OrderID,
                    CustomerName = f.CustomerName,
                    FeedbackText = f.FeedbackText
                });

            return Page();
        }

    }

  
    public class FeedbackEntry
    {
        public int OrderID { get; set; }
        public string CustomerName { get; set; }
        public string FeedbackText { get; set; }
    }
}
