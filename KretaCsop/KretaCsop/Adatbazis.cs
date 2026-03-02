using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KretaCsop
{
    public class Felhasznalo
    {
        public string OmAzonosito { get; set; }
        public string Jelszo { get; set; }
        public bool IsTanar { get; set; }
        public string Osztaly { get; set; }
        public string Nev { get; set; }
    }

    public class JegyBejegyzes
    {
        public string DiakOm { get; set; }
        public string Tantargy { get; set; }
        public int Ertek { get; set; }
        public string Datum { get; set; }
    }

    public class HianyzasBejegyzes
    {
        public string DiakOm { get; set; }
        public string Datum { get; set; }
        public string OraSzam { get; set; }
        public bool IsIgazolt { get; set; }
    }

    public static class Adatbazis
    {
        public static List<Felhasznalo> Felhasznalok = new List<Felhasznalo>();
        public static List<JegyBejegyzes> Jegyek = new List<JegyBejegyzes>();
        public static List<HianyzasBejegyzes> Hianyzasok = new List<HianyzasBejegyzes>();

        public static Felhasznalo AktualisFelhasznalo { get; set; }

        static Adatbazis()
        {
            // Tanár hozzáadása a teszteléshez
            Felhasznalok.Add(new Felhasznalo { OmAzonosito = "tanar", Jelszo = "tanar", IsTanar = true, Nev = "Nagy Tamás" });

            Random rnd = new Random();
            string[] vezetekNevek = { "Kovács", "Szabó", "Tóth", "Varga", "Farkas", "Kiss", "Németh", "Nagy", "Balogh", "Horváth", "Papp", "Takács", "Juhász", "Mészáros", "Simon", "Szilágyi", "Fekete", "Lakatos", "Gál", "Somogyi", "Fodor", "Sipos", "Kelemen", "Hegedűs", "Kocsis", "Pintér", "Sándor", "Vincze", "Gáspár", "Antal" };
            string[] ffiKereszt = { "Bence", "Máté", "Dávid", "Gergő", "Péter", "Gábor", "László", "Zoltán", "Attila", "Tamás", "Balázs", "Patrik", "Krisztián", "Márk", "Ádám", "Bálint", "Milán", "Zsombor", "Levente", "Roland" };
            string[] noiKereszt = { "Anna", "Hanna", "Réka", "Zsófia", "Laura", "Boglárka", "Viktória", "Dóra", "Eszter", "Lilla", "Fanni", "Petra", "Nikolett", "Noémi", "Vivien", "Kitti", "Dorina", "Lili", "Luca", "Flóra" };

            string[] osztalyok = { "9.A", "9.B", "9.C", "10.A", "10.B", "10.C", "11.A", "11.B", "11.C", "12.A", "12.B", "12.C" };
            string[] tantargyak = { "Matek", "Magyar", "Töri", "Angol", "Földrajz", "Fizika", "Tesi" };

            long alapOm = 72833500000;

            DateTime startDatum = new DateTime(2025, 9, 1);
            int napokSzama = (new DateTime(2025, 11, 30) - startDatum).Days;

            foreach (var osztaly in osztalyok)
            {
                int diakokSzama = rnd.Next(15, 21);

                for (int i = 0; i < diakokSzama; i++)
                {
                    alapOm++;
                    bool isFiu = rnd.Next(0, 2) == 0;
                    string keresztNev = isFiu ? ffiKereszt[rnd.Next(ffiKereszt.Length)] : noiKereszt[rnd.Next(noiKereszt.Length)];
                    string diakNev = $"{vezetekNevek[rnd.Next(vezetekNevek.Length)]} {keresztNev}";
                    string omStr = alapOm.ToString();

                    Felhasznalok.Add(new Felhasznalo
                    {
                        OmAzonosito = omStr,
                        Jelszo = "1234",
                        IsTanar = false,
                        Osztaly = osztaly,
                        Nev = diakNev
                    });

                    // Jegyek generálása
                    foreach (var targy in tantargyak)
                    {
                        int jegyekSzama = rnd.Next(2, 4);
                        for (int j = 0; j < jegyekSzama; j++)
                        {
                            DateTime randomDatum = startDatum.AddDays(rnd.Next(napokSzama));
                            Jegyek.Add(new JegyBejegyzes
                            {
                                DiakOm = omStr,
                                Tantargy = targy,
                                Ertek = rnd.Next(1, 6),
                                Datum = randomDatum.ToString("yyyy.MM.dd")
                            });
                        }
                    }

                    // Hiányzások generálása - JAVÍTVA
                    int hianyzasokSzama = rnd.Next(0, 4);
                    for (int h = 0; h < hianyzasokSzama; h++)
                    {
                        DateTime randomDatum = startDatum.AddDays(rnd.Next(napokSzama));
                        Hianyzasok.Add(new HianyzasBejegyzes
                        {
                            DiakOm = omStr,
                            Datum = randomDatum.ToString("yyyy.MM.dd"),
                            OraSzam = $"{rnd.Next(1, 8)}. óra", // OraSzam szövegként, véletlenszerűen
                            IsIgazolt = rnd.Next(0, 2) == 0      // IsIgazolt logikai értékként
                        });
                    }
                }
            }
        }
    }
}
