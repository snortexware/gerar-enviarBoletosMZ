using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using envioBoletos.MegaZap;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace envioBoletos
{
    internal class Program
    {


        public static async Task Main(string[] args)
        {
            Service service = new();


            SendData send = new SendData();


            service.InputUserData();
            await service.Loading();
            await send.StartProcessing(service.cpf, service.vencimento, service.nome, service.number);


        }
    }
}
