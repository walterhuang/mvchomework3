﻿using System.Web;
using System.Web.Mvc;

namespace MvcHomework3
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new Models.CheckIdFilterAttribute());
        }
    }
}
