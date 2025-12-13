using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PHARMACY.Pages
{
    public class BasePageModel : PageModel
    {
        public override void OnPageHandlerExecuting(
            Microsoft.AspNetCore.Mvc.Filters.PageHandlerExecutingContext context)
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            base.OnPageHandlerExecuting(context);
        }
    }
}
