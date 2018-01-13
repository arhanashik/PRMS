using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PRMS.Models
{
    public class Marks
    {
        public int Id { set; get; }
        public int Reg { set; get; }
        public float Mid { set; get; }
        public float Attendence { set; get; }
        public float Assignment { set; get; }
        public float Final { set; get; }
    }
}