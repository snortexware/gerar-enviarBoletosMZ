using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace envioBoletos
{
    internal class Program
    {


        static void Main(string[] args)
        {
            Service service = new Service();
            service.ExtractData();
            service.ShowData();
        }
    }
}
