using Proyecto_Getsemani.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace Proyecto_Getsemani.Logica
{
    public class VentaLogica
    {
        private static VentaLogica instancia = null;

        public VentaLogica()
        {

        }

        public static VentaLogica Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new VentaLogica();
                }

                return instancia;
            }
        }

        public bool Registrar(Venta objeto)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarVenta", oConexion);
                    cmd.Parameters.AddWithValue("IdEstadoVenta", objeto.oEstadoVenta.IdEstadoVenta);
                    cmd.Parameters.AddWithValue("IdUsuario", objeto.oUsuario.IdUsuario);
                    cmd.Parameters.AddWithValue("IdLibro", objeto.oLibro.IdLibro);
                    cmd.Parameters.AddWithValue("FechaVenta", Convert.ToDateTime(objeto.TextoFechaVenta,new CultureInfo("es-PE")) );
                    cmd.Parameters.AddWithValue("EstadoEntregado", objeto.EstadoEntregado);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);

                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public bool Existe(Venta objeto)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_existeVenta", oConexion);
                    cmd.Parameters.AddWithValue("IdUsuario", objeto.oUsuario.IdUsuario);
                    cmd.Parameters.AddWithValue("IdLibro", objeto.oLibro.IdLibro);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public List<EstadoVenta> ListarEstados()
        {
            List<EstadoVenta> Lista = new List<EstadoVenta>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("select IdEstadoVenta,Descripcion from ESTADO_VENTA", oConexion);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Lista.Add(new EstadoVenta()
                            {
                                IdEstadoVenta = Convert.ToInt32(dr["IdEstadoVenta"]),
                                Descripcion = dr["Descripcion"].ToString()
                            });
                        }
                    }

                }
                catch (Exception ex)
                {
                    Lista = new List<EstadoVenta>();
                }
            }
            return Lista;
        }


        public List<Venta> Listar(int idestadoVenta, int idUsuario)
        {
            List<Venta> Lista = new List<Venta>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    StringBuilder query = new StringBuilder();

                    query.AppendLine("set dateformat dmy");
                    query.AppendLine("select p.IdVenta,ep.IdEstadoVenta,ep.Descripcion,pe.Codigo,pe.Nombre,pe.Apellido,li.Titulo,convert(char(10),p.FechaVenta,103)[FechaVenta],convert(char(10),p.FechaConfirmacionVenta,103)[FechaConfirmacionVenta],p.EstadoEntregado,p.EstadoRecibido from VENTA p");
                    query.AppendLine("inner join estado_venta ep on ep.IdEstadoVenta = p.IdEstadoVenta");
                    query.AppendLine("inner join Usuario pe on pe.IdUsuario = p.IdUsuario");
                    query.AppendLine("inner join LIBRO li on li.IdLibro = p.IdLibro");
                    query.AppendLine("where ep.IdEstadoVenta = iif(@idestadoVenta=0,ep.IdEstadoVenta,@idestadoVenta) and pe.IdUsuario = iif(@idUsuario=0,pe.IdUsuario,@idUsuario)");


                    SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
                    cmd.Parameters.AddWithValue("@idEstadoVenta", idestadoVenta);
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Lista.Add(new Venta()
                            {
                                IdVenta = Convert.ToInt32(dr["IdVenta"]),
                                oEstadoVenta = new EstadoVenta() { IdEstadoVenta= Convert.ToInt32(dr["IdEstadoVenta"]),  Descripcion = dr["Descripcion"].ToString() },
                                oUsuario = new Usuario() { Codigo = dr["Codigo"].ToString(), Nombre = dr["Nombre"].ToString(), Apellido = dr["Apellido"].ToString() },
                                oLibro= new Libro() { Titulo = dr["Titulo"].ToString() },
                                TextoFechaVenta = dr["FechaVenta"].ToString(),
                                TextoFechaConfirmacionVenta = dr["FechaConfirmacionVenta"].ToString(),
                                EstadoEntregado = dr["EstadoEntregado"].ToString(),
                                EstadoRecibido = dr["EstadoRecibido"].ToString()
                            });
                        }
                    }

                }
                catch (Exception ex)
                {
                    Lista = new List<Venta>();
                }
            }
            return Lista;
        }

        public bool Devolver(string estadorecibido, int idVenta)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("update Venta set IdEstadoVenta = 2 ,FechaConfirmacionVenta = GETDATE(),EstadoRecibido =@estadorecibido");
                    query.AppendLine("where IdVenta = @idVenta");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
                    cmd.Parameters.AddWithValue("@estadorecibido", estadorecibido);
                    cmd.Parameters.AddWithValue("@idVenta", idVenta);
                    cmd.CommandType = CommandType.Text;
                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }



    }
}