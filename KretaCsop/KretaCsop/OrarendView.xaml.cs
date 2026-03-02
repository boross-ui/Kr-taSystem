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
    /// Interaction logic for OrarendView.xaml
    /// </summary>
    public partial class OrarendView : UserControl
    {
        public OrarendView()
        {
            InitializeComponent();
            ÓrarendBetöltése();
        }

        private void ÓrarendBetöltése()
        {

            Felhasznalo user = Adatbazis.AktualisFelhasznalo;
            if (user == null) return;

            List<OrarendBejegyzes> orarendLista = new List<OrarendBejegyzes>();

            if (user.IsTanar)
            {
                OrarendCim.Text = $"TANÁRI ÓRAREND - {user.Nev.ToUpper()}";

                orarendLista.Add(new OrarendBejegyzes { Idoszak = "1. (8:00)", Hetfo = "Matek (9.A)\nT: 101", Kedd = "-", Szerda = "Földrajz (10.C)\nT: 204", Csutortok = "Matek (9.A)\nT: 101", Pentek = "-" });
                orarendLista.Add(new OrarendBejegyzes { Idoszak = "2. (8:55)", Hetfo = "Földrajz (11.B)\nT: 202", Kedd = "Matek (12.A)\nT: 101", Szerda = "-", Csutortok = "Földrajz (11.B)\nT: 202", Pentek = "Matek (12.A)\nT: 101" });
                orarendLista.Add(new OrarendBejegyzes { Idoszak = "3. (9:50)", Hetfo = "-", Kedd = "Földrajz (9.C)\nT: 204", Szerda = "Matek (12.A)\nT: 101", Csutortok = "-", Pentek = "Földrajz (9.B)\nT: 202" });
                orarendLista.Add(new OrarendBejegyzes { Idoszak = "4. (10:55)", Hetfo = "Fogadóóra\nT: Tanári", Kedd = "-", Szerda = "-", Csutortok = "Földrajz (10.C)\nT: 204", Pentek = "-" });
                orarendLista.Add(new OrarendBejegyzes { Idoszak = "5. (11:50)", Hetfo = "-", Kedd = "Matek (9.A)\nT: 101", Szerda = "Földrajz (9.B)\nT: 202", Csutortok = "-", Pentek = "Matek (9.A)\nT: 101" });
                orarendLista.Add(new OrarendBejegyzes { Idoszak = "6. (12:45)", Hetfo = "-", Kedd = "-", Szerda = "-", Csutortok = "-", Pentek = "-" });
            }
            else
            {
                OrarendCim.Text = $"DIÁK ÓRAREND - {user.Osztaly} ({user.Nev.ToUpper()})";

                string emeltTargy = "Emelt Történelem";
                if (user.Osztaly.Contains("A")) emeltTargy = "Emelt Matek";
                else if (user.Osztaly.Contains("C")) emeltTargy = "Emelt Angol";

                orarendLista.Add(new OrarendBejegyzes { Idoszak = "1. (8:00)", Hetfo = $"{emeltTargy}\nNagy T.", Kedd = "Irodalom\nKiss M.", Szerda = "Nyelvtan\nKiss M.", Csutortok = "Tesi\nErős B.", Pentek = "Töri\nVarga E." });
                orarendLista.Add(new OrarendBejegyzes { Idoszak = "2. (8:55)", Hetfo = "Matek\nSzabó K.", Kedd = "Földrajz\nTóth A.", Szerda = $"{emeltTargy}\nNagy T.", Csutortok = "Matek\nSzabó K.", Pentek = "Irodalom\nKiss M." });
                orarendLista.Add(new OrarendBejegyzes { Idoszak = "3. (9:50)", Hetfo = "Töri\nVarga E.", Kedd = "Angol\nKovács J.", Szerda = "Matek\nSzabó K.", Csutortok = "Földrajz\nTóth A.", Pentek = "Angol\nKovács J." });
                orarendLista.Add(new OrarendBejegyzes { Idoszak = "4. (10:55)", Hetfo = "Nyelvtan\nKiss M.", Kedd = "Tesi\nErős B.", Szerda = "Fizika\nNémeth L.", Csutortok = "Töri\nVarga E.", Pentek = $"{emeltTargy}\nNagy T." });
                orarendLista.Add(new OrarendBejegyzes { Idoszak = "5. (11:50)", Hetfo = "Angol\nKovács J.", Kedd = "Osztályfőnöki\nVarga E.", Szerda = "Tesi\nErős B.", Csutortok = "Irodalom\nKiss M.", Pentek = "Fizika\nNémeth L." });
                orarendLista.Add(new OrarendBejegyzes { Idoszak = "6. (12:45)", Hetfo = "Fizika\nNémeth L.", Kedd = "-", Szerda = "Angol\nKovács J.", Csutortok = $"{emeltTargy}\nNagy T.", Pentek = "Matek\nSzabó K." });
                orarendLista.Add(new OrarendBejegyzes { Idoszak = "7. (13:40)", Hetfo = "-", Kedd = "-", Szerda = "-", Csutortok = "Ének\nFehér Z.", Pentek = "-" });
            }

            OrarendGrid.ItemsSource = orarendLista;
        }
    }

    public class OrarendBejegyzes
    {
      
        public string Idoszak { get; set; }
        public string Hetfo { get; set; }
        public string Kedd { get; set; }
        public string Szerda { get; set; }
        public string Csutortok { get; set; }
        public string Pentek { get; set; }
    }
}

