using PRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data.Entity;
using System.IO;
using OfficeOpenXml;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;

namespace PRMS.Controllers
{
    public class TeacherController : Controller
    {
        //
        // GET: /Teacher/

        private ProjectDB db = new ProjectDB();
        public ActionResult Index()
        {
            Teacher teacher = HttpContext.Session["TeacherProfile"] as Teacher;
            if (teacher != null)
            {
                ViewBag.Message = "Welcome " + teacher.Name + "!";
            }
            //  ViewBag.Email = teacher.Email;   
            return View();
        }

        public ActionResult History(int? id)
        {
            Teacher teacher = HttpContext.Session["TeacherProfile"] as Teacher;
            if (teacher == null) teacher = db.Teacher.Find(id);

            string tId = teacher.TeacherID.ToString();
            List<Activity> activities = db.Activity.Where(b => b.UserId == tId).ToList();
            ViewBag.Activities = activities;

            return View(teacher);

        }

        public ActionResult ChangePassword(int? id)
        {
            Teacher teacher = db.Teacher.Find(id);
            return View(teacher);
        }

        [HttpPost]
        public ActionResult ChangePassword(int? TeacherID, String oldpassword, String newpassword, String retypenewpassword)
        {
            Teacher teacher = db.Teacher.Find(TeacherID);

            if (newpassword.Equals(retypenewpassword))
            {

                if (teacher.PasswordChanged == true)
                {
                    oldpassword = new EncryptionDectryption().Encryptdata(oldpassword);
                }

                if (teacher.Password.Equals(oldpassword))
                {
                    teacher.Password = new EncryptionDectryption().Encryptdata(newpassword);
                    teacher.PasswordChanged = true;
                    db.Entry(teacher).State = EntityState.Modified;
                    db.SaveChanges();

                    ViewBag.Message = "Successfully password changed";
                    //return RedirectToAction("index", "Teacher");
                    return View(teacher);
                }
                else
                {
                    ViewBag.Message = "Invalid Old Password..!";

                    return View(teacher);
                }
            }
            else
            {
                ViewBag.Message = "Miss Match new Password..!";
                return View(teacher);
            }
        }

