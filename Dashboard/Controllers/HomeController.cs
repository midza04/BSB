using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dashboard.Models;
using Dashboard.Repository;

namespace Dashboard.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        ParameterRepository _repo = new ParameterRepository();
        
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult About()
        {

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Settings()
        {
        
            ParameterViewModel model = new ParameterViewModel();
            model.Parameters = _repo.GetAllParameters();

            if (TempData["IsUpdated"] != null)
            {
                ViewBag.Updated = true;
                TempData.Remove("IsUpdated");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult SaveSettings(ParameterViewModel model)
        {
            _repo.UpdateParameters(model.Parameters);
            TempData["IsUpdated"] = true;
            return RedirectToAction("Settings", "Home");
        }
    }
}
