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
    /// Логика взаимодействия для Models.xaml
    /// </summary>
    public partial class Models : Window
    {
        Context context;
        List<TextBox> textBoxes;
        public Models()
        {
            InitializeComponent();
            textBoxes = new List<TextBox>
            {
                NameBox,
                NumberBox
            };
            foreach (TextBox textBox in textBoxes)
            {
                textBox.Clear();
            }
            LoadData();
        }
        public void LoadData()
        {
            context = new Context();

            ModelsTable.ItemsSource = context.Models.ToList();
        }

        public bool ModelExists(Model model)
        {
            if (context.Models.Where(m => m.Name == model.Name & m.Capacity == model.Capacity).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ModelExistsByTextBoxes()
        {
            string name = textBoxes[0].Text;
            string number = textBoxes[1].Text;

            if (context.Models.Where(m => m.Name == name & m.Capacity.ToString() == number).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsTheSame(Model model)
        {
            if (textBoxes[0].Text == model.Name & textBoxes[1].Text == model.Capacity.ToString())
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
            if (!int.TryParse(NumberBox.Text, out int num) | num <= 0)
            {
                NumberBox.Clear();
                NumberBox.Focus();
            }
            if (!textBoxes.Where(b => String.IsNullOrEmpty(b.Text)).Any())
            {
                var newModel = new Model(textBoxes[0].Text, textBoxes[1].Text);
                foreach (TextBox textBox in textBoxes)
                {
                    textBox.Clear();
                }
                if (ModelExists(newModel))
                {
                    MessageBox.Show("Информация об этой модели уже есть.");
                }
                else
                {
                    context.Models.Add(newModel);
                    context.SaveChanges();
                    LoadData();
                }
            }
        }

        private void Button_Click_SaveChanges(object sender, RoutedEventArgs e)
        {
            if (ModelsTable.SelectedItem != null & !textBoxes.Where(b => String.IsNullOrEmpty(b.Text)).Any())
            {
                if (!ModelExistsByTextBoxes() | IsTheSame(ModelsTable.SelectedItem as Model))
                {
                    (ModelsTable.SelectedItem as Client).Name = textBoxes[0].Text;
                    (ModelsTable.SelectedItem as Client).PhoneNumber = textBoxes[1].Text;
                    foreach (TextBox textBox in textBoxes)
                    {
                        textBox.Clear();
                    }
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
            if (ModelsTable.SelectedItem != null)
            {
                if (!context.Buses.ToList().Where(b => b.Model == (ModelsTable.SelectedItem as Model)).Any())
                {
                    context.Models.Remove(ModelsTable.SelectedItem as Model);
                    context.SaveChanges();
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Нельзя удалить данные о модели, так как есть связанные автобусы.");
                }

            }
        }

        private void Button_Click_Edit(object sender, RoutedEventArgs e)
        {
            if (ModelsTable.SelectedItem != null)
            {
                textBoxes[0].Text = (ModelsTable.SelectedItem as Model).Name;
                textBoxes[1].Text = (ModelsTable.SelectedItem as Model).Capacity.ToString();
            }
        }
    }
}
