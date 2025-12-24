using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PHARMACY.DAO;
using System.Collections.Generic;

namespace PHARMACY.Pages.Pharmacist.Notifications
{
    public class IndexModel : PageModel
    {
        public List<PHARMACY.Model.Notification> Notifications { get; set; } = new();

        private readonly NotificationDAO dao = new();

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (string.IsNullOrEmpty(role) || userId == null)
                return RedirectToPage("/Account/Login");

            ViewData["NotifCount"] = dao.CountUnread(userId.Value);
            Notifications = dao.GetByUser(userId.Value);

            return Page();
        }

        //  Mark as Read
        public IActionResult OnPostRead(int id)
        {
            dao.MarkAsRead(id);
            return RedirectToPage();
        }
    }
}
