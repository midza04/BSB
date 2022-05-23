using Dashboard.Models;
using Dashboard.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        //
        // GET: /Settings/
        ParameterRepository _repo = new ParameterRepository();
        public ActionResult Index()
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

        public ActionResult Emails()
        {
            EmailParameterModel settings = _repo.GetAllEmailParameters();
            
            if (TempData["IsUpdated"] != null)
            {
                ViewBag.Updated = true;
                TempData.Remove("IsUpdated");
            }

            return View(settings);
        }

        [HttpPost]
        public ActionResult SaveSettings(ParameterViewModel model)
        {            
            _repo.UpdateParameters(model.Parameters);
            TempData["IsUpdated"] = true;
            return RedirectToAction("Index", "Settings");
        }

        [HttpPost]
        public ActionResult SaveEmailSettings(EmailParameterModel model)
        {
            _repo.UpdateEmailParameters(model);
            TempData["IsUpdated"] = true;
            return RedirectToAction("Emails", "Settings");
        }      
    }
}
