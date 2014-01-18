using System.ServiceProcess;

namespace Example.Web.Denormalizer.Host
{
    partial class WebDenormalizerInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        ServiceProcessInstaller serviceProcessInstaller1;
        ServiceInstaller serviceInstaller1;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller1
            // 
            this.serviceProcessInstaller1.Password = null;
            this.serviceProcessInstaller1.Username = null;
            this.serviceProcessInstaller1.Account = ServiceAccount.LocalSystem;
            // 
            // serviceInstaller1
            // 
            this.serviceInstaller1.Description = "Denormalize domain events into viewmodel data";
            this.serviceInstaller1.DisplayName = "Example Web Denormalizer";
            this.serviceInstaller1.ServiceName = "Example.CaseManagement.Web.Denormalizer";
            this.serviceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // WebDenormalizerInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller1,
            this.serviceInstaller1});

        }

        #endregion

        private System.ServiceProcess.ServiceController serviceController1;
    }
}