using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using VTIntranetD.Models.Dto;
using VTIntranetD.Models.Entities;
using System.Data.Entity;
using NLog;

namespace VTIntranetD.Models.Helpers
{
    public class EventHelper
    {
        private SqlConnection con;
        private UserDataModel db = new UserDataModel();
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

        //get all event
        public List<EventFull> GetAllEvent()
        {
            Conectar();
            List<EventFull> events = new List<EventFull>();

            try
            {
                string query = @"SELECT e.idEvent, a.title, a.description, e.fileName, e.path, e.url 
                            FROM Event as e
                            INNER JOIN Activity as a
                            ON e.idActivity = a.idActivity;";

                SqlCommand com = new SqlCommand(query, con);
                con.Open();
                SqlDataReader registros = com.ExecuteReader();

                while (registros.Read())
                {
                    EventFull evt = new EventFull
                    {
                        IdEvent = int.Parse(registros["idEvent"].ToString()),
                        Title = registros["title"].ToString(),
                        Description = registros["description"].ToString(),
                        FileName = registros["fileName"].ToString(),
                        Path = registros["path"].ToString(),
                        Url = registros["url"].ToString(),

                    };
                    events.Add(evt);
                }
                con.Close();
            }
            catch (NullReferenceException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + " ConnectionString no encontrado en EventHelper->GetAllEvent." + Environment.NewLine + DateTime.Now);
            }
            catch (SqlException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + Environment.NewLine + DateTime.Now);
                return null;
            }

            return events;
        }

        //save activity
        public int CreateActivity(EventFull evt)
        {
            int n = 0;
            Conectar();

            try
            {
                string query = "INSERT INTO Activity(title, description, date) VALUES (@title, @description, @date);";
                SqlCommand addActivity = new SqlCommand(query, con);
                addActivity.Parameters.Add("@title", SqlDbType.VarChar);
                addActivity.Parameters.Add("@description", SqlDbType.VarChar);
                addActivity.Parameters.Add("@date", SqlDbType.DateTime);

                addActivity.Parameters["@title"].Value = evt.Title;
                addActivity.Parameters["@description"].Value = evt.Description;
                addActivity.Parameters["@date"].Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                con.Open();
                int i = addActivity.ExecuteNonQuery();
                con.Close();

                if (i == 1)
                {
                    SqlCommand com = new SqlCommand("SELECT MAX(idActivity) FROM Activity", con);
                    con.Open();
                    evt.IdActivity = Convert.ToInt32(com.ExecuteScalar());
                    con.Close();

                    n = CreateEvent(evt);
                }
                else
                {
                    return 0;
                }
            }
            catch (NullReferenceException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + " ConnectionString no encontrado en EventHelper->GetAllEvent." + Environment.NewLine + DateTime.Now);
            }
            catch (SqlException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + Environment.NewLine + DateTime.Now);
                return 0;
            }

            return n;
        }

        //save event
        public int CreateEvent(EventFull evt)
        {
            Event evnt = new Event()
            {
                FileName = evt.FileName,
                Path = evt.Path,
                Url = evt.Url,
                IdActivity = evt.IdActivity,
            };

            db.Event.Add(evnt);
            int r = db.SaveChanges();

            return r;

        }
    }
}