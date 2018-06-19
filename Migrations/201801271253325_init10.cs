namespace PRMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init10 : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Results");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Results",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StudentId = c.Int(nullable: false),
                        RegNo = c.Int(nullable: false),
                        Faculty = c.String(),
                        Session = c.String(),
                        Semester = c.Int(nullable: false),
                        Degree = c.String(),
                        GPA = c.Single(nullable: false),
                        PrevCGPA = c.Single(nullable: false),
                        PrevCCH = c.Single(nullable: false),
                        CGPA = c.Single(nullable: false),
                        CCH = c.Single(nullable: false),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
    }
}
