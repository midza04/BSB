using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard.Models
{
    public class EmailRecipientViewModel
    {
        public List<EmailRecipientModel> Emails { get; set; }
    }

    public class EmailRecipientModel
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public bool PrimaryRecipient { get; set; }
    }
}