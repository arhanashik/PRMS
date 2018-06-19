namespace PRMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init7 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourseStatus",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Course_code = c.String(),
                        Course_title = c.String(),
                        Credit_hour = c.Single(nullable: false),
                        Semester = c.Int(nullable: false),
                        UnderFaculty = c.String(),
                        UnderDepartment = c.String(),
                        CourseTeacherID = c.Int(nullable: false),
                        CourseTeacherName = c.String(),
                        CourseTeacherDepartment = c.String(),
                        CourseTeacherFaculty = c.String(),
                        UnderSession = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CourseStatus");
        }
    }
}
