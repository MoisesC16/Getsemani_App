using Proyecto_Getsemani.Logica;
using Proyecto_Getsemani.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto_Getsemani.Controllers
{
    public class VentaController : Controller
    {
        // GET: Venta
        public ActionResult Registrar()
        {
            return View();
        }

        public ActionResult Consultar()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GuardarVentas(Venta objeto)
        {
            bool _respuesta = false;
            string _mensaje = string.Empty;

            _respuesta = VentaLogica.Instancia.Existe(objeto);

            if (_respuesta)
            {
                _respuesta = false;
                _mensaje = "El lector ya tiene un Venta pendiente con el mismo libro";
            }
            else {
                _respuesta = VentaLogica.Instancia.Registrar(objeto);
                _mensaje = _respuesta ? "Registro completo" : "No se pudo registrar";
            }

            
            return Json(new { resultado = _respuesta, mensaje = _mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarEstados()
        {
            List<EstadoVenta> oLista = new List<EstadoVenta>();
            oLista = VentaLogica.Instancia.ListarEstados();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Listar(int idestadoVenta, int idUsuario)
        {
            List<Venta> oLista = new List<Venta>();
            oLista = VentaLogica.Instancia.Listar(idestadoVenta, idUsuario);
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Devolver(string estadorecibido,int idVenta)
        {
            bool respuesta = false;
            respuesta = VentaLogica.Instancia.Devolver(estadorecibido,idVenta);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
    }
}