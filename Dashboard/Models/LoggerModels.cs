using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard.Models
{
    public class LoggerModel
    {
        public int ID { get; set; }
        public string StepDescription { get; set; }
        public string LoggerDescription { get; set; }
        public string DateLogged { get; set; }
    }
}