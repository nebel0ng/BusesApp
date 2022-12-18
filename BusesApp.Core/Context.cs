using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusesApp.Core
{
    public class Context : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<EmpType> EmpTypes { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<RouteType> RouteTypes { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<ClientTour> ClientTours { get; set; }
        public DbSet<EmpTour> EmpTours { get; set; }
        public DbSet<LocRoute> LocRoutes { get; set; }
        public Context() : base("DBConnection")
        {

        }
    }
}
