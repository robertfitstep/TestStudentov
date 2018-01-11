using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStudentov
{
    class Otazka
    {
        public readonly string TextOtazky;
        public readonly TypOtazky Typ;
        public readonly Odpoved[] Odpovede;
        
        public Otazka(string textOtazky, Odpoved[] odpovede)
        {
            TextOtazky = textOtazky;
            Odpovede = odpovede;
            Typ = vratTypOtazky(odpovede);
            overMinPocetOdpovedi(odpovede);
        }

        public int VratSpravnuOdpoved(Otazka aktualnaOtazka)
        {
            int cisloOdpovede = 1;

            foreach (Odpoved odpoved in aktualnaOtazka.Odpovede)
            {
                if (odpoved.spravnostOdpovede) return cisloOdpovede;
                cisloOdpovede++;
            }
            return 0;
        }

        public List<int> VratSpravneOdpovede(Otazka aktualnaOtazka)
        {
            List<int> spravneOdpovede = new List<int>();
            int cisloOdpovede = 1;

            foreach (Odpoved odpoved in aktualnaOtazka.Odpovede)
            {
                if (odpoved.spravnostOdpovede) spravneOdpovede.Add(cisloOdpovede);
                cisloOdpovede++;
            }

            return spravneOdpovede;
        }

        private TypOtazky vratTypOtazky(Odpoved[] odpovede)
        {
            int pocetSpravnychOdpovedi = 0;

            foreach(Odpoved odpoved in odpovede) if (odpoved.spravnostOdpovede == true) pocetSpravnychOdpovedi++;

            if (pocetSpravnychOdpovedi == 0) throw new Exception("V kvíze musí byť aspoň jedna odpoveď správna");
            if (pocetSpravnychOdpovedi == 1) return TypOtazky.SingleChoice;
            else return TypOtazky.MultipleChoice;
        }

        private void overMinPocetOdpovedi(Odpoved[] odpovede)
        {
            if (odpovede.Length < 3) throw new Exception("Minimálny počet odpovedí je 3");
        }

    }
}
