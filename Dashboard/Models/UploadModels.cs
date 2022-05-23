using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard.Models
{
    public class UploadModel
    {
        public int ID { get; set; }
        public string FileNameXML { get; set; }
        public int RecordCount { get; set; }
        public string FileSizeMB { get; set; }
        public int BatchNumber { get; set; }
        public string DateCreated { get; set; }
        public DateTime DateEmailSend { get; set; }
        public bool SendYesNo { get; set; }
    }
}