using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MedArchon.Web.App_Start;
using MedArchon.Web.Infrastructure;
using StructureMap;

namespace MedArchon.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            GlobalConfiguration.Configuration.Filters.Add(new HttpErrorFilter(ObjectFactory.GetInstance<IExceptionFormatter>()));
            FilterConfig.RegisterHttpFilters(GlobalConfiguration.Configuration.Filters);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            ServerErrorHelper.HandleApplicationError(new HttpContextWrapper(HttpContext.Current), ObjectFactory.GetInstance<IExceptionFormatter>());
        }
    }
}