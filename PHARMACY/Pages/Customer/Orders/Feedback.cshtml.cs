using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PHARMACY.DAO;

namespace PHARMACY.Pages.Customer.Orders
{
    public class FeedbackModel : PageModel
    {
        FeedbackDAO dao = new();

        [BindProperty(SupportsGet = true)]
        public int OrderId { get; set; }

        [BindProperty]
        public string FeedbackText { get; set; } = "";

        public bool AlreadySubmitted { get; set; }

        public IActionResult OnGet(int orderId)
        {
            if (HttpContext.Session.GetString("Role") != "Customer")
                return RedirectToPage("/Account/Login");

            int customerId = HttpContext.Session.GetInt32("UserID") ?? 0;

            OrderId = orderId;
            AlreadySubmitted = dao.FeedbackExists(orderId, customerId);

            return Page();
        }

        public IActionResult OnPost()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Customer")
                return RedirectToPage("/Account/Login");

            int? customerId = HttpContext.Session.GetInt32("UserID");
            if (customerId == null)
                return RedirectToPage("/Account/Login");

            dao.AddFeedback(OrderId, customerId.Value, FeedbackText);

            
            TempData["SuccessMessage"] =
                "Your feedback has been submitted successfully. Thank you for your interest 🙏";

            return RedirectToPage("/Customer/Orders/Index");
        }


    }
}
