using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusesApp.Core
{
    public class Route
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Length { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public virtual Country Country { get; set; }
        public virtual RouteType RType { get; set; }
        public List<LocRoute> LocRoutes { get; set; }
        public Route(string name, int length, int price, string desc, Country country, RouteType rtype)
        {
            Name = name;
            Length = length;
            Price = price;
            Description = desc;
            Country = country;
            RType = rtype;
        }
        public Route()
        {

        }
    }
}
