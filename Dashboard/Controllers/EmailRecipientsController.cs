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
    public class EmailRecipientsController : Controller
    {
        EmailRecipientRepository _repo;

        public EmailRecipientsController()
        {
            _repo = new EmailRecipientRepository();
        }
        //
        // GET: /Email/
        public ActionResult Index()
        {
            
            EmailRecipientViewModel model = new EmailRecipientViewModel();
            model.Emails = _repo.GetAllEmailRecipients();

            if (TempData["IsUpdated"] != null)
            {
                ViewBag.Updated = true;
                TempData.Remove("IsUpdated");
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            EmailRecipientViewModel model = new EmailRecipientViewModel();
            var emailRecipient = _repo.GetEmailRecipient(id);
            return View(emailRecipient);
        }

        [HttpPost]
        public ActionResult EditEmail(EmailRecipientModel emailRecipient)
        {
            EmailRecipientRepository repo = new EmailRecipientRepository();
            EmailRecipientViewModel model = new EmailRecipientViewModel();

            repo.UpdateEmailRecipient(emailRecipient);

            TempData["IsUpdated"] = true;
            return RedirectToAction("Index", "EmailRecipients");
        }


        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveEmail(EmailRecipientModel emailRecipient)
        {
            EmailRecipientViewModel model = new EmailRecipientViewModel();

            _repo.InsertEmailRecipient(emailRecipient);

            TempData["IsUpdated"] = true;
            return RedirectToAction("Index", "EmailRecipients");
        }


        public ActionResult Delete(int id)
        {
            EmailRecipientViewModel model = new EmailRecipientViewModel();
            var emailRecipient = _repo.GetEmailRecipient(id);
            return View(emailRecipient);
        }

        [HttpPost]
        public ActionResult Delete(EmailRecipientModel emailRecipient)
        {
            EmailRecipientViewModel model = new EmailRecipientViewModel();
            _repo.DeleteEmailRecipient(emailRecipient.ID);
            TempData["IsUpdated"] = true;
            return RedirectToAction("Index", "EmailRecipients");
        }

    }
}
