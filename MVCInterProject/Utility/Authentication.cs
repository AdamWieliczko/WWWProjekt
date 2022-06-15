using Microsoft.AspNetCore.Mvc.Filters;
using MVCInterProject.Models;

namespace MVCInterProject{
public class BasicAuthenticationAttribute : ActionFilterAttribute
    {
        private readonly AW_TestContext _context = new AW_TestContext();
        public static string nickname;
        public static string password;


        public BasicAuthenticationAttribute()
        {
            
        }
 
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
            if (true) //sprawdzenie poprawnosci z bazy
            {
                return;
            }
        }
    }
}