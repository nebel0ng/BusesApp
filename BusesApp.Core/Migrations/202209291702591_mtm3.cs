namespace BusesApp.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mtm3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientTours", "DateOfPurchase", c => c.DateTime(nullable: false));
            AddColumn("dbo.ClientTours", "SoldFor", c => c.Int(nullable: false));
            AddColumn("dbo.LocRoutes", "SeqNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LocRoutes", "SeqNumber");
            DropColumn("dbo.ClientTours", "SoldFor");
            DropColumn("dbo.ClientTours", "DateOfPurchase");
        }
    }
}
