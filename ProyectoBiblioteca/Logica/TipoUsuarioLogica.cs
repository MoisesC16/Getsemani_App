using Proyecto_Getsemani.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Proyecto_Getsemani.Logica
{
    public class TipoUsuarioLogica
    {
        private static TipoUsuarioLogica instancia = null;

        public TipoUsuarioLogica()
        {

        }

        public static TipoUsuarioLogica Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new TipoUsuarioLogica();
                }

                return instancia;
            }
        }

        public List<TipoUsuario> Listar()
        {
            List<TipoUsuario> Lista = new List<TipoUsuario>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("select IdTipoUsuario,Descripcion from TIPO_USUARIO", oConexion);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Lista.Add(new TipoUsuario()
                            {
                                IdTipoUsuario = Convert.ToInt32(dr["IdTipoUsuario"]),
                                Descripcion = dr["Descripcion"].ToString()
                            });
                        }
                    }

                }
                catch (Exception ex)
                {
                    Lista = new List<TipoUsuario>();
                }
            }
            return Lista;
        }
    }
}