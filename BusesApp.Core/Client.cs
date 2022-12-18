using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusesApp.Core
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public virtual Country Citizenship { get; set; }
        public List<ClientTour> ClientTours { get; set; }
        public Client(string name, DateTime? bd, string number, Country country)
        {
            Name = name;
            BirthDate = bd;
            PhoneNumber = number;
            Citizenship = country;
        }
        public Client()
        {
                
        }
    }
}
