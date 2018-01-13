using PRMS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

using System.Data.Entity;

namespace PSTU_RESULT.Models
{
    public class Login
    {


        private ProjectDB db = new ProjectDB();


        string connectionString = WebConfigurationManager.ConnectionStrings["prmsDbConnectionString"].ConnectionString;

        public Boolean LoginCheck(string username, string password)
        {
            string query = "Select * from [Login].[dbo].[adminInfo] where username = '" + username + "' and password = '" + password + "'";
            SqlConnection aSqlConnection = new SqlConnection(connectionString);
            try
            {
                aSqlConnection.Open();
                SqlCommand aCommand = new SqlCommand(query, aSqlConnection);

                SqlDataReader aReader = aCommand.ExecuteReader();

                if (aReader.Read())
                {
                    //aSqlConnection.Close();
                    return true;
                }

            }
            catch (Exception e)
            {
                e.ToString();
                return false;
            }
            finally {
                aSqlConnection.Close();
            }
            return false;

        }




        public Teacher TeacherLoginCheck(string username)
        {

            Teacher teacher = new Teacher();
               // Teacher teacher = db.Teacher.FindAsync
            try
            {
               teacher = db.Teacher.Single(p => p.Email == username);
            } catch(InvalidOperationException){

            }
            

            return teacher;

        }



    }
}