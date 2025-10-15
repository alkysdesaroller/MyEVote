using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using myEVote.Helpers;

namespace myEVote.Filters;

public class AdminAuthorizacionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        
        if (!SessionHelper.IsAuthenticated(context.HttpContext.Session))
        {
            context.Result = new RedirectToActionResult("Login", "Account", null);
            return;
        }

        if (!SessionHelper.IsAdmin(context.HttpContext.Session))
        {
            context.Result = new RedirectToActionResult("AccessDenied", "Home", null);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        throw new NotImplementedException();
    }
}