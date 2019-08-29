using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using NLog;
using VTIntranetD.Models.Dto;

namespace VTIntranetD.Models.Helpers
{
    public class NoticeHelper
    {
        private SqlConnection con;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        //connection db
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

        //getAll Notices
        public List<Notice> getAllNotice()
        {
            Conectar();
            List<Notice> notices = new List<Notice>();

            try
            {
                SqlCommand com = new SqlCommand("SELECT * FROM Notice INNER JOIN Activity ON Notice.idActivity = Activity.idActivity ORDER BY Notice.startDate DESC;", con);
                con.Open();
                SqlDataReader rows = com.ExecuteReader();
                while (rows.Read())
                {
                    Notice notice = new Notice()
                    {
                        IdNotice = int.Parse(rows["idNotice"].ToString()),
                        Title = rows["title"].ToString(),
                        Description = rows["description"].ToString(),
                        StartDateNotice = rows["startDate"].ToString(),
                        EndDateNotice = rows["endDate"].ToString(),
                        //StartDateNotice = DateTime.Parse(rows["startDateNotice"].ToString()),
                    };
                    notices.Add(notice);
                }
                con.Close();
            }
            catch (NullReferenceException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + " ConnectionString no encontrado en NoticeHelper->getAllNotice." + Environment.NewLine + DateTime.Now);
            }
            catch (SqlException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + Environment.NewLine + DateTime.Now);
                return null;
            }
           
            return notices;
        }

        //save activity
        public int CreateActivity(Activity activity, Notice notice)
        {
            int n = 0;
            Conectar();

            try
            {
                SqlCommand addActivity = new SqlCommand("insert into Activity(title, description, date) values (@title, @description, @date)", con);
                addActivity.Parameters.Add("@title", SqlDbType.VarChar);
                addActivity.Parameters.Add("@description", SqlDbType.VarChar);
                addActivity.Parameters.Add("@date", SqlDbType.DateTime);

                addActivity.Parameters["@title"].Value = activity.Title;
                addActivity.Parameters["@description"].Value = activity.Description;
                addActivity.Parameters["@date"].Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                con.Open();
                int i = addActivity.ExecuteNonQuery();
                con.Close();

                if (i == 1)
                {
                    try
                    {
                        SqlCommand com = new SqlCommand("select max(idActivity) from Activity", con);
                        con.Open();
                        int idActivity = Convert.ToInt32(com.ExecuteScalar());
                        con.Close();

                        n = CreateNotice(notice, idActivity);
                    }
                    catch (NullReferenceException ex)
                    {
                        string errMsg = ex.Message;
                        logger.Error(errMsg + " ConnectionString no encontrado en NoticeHelper->CreateActivity." + Environment.NewLine + DateTime.Now);
                    }
                    catch (SqlException ex)
                    {
                        string errMsg = ex.Message;
                        logger.Error(errMsg + Environment.NewLine + DateTime.Now);
                        return 0;
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + " ConnectionString no encontrado en NoticeHelper->CreateActivity." + Environment.NewLine + DateTime.Now);
            }
            catch (SqlException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + Environment.NewLine + DateTime.Now);
                return 0;
            }
            
            return n;
        }

        //save notice
        public int CreateNotice(Notice notice, int idActivity)
        {
            int r = 0;

            try
            {
                Conectar();
                
                SqlCommand addNotice = new SqlCommand("insert into Notice(startDate, endDate, idActivity) values (@startDateNotice, @endDateNotice, @idActivity)", con);
                addNotice.Parameters.Add("@startDateNotice", SqlDbType.DateTime);
                addNotice.Parameters.Add("@endDateNotice", SqlDbType.DateTime);
                addNotice.Parameters.Add("@idActivity", SqlDbType.Int);

                addNotice.Parameters["@startDateNotice"].Value = notice.StartDateNotice;
                addNotice.Parameters["@endDateNotice"].Value = notice.EndDateNotice;
                addNotice.Parameters["@idActivity"].Value = idActivity;

                con.Open();
                r = addNotice.ExecuteNonQuery();
                con.Close();
            }
            catch (NullReferenceException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + " ConnectionString no encontrado en public int CreateNotice." + Environment.NewLine + DateTime.Now);
            }
            catch (SqlException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + Environment.NewLine + DateTime.Now);
                return 0;
            }

            return r;

        }

        //get notice for id
        public Notice getNotice(int idNotice)
        {
            Notice notice = new Notice();

            try
            {
                Conectar();
                string query = @"SELECT Notice.idNotice, Activity.title, 
	                                Activity.description, Notice.startDate, Notice.endDate
                                FROM Notice 
                                INNER JOIN Activity 
                                ON Notice.idActivity = Activity.idActivity WHERE idNotice = @idNotice";
                SqlCommand com = new SqlCommand(query, con);
                com.Parameters.Add("@idNotice", SqlDbType.Int);
                com.Parameters["@idNotice"].Value = idNotice;
                con.Open();

                SqlDataReader rows = com.ExecuteReader();
                notice = new Notice();

                while (rows.Read())
                {

                    notice.IdNotice = int.Parse(rows["idNotice"].ToString());
                    notice.Title = rows["title"].ToString();
                    notice.Description = rows["description"].ToString();
                    notice.StartDateNotice = rows["startDate"].ToString();
                    notice.EndDateNotice = rows["endDate"].ToString();

                }
                con.Close();
            }
            catch (NullReferenceException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + " ConnectionString no encontrado en NoticeHelper->getNotice()." + Environment.NewLine + DateTime.Now);
            }
            catch (SqlException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + Environment.NewLine + DateTime.Now);
                return null;
            }

            return notice;
        }
    }
}