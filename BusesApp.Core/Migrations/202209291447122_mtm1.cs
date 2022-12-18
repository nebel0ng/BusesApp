namespace BusesApp.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mtm1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmpTours",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        TourId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.Tours", t => t.TourId, cascadeDelete: true)
                .Index(t => t.EmployeeId)
                .Index(t => t.TourId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmpTours", "TourId", "dbo.Tours");
            DropForeignKey("dbo.EmpTours", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.EmpTours", new[] { "TourId" });
            DropIndex("dbo.EmpTours", new[] { "EmployeeId" });
            DropTable("dbo.EmpTours");
        }
    }
}
