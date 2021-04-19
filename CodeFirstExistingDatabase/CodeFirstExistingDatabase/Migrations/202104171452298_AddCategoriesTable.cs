namespace CodeFirstExistingDatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCategoriesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            Sql("INSERT INTO Categories VALUES (1, 'Web Developement')");
            Sql("INSERT INTO Categories VALUES (2, 'Mobile Developement')");
            Sql("INSERT INTO Categories VALUES (3, 'Programming')");
            Sql("INSERT INTO Categories VALUES (4, 'Web Developement')");

            //with sql() method you can run any command on your database
        }
        
        public override void Down()
        {
            DropTable("dbo.Categories");
        }
    }
}
