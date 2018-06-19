using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PRMS.Models
{
    public class GetEnrollment
    {
        SqlConnection myConn = new SqlConnection();
        public GetEnrollment(string Faculty)
        {
            myConn = new SqlConnectionGenerator().FromFaculty(Faculty);

        }

        public List<EnrolledStudent> GetStudentEnrollment(string course_code, int semester)
        {
            List<EnrolledStudent> studentList = new List<EnrolledStudent>();

            string sql = "SELECT CurrentSemester.StudentId , RegNo FROM (";
            if (semester % 2 == 0)
            {
                sql += "JulEnrollment INNER JOIN CurrentSemester ON JulEnrollment.StudentId = CurrentSemester.StudentId and  JulEnrollment.Semester = CurrentSemester.Semester)";
            }
            else
            {
                sql += "JanEnrollment INNER JOIN CurrentSemester ON JanEnrollment.StudentId = CurrentSemester.StudentId and  JanEnrollment.Semester = CurrentSemester.Semester)";

            }
            sql += " WHERE " + course_code + "='true'";

            SqlCommand myCommand = new SqlCommand(sql, myConn);

            try
            {
                myConn.Open();
                SqlDataReader oReader = myCommand.ExecuteReader();

                while (oReader.Read())
                {
                    studentList.Add(new EnrolledStudent { StudentId = Convert.ToInt32(oReader["StudentId"]), RegNo = Convert.ToInt32(oReader["RegNo"]) });
                }
            }
            catch (System.Exception ex)
            {
                studentList = null;
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }


            return studentList;
        }

        public static List<Course> GetEnrollCourses(string Faculty, string Session, int Semester)
        {
            ProjectDB db = new ProjectDB();
            List<Course> Courses = new List<Course>();

            string tableName = (Semester % 2 == 0) ? "JulEnrollment" : "JanEnrollment";
            SqlConnection myConn = new SqlConnectionGenerator().FromFaculty(Faculty);
            string SQLCmd = "select column_name from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + tableName + "'";
            // * can be column_name, data_type, column_default, is_nullable
            SqlCommand myCommand = new SqlCommand(SQLCmd, myConn);
            try
            {
                myConn.Open();
                SqlDataReader oReader = myCommand.ExecuteReader();
                while (oReader.Read())
                {
                    string key = Convert.ToString(oReader.GetValue(0));
                    if (!(key.Equals("id") || key.Equals("Name") || key.Equals("StudentId") || key.Equals("RegNo") || key.Equals("Session") || key.Equals("Semester")))
                    {
                        Course Course = db.Courses.Where(c => c.Course_code == key).FirstOrDefault();
                        if (Course != null)
                        {
                            int ObSem = Course.GetSemesterFromCourse(Course.Course_code);
                            if(ObSem == Semester) Courses.Add(Course);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                
            }
            finally
            {
                if (myConn.State == ConnectionState.Open) myConn.Close();
            }

            return Courses;
        }
    }
}