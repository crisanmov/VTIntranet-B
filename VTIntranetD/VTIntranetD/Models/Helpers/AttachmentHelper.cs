﻿using System;
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
    public class AttachmentHelper
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

        //save attachment
        public int CreateAttachment(Attachment file)
        {
            Conectar();
            int result = 0;
            int idAttachment = 0;

            try
            {
                
                string query = @"INSERT INTO Attachment(attachmentName, attachmentDirectory, attachmentActive) 
                            VALUES (@attachmentName, @attachmentDirectory, @attachmentActive)";

                SqlCommand addAttach = new SqlCommand(query, con);
                addAttach.Parameters.Add("@attachmentName", SqlDbType.VarChar);
                addAttach.Parameters.Add("@attachmentDirectory", SqlDbType.VarChar);
                addAttach.Parameters.Add("@attachmentActive", SqlDbType.Bit);
                addAttach.Parameters["@attachmentName"].Value = file.AttachmentName;
                addAttach.Parameters["@attachmentDirectory"].Value = file.AttachmentDirectory;
                addAttach.Parameters["@attachmentActive"].Value = true;

                con.Open();
                result = addAttach.ExecuteNonQuery();
                con.Close();
            }
            catch (NullReferenceException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + " ConnectionString no encontrado en AttachmentHelper->CreateAttachment." + Environment.NewLine + DateTime.Now);
            }
            catch (SqlException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + Environment.NewLine + DateTime.Now);
                return 0;
            }


            if (result == 1)
            {
                try
                {
                    SqlCommand com = new SqlCommand("SELECT MAX(idAttachment) from Attachment", con);
                    con.Open();
                    idAttachment = Convert.ToInt32(com.ExecuteScalar());
                    con.Close();
                }
                catch (NullReferenceException ex)
                {
                    string errMsg = ex.Message;
                    logger.Error(errMsg + " ConnectionString no encontrado en AttachmentHelper->CreateAttachment." + Environment.NewLine + DateTime.Now);
                }
                catch (SqlException ex)
                {
                    string errMsg = ex.Message;
                    logger.Error(errMsg + Environment.NewLine + DateTime.Now);
                    return 0;
                }

                return idAttachment;
            }
            else { return 0; }

        }

        public int CreateRelationAttachment(int idTag, int idParent, int idDepto, int idAttachent)
        {
            Conectar();
            int r = 0;
            try
            {
                string query = @"INSERT INTO TagDeptoAttach(idTag, idParent, idDepto, idAttachment) VALUES (@idTag, @idParent, @idDepto, @idAttachment);";
                SqlCommand attachTagDepto = new SqlCommand(query, con);
                attachTagDepto.Parameters.Add("@idTag", SqlDbType.Int);
                attachTagDepto.Parameters.Add("@idParent", SqlDbType.Int);
                attachTagDepto.Parameters.Add("@idDepto", SqlDbType.Int);
                attachTagDepto.Parameters.Add("@idAttachment", SqlDbType.Int);
                attachTagDepto.Parameters["@idTag"].Value = idTag;
                attachTagDepto.Parameters["@idParent"].Value = idParent;
                attachTagDepto.Parameters["@idDepto"].Value = idDepto;
                attachTagDepto.Parameters["@idAttachment"].Value = idAttachent;

                con.Open();
                r = attachTagDepto.ExecuteNonQuery();
                con.Close();
            }
            catch (NullReferenceException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + " ConnectionString no encontrado en AttachmentHelper->CreateRelationAttachment." + Environment.NewLine + DateTime.Now);
            }
            catch (SqlException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + Environment.NewLine + DateTime.Now);
                return r;
            }

            return r;
        }




        //delete attachment
        public int DeleteAttach(int idAttachment)
        {
            Conectar();

            string query = "DELETE FROM Attachment WHERE idAttachment = @idAttachment";
            SqlCommand deleteAttach = new SqlCommand(query, con);
            deleteAttach.Parameters.Add("@idAttachment", SqlDbType.Int);
            deleteAttach.Parameters["@idAttachment"].Value = idAttachment;

            con.Open();
            int r = deleteAttach.ExecuteNonQuery();
            con.Close();

            return r;
        }

        //delete attachment relationship
        public int DeleteAttachRelation(int idAttachment, int idDepto)
        {
            Conectar();

            string query = "DELETE FROM TagDeptoAttach WHERE idAttachment= @idAttachment And idDepto= @idDepto;";
            SqlCommand deleteAttach = new SqlCommand(query, con);
            deleteAttach.Parameters.Add("@idAttachment", SqlDbType.Int);
            deleteAttach.Parameters.Add("@idDepto", SqlDbType.Int);
            deleteAttach.Parameters["@idAttachment"].Value = idAttachment;
            deleteAttach.Parameters["@idDepto"].Value = idDepto;

            con.Open();
            int r = deleteAttach.ExecuteNonQuery();
            con.Close();

            return r;

        }

        public List<Attachment> GetAttachDepto(string tagClabe)
        {
            Conectar();
            List<Attachment> attachments = new List<Attachment>();

            string query = @"SELECT Depto.idDepto, Attachment.idAttachment, Attachment.attachmentName, Depto.name, Attachment.attachmentDirectory
                            FROM Attachment
                            INNER JOIN TagDeptoAttach on Attachment.idAttachment = TagDeptoAttach.idAttachment
                            INNER JOIN Tag on TagDeptoAttach.idTag = Tag.idTag
                            INNER JOIN Depto on TagDeptoAttach.idDepto = Depto.idDepto
                            WHERE Tag.tagClabe = @tagClabe;";

            SqlCommand com = new SqlCommand(query, con);
            com.Parameters.Add("@tagClabe", SqlDbType.VarChar);
            com.Parameters["@tagClabe"].Value = tagClabe;
            
            con.Open();
            SqlDataReader rows = com.ExecuteReader();
            

            while (rows.Read())
            {
                Attachment attach = new Attachment()
                {
                    IdAttachment = int.Parse(rows["IdAttachment"].ToString()),
                    AttachmentName = rows["AttachmentName"].ToString(),
                    IdDepto = int.Parse(rows["idDepto"].ToString()),
                    DeptoName = rows["name"].ToString(),
                    AttachmentDirectory = rows["AttachmentDirectory"].ToString(),
                };
                attachments.Add(attach);
            }

            con.Close();
            return attachments;
        }

        public List<Attachment> GetAttachArea(int idParent)
        {
            Conectar();
            List<Attachment> attachments = new List<Attachment>();

            string query = @"SELECT Depto.idDepto, Attachment.idAttachment, Attachment.attachmentName, Depto.name, Attachment.attachmentDirectory
                            FROM Attachment
                            INNER JOIN TagDeptoAttach on Attachment.idAttachment = TagDeptoAttach.idAttachment
                            INNER JOIN Depto on TagDeptoAttach.idDepto = Depto.idDepto
                            WHERE TagDeptoAttach.idParent = @idParent;";

            SqlCommand com = new SqlCommand(query, con);
            com.Parameters.Add("@idParent", SqlDbType.Int);
            com.Parameters["@idParent"].Value = idParent;

            con.Open();
            SqlDataReader rows = com.ExecuteReader();


            while (rows.Read())
            {
                Attachment attach = new Attachment()
                {
                    IdAttachment = int.Parse(rows["IdAttachment"].ToString()),
                    AttachmentName = rows["attachmentName"].ToString(),
                    IdDepto = int.Parse(rows["idDepto"].ToString()),
                    DeptoName = rows["name"].ToString(),
                    AttachmentDirectory = rows["attachmentDirectory"].ToString(),
                };
                attachments.Add(attach);
            }

            con.Close();
            return attachments;
        }

    }
}