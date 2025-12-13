using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using PHARMACY.Pages.Customer.Orders; // نجيب namespace صفحة الأوردرز

namespace PHARMACY.Pages.Customer
{
    public class IndexModel : PageModel
    {
        public string CustomerName { get; set; } = "Ahmed Ali";

        // عدد الأوردرات الفعلي
        public int OrdersCount { get; set; }

        // إجمالي المبلغ المدفوع
        public decimal TotalSpent { get; set; }

        public IActionResult OnGet()
        {
            // التأكد من صلاحية الدور
            if (HttpContext.Session.GetString("Role") != "Customer")
            {
                return RedirectToPage("/Account/Login");
            }

            // 🌟 ربط القيم بالبيانات الحقيقية
            OrdersCount = Orders.IndexModel.OrdersData.Count;
            TotalSpent = Orders.IndexModel.OrdersData.Sum(o => o.Total);

            // الاسم من الجلسة لو موجود
            CustomerName = HttpContext.Session.GetString("Username") ?? CustomerName;

            return Page();
        }
    }
}