        public ActionResult UploadMark()
        {

            //   ViewBag.Name = teacher.Name;
            //  ViewBag.Email = teacher.Email; 
            ViewBag.faculties = db.Faculty.ToList();
            ViewBag.all_session = db.AllSession.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult UploadMark(string faculty, string session, string semester, string course_code, HttpPostedFileBase file, string msg)
        {
            if (faculty != null && session != null && semester != null && course_code != null && file != null)
            {
                course_code = course_code.Trim().Replace(" ", "_").ToUpper();
                string fileName = "", targetFileName = course_code + "_" + session.Replace("-", "_");
                string content = course_code;

                if ((file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    //string fileContentType = Path.GetExtension(file.FileName).ToLower();
                    fileName = Path.GetFileName(file.FileName);

                    if (fileName.ToLower().Contains(".xls") || fileName.ToLower().Contains(".xlsx"))
                    {
                        List<Marks> marks = readMarksFromExcel(file);
                        try
                        {
                            string marksConnStr = ConfigurationManager.ConnectionStrings["cseRmsDbConnectionString"].ConnectionString;
                            SqlConnection con = new SqlConnection(marksConnStr);
                            con.Open();

                            int res = -1;

                            foreach (Marks mark in marks)
                            {
                                content += "," + mark.Id;
                                SqlCommand cmd = new SqlCommand("SELECT * FROM " + course_code + " WHERE Id=" + mark.Id, con);
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        cmd = new SqlCommand("UPDATE " + course_code + " SET Final=" + mark.Final + " WHERE Id=" + mark.Id, con);
                                    }
                                    else
                                    {
                                        cmd = new SqlCommand("INSERT INTO " + course_code + " VALUES(@Id, @Reg, @Mid, @Attendence, @Assignment, @Final)", con);

                                        cmd.Parameters.AddWithValue("@Id", mark.Id);
                                        cmd.Parameters.AddWithValue("@Reg", mark.Reg);
                                        cmd.Parameters.AddWithValue("@Mid", mark.Mid);
                                        cmd.Parameters.AddWithValue("@Attendence", mark.Attendence);
                                        cmd.Parameters.AddWithValue("@Assignment", mark.Assignment);
                                        cmd.Parameters.AddWithValue("@Final", mark.Final);
                                    }
                                }

                                res = cmd.ExecuteNonQuery();
                            }
                            con.Close();

                            ViewBag.Marks = marks;
                            ViewBag.Course = course_code;
                            ViewBag.Message = (res <= 0) ? "Data upload failed" : "Data uploaded successfully!";
                        }
                        catch (SqlException ex)
                        {
                            ViewBag.Error = ex.ToString();
                        }

                        try
                        {
                            //targetFileName += "_" + DateTime.Now.ToString().Replace("/", "_").Replace(" ", "_").Replace(":", "_") + ".xlsx";
                            targetFileName += ".xlsx";
                            string path = Path.Combine(Server.MapPath("~/App_Data/Marks/"), targetFileName);
                            file.SaveAs(path);
                            ViewBag.FileInfo = targetFileName + " uploaded successfully!";

                            Teacher teacher = HttpContext.Session["TeacherProfile"] as Teacher;
                            Activity activity = new Activity();
                            activity.Name = teacher.Name;
                            activity.Designation = teacher.Department;
                            activity.UserId = teacher.TeacherID.ToString();
                            activity.Time = DateTime.Now.ToString();
                            msg = "Marks uploaded for: " + course_code + ", " + session + ".\n" + msg;
                            activity.Message = msg;
                            activity.Content = content;
                            activity.FilePath = path;

                            db.Activity.Add(activity);
                            db.SaveChanges();

                        }
                        catch (Exception ex)
                        {
                            ViewBag.FileInfo = "ERROR:" + ex.Message.ToString();
                        }
                    }
                    else
                    {
                        //format not supported
                        ViewBag.Message = "File formate not supported. Only .xls or .xlsx is required.";
                    }
                }

            }
            else
            {
                ViewBag.Message = "Required field(s) missing.";
            }


            ViewBag.faculties = db.Faculty.ToList();
            ViewBag.all_session = db.AllSession.ToList();

            return View();
        }
        public ActionResult ManageMarks()
        {
            ViewBag.faculties = db.Faculty.ToList();
            ViewBag.all_session = db.AllSession.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult ManageMarks(string Faculty, string session, string semester, string course_code)
        {
            if (course_code == null || session == null) return null;

            int batchId = Convert.ToInt32(session.Substring(2, 2)); //startIndex, Length
            int startRange = batchId * 100000;
            int endRange = ((batchId + 1) * 100000) - 1;

            string marksConnStr = ConfigurationManager.ConnectionStrings["cseRmsDbConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(marksConnStr);
            con.Open();

            List<Marks> marks = new List<Marks>();
            SqlCommand cmd = new SqlCommand("SELECT * FROM " + course_code + " WHERE Id BETWEEN " + startRange + " AND " + endRange + ";", con);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Marks mark = new Marks();
                        mark.Id = reader.GetInt32(0);
                        mark.Reg = reader.GetInt32(1);
                        mark.Mid = reader.GetFloat(2);
                        mark.Attendence = reader.GetFloat(3);
                        mark.Assignment = reader.GetFloat(4);
                        mark.Final = reader.GetFloat(5);
                        marks.Add(mark);

                        ViewBag.Message = "Marks showing for " + course_code + ", Session: " + session;
                    }
                    reader.Close();
                }
                else
                {
                    ViewBag.Message = "No data found for " + course_code + ", Session: " + session;
                }
            }
            con.Close();

            ViewBag.faculties = db.Faculty.ToList();
            ViewBag.all_session = db.AllSession.ToList();
            ViewBag.Marks = marks;
            ViewBag.Course = course_code;

            return View();
        }

        public ActionResult EditMark(int? id, string course_code)
        {
            return RedirectToAction("ChangeMark", "Teacher", new { nid = id, ncourse_code = course_code });
        }

        public ActionResult ChangeMark(int? nid, string ncourse_code)
        {
            if (nid == null || ncourse_code == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string marksConnStr = ConfigurationManager.ConnectionStrings["cseRmsDbConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(marksConnStr);
            con.Open();

            Marks mark = new Marks();
            SqlCommand cmd = new SqlCommand("SELECT * FROM " + ncourse_code + " WHERE Id=" + nid, con);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    mark.Id = Convert.ToInt32(reader.GetValue(0));
                    mark.Reg = Convert.ToInt32(reader.GetValue(1));
                    mark.Mid = Convert.ToSingle(reader.GetValue(2));
                    mark.Attendence = Convert.ToSingle(reader.GetValue(3));
                    mark.Assignment = Convert.ToSingle(reader.GetValue(4));
                    mark.Final = Convert.ToSingle(reader.GetValue(5));
                    reader.Close();
                    ViewBag.Mark = mark;
                    ViewBag.Course_code = ncourse_code;
                }
                else
                {
                    ViewBag.Message = "Record not found.";
                }
            }
            con.Close();
            if (mark == null) return HttpNotFound();

            return View();
        }

