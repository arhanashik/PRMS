using PRMS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PRMS.Controllers
{
    public class CourseController : Controller
    {
        private ProjectDB db = new ProjectDB();
        //
        // GET: /Faculty/
        public ActionResult Index()
        {
            return RedirectToAction("AddCourse", "Course");
        }


        public ActionResult AddCourse()
        {
            ViewBag.faculties = db.Faculty.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult AddCourse([Bind(Include = "Course_code,Course_title,Credit_hour,Semester,UnderFaculty")] Course course)
        {

            CreateDatabaseAndTable cdt = new CreateDatabaseAndTable(course.UnderFaculty);
          string message=  cdt.AddCourse(course.Course_code,course.Semester);


            ViewBag.Message = message+" Under"
                   + course.UnderFaculty + " faculty.";

            //ViewBag.Message = course.Course_code + " added failed or already exists.";
            ViewBag.faculties = db.Faculty.ToList();

            return View();
        }

        public ActionResult ManageCourse()
        {
            ViewBag.faculties = db.Faculty.ToList();
            return View(db.Courses.ToList());

        }
        public ActionResult EditMe(int? id)
        {

            //  return View("~/Views/Admin/EditTeacher.cshtml");
            return RedirectToAction("EditCourse", "Course", new { ind = id });

            // return View(db.Faculty.Find(id));
        }
        public ActionResult EditCourse(int? ind)
        {
            if (ind == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(ind);
            if (course == null)
            {
                return HttpNotFound();
            }

            ViewBag.faculties = db.Faculty.ToList();
            return View(course);
        }

        [HttpPost]
        public ActionResult EditCourse([Bind(Include = "id,Course_code,Course_title,Credit_hour,Semester,UnderFaculty")] Course course)
        {

            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("ManageCourse", "Course");
        }


        public ActionResult DeleteCourse(int? id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("ManageCourse", "Course");
        }
    }
}