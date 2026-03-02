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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KretaCsop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isTanar;

        public MainWindow(bool isTanar)
        {
            InitializeComponent();
            _isTanar = isTanar;


            MainContent.Content = new OrarendView();


            if (_isTanar)
            {
                this.Title = "Kréta - Tanári Felület";
            }
            else
            {
                this.Title = "Kréta - Diák Felület";
            }

            FrissitProfilAdatokat();
        }

        private void FrissitProfilAdatokat()
        {
            if (Adatbazis.AktualisFelhasznalo != null)
            {
                ProfilNevText.Text = Adatbazis.AktualisFelhasznalo.Nev.ToUpper();

                if (Adatbazis.AktualisFelhasznalo.IsTanar)
                {
                    ProfilSzerepText.Text = "TANÁR";
                }
                else
                {
                    ProfilSzerepText.Text = "DIÁK (" + Adatbazis.AktualisFelhasznalo.Osztaly + ")";
                }
            }
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                this.DragMove();
        }

        private void Button_Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private void Button_OrarendBTN_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new OrarendView();
        }

        private void Button_JegyekBTN_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new JegyekView();
        }

        private void Button_HianyzasokBTN_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new HianyzasokView();
        }

        private void Button_Kijelentkezes_Click(object sender, RoutedEventArgs e)
        {
            Adatbazis.AktualisFelhasznalo = null;
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}
