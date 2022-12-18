using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusesApp.Core
{
    public class Bus
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public virtual Model Model { get; set; }
        public Bus(string number, Model model)
        {
            Number = number;
            Model = model;
        }
        public Bus()
        {
            
        }
    }
}
