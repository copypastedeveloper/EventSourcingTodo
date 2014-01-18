using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Mvc;
using Example.Web.Infrastructure;
using IExceptionFilter = System.Web.Http.Filters.IExceptionFilter;

namespace Example.Web.App_Start
{
    public class HttpErrorFilter : IExceptionFilter
    {
        readonly IExceptionFormatter _exceptionFormatter;

        public HttpErrorFilter(IExceptionFormatter exceptionFormatter)
        {
            _exceptionFormatter = exceptionFormatter;
        }

        // I don't think with Web API that this will ever be fired.
        public void OnException(ExceptionContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            ServerErrorHelper.HandleApplicationError(context.HttpContext, _exceptionFormatter);
        }

        // This is relavent if we are using this as a contorller or action attribute.
        // http://msdn.microsoft.com/en-us/library/system.web.http.filters.iexceptionfilter_properties(v=vs.108).aspx
        public bool AllowMultiple { get { return false; } }

        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext context, CancellationToken cancellationToken)
        {
            if (context == null) throw new ArgumentNullException("context");

            _exceptionFormatter.LogException(context.Exception);
            return null;
        }
    }
}