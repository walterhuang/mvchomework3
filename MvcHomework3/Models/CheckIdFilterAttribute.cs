using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcHomework3.Models
{
    public class CheckIdFilterAttribute : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var obj = filterContext.RouteData.Values["id"];
            if (obj != null)
            {
                int id;
                if (!int.TryParse(obj.ToString(), out id))
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = "Home" }));
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}