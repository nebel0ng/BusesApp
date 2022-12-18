using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusesApp.Core
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Town Town { get; set; }
        public List<LocRoute> LocRoutes { get; set; }
        public Location(string name, Town town)
        {
            Name = name;
            Town = town;
        }
        public Location()
        {

        }
    }
}
