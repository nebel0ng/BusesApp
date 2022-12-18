using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusesApp.Core
{
    public class ClientTour
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
        public int TourId { get; set; }
        public virtual Tour Tour { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public double SoldFor { get; set; }
        public ClientTour(Client client, Tour tour, DateTime dop)
        {
            Client = client;
            ClientId = client.Id;
            Tour = tour;
            TourId = tour.Id;
            DateOfPurchase = dop;
            SoldFor = Tour.Route.Price * (1 - Tour.Discount / 100.000);
        }
        public ClientTour()
        {

        }
    }
}
