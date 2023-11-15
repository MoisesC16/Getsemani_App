using Proyecto_Getsemani.Logica;
using Proyecto_Getsemani.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto_Getsemani.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Usuarios()
        {
            return View();
        }

        public ActionResult Lectores()
        {
            return View();
        }

        
    }
}