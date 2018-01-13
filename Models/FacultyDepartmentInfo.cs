using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace PRMS.Models
{
    public class FacultyDepartmentInfo
    {

        public List<String> getFaculty()
        {
            List<String> faculties = new List<String>();
            string connectionString = WebConfigurationManager.ConnectionStrings["prmsDbConnectionString"].ConnectionString;
            string query = string.Format("SELECT *  FROM [prms].[dbo].[faculties]");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        faculties.Add(reader.GetString(1));
                    }
                }
                con.Close();
            }
            return faculties;

        }



        public List<String> getDepartment(String faculty)
        {
            List<String> departments = new List<String>();


            string connectionString = WebConfigurationManager.ConnectionStrings["prmsDbConnectionString"].ConnectionString;
            string query = string.Format("SELECT *  FROM [prms].[dbo].[departments] WHERE faculty='" + faculty + "'");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        departments.Add(reader.GetString(1));
                    }
                }
                con.Close();
            }

            return departments;
        }


        public  string createRandomPassword()
        {
            int PasswordLength = 8;
            string _allowedChars = "0123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
            Random randNum = new Random();
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;
            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }


    }
}