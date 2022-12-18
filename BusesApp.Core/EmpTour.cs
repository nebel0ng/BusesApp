using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusesApp.Core
{
    public class EmpTour
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int TourId { get; set; }
        public Tour Tour { get; set; }
        public EmpTour(Employee emp, Tour tour, DateTime dop)
        {
            Employee = emp;
            EmployeeId = emp.Id;
            Tour = tour;
            TourId = tour.Id;
        }
        public EmpTour()
        {

        }
    }
}
