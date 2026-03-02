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
    /// Interaction logic for HianyzasokView.xaml
    /// </summary>
    public partial class HianyzasokView : UserControl
    {
        private MegjelenitettHianyzas _hianyzasTorlesre;

        public HianyzasokView()
        {
            InitializeComponent();
            Felhasznalo user = Adatbazis.AktualisFelhasznalo;
            if (user == null) return;

            HianyzasDatumValaszto.SelectedDate = DateTime.Now;

            if (user.IsTanar)
            {
                TanariPanel.Visibility = Visibility.Visible;
                DiakInformacioPanel.Visibility = Visibility.Collapsed;
                OszlopDiak.Visibility = Visibility.Visible;
                OszlopTorles.Visibility = Visibility.Visible;
                OsztalyBox.ItemsSource = new List<string> { "9.A", "9.B", "9.C", "10.A", "10.B", "10.C", "11.A", "11.B", "11.C", "12.A", "12.B", "12.C" };
            }
            else
            {
                TanariPanel.Visibility = Visibility.Collapsed;
                DiakInformacioPanel.Visibility = Visibility.Visible;
                OszlopDiak.Visibility = Visibility.Collapsed;
                OszlopTorles.Visibility = Visibility.Collapsed;
            }

            TablazatFrissitese();
        }

        private void OsztalyBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string osztaly = OsztalyBox.SelectedItem as string;
            if (!string.IsNullOrEmpty(osztaly))
            {
                DiakKivalasztoBox.ItemsSource = Adatbazis.Felhasznalok
                    .Where(f => f.Osztaly == osztaly && !f.IsTanar).OrderBy(f => f.Nev).ToList();
            }
            TablazatFrissitese();
        }

        private void Szuro_SelectionChanged(object sender, SelectionChangedEventArgs e) => TablazatFrissitese();

        private void TablazatFrissitese()
        {
            Felhasznalo user = Adatbazis.AktualisFelhasznalo;
            if (user == null) return;

            List<MegjelenitettHianyzas> lathato = new List<MegjelenitettHianyzas>();
            int igazoltOsszes = 0;
            int igazolatlanOsszes = 0;

            if (user.IsTanar)
            {
                string osztaly = OsztalyBox.SelectedItem as string;
                Felhasznalo valasztottDiak = DiakKivalasztoBox.SelectedItem as Felhasznalo;

                if (!string.IsNullOrEmpty(osztaly))
                {
                    foreach (var h in Adatbazis.Hianyzasok)
                    {
                        var diak = Adatbazis.Felhasznalok.FirstOrDefault(f => f.OmAzonosito == h.DiakOm && f.Osztaly == osztaly);
                        if (diak != null && (valasztottDiak == null || diak.OmAzonosito == valasztottDiak.OmAzonosito))
                        {
                            lathato.Add(new MegjelenitettHianyzas { DiakOm = h.DiakOm, DiakNev = diak.Nev, Datum = h.Datum, OraSzam = h.OraSzam, Allapot = h.IsIgazolt ? "Igazolt" : "Igazolatlan" });
                        }
                    }
                }
            }
            else
            {

                foreach (var h in Adatbazis.Hianyzasok.Where(x => x.DiakOm == user.OmAzonosito))
                {

                    int orak = 0;
                    string tisztaSzam = h.OraSzam.Replace(".", "").Replace(" óra", "").Trim();
                    int.TryParse(tisztaSzam, out orak);

                    if (h.IsIgazolt) igazoltOsszes += orak;
                    else igazolatlanOsszes += orak;

                    lathato.Add(new MegjelenitettHianyzas { DiakOm = user.OmAzonosito, DiakNev = user.Nev, Datum = h.Datum, OraSzam = h.OraSzam, Allapot = h.IsIgazolt ? "Igazolt" : "Igazolatlan" });
                }


                if (IgazoltCountText != null) IgazoltCountText.Text = igazoltOsszes.ToString();
                if (IgazolatlanCountText != null) IgazolatlanCountText.Text = igazolatlanOsszes.ToString();
            }

            HianyzasGrid.ItemsSource = lathato.OrderByDescending(x => x.Datum).ToList();
        }

        private void Button_Torles_Click(object sender, RoutedEventArgs e)
        {
            if (!Adatbazis.AktualisFelhasznalo.IsTanar) return;
            _hianyzasTorlesre = (sender as Button).DataContext as MegjelenitettHianyzas;
            if (_hianyzasTorlesre != null) TorlesAblakOverlay.Visibility = Visibility.Visible;
        }

        private void Button_Megsem_Click(object sender, RoutedEventArgs e)
        {
            TorlesAblakOverlay.Visibility = Visibility.Collapsed;
            _hianyzasTorlesre = null;
        }

        private void Button_VeglegesTorles_Click(object sender, RoutedEventArgs e)
        {
            if (_hianyzasTorlesre != null && Adatbazis.AktualisFelhasznalo.IsTanar)
            {
                var eredeti = Adatbazis.Hianyzasok.FirstOrDefault(h =>
                    h.DiakOm == _hianyzasTorlesre.DiakOm && h.Datum == _hianyzasTorlesre.Datum && h.OraSzam == _hianyzasTorlesre.OraSzam);
                if (eredeti != null) Adatbazis.Hianyzasok.Remove(eredeti);

                TorlesAblakOverlay.Visibility = Visibility.Collapsed;
                _hianyzasTorlesre = null;
                TablazatFrissitese();
            }
        }

        private void Button_HianyzasMentese_Click(object sender, RoutedEventArgs e)
        {
            Felhasznalo diak = DiakKivalasztoBox.SelectedItem as Felhasznalo;
            ComboBoxItem ora = OraBox.SelectedItem as ComboBoxItem;
            DateTime? datumValasztva = HianyzasDatumValaszto.SelectedDate;

            if (diak == null || ora == null || datumValasztva == null)
            {
                MessageBox.Show("Minden adatot válassz ki!", "Hiba");
                return;
            }

            string datumSzoveg = datumValasztva.Value.ToString("yyyy.MM.dd");

            if (Adatbazis.Hianyzasok.Any(h => h.DiakOm == diak.OmAzonosito && h.Datum == datumSzoveg))
            {
                MessageBox.Show("Ezen a napon már van rögzített hiányzás!");
                return;
            }

            Adatbazis.Hianyzasok.Add(new HianyzasBejegyzes
            {
                DiakOm = diak.OmAzonosito,
                Datum = datumSzoveg,
                OraSzam = ora.Content.ToString(),
                IsIgazolt = IgazoltCheckBox.IsChecked == true
            });

            TablazatFrissitese();
        }
    }

    public class MegjelenitettHianyzas
    {
        public string DiakOm { get; set; }
        public string DiakNev { get; set; }
        public string Datum { get; set; }
        public string OraSzam { get; set; }
        public string Allapot { get; set; }
    }
}


