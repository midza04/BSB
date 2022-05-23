using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.ServiceProcess;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Configuration;

namespace SignalRChat
{
    public class DashboardHub : Hub
    {

        PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        PerformanceCounter ramCounter;
        string _serverName = ConfigurationManager.AppSettings.Get("ServerName");
        string _SqlServerName = ConfigurationManager.AppSettings.Get("SQLServerName");
        string _dbfFintechServiceName = ConfigurationManager.AppSettings.Get("DBFFintechServiceName");
        string _sqlServerviceName = ConfigurationManager.AppSettings.Get("SQLServiceName");

        ServiceController svc;
        ServiceController sql;


        public DashboardHub()
        {
            try
            {
                svc = new ServiceController(_dbfFintechServiceName, _serverName);
                sql = new ServiceController(_sqlServerviceName, _SqlServerName);
                ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            }
            catch (Exception ex)
            {
                Clients.Caller.throwError(ex.Message);
            }
        }

        public void ServiceStatus()
        {
            
            try
            {
                StatusModel clientModel = new StatusModel();

                clientModel.serviceStatus = svc.Status.ToString();
                clientModel.sqlStatus = sql.Status.ToString();
                clientModel.cpuUsage = cpuCounter.NextValue();
                clientModel.ramUsage = ramCounter.NextValue().ToString() + "MB";
                clientModel.spaceAvailable = GetAvailableDiskSpace() + "GB";
                Clients.Caller.updateServiceStatus(clientModel);
            }
            catch (Exception ex)
            {
                Clients.Caller.throwError(ex.Message);
            }
        }

        #region DBFfintech goAML Service functions

        public void StartDBFFintechService()
        {
            try
            {
                if (svc.Status == ServiceControllerStatus.Stopped)
                {
                    svc.Start();
                    Clients.Caller.startService("Done");
                }
            }
            catch (Exception)
            {

                Clients.Caller.startService("Error");
            }
        }

        public void StopDBFFintechService()
        {
            try
            {
                if (svc.Status == ServiceControllerStatus.Running)
                {
                    svc.Stop();
                    Clients.Caller.stopService("Done");
                }
            }
            catch (Exception)
            {

                Clients.Caller.stopService("Error");
            }
        }

        public void RestartDBFFintechService()
        {
            try
            {
                if (svc.Status == ServiceControllerStatus.Running)
                {
                    svc.Stop();
                    svc.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Stopped);                
                }                

                svc.Start();
                svc.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Running);
                Clients.Caller.startService("Done");
            }
            catch (Exception)
            {

                Clients.Caller.stopService("Error");
            }

        } 
        #endregion

        #region SQL server Service functions
        public void StartSQLService()
        {
            try
            {
                if (sql.Status == ServiceControllerStatus.Stopped)
                {
                    sql.Start();
                    Clients.Caller.startSQLService("Done");
                }
            }
            catch (Exception)
            {

                Clients.Caller.startService("Error");
            }
        }

        public void StopSQLService()
        {
            try
            {
                if (sql.Status == ServiceControllerStatus.Running)
                {
                    sql.Stop();
                    Clients.Caller.stopSQLService("Done");
                }
            }
            catch (Exception)
            {

                Clients.Caller.startService("Error");
            }
        }

        public void RestartSQLService()
        {
            try
            {
                if (svc.Status == ServiceControllerStatus.Running)
                {
                    sql.Stop();
                    sql.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Stopped);
                    sql.Start();
                    sql.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Running);
                    Clients.Caller.startSQLService("Done");
                }
            }
            catch (Exception)
            {

                Clients.Caller.stopService("Error");
            }
        }

        #endregion


        public string GetAvailableDiskSpace()
        {
            DriveInfo[] driveInfoArray = System.IO.DriveInfo.GetDrives();
            float megaBytes =  (driveInfoArray[0].TotalFreeSpace / 1024f) / 1024f / 1024f;
            return megaBytes.ToString("0.00");
        }

        public class StatusModel
        {
            [JsonProperty("serviceStatus")]
            public string serviceStatus { get; set; }
            [JsonProperty("sqlStatus")]
            public string sqlStatus { get; set; }
            [JsonProperty("cpuUsage")]
            public float cpuUsage { get; set; }
            [JsonProperty("ramUsage")]            
            public string ramUsage { get; set; }
            [JsonProperty("spaceAvailable")]
            public string spaceAvailable { get; set; }
        }
    }
}