using DBFFintech.goAML;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mail;
using System.ServiceProcess;
using System.Text;
using System.Threading;
//using System.Net;
using System.Xml;


namespace DBFfintech_Interface_Portal
{
    public partial class DBFfintech_Interface_Main : ServiceBase
    {

        SettingsModel _settings;
        #region Connection objects
        readonly String strCon = ConfigurationSettings.AppSettings.Get("StrConn");
        //readonly String strCon = "Server=41.76.211.191;initial catalog=DBFfintech_goAML;User id=sa;Password=Integrator31302";
        
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter da;
        #endregion

        private static string _pdfGeneratorPath = ConfigurationManager.AppSettings["PDFGenerator"].ToString();


        //private static string _pdfGeneratorPath = HttpContext.Current.Server.MapPath("~/PDFGenerator/wkhtmltopdf.exe");

        public static string _OutgoingGeneratedPDFReport_Styles_Logo = ConfigurationManager.AppSettings["Styles"];
        public static string _OutgoingGeneratedPDFReport = ConfigurationManager.AppSettings["TransactionPath"];

        public DBFfintech_Interface_Main()
        {
            InitializeComponent();
            Datalayer obj = new Datalayer();
            _settings = obj.LoadSettings();  
        }

        protected override void OnStart(string[] args)
        {
            Datalayer obj = new Datalayer();
            try
            {
                obj.LogError("Step 4 Start", "Step 4 ");
                timer1.Enabled = true;
                Thread thread = new Thread(new ParameterizedThreadStart(DoWork));
                obj.LogError("Step 1 End", "Step 4 ");
                thread.Start("goAML_FCUBS_CASH_DEPOSIT");
            }
            catch (Exception ex)
            {
               obj.LogError("OnStart function", ex.Message);
               obj = null; 
            } 
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }

        public IEnumerable<DateTime> EachCalendarDay(DateTime startDate, DateTime endDate)
        {
            for (var date = startDate.Date; date.Date <= endDate.Date; date = date.AddDays(1)) yield
            return date;
        }


