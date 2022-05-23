using Dashboard.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.Controllers
{
    [Authorize]
    public class LoggerController : Controller
    {
        //
        // GET: /Logger/

        public ActionResult Index()
        {
            LoggerRepository repo = new LoggerRepository();
            return View(repo.GetAllLoggerEntries());         
        }

    }
}
