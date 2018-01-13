using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PRMS.Models
{
    public class Course
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int id { set; get; }
        public string Course_code { get; set; }
        public string Course_title { get; set; }
        public string Credit_hour { get; set; }
        public int Semester { get; set; }
        public string UnderFaculty { get; set; }
    }
}