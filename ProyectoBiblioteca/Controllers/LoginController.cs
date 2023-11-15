using Proyecto_Getsemani.Logica;
using Proyecto_Getsemani.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto_Getsemani.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {

            Usuario ousuario = UsuarioLogica.Instancia.Listar().Where(u => u.Correo == correo && u.Clave == clave && u.oTipoUsuario.IdTipoUsuario != 3).FirstOrDefault();

            if (ousuario == null)
            {
                ViewBag.Error = "Usuario o contraseña no correcta";
                return View();
            }

            Session["Usuario"] = ousuario;

            return RedirectToAction("Index", "Admin");
        }
    }
}