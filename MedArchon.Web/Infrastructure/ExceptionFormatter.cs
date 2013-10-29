using System;
using System.Web;
using log4net;

namespace MedArchon.Web.Infrastructure
{
    public class ExceptionFormatter : IExceptionFormatter
    {
        internal static readonly string ErrorFormat = String.Format("An unhandled exception occurred:{0}Message: {{0}}{0}{0}Stack Trace:{0}{{1}}", Environment.NewLine);
        internal static readonly string HttpErrorFormat = String.Format("An unhandled exception occurred:{0}Message: {{0}}{0}{0}UserAgent:{{1}}{0}{0}URL:{{2}}{0}{0}Stack Trace:{0}{{3}}", Environment.NewLine);

        readonly ILog _log;

        public ExceptionFormatter(ILog log)
        {
            _log = log;
        }

        public void LogException(Exception exception)
        {
            if (exception == null) return;
            _log.ErrorFormat(ErrorFormat, exception.Message, exception.StackTrace);
        }

        public void LogHttpException(HttpRequestBase httpRequest, HttpException httpException)
        {
            if (httpException == null) return;
            if (httpRequest == null)
            {
                LogException(httpException);
                return;
            }
            _log.ErrorFormat(HttpErrorFormat, httpException.Message, httpRequest.UserAgent, httpRequest.Url, httpException.StackTrace);
        }
    }
}