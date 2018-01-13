using PRMS.Models;
using PSTU_RESULT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PRMS.Controllers
{
    public class HomeController : Controller
    {

        private ProjectDB db = new ProjectDB(); 
        public ActionResult Index(string usertype,string username, string password)
        {
            try { 
           username= username.Replace(" ", String.Empty);
             password=   password.Replace(" ", String.Empty);
            }
            catch (NullReferenceException e)
            {

            }

            if (username != null && password != null)
            {   
                if (usertype.Equals("admin"))
                {

                    Admin admin = db.Admin.Where(a => a.Username == username && a.Password == password).FirstOrDefault();


                    if (admin !=null)
                    {
                        HttpContext.Session["AdminProfile"] = admin;
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        ViewBag.Message = "Invalid Username or Password!";
                    }
                    
               
               }
                else if (usertype.Equals("teacher"))
                {    
                    Teacher teacher = db.Teacher.Where(a => a.Email == username && a.Password == password).FirstOrDefault();


                    if (teacher != null)
                    {
                        HttpContext.Session["TeacherProfile"] = teacher;
                        return RedirectToAction("Index", "Teacher");
                    }
                    else
                    {
                        ViewBag.Message = "Invalid Username or Password!";
                    }
                    
                }
                

                }

            return View();
        }

     
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}