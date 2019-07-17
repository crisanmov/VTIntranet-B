using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using VTIntranetD.Models.Dto;

namespace VTIntranetD.Models.Helpers
{
    public class MultimediaHelper
    {
        private SqlConnection con;

        //connection db
        private void Conectar()
        {
            string constr = ConfigurationManager.ConnectionStrings["DB_Entities"].ToString();
            con = new SqlConnection(constr);
        }

        //getAll Multimedia
        public List<Multimedia> GetAlbumPortriat(int idAlbum)
        {
            Conectar();
            List<Multimedia> portrait = new List<Multimedia>();

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
            return portrait;
        }

        //save Multimedia
        public int CreateMultimedia(Multimedia mult)
        {
            Conectar();

            SqlCommand addEvent = new SqlCommand("INSERT INTO Multimedia(fileName, path) values (@fileName, @path)", con);
            addEvent.Parameters.Add("@fileName", SqlDbType.VarChar);
            addEvent.Parameters.Add("@path", SqlDbType.VarChar);
            addEvent.Parameters["@fileName"].Value = mult.FileName;
            addEvent.Parameters["@path"].Value = mult.Path;

            con.Open();
            int r = addEvent.ExecuteNonQuery();
            con.Close();

            SqlCommand com = new SqlCommand("SELECT MAX(idMultimedia) FROM Multimedia", con);
            con.Open();
            int idMultimedia = Convert.ToInt32(com.ExecuteScalar());
            con.Close();

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

            SqlCommand evtMult = new SqlCommand("INSERT INTO EventMult(idEvent, idMult) values (@idEvent, @idMultimedia)", con);
            evtMult.Parameters.Add("@idEvent", SqlDbType.Int);
            evtMult.Parameters.Add("@idMultimedia", SqlDbType.Int);
            evtMult.Parameters["@idEvent"].Value = idEvt;
            evtMult.Parameters["@idMultimedia"].Value = idMult;

            con.Open();
            int r = evtMult.ExecuteNonQuery();
            con.Close();

            return r;

        }

        //get images for ID event
        public List<Multimedia> GetImages(int idEvent)
        {
            Conectar();
            List<Multimedia> album = new List<Multimedia>();

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
            return album;
        }
    }
}