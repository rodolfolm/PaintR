using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SignalrWeb.Models;

namespace SignalrWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            PaintModel paintModel = new PaintModel { Id = "public" };
            return View("Paint", paintModel);
        }

        public ActionResult Private()
        {
            return RedirectToAction("PrivatePaint", new { id = Guid.NewGuid() });
        }


        public ViewResult PrivatePaint(Guid id)
        {
            PaintModel paintModel = new PaintModel {Id = id.ToString()};
            return View(paintModel);
        }
    }
}
