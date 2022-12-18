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
    /// Логика взаимодействия для RouteTypes.xaml
    /// </summary>
    public partial class RouteTypes : Window
    {        
        
        Context context;
        public RouteTypes()
        {
            InitializeComponent();
            NameBox.Clear();
            LoadData();
        }
        public void LoadData()
        {
            context = new Context();

            TypeTable.ItemsSource = context.RouteTypes.ToList();
        }

        public bool TypeExists(RouteType type)
        {
            if (context.RouteTypes.Where(t => t.Name == type.Name).Any())
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
            string name = NameBox.Text;

            if (context.RouteTypes.Where(t => t.Name == name).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsTheSame(RouteType type)
        {
            if (NameBox.Text == type.Name)
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
                var newType = new RouteType(NameBox.Text);
                NameBox.Clear();

                if (TypeExists(newType))
                {
                    MessageBox.Show("Информация об этом типе уже есть.");
                }
                else
                {
                    context.RouteTypes.Add(newType);
                    context.SaveChanges();
                    LoadData();
                }
            }
        }

        private void Button_Click_SaveChanges(object sender, RoutedEventArgs e)
        {
            if (TypeTable.SelectedItem != null & !String.IsNullOrEmpty(NameBox.Text))
            {
                if (!ExistsByTextBoxes() | IsTheSame(TypeTable.SelectedItem as RouteType))
                {
                    (TypeTable.SelectedItem as RouteType).Name = NameBox.Text;
                    NameBox.Clear();
                    context.SaveChanges();
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Информация об этом типе уже есть.");
                }
            }
        }

        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            if (TypeTable.SelectedItem != null)
            {
                if (!context.Routes.ToList().Where(r => r.RType == (TypeTable.SelectedItem as RouteType)).Any())
                {
                    context.RouteTypes.Remove(TypeTable.SelectedItem as RouteType);
                    context.SaveChanges();
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Нельзя удалить данные о типе, так как есть связанные направления.");
                }

            }
        }

        private void Button_Click_Edit(object sender, RoutedEventArgs e)
        {
            if (TypeTable.SelectedItem != null)
            {
                NameBox.Text = (TypeTable.SelectedItem as RouteType).Name;
            }
        }
    }
}
