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
    /// Логика взаимодействия для ClientTours.xaml
    /// </summary>
    public partial class ClientTours : Window
    {
        Context context;
        List<ComboBox> listBoxes;
        public ClientTours()
        {
            InitializeComponent();
            listBoxes = new List<ComboBox>
            {
                Clients,
                Routes,
                Dates
            };
            LoadData();
        }
        public void LoadData()
        {
            context = new Context();

            var linqQuery = context.Clients
                            .Join(context.ClientTours, c => c, ct => ct.Client, (c, ct) => new { Id = ct.Id, Client = c.Name, Tour = ct.Tour })
                            .Join(context.Tours, cct => cct.Tour, t => t, (cct, t) => new { Id = cct.Id, Client = cct.Client, Route = t.Route.Name, SD = t.StartDate.ToString() });

            CTTable.ItemsSource = linqQuery.ToList();

            Routes.ItemsSource = context.Routes.Select(r => r.Name).ToList();
            Clients.ItemsSource = context.Clients.Select(c => c.Name).ToList();
            Dates.ItemsSource = context.Tours.Select(t => t.StartDate.ToString()).ToList();
        }

        public bool RecordExists(ClientTour ctour)
        {
            if (context.ClientTours.Where(ct => ct.Tour.Route.Name == ctour.Tour.Route.Name & ct.Tour.StartDate == ctour.Tour.StartDate & ct.Client.Name == ctour.Client.Name & ct.SoldFor == ctour.SoldFor).Any())
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
            string sd = listBoxes[2].SelectedItem.ToString();
            string client = listBoxes[0].SelectedItem.ToString();
            string route = listBoxes[1].SelectedItem.ToString();

            if (context.ClientTours.Where(t => t.Tour.StartDate.ToString() == sd & t.Tour.Route.Name == route & t.Client.Name == client).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsTheSame(ClientTour t)
        {
            if (listBoxes[0].SelectedItem.ToString() == t.Client.Name & listBoxes[1].SelectedItem.ToString() == t.Tour.Route.Name & listBoxes[2].SelectedItem.ToString() == t.Tour.StartDate.ToString())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Button_Click_AddClient(object sender, RoutedEventArgs e)
        {
            Clients clients = new Clients();
            clients.Show();
        }

        //private void Button_Click_SaveChanges(object sender, RoutedEventArgs e)
        //{
        //    if (!listBoxes.Where(b => b.SelectedItem == null).Any())
        //    {
        //        int index = int.Parse(CTTable.SelectedItem.ToString().Split()[3].TrimEnd(','));
        //        var selectedTour = context.ClientTours.Where(t => t.Id == index).FirstOrDefault();
        //        if (!ExistsByTextBoxes() | IsTheSame(selectedTour))
        //        {
        //            string name = listBoxes[1].SelectedItem.ToString();
        //            string sd = listBoxes[2].SelectedItem.ToString();
        //            selectedTour.Tour = context.Tours.Where(t => t.Route.Name == name & t.StartDate.ToString() == sd).FirstOrDefault();
        //            selectedTour.Client = context.Clients.Where(c => c.Name == listBoxes[0].SelectedItem.ToString()).FirstOrDefault();
        //            selectedTour.DateOfPurchase = DateTime.Today;

        //            foreach (var lb in listBoxes)
        //            {
        //                lb.SelectedItem = null;
        //            }
        //            context.SaveChanges();
        //            LoadData();
        //        }
        //        else
        //        {
        //            MessageBox.Show("Информация об этом туре уже есть.");
        //        }
        //    }
        //}

        //private void Button_Click_Edit(object sender, RoutedEventArgs e)
        //{
        //    if (CTTable.SelectedItem != null)
        //    {
        //        int index = int.Parse(CTTable.SelectedItem.ToString().Split()[3].TrimEnd(','));
        //        var ctour = context.ClientTours.Where(ct => ct.Id == index).FirstOrDefault();
        //        listBoxes[0].SelectedItem = ctour.Client.Name;
        //        listBoxes[1].SelectedItem = ctour.Tour.Route.Name;
        //        listBoxes[2].SelectedItem = ctour.Tour.StartDate.ToString();

        //    }
        //}

        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            if (CTTable.SelectedItem != null)
            {
                int index = int.Parse(CTTable.SelectedItem.ToString().Split()[3].TrimEnd(','));
                context.ClientTours.Remove(context.ClientTours.Where(t => t.Id == index).FirstOrDefault());
                context.SaveChanges();
                LoadData();
            }
        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            if (!listBoxes.Where(b => b.SelectedItem == null).Any())
            {
                string name = listBoxes[0].SelectedItem.ToString();
                string route = listBoxes[1].SelectedItem.ToString();

                var newClientTour = new ClientTour(context.Clients.Where(c => c.Name == name).FirstOrDefault(), context.Tours.Where(t => t.Route == context.Routes.Where(r => r.Name == route).FirstOrDefault()).FirstOrDefault(), DateTime.Today);

                if (RecordExists(newClientTour))
                {
                    MessageBox.Show("Этот клиент уже приобретал билет на этот тур.");
                }
                else
                {
                    foreach (var lb in listBoxes)
                    {
                        lb.SelectedItem = null;
                    }
                    context.ClientTours.Add(newClientTour);
                    context.SaveChanges();
                    LoadData();
                }
            }
        }
    }
}
