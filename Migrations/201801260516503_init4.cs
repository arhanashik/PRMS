namespace PRMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "CourseTeacherID", c => c.String());
            AddColumn("dbo.Courses", "CourseTeacherName", c => c.String());
            AddColumn("dbo.Courses", "CourseTeacherDepartment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "CourseTeacherDepartment");
            DropColumn("dbo.Courses", "CourseTeacherName");
            DropColumn("dbo.Courses", "CourseTeacherID");
        }
    }
}
