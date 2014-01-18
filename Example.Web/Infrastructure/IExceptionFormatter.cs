using System;
using System.Web;

namespace Example.Web.Infrastructure
{
    public interface IExceptionFormatter
    {
        void LogException(Exception exception);

        void LogHttpException(HttpRequestBase httpRequest, HttpException httpException);
    }
}