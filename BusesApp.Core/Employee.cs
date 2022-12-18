using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusesApp.Core
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual EmpType Type { get; set; }
        public List<EmpTour> EmpTours { get; set; }
        public Employee(string name, EmpType type)
        {
            Name = name;
            Type = type;
        }
        public Employee()
        {

        }
    }
}
