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
            service.InputUserData();
            await service.LoadingTask();
        }
    }
}
