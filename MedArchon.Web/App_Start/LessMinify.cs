using System;
using System.Web.Optimization;

namespace MedArchon.Web.App_Start
{
    public class LessMinify : IBundleTransform
    {
        private readonly CssMinify _cssMinify = new CssMinify();
        public void Process(BundleContext context, BundleResponse response)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (response == null)
                throw new ArgumentNullException("response");

            response.Content = dotless.Core.Less.Parse(response.Content);
            _cssMinify.Process(context, response);
        }
    }
}