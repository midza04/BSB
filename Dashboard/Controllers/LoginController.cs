using Dashboard.Models;
using Dashboard.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UserAuthentication;

namespace Dashboard.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            
            if (ModelState.IsValid)
            {

                bool authenticated = AuthenticateUser(model.UserName, model.Password);

                if (authenticated)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = "Login credentials are incorrect";
                    return View();
                }

                //if (login.MESSAGE == "User has Permission")
                //{
                //    return RedirectToAction("Home");
                //}
            }
            //ViewBag["Message"] = "Login credentials are incorrect";
            return View();            
        }

        private bool AuthenticateUser(string userName, string password)
        {
            // Path to you LDAP directory server.
            // Contact your network administrator to obtain a valid path.
            ParameterRepository paramRepo = new ParameterRepository();
            LoggerRepository loggerRepo = new LoggerRepository();
            UserRepository userRepo = new UserRepository();            

            bool results = false;            

            try
            {

#if (!DEBUG) 
                List<ParameterModel> parameters = paramRepo.GetAllParameters();

                string adPath = parameters.Where(x => x.Name == "DefaultActiveDirectoryServer").FirstOrDefault().Value;
                string domain = parameters.Where(x => x.Name == "Domain").FirstOrDefault().Value;
                //Create an Object of a Class 
               // ActiveDirectoryValidator adAuth = new ActiveDirectoryValidator(adPath);
               if (true) // By pass AD integration
                  // if (true == adAuth.IsAuthenticated(domain, userName, password))
                   {

                    // Create the authetication ticket
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddMinutes(60), false, "");
                    // Now encrypt the ticket.
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    // Create a cookie and add the encrypted ticket to the
                    // cookie as data.
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    // Add the cookie to the outgoing cookies collection.
                    System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);
                    // Redirect the user to the originally requested page
                    // Response.Redirect("Default.aspx", false);

                    UserModel user = userRepo.GetUserByName(userName);

                    if(user != null){
                        results = true;
                    }
                    //Check the DB if this user is one of the users that needs to have access 
                }
                else
                {
                    results = false;
                }
#else                
                // Create the authentication ticket
                //FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddMinutes(60), false, "");
                //// Now encrypt the ticket.
                //string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                //// Create a cookie and add the encrypted ticket to the
                //// cookie as data.
                //HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                //// Add the cookie to the outgoing cookies collection.
                //System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);
                //UserModel user = userRepo.GetUserByName(userName);

                //if (user != null)
                //{
                //    results = true;
                //}
#endif
            }
            catch (Exception ex)
            {
                //throw ex;
                loggerRepo.LogError(String.Format("{0} attempted to log in.", userName) , ex.Message);
                results = false;
            }
            finally
            {
            }
            return results;
        }

        public ActionResult Logout(){

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
            
        }
    }
}
