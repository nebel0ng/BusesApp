namespace BusesApp.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Double : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ClientTours", "SoldFor", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ClientTours", "SoldFor", c => c.Int(nullable: false));
        }
    }
}
