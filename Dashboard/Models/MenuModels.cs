using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard.Models
{
    public class MenuItem
    {
        public string Text { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool Selected { get; set; }
        public List<MenuItem> SubMenu { get; set; }
    }
}