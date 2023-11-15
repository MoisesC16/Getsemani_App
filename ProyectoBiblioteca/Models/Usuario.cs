using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_Getsemani.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
        public string Codigo { get; set; }
        public TipoUsuario oTipoUsuario { get; set; }
        public bool Estado { get; set; }
    }
}