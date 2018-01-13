using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PRMS.Models
{
    public class Activity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int id { set; get; }
        public string Name { set; get; }
        public string Designation { set; get; }
        public string UserId { set; get; }
        public string Time { set; get; }
        public string Message { set; get; }
        public string Content { set; get; }
        public string FilePath { set; get; }
    }
}