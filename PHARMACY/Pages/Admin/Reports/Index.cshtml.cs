//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using PHARMACY.DAO;
//using PHARMACY.Model;
//using System.Collections.Generic;

//namespace PHARMACY.Pages.Admin.Reports
//{
//    public class IndexModel : PageModel
//    {
//        private readonly ReportDAO reportDAO = new();

//        public List<Report> Reports { get; set; } = new();

//        public IActionResult OnGet()
//        {
//            var role = HttpContext.Session.GetString("Role");
//            if (role != "Admin")
//                return RedirectToPage("/Account/Login");

//            Reports = reportDAO.GetAll();
//            return Page();
//        }

//        // 🔘 Generate Sales Report
//        public IActionResult OnPostGenerateSalesReport()
//        {
//            int adminId = HttpContext.Session.GetInt32("UserID").Value;

//            reportDAO.Add(
//                "Monthly sales report",
//                "Sales",
//                adminId
//            );

//            TempData["Success"] = "Sales report generated successfully";
//            return RedirectToPage();
//        }

//        // 🔘 Generate Inventory Report
//        public IActionResult OnPostGenerateInventoryReport()
//        {
//            int adminId = HttpContext.Session.GetInt32("UserID").Value;

//            reportDAO.Add(
//                "Inventory status report",
//                "Inventory",
//                adminId
//            );

//            TempData["Success"] = "Inventory report generated successfully";
//            return RedirectToPage();
//        }

//        // 🔘 Generate Feedback Report
//        public IActionResult OnPostGenerateFeedbackReport()
//        {
//            int adminId = HttpContext.Session.GetInt32("UserID").Value;

//            reportDAO.Add(
//                "Customer feedback report",
//                "Feedback",
//                adminId
//            );

//            TempData["Success"] = "Feedback report generated successfully";
//            return RedirectToPage();
//        }
//    }
//}
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PHARMACY.Pages.Admin.Reports
{
    public class IndexModel : PageModel
    {
        public void OnGet() { }
    }
}
