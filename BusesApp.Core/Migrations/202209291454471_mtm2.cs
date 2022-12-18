namespace BusesApp.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mtm2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientTours",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                        TourId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .ForeignKey("dbo.Tours", t => t.TourId, cascadeDelete: true)
                .Index(t => t.ClientId)
                .Index(t => t.TourId);
            
            CreateTable(
                "dbo.LocRoutes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LocationId = c.Int(nullable: false),
                        RouteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .ForeignKey("dbo.Routes", t => t.RouteId, cascadeDelete: true)
                .Index(t => t.LocationId)
                .Index(t => t.RouteId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LocRoutes", "RouteId", "dbo.Routes");
            DropForeignKey("dbo.LocRoutes", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.ClientTours", "TourId", "dbo.Tours");
            DropForeignKey("dbo.ClientTours", "ClientId", "dbo.Clients");
            DropIndex("dbo.LocRoutes", new[] { "RouteId" });
            DropIndex("dbo.LocRoutes", new[] { "LocationId" });
            DropIndex("dbo.ClientTours", new[] { "TourId" });
            DropIndex("dbo.ClientTours", new[] { "ClientId" });
            DropTable("dbo.LocRoutes");
            DropTable("dbo.ClientTours");
        }
    }
}
