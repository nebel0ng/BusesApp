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
using System.Windows.Shapes;

namespace BusesApp.Design
{
    /// <summary>
    /// Логика взаимодействия для Tours.xaml
    /// </summary>
    public partial class Tours : Window
    {
        Context context;
        List<ComboBox> listBoxes;
        public Tours()
        {
            InitializeComponent();
            listBoxes = new List<ComboBox>
            {
                Routes,
                Buses
            };
            foreach (var lb in listBoxes)
            {
                lb.SelectedItem = null;
            }
            LoadData();
        }
        public void LoadData()
        {
            context = new Context();

            var linqQuery = context.Routes
                            .Join(context.Tours, r => r, t => t.Route, (r, t) => new { Id = t.Id, SD = t.StartDate, ED = t.EndDate, Discount = t.Discount.ToString(), Route = t.Route.Name, Bus = t.Bus })
                            .Join(context.Buses, rt => rt.Bus.Number, b => b.Number, (rt, b) => new { Id = rt.Id, SD = rt.SD, ED = rt.ED, Discount = rt.Discount, Route = rt.Route, Bus = rt.Bus.Number });

            TourTable.ItemsSource = linqQuery.ToList();

            Routes.ItemsSource = context.Routes.Select(r => r.Name).ToList();
            Buses.ItemsSource = context.Buses.Select(b => b.Number).ToList();
        }
        private void Button_Click_AddRoute(object sender, RoutedEventArgs e)
        {
            Routes routes = new Routes();
            routes.Show();
        }

        private void Button_Click_AddBus(object sender, RoutedEventArgs e)
        {
            Buses buses = new Buses();
            buses.Show();
        }

        public bool TourExists(Tour tour)
        {
            if (context.Tours.Where(t => t.StartDate == tour.StartDate & t.Route.Name == tour.Route.Name & t.Bus.Number == tour.Bus.Number & t.Discount.ToString() == tour.Discount.ToString()).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ExistsByTextBoxes()
        {
            string sd = DDPicker.SelectedDate.ToString();
            string discount = Discount.Text;
            string route = listBoxes[0].SelectedItem.ToString();
            string bus = listBoxes[1].SelectedItem.ToString();

            if (context.Tours.Where(t => t.StartDate.ToString() == sd & t.Route.Name == route & t.Bus.Number == bus & t.Discount.ToString() == discount).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsTheSame(Tour t)
        {
            if (Discount.Text == t.Discount.ToString() & listBoxes[0].SelectedItem.ToString() == t.Route.Name & listBoxes[1].SelectedItem.ToString() == t.Bus.Number & DDPicker.SelectedDate == t.StartDate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(Discount.Text, out int price) | price <= 0)
            {
                Discount.Clear();
                Discount.Focus();
            }
            
            if (!listBoxes.Where(b => b.SelectedItem == null).Any() & !String.IsNullOrEmpty(Discount.Text) & DDPicker.SelectedDate != null)
            {
                string name = listBoxes[0].SelectedItem.ToString();
                string number = listBoxes[1].SelectedItem.ToString();

                var newTour = new Tour(DateTime.Parse(DDPicker.SelectedDate.ToString()), context.Routes.Where(r => r.Name == name).FirstOrDefault(), context.Buses.Where(b => b.Number == number).FirstOrDefault(), int.Parse(Discount.Text));

                if (TourExists(newTour))
                {
                    MessageBox.Show("Информация об этом туре уже есть.");
                }
                else
                {
                    foreach (var lb in listBoxes)
                    {
                        lb.SelectedItem = null;
                    }
                    Discount.Text = "";
                    DDPicker.SelectedDate = null;
                    context.Tours.Add(newTour);
                    context.SaveChanges();
                    LoadData();
                }
            }
        }

        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            if (TourTable.SelectedItem != null)
            {
                int index = int.Parse(TourTable.SelectedItem.ToString().Split()[3].TrimEnd(','));
                context.Tours.Remove(context.Tours.Where(t => t.Id == index).FirstOrDefault());
                context.SaveChanges();
                LoadData();
            }
        }

        private void Button_Click_Edit(object sender, RoutedEventArgs e)
        {
            if (TourTable.SelectedItem != null)
            {
                int index = int.Parse(TourTable.SelectedItem.ToString().Split()[3].TrimEnd(','));
                var tour = context.Tours.Where(t => t.Id == index).FirstOrDefault();
                listBoxes[0].SelectedItem = tour.Route.Name;
                listBoxes[1].SelectedItem = tour.Bus.Number;
                Discount.Text = tour.Discount.ToString();
                DDPicker.SelectedDate = tour.StartDate;

            }
        }

        private void Button_Click_SaveChanges(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(Discount.Text, out int price) | price <= 0)
            {
                Discount.Clear();
                Discount.Focus();
            }

            if (!listBoxes.Where(b => b.SelectedItem == null).Any() & !String.IsNullOrEmpty(Discount.Text) & DDPicker.SelectedDate != null)
            {
                int index = int.Parse(TourTable.SelectedItem.ToString().Split()[3].TrimEnd(','));
                var selectedTour = context.Tours.Where(t => t.Id == index).FirstOrDefault();
                if (!ExistsByTextBoxes() | IsTheSame(selectedTour))
                {
                    selectedTour.StartDate = DateTime.Parse(DDPicker.SelectedDate.ToString());
                    selectedTour.Route = context.Routes.Where(r => r.Name == listBoxes[0].SelectedItem.ToString()).FirstOrDefault();
                    selectedTour.Bus = context.Buses.Where(b => b.Number == listBoxes[1].SelectedItem.ToString()).FirstOrDefault();
                    selectedTour.EndDate = selectedTour.StartDate.AddDays(selectedTour.Route.Length);
                    selectedTour.Discount = int.Parse(Discount.Text);

                    foreach (var lb in listBoxes)
                    {
                        lb.SelectedItem = null;
                    }
                    Discount.Text = "";
                    DDPicker.SelectedDate = null;
                    context.SaveChanges();
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Информация об этом туре уже есть.");
                }
            }
        }
    }
}
