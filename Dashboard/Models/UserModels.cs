using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard.Models
{
    public class UserViewModel
    {
        public List<UserModel> Users { get; set; }
    }

    public class UserModel
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public bool IsEnabled { get; set; }
    }
}