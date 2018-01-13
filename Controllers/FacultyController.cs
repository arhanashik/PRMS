using PRMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PRMS.Controllers
{
    public class FacultyController : Controller
    {

        private ProjectDB db = new ProjectDB(); 
        //
        // GET: /Faculty/
        public ActionResult Index()
        {
            return RedirectToAction("AddFaculty", "Faculty");
        }


        public ActionResult AddFaculty()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddFaculty([Bind(Include = "FacultyName,ShortForm")] Faculty faculty)
        {
            CreateDatabaseAndTable cdt = new CreateDatabaseAndTable(faculty.ShortForm);
            cdt.CreateDatabase(faculty.ShortForm);

            db.Faculty.Add(faculty);
            db.SaveChanges();

            return View();
        }

        public ActionResult ManageFaculty()
        {
            return View(db.Faculty.ToList());

        }
        public ActionResult EditFaculty(int? id)
        {

            //  return View("~/Views/Admin/EditTeacher.cshtml");
            return RedirectToAction("EditFaculties", "Faculty", new { ind = id });

           // return View(db.Faculty.Find(id));
        }
        public ActionResult EditFaculties(int? ind)
        {
            if (ind == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Faculty faculty = db.Faculty.Find(ind);
            if (faculty == null)
            {
                return HttpNotFound();
            }


            return View(faculty);
        }

        [HttpPost]
        public ActionResult EditFaculties([Bind(Include = "id,FacultyName,ShortForm")] Faculty faculty)
        {
            
            if (ModelState.IsValid)
            {
                db.Entry(faculty).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("ManageFaculty", "Faculty");
        }


        public ActionResult DeleteFaculty(int? id)
        {
            Faculty faculty = db.Faculty.Find(id);
            db.Faculty.Remove(faculty);
            db.SaveChanges();
            return RedirectToAction("ManageFaculty", "Faculty");
        }
    
	}
}