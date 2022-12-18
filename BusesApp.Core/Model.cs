using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusesApp.Core
{
    public class Model
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public Model(string name, string cap)
        {
            Name = name;
            Capacity = int.Parse(cap);
        }
        
        public Model()
        {

        }
    }
}
