using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using VTIntranetD.Models.Dto;

namespace VTIntranetD.Controllers
{
    public class SessionTimeOutAttribute : ActionFilterAttribute
    {
        private SessionModel model;
        private RedirectToRouteResult routeData;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            model = (SessionModel)HttpContext.Current.Session["SessionData"];
            

            if ((HttpContext.Current.Session["SessionData"] == null) || (!model.UserActive))
            {
                routeData = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                    (new { controller = "Login", action = "Index" })
                );
                filterContext.Result = routeData;

                logger.Error("The Session Data is NULL, Return to Login System " + Environment.NewLine + DateTime.Now);

                return;
            }
            base.OnActionExecuting(filterContext);
        }


    }
}