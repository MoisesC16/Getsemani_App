using Proyecto_Getsemani.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto_Getsemani.Controllers
{
    public class AdminController : Controller
    {
        private static Usuario oPesona;
        // GET: Admin
        public ActionResult Index()
        {
            if(Session["Usuario"] == null)
                return RedirectToAction("Index", "Login");

            oPesona = (Usuario)Session["Usuario"];

            return View();
        }

        public ActionResult CerrarSesion()
        {
            Session["Usuario"] = null;
            return RedirectToAction("Index", "Login");
        }

    }
}