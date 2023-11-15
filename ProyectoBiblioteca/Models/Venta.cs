using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_Getsemani.Models
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public EstadoVenta oEstadoVenta { get; set; }
        public Usuario oUsuario { get; set; }
        public Libro oLibro { get; set; }
        public DateTime FechaVenta { get; set; }
        public string TextoFechaVenta { get; set; }
        public DateTime FechaConfirmacionVenta { get; set; }
        public string TextoFechaConfirmacionVenta { get; set; }
        public string EstadoEntregado { get; set; }
        public string EstadoRecibido { get; set; }
        public bool Estado { get; set; }
    }
}