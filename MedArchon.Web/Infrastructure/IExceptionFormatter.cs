using System;
using System.Web;

namespace MedArchon.Web.Infrastructure
{
    public interface IExceptionFormatter
    {
        void LogException(Exception exception);

        void LogHttpException(HttpRequestBase httpRequest, HttpException httpException);
    }
}