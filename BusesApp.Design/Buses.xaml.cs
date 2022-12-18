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
    /// Логика взаимодействия для Buses.xaml
    /// </summary>
    public partial class Buses : Window
    {
        Context context;
        public Buses()
        {
            InitializeComponent();
            NumberBox.Clear();
            LoadData();
        }

        public void LoadData()
        {
            context = new Context();

            var linqQuery = context.Models
                            .Join(context.Buses, model => model, bus => bus.Model, (model, bus) => new { Id = bus.Id.ToString(), Number = bus.Number, Model = bus.Model.Name });

            BusesTable.ItemsSource = linqQuery.ToList();

            Models.ItemsSource = context.Models.Select(m => m.Name).ToList();

        }

        public bool BusExistsByTextBoxes()
        {
            string num = NumberBox.Text;
            string model = Models.SelectedItem.ToString();

            if (context.Buses.Where(b => b.Number == num & b.Model.Name == model).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsTheSame(Bus bus)
        {
            if (NumberBox.Text == bus.Number & Models.SelectedItem.ToString() == bus.Model.Name)
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
            if (Models.SelectedItem != null & !String.IsNullOrEmpty(NumberBox.Text))
            {
                var newBus = new Bus(NumberBox.Text, context.Models.Where(m => m.Name == Models.SelectedItem.ToString()).FirstOrDefault());

                if (context.Buses.Where(b => b.Number == newBus.Number & b.Model.Name == newBus.Model.Name).Any())
                {
                    MessageBox.Show("Информация об этом автобусе уже есть.");
                }
                else
                {
                    NumberBox.Clear();
                    context.Buses.Add(newBus);
                    context.SaveChanges();
                    LoadData();
                }
            }
        }

        private void Button_Click_SaveChanges(object sender, RoutedEventArgs e)
        {
            if (Models.SelectedItem != null & !String.IsNullOrEmpty(NumberBox.Text))
            {
                int index = int.Parse(BusesTable.SelectedItem.ToString().Split()[3].TrimEnd(','));
                var selectedBus = context.Buses.Where(b => b.Id == index).FirstOrDefault();
                if (!BusExistsByTextBoxes() | IsTheSame(selectedBus))
                {
                    selectedBus.Number = NumberBox.Text;
                    selectedBus.Model = context.Models.Where(m => m.Name == Models.SelectedItem.ToString()).FirstOrDefault();

                    NumberBox.Clear();
                    context.SaveChanges();
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Информация об этой записи уже есть.");
                }
            }
        }

        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            if (BusesTable.SelectedItem != null)
            {
                if (!context.Tours.ToList().Where(t => t.Bus == (BusesTable.SelectedItem as Bus)).Any())
                {
                    int index = int.Parse(BusesTable.SelectedItem.ToString().Split()[3].TrimEnd(','));
                    context.Buses.Remove(context.Buses.Where(b => b.Id == index).FirstOrDefault());
                    context.SaveChanges();
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Нельзя удалить данные об автобусе, так как есть связанные туры.");
                }
            }
        }

        private void Button_Click_Edit(object sender, RoutedEventArgs e)
        {
            if (BusesTable.SelectedItem != null)
            {
                int index = int.Parse(BusesTable.SelectedItem.ToString().Split()[3].TrimEnd(','));
                var bus = context.Buses.Where(b => b.Id == index).FirstOrDefault();
                NumberBox.Text = bus.Number;
                Models.SelectedItem = bus.Model.Name.ToString();
            }
        }

        private void Button_Click_AddModel(object sender, RoutedEventArgs e)
        {
            Models models = new Models();
            models.Show();
        }
    }
}
