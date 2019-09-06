using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using NLog;

namespace VTIntranetD.Models.Helpers
{
    public class UserHelper
    {
        private SqlConnection con;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private void Conectar()
        {
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["DB_Entities"].ToString();
                con = new SqlConnection(constr);
            }
            catch (NullReferenceException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + " ConnectionString en Conectar() no encontrado." + Environment.NewLine + DateTime.Now);
            }
        }

        public int GetUser()
        {
            Conectar();
            int idUser = 0;

            try
            {
                SqlCommand com = new SqlCommand("SELECT MAX(idUser) FROM [vtintranet].[dbo].[User]", con);
                con.Open();
                idUser = Convert.ToInt32(com.ExecuteScalar());
                con.Close();
            }
            catch (NullReferenceException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + " ConnectionString no encontrado en UserHelper->GetUser." + Environment.NewLine + DateTime.Now);
            }
            catch (SqlException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + Environment.NewLine + DateTime.Now);
                return 0;
            }

            return idUser;
        }

        public int GetIDProfile(int idUser)
        {
            Conectar();
            int idProfile = 0;

            try
            {
                string query = @"SELECT Profile.idProfile FROM [User] 
                                INNER JOIN Profile ON Profile.idUser = [User].idUser
                                WHERE [User].idUser = @idUser;";

                SqlCommand com = new SqlCommand(query, con);
                com.Parameters.Add("@idUser", SqlDbType.Int);
                com.Parameters["@idUser"].Value = idUser;
                con.Open();
                idProfile = Convert.ToInt32(com.ExecuteScalar());
                con.Close();
            }
            catch (NullReferenceException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + " ConnectionString no encontrado en UserHelper->GetIDProfile." + Environment.NewLine + DateTime.Now);
            }
            catch (SqlException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + Environment.NewLine + DateTime.Now);
                return 0;
            }

            return idProfile;
        }
    }
}