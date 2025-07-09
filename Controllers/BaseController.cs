using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OutletStatusPortal.Models;

using Microsoft.AspNetCore.Authorization;


public class BaseController : Controller
{
    protected readonly Outletdbcontext _context;

    public BaseController(Outletdbcontext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Automatically skip if the action allows anonymous access
        var actionDescriptor = context.ActionDescriptor;
        var endpointMetadata = actionDescriptor.EndpointMetadata;

        bool isAnonymousAllowed = endpointMetadata.Any(m => m is AllowAnonymousAttribute);

        if (!isAnonymousAllowed && User.Identity?.IsAuthenticated == true)
        {
            var user = _context.Users.FirstOrDefault(u => u.StafId == User.Identity.Name);
            if (user != null)
            {
                ViewBag.CurrentUserName = user.Name;
            }
        }

        base.OnActionExecuting(context);
    }
}

