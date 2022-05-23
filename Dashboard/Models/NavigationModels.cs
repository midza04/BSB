using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard.Models
{
    public class NavigationViewModel
    {
        public string UserName {get; set;}
        public List<MenuItem> MenuItems { get; set; }
    }
}