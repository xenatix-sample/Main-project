using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace SynchronizationService
{
    [RunInstaller(true)]
    public partial class ServiceInstaller : System.Configuration.Install.Installer
    {
        public ServiceInstaller()
        {
            InitializeComponent();
        }

        private void ADServiceProcessInstaller_AfterInstall(object sender, InstallEventArgs e)
        {

        }

        private void ADserviceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
