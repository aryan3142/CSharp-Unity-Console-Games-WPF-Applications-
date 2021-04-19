namespace CodeFirstExistingDatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeCourseColumnHeadingFromTitleToName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Name", c => c.String());
            DropColumn("dbo.Courses", "Title");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "Title", c => c.String());
            DropColumn("dbo.Courses", "Name");
        }
    }
}
