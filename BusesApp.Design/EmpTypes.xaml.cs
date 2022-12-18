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
    /// Логика взаимодействия для EmpTypes.xaml
    /// </summary>
    public partial class EmpTypes : Window
    {
        Context context;
        public EmpTypes()
        {
            InitializeComponent();
            NameBox.Clear();
            LoadData();
        }
        public void LoadData()
        {
            context = new Context();

            TypeTable.ItemsSource = context.EmpTypes.ToList();
        }

        public bool TypeExists(EmpType type)
        {
            if (context.EmpTypes.Where(t => t.Name == type.Name).Any())
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

            if (context.EmpTypes.Where(t => t.Name == name).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsTheSame(EmpType type)
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
                var newType = new EmpType(NameBox.Text);
                NameBox.Clear();

                if (TypeExists(newType))
                {
                    MessageBox.Show("Информация об этой должности уже есть.");
                }
                else
                {
                    context.EmpTypes.Add(newType);
                    context.SaveChanges();
                    LoadData();
                }
            }
        }

        private void Button_Click_SaveChanges(object sender, RoutedEventArgs e)
        {
            if (TypeTable.SelectedItem != null & !String.IsNullOrEmpty(NameBox.Text))
            {
                if (!ExistsByTextBoxes() | IsTheSame(TypeTable.SelectedItem as EmpType))
                {
                    (TypeTable.SelectedItem as EmpType).Name = NameBox.Text;
                    NameBox.Clear();
                    context.SaveChanges();
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Информация об этой должности уже есть.");
                }
            }
        }

        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            if (TypeTable.SelectedItem != null)
            {
                if (!context.Employees.ToList().Where(em => em.Type == (TypeTable.SelectedItem as EmpType)).Any())
                {
                    context.EmpTypes.Remove(TypeTable.SelectedItem as EmpType);
                    context.SaveChanges();
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Нельзя удалить данные о должности, так как есть связанные сотрудники.");
                }

            }
        }

        private void Button_Click_Edit(object sender, RoutedEventArgs e)
        {
            if (TypeTable.SelectedItem != null)
            {
                NameBox.Text = (TypeTable.SelectedItem as EmpType).Name;
            }
        }
    }
}
