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
    /// Логика взаимодействия для Clients.xaml
    /// </summary>
    public partial class Clients : Window
    {
        Context context;
        List<TextBox> textBoxes;
        public Clients()
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

            var linqQuery = context.Countries
                            .Join(context.Clients, country => country, cl => cl.Citizenship, (country, cl) => new { Id = cl.Id, Name = cl.Name, PhoneNumber = cl.PhoneNumber, BirthDate = cl.BirthDate.ToString(), Country = cl.Citizenship.Name});;
                            
            ClientTable.ItemsSource = linqQuery.ToList();

            Countries.ItemsSource = context.Countries.Select(c => c.Name).ToList();

        }

        public bool ClientExists(Client client)
        {
            if (context.Clients.Where(c => c.Name == client.Name & c.Citizenship.Name == client.Citizenship.Name & c.PhoneNumber == client.PhoneNumber & c.BirthDate == client.BirthDate).Any())
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
            string name = textBoxes[0].Text;
            string number = textBoxes[1].Text;
            DateTime? bdate = BDatePicker.SelectedDate;
            string country = Countries.SelectedItem.ToString();

            if (context.Clients.Where(c => c.Name == name & c.PhoneNumber == number & c.BirthDate == bdate & c.Citizenship.Name == country).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsTheSame(Client client)
        {
            if (textBoxes[0].Text == client.Name & textBoxes[1].Text == client.PhoneNumber & BDatePicker.SelectedDate == client.BirthDate & Countries.SelectedItem == client.Citizenship.Name)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Button_Click_AddCountry(object sender, RoutedEventArgs e)
        {
            Countries countries = new Countries();
            countries.Show();

        }

        private void Button_Click_SaveChanges(object sender, RoutedEventArgs e)
        {
            if (!textBoxes.Where(b => b.Text == null).Any() & (Countries.SelectedItem != null) & (BDatePicker.SelectedDate != null))
            {
                int index = int.Parse(ClientTable.SelectedItem.ToString().Split()[3].TrimEnd(','));
                var selectedClient = context.Clients.Where(c => c.Id == index).FirstOrDefault();
                if (!ExistsByTextBoxes() | IsTheSame(selectedClient))
                {
                    selectedClient.BirthDate = BDatePicker.SelectedDate;
                    selectedClient.Name = textBoxes[0].Text;
                    selectedClient.PhoneNumber = textBoxes[1].Text;
                    selectedClient.Citizenship = context.Countries.Where(c => c.Name == Countries.SelectedItem.ToString()).FirstOrDefault();

                    foreach (var box in textBoxes)
                    {
                        box.Clear();
                    }
                    BDatePicker.SelectedDate = null;
                    Countries.SelectedItem = null;
                    context.SaveChanges();
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Информация об этой записи уже есть.");
                }
            }
        }

        private void Button_Click_Edit(object sender, RoutedEventArgs e)
        {
            if (ClientTable.SelectedItem != null)
            {
                int index = int.Parse(ClientTable.SelectedItem.ToString().Split()[3].TrimEnd(','));
                var selectedClient = context.Clients.Where(c => c.Id == index).FirstOrDefault();
                textBoxes[0].Text = selectedClient.Name;
                textBoxes[1].Text = selectedClient.PhoneNumber;
                BDatePicker.SelectedDate = selectedClient.BirthDate;
                Countries.SelectedItem = selectedClient.Citizenship.Name;
            }
        }

        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            if (ClientTable.SelectedItem != null)
            {
                int index = int.Parse(ClientTable.SelectedItem.ToString().Split()[3].TrimEnd(','));
                context.Clients.Remove(context.Clients.Where(c => c.Id == index).FirstOrDefault());
                context.SaveChanges();
                LoadData();
                //if (!context.Clients.ToList().Where( => a.Client == (ClientTable.SelectedItem as Client)).Any())
                //{
                    
                //}
                //else
                //{
                //    MessageBox.Show("Нельзя удалить данные о клиенте, так как есть связанные записи.");
                //}

            }
        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            if (!textBoxes.Where(b => String.IsNullOrEmpty(b.Text)).Any() & Countries.SelectedItem != null & BDatePicker.SelectedDate != null)
            {
                var newClient = new Client(textBoxes[0].Text, BDatePicker.SelectedDate, textBoxes[1].Text, context.Countries.Where(c => c.Name == Countries.SelectedItem.ToString()).FirstOrDefault());
                foreach (TextBox textBox in textBoxes)
                {
                    textBox.Clear();
                }
                BDatePicker.SelectedDate = null;
                Countries.SelectedItem = null;
                if (ClientExists(newClient))
                {
                    MessageBox.Show("Информация об этом клиенте уже есть.");
                }
                else
                {
                    context.Clients.Add(newClient);
                    context.SaveChanges();
                    LoadData();
                }
            }
        }
    }
}
