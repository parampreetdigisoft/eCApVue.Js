using ECap.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ECap.Web.Controllers
{
    public class BaseAdminController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userId = filterContext.HttpContext.Session.GetInt32(SessionConstants.USER_ID);
            if (!(userId > 0))
            {
                bool isAjaxRequest =
                    Convert.ToString(filterContext.HttpContext.Request?.Headers["X-Requested-With"]) == "XMLHttpRequest";
                if (!isAjaxRequest)
                {
                    var actionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)filterContext.ActionDescriptor).ActionName;
                    var controllerName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)filterContext.ActionDescriptor).ControllerName;

                    string redirectURL = Url.Action(action: "login",
                        controller: "profile", values: new { returnUrl = Url.Action(action: actionName, controller: controllerName) });
                    filterContext.Result = new RedirectResult(System.Web.HttpUtility.UrlDecode(redirectURL));
                    return;
                } 
            }
        }
    }
}
