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
    /// Логика взаимодействия для Locations.xaml
    /// </summary>
    public partial class Locations : Window
    {
        Context context;
        public Locations()
        {
            InitializeComponent();
            NameBox.Clear();
            LoadData();

        }

        public void LoadData()
        {
            context = new Context();

            var linqQuery = context.Towns
                            .Join(context.Locations, town => town, loc => loc.Town, (town, loc) => new { Id = loc.Id.ToString(), Name = loc.Name, Town = loc.Town.Name });

            LocTable.ItemsSource = linqQuery.ToList();

            Towns.ItemsSource = context.Towns.Select(t => t.Name).ToList();

        }

        public bool ExistsByTextBoxes()
        {
            string name = NameBox.Text;
            string town = Towns.SelectedItem.ToString();

            if (context.Locations.Where(l => l.Name == name & l.Town.Name == town).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsTheSame(Location loc)
        {
            if (NameBox.Text == loc.Name & Towns.SelectedItem.ToString() == loc.Town.Name)
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
            if (Towns.SelectedItem != null & !String.IsNullOrEmpty(NameBox.Text))
            {
                var newLoc = new Location(NameBox.Text, context.Towns.Where(t => t.Name == Towns.SelectedItem.ToString()).FirstOrDefault());

                if (context.Locations.Where(l => l.Name == newLoc.Name & l.Town.Name == newLoc.Town.Name).Any())
                {
                    MessageBox.Show("Информация об этой локации уже есть.");
                }
                else
                {
                    NameBox.Clear();
                    context.Locations.Add(newLoc);
                    context.SaveChanges();
                    LoadData();
                }
            }

        }

        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            if (LocTable.SelectedItem != null)
            {
                int index = int.Parse(LocTable.SelectedItem.ToString().Split()[3].TrimEnd(','));
                context.Locations.Remove(context.Locations.Where(l => l.Id == index).FirstOrDefault());
                context.SaveChanges();
                LoadData();
                // многие ко многим
                //if (!context.Tours.ToList().Where(t => t.Bus == (BusesTable.SelectedItem as Bus)).Any())
                //{
                    
                //}
                //else
                //{
                //    MessageBox.Show("Нельзя удалить данные об автобусе, так как есть связанные туры.");
                //}
            }
        }

        private void Button_Click_Edit(object sender, RoutedEventArgs e)
        {
            if (LocTable.SelectedItem != null)
            {
                int index = int.Parse(LocTable.SelectedItem.ToString().Split()[3].TrimEnd(','));
                var loc = context.Locations.Where(l => l.Id == index).FirstOrDefault();
                NameBox.Text = loc.Name;
                Towns.SelectedItem = loc.Town.Name.ToString();
            }
        }

        private void Button_Click_SaveChanges(object sender, RoutedEventArgs e)
        {
            if (Towns.SelectedItem != null & !String.IsNullOrEmpty(NameBox.Text))
            {
                int index = int.Parse(LocTable.SelectedItem.ToString().Split()[3].TrimEnd(','));
                var selectedLoc = context.Locations.Where(l => l.Id == index).FirstOrDefault();
                if (!ExistsByTextBoxes() | IsTheSame(selectedLoc))
                {
                    selectedLoc.Name = NameBox.Text;
                    selectedLoc.Town = context.Towns.Where(t => t.Name == Towns.SelectedItem.ToString()).FirstOrDefault();

                    NameBox.Clear();
                    Towns.SelectedItem = null;
                    context.SaveChanges();
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Информация об этой записи уже есть.");
                }
            }
        }

        private void Button_Click_AddTown(object sender, RoutedEventArgs e)
        {
            Towns towns = new Towns();
            towns.Show();
        }
    }
}
