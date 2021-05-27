using System.Web.Mvc;
using System.Web.Routing;
using OtherClasses;
using msfunc;

namespace OtherClasses
{
    public class AuthUser : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.RouteData.Values["controller"].ToString() != "Login")
            {
                //user exists
                if (filterContext.HttpContext.Request.Cookies["mrCooks"] != null)
                {
                    var vv = ErpFunc.RGet<OtherClasses.Models.MR_DATA.MRSTLEV>("MR_DATA", "Select PASSWORD"+
                        " from MRSTLEV WHERE OPERATOR = @p1 AND PASSWORD = @p2 ",
                            filterContext.HttpContext.Request.Cookies["mrName"].Value,
                            filterContext.HttpContext.Request.Cookies["mrCooks"].Value);
                    
                    if (vv == null)
                    {
                        filterContext.Result = new RedirectToRouteResult(
                              new RouteValueDictionary(new { controller = "Login", action = "Index" }));
                    }
                }
                else
                {
                    //save where you are coming from and redirect to it after successful login
                    string idVal = filterContext.RouteData.Values["id"] == null ?
                        " " : filterContext.RouteData.Values["id"].ToString();

                    filterContext.HttpContext.Response.Cookies["mrRefrURL"].Value =
                        filterContext.RouteData.Values["controller"].ToString() + "/" +
                        filterContext.RouteData.Values["action"].ToString() + "/" + idVal;

                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = "Login", action = "Index" }));
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}