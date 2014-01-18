using System.ServiceProcess;

namespace Example.Web.Denormalizer.Host
{
    partial class WebDenormalizerHost : ServiceBase
    {
        public WebDenormalizerHost()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            StartUp();
        }

        protected override void OnStop()
        {
            ShutDown();
        }

        public void StartUp()
        {
        }

        public void ShutDown()
        {
        }
    }
}
