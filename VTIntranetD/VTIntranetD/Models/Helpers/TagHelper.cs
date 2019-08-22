using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using VTIntranetD.Models.Entities;
using VTIntranetD.Models.Dto;
using NLog;

namespace VTIntranetD.Models.Helpers
{
    public class TagHelper
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

        //get all tags for idProfile
        public List<Brand> getTagsDeptos(int idProfile)
        {
            Conectar();
            List<Brand> brands = new List<Brand>();

            try
            {
                string query = @"SELECT Tag.idTag, Tag.tagName, Tag.tagClabe, Depto.idDepto, Depto.name, Depto.clabe
                            FROM Depto
                            INNER join ProfileTagDepto ON ProfileTagDepto.idDepto = Depto.idDepto
                            INNER join Tag ON Tag.idTag = ProfileTagDepto.idTag
                            WHERE ProfileTagDepto.idProfile = @idProfile and ProfileTagDepto.idParent = 0
                            Order By Tag.tagName, Depto.name;";

                SqlCommand com = new SqlCommand(query, con);
                com.Parameters.Add("@idProfile", SqlDbType.Int);
                com.Parameters["@idProfile"].Value = idProfile;
                con.Open();
                SqlDataReader rows = com.ExecuteReader();

                while (rows.Read())
                {
                    Brand brand = new Brand()
                    {
                        IdTag = int.Parse(rows["idTag"].ToString()),
                        TagName = rows["tagName"].ToString(),
                        TagClabe = rows["tagClabe"].ToString(),
                        IdDepto = int.Parse(rows["idDepto"].ToString()),
                        DeptoName = rows["name"].ToString(),
                        DeptoClabe = rows["clabe"].ToString()

                    };

                    brands.Add(brand);
                }

                con.Close();
            }
            catch (NullReferenceException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + " ConnectionString no encontrado en TagHelper->getTagsDeptos." + Environment.NewLine + DateTime.Now);
            }
            catch (SqlException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + Environment.NewLine + DateTime.Now);
                return null;
            }

            return brands;

        }

        //get all tags
        public List<Tag> GetTagsAll()
        {
            List<Tag> tags = new List<Tag>();

            try
            {
                Conectar();
                
                SqlCommand com = new SqlCommand("SELECT * FROM Tag ORDER BY tagName;", con);
                con.Open();
                SqlDataReader registros = com.ExecuteReader();
                while (registros.Read())
                {
                    Tag tag = new Tag
                    {
                        idTag = int.Parse(registros["idTag"].ToString()),
                        tagName = registros["tagName"].ToString(),
                        clabe = registros["tagClabe"].ToString()
                        //tagActive = int.Parse(registros["tagActive"].ToString())
                    };
                    tags.Add(tag);
                }
                con.Close();
            }
            catch (NullReferenceException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + " ConnectionString no encontrado en TagHelper->GetTagsAll." + Environment.NewLine + DateTime.Now);
            }
            catch (SqlException ex)
            {
                string errMsg = ex.Message;
                logger.Error(errMsg + Environment.NewLine + DateTime.Now);
                return null;
            }

            return tags;
        }

        //get deptos for each tag in Attachments
        public List<Department> GetDepto(string brand, int idProfile)
        {
            Conectar();
            List<Department> deptos = new List<Department>();

            /*string query = @"SELECT Depto.idDepto, Depto.name, Depto.clabe
                            FROM Depto
                            INNER join ProfileTagDepto ON ProfileTagDepto.idDepto = Depto.idDepto
                            INNER join Tag ON Tag.idTag = ProfileTagDepto.idTag
                            WHERE (ProfileTagDepto.idProfile = 1 and Tag.tagClabe = @tagName) and ProfileTagDepto.idParent = 0
                            Order By Tag.tagName, Depto.name;";*/

            string query = @"SELECT Depto.idDepto, Depto.name, Depto.clabe
                            FROM Depto
                            INNER JOIN ProfileTagDepto ON Depto.idDepto = ProfileTagDepto.idDepto
                            INNER JOIN Tag ON ProfileTagDepto.idTag = Tag.idTag
                            WHERE (ProfileTagDepto.idParent = 0 and Tag.tagClabe=@tagName) and ProfileTagDepto.idProfile = @idProfile";

            SqlCommand com = new SqlCommand(query, con);
            com.Parameters.Add("@tagName", SqlDbType.VarChar);
            com.Parameters.Add("@idProfile", SqlDbType.Int);
            com.Parameters["@tagName"].Value = brand;
            com.Parameters["@idProfile"].Value = idProfile;
            con.Open();
            SqlDataReader rows = com.ExecuteReader();

            while (rows.Read())
            {
                Department depto = new Department()
                {

                    IdDepto = int.Parse(rows["idDepto"].ToString()),
                    Name = rows["name"].ToString(),
                    Clabe = rows["clabe"].ToString(),

                };
                deptos.Add(depto);
            }
            return deptos;

        }


       

    }
}