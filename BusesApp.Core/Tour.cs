using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusesApp.Core
{
    public class Tour
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Discount { get; set; }
        public virtual Route Route { get; set; }
        public virtual Bus Bus { get; set; }
        public List<EmpTour> EmpTours { get; set; }
        public List<ClientTour> ClientTours { get; set; }
        public Tour(DateTime sd, Route route, Bus bus, int discount=0)
        {
            StartDate = sd;
            Route = route;
            EndDate = sd.AddDays(Route.Length);            
            Bus = bus;
            Discount = discount;
        }
        public Tour()
        {

        }
    }
}
