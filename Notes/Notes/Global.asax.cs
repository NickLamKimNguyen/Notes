using Notes.Binders;
using Notes.Common.Models.Entities;
using Notes.Common.Models.Enums;
using Notes.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Notes
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBinders.Binders.Add(typeof(DaysOfTheWeek), new FlagEnumModelBinder<DaysOfTheWeek>());
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                var ex = Server.GetLastError();
                using (var logRepo = new LogRepository())
                {
                    logRepo.Insert(new Log
                    {
                        App = "Notes",
                        Controller = "Global ASAX",
                        Action = "Application Error",
                        Data = string.Empty,
                        Message = string.Format("Main: {0} | Inner: {1}", ex.Message, ex.InnerException != null ? ex.InnerException.Message : string.Empty)
                    }).Wait();
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
