using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PRMS.Models.CSE
{
    public class CseEnrollment
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public String Name { get; set; }
        public int StudentId { get; set; }
        public int RegNo { get; set; }
        public String Session { get; set; }
        public int Semester { get; set; }



        //8th
        [DefaultValue("false")]
        public Boolean CSE_420 { get; set; }
        [DefaultValue("false")]
        public Boolean CSE_421 { get; set; }
        [DefaultValue("false")]
        public Boolean CCE_421 { get; set; }
        [DefaultValue("false")]
        public Boolean CCE_422 { get; set; }
        [DefaultValue("false")]
        public Boolean CIT_421 { get; set; }
        [DefaultValue("false")]
        public Boolean CIT_422 { get; set; }
        [DefaultValue("false")]
        public Boolean CIT_423 { get; set; }







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

        //6th                                 
        [DefaultValue("false")]
        public Boolean CIT_320 { get; set; }
        [DefaultValue("false")]
        public Boolean CIT_321 { get; set; }
        [DefaultValue("false")]
        public Boolean CIT_322 { get; set; }
        [DefaultValue("false")]
        public Boolean CIT_323 { get; set; }
        [DefaultValue("false")]
        public Boolean CIT_324 { get; set; }
        [DefaultValue("false")]
        public Boolean EEE_321 { get; set; }
        [DefaultValue("false")]
        public Boolean EEE_322 { get; set; }
        [DefaultValue("false")]
        public Boolean CCE_320 { get; set; }
        [DefaultValue("false")]
        public Boolean CCE_321 { get; set; }
        [DefaultValue("false")]
        public Boolean CCE_322 { get; set; }
        [DefaultValue("false")]
        public Boolean CCE_323 { get; set; }

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

        //4th  Boolean                          

        [DefaultValue("false")]
        public Boolean CCE_221 { get; set; }
        [DefaultValue("false")]
        public Boolean CCE_222 { get; set; }
        [DefaultValue("false")]
        public Boolean CCE_223 { get; set; }
        [DefaultValue("false")]
        public Boolean CCE_224 { get; set; }
        [DefaultValue("false")]
        public Boolean AES_221 { get; set; }
        [DefaultValue("false")]
        public Boolean MAT_221 { get; set; }
        [DefaultValue("false")]
        public Boolean CIT_220 { get; set; }
        [DefaultValue("false")]
        public Boolean CIT_221 { get; set; }
        [DefaultValue("false")]
        public Boolean CIT_222 { get; set; }
        [DefaultValue("false")]
        public Boolean CIT_224 { get; set; }

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

        //2nd  Boolean                       
        [DefaultValue("false")]
        public Boolean PHY_121 { get; set; }
        [DefaultValue("false")]
        public Boolean PHY_122 { get; set; }
        [DefaultValue("false")]
        public Boolean MAT_121 { get; set; }
        [DefaultValue("false")]
        public Boolean CIT_121 { get; set; }
        [DefaultValue("false")]
        public Boolean LCM_121 { get; set; }
        [DefaultValue("false")]
        public Boolean EEE_121 { get; set; }
        [DefaultValue("false")]
        public Boolean EEE_122 { get; set; }
        [DefaultValue("false")]
        public Boolean CCE_121 { get; set; }
        [DefaultValue("false")]
        public Boolean CCE_122 { get; set; }
        [DefaultValue("false")]
        public Boolean CCE_124 { get; set; }

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