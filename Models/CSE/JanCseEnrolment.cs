using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PRMS.Models.CSE
{
    public class JanCseEnrolment
    {

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public String Name { get; set; }
        public int StudentId { get; set; }
        public int RegNo { get; set; }
        public String Session { get; set; }
        public int Semester { get; set; }


        //7th
        [DefaultValue("false")]
        public Boolean CSE_410 { get; set; }
        [DefaultValue("false")]
        public Boolean CSE_412 { get; set; }
        [DefaultValue("false")]
        public Boolean CCE_411 { get; set; }
        [DefaultValue("false")]
        public Boolean CCE_413 { get; set; }
        [DefaultValue("false")]
        public Boolean CCE_415 { get; set; }
        [DefaultValue("false")]
        public Boolean CCE_416 { get; set; }
        [DefaultValue("false")]
        public Boolean CCE_417 { get; set; }
        [DefaultValue("false")]
        public Boolean CIT_411 { get; set; }
        [DefaultValue("false")]
        public Boolean CIT_412 { get; set; }
         



        //5th  Boolean               
        [DefaultValue("false")]
        public Boolean CIT_311 { get; set; }
        [DefaultValue("false")]
        public Boolean CIT_312 { get; set; }
        [DefaultValue("false")]
        public Boolean CIT_313 { get; set; }
        [DefaultValue("false")]
        public Boolean CIT_315 { get; set; }
        [DefaultValue("false")]
        public Boolean CIT_316 { get; set; }
        [DefaultValue("false")]
        public Boolean CCE_310 { get; set; }
        [DefaultValue("false")]
        public Boolean CCE_311 { get; set; }
        [DefaultValue("false")]
        public Boolean CCE_312 { get; set; }
        [DefaultValue("false")]
        public Boolean CCE_313 { get; set; }
        [DefaultValue("false")]
        public Boolean CCE_314 { get; set; }
        


        //3rd  Boolean                              
        [DefaultValue("false")]
        public Boolean CIT_211 { get; set; }
        [DefaultValue("false")]
        public Boolean CIT_212 { get; set; }
        [DefaultValue("false")]
        public Boolean CIT_213 { get; set; }
        [DefaultValue("false")]
        public Boolean CCE_212 { get; set; }
        [DefaultValue("false")]
        public Boolean MAT_211 { get; set; }
        [DefaultValue("false")]
        public Boolean EEE_211 { get; set; }
        [DefaultValue("false")]
        public Boolean EEE_212 { get; set; }
        [DefaultValue("false")]
        public Boolean AIS_211 { get; set; }
        
        //1st  Boolean              


        [DefaultValue("false")]
        public Boolean PHY_111 { get; set; }
        [DefaultValue("false")]
        public Boolean PHY_112 { get; set; }
        [DefaultValue("false")]
        public Boolean CHE_111 { get; set; }
        [DefaultValue("false")]
        public Boolean CHE_112 { get; set; }
        [DefaultValue("false")]
        public Boolean MAT_111 { get; set; }
        [DefaultValue("false")]
        public Boolean EEE_111 { get; set; }
        [DefaultValue("false")]
        public Boolean EEE_112 { get; set; }
        [DefaultValue("false")]
        public Boolean CIT_111 { get; set; }
        [DefaultValue("false")]
        public Boolean CIT_112 { get; set; }
        [DefaultValue("false")]
        public Boolean CCE_112 { get; set; }

    }
}