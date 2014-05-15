using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcHomework3.Controllers
{
    public class BaseController : Controller
    {
        protected override void HandleUnknownAction(string actionName)
        {
            Response.Redirect("/", true);
        }
    }
}