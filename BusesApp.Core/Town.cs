using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusesApp.Core
{
    public class Town
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Country Country { get; set; }
        public Town(string name, Country country)
        {
            Name = name;
            Country = country;
        }
        public Town()
        {

        }
    }
}
