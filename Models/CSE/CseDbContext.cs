using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PRMS.Models.CSE
{
    public class CseDbContext : DbContext
    {


        public CseDbContext()
            : base("cseRmsDbConnectionString")
        {     
        }

        public DbSet<CseEnrollment> CseEnrollment { set; get; }

        public DbSet<JanCseEnrolment> JanCseEnrolment { set; get; }

        public DbSet<JuneCseEnrollment> JuneCseEnrollment { set; get; }
        public DbSet<CurrentSemester> CurrentSemester { set; get; }

        


    }
}