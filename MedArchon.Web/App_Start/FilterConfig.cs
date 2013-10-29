using System.Web.Http.Filters;
using System.Web.Mvc;
using MedArchon.Web.Infrastructure;
using StructureMap;

namespace MedArchon.Web.App_Start
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