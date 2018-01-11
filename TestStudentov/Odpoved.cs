using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStudentov
{
    class Odpoved
    {
        public readonly string textOdpovede;
        public readonly bool spravnostOdpovede;
        
        public Odpoved(string textOtazky, bool spravnostOdpovede)
        {
            this.textOdpovede = textOtazky;
            this.spravnostOdpovede = spravnostOdpovede;
        }
    }
}
