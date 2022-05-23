namespace Process_Killer
{
    partial class DBFfintech_Interface_Portal
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
            this.DBFfintech_Interface_Install = new System.ServiceProcess.ServiceProcessInstaller();
            this.DBFfintech_Interface_Main = new System.ServiceProcess.ServiceInstaller();
            // 
            // DBFfintech_Interface_Install
            // 
            this.DBFfintech_Interface_Install.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.DBFfintech_Interface_Install.Password = null;
            this.DBFfintech_Interface_Install.Username = null;
            // 
            // DBFfintech_Interface_Main
            // 
            this.DBFfintech_Interface_Main.Description = "FIA XML goAML file generation for upload from the Bankers Real System Developed b" +
    "y DBFfintech";
            this.DBFfintech_Interface_Main.DisplayName = "DBFfintech_Interface_Portal";
            this.DBFfintech_Interface_Main.ServiceName = "DBFfintech_Interface_Portal";
            this.DBFfintech_Interface_Main.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.DBFfintech_Interface_Main.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.Service_Killer1_AfterInstall);
            // 
            // DBFfintech_Interface_Portal
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.DBFfintech_Interface_Main,
            this.DBFfintech_Interface_Install});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller DBFfintech_Interface_Install;
        private System.ServiceProcess.ServiceInstaller DBFfintech_Interface_Main;
    }
}