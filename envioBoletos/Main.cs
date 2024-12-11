using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using envioBoletos.MegaZap;

namespace envioBoletos
{
    internal class Program
    {


        public static async Task Main(string[] args)
        {
            Service service = new Service();

            SendData send = new SendData(service);

            await send.StartProcessing();



        }
    }
}
