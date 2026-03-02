using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for JegyekView.xaml
    /// </summary>
    public partial class JegyekView : UserControl
    {
        private MegjelenitettJegy _jegyTorlesre;

        public JegyekView()
        {
            InitializeComponent();
            Felhasznalo user = Adatbazis.AktualisFelhasznalo;
            if (user == null) return;

            JegyDatumValaszto.SelectedDate = DateTime.Now;

            if (user.IsTanar)
            {
                TanariPanel.Visibility = Visibility.Visible;
                DiakSzuroPanel.Visibility = Visibility.Collapsed;
                OszlopDiakNev.Visibility = Visibility.Visible;
                OszlopOsztaly.Visibility = Visibility.Visible;
                OszlopTorles.Visibility = Visibility.Visible; // Tanár látja

                OsztalyBox.ItemsSource = new List<string> { "9.A", "9.B", "9.C", "10.A", "10.B", "10.C", "11.A", "11.B", "11.C", "12.A", "12.B", "12.C" };
                TantargyBox.ItemsSource = new List<string> { "Matek", "Magyar", "Töri", "Angol", "Földrajz", "Fizika", "Tesi" };
            }
            else
            {
                TanariPanel.Visibility = Visibility.Collapsed;
                DiakSzuroPanel.Visibility = Visibility.Visible;
                OszlopDiakNev.Visibility = Visibility.Collapsed;
                OszlopOsztaly.Visibility = Visibility.Collapsed;
                OszlopTorles.Visibility = Visibility.Collapsed; // Diák számára ELTŰNIK a törlés oszlop

                DiakTantargyBox.ItemsSource = new List<string> { "Minden tantárgy", "Matek", "Magyar", "Töri", "Angol", "Földrajz", "Fizika", "Tesi" };
                DiakTantargyBox.SelectedIndex = 0;
            }
            TablazatFrissitese();
        }

        private void OsztalyBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string valasztottOsztaly = OsztalyBox.SelectedItem as string;
            if (!string.IsNullOrEmpty(valasztottOsztaly))
            {
                DiakKivalasztoBox.ItemsSource = Adatbazis.Felhasznalok.Where(f => f.Osztaly == valasztottOsztaly && !f.IsTanar).OrderBy(f => f.Nev).ToList();
            }
            TablazatFrissitese();
        }

        private void Szuro_SelectionChanged(object sender, SelectionChangedEventArgs e) => TablazatFrissitese();

        private void TablazatFrissitese()
        {
            Felhasznalo user = Adatbazis.AktualisFelhasznalo;
            if (user == null) return;

            List<MegjelenitettJegy> lathatoJegyek = new List<MegjelenitettJegy>();
            double jegyekOsszege = 0;
            int jegyekDarab = 0;

            if (user.IsTanar)
            {
                string valasztottOsztaly = OsztalyBox.SelectedItem as string;
                string valasztottTantargy = TantargyBox.SelectedItem as string;
                Felhasznalo valasztottDiak = DiakKivalasztoBox.SelectedItem as Felhasznalo;

                if (string.IsNullOrEmpty(valasztottOsztaly) || string.IsNullOrEmpty(valasztottTantargy))
                {
                    JegyekTablazat.ItemsSource = null;
                    AtlagTextBlock.Text = "-";
                    return;
                }

                foreach (var jegy in Adatbazis.Jegyek.Where(j => j.Tantargy == valasztottTantargy))
                {
                    var diak = Adatbazis.Felhasznalok.FirstOrDefault(f => f.OmAzonosito == jegy.DiakOm && f.Osztaly == valasztottOsztaly);
                    if (diak != null && (valasztottDiak == null || diak.OmAzonosito == valasztottDiak.OmAzonosito))
                    {
                        lathatoJegyek.Add(new MegjelenitettJegy { DiakOm = diak.OmAzonosito, Osztaly = diak.Osztaly, DiakNeve = diak.Nev, Tantargy = jegy.Tantargy, Ertek = jegy.Ertek, Datum = jegy.Datum });
                        jegyekOsszege += jegy.Ertek;
                        jegyekDarab++;
                    }
                }
            }
            else
            {
                string valasztottTantargy = DiakTantargyBox.SelectedItem as string;
                bool mutassMindent = valasztottTantargy == "Minden tantárgy" || string.IsNullOrEmpty(valasztottTantargy);
                foreach (var jegy in Adatbazis.Jegyek.Where(j => j.DiakOm == user.OmAzonosito))
                {
                    if (mutassMindent || jegy.Tantargy == valasztottTantargy)
                    {
                        lathatoJegyek.Add(new MegjelenitettJegy { DiakOm = user.OmAzonosito, Osztaly = user.Osztaly, DiakNeve = user.Nev, Tantargy = jegy.Tantargy, Ertek = jegy.Ertek, Datum = jegy.Datum });
                        jegyekOsszege += jegy.Ertek;
                        jegyekDarab++;
                    }
                }
            }

            AtlagTextBlock.Text = jegyekDarab > 0 ? Math.Round(jegyekOsszege / jegyekDarab, 2).ToString() : "-";
            JegyekTablazat.ItemsSource = lathatoJegyek.OrderByDescending(x => x.Datum).ToList();
        }

        private void Button_Torles_Click(object sender, RoutedEventArgs e)
        {
            if (!Adatbazis.AktualisFelhasznalo.IsTanar) return;
            _jegyTorlesre = (sender as Button).DataContext as MegjelenitettJegy;
            if (_jegyTorlesre != null) TorlesAblakOverlay.Visibility = Visibility.Visible;
        }

        private void Button_Megsem_Click(object sender, RoutedEventArgs e)
        {
            TorlesAblakOverlay.Visibility = Visibility.Collapsed;
            _jegyTorlesre = null;
        }

        private void Button_VeglegesTorles_Click(object sender, RoutedEventArgs e)
        {
            if (_jegyTorlesre != null && Adatbazis.AktualisFelhasznalo.IsTanar)
            {
                var target = Adatbazis.Jegyek.FirstOrDefault(j =>
                    j.DiakOm == _jegyTorlesre.DiakOm &&
                    j.Tantargy == _jegyTorlesre.Tantargy &&
                    j.Ertek == _jegyTorlesre.Ertek &&
                    j.Datum == _jegyTorlesre.Datum);

                if (target != null) Adatbazis.Jegyek.Remove(target);
                TorlesAblakOverlay.Visibility = Visibility.Collapsed;
                _jegyTorlesre = null;
                TablazatFrissitese();
            }
        }

        private void Button_UjJegyMentese_Click(object sender, RoutedEventArgs e)
        {
            Felhasznalo diak = DiakKivalasztoBox.SelectedItem as Felhasznalo;
            string tárgy = TantargyBox.SelectedItem as string;
            ComboBoxItem jegy = JegyBox.SelectedItem as ComboBoxItem;
            DateTime? datum = JegyDatumValaszto.SelectedDate;

            if (diak == null || string.IsNullOrEmpty(tárgy) || jegy == null || datum == null)
            {
                MessageBox.Show("Minden adatot válassz ki!", "Hiba");
                return;
            }

            Adatbazis.Jegyek.Add(new JegyBejegyzes { DiakOm = diak.OmAzonosito, Tantargy = tárgy, Ertek = int.Parse(jegy.Content.ToString()), Datum = datum.Value.ToString("yyyy.MM.dd") });
            TablazatFrissitese();
        }
    }

    public class MegjelenitettJegy
    {
        public string DiakOm { get; set; }
        public string Osztaly { get; set; }
        public string DiakNeve { get; set; }
        public string Tantargy { get; set; }
        public int Ertek { get; set; }
        public string Datum { get; set; }
    }
}

