using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using OfficeOpenXml;
using System.Configuration;
using System.Data.SqlClient;
using PRMS.Models;
using System.Web.Configuration;
using System.Net;
using System.Data.Entity;
using System.Net.Mail;


namespace PRMS.Controllers
{
    public class AdminController : Controller
    {

        private ProjectDB db = new ProjectDB(); 
        
        //
        // GET: /Admin/
        public ActionResult Index(string username,string email)
        {
            ViewBag.username = username;
            ViewBag.email = email;
            return View();
        }
        public ActionResult AddTeacher(string fullanme, string emailid, string mobile, string faculty, string department)
        {                                  //     [Bind(Include="TeacherID,Name,Email,Mobile,Faculty,Department")] Teacher teacher
              
            ViewBag.faculties = db.Faculty.ToList();




                if(fullanme!=null & emailid!=null & mobile!=null & faculty !=null & department !=null ){

                    Teacher teacher = new Teacher(fullanme, emailid, mobile, faculty, department, new FacultyDepartmentInfo().createRandomPassword());
                    db.Teacher.Add(teacher);
                    db.SaveChanges();
                    SendMail(teacher);
                }

          


            return View();

        }
        public ActionResult TeachersList()
        {
               return View(db.Teacher.ToList());

        }
        public ActionResult Edit(int? id)
        {

          //  return View("~/Views/Admin/EditTeacher.cshtml");
           return RedirectToAction("EditTeacher", "Admin", new { ind = id });

        }
        public ActionResult EditTeacher(int? ind)
        {
            if (ind == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teacher.Find(ind);
            if (teacher == null)
            {
                return HttpNotFound();
            }

            FacultyDepartmentInfo facultyDeptInfo = new FacultyDepartmentInfo();
            ViewBag.faculties = facultyDeptInfo.getFaculty();
            return View(teacher);

        }
        [HttpPost]
        public ActionResult EditTeacher([Bind(Include = "TeacherID,Name,Email,Mobile,Faculty,Department,Password")] Teacher teacher)
        {
            Teacher tcr = db.Teacher.Find(teacher.TeacherID);
            db.Entry(tcr).State = EntityState.Detached;
                
                  if(tcr.PasswordChanged==true){
                      EncryptionDectryption sc = new EncryptionDectryption();
                       teacher.Password = sc.Encryptdata(teacher.Password);
                       teacher.PasswordChanged = true;
                  }


           if (teacher.Password.Equals(tcr.Password)) { 
           
            if (ModelState.IsValid)
            {                                        
                   db.Entry(teacher).State = EntityState.Modified;
                   db.SaveChanges();
            }
            
            return RedirectToAction("TeachersList", "Admin");
                  }
                  else
                  {
                      ViewBag.Message = "Please Put the correct password";
                      return View();
                  }

        }
        public ActionResult Delete(int? id)
        {
            Teacher teacher = db.Teacher.Find(id);
            db.Teacher.Remove(teacher);
            db.SaveChanges();
            return RedirectToAction("TeachersList", "Admin");
        }
        public ActionResult Detail(int? id)
        {

            //  return View("~/Views/Admin/EditTeacher.cshtml");
            return RedirectToAction("TeacherDetail", "Admin", new { ind = id });

        }
        public ActionResult TeacherDetail(int? ind)
        {
            if (ind == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teacher.Find(ind);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }
        public ActionResult AddSemesters()
        {
            return View();

        }

        public ActionResult AddSession()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddSession(string session, HttpPostedFileBase file)
        {
            if(session != null && file != null){
                string fileName = "", targetFileName = session.Trim().Replace("-", "_") + "_students";
                string content = "";

                if ((file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    fileName = Path.GetFileName(file.FileName);

                    if (fileName.ToLower().Contains(".xls") || fileName.ToLower().Contains(".xlsx"))
                    {
                        //insert new session into AllSession table
                        AllSession a = new AllSession();
                        a.Session = session;
                        db.AllSession.Add(a);
                        db.SaveChanges();

                        //read excel data
                        List<StudentInfo> stuInfo = readStuInfoFromExcel(file);
                        //insret student informations into StudentInfo table
                        foreach (StudentInfo info in stuInfo)
                        {
                            content += info.StudentId + ",";

                            db.StudentInfo.Add(info);
                            db.SaveChanges();

                            ViewBag.Message = "Session Created Successfully!";
                        }

                        //add this activity into Activity table
                        try
                        {
                            //targetFileName += "_" + DateTime.Now.ToString().Replace("/", "_").Replace(" ", "_").Replace(":", "_") + ".xlsx";
                            targetFileName += ".xlsx";
                            string path = Path.Combine(Server.MapPath("~/App_Data/Stu_Info/"), targetFileName);
                            file.SaveAs(path);
                            ViewBag.FileInfo = targetFileName + " uploaded successfully!";

                            Activity activity = new Activity();
                            activity.Name = "admin";
                            activity.Designation = "admin";
                            activity.UserId = "admin-1";
                            activity.Time = DateTime.Now.ToString();
                            activity.Message = "New Session created: " + session;
                            activity.Content = content;
                            activity.FilePath = path;

                            db.Activity.Add(activity);
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Error = "ERROR:" + ex.Message.ToString();
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
                    //empty file
                    ViewBag.Message = "Empty File.";
                }

            }
            else
            {
                ViewBag.Message = "Required field(s) missing.";
            }

            return View();

        }
        public ActionResult ManageSession()
        {
            return View();

        }
        public JsonResult GetDepartment(string faculty)
        {
            List<Department> dept = db.Department.Where(b => b.Faculty == faculty).ToList(); 
          //  db.Faculty.ToList();

            List<String> departments = new List<String>();

            foreach (Department dp in dept){
                
                    departments.Add(dp.ShortForm);
                
            }


            return Json(departments, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetHistory()
        {
            List<Activity> activity = db.Activity.ToList();
            ViewBag.History = activity;

            return View();

        }

        public List<StudentInfo> readStuInfoFromExcel(HttpPostedFileBase file)
        {
            List<StudentInfo> stuInfo = new List<StudentInfo>();
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
                        var info = new StudentInfo();
                        info.Name = workSheet.Cells[rowIterator, 1].Value.ToString();
                        info.StudentId = Convert.ToInt32(workSheet.Cells[rowIterator, 2].Value);
                        info.Reg = Convert.ToInt32(workSheet.Cells[rowIterator, 3].Value);
                        info.Faculty = workSheet.Cells[rowIterator, 4].Value.ToString();
                        info.Session = workSheet.Cells[rowIterator, 5].Value.ToString();
                        info.Regularity = workSheet.Cells[rowIterator, 6].Value.ToString();
                        info.Hall = workSheet.Cells[rowIterator, 7].Value.ToString();
                        info.Blood = workSheet.Cells[rowIterator, 8].Value.ToString();
                        info.Sex = workSheet.Cells[rowIterator, 9].Value.ToString();
                        info.Fathers_name = workSheet.Cells[rowIterator, 10].Value.ToString();
                        info.Mothers_name = workSheet.Cells[rowIterator, 11].Value.ToString();
                        info.Phone = workSheet.Cells[rowIterator, 12].Value.ToString();
                        info.Email = workSheet.Cells[rowIterator, 13].Value.ToString();
                        info.Nationality = workSheet.Cells[rowIterator, 14].Value.ToString();
                        info.Religion = workSheet.Cells[rowIterator, 15].Value.ToString();

                        stuInfo.Add(info);
                    }
                }
                catch (SqlException ex)
                {
                    return null;
                }

            }

            return stuInfo;
        }
      
  
          public static void SendMail(Teacher teacher) {
              string name = teacher.Name;
              string email = teacher.Email;







              string body = "<div style='border: medium solid White; width: 1000px; height: 700px;font-family: arial,sans-serif; font-size: 17px;'>";
              body += "<h3 style='background-color: blue; margin-top:0px;'>Admin Pstu</h3>";
              body += "<br />";
              body += "Dear " + teacher.Name + ",";
              body += "<br />";
              body += "<p>";
              body += "Now you are registered at result.pstu.ac.bd </p>";
              body += "<p>Username : "+teacher.Email+"</p>";
              body += "<p>Password : " + teacher.Password + "</p>";
              body += "<p>Please Change The password to Secure the System.</p>";
              body += "Thanks,";
              body += "<br />";
            
              body += "</div>";











                  try
                  {
                      MailMessage mail = new MailMessage();
                      mail.To.Add(email);
                      mail.From = new MailAddress("resultpstu@gmail.com");
                      mail.Subject = "Registration Password";
                  
                      mail.Body = body;
                      mail.IsBodyHtml = true;
                      SmtpClient smtp = new SmtpClient();
                      smtp.Host = "smtp.gmail.com";
                      smtp.Port = 587;
                      smtp.UseDefaultCredentials = false;
                      smtp.Credentials = new System.Net.NetworkCredential("resultpstu", "RoGaSeUqAhRiVnAt"); // Enter seders User name and password   
                      smtp.EnableSsl = true;
                      smtp.Send(mail);  
                      // WriteToFile("Email sent successfully to: " + name + " " + email);
                  }
                  catch (System.Net.Mail.SmtpException e)
                  {

                  }

            }
     
    
    
    
    }
    
    }