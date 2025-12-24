using Microsoft.AspNetCore.Mvc.RazorPages;
using PHARMACY.DAO;
using System;
using System.Collections.Generic;

namespace PHARMACY.Pages.Admin.Reports
{
    public class SalesModel : PageModel
    {
        private readonly DashboardDAO dao = new();

        public decimal TotalRevenue { get; set; }
        public int CompletedOrders { get; set; }

        public string ReportMonth { get; set; }

        public List<PharmacistSalesVM> PharmacistSales { get; set; } = new();

        public void OnGet()
        {
            DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime end = start.AddMonths(1);

            ReportMonth = start.ToString("MMMM yyyy");

            TotalRevenue = dao.GetTotalRevenueByDate(start, end);
            CompletedOrders = dao.CountCompletedOrdersByDate(start, end);

            PharmacistSales = dao.GetSalesByPharmacist(start, end);
        }
    }

    public class PharmacistSalesVM
    {
        public string PharmacistName { get; set; }
        public decimal TotalSales { get; set; }
        public int OrdersCount { get; set; }
    }
}
