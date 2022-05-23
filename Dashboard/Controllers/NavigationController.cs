using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Dashboard.Controllers
{
    public class NavigationController : Controller
    {
        //
        // GET: /Navigation/
        [ChildActionOnly]
        public ActionResult MainMenuPartial()
        {
            NavigationViewModel model = new NavigationViewModel();

            bool authenticated = System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            string userName = string.Empty;
            
            if (authenticated)
            {
                userName = System.Web.HttpContext.Current.User.Identity.Name;                
            }

            List<MenuItem> testing = new List<MenuItem>();

            MenuItem settings = new MenuItem { Text = "Settings Test", Action = "Index", Controller = "Settings", Selected = false };

            testing.Add(settings);

            var items = new List<MenuItem>
            {
                new MenuItem{ Text = "Dashboard", Action = "Index", Controller = "Home", Selected=false },
                //new MenuItem{ Text = "Reports", Action = "Index", Controller = "Reports", Selected = false },
                new MenuItem{ Text = "Uploads", Action = "Index", Controller = "Uploads", Selected = false},
                new MenuItem{ Text = "Logger", Action = "Index", Controller = "Logger", Selected = false},
                new MenuItem{ Text = "Users", Action = "Index", Controller = "Users", Selected = false},                
                new MenuItem{ Text = "Email Recipients", Action = "Index", Controller = "EmailRecipients", Selected = false},
                new MenuItem{ Text = "Email Settings", Action = "Emails", Controller = "Settings", Selected = false},
                new MenuItem{ Text = "Service Settings", Action = "Index", Controller = "Settings", Selected = false, SubMenu = testing}                                                         
            };

            string action = ControllerContext.ParentActionViewContext.RouteData.Values["action"].ToString();
            string controller = ControllerContext.ParentActionViewContext.RouteData.Values["controller"].ToString();

            foreach (var item in items)
            {

                if (item.Controller == controller && item.Action == action)
                {
                    item.Selected = true;
                }
            }

            model.MenuItems = items;
            model.UserName = userName;

            return PartialView(model);
        }

    }
}
