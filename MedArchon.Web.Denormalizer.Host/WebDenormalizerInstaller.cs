using System.ComponentModel;
using System.Configuration.Install;

namespace MedArchon.Web.Denormalizer.Host
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
