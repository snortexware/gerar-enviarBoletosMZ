using System.Text.RegularExpressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Globalization;
using OpenQA.Selenium.Interactions;
using static System.Collections.Specialized.BitVector32;

public class Service
{
    IWebDriver driver;
    string codClient;
    string valor;
    string referente;
    string cpf;
    string vencimento;
    string nome;



    public void ExtractData()
    {
        try
        {
            ChromeOptions options = new ChromeOptions();

            // Desativar a exibição de logs do ChromeDriver
            options.AddArgument("--silent");
            options.AddArgument("--log-level=3");
            options.AddArgument("--headless=old");


            Console.Write("Digite o codigo do cliente: ");
            codClient = Console.ReadLine();
            Console.Write("Digite o valor da visita em R$: ");
            valor = Console.ReadLine();
            Console.Write("A cobrança é referente a? ");
            referente = Console.ReadLine();



            driver = new ChromeDriver(options);

            driver.Navigate().GoToUrl($"https://btv2.ksys.net.br/gramnet/bemtevi/cadastros/cliente.php?codCliente={codClient}");



            driver.FindElement(By.Name("usuario")).SendKeys("lucas.moreira");
            driver.FindElement(By.Name("senha")).SendKeys("Lucas.moreira1");
            driver.FindElement(By.Name("acao")).Click();



            string href = $"codCliente={codClient}";

            // Obrigado Chat GPT =)
            IWebElement elemento = driver.FindElement(By.XPath($"//a[starts-with(@href, 'cob_admin.php?codigo=') and contains(@href, 'codCliente={codClient}')]"));

            string textoDoElemento = elemento.Text;

            IWebElement extractedName = driver.FindElement(By.TagName("h1"));

            nome = extractedName.Text;

            // Um dia eu aprendo a usar REGEX
            vencimento = Regex.Replace(textoDoElemento, @"\D", "");


            IList<IWebElement> h3 = driver.FindElements(By.TagName("h3"));

            string extractedValue = h3[1].Text;


            cpf = Regex.Replace(extractedValue, @"\D*(\d{3})\.(\d{3})\.(\d{3})-(\d{2})", "$3-$4");


        }
        catch
        {
            throw new Exception("erro");
        }


    }

    public void ShowData()
    {





        Console.WriteLine("");

        Console.WriteLine(format: "{0, -10} {1}",
           "Codigo do cliente:",
           codClient);
        Console.WriteLine(format: "{0, -10} {1}",
          "Nome:",
          nome);


        Console.WriteLine(format: "{0, -10} {1}",
           "Valor da cobrança R$:",
           valor);

        Console.WriteLine(format: "{0, -10} {1}",
           "Descrição da cobrança:",
           referente);

        Console.WriteLine(format: "{0} {1}",
           "CPF: ",
           cpf);

        Console.WriteLine(format: "{0, -10} {1}",
          "Vencimento: ",
          vencimento);


        Console.WriteLine("_____________________________________");


        Console.WriteLine("Os dados estão de acordo? Digite yes ou no: ");
        string confirm = Console.ReadLine();

        if (confirm != "yes")
        {
            Console.WriteLine("missão abortada");
        }
        else
        {

            driver.Navigate().GoToUrl($"https://btv2.ksys.net.br/gramnet/bemtevi/cadastros/adicionar_especiais_novo.php?codCliente={codClient}");

            IList<IWebElement> Atag = driver.FindElements(By.TagName("a"));

            Atag[98].Click();




            Actions action = new Actions(driver);


            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript(@"
    var select = document.getElementById('tipoCobrancaCodigo');
    select.value = '44|1';
    select.dispatchEvent(new Event('change'));
");


            IWebElement description = driver.FindElement(By.Id("descricao"));
            description.SendKeys(referente);

            DateTime dt = DateTime.Now;

            int Day = dt.Day;
            int Month = dt.Month;
            int Year = dt.Year;


            int vencimentoNumber = int.Parse(vencimento);
            DateTime time = new DateTime(dt.Year, dt.Month, vencimentoNumber);

            double totalDays = Math.Floor((time - dt).TotalDays);


            if (totalDays < 20)
            {
                dt = dt.AddMonths(2);
            }
            else
            {
                dt = dt.AddMonths(1);
            }

            DateTime time2 = new DateTime(dt.Year, dt.Month, vencimentoNumber);

            string convertDate = time2.ToString();

            IWebElement dateInput = driver.FindElement(By.Id("dataCobranca"));
            Console.WriteLine(convertDate);

            dateInput.SendKeys(convertDate);


            IJavaScriptExecutor jsScript1 = (IJavaScriptExecutor)driver;
            jsScript1.ExecuteScript(@"
    var select = document.getElementById('grupoImpostosCodigo');
    select.value = '6';
    select.dispatchEvent(new Event('change'));
");
            IJavaScriptExecutor jsScript2 = (IJavaScriptExecutor)driver;
            jsScript2.ExecuteScript(@"
    var select = document.getElementById('codTipoNotaFiscal');
    select.value = '3';
    select.dispatchEvent(new Event('change'));
");






            IJavaScriptExecutor jsScript4 = (IJavaScriptExecutor)driver;
            jsScript4.ExecuteScript(@"
    var select = document.getElementsByName('enderecoNotaFiscalCodigo')[0];
    select.click();
    select.dispatchEvent(new Event('change'));
");
        }


        IWebElement inputDescription = driver.FindElement(By.Id("descPlano_1"));


        SelectElement select = new SelectElement(inputDescription);


        select.SelectByValue("4781");
        Thread.Sleep(10000);

        IWebElement sendTextItem = driver.FindElement(By.Id("descItem_1"));

        string formatValor = valor + ",00";



        sendTextItem.Clear();
        sendTextItem.SendKeys(referente);

        IWebElement sendTextPrice = driver.FindElement(By.Id("valorItem_1"));
        sendTextPrice.Clear();
        sendTextPrice.SendKeys(formatValor);
        Actions action1 = new Actions(driver);

        IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;



        jsExecutor.ExecuteScript("enviarForm();");

        Console.WriteLine("Gerado com sucesso");

        driver.Quit();


    }




}
