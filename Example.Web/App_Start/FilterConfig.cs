using System.Web.Http.Filters;
using System.Web.Mvc;
using Example.Web.Infrastructure;
using StructureMap;

namespace Example.Web.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterHttpFilters(HttpFilterCollection filters)
        {
            filters.Add(new HttpErrorFilter(ObjectFactory.GetInstance<IExceptionFormatter>()));
        }
    }
}