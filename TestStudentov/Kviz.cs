using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestStudentov
{
    class Kviz
    {
        private Otazka[] Otazky;
        private int[] cislaVylosovanychOtazok;
        private int indexOtazky;
        private Otazka aktualnaOtazka;
        private List<int> studentoveOdpovede;
        private int studentoveBody;

        public Kviz()
        {
            Otazky = nadefinujOtazky();
            cislaVylosovanychOtazok = vylosujOtazky();
            indexOtazky = 0;
            studentoveBody = 0;
        }

        public void OpytajSaOtazku()
        {
            if (indexOtazky < cislaVylosovanychOtazok.Length)
            {
                vypisOtazku();
                vypytajSiOdpoved();
            }
            else
            {
                Console.WriteLine("\nŠtudent získal v kvíze spolu {0} bodov.", studentoveBody);
                Console.ReadLine();
            }

        }

        public void VypisVsetkyOtazkyAOdpovede()
        {
            foreach (Otazka otazka in Otazky)
            {
                int i = 1;
                Console.WriteLine("{0} ({1})", otazka.TextOtazky, otazka.Typ);
                foreach (Odpoved odpoved in otazka.Odpovede)
                {
                    Console.WriteLine("({0}) {1} {2}", i, odpoved.textOdpovede, odpoved.spravnostOdpovede);
                    i++;
                }
                Console.WriteLine("\n");
            }
            Console.ReadLine();
        }

        private bool validujAVratVstupUzivatela(string vstupUzivatela, out List<int> cisla)
        {
            cisla = new List<int>();
            int c;

            if (aktualnaOtazka.Typ == TypOtazky.SingleChoice && vstupUzivatela.Length == 1 && 
                Int32.TryParse(vstupUzivatela, out c) && (c == 1 || c == 2 || c == 3))
            {
                cisla.Add(c);
                return true;
            }
            if (aktualnaOtazka.Typ == TypOtazky.MultipleChoice)
            {
                int pocetCisiel = validujCiarkyADlzku(vstupUzivatela);
                cisla = vratCislaZoVstupUz(vstupUzivatela, pocetCisiel);
                return true;
            }
            else
            {
                cisla = new List<int>();
                return false;
            }

        }   

        private int validujCiarkyADlzku(string vstupUzivatela)
        {
            switch (vstupUzivatela.Length)
            {
                case 1:
                    return 1;
                case 3:
                    if (vstupUzivatela.Substring(1, 1) == ",") return 2;
                    else goto default;
                case 5:
                    if (vstupUzivatela.Substring(1, 1) == "," && vstupUzivatela.Substring(3, 1) == ",") return 3;
                    else goto default;
                default:
                    zleZadVstupUzivatela();
                    return 0;
            }
        }

        private List<int> vratCislaZoVstupUz(string vstupUzivatela, int pocetCisiel)
        {
            int c;
            int i=0;
            List<int> cisla = new List<int>();
            
            for (int _=0; _ < pocetCisiel; _++)
            {
                if (Int32.TryParse(vstupUzivatela.Substring(i, 1), out c) && c == 1 || c == 2 || c == 3)
                {
                    cisla.Add(c);
                    i = i + 2;
                }
                else zleZadVstupUzivatela();
            }

            return cisla;
        }


        private void vypytajSiOdpoved()
        {
            Console.WriteLine("\nZadajte {0}", zadJedAleboViacOdp());
            if (validujAVratVstupUzivatela(Console.ReadLine(), out studentoveOdpovede))
            {
                studentoveBody = studentoveBody + vyhodnotOdpovede();
                //Console.WriteLine(studentoveBody);
                OpytajSaOtazku();
            }
            else zleZadVstupUzivatela();
        }

        private void zleZadVstupUzivatela()
        {
            Console.WriteLine("Zadali ste nesprávny vstup.");
            vypytajSiOdpoved();
        }

        private bool vstupJe123(string pismeno, out int cislo)
        {
            switch (pismeno)
            {
                case "1":
                case "2":
                case "3":
                cislo = Int32.Parse(pismeno);
                return true;
                default:
                cislo = 0;
                return false;
            }
        }

        private string zadJedAleboViacOdp()
        {
            if (aktualnaOtazka.Typ == TypOtazky.SingleChoice) return "číslo správnej odpovede.";
            if (aktualnaOtazka.Typ == TypOtazky.MultipleChoice) return "jednu alebo viac odpovedí oddelených čiarkou.\nnapr.1,2";
            else return "";
        }

        private void vypisOtazku()
        {
            int cisloOdpovede = 1;
            
            aktualnaOtazka = Otazky[cislaVylosovanychOtazok[indexOtazky]];
            Console.WriteLine(aktualnaOtazka.TextOtazky);
            foreach (Odpoved odpoved in aktualnaOtazka.Odpovede)
             {
                Console.WriteLine("{0} {1}", cisloOdpovede, odpoved.textOdpovede);
                cisloOdpovede++;
             }
            indexOtazky++;
        }
        
        private int[] vylosujOtazky()
        {
            System.Random rnd = new System.Random();
            int[] zamiesaneOtazky = Enumerable.Range(0, 9).OrderBy(r => rnd.Next()).ToArray();
            return zamiesaneOtazky.Take(5).ToArray();

        }

        private int vyhodnotOdpovede()
        {
            if (aktualnaOtazka.Typ == TypOtazky.SingleChoice && studentoveOdpovede[0] == aktualnaOtazka.VratSpravnuOdpoved(aktualnaOtazka)) return 1;
            if (aktualnaOtazka.Typ == TypOtazky.MultipleChoice) return vyhodnotMultipleChoice();
            else return 0;
        }

        private int vyhodnotMultipleChoice()
        {
            int studentoveBody = 0;

            foreach(int studentovaOdpoved in studentoveOdpovede)
            {
                bool najdenaOdpoved = false;
                foreach(int spravnaOdpoved in aktualnaOtazka.VratSpravneOdpovede(aktualnaOtazka))
                {
                    if (spravnaOdpoved == studentovaOdpoved) najdenaOdpoved=true;
                }
                if (najdenaOdpoved) studentoveBody++;
                else studentoveBody--;
            }
            return studentoveBody;
        }

        private Otazka[] nadefinujOtazky()
        {
            Otazka[] otazky = new Otazka[10];

            otazky[0] = new Otazka(
                "Keď morské vydry spia, tak...?",
                new Odpoved[3]
                {
                    new Odpoved("Veľmi nahlas chrápu.", false),
                    new Odpoved("Držia sa za labky.", true),
                    new Odpoved("Prikrývajú si oči morskými riasami.", false)
                }
                );

            otazky[1] = new Otazka(
                "Aký je genitív plurálu od slova mäso?",
                new Odpoved[3]
                {
                    new Odpoved("mies", false),
                    new Odpoved("mäsov", false),
                    new Odpoved("mias", true)
                }
                );

            otazky[2] = new Otazka(
                "Vedci Darryl Gwynne a David Rentz zistili, že...?",
                new Odpoved[3]
                {
                    new Odpoved("Austrálske chrobáky sa chcú páriť s fľaškami.", true),
                    new Odpoved("Austrálske športy si rozprávajú vtipy.", false),
                    new Odpoved("Austrálske raky jedia pneumatiky.", false)
                }
                );

            otazky[3] = new Otazka(
                "Ako zabránime nepríjemnému stekaniu svieťčok?",
                new Odpoved[3]
                {
                    new Odpoved("Potrieme ich cmarom.", false),
                    new Odpoved("Ponoríme ich do slanej vody", true),
                    new Odpoved("Vyložíme ich na priame slnko.", false)
                }
                );

            otazky[4] = new Otazka(
                "V ktorých z uvedených okresov na evidenčnom čísle vozidla nenájdete túto skratku: \"SV\"?",
                new Odpoved[3]
                {
                    new Odpoved("Sabinov", true),
                    new Odpoved("Svidník", true),
                    new Odpoved("Snina", false)
                }
                );

            otazky[5] = new Otazka(
                "Pohľad na človeka trasúceho sa od zimy...",
                new Odpoved[3]
                {
                    new Odpoved("Spôsobuje zníženie telesnej teploty.", true),
                    new Odpoved("Spôsobuje prudké návaly hladu.", false),
                    new Odpoved("Ovplivňuje našu rovnováhu.", false)
                }
                );

            otazky[6] = new Otazka(
                "U koho sa často vyskytuje \"hallux valgus\"?",
                new Odpoved[3]
                {
                    new Odpoved("U mužov, ktorí nosia príliš úzke nohavice.", false),
                    new Odpoved("U žien, ktoré nosia topánky na vysokých podpätkoch.", true),
                    new Odpoved("U šoférov z povolania.", false)
                }
                );

            otazky[7] = new Otazka(
                "Ktorá činnosť nás najlepšie upokojí, keď sme nervózni?",
                new Odpoved[3]
                {
                    new Odpoved("U mužov, ktorí nosia príliš úzke nohavice.", false),
                    new Odpoved("U žien, ktoré nosia topánky na vysokých podpätkoch.", true),
                    new Odpoved("U šoférov z povolania.", false)
                }
                );

            otazky[8] = new Otazka(
                "Ktoré farby sú na francúzskej vlajke?",
                new Odpoved[3]
                {
                    new Odpoved("Modrá", true),
                    new Odpoved("Zelená", false),
                    new Odpoved("Biela", true)
                }
                );

            otazky[9] = new Otazka(
                "Ktoré obce patria do okresu Rimavská sobota?",
                new Odpoved[3]
                {
                    new Odpoved("Hnúšťa", true),
                    new Odpoved("Bátka", true),
                    new Odpoved("Tornaľa", false)
                }
                );

            return otazky;
        }
    }
}
