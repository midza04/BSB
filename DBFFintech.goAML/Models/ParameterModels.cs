using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBFFintech.goAML
{
    public class ParameterViewModel
    {
        public List<ParameterModel> Parameters { get; set; }
    }

    public class ParameterModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public string FriendlyName { get; set; }
        public string Description { get; set; }
    }

    public class EmailParameterModel
    {
        public string EmailSignatureName1 { get; set; }
        public string EmailSignatureEmail1 { get; set; }
        public string EmailSignaturePhoneNumber1 { get; set; }
        public string EmailSignatureName2 { get; set; }
        public string EmailSignatureEmail2 { get; set; }
        public string EmailSignaturePhoneNumber2 { get; set; }
        public string EmailBody{ get; set; }
    }
}