namespace BusesApp.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FullDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        PhoneNumber = c.String(),
                        Citizenship_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.Citizenship_Id)
                .Index(t => t.Citizenship_Id);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Type_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EmpTypes", t => t.Type_Id)
                .Index(t => t.Type_Id);
            
            CreateTable(
                "dbo.EmpTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Town_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Towns", t => t.Town_Id)
                .Index(t => t.Town_Id);
            
            CreateTable(
                "dbo.Routes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Length = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        Description = c.String(),
                        Country_Id = c.Int(),
                        RType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.Country_Id)
                .ForeignKey("dbo.RouteTypes", t => t.RType_Id)
                .Index(t => t.Country_Id)
                .Index(t => t.RType_Id);
            
            CreateTable(
                "dbo.RouteTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tours",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Discount = c.Int(nullable: false),
                        Bus_Id = c.Int(),
                        Route_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Buses", t => t.Bus_Id)
                .ForeignKey("dbo.Routes", t => t.Route_Id)
                .Index(t => t.Bus_Id)
                .Index(t => t.Route_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tours", "Route_Id", "dbo.Routes");
            DropForeignKey("dbo.Tours", "Bus_Id", "dbo.Buses");
            DropForeignKey("dbo.Routes", "RType_Id", "dbo.RouteTypes");
            DropForeignKey("dbo.Routes", "Country_Id", "dbo.Countries");
            DropForeignKey("dbo.Locations", "Town_Id", "dbo.Towns");
            DropForeignKey("dbo.Employees", "Type_Id", "dbo.EmpTypes");
            DropForeignKey("dbo.Clients", "Citizenship_Id", "dbo.Countries");
            DropIndex("dbo.Tours", new[] { "Route_Id" });
            DropIndex("dbo.Tours", new[] { "Bus_Id" });
            DropIndex("dbo.Routes", new[] { "RType_Id" });
            DropIndex("dbo.Routes", new[] { "Country_Id" });
            DropIndex("dbo.Locations", new[] { "Town_Id" });
            DropIndex("dbo.Employees", new[] { "Type_Id" });
            DropIndex("dbo.Clients", new[] { "Citizenship_Id" });
            DropTable("dbo.Tours");
            DropTable("dbo.RouteTypes");
            DropTable("dbo.Routes");
            DropTable("dbo.Locations");
            DropTable("dbo.EmpTypes");
            DropTable("dbo.Employees");
            DropTable("dbo.Clients");
        }
    }
}
