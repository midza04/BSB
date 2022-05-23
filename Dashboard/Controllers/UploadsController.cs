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
    public class UploadsController : Controller
    {
        //
        // GET: /Uploads/

        public ActionResult Index()
        {
            UploadsRepository repo = new UploadsRepository();
            List<UploadModel> model = new List<UploadModel>();
            model = repo.GetAllUploads();
            return View(model);
        }

        public ActionResult UploadedFilesPartial()
        {
            UploadsRepository repo = new UploadsRepository();
            List<UploadModel> model = new List<UploadModel>();
            model = repo.GetAllUploads().Take(5).ToList();
            return View(model);
        }

    }
}
