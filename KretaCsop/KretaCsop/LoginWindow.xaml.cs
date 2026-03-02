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

namespace KretaCsop
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }

        private void Button_Exit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void RegSzerepkorBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RegOsztalyPanel == null) return;

            if (RegSzerepkorBox.SelectedIndex == 0)
                RegOsztalyPanel.Visibility = Visibility.Visible;
            else
                RegOsztalyPanel.Visibility = Visibility.Collapsed;
        }

        private void Button_Bejelentkezes_Click(object sender, RoutedEventArgs e)
        {
            string om = LoginOmBox.Text;
            string jelszo = LoginJelszoBox.Password;

            var user = Adatbazis.Felhasznalok.FirstOrDefault(f => f.OmAzonosito == om && f.Jelszo == jelszo);
            if (user != null)
            {
                Adatbazis.AktualisFelhasznalo = user;
                new MainWindow(user.IsTanar).Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Hibás adatok!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Regisztracio_Click(object sender, RoutedEventArgs e)
        {
            string nev = RegNevBox.Text;
            string om = RegOmBox.Text;
            string jelszo = RegJelszoBox.Password;
            string jelszo2 = RegJelszoUjraBox.Password;
            bool isTanar = (RegSzerepkorBox.SelectedIndex == 1);

            // Osztály lekérése: ha diák, a választott érték, ha tanár, akkor üres/kötőjel
            string osztaly = "-";
            if (!isTanar)
            {
                if (RegOsztalyBox.SelectedItem == null)
                {
                    MessageBox.Show("Diákregisztrációhoz válassz osztályt!");
                    return;
                }
                osztaly = (RegOsztalyBox.SelectedItem as ComboBoxItem).Content.ToString();
            }

            // Ellenőrzések
            if (string.IsNullOrWhiteSpace(nev) || om.Length != 11 || jelszo.Length < 4 || jelszo != jelszo2)
            {
                MessageBox.Show("Ellenőrizd az adatokat (OM: 11 számjegy, Jelszavak egyezése)!");
                return;
            }

            if (Adatbazis.Felhasznalok.Any(f => f.OmAzonosito == om))
            {
                MessageBox.Show("Ez az OM azonosító már létezik!");
                return;
            }

            // Regisztráció mentése
            Adatbazis.Felhasznalok.Add(new Felhasznalo
            {
                Nev = nev,
                OmAzonosito = om,
                Jelszo = jelszo,
                IsTanar = isTanar,
                Osztaly = osztaly
            });

            MessageBox.Show("Sikeres regisztráció!");

            // Mezők ürítése
            RegNevBox.Clear(); RegOmBox.Clear();
            RegJelszoBox.Clear(); RegJelszoUjraBox.Clear();
        }
    }
}
