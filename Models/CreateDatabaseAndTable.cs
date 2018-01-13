using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PRMS.Models
{
    public class CreateDatabaseAndTable
    {
        SqlConnection myConn = new SqlConnection();

        public CreateDatabaseAndTable(String faculty)
        {
              myConn = new SqlConnection("Server=OVI-PC\\SQLEXPRESS;Integrated security=true;Initial Catalog=" + faculty + ";");
        }
      
        public void CreateDatabase(String faculty)
        {

            String str;
            SqlConnection Conn = new SqlConnection("Server=OVI-PC\\SQLEXPRESS;Integrated security=SSPI;database=master;");
            str = "CREATE DATABASE " + faculty;

            SqlCommand myCommand = new SqlCommand(str, Conn);
            try
            {
                Conn.Open();
                myCommand.ExecuteNonQuery();
                
            }
            catch (System.Exception ex)
            {
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
            CreatejanJuneTable(faculty);
        }
        public void CreatejanJuneTable(String database)
        {

            String sql = "CREATE TABLE JanEnrollment (id INTEGER  PRIMARY KEY IDENTITY, Name nvarchar(MAX), StudentId int, RegNo int,Session nvarchar(MAX) ,Semester int)"
                        + "CREATE TABLE JuneEnrollment (id INTEGER  PRIMARY KEY IDENTITY, Name nvarchar(MAX), StudentId int, RegNo int,Session nvarchar(MAX) ,Semester int)"
                        + "CREATE TABLE CurrentSemester (id INTEGER  PRIMARY KEY IDENTITY, StudentId int ,Semester int)";
                      
            
            // "CREATE TABLE StudentInfo (id INTEGER  PRIMARY KEY IDENTITY, Name nvarchar(MAX),Fathers_name nvarchar(MAX),Mothers_name nvarchar(MAX), StudentId int, RegNo int,Session nvarchar(MAX), Regularity nvarchar(MAX), Hall nvarchar(MAX), Sex nvarchar(MAX), Nationality nvarchar(MAX), Religion nvarchar(MAX), Phone nvarchar(MAX), Email nvarchar(MAX), BloodGroup nvarchar(MAX))";



            SqlCommand myCommand = new SqlCommand(sql, myConn);
            try
            {
                myConn.Open();
                myCommand.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }

           // AddCourse("CIT_111",1);
        }
        public  String AddCourse(String courseCode, int semester)
           {
              String sql;
            if (semester % 2 == 0)
            {
                sql = "ALTER TABLE JuneEnrollment add  [" + courseCode + "] BIT default 'FALSE'";
            }
            else
            {
                sql = "ALTER TABLE JanEnrollment add  [" + courseCode + "] BIT default 'FALSE'";
            }
                
            SqlCommand myCommand = new SqlCommand(sql, myConn);
            try
            {
                myConn.Open();
                myCommand.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
           string message= CreateCourseTable(courseCode);
           return message;
        }

        public string CreateCourseTable(String courseCode )
        {
                String sql;
             

            sql = "CREATE TABLE " + courseCode + " (StudentId INTEGER, RegNo float, Mid float, Attendence float, Assignment float, Final float)";


            SqlCommand myCommand = new SqlCommand(sql, myConn);
            try
            {
                myConn.Open();
                myCommand.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                myConn.Close();
                return courseCode+"Course Already Exist..";
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                   
                }
               
            }
            return courseCode+" Course Added Successfulley..";
        }

        

    }
}