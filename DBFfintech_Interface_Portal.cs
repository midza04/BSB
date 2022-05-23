using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;

namespace Process_Killer
{
    [RunInstaller(true)]
    public partial class DBFfintech_Interface_Portal : Installer
    {
        public DBFfintech_Interface_Portal()
        {
            InitializeComponent();
        }

        private void Service_Killer1_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}