using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MVCInterProject.Models;

namespace MVCInterProject{
public class BasicAuthenticationAttribute : ActionFilterAttribute
    {
        private readonly AW_TestContext _context = new AW_TestContext();

        public BasicAuthenticationAttribute()
        {
            
        }
 
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var isAnyUserRegistered = _context.People
            .Where(i => i.PerName.Equals(UsernameAndPassword.username) && i.PerPassword.Equals(UsernameAndPassword.password)).Any();

            if (isAnyUserRegistered)
            {
                return;
            }

            filterContext.Result = new RedirectResult(string.Format("/Login",filterContext.HttpContext.Request.Path));
        }
    }
}