using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SlaughterChargeMS.Content
{
    public class SettingClassController : Controller
    {
        public ActionResult Index()
        {
            Response.Redirect("~/Login/Index");
            return View();
        }
    }
}
