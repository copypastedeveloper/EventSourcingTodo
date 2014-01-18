using System.ComponentModel;
using System.Configuration.Install;

namespace Example.Web.Denormalizer.Host
{
    [RunInstaller(true)]
    public partial class WebDenormalizerInstaller : Installer
    {
        public WebDenormalizerInstaller()
        {
            InitializeComponent();
        }
    }
}
