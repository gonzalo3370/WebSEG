using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace ECommerce.SQL
{
    public class datos
    {
        public string cadena = "data source= DESKTOP-I3U4BT1\\SQLEXPRESS;initial catalog=SEG2:integrated security=true;";
        public SqlConnection cn;
        private SqlCommandBuilder cmb;
        public DataSet ds = new DataSet();
        public SqlDataAdapter da;
        public SqlCommand comando;

        private void conectar()
        {
            cn = new SqlConnection(cadena);
        }

        public datos()
        {
            conectar();
        }

        //consultar
        public void consultar(string sql, string tabla)
        {
            ds.Tables.Clear();
            da = new SqlDataAdapter(sql, cn);
            cmb = new SqlCommandBuilder(da);
            da.Fill(ds, tabla);
        }

        //eliminar
        public bool eliminar(string tabla, string condicion)
        {
            cn.Open();
            string sql = "delete from" + tabla + "where" + condicion;
            comando = new SqlCommand(sql, cn);
            int i = comando.ExecuteNonQuery();
            cn.Close();
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //actualizar
        public bool actualizar(string tabla, string campos, string condicion)
        {
            cn.Open();
            string sql = "update" + tabla + "set" + campos + "where" + condicion;
            comando = new SqlCommand(sql, cn);
            int i = comando.ExecuteNonQuery();
            cn.Close();
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataTable consultar2(string tabla)
        {
            string sql = "select * from" + tabla;
            da = new SqlDataAdapter(sql, cn);
            DataSet dts = new DataSet();
            da.Fill(dts, tabla);
            DataTable dt = new DataTable();
            dt = dts.Tables[tabla];
            return dt;
        }

        public bool insertar(string sql)
        {
            cn.Open();
            comando = new SqlCommand(sql, cn);
            int i = comando.ExecuteNonQuery();
            cn.Close();
            if (i>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
               
    }
}