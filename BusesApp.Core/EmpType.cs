using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusesApp.Core
{
    public class EmpType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public EmpType(string name)
        {
            Name = name;
        }
        public EmpType()
        {
            
        }
    }
}
