using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using VTIntranetD.Models.Dto;
using NLog;

namespace VTIntranetD.Models.Helpers
{
    public class MultimediaHelper
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

        //getAll Multimedia
        public List<Multimedia> GetAlbumPortriat(int idAlbum)
        {
            Conectar();
            List<Multimedia> portrait = new List<Multimedia>();

            try
            {
                string q = @"SELECT Event.idEvent, Activity.title, Activity.description, 
                            Multimedia.fileName, Multimedia.idMultimedia, Multimedia.path 
                        FROM Event 
                        INNER JOIN EventMult ON Event.idEvent = EventMult.idEvent 
                        INNER JOIN Activity ON Activity.idActivity = Event.idActivity 
                        INNER JOIN Multimedia ON Multimedia.idMultimedia = EventMult.idMult 
                        WHERE Event.idEvent = @idAlbum
                        ORDER BY Multimedia.idMultimedia
                        OFFSET 0 ROWS FETCH FIRST 4 ROWS ONLY";

                SqlCommand com = new SqlCommand(q, con);
                com.Parameters.Add("@idAlbum", SqlDbType.Int);
                com.Parameters["@idAlbum"].Value = idAlbum;
                con.Open();
                SqlDataReader rows = com.ExecuteReader();
                while (rows.Read())
                {
                    Multimedia img = new Multimedia()
                    {
                        IdMultimedia = int.Parse(rows["idMultimedia"].ToString()),
                        FileName = rows["fileName"].ToString(),
                        Path = rows["path"].ToString()

                    };
                    portrait.Add(img);
                }
                con.Close();
            }
            catch (NullReferenceException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + " ConnectionString no encontrado en MultimediaHelper->GetAlbumPortriat." + Environment.NewLine + DateTime.Now);
            }
            catch (SqlException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + Environment.NewLine + DateTime.Now);
                return null;
            }

            return portrait;
        }

        //save Multimedia
        public int CreateMultimedia(Multimedia mult)
        {
            Conectar();

            int r = 0;
            int idMultimedia = 0;

            try
            {
                SqlCommand addEvent = new SqlCommand("INSERT INTO Multimedia(fileName, path) values (@fileName, @path)", con);
                addEvent.Parameters.Add("@fileName", SqlDbType.VarChar);
                addEvent.Parameters.Add("@path", SqlDbType.VarChar);
                addEvent.Parameters["@fileName"].Value = mult.FileName;
                addEvent.Parameters["@path"].Value = mult.Path;

                con.Open();
                r = addEvent.ExecuteNonQuery();
                con.Close();

                SqlCommand com = new SqlCommand("SELECT MAX(idMultimedia) FROM Multimedia", con);
                con.Open();
                idMultimedia = Convert.ToInt32(com.ExecuteScalar());
                con.Close();
            }
            catch (NullReferenceException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + " ConnectionString no encontrado en MultimediaHelper->CreateMultimedia." + Environment.NewLine + DateTime.Now);
            }
            catch (SqlException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + Environment.NewLine + DateTime.Now);
                return 0;
            }


            if (r == 1)
            {
                return idMultimedia;
            }
            else
            {
                return 0;
            }

        }

        //save relationship event -> image
        public int SaveEventMult(int idEvt, int idMult)
        {
            Conectar();
            int r = 0;

            try
            {
                SqlCommand evtMult = new SqlCommand("INSERT INTO EventMult(idEvent, idMult) values (@idEvent, @idMultimedia)", con);
                evtMult.Parameters.Add("@idEvent", SqlDbType.Int);
                evtMult.Parameters.Add("@idMultimedia", SqlDbType.Int);
                evtMult.Parameters["@idEvent"].Value = idEvt;
                evtMult.Parameters["@idMultimedia"].Value = idMult;

                con.Open();
                r = evtMult.ExecuteNonQuery();
                con.Close();
            }
            catch (NullReferenceException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + " ConnectionString no encontrado en MultimediaHelper->SaveEventMult." + Environment.NewLine + DateTime.Now);
            }
            catch (SqlException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + Environment.NewLine + DateTime.Now);
                return 0;
            }


            return r;

        }

        //get images for ID event
        public List<Multimedia> GetImages(int idEvent)
        {
            Conectar();
            List<Multimedia> album = new List<Multimedia>();

            try
            {
                string query = @"SELECT Multimedia.idMultimedia, Event.idEvent, Multimedia.fileName, Multimedia.path
                            FROM Event
                            INNER JOIN EventMult ON Event.idEvent = EventMult.idEvent
                            INNER JOIN Activity ON Activity.idActivity = Event.idActivity
                            INNER JOIN Multimedia ON Multimedia.idMultimedia = EventMult.idMult
                            WHERE Event.idEvent = @idEvent;";

                SqlCommand com = new SqlCommand(query, con);
                com.Parameters.Add("@idEvent", SqlDbType.Int);
                com.Parameters["@idEvent"].Value = idEvent;
                con.Open();
                SqlDataReader rows = com.ExecuteReader();
                while (rows.Read())
                {
                    Multimedia img = new Multimedia()
                    {
                        IdMultimedia = int.Parse(rows["idMultimedia"].ToString()),
                        FileName = rows["fileName"].ToString(),
                        Path = "/UploadedFiles/events/" + rows["fileName"].ToString()

                    };
                    album.Add(img);
                }
            }
            catch (NullReferenceException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + " ConnectionString no encontrado en MultimediaHelper->GetImages." + Environment.NewLine + DateTime.Now);
            }
            catch (SqlException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + Environment.NewLine + DateTime.Now);
                return null;
            }

            return album;
        }
    }
}