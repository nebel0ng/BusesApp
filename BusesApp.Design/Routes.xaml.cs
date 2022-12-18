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
    /// Логика взаимодействия для Routes.xaml
    /// </summary>
    public partial class Routes : Window
    {
        Context context;
        List<TextBox> textBoxes;
        List<ComboBox> listBoxes;
        public Routes()
        {
            InitializeComponent();
            textBoxes = new List<TextBox>
            {
                NameBox,
                LengthBox,
                PriceBox
            };
            listBoxes = new List<ComboBox>
            {
                Countries,
                Types
            };
            foreach (TextBox textBox in textBoxes)
            {
                textBox.Clear();
            }
            foreach (var lb in listBoxes)
            {
                lb.SelectedItem = null;
            }
            LoadData();
        }

        public void LoadData()
        {
            context = new Context();

            var linqQuery = context.Countries
                            .Join(context.Routes, country => country, r => r.Country, (country, r) => new { Id = r.Id, Name = r.Name, Price = r.Price.ToString(), Length = r.Length.ToString(), Description = r.Description, Country = r.Country.Name, Type = r.RType })
                            .Join(context.RouteTypes, rc => rc.Type.Name, rt => rt.Name, (rc, rt) => new { Id = rc.Id, Name = rc.Name, Price = rc.Price, Length = rc.Length, Description = rc.Description, Country = rc.Country, Type = rt.Name });

            RouteTable.ItemsSource = linqQuery.ToList();

            Countries.ItemsSource = context.Countries.Select(c => c.Name).ToList();
            Types.ItemsSource = context.RouteTypes.Select(rt => rt.Name).ToList();
        }

        public bool ExistsByTextBoxes()
        {
            string name = textBoxes[0].Text;
            string price = textBoxes[1].Text;
            string length = textBoxes[2].Text;
            string desc = DescBlock.Text;
            string country = listBoxes[0].SelectedItem.ToString();
            string type = listBoxes[1].SelectedItem.ToString();

            if (context.Routes.Where(r => r.Name ==name & r.Country.Name == country & r.RType.Name == type & r.Description == desc & r.Price.ToString() == price & r.Length.ToString() == length).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RouteExists(Route route)
        {
            if (context.Routes.Where(r => r.Name == route.Name & r.Country.Name == route.Country.Name & r.RType.Name == route.RType.Name & r.Description == route.Description & r.Price == route.Price & r.Length == route.Length).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsTheSame(Route r)
        {
            if (textBoxes[0].Text == r.Name.ToString() & textBoxes[1].Text == r.Price.ToString() & textBoxes[2].Text == r.Length.ToString() & listBoxes[0].SelectedItem.ToString() == r.Country.Name & listBoxes[1].SelectedItem.ToString() == r.RType.Name & DescBlock.Text == r.Description)
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
            if (!int.TryParse(PriceBox.Text, out int price) | price <= 0)
            {
                PriceBox.Clear();
                PriceBox.Focus();
            }
            if (!int.TryParse(LengthBox.Text, out int length) | length <= 0)
            {
                LengthBox.Clear();
                LengthBox.Focus();
            }
            if (!listBoxes.Where(b => b.SelectedItem == null).Any() & !textBoxes.Where( b => String.IsNullOrEmpty(b.Text)).Any() & !String.IsNullOrEmpty(DescBlock.Text))
            {
                string country = listBoxes[0].SelectedItem.ToString();
                string rType = listBoxes[1].SelectedItem.ToString();
                var newRoute = new Route(textBoxes[0].Text, int.Parse(textBoxes[1].Text), int.Parse(textBoxes[2].Text), DescBlock.Text, context.Countries.Where(c => c.Name == country).FirstOrDefault(), context.RouteTypes.Where(t => t.Name == rType).FirstOrDefault());

                if (RouteExists(newRoute))
                {
                    MessageBox.Show("Информация об этом направлении уже есть.");
                }
                else
                {
                    foreach (TextBox textBox in textBoxes)
                    {
                        textBox.Clear();
                    }
                    foreach (var lb in listBoxes)
                    {
                        lb.SelectedItem = null;
                    }
                    DescBlock.Text = "";
                    context.Routes.Add(newRoute);
                    context.SaveChanges();
                    LoadData();
                }
            }
        }

        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            if (RouteTable.SelectedItem != null)
            {
                int index = int.Parse(RouteTable.SelectedItem.ToString().Split()[3].TrimEnd(','));
                context.Routes.Remove(context.Routes.Where(r => r.Id == index).FirstOrDefault());
                context.SaveChanges();
                LoadData();
            }
        }

        private void Button_Click_Edit(object sender, RoutedEventArgs e)
        {
            if (RouteTable.SelectedItem != null)
            {
                int index = int.Parse(RouteTable.SelectedItem.ToString().Split()[3].TrimEnd(','));
                var route = context.Routes.Where(r => r.Id == index).FirstOrDefault();
                listBoxes[0].SelectedItem = route.Country.Name;
                listBoxes[1].SelectedItem = route.RType.Name;
                NameBox.Text = route.Name;
                PriceBox.Text = route.Price.ToString();
                LengthBox.Text = route.Length.ToString();
                DescBlock.Text = route.Description;

            }
        }

        private void Button_Click_SaveChanges(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(PriceBox.Text, out int price) | price <= 0)
            {
                PriceBox.Clear();
                PriceBox.Focus();
            }
            if (!int.TryParse(LengthBox.Text, out int length) | length <= 0)
            {
                LengthBox.Clear();
                LengthBox.Focus();
            }
            if (!listBoxes.Where(b => b.SelectedItem == null).Any() & !textBoxes.Where(b => String.IsNullOrEmpty(b.Text)).Any() & !String.IsNullOrEmpty(DescBlock.Text))
            {
                int index = int.Parse(RouteTable.SelectedItem.ToString().Split()[3].TrimEnd(','));
                var selectedRoute = context.Routes.Where(r => r.Id == index).FirstOrDefault();
                if (!ExistsByTextBoxes() | IsTheSame(selectedRoute))
                {
                    selectedRoute.Name = textBoxes[0].Text;
                    selectedRoute.Price = int.Parse(textBoxes[1].Text);
                    selectedRoute.Length = int.Parse(textBoxes[2].Text);
                    selectedRoute.Description =DescBlock.Text;
                    selectedRoute.Country = context.Countries.Where(r => r.Name == listBoxes[0].SelectedItem.ToString()).FirstOrDefault();
                    selectedRoute.RType = context.RouteTypes.Where(t => t.Name == listBoxes[1].SelectedItem.ToString()).FirstOrDefault();

                    foreach (TextBox textBox in textBoxes)
                    {
                        textBox.Clear();
                    }
                    foreach (var lb in listBoxes)
                    {
                        lb.SelectedItem = null;
                    }
                    DescBlock.Text = "";
                    context.SaveChanges();
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Информация об этом направлении уже есть.");
                }
            }
        }

        private void Button_Click_AddType(object sender, RoutedEventArgs e)
        {
            RouteTypes types = new RouteTypes();
            types.Show();
        }
        private void Button_Click_AddCountry(object sender, RoutedEventArgs e)
        {
            Countries countries = new Countries();
            countries.Show();
        }
    }
}
