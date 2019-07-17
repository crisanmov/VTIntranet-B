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
    public class DeptoHelper
    {

        private SqlConnection con;

        private void Conectar()
        {
            string constr = ConfigurationManager.ConnectionStrings["DB_Entities"].ToString();
            con = new SqlConnection(constr);
        }

        public List<Area> GetArea(int idDepto, int idProfile)
        {
            Conectar();
            List<Area> areas = new List<Area>();

            string query = @"SELECT Depto.idDepto, Depto.name, ProfileTagDepto.idParent 
                            FROM Depto
                            INNER JOIN ProfileTagDepto on Depto.idDepto = ProfileTagDepto.idDepto
                            WHERE ProfileTagDepto.idParent=@idDepto and ProfileTagDepto.idProfile=@idProfile;";

            SqlCommand com = new SqlCommand(query, con);
            com.Parameters.Add("@idDepto", SqlDbType.Int);
            com.Parameters["@idDepto"].Value = idDepto;
            com.Parameters.Add("@idProfile", SqlDbType.Int);
            com.Parameters["@idProfile"].Value = idProfile;
            con.Open();
            SqlDataReader rows = com.ExecuteReader();

            while (rows.Read())
            {
                Area a = new Area()
                {
                    IdDepto = int.Parse(rows["idDepto"].ToString()),
                    IdParent = int.Parse(rows["idParent"].ToString()),
                    Name = rows["name"].ToString(),
                    State = false,
                };
                areas.Add(a);
            }

            con.Close();
            return areas;
        }

        public List<Generic> GetAreaDepto(int idDepto, int idProfile)
        {
            Conectar();
            List<Generic> areas = new List<Generic>();

            string query = @"SELECT Depto.idDepto, Depto.name
                            FROM Depto
                            INNER JOIN ProfileTagDepto on Depto.idDepto = ProfileTagDepto.idDepto
                            WHERE ProfileTagDepto.idParent = @idDepto and ProfileTagDepto.idProfile = @idProfile";

            SqlCommand com = new SqlCommand(query, con);
            com.Parameters.Add("@idDepto", SqlDbType.Int);
            com.Parameters.Add("@idProfile", SqlDbType.Int);
            com.Parameters["@idDepto"].Value = idDepto;
            com.Parameters["@idProfile"].Value = idProfile;
            con.Open();
            SqlDataReader rows = com.ExecuteReader();

            while (rows.Read())
            {
                Generic a = new Generic()
                {
                    Id = int.Parse(rows["idDepto"].ToString()),
                    Name = rows["name"].ToString(),
                    
                };
                areas.Add(a);
            }

            con.Close();
            return areas;
        }

        public List<Department> GetDepto(string brand, int idProfile)
        {
           Conectar();
           List<Department> deptos = new List<Department>();

           string query = @"SELECT Depto.idDepto, Depto.name, Depto.clabe
                            FROM Depto
                                INNER JOIN ProfileTagDepto ON Depto.idDepto = ProfileTagDepto.idDepto
                                INNER JOIN Tag ON ProfileTagDepto.idTag = Tag.idTag
                            WHERE (Tag.tagClabe = @tagName and ProfileTagDepto.idParent = 0) and ProfileTagDepto.idProfile = @idProfile
                            ORDER BY Depto.name";

           SqlCommand com = new SqlCommand(query, con);
            com.Parameters.Add("@tagName", SqlDbType.VarChar);
            com.Parameters.Add("@idProfile", SqlDbType.VarChar);
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


        public int DeleteProfileDepto(int idProfile)
        {
            Conectar();
            string query = @"DELETE 
                            FROM ProfileTagDepto
                            WHERE idProfile=@idProfile;";

            SqlCommand com = new SqlCommand(query, con);
            com.Parameters.Add("@idProfile", SqlDbType.Int);
            com.Parameters["@idProfile"].Value = idProfile;
            con.Open();
            SqlDataReader rows = com.ExecuteReader();
            con.Close();
            return 1;
        }

    }
}