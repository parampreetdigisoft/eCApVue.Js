using ECap.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ECap.Web.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        
        {
            string language = string.Empty;
            if (HttpContext.Request.Query.Count > 0 && !string.IsNullOrEmpty(HttpContext.Request.Query["lang"]))
            {
                language = Convert.ToString(HttpContext.Request.Query["lang"]);
                HttpContext.SetCookies(CookieConstants.CULTURE, language, null);
            }
        }
    }
}
