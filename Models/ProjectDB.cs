using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PRMS.Models
{
    public class ProjectDB : DbContext
    {
        public ProjectDB()
            : base("prmsDbConnectionString")
        {     
        }
       
       public DbSet<Teacher> Teacher{set;get;}
       public DbSet<StudentInfo> StudentInfo { set; get; }
       public DbSet<Faculty> Faculty { set; get; }
       public DbSet<Department> Department { set; get; }
       public DbSet<Course> Courses { set; get; }
       public DbSet<AllSession> AllSession { set; get; }
       public DbSet<Activity> Activity { set; get; }
       public DbSet<Admin> Admin { set; get; }
        

    }
}