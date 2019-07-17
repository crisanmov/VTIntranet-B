using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace VTIntranetD.Models.Helpers
{
    public class UserHelper
    {
        private SqlConnection con;

        private void Conectar()
        {
            string constr = ConfigurationManager.ConnectionStrings["DB_Entities"].ToString();
            con = new SqlConnection(constr);
        }

        public int GetUser()
        {
            Conectar();
            SqlCommand com = new SqlCommand("SELECT MAX(idUser) FROM [vtintranet].[dbo].[User]", con);
            con.Open();
            int idUser = Convert.ToInt32(com.ExecuteScalar());
            con.Close();

            return idUser;
        }
    }
}