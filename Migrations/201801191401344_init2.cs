namespace PRMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init2 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Activities", newName: "TeacherActivities");
            AddColumn("dbo.TeacherActivities", "CourseCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TeacherActivities", "CourseCode");
            RenameTable(name: "dbo.TeacherActivities", newName: "Activities");
        }
    }
}
