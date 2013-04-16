using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Heritage.Areas.Contabil.Controllers
{
    [Authorize(Roles="Contabil")]
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

    }
}
