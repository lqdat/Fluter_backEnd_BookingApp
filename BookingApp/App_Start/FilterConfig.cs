﻿using System.Web.Mvc;

namespace BookingApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new LogAttribute());
        }
    }
}
