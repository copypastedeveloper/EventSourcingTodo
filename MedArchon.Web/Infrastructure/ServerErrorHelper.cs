using System;
using System.Web;

namespace MedArchon.Web.Infrastructure
{
    public class ServerErrorHelper
    {
        internal static void HandleApplicationError(HttpContextBase context, IExceptionFormatter exceptionFormatter)
        {
            HttpServerUtilityBase httpServerUtility = context.Server;
            Exception exception = httpServerUtility.GetLastError();

            HttpException httpException = exception as HttpException;
            if (httpException != null)
            {
                exceptionFormatter.LogHttpException(context.Request, httpException);
            }
            else
            {
                exceptionFormatter.LogException(exception);
            }
        }
    }
}