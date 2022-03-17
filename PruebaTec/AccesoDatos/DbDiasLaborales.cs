using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaTec.AccesoDatos
{
    public class DbDiasLaborales
    {
        private string _conexion { get; set; }
        public DbDiasLaborales(string conexion)
        {
            _conexion = conexion;
        }

        public bool ConsultarDiaLaboral(int dia, int mes)
        {
            var sql = "select * from diasFestivos where dia = @dia and mes = @mes";
            SqlDataReader read;
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var com = new SqlCommand(sql,con);
                    com.Parameters.AddWithValue("@dia", dia);
                    com.Parameters.AddWithValue("@mes", mes);

                    read = com.ExecuteReader();

                    return read.HasRows;

                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
