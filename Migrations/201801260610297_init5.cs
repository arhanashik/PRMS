namespace PRMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "CourseTeacherFaculty", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "CourseTeacherFaculty");
        }
    }
}
