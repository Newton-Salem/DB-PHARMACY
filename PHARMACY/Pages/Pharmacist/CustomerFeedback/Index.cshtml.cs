using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PHARMACY.DAO;
using System.Collections.Generic;

namespace PHARMACY.Pages.Pharmacist.CustomerFeedback
{
    public class IndexModel : PageModel
    {
        FeedbackDAO dao = new();

        public List<FeedbackEntry> Feedbacks { get; set; } = new();

        public IActionResult OnGet()
        {
            // 🔐 Security check
            if (HttpContext.Session.GetString("Role") != "Pharmacist")
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Account/Login");
            }

            var data = dao.GetAll();

            foreach (var f in data)
            {
                Feedbacks.Add(new FeedbackEntry
                {
                    OrderID = f.OrderID,
                    CustomerName = f.CustomerName,
                    FeedbackText = f.Message
                });
            }

            return Page();
        }
    }

    public class FeedbackEntry
    {
        public int OrderID { get; set; }
        public string CustomerName { get; set; } = "";
        public string FeedbackText { get; set; } = "";
    }
}
