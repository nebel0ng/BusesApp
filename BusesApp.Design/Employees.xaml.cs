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
    /// Логика взаимодействия для Employees.xaml
    /// </summary>
    public partial class Employees : Window
    {
        Context context;
        public Employees()
        {
            InitializeComponent();
            NameBox.Clear();
            LoadData();
        }
        public void LoadData()
        {
            context = new Context();

            var linqQuery = context.EmpTypes
                            .Join(context.Employees, t => t, e => e.Type, (t, e) => new { Id = e.Id.ToString(), Name = e.Name, Type = e.Type.Name });

            EmpTable.ItemsSource = linqQuery.ToList();

            Types.ItemsSource = context.EmpTypes.Select(t => t.Name).ToList();

        }
        public bool ExistsByTextBoxes()
        {
            string name = NameBox.Text;
            string type = Types.SelectedItem.ToString();

            if (context.Employees.Where(e => e.Name == name & e.Type.Name == type).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsTheSame(Employee emp)
        {
            if (NameBox.Text == emp.Name & Types.SelectedItem.ToString() == emp.Type.Name)
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
            if (Types.SelectedItem != null & !String.IsNullOrEmpty(NameBox.Text))
            {
                var newEmp = new Employee(NameBox.Text, context.EmpTypes.Where(t => t.Name == Types.SelectedItem.ToString()).FirstOrDefault());

                if (context.Employees.Where(em => em.Name == newEmp.Name & em.Type.Name == newEmp.Type.Name).Any())
                {
                    MessageBox.Show("Информация об этом сотруднике уже есть.");
                }
                else
                {
                    NameBox.Clear();
                    context.Employees.Add(newEmp);
                    context.SaveChanges();
                    LoadData();
                }
            }
        }

        private void Button_Click_SaveChanges(object sender, RoutedEventArgs e)
        {
            if (Types.SelectedItem != null & !String.IsNullOrEmpty(NameBox.Text))
            {
                int index = int.Parse(EmpTable.SelectedItem.ToString().Split()[3].TrimEnd(','));
                var selectedEmp = context.Employees.Where(eь => eь.Id == index).FirstOrDefault();
                if (!ExistsByTextBoxes() | IsTheSame(selectedEmp))
                {
                    selectedEmp.Name = NameBox.Text;
                    selectedEmp.Type = context.EmpTypes.Where(t => t.Name == Types.SelectedItem.ToString()).FirstOrDefault();

                    NameBox.Clear();
                    context.SaveChanges();
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Информация об этом сотруднике уже есть.");
                }
            }
        }

        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            if (EmpTable.SelectedItem != null)
            {
                if (!context.Tours.ToList().Where(t => t.Bus == (EmpTable.SelectedItem as Bus)).Any())
                {
                    int index = int.Parse(EmpTable.SelectedItem.ToString().Split()[3].TrimEnd(','));
                    context.Buses.Remove(context.Buses.Where(b => b.Id == index).FirstOrDefault());
                    context.SaveChanges();
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Нельзя удалить данные о сотруднике, так как есть связанные туры.");
                }
            }
        }

        private void Button_Click_Edit(object sender, RoutedEventArgs e)
        {
            if (EmpTable.SelectedItem != null)
            {
                int index = int.Parse(EmpTable.SelectedItem.ToString().Split()[3].TrimEnd(','));
                var emp = context.Employees.Where(em => em.Id == index).FirstOrDefault();
                NameBox.Text = emp.Name;
                Types.SelectedItem = emp.Type.Name.ToString();                
            }
        }

        private void Button_Click_AddType(object sender, RoutedEventArgs e)
        {
            EmpTypes types = new EmpTypes();
            types.Show();
        }
    }
}
