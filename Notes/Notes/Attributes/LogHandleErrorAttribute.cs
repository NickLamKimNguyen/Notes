using Notes.Common.Models.Entities;
using Notes.Common.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Notes.Attributes
{
    public class LogHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            try
            {
                var ex = filterContext.Exception;
                var baseEx = ex.GetBaseException();
                var request = filterContext.HttpContext.Request;
                var inputStream = request.InputStream;
                inputStream.Position = 0;
                using (var reader = new StreamReader(inputStream))
                using (var logRepo = new LogRepository())
                {
                    logRepo.Insert(new Log
                    {
                        App = "Notes",
                        Controller = filterContext.RouteData.Values["controller"].ToString(),
                        Action = filterContext.RouteData.Values["action"].ToString(),
                        Data = string.Format("QueryString: {0} | InputStream: {1}", request.QueryString, reader.ReadToEnd()),
                        Message = string.Format("Base: {0} | Main: {1} | Inner: {2}", baseEx != null ? baseEx.Message : string.Empty, ex.Message, ex.InnerException != null ? ex.InnerException.Message : string.Empty)
                    }).Wait();
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}