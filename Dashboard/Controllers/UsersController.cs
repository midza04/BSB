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
    public class UsersController : Controller
    {
        //
        // GET: /Users/
        UserRepository _repo;

        public UsersController()
        {
            _repo = new UserRepository();
        }

        public ActionResult Index()
        {
            UserViewModel model = new UserViewModel();
            model.Users = _repo.GetAllUsers();

            if (TempData["IsUpdated"] != null)
            {
                ViewBag.Updated = true;
                TempData.Remove("IsUpdated");
            }

            return View(model);
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveUser(UserModel user)
        {
            _repo.InsertUser(user);
            TempData["IsUpdated"] = true;
            return RedirectToAction("Index", "Users");            
        }
        
        public ActionResult Edit(int id)
        {
            var user = _repo.GetUser(id);
            return View(user);
        }

        [HttpPost]
        public ActionResult EditUser(UserModel model)
        {            
            _repo.UpdateUser(model);
            TempData["IsUpdated"] = true;
            return RedirectToAction("Index", "Users");
        }


        public ActionResult Delete(int id)
        {
            var user = _repo.GetUser(id);
            return View(user);
        }

        [HttpPost]
        public ActionResult Delete(UserModel user)
        {
            _repo.DeleteEmailRecipient(user.ID);
            TempData["IsUpdated"] = true;
            return RedirectToAction("Index", "Users");
        }
    }
}