        [HttpPost]
        public ActionResult ChangeMark(string course_code, [Bind(Include = "Id,Reg,Mid,Attendence,Assignment,Final")] Marks mark)
        {
            if (course_code == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            string marksConnStr = ConfigurationManager.ConnectionStrings["cseRmsDbConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(marksConnStr);

            int res = -1;
            SqlCommand cmd = new SqlCommand("UPDATE " + course_code + " SET Mid=@mid, Attendence=@attendence, Assignment=@assignment, Final=@final WHERE Id=@id;", con);

            cmd.Parameters.AddWithValue("@mid", mark.Mid);
            cmd.Parameters.AddWithValue("@attendence", mark.Attendence);
            cmd.Parameters.AddWithValue("@assignment", mark.Assignment);
            cmd.Parameters.AddWithValue("@final", mark.Final);
            cmd.Parameters.AddWithValue("@id", mark.Id);
            try
            {
                con.Open();
                res = cmd.ExecuteNonQuery();
                ViewBag.Message = (res <= 0) ? "Data update failed" : "Data updated successfully!";
            }
            catch (SqlException ex)
            {
                ViewBag.Message = "Error: " + ex.ToString();
            }
            finally
            {
                con.Close();
            }

            ViewBag.Mark = mark;

            return View();
        }

        public ActionResult DeleteMark(int? id, string course_code)
        {
            string marksConnStr = ConfigurationManager.ConnectionStrings["cseRmsDbConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(marksConnStr);
            con.Open();

            int res = -1;
            SqlCommand cmd = new SqlCommand("SELECT * FROM " + course_code + " WHERE Id=" + id, con);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    reader.Close();
                    cmd = new SqlCommand("DELETE FROM " + course_code + " WHERE Id=" + id, con);
                    res = cmd.ExecuteNonQuery();
                    ViewBag.Message = "Successfuly deleted.";
                }
                else
                {
                    ViewBag.Message = "Record not found. Failed to delete.";
                }
            }
            con.Close();
            ViewBag.faculties = db.Faculty.ToList();
            ViewBag.all_session = db.AllSession.ToList();

            return RedirectToAction("UploadMark", "Teacher");
        }

        public JsonResult GetCourses(int semester, string faculty)
        {
            List<Course> courses = db.Courses.Where(b => b.Semester == semester && b.UnderFaculty == faculty).ToList();
            List<String> course_codes = new List<String>();
            foreach (Course course in courses)
            {
                course_codes.Add(course.Course_code);
            }
            return Json(course_codes, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangeHistory(int? id)
        {
            return RedirectToAction("ChangeExcelHistory", "Teacher", new { nid = id });
        }

        public ActionResult ChangeExcelHistory(int? nid)
        {
            if (nid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.Id = nid;
            return View();
        }

        [HttpPost]
        public ActionResult ChangeExcelHistory(int id, HttpPostedFileBase file)
        {
            Activity activity = db.Activity.Find(id);
            string[] Ids = activity.Content.Split(',');
            string content = Ids[0];

            if ((file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            {
                //string fileContentType = Path.GetExtension(file.FileName).ToLower();
                string fileName = Path.GetFileName(file.FileName);
                if (fileName.ToLower().Contains(".xls") || fileName.ToLower().Contains(".xlsx"))
                {
                    //delete old marks
                    string marksDeleteConnStr = ConfigurationManager.ConnectionStrings["cseRmsDbConnectionString"].ConnectionString;
                    Boolean isDeleted = DeleteEntries(Ids, marksDeleteConnStr);

                    //insert new marks
                    List<Marks> marks = readMarksFromExcel(file);
                    try
                    {
                        string marksConnStr = ConfigurationManager.ConnectionStrings["cseRmsDbConnectionString"].ConnectionString;
                        SqlConnection con = new SqlConnection(marksConnStr);
                        con.Open();

                        int res = -1;

                        foreach (Marks mark in marks)
                        {
                            content += "," + mark.Id;
                            SqlCommand cmd = new SqlCommand("SELECT * FROM " + Ids[0] + " WHERE Id=" + mark.Id, con);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    cmd = new SqlCommand("UPDATE " + Ids[0] + " SET Final=" + mark.Final + " WHERE Id=" + mark.Id, con);
                                }
                                else
                                {
                                    cmd = new SqlCommand("INSERT INTO " + Ids[0] + " VALUES(@Id, @Reg, @Mid, @Attendence, @Assignment, @Final)", con);

                                    cmd.Parameters.AddWithValue("@Id", mark.Id);
                                    cmd.Parameters.AddWithValue("@Reg", mark.Reg);
                                    cmd.Parameters.AddWithValue("@Mid", mark.Mid);
                                    cmd.Parameters.AddWithValue("@Attendence", mark.Attendence);
                                    cmd.Parameters.AddWithValue("@Assignment", mark.Assignment);
                                    cmd.Parameters.AddWithValue("@Final", mark.Final);
                                }
                            }

                            res = cmd.ExecuteNonQuery();
                        }
                        con.Close();

                        ViewBag.Message = (res <= 0) ? "Marks update failed" : "Marks updated successfully!";
                    }
                    catch (SqlException ex)
                    {
                        ViewBag.Error = ex.ToString();
                    }

                    try
                    {
                        file.SaveAs(activity.FilePath);

                        Activity newActivity = new Activity();
                        newActivity.Name = activity.Name;
                        newActivity.Designation = activity.Designation;
                        newActivity.UserId = activity.UserId;
                        newActivity.Time = DateTime.Now.ToString();
                        newActivity.Message = activity.Message.Replace("uploaded", "changed");
                        newActivity.Content = content;
                        newActivity.FilePath = activity.FilePath;

                        db.Activity.Add(newActivity);
                        activity.FilePath = null;
                        db.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        ViewBag.FileInfo = "ERROR:" + ex.Message.ToString();
                    }
                }
                else
                {
                    //format not supported
                    ViewBag.Message = "File formate not supported. Only .xls or .xlsx is required.";
                }
            }
            else
            {
                ViewBag.Message = "No input data!";
            }

            ViewBag.Id = id;

            return View();
        }

        public ActionResult ChangeMarksFromExcel(int? id)
        {
            Activity activity = db.Activity.Find(id);
            string[] Ids = activity.Content.Split(',');

            string marksDeleteConnStr = ConfigurationManager.ConnectionStrings["cseRmsDbConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(marksDeleteConnStr);
            con.Open();

            int res = -1;

            for (int i = 1; i < Ids.Length; i++)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM " + Ids[0] + " WHERE Id=@key;", con);
                    cmd.Parameters.AddWithValue("@key", Ids[i]);
                    res = cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    ViewBag.Message = "Error: " + ex.ToString();
                }
            }
            con.Close();

            return RedirectToAction("History", "Teacher");
        }

        public ActionResult DeleteMarksFromExcel(int? id)
        {
            Activity activity = db.Activity.Find(id);
            string[] Ids = activity.Content.Split(',');

            string marksDeleteConnStr = ConfigurationManager.ConnectionStrings["cseRmsDbConnectionString"].ConnectionString;

            Boolean isDeleted = DeleteEntries(Ids, marksDeleteConnStr);

            if (isDeleted)
            {
                activity.FilePath = null;

                Activity newActivity = new Activity();
                newActivity.Name = activity.Name;
                newActivity.Designation = activity.Designation;
                newActivity.UserId = activity.UserId;
                newActivity.Time = DateTime.Now.ToString();
                newActivity.Message = activity.Message.Replace("uploaded", "deleted");

                db.Activity.Add(newActivity);
                db.SaveChanges();
            }

            return RedirectToAction("History", "Teacher");
        }

        public Boolean DeleteEntries(string[] Ids, string connStr)
        {
            SqlConnection con = new SqlConnection(connStr);
            con.Open();

            int res = -1;

            for (int i = 1; i < Ids.Length; i++)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM " + Ids[0] + " WHERE Id=@key;", con);
                    cmd.Parameters.AddWithValue("@key", Ids[i]);
                    res = cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    ViewBag.Message = "Error: " + ex.ToString();
                    return false;
                }
            }
            con.Close();

            return true;
        }

        public List<Marks> readMarksFromExcel(HttpPostedFileBase file)
        {
            List<Marks> marks = new List<Marks>();
            //byte[] fileBytes = new byte[file.ContentLength];

            //var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
            using (var package = new ExcelPackage(file.InputStream))
            {
                var currentSheet = package.Workbook.Worksheets;
                var workSheet = currentSheet.First();
                var noOfCol = workSheet.Dimension.End.Column;
                var noOfRow = workSheet.Dimension.End.Row;

                try
                {
                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                    {
                        var mark = new Marks();
                        mark.Id = Convert.ToInt32(workSheet.Cells[rowIterator, 1].Value);
                        mark.Reg = Convert.ToInt32(workSheet.Cells[rowIterator, 2].Value);
                        mark.Mid = Convert.ToSingle(workSheet.Cells[rowIterator, 3].Value);
                        mark.Attendence = Convert.ToSingle(workSheet.Cells[rowIterator, 4].Value);
                        mark.Assignment = Convert.ToSingle(workSheet.Cells[rowIterator, 5].Value);
                        mark.Final = Convert.ToSingle(workSheet.Cells[rowIterator, 6].Value);

                        marks.Add(mark);
                    }
                }
                catch (SqlException ex)
                {
                    return null;
                }

            }

            return marks;
        }
    }
}