using OfficeOpenXml;
using PRMS.Models;
using PRMS.Models.CSE;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace PRMS.Controllers
{
    public class EnrollmentController : Controller
    {
        private CseDbContext csedb = new CseDbContext();
        private ProjectDB db = new ProjectDB();
        //
        // GET: /Enrollment/
        public ActionResult Index()
        {

            ViewBag.faculties = db.Faculty.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Index(String faculty, int semester)
        {

            String fileName = faculty + "_Enroll_" + semester + ".xlsx";

            Server.MapPath("~/App_Data/CSE/" + fileName);

            return RedirectToAction("UploadEnrollment", "Enrollment", new { faculty = faculty, semester = semester });
        }



        public ActionResult Download(String faculty, int semester)
        {



            ViewBag.faculties = db.Faculty.ToList();
            return View();
        }


        public ActionResult UploadEnrollment(String faculty, int semester)
        {

            String sem = SemesterName(semester);


            ViewBag.semester = semester;
            ViewBag.faculty = faculty;
            return View();
        }


        [HttpPost]
        public ActionResult UploadEnrollment(String faculty, int semester, HttpPostedFileBase postedFile)
        {
            String fileName;
            if (faculty != null && postedFile != null)
            {

                if ((postedFile.ContentLength > 0) && !string.IsNullOrEmpty(postedFile.FileName))
                {
                    //string fileContentType = Path.GetExtension(file.FileName).ToLower();
                    fileName = Path.GetFileName(postedFile.FileName);

                    if (fileName.ToLower().Contains(".xls") || fileName.ToLower().Contains(".xlsx"))
                    {

                        byte[] fileBytes = new byte[postedFile.ContentLength];

                        var data = postedFile.InputStream.Read(fileBytes, 0, Convert.ToInt32(postedFile.ContentLength));
                        using (var package = new ExcelPackage(postedFile.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;

                            String property;
                            Boolean ck=false;

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                IDictionary<string, object> dict = new Dictionary<string, object>();
                          

                                dynamic enrollment = new DynamicDictionary();
                            //    List<KeyValuePair<string, object>> enroll = new List<KeyValuePair<string, object>>();

                                for (int i = 1; i <= noOfCol; i++)
                                {
                                      
                                    property  = workSheet.Cells[1, i].Value.ToString();

                                    if (i == 1 || i == 4)
                                    {
                                        dict.Add(new KeyValuePair<string, object>(property, workSheet.Cells[rowIterator, i].Value));  
                                    }
                                    else if (i == 2 || i == 3 || i == 5)
                                    {
                                         dict.Add(new KeyValuePair<string, object>(property,Convert.ToInt32( workSheet.Cells[rowIterator, i].Value)));  
                                      
                                    }
                                    else
                                    {
                                          dict.Add(new KeyValuePair<string, object>(property, Convert.ToBoolean(workSheet.Cells[rowIterator, i].Value)));  
                                      
                                    }
                                      
                                }

                                if (faculty.Equals("selected"))
                                {
                                    ViewBag.Message = "Select A Faculty";
                                }
                                else
                                {
                                    CRUD crd = new CRUD(faculty);
                                   ck=  crd.InsertEnrollment(dict);

                                  if (ck == false)
                                  {
                                      ViewBag.ck = ck;
                                      break;
                                  }
                                 
                                }

                           }

                            ViewBag.ck = ck;

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


            ViewBag.semester = semester;
            ViewBag.faculty = faculty;
            return View();
        }


        public ActionResult ManageEnrollment(string msg)
        {
            ViewBag.msg = msg;
            ViewBag.faculties = db.Faculty.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult ManageEnrollment(string faculty, int semester, int id)
        {


               IDictionary<string, object> dict = new Dictionary<string, object>();

            CRUD crud=new CRUD(faculty);
            dict = crud.SelectEnrollment(semester, id);

             

            if (dict == null)
            {
                ViewBag.faculties = db.Faculty.ToList();
                ViewBag.message = "Student no found..!";
                return View("ManageEnrollment");
            }
            else
            {
                ViewBag.en = dict;
                ViewBag.faculty = faculty;
                ViewBag.semester = semester;

                return View("EditEnrollment");
            }
        }



        [HttpPost]
        public ActionResult EditEnrollment(String faculty, String name, int studentid, int regno, int semester, String session, String[] subjectlist)
        {



            IDictionary<string, object> dict = new Dictionary<string, object>();
             
            CRUD crud = new CRUD(faculty);
       //     dict = 
            dict = crud.SelectEnrollment(semester, studentid);

            List<string> keys = new List<string>(dict.Keys);
            
            foreach (string item in keys)
            {
                if (dict[item].Equals(true))
                {
                  dict[item] = false;
                }

            }
              
            foreach (String s in subjectlist)
            {
                dict[s]= true; 
            }
            crud.UpdateEnrollment(dict);

            dict = crud.SelectEnrollment(semester, studentid);

            ViewBag.en = dict;
            ViewBag.faculty = faculty;
            ViewBag.semester = semester;
            ViewBag.message = "Updated Successfully..!";
            return View();
        }
       
        public ActionResult Delete(int id, int semester, String faculty)
        {
            IDictionary<string, object> dict = new Dictionary<string, object>();

            CRUD crud = new CRUD(faculty);
            dict = crud.SelectEnrollment(semester, id);
           Boolean ck=  crud.DeleteEnrollment( id, semester);

           if (ck == true)
           {
              string message = "Student Enrollment Delete Successfully..!";
               return RedirectToAction("ManageEnrollment", "Enrollment", new  { msg =message });

           }
           else
           {
               ViewBag.message = "Some Problem when Delete Student Enrollment..!";
               ViewBag.en = dict;
               ViewBag.faculty = faculty;
               ViewBag.semester = semester;
               return View("EditEnrollment");

           }


        }



        private static string SemesterName(int semester)
        {
            String sem = semester.ToString();

            switch (semester)
            {
                case 1:
                    sem += "st ";
                    break;
                case 2:
                    sem += "nd ";
                    break;
                case 3:
                    sem += "rd ";
                    break;
                case 4:
                    sem += "th ";
                    break;
                case 5:
                    sem += "th ";
                    break;
                case 6:
                    sem += "th ";
                    break;
                case 7:
                    sem += "th ";
                    break;
                case 8:
                    sem += "th ";
                    break;


            }
            return sem;
        }



    }
}