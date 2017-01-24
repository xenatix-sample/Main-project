namespace SynchronizationService
{
    partial class ServiceInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.ADServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.ADserviceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // ADServiceProcessInstaller
            // 
            this.ADServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.ADServiceProcessInstaller.Password = null;
            this.ADServiceProcessInstaller.Username = null;
            this.ADServiceProcessInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.ADServiceProcessInstaller_AfterInstall);
            // 
            // ADserviceInstaller
            // 
            this.ADserviceInstaller.Description = "Service for synchronizing various resources.";
            this.ADserviceInstaller.ServiceName = "XenatiX Synchronizer";
            this.ADserviceInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.ADserviceInstaller_AfterInstall);
            // 
            // ServiceInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ADServiceProcessInstaller,
            this.ADserviceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller ADServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller ADserviceInstaller;
    }
}