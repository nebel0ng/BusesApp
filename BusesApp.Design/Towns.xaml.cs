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
    /// Логика взаимодействия для Towns.xaml
    /// </summary>
    public partial class Towns : Window
    {
        Context context;
        public Towns()
        {
            InitializeComponent();
            NameBox.Clear();
            LoadData();
        }
        public void LoadData()
        {
            context = new Context();

            var linqQuery = context.Countries
                            .Join(context.Towns, country => country, town => town.Country, (country, town) => new { Id = town.Id.ToString(), Name = town.Name, Country = town.Country.Name });

            TownTable.ItemsSource = linqQuery.ToList();

            Countries.ItemsSource = context.Countries.Select(c => c.Name).ToList();

        }

        public bool ExistsByTextBoxes()
        {
            string name = NameBox.Text;
            string country = Countries.SelectedItem.ToString();

            if (context.Towns.Where(t => t.Name == name & t.Country.Name == country).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsTheSame(Town town)
        {
            if (NameBox.Text == town.Name & Countries.SelectedItem.ToString() == town.Country.Name)
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
            if (Countries.SelectedItem != null & !String.IsNullOrEmpty(NameBox.Text))
            {
                var newTown = new Town(NameBox.Text, context.Countries.Where(c => c.Name == Countries.SelectedItem.ToString()).FirstOrDefault());

                if (context.Towns.Where(t => t.Name == newTown.Name & t.Country.Name == newTown.Country.Name).Any())
                {
                    MessageBox.Show("Информация об этом городе уже есть.");
                }
                else
                {
                    NameBox.Clear();
                    context.Towns.Add(newTown);
                    context.SaveChanges();
                    LoadData();
                }
            }

        }

        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            if (TownTable.SelectedItem != null)
            {                
                if (!context.Locations.ToList().Where(l => l.Town == (TownTable.SelectedItem as Town)).Any())
                {
                    int index = int.Parse(TownTable.SelectedItem.ToString().Split()[3].TrimEnd(','));
                    context.Towns.Remove(context.Towns.Where(t => t.Id == index).FirstOrDefault());
                    context.SaveChanges();
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Нельзя удалить данные о городе, так как есть связанные локации.");
                }
            }
        }

        private void Button_Click_Edit(object sender, RoutedEventArgs e)
        {
            if (TownTable.SelectedItem != null)
            {
                int index = int.Parse(TownTable.SelectedItem.ToString().Split()[3].TrimEnd(','));
                var town = context.Towns.Where(t => t.Id == index).FirstOrDefault();
                NameBox.Text = town.Name;
                Countries.SelectedItem = town.Country.Name.ToString();
            }
        }

        private void Button_Click_SaveChanges(object sender, RoutedEventArgs e)
        {
            if (Countries.SelectedItem != null & !String.IsNullOrEmpty(NameBox.Text))
            {
                int index = int.Parse(TownTable.SelectedItem.ToString().Split()[3].TrimEnd(','));
                var selectedTown = context.Towns.Where(t => t.Id == index).FirstOrDefault();
                if (!ExistsByTextBoxes() | IsTheSame(selectedTown))
                {
                    selectedTown.Name = NameBox.Text;
                    selectedTown.Country = context.Countries.Where(c => c.Name == Countries.SelectedItem.ToString()).FirstOrDefault();

                    NameBox.Clear();
                    Countries.SelectedItem = null;
                    context.SaveChanges();
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Информация об этом городе уже есть.");
                }
            }
        }

        private void Button_Click_AddCountry(object sender, RoutedEventArgs e)
        {
            Countries countries = new Countries();
            countries.Show();
        }
    }
}
