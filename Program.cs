using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System.Diagnostics;
using System;

namespace DBFfintech_Interface_Portal
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        { 
         #if (!DEBUG)  

                     
          
            // More than one user Service may run within the same process. To add
            // another service to this process, change the following line to
            // create a second service object. For example,
            //
            //   ServicesToRun = new ServiceBase[] {new Service1(), new MySecondUserService()};
            Datalayer obj = new Datalayer();
            try
            {
                // obj.LogError("Before Step1 Start", "BF 1 ");
                ServiceBase[] ServicesToRun;
                // obj.LogError("Step 1 Start", "Step 1 ");
                ServicesToRun = new ServiceBase[] { new DBFfintech_Interface_Main() };
                // obj.LogError("Step 1 End", "Step 1 ");
                //obj.LogError("Step 2 Start", "Step 2 ");
                ServiceBase.Run(ServicesToRun);
                // obj.LogError("Step 2 end", "Step 2 "); 
                // obj.LogError("Step 3 Start", "Step 3 ");
                System.ServiceProcess.ServiceBase.Run(ServicesToRun);
                // obj.LogError("Step 3 Start", "Step 3  ");
            }
            catch (Exception ex)
            {
                obj.LogError("Before service start", ex.Message);
            }
            finally
            {
                obj = null;
            }

#else
            DBFfintech_Interface_Main service = new DBFfintech_Interface_Main();
            service.ProcessAll();
            //service.ProcessSTR();

            //run this to update MasterData table with records from corebanking


            //service.GetAllTransactions(); 
            //service.UploadXML();  
            //service.SaveXML("<report></report>");

            //service.SendEmailOfNoRecords();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);

#endif
        }

    }
}