using PRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PRMS.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/
        public ActionResult Index()
        {

            dynamic sampleObject = new DynamicDictionary();

            sampleObject.Name = "A";
            sampleObject.StudentId = 1234;

            sampleObject.RegNo = 1234;
            sampleObject.Session = "2013-2014";
            sampleObject.Semester = 1;



            CRUD crud = new CRUD("CSE");
          ViewBag.c= crud.InsertCurrentSemester(sampleObject);

            ViewBag.sample = sampleObject;
            return View();
        }
    }
}