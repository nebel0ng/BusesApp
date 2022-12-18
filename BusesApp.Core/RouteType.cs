using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusesApp.Core
{
    public class RouteType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public RouteType(string name)
        {
            Name = name;
        }
        public RouteType()
        {

        }
    }
}
