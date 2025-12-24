using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PHARMACY.DAO;
using PHARMACY.Model;
using System.Collections.Generic;

namespace PHARMACY.Pages.Supplier.Notifications
{
    public class IndexModel : PageModel
    {
        private readonly NotificationDAO dao = new();

        public List<PHARMACY.Model.Notification> Notifications { get; set; } = new();


        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Role") != "Supplier")
                return RedirectToPage("/Account/Login");

            int supplierId = HttpContext.Session.GetInt32("UserID").Value;
            Notifications = dao.GetByUser(supplierId);

            return Page();
        }

        public IActionResult OnPostRead(int id)
        {
            dao.MarkAsRead(id);
            return RedirectToPage();
        }
    }
}
