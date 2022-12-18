using BusesApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BusesApp.Design
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //InitialCreate();
            //Countries();
        }

        private void Countries()
        {
            using (Context context = new Context()) //Создание подключения
            {
                var country1 = new Country
                {
                    Name = "Беларусь"
                };
                context.Countries.Add(country1);
                context.SaveChanges();
                MessageBox.Show("Data saved.");

                var town = new Town
                {
                    Name = "Minsk",
                    Country = country1
                };
                context.Towns.Add(town);
                context.SaveChanges();
                MessageBox.Show("Data saved.");
            }
        }
        private void InitialCreate()
        {
            using (Context context = new Context()) //Создание подключения
            {
                Model model1 = new Model
                {
                    Name = "fjghf",
                    Capacity = 57
                };
                context.Models.Add(model1);
                context.SaveChanges();
                MessageBox.Show("Data saved.");

                Bus bus1 = new Bus
                {
                    Number = "lj,lf",
                    Model = model1
                };
                context.Buses.Add(bus1);
                context.SaveChanges();
                MessageBox.Show("Data saved.");
            }

        }

        private void Button_Click_Buses(object sender, RoutedEventArgs e)
        {
            Buses buses = new Buses();
            buses.Show();
        }

        private void Button_Click_Locations(object sender, RoutedEventArgs e)
        {
            Locations locations = new Locations();
            locations.Show();
        }

        private void Button_Click_Clients(object sender, RoutedEventArgs e)
        {
            Clients clients = new Clients();
            clients.Show(); 
        }

        private void Button_Click_Employees(object sender, RoutedEventArgs e)
        {
            Employees emps = new Employees();
            emps.Show();
        }

        private void Button_Click_Routes(object sender, RoutedEventArgs e)
        {
            Routes routes = new Routes();
            routes.Show();
        }

        private void Button_Click_Tours(object sender, RoutedEventArgs e)
        {
            Tours tours = new Tours();
            tours.Show();
        }

        private void Button_Click_Buy(object sender, RoutedEventArgs e)
        {
            ClientTours ct = new ClientTours();
            ct.Show();
        }

        private void Button_Click_EmpTour(object sender, RoutedEventArgs e)
        {
            EmpTours et = new EmpTours();
            et.Show();
        }
    }
}
