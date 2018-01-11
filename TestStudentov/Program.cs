using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStudentov
{
    class Program
    {
        static void Main(string[] args)
        {
            Kviz MojKviz = new Kviz();
            //MojKviz.VypisVsetkyOtazkyAOdpovede();
            MojKviz.OpytajSaOtazku();
        }
    }
}
