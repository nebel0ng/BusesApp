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
    /// Логика взаимодействия для EmpTours.xaml
    /// </summary>
    public partial class EmpTours : Window
    {
        Context context;
        List<ComboBox> listBoxes;
        public EmpTours()
        {
            InitializeComponent();
            listBoxes = new List<ComboBox>
            {
                Employees,
                Routes,
                Dates
            };
            LoadData();
        }
        public void LoadData()
        {
            context = new Context();

            var linqQuery = context.Employees
                            .Join(context.EmpTours, em => em, et => et.Employee, (em, et) => new { Id = et.Id, Employee = em.Name, Tour = et.Tour })
                            .Join(context.Tours, emt => emt.Tour, t => t, (emt, t) => new { Id = emt.Id, Employee = emt.Employee, Route = t.Route.Name, SD = t.StartDate.ToString() });

            ETTable.ItemsSource = linqQuery.ToList();

            Routes.ItemsSource = context.Routes.Select(r => r.Name).ToList();
            Employees.ItemsSource = context.Clients.Select(c => c.Name).ToList();
            Dates.ItemsSource = context.Tours.Select(t => t.StartDate.ToString()).ToList();
        }
        public bool RecordExists(EmpTour etour)
        {
            if (context.EmpTours.Where(et => et.Tour.Route.Name == etour.Tour.Route.Name & et.Tour.StartDate == etour.Tour.StartDate & et.Employee.Name == etour.Employee.Name).Any())
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
            string emp = listBoxes[0].SelectedItem.ToString();
            string route = listBoxes[1].SelectedItem.ToString();

            if (context.EmpTours.Where(t => t.Tour.StartDate.ToString() == sd & t.Tour.Route.Name == route & t.Employee.Name == emp).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsTheSame(EmpTour t)
        {
            if (listBoxes[0].SelectedItem.ToString() == t.Employee.Name & listBoxes[1].SelectedItem.ToString() == t.Tour.Route.Name & listBoxes[2].SelectedItem.ToString() == t.Tour.StartDate.ToString())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            if (ETTable.SelectedItem != null)
            {
                int index = int.Parse(ETTable.SelectedItem.ToString().Split()[3].TrimEnd(','));
                context.EmpTours.Remove(context.EmpTours.Where(t => t.Id == index).FirstOrDefault());
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

                var newEmpTour = new EmpTour(context.Employees.Where(em => em.Name == name).FirstOrDefault(), context.Tours.Where(t => t.Route == context.Routes.Where(r => r.Name == route).FirstOrDefault()).FirstOrDefault(), DateTime.Today);

                if (RecordExists(newEmpTour))
                {
                    MessageBox.Show("Сотрудник уже назначен на этот тур.");
                }
                else
                {
                    foreach (var lb in listBoxes)
                    {
                        lb.SelectedItem = null;
                    }
                    context.EmpTours.Add(newEmpTour);
                    context.SaveChanges();
                    LoadData();
                }
            }
        }

        
    }
}
