using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestApp.Filter
{
    public class OAuthFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Debug.Write($"OnActionExecuting:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} \n");
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Debug.Write($"OnActionExecuted:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} \n");
            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            Debug.Write($"OnResultExecuting:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} \n");
            base.OnResultExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            Debug.Write($"OnResultExecuted:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} \n");
            base.OnResultExecuted(filterContext);
        }
    }
}