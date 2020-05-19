namespace News24.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class views : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "Views", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "Views");
        }
    }
}
