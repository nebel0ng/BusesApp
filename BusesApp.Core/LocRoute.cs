using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusesApp.Core
{
    public class LocRoute
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public int RouteId { get; set; }
        public Route Route { get; set; }
        public int SeqNumber { get; set; }
    }
}
