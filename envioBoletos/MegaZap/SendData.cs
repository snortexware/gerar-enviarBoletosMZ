using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V129.Network;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Selenium.DefaultWaitHelpers;
using SeleniumExtras.WaitHelpers;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.IO;
using iText.Kernel.Pdf.Canvas.Parser;


namespace envioBoletos.MegaZap
{


    internal class SendData : Service
    {
        public IWebDriver _driver;
        public string confirm;

        public async Task StartProcessing(string cpf, string vencimento, string nome, string number, string codSuporte, string valor)
        {
            _driver = new ChromeDriver();
            Service service = new Service().StartLoadingAnimation();

            await Task.Run(() => ReadPdf(nome));
            service.StartLoadingAnimation();
            await Task.Run(() => MzLogin());
            await Task.Run(() => AddNumber(cpf, vencimento, nome, number, codSuporte, valor));

        }


        public void MzLogin()
        {

            _driver.Navigate().GoToUrl("https://www.megazap.chat/login.html");
            _driver.FindElement(By.Id("email")).SendKeys("lucas.gramnet");
            _driver.FindElement(By.Id("password")).SendKeys("Lucas.moreira2");

            IWebElement enterButton = _driver.FindElement(By.CssSelector(".btn.btn-lg.btn-block.m-t-20.bgm-black.waves-effect.ng-binding"));
            enterButton.Click();

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(8));



            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//button[@ng-click='close()']")));

            _driver.FindElement(By.XPath(".//button[@ng-click='close()']")).Click();


        }

        public void ReadPdf(string nome)
        {



            var pdfPath = @$"C:\Users\Suporte Lucas\Documents\Boletos1\{nome}";

            string[] pdfFiles = Directory.GetFiles(pdfPath, "*.pdf");

            if (pdfFiles.Length == 0)
            {
                Console.Write("no pdf found ");

                return;
            }


            foreach (string pdfFile in pdfFiles)
            {

                PdfReader pdfReader = new PdfReader(pdfFile);
                PdfDocument pdfDocument = new PdfDocument(pdfReader);

                for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
                {

                    string texto = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(i));

                    Console.WriteLine(texto);

                }







            }



            Console.Write("Este boleto é o boleto correto? Responda com yes e no: ");
            confirm = Console.ReadLine();

            if (confirm != "yes")
            {
                Console.Write("O boleto não vai ser enviado, abra o cadastro \n" +
                    "e envie manualmente após a mensagem que vai ser enviada");


            }




        }



        public void AddNumber(string cpf, string vencimento, string nome, string number, string codSuporte, string valor)
        {
            Service service = new Service();

            Thread.Sleep(2000);
            _driver.FindElement(By.XPath(".//div[@ng-click='onMostrarModalCriarAtendimentoNovoContato()']")).Click();


            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(4));

            wait.Until(ExpectedConditions.ElementExists(By.XPath("//select[@ng-change='selectChannelNewTicket()']")));
            IWebElement selectChanel = _driver.FindElement(By.XPath("//select[@ng-change='selectChannelNewTicket()']"));


            SelectElement valueInput1 = new SelectElement(selectChanel);


            valueInput1.SelectByValue("0");

            IWebElement teste1 = _driver.FindElement(By.Id("novoAtendimentoNovoContatoTelefone"));
            teste1.Click();
            teste1.SendKeys(number);


            try
            {
                var element = _driver.FindElement(By.Name("nome"));
                if (element.Displayed && element.Enabled) // Check if the element is interactable
                {
                    element.SendKeys(nome);
                }
                else
                {
                    Console.WriteLine("Element exists but is not interactable. Continuing execution...");
                }
            }
            catch (ElementNotInteractableException ex)
            {
                Console.WriteLine($"Element is not interactable: {ex.Message}. Continuing execution...");
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine($"Element not found: {ex.Message}. Continuing execution...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }


            IWebElement selectDepartment = _driver.FindElement(By.XPath(".//select[@ng-model='departamentoSelecionado.atendenteId']"));
            Thread.Sleep(2000);

            SelectElement valueInput2 = new SelectElement(selectDepartment);
            Thread.Sleep(2000);

            valueInput2.SelectByValue("565207");

            Thread.Sleep(2000);

            _driver.FindElement(By.XPath(".//button[@ng-click='onModalCriarAtendimentoNovoContato()']")).Click();

            Thread.Sleep(2000);


            IWebElement textarea = _driver.FindElement(By.Id("novaMensagem"));
            Thread.Sleep(5000);


            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)_driver;
            string message = $"Gr@mnet Telecom Caro Cliente,\n" +
                             $"segue abaixo boleto referente a visita/suporte no valor de {valor} efetuado para o cadastro dos dados abaixo:\n" +
                             $"Nome: {nome} \n" +
                             $"Final CPF/CNPJ: {cpf} \n" +
                             $"Protocolo de Atendimento Nº:{codSuporte} \n" +
                             $"Em caso de dúvidas, favor iniciar atendimento com nosso suporte! Digitando a opção 1.";

            jsExecutor.ExecuteScript("arguments[0].value = arguments[1];", textarea, message);
            Actions actions = new Actions(_driver);
            actions.SendKeys(Keys.Space).Perform();
            Thread.Sleep(2000);

            _driver.FindElement(By.XPath(".//span[@ng-click='enviarMensagem()']")).Click();

            Thread.Sleep(2000);

            _driver.FindElement(By.CssSelector(".pull-left.dropdown-fab-container.ng-isolate-scope")).Click();

            Thread.Sleep(2000);
            var pdfPath = @$"C:\Users\Suporte Lucas\Documents\Boletos1\{nome}";

            string[] pdfFiles = Directory.GetFiles(pdfPath, "*.pdf");

            var inputvalue = _driver.FindElement(By.Id("fileInput"));

            string selectedBoleto = pdfFiles[0];

            Thread.Sleep(2000);

            inputvalue.SendKeys(selectedBoleto);

            Thread.Sleep(2000);

            var botaoFinal = _driver.FindElement(By.XPath(".//button[@ng-click='onSendFiles()']"));


            Thread.Sleep(2000);

            botaoFinal.Click();







        }


    }
}