        public void ProcessAll()
        {
            Datalayer obj = new Datalayer();            
            try
            {
                //obj.LogError("INSIDE PROCESS ALL", "START ");
                _settings = obj.LoadSettings();     

                //int LoggingAll = Convert.ToInt32(ConfigurationManager.AppSettings.Get("LoggingAll"));

                if (_settings.LoggingAll)
                {
                    obj.LogStep("Step 1", "Inside ProcessAll");
                }

                //try
                //{


                string host = string.Empty;

                try
                {
                    host = GenericMethods.GetServerNameAndIPAddress();

                    //DataTable dt = obj.GET_CASH_Deposit();



               
                    //if (dt != null)
                    //{
                    //    if (dt.Rows.Count > 0)
                    //    {
                    //        ProcessXML(dt);
                    //    }
                    //    else
                    //    {
                    //        SendEmailOfNoRecords();
                    //    }
                    //}


                    DateTime StartDate = Convert.ToDateTime("13-12-2021");
                    DateTime EndDate = Convert.ToDateTime("07-02-2022");
                    foreach (DateTime day in EachCalendarDay(StartDate, EndDate))
                    {
                        Console.WriteLine("Date is : " + day.ToString("yyyy-MM-dd"));

                        DataTable dt = obj.GET_CASH_Deposit_WithDate(day.ToString("yyyy-MM-dd") + " 00:00:00");
                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                ProcessXMLBulk(dt, day.ToString("dd-MM-yyyy"));
                            }
                            else
                            {
                                //SendEmailOfNoRecords();
                            }
                        }
                    }


                    if (_settings.LoggingSecondLevel)
                    {
                        obj.LogStep("LoggingSecondLevel 1", "Service Finished ProcessXML");
                    }

                }
                catch (Exception ex)
                {
                    obj.LogError("Inside ProcessXML function", ex.ToString());
                    throw ex;
                }
                finally
                {
                   
                }
            }
            catch(Exception ex)
            {

                obj.LogError("PROCESS ALL COVER", ex.Message);
                    
            }
            finally
            {
                obj = null; 
            }

        }

        //Militant Code for generating XML code for suspicious transaction
        public void ProcessSTR()
        {
            
            Datalayer obj = new Datalayer();
            obj.LogStep("Step 1", "New Windows Service By Sesfont Running");
            try
            {
                //obj.LogError("INSIDE PROCESS ALL", "START ");
                _settings = obj.LoadSettings();

                //int LoggingAll = Convert.ToInt32(ConfigurationManager.AppSettings.Get("LoggingAll"));

                if (_settings.LoggingAll)
                {
                    obj.LogStep("Step 1", "New Windows Service By Sesfont Running");
                }
                //try
                //{


                string host = string.Empty;

                try
                {
                    host = GenericMethods.GetServerNameAndIPAddress();

                    DataTable dt = obj.GET_STR();
                    string _toptrans = string.Empty;
                    _toptrans = sendTopTransactions();

                    if (dt != null)
                    {

                        if (dt.Rows.Count > 0)
                        {
                            ProcessSTRXML(dt);
                        }
                        else
                        {
                            SendEmailOfNoRecordsSTR();
                        }
                    }

                    if (_settings.LoggingSecondLevel)
                    {
                        obj.LogStep("LoggingSecondLevel 1", "Service Finished ProcessXML");
                    }

                }
                catch (Exception ex)
                {
                    obj.LogError("Inside ProcessXML function", ex.ToString());
                    throw ex;
                }
                finally
                {

                }
            }
            catch (Exception ex)
            {

                obj.LogError("PROCESS ALL COVER", ex.Message);

            }
            finally
            {
                obj = null;
            }

        }



                          
        /// <summary>
        /// New Version of the Process XML method.
        /// </summary>
        /// <param name="dt"></param>
        private void ProcessXML(DataTable dt)
        {
            try
            {
                XmlGeneratorModel generatorSettings = new XmlGeneratorModel();
                int recordCount = dt.Rows.Count;
                // Populate Settings.
                generatorSettings.LoggingAll = _settings.LoggingAll;
                generatorSettings.DynamicReportingPerson = _settings.ReportHeader_Dynamic;
                generatorSettings.EntityId = _settings.R_Entity_Id;
                generatorSettings.LoggingSecondLevel = _settings.LoggingSecondLevel;
                generatorSettings.SubmissionDate = DateTime.Now.ToString();

                string reportXML = XmlGenerator.GenerateXML(generatorSettings, dt);

                string xmlFilePath = SaveXML2(reportXML);
                
                string zipFilePath = ZipXMLFile(xmlFilePath);


                if (_settings.Allow_Automatic_XML_Upload)
                {
                    UploadXML(zipFilePath);
                }

                long recordID = ArchiveUploadFiles(xmlFilePath, zipFilePath, recordCount);

                if (_settings.Email_Notifications)
                {
                    //SendEmail(zipFilePath, recordCount, recordID,"CASH");
                }

            }
            catch(Exception ex)
            {                
                throw ex;
            }
        }

        private void ProcessXMLBulk(DataTable dt ,string date)
        {
            try
            {
                XmlGeneratorModel generatorSettings = new XmlGeneratorModel();
                int recordCount = dt.Rows.Count;
                // Populate Settings.
                generatorSettings.LoggingAll = _settings.LoggingAll;
                generatorSettings.DynamicReportingPerson = _settings.ReportHeader_Dynamic;
                generatorSettings.EntityId = _settings.R_Entity_Id;
                generatorSettings.LoggingSecondLevel = _settings.LoggingSecondLevel;
                generatorSettings.SubmissionDate = DateTime.Now.ToString();

                string reportXML = XmlGenerator.GenerateXML(generatorSettings, dt);

                string xmlFilePath = SaveXMLBUlk(reportXML, date); 

                string zipFilePath = ZipXMLFile(xmlFilePath);


                if (_settings.Allow_Automatic_XML_Upload)
                {
                    //UploadXML(zipFilePath);
                }

                long recordID = ArchiveUploadFiles(xmlFilePath, zipFilePath, recordCount);

                if (_settings.Email_Notifications)
                {
                    //SendEmail(zipFilePath, recordCount, recordID,"CASH");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private string sendTopTransactions()
        {
            string filename = string.Empty;
            try
            {
                DataSet dta = new DataSet();

                Datalayer obje = new Datalayer();

                dta = obje.GetTopTransactions();

                string InvoiceHTMLString = string.Empty;

                InvoiceHTMLString = TopTransactions.createHTMLString(dta);

                var varDateTimeTodayScrabed = System.Text.RegularExpressions.Regex.Replace(DateTime.Now.ToString("yyyy MM dd hh:mm:ss"), @":", "_");
                varDateTimeTodayScrabed = System.Text.RegularExpressions.Regex.Replace(varDateTimeTodayScrabed, @" ", "_");
                filename = "TopTransactions_"+ varDateTimeTodayScrabed;
                string pdfFilePath = GeneratePDF(InvoiceHTMLString, filename, "Top Transactions");
                var Archived_Folder = ConfigurationManager.AppSettings["TransactionPath"];
                SendEmailOfTopTransactions(Archived_Folder+ "Top_Transactions/" + filename+".pdf");


            }
            catch (Exception ex)
            {
                return "0";
            }
            return filename;
           

        }


        private void ProcessSTRXML(DataTable dt)
        {
            try
            {
                XmlGeneratorModel generatorSettings = new XmlGeneratorModel();
                int recordCount = dt.Rows.Count;
                // Populate Settings.
                generatorSettings.LoggingAll = _settings.LoggingAll;
                generatorSettings.DynamicReportingPerson = _settings.ReportHeader_Dynamic;
                generatorSettings.EntityId = _settings.R_Entity_Id;
                generatorSettings.LoggingSecondLevel = _settings.LoggingSecondLevel;
                generatorSettings.SubmissionDate = DateTime.Now.ToString();

                string reportXML = STRXMLgenerator.GenerateXML(generatorSettings, dt);

                string xmlFilePath = SaveXML(reportXML);

                string zipFilePath = ZipXMLFile(xmlFilePath);

              

                if (_settings.Allow_Automatic_XML_Upload)
                {
                  // UploadXML(zipFilePath);
                   
                }

                long recordID = ArchiveUploadFiles(xmlFilePath, zipFilePath, recordCount);

                if (_settings.Email_Notifications)
                {
                    SendEmail(zipFilePath, recordCount, recordID ,"STR");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public string GeneratePDF(string htmlString, string fileName, string InstTypeFolder)
        {


            if (!Directory.Exists(_OutgoingGeneratedPDFReport + "/" + InstTypeFolder.Replace(" ", "_")))
            {
                //If Directory (Folder) does not exists. Create it.
                Directory.CreateDirectory(_OutgoingGeneratedPDFReport + "/" + InstTypeFolder.Replace(" ", "_"));
            }

            string pdfPath = string.Format(@"{0}{1}.pdf", _OutgoingGeneratedPDFReport + "/" + InstTypeFolder.Replace(" ", "_") + "/", fileName);
            Datalayer dataLayer = new Datalayer();
            try
            {
                dataLayer.LogError(pdfPath + "  PDF generation started", "PDF generation process started - please wait this may take a long time");

                string htmlPath = string.Format(@"{0}{1}.html", _OutgoingGeneratedPDFReport, fileName);

                System.IO.File.WriteAllText(htmlPath, htmlString);
                var watch = System.Diagnostics.Stopwatch.StartNew();


                Process p = new Process
                {
                    StartInfo =
                    {
                        FileName = _pdfGeneratorPath + "wkhtmltopdf.exe",
                        Arguments = @"-O portrait --print-media-type " + htmlPath + " " + pdfPath,
                        UseShellExecute = false,
                        RedirectStandardOutput = false,
                        RedirectStandardError = false,
                        RedirectStandardInput = false,
                        CreateNoWindow = false,
                        //WorkingDirectory = @"C:\Program Files\wkhtmltopdf\bin"
                    }
                };

                try
                {
                    p.Start();
                }
                catch (Exception ex)
                {
                    //Log(ex.Message, "Fee Generated");
                }

                // I think it's timimg out here for shoo
                // Emmanuel Picked this up 
                // This should be put conditionally if records are more then increase the wait period
                p.WaitForExit(999999999);

                //dataLayer.LogStep("p WaitForExit", "Generate PDF finished waiting");

                //p.WaitForExit(); 

                int returnCode = p.ExitCode;
                p.Close();

                watch.Stop();
                var elapsedTime = watch.ElapsedMilliseconds;

                //dataLayer.LogStep("PDF Generator", "PDF successfully generated");

                if (returnCode != 0 && returnCode != 2)
                {
                    //throw new Exception();
                }

                File.Delete(htmlPath);

                p.Dispose();

                //dataLayer.LogStep("return pdfPath", "Generate PDF done");

                return pdfPath;
            }
            catch (Exception ex)
            {
                //Log(ex.Message, "PDF Creation");

                //throw;
            }

            return pdfPath;
        }




        //Militant - get all records from corebanking
        public void GetAllTransactions()
        {
            con = new SqlConnection(strCon);
            cmd = new SqlCommand();
            cmd.CommandText = "[spx_Fetch_All_Records_From_Core_Banking]";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.Parameters.Add("@Source", SqlDbType.VarChar).Value = "service";

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public void DoWork(object data)
        {

            while (true)
            {
                Datalayer obj = new Datalayer();
                try
                {
                    obj.LogError("BEFORE PROCESS ALL", "BEFORE PROCESS ALL");
                    ProcessAll();
                    //Militant's service 
                    ProcessSTR();
                    Thread.Sleep(Convert.ToInt32(_settings.Time));
                    obj.LogError("BEFORE PROCESS ALL", "BEFORE PROCESS ALL");
                    
                }
                catch(Exception ex)
                {
                    obj.LogError("error in Processall", ex.Message);

                }
                finally
                {
                    obj = null;

                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Saves XML file to the file systems, returning the file path.
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public string SaveXML(string xmlData)
        {
            try
            {
                Datalayer dataLayer = new Datalayer();
                XmlDocument xml_new = new XmlDocument();                
                if (_settings.LoggingAll)
                {
                    
                    dataLayer.LogStep("Step 5", "Inside SaveXML method");
                }

                xml_new.LoadXml(xmlData);

                var var_goAML_CASH_DEPOSIT_File_To_Be_Send = ConfigurationManager.AppSettings["goAML_CASH_DEPOSIT_File_To_Be_Send"];

                var varDateTimeToday = DateTime.Today;

                var varDateTimeTodayScrabed = System.Text.RegularExpressions.Regex.Replace(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), @":", "-");
                string fileName = string.Format("Report_{0}.xml", varDateTimeTodayScrabed, ".xml");
                string xmlFullPathName = Path.Combine(var_goAML_CASH_DEPOSIT_File_To_Be_Send, fileName);
                xml_new.Save(xmlFullPathName);                

                return xmlFullPathName;
            }
            catch (Exception ex)
            {                
                throw;
            }            
        }
        public string SaveXML2(string xmlData)
        {
            try
            {
                Datalayer dataLayer = new Datalayer();
                XmlDocument xml_new = new XmlDocument();
                if (_settings.LoggingAll)
                {

                    dataLayer.LogStep("Step 5", "Inside SaveXML method");
                }

                xml_new.LoadXml(xmlData);

                var var_goAML_CASH_DEPOSIT_File_To_Be_Send = ConfigurationManager.AppSettings["goAML_CASH_DEPOSIT_File_To_Be_Send"];
                var varDateTimeToday = DateTime.Today;

                var varDateTimeTodayScrabed = System.Text.RegularExpressions.Regex.Replace(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), @":", "-");


                //var varDateTimeTodayScrabed = System.Text.RegularExpressions.Regex.Replace(names, @":", "-");
                string fileName = string.Format("Report_{0}.xml", varDateTimeTodayScrabed, ".xml");
                string xmlFullPathName = Path.Combine(var_goAML_CASH_DEPOSIT_File_To_Be_Send, fileName);
                xml_new.Save(xmlFullPathName);

                return xmlFullPathName;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string SaveXMLBUlk(string xmlData,string Dt)
        {
            try
            {
                Datalayer dataLayer = new Datalayer();
                XmlDocument xml_new = new XmlDocument();
                if (_settings.LoggingAll)
                {

                    dataLayer.LogStep("Step 5", "Inside SaveXML method");
                }

                xml_new.LoadXml(xmlData);

                var var_goAML_CASH_DEPOSIT_File_To_Be_Send = ConfigurationManager.AppSettings["goAML_CASH_DEPOSIT_File_To_Be_Send"];
                var varDateTimeToday =Dt;

                var varDateTimeTodayScrabed = System.Text.RegularExpressions.Regex.Replace(Dt, @":", "-");


                //var varDateTimeTodayScrabed = System.Text.RegularExpressions.Regex.Replace(names, @":", "-");
                string fileName = string.Format("Report_{0}.xml", varDateTimeTodayScrabed, ".xml");
                string xmlFullPathName = Path.Combine(var_goAML_CASH_DEPOSIT_File_To_Be_Send, fileName);
                xml_new.Save(xmlFullPathName);

                return xmlFullPathName;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Automatically upload the XML report using the IE WebDriver.
        /// </summary>
        /// <param name="zipFilePath">Path to Zip File which will be uploaded.</param>
        public void UploadXML(string zipFilePath)
        {
            Datalayer obj = new Datalayer();
            // Try up to five times to do the upload.
            for (int i = 0; i < 5; i++)
            {
                try
                {

                    var webDriverPath = ConfigurationManager.AppSettings["WebDriverPath"];

                    var driverService = InternetExplorerDriverService.CreateDefaultService(webDriverPath);
                    driverService.HideCommandPromptWindow = true;

                    InternetExplorerOptions options = new InternetExplorerOptions();
                    options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                    options.IgnoreZoomLevel = true;
                    options.UnexpectedAlertBehavior = InternetExplorerUnexpectedAlertBehavior.Dismiss;

                    string logOnPage = String.Format("{0}/Account/LogOn", _settings.goAML_URL); //"https://www.fia.org.bw/goamltrn/Account/LogOn"; //
                    string uploadPage = String.Format("{0}/XMLUpload", _settings.goAML_URL); //"https://www.fia.org.bw/goamltrn/XMLUpload/"; //

                    obj.LogStep("UploadXML", "Opening the FIA website.");



                    using (IWebDriver driver = new InternetExplorerDriver(driverService, options, TimeSpan.FromSeconds(10)))
                    {                     
                        
                        // Navigate to the login page.

                        //driver.Navigate().GoToUrl("Url_to_be_loaded");

                        //IWebDriver wait = new IWebDriver(driver,TimeSpan.FromSeconds(10));
                        //wait.Until((d) =>
                        //{
                        //    return
                        //        driver.Url.Contains("part_of_the_Url_that will_definitely_apear_in_Url");
                        //});

                        //var elementToFind = wait.Until((d) =>
                        //{
                        //    return
                        //        driver.FindElement(By.CssSelector("Username_I"));
                        //});
                        

                        driver.Navigate().GoToUrl(logOnPage);

                        
                        //Thread.Sleep(2000);

                        // Get references to the HTML elements that we need.
                        //driver.Document.GetElementById("subject").SetAttribute("value", subject.text); 


                        //IWebElement varusername = driver.FindElement(By.CssSelector("Username_I"));

                        IWebElement username = driver.FindElement(By.Id("Username_I"));
                        IWebElement password = driver.FindElement(By.Id("Password_I"));
                        IWebElement button = driver.FindElement(By.Id("btnLogin_CD"));

                        obj.LogStep("UploadXML", "Automatic login onto FIA website successfully");

                        // Enter in the Users login credentials and press the login button.
                        username.SendKeys(_settings.goAML_User);
                        
                        //Thread.Sleep(2000);
                        password.SendKeys(_settings.goAML_Password);
                        //Thread.Sleep(2000);
                        button.Click();

                        // Navigate to the XML Upload page.
                        driver.Navigate().GoToUrl(uploadPage);

                       
                        // Click the "Browse" button.
                        IWebElement input = driver.FindElement(By.Id("ucReportFileSelection_Browse0"));
                        input.Click();

                        // Enter in the Path to the XML file and press enter. Sleep for 2 seconds.
                        System.Windows.Forms.SendKeys.SendWait(zipFilePath);
                        System.Windows.Forms.SendKeys.SendWait("{ENTER}");
                        

                        // Once the upload has been complet
                        driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 120));
                        IWebElement upload = driver.FindElement(By.Id("btnUpload"));
                        upload.Click();

                        // Once the upload has been completed, sleep for 5 seconds and then close the WebDriver.
                        Thread.Sleep(2000);
                        driver.Close();
                        driver.Quit();
                        obj.LogStep("UploadXML", "Automatic Upload onto FIA website of the XML has been completed.");

                        break;
                        
                    }
                }
                catch (WebDriverException wde)
                {
                    obj.LogError("Error on Automatic XML Upload", wde.Message);
                    Thread.Sleep(2000);
                    continue;
                }                
            }            
        }

        /// <summary>
        /// Zips an XML file that has been passed in, returns path to the zipped file.
        /// </summary>
        /// <param name="filePath">Name of file that will be uploaded.</param>
        /// <returns></returns>
        public string ZipXMLFile(string filePath)
        {
            // implement proper exception handling for zipping.
            try
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string zippedPath = ConfigurationManager.AppSettings["Zipped_Folder"];
                string zippedFileName = String.Format("{0}.zip", fileName);
                string zippedFullPath = String.Format(@"{0}{1}", zippedPath, zippedFileName);

                using (var zipFile = ZipFile.Open(zippedFullPath, ZipArchiveMode.Create))
                {
                    zipFile.CreateEntryFromFile(filePath, Path.GetFileName(filePath));
                }

                return zippedFullPath;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// <summary>
        /// Send out an email notifiying users about the uploaded file.
        /// </summary>
        public void SendEmail(string zipFilePath, int recordCount, long recordID,string source)
        {
            //int LoggingAll = Convert.ToInt32(ConfigurationManager.AppSettings.Get("LoggingAll"));
            Datalayer obj = new Datalayer();

            MailMessage mail = new MailMessage();

            var ExChangeServerName = _settings.ExChangeServerName;

            SmtpClient SmtpServer = new SmtpClient(ExChangeServerName);

            var Archived_Folder = ConfigurationManager.AppSettings["Archived_Folder"];
            var TempForAttachements_Folder = ConfigurationManager.AppSettings["TempForAttachements_Folder"];

            var strSender_From = _settings.Sender_From;
            var Receiver_To = _settings.Receiver_To;
            var Subject = string.Empty;
            #region Email Settings
            if (source == "STR")
            {
                Subject = ConfigurationManager.AppSettings["Subject2"];
            }
            else
            {
                Subject = ConfigurationManager.AppSettings["Subject"];
            }
            
            var BodyLineAutomatedMail = ConfigurationManager.AppSettings["BodyLineAutomatedMail"];
            var BodyLineOne = ConfigurationManager.AppSettings["BodyLineOne"];
            var BodyLineTwo = ConfigurationManager.AppSettings["BodyLineTwo"];
            var BodyLineThree = ConfigurationManager.AppSettings["BodyLineThree"];
            var BodyLineFour = ConfigurationManager.AppSettings["BodyLineFour"];
            var BodyLineFive = ConfigurationManager.AppSettings["BodyLineFive"];
            var BodyLineSix = ConfigurationManager.AppSettings["BodyLineSix"];
            var BodyLineSeven = ConfigurationManager.AppSettings["BodyLineSeven"];
            var BodyLineEight = ConfigurationManager.AppSettings["BodyLineEight"];
            var BodyLineNine = ConfigurationManager.AppSettings["BodyLineNine"];
            var BodyLineNineTwo = ConfigurationManager.AppSettings["BodyLineNineTwo"];
            var BodyLineTen = ConfigurationManager.AppSettings["BodyLineTen"];
            var BodyLineElven = ConfigurationManager.AppSettings["BodyLineElven"];
            var BodyLineTwelve = ConfigurationManager.AppSettings["BodyLineTwelve"];
            var CellNumberToSendTo = ConfigurationManager.AppSettings["CellNumberToSendTo"];
            var SMSMessage = ConfigurationManager.AppSettings["SMSMessage"];
            var SMSMessageSalutation = ConfigurationManager.AppSettings["SMSMessageSalutation"]; 
            #endregion


            if (_settings.LoggingAll)
            {
                obj.LogStep("Step 8", "Inside ProcessXML Email start - var_goAML_CASH_DEPOSIT_File_To_Be_Send");
            }

            if (File.Exists(zipFilePath))
            {
                mail.From = new MailAddress(strSender_From);

                DataTable dataTable = obj.LoadEmailRecipients();
                String primaryEmails = String.Empty;
                
                foreach (DataRow row in dataTable.Rows)
                {
                    if (Convert.ToBoolean(row["PrimaryRecipient"]))
                    {
                        mail.To.Add(row["EmailAddress"].ToString());
                    }
                    else
                    {
                        mail.CC.Add(row["EmailAddress"].ToString());
                    }
                }

                SmtpServer.Port = 587;
         
                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(zipFilePath);
                mail.Attachments.Add(attachment);


                var UsernameToPushEmail = _settings.UsernameToPushEmail;
                var PasswordForEmailAccount = _settings.PasswordForEmailAccount;

                SmtpServer.Credentials = new System.Net.NetworkCredential(UsernameToPushEmail, PasswordForEmailAccount);

                if (_settings.EnableSsl_SMTP)
                {
                    SmtpServer.EnableSsl = true;
                }
                else
                {
                    SmtpServer.EnableSsl = false;
                }

                string emailBody = BodyLineAutomatedMail + "\r" + "\r" + BodyLineOne + "\r" + "\r" + "Total of " + recordCount + " " + 
                            BodyLineTwo + "\r" + "\r" + BodyLineThree + "\r";
                string footer = GenerateEmailFooter();
                mail.Subject = BodyLineAutomatedMail + " with " + " " + recordCount + " " + Subject;                
                mail.Body = String.Format("{0}\r\n{1}", emailBody, footer);

                if (_settings.LoggingAll)
                {
                    obj.LogStep("Step 9", "Before Sending email");
                }

                SmtpServer.Send(mail);

                if (_settings.LoggingAll)
                {
                    obj.LogStep("Step 10", "After Sending email");
                }

                obj.UpdateXmlFileEntry(recordID);
            }


            // TODO: Clean up this code.
            // TODO: Add functionality from my service.
            //if (Directory.Exists(var_goAML_CASH_DEPOSIT_File_To_Be_Send))
            //{
            //    mail.From = new MailAddress(strSender_From);
            //    mail.To.Add(Receiver_To);

            //    SqlConnection con = new SqlConnection();
            //    con.ConnectionString = ConfigurationManager.AppSettings["StrConn"];
            //    con.Open();

            //    SqlCommand cmd = new SqlCommand();
            //    cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //    cmd.Connection = con;

            //    cmd.CommandText = "subGet_Emails";

            //    string strEmails = "";
            //    SqlDataReader sqlReader = cmd.ExecuteReader();
            //    if (sqlReader != null && sqlReader.HasRows)
            //    {
            //        while (sqlReader.Read())
            //        {
            //            if (sqlReader["EmailAddress"].ToString() != "EmailAddress")
            //                strEmails = strEmails + "," + sqlReader["EmailAddress"].ToString();

            //        }
            //    }
            //    mail.CC.Add(strEmails);
            //    con.Close();

            //    string pathFile = var_goAML_CASH_DEPOSIT_File_To_Be_Send;
            //    string pathToArchiveFile = Archived_Folder;
            //    string pathTempForAttachements = TempForAttachements_Folder;

            //    var pattern1 = ConfigurationManager.AppSettings["pattern1"];

            //    string[] ListfilesToBeCopied = System.IO.Directory.GetFiles(var_goAML_CASH_DEPOSIT_File_To_Be_Send);

            //    if (ListfilesToBeCopied.Length != 0)
            //    {
            //        pathTempForAttachements = pathTempForAttachements + String.Format("{0:d-MMMM-yyyy}", varDateTimeToday);

            //        pathTempForAttachements = pathTempForAttachements + " " + DateTime.Now.ToString("HH mm ss tt");

            //        System.IO.Directory.CreateDirectory(pathTempForAttachements);
            //    }

            //    ListfilesToBeCopied = null;

            //    foreach (var file in new DirectoryInfo(pathFile).GetFiles(pattern1))
            //    {
            //        if (File.Exists(pathTempForAttachements + "\\" + file.Name))
            //        {
            //            File.Delete(pathTempForAttachements + "\\" + file.Name);
            //        }

            //        System.IO.File.Copy(pathFile + "\\" + file.Name, pathTempForAttachements + "\\" + file.Name);

            //        System.Net.Mail.Attachment attachment;
            //        attachment = new System.Net.Mail.Attachment(pathTempForAttachements + "\\" + file.Name);
            //        mail.Attachments.Add(attachment);

            //    }

            //    // Move the Files for permanent Archive storage folder to audit trail purposes
            //    var pattern2 = ConfigurationManager.AppSettings["pattern2"];
            //    foreach (var file in new DirectoryInfo(pathFile).GetFiles(pattern2))
            //    {
            //        if (File.Exists(pathToArchiveFile + "\\" + file.Name))
            //        {
            //            //File.Delete(pathToArchiveFile + "\\" + file.Name);
            //            System.IO.File.Move(pathFile + "\\" + file.Name, pathToArchiveFile + "\\" + "Duplicate_" + file.Name);
            //            //obj.LogStep("Step 6 Duplicate file ", pathFile + "\\" + file.Name);
            //            return;
            //        }
            //        else
            //        {
            //            // Come back
            //            System.IO.File.Move(pathFile + "\\" + file.Name, pathToArchiveFile + "\\" + file.Name);
            //        }
            //    }

            //    // Attach the files to an email and leave them in that folder
            //    // The next Run of this Windows Service Will first delete the Attachment folder before it copies newer files
            //    if (Directory.Exists(pathTempForAttachements))
            //    {

            //        string[] Listfiles = System.IO.Directory.GetFiles(pathTempForAttachements);
            //        if (Listfiles.Length != 0)
            //        {


            //            SmtpServer.Port = 587;
            //            //SmtpServer.Port = 465; // Works with gmail in printer
            //            // SmtpServer.Port = 25;

            //            var UsernameToPushEmail = ConfigurationManager.AppSettings["UsernameToPushEmail"];
            //            var PasswordForEmailAccount = ConfigurationManager.AppSettings["PasswordForEmailAccount"];

            //            SmtpServer.Credentials = new System.Net.NetworkCredential(UsernameToPushEmail, PasswordForEmailAccount);


            //            int varEnableSsl_SMTP = Convert.ToInt32(ConfigurationManager.AppSettings.Get("EnableSsl_SMTP"));

            //            //SmtpServer.EnableSsl = false; // DBFfinetch cloud needs this
            //            // SmtpServer.EnableSsl = true; // smtp.office365.com

            //            if (varEnableSsl_SMTP == 1)
            //            {
            //                SmtpServer.EnableSsl = true;
            //            }
            //            else
            //            {
            //                SmtpServer.EnableSsl = false;
            //            }


            //            mail.Subject = BodyLineAutomatedMail + " with " + " " + i + " " + Subject;
            //            mail.Body = BodyLineAutomatedMail + "\r" + "\r" + BodyLineOne + "\r" + "\r" + "Total of  " + i + "  " + BodyLineTwo + "\r" + "\r" + BodyLineThree + "\r" + BodyLineFour + "\r" + BodyLineFive + "\r" + BodyLineSix + "\r" + BodyLineSeven + "\r" + BodyLineEight + "\r" + BodyLineNine + "\r" + BodyLineNineTwo + "\r" + BodyLineTen + "\r" + BodyLineElven + "\r" + BodyLineTwelve;

            //            if (LoggingAll == 1)
            //            {
            //                obj.LogStep("Step 9", "Before Sending email");
            //            }



            //            SmtpServer.Send(mail);



            //            if (LoggingAll == 1)
            //            {
            //                obj.LogStep("Step 10", "After Sending email");
            //            }

            //            if (LoggingSecondLevel == 1)
            //            {
            //                obj.LogStep("LoggingSecondLevel 1", "Service Finished ProcessXML");
            //            }


            //        }

            //    }


            //}
        }




        public void SendEmailOfTopTransactions(string zipFilePath)
        {
            //int LoggingAll = Convert.ToInt32(ConfigurationManager.AppSettings.Get("LoggingAll"));
            Datalayer obj = new Datalayer();

            MailMessage mail = new MailMessage();

            var ExChangeServerName = _settings.ExChangeServerName;

            SmtpClient SmtpServer = new SmtpClient(ExChangeServerName);

            var Archived_Folder = ConfigurationManager.AppSettings["TransactionPath"];
            var TempForAttachements_Folder = ConfigurationManager.AppSettings["TransactionPath"];

            var strSender_From = _settings.Sender_From;
            var Receiver_To = _settings.Receiver_To;
            var Subject = string.Empty;
            #region Email Settings

            Subject = "Top Deposits and Withdrawals for yesterday";
           

            var BodyLineAutomatedMail = ConfigurationManager.AppSettings["BodyLineAutomatedMail"];
            var BodyLineOne = ConfigurationManager.AppSettings["BodyLineOne"];
            var BodyLineTwo = ConfigurationManager.AppSettings["BodyLineTwo"];
            var BodyLineThree = ConfigurationManager.AppSettings["BodyLineThree"];
            var BodyLineFour = ConfigurationManager.AppSettings["BodyLineFour"];
            var BodyLineFive = ConfigurationManager.AppSettings["BodyLineFive"];
            var BodyLineSix = ConfigurationManager.AppSettings["BodyLineSix"];
            var BodyLineSeven = ConfigurationManager.AppSettings["BodyLineSeven"];
            var BodyLineEight = ConfigurationManager.AppSettings["BodyLineEight"];
            var BodyLineNine = ConfigurationManager.AppSettings["BodyLineNine"];
            var BodyLineNineTwo = ConfigurationManager.AppSettings["BodyLineNineTwo"];
            var BodyLineTen = ConfigurationManager.AppSettings["BodyLineTen"];
            var BodyLineElven = ConfigurationManager.AppSettings["BodyLineElven"];
            var BodyLineTwelve = ConfigurationManager.AppSettings["BodyLineTwelve"];
            var CellNumberToSendTo = ConfigurationManager.AppSettings["CellNumberToSendTo"];
            var SMSMessage = ConfigurationManager.AppSettings["SMSMessage"];
            var SMSMessageSalutation = ConfigurationManager.AppSettings["SMSMessageSalutation"];
            #endregion


            if (_settings.LoggingAll)
            {
                obj.LogStep("Step 8", "Inside ProcessXML Email start - var_goAML_CASH_DEPOSIT_File_To_Be_Send");
            }

          
                mail.From = new MailAddress(strSender_From);

                DataTable dataTable = obj.LoadEmailRecipients();
                String primaryEmails = String.Empty;

                foreach (DataRow row in dataTable.Rows)
                {
                    if (Convert.ToBoolean(row["PrimaryRecipient"]))
                    {
                        mail.To.Add(row["EmailAddress"].ToString());
                    }
                    else
                    {
                        mail.CC.Add(row["EmailAddress"].ToString());
                    }
                }

                SmtpServer.Port = 587;

                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(zipFilePath);
                mail.Attachments.Add(attachment);


                var UsernameToPushEmail = _settings.UsernameToPushEmail;
                var PasswordForEmailAccount = _settings.PasswordForEmailAccount;

                SmtpServer.Credentials = new System.Net.NetworkCredential(UsernameToPushEmail, PasswordForEmailAccount);

                if (_settings.EnableSsl_SMTP)
                {
                    SmtpServer.EnableSsl = true;
                }
                else
                {
                    SmtpServer.EnableSsl = false;
                }

                string emailBody = "Top transactions for t" + "\r" + "\r" + BodyLineOne + "\r" + "\r" + "Total of " +
                            BodyLineTwo + "\r" + "\r" + BodyLineThree + "\r";
                string footer = GenerateEmailFooter();
                mail.Subject = BodyLineAutomatedMail + " with " + " "  + " " + Subject;
                mail.Body = String.Format("{0}\r\n{1}", emailBody, footer);

                if (_settings.LoggingAll)
                {
                    obj.LogStep("Step 9", "Before Sending email");
                }

                SmtpServer.Send(mail);

                if (_settings.LoggingAll)
                {
                    obj.LogStep("Step 10", "After Sending email");
                }

             

        }

        /// <summary>
        /// 
        /// </summary>
        public void SendEmailOfNoRecords()
        {
            Datalayer obj = new Datalayer();
            MailMessage mail = new MailMessage();
            string exchangeServerName = _settings.ExChangeServerName;
            SmtpClient SmtpServer = new SmtpClient(exchangeServerName);

            var strSender_From = _settings.Sender_From;
            var Receiver_To = _settings.Receiver_To;

            if (_settings.LoggingAll)
            {
                obj.LogStep("Step 8", "Inside ProcessXML Email start - var_goAML_CASH_DEPOSIT_File_To_Be_Send");
            }

            mail.From = new MailAddress(strSender_From);

            DataTable dataTable = obj.LoadEmailRecipients();

            foreach (DataRow row in dataTable.Rows)
            {
                if (Convert.ToBoolean(row["PrimaryRecipient"]))
                {
                    mail.To.Add(row["EmailAddress"].ToString());
                }
                else
                {
                    mail.CC.Add(row["EmailAddress"].ToString());
                }
            }

            SmtpServer.Port = 587;

            var UsernameToPushEmail = _settings.UsernameToPushEmail;
            var PasswordForEmailAccount = _settings.PasswordForEmailAccount;

            SmtpServer.Credentials = new System.Net.NetworkCredential(UsernameToPushEmail, PasswordForEmailAccount);

            if (_settings.EnableSsl_SMTP)
            {
                SmtpServer.EnableSsl = true;
            }
            else
            {
                SmtpServer.EnableSsl = false;
            }

            string footer = GenerateEmailFooter();
            string emailBody = obj.LoadEmailSetting("Email_No_Records_Message");
            mail.Subject = "No records today";
            mail.Body = String.Format("{0}\r\n{1}", emailBody, footer);


            //mail.Subject = BodyLineAutomatedMail + " with " + " " + recordCount + " " + Subject;
            //mail.Body = BodyLineAutomatedMail + "\r" + "\r" + BodyLineOne + "\r" + "\r" + "Total of  " + recordCount + "  " + BodyLineTwo + "\r" + "\r" + BodyLineThree + "\r" + BodyLineFour + "\r" + BodyLineFive + "\r" + BodyLineSix + "\r" + BodyLineSeven + "\r" + BodyLineEight + "\r" + BodyLineNine + "\r" + BodyLineNineTwo + "\r" + BodyLineTen + "\r" + BodyLineElven + "\r" + BodyLineTwelve;

            if (_settings.LoggingAll)
            {
                obj.LogStep("Step 9", "Before Sending email");
            }

            SmtpServer.Send(mail);

        }
        public void SendEmailOfNoRecordsSTR()
        {
            Datalayer obj = new Datalayer();
            MailMessage mail = new MailMessage();
            string exchangeServerName = _settings.ExChangeServerName;
            SmtpClient SmtpServer = new SmtpClient(exchangeServerName);

            var strSender_From = _settings.Sender_From;
            var Receiver_To = _settings.Receiver_To;

            if (_settings.LoggingAll)
            {
                obj.LogStep("Step 8", "Inside ProcessXML Email start - var_goAML_STR_DEPOSIT_File_To_Be_Send");
            }

            mail.From = new MailAddress(strSender_From);

            DataTable dataTable = obj.LoadEmailRecipients();

            foreach (DataRow row in dataTable.Rows)
            {
                if (Convert.ToBoolean(row["PrimaryRecipient"]))
                {
                    mail.To.Add(row["EmailAddress"].ToString());
                }
                else
                {
                    mail.CC.Add(row["EmailAddress"].ToString());
                }
            }

            SmtpServer.Port = 587;

            var UsernameToPushEmail = _settings.UsernameToPushEmail;
            var PasswordForEmailAccount = _settings.PasswordForEmailAccount;

            SmtpServer.Credentials = new System.Net.NetworkCredential(UsernameToPushEmail, PasswordForEmailAccount);

            if (_settings.EnableSsl_SMTP)
            {
                SmtpServer.EnableSsl = true;
            }
            else
            {
                SmtpServer.EnableSsl = false;
            }

            string footer = GenerateEmailFooter();
            string emailBody = obj.LoadEmailSetting("Email_No_Records_Message");
            mail.Subject = "No Suspicious Transaction Report(STR) records today";
            mail.Body = String.Format("{0}\r\n{1}", emailBody, footer);


            //mail.Subject = BodyLineAutomatedMail + " with " + " " + recordCount + " " + Subject;
            //mail.Body = BodyLineAutomatedMail + "\r" + "\r" + BodyLineOne + "\r" + "\r" + "Total of  " + recordCount + "  " + BodyLineTwo + "\r" + "\r" + BodyLineThree + "\r" + BodyLineFour + "\r" + BodyLineFive + "\r" + BodyLineSix + "\r" + BodyLineSeven + "\r" + BodyLineEight + "\r" + BodyLineNine + "\r" + BodyLineNineTwo + "\r" + BodyLineTen + "\r" + BodyLineElven + "\r" + BodyLineTwelve;

            if (_settings.LoggingAll)
            {
                obj.LogStep("Step 9", "Before Sending email");
            }

            SmtpServer.Send(mail);

        }
        /// <summary>
        /// Generates Email footer with contact details of the support persons from the database.
        /// </summary>
        /// <returns></returns>
        public string GenerateEmailFooter()
        {
            Datalayer obj = new Datalayer();
            DataTable emailParameters = obj.LoadEmailSettings();
            
            List<ParameterModel> paramList = new List<ParameterModel>();

            paramList = (from DataRow dr in emailParameters.Rows
                         select new ParameterModel()
                         {
                             ID = Convert.ToInt32(dr["ID"]),
                             Name = Convert.ToString(dr["Name"]),
                             Value = Convert.ToString(dr["Value"]),
                             Type = Convert.ToString(dr["Type"]),
                             FriendlyName = Convert.ToString(dr["FriendlyName"]),
                             Description = Convert.ToString(dr["Description"])
                         }).ToList();

            #region Map DataTable rows to model.
            EmailParameterModel emailModel = new EmailParameterModel();
            emailModel.EmailSignatureName1 = paramList.Where(x => x.Name == "Email_Signature_Name_1").FirstOrDefault().Value;
            emailModel.EmailSignatureEmail1 = paramList.Where(x => x.Name == "Email_Signature_Email_1").FirstOrDefault().Value;
            emailModel.EmailSignaturePhoneNumber1 = paramList.Where(x => x.Name == "Email_Signature_PhoneNumber_1").FirstOrDefault().Value;

            emailModel.EmailSignatureName2 = paramList.Where(x => x.Name == "Email_Signature_Name_2").FirstOrDefault().Value;
            emailModel.EmailSignatureEmail2 = paramList.Where(x => x.Name == "Email_Signature_Email_2").FirstOrDefault().Value;
            emailModel.EmailSignaturePhoneNumber2 = paramList.Where(x => x.Name == "Email_Signature_PhoneNumber_2").FirstOrDefault().Value;
            #endregion

            #region Email Settings
            // TODO: Clean up web config.
            var BodyLineFour = ConfigurationManager.AppSettings["BodyLineFour"];
            var BodyLineFive = ConfigurationManager.AppSettings["BodyLineFive"];
            var BodyLineSix = ConfigurationManager.AppSettings["BodyLineSix"];
            var BodyLineSeven = ConfigurationManager.AppSettings["BodyLineSeven"];
            var BodyLineEight = ConfigurationManager.AppSettings["BodyLineEight"];
            var BodyLineNine = ConfigurationManager.AppSettings["BodyLineNine"];
            var BodyLineNineTwo = ConfigurationManager.AppSettings["BodyLineNineTwo"];
            var BodyLineTen = ConfigurationManager.AppSettings["BodyLineTen"];
            var BodyLineElven = ConfigurationManager.AppSettings["BodyLineElven"];
            var BodyLineTwelve = ConfigurationManager.AppSettings["BodyLineTwelve"];
            #endregion

            string dbfFooter = String.Format("{0}\r\n{1}\r\n{2}\r\n{3}\r\n{4}\r\n{5}\r\n{6}\r\n{7}\r\n", 
                                                BodyLineFour, 
                                                BodyLineFive, 
                                                BodyLineSix, 
                                                BodyLineSeven, 
                                                BodyLineEight, 
                                                BodyLineNine, 
                                                BodyLineNineTwo, 
                                                BodyLineTen, 
                                                BodyLineElven, 
                                                BodyLineTwelve);

            StringBuilder emailFooter = new StringBuilder();
            emailFooter.AppendLine(emailModel.EmailSignatureName1);
            emailFooter.AppendFormat("Email: {0}\n", emailModel.EmailSignatureEmail1);
            emailFooter.AppendFormat("Phone: {0}\n\n", emailModel.EmailSignaturePhoneNumber1);

            emailFooter.AppendLine(emailModel.EmailSignatureName2);
            emailFooter.AppendFormat("Email: {0}\n", emailModel.EmailSignatureEmail2);
            emailFooter.AppendFormat("Phone: {0}\n", emailModel.EmailSignaturePhoneNumber2);
            
            return String.Format("{0}\r\n{1}", emailFooter.ToString(), dbfFooter); 
        }

        public Int64 ArchiveUploadFiles(string xmlFilePath, string zipFilePath, int recordCount)
        {
            try
            {
                Datalayer dataLayer = new Datalayer();
                string archiveFolder = ConfigurationManager.AppSettings.Get("Archived_Folder");

                var xmlFileInfo = new FileInfo(xmlFilePath);
                var zipFileInfo = new FileInfo(zipFilePath);
                string xmlArchive = Path.Combine(archiveFolder, xmlFileInfo.FullName);
                string zipArchive = Path.Combine(archiveFolder, zipFileInfo.FullName);
                File.Move(xmlFilePath, xmlArchive);
                File.Move(zipFilePath, zipArchive);
                Int64 recordID = dataLayer.InsertXmlFileEntry(xmlArchive, zipArchive, recordCount, dataLayer.SizeSuffix(xmlFileInfo.Length));

                return recordID;
            }
            catch (Exception ex)
            {                
                // TODO: implement this error handling.
                throw ex;
            }
        }
    }

}




