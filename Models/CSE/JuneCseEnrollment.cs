using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PRMS.Models.CSE
{
    public class JuneCseEnrollment
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

        //6th  Boolean                              

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

    }
}