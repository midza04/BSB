using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFfintech_Interface_Portal
{
    public class SettingsModel
    {
        public string ID_Number { get; set; }
        public string Maximum_Amount { get; set; }
        public string Effective_Date { get; set; }
        public string R_Entity_Id { get; set; }
        public string Time { get; set; }
        public string Timer { get; set; }
        public string Number { get; set; }
        public string ExChangeServerName { get; set; }
        public string UsernameToPushEmail { get; set; }
        public string PasswordForEmailAccount { get; set; }
        public string Sender_From { get; set; }
        public string Receiver_To { get; set; }
        public string DefaultActiveDirectoryServer { get; set; }
        public string Domain { get; set; }
        public string goAML_User { get; set; }
        public string goAML_Password { get; set; }
        public string goAML_URL { get; set; }
        public string Server_Name { get; set; }

        public bool LoggingAll { get; set; }
        public bool LoggingSecondLevel { get; set; }
        public bool LoggingThirdLevel { get; set; }
        public bool ReportHeader_Dynamic { get; set; }
        public bool EnableSsl_SMTP { get; set; }
        public bool Allow_Automatic_XML_Upload { get; set; }
        public bool Email_Notifications { get; set; }
        
    }
}
