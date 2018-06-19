namespace PRMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Designation = c.String(),
                        UserId = c.String(),
                        Time = c.String(),
                        Message = c.String(),
                        Content = c.String(),
                        FilePath = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.AllSessions",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Session = c.String(),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Course_code = c.String(),
                        Course_title = c.String(),
                        Credit_hour = c.Single(nullable: false),
                        Semester = c.Int(nullable: false),
                        UnderFaculty = c.String(),
                        UnderDepartment = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(),
                        ShortForm = c.String(),
                        Faculty = c.String(),
                        ChairmanId = c.Int(nullable: false),
                        ChairmanName = c.String(),
                        ChairmanEmail = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Faculties",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        FacultyName = c.String(),
                        ShortForm = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.StudentInfoes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StudentId = c.Int(nullable: false),
                        Reg = c.Int(nullable: false),
                        Faculty = c.String(),
                        Session = c.String(),
                        Regularity = c.String(),
                        Hall = c.String(),
                        Blood = c.String(),
                        Sex = c.String(),
                        Fathers_name = c.String(),
                        Mothers_name = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        Nationality = c.String(),
                        Religion = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        TeacherID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Mobile = c.String(),
                        Faculty = c.String(),
                        Department = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.TeacherID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Teachers");
            DropTable("dbo.StudentInfoes");
            DropTable("dbo.Faculties");
            DropTable("dbo.Departments");
            DropTable("dbo.Courses");
            DropTable("dbo.AllSessions");
            DropTable("dbo.Admins");
            DropTable("dbo.Activities");
        }
    }
}
