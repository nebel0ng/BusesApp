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
    /// Логика взаимодействия для Countries.xaml
    /// </summary>
    public partial class Countries : Window
    {
        Context context;
        public Countries()
        {
            InitializeComponent();
            NameBox.Clear();
            LoadData();
        }


        public void LoadData()
        {
            context = new Context();

            CountryTable.ItemsSource = context.Countries.ToList();
        }

        public bool CountryExists(Country country)
        {
            if (context.Countries.Where(c => c.Name == country.Name).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CountryExistsByTextBoxes()
        {
            string name = NameBox.Text;

            if (context.Countries.Where(c => c.Name == name).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsTheSame(Country country)
        {
            if (NameBox.Text == country.Name)
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
            if (!String.IsNullOrEmpty(NameBox.Text))
            {
                var newCountry = new Country(NameBox.Text);
                NameBox.Clear();
                
                if (CountryExists(newCountry))
                {
                    MessageBox.Show("Информация об этой стране уже есть.");
                }
                else
                {
                    context.Countries.Add(newCountry);
                    context.SaveChanges();
                    LoadData();
                }
            }
        }

        private void Button_Click_SaveChanges(object sender, RoutedEventArgs e)
        {
            if (CountryTable.SelectedItem != null & !String.IsNullOrEmpty(NameBox.Text))
            {
                if (!CountryExistsByTextBoxes() | IsTheSame(CountryTable.SelectedItem as Country))
                {
                    (CountryTable.SelectedItem as Country).Name = NameBox.Text;
                    NameBox.Clear();
                    context.SaveChanges();
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Информация об этой модели уже есть.");
                }
            }
        }

        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            if (CountryTable.SelectedItem != null)
            {
                if (!context.Towns.ToList().Where(t => t.Country == (CountryTable.SelectedItem as Country)).Any())
                {
                    context.Countries.Remove(CountryTable.SelectedItem as Country);
                    context.SaveChanges();
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Нельзя удалить данные о стране, так как есть связанные города.");
                }

            }
        }

        private void Button_Click_Edit(object sender, RoutedEventArgs e)
        {
            if (CountryTable.SelectedItem != null)
            {
                NameBox.Text = (CountryTable.SelectedItem as Country).Name;
            }
        }
    }
}
