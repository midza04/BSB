using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFFintech.goAML
{
    public class XmlGeneratorModel
    {
        public string EntityId { get; set; }
        public bool DynamicReportingPerson { get; set; }
        public string SubmissionDate { get; set; }
        public bool LoggingAll { get; set; }
        public bool LoggingSecondLevel { get; set; }
    }
}
