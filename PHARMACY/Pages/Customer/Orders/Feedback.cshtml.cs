using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace PHARMACY.Pages.Customer.Orders
{
    public class FeedbackModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int OrderId { get; set; } // رقم الأوردر اللي هنعمل له فيدباك

        [BindProperty]
        public string FeedbackText { get; set; } // نص الفيدباك

        public string Message { get; set; }

        
        public static List<OrderFeedback> Feedbacks { get; set; } = new List<OrderFeedback>();

        public IActionResult OnGet(int orderId)
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role != "Customer")
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Account/Login");
            }

            // حدد الأوردر الحالي
            OrderId = orderId;

            // تحقق لو الفيدباك موجود 
            var existing = Feedbacks.FirstOrDefault(f => f.OrderID == orderId
                                                        && f.CustomerName == HttpContext.Session.GetString("Username"));
            if (existing != null)
            {
                Message = "You have already submitted feedback for this order!";
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToPage("/Account/Login");
            }

            // تحقق لو الفيدباك موجود
            var existing = Feedbacks.FirstOrDefault(f => f.OrderID == OrderId
                                                        && f.CustomerName == username);
            if (existing != null)
            {
                Message = "You have already submitted feedback for this order!";
                return Page();
            }

            // اضافة الفيدباك الجديد
            Feedbacks.Add(new OrderFeedback
            {
                OrderID = OrderId,
                CustomerName = username,
                FeedbackText = FeedbackText
            });

            Message = "Feedback submitted successfully!";
            // بعد ما يبعته يرجعه على صفحة الأوردرز
            return RedirectToPage("/Customer/Orders/Index");
        }
    }

    public class OrderFeedback
    {
        public int OrderID { get; set; }
        public string CustomerName { get; set; }
        public string FeedbackText { get; set; }
    }
}
