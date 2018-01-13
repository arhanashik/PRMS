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
    public class DepartmentController : Controller
    {

        private ProjectDB db = new ProjectDB();
        //
        // GET: /Faculty/
        public ActionResult Index()
        {
            return RedirectToAction("AddDepartment", "Department");
        }


        public ActionResult AddDepartment()
        {
            ViewBag.faculties = db.Faculty.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult AddDepartment([Bind(Include = "DepartmentName,ShortForm,Faculty")] Department department)
        {
            db.Department.Add(department);
            db.SaveChanges();
            ViewBag.faculties = db.Faculty.ToList();
            return View();
        }

        public ActionResult ManageDepartment()
        {
            ViewBag.faculties = db.Faculty.ToList();
            return View(db.Department.ToList());

        }
        public ActionResult EditDepartment(int? id)
        {

            //  return View("~/Views/Admin/EditTeacher.cshtml");
            return RedirectToAction("EditDepartments", "Department", new { ind = id });

            // return View(db.Faculty.Find(id));
        }
        public ActionResult EditDepartments(int? ind)
        {
            if (ind == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Department.Find(ind);
            if (department == null)
            {
                return HttpNotFound();
            }

            ViewBag.faculties = db.Faculty.ToList();
            return View(department);
        }

        [HttpPost]
        public ActionResult EditDepartments([Bind(Include = "id,DepartmentName,ShortForm,Faculty")] Department department)
        {

            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("ManageDepartment", "Department");
        }


        public ActionResult DeleteDepartment(int? id)
        {
            Department department = db.Department.Find(id);
            db.Department.Remove(department);
            db.SaveChanges();
            return RedirectToAction("ManageDepartment", "Department");
        }

    }
}