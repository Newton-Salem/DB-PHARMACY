//using Microsoft.AspNetCore.Mvc.RazorPages;
//using PHARMACY.DAO;

//namespace PHARMACY.Pages.Admin
//{
//    public class IndexModel : PageModel
//    {
//        private readonly DashboardDAO dao = new();

//        public int CompletedOrders { get; set; }
//        public int PendingOrders { get; set; }
//        public int CancelledOrders { get; set; }

//        public decimal TotalRevenue { get; set; }  // 👈 لازم هنا

//        public void OnGet()
//        {
//            CompletedOrders = dao.CountOrdersByStatus("Completed");
//            PendingOrders = dao.CountOrdersByStatus("Pending");
//            CancelledOrders = dao.CountOrdersByStatus("Cancelled");

//            TotalRevenue = dao.GetTotalRevenue(); ;   // 👈 هنا بيتحط الرقم
//        }





//    }
//}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PHARMACY.DAO;

namespace PHARMACY.Pages.Admin   // 🔥 دي أهم سطر
{
    public class IndexModel : PageModel
    {
        private readonly DashboardDAO dao = new();
        private readonly ReportDAO reportDAO = new();

        public int CompletedOrders { get; set; }
        public int PendingOrders { get; set; }
        public int CancelledOrders { get; set; }
        public decimal TotalRevenue { get; set; }

        public void OnGet()
        {
            CompletedOrders = dao.CountOrdersByStatus("Completed");
            PendingOrders = dao.CountOrdersByStatus("Pending");
            CancelledOrders = dao.CountOrdersByStatus("Cancelled");
            TotalRevenue = dao.GetTotalRevenue();
        }



    }
}
