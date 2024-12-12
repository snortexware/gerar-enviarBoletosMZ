using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Selenium.DefaultWaitHelpers;
using SeleniumExtras.WaitHelpers;

namespace envioBoletos.MegaZap
{
    internal class SendData
    {

        string mz_codCliente;
        string mz_valor;
        string mz_referente;
        string mz_cpf;
        string mz_vencimento;
        string mz_nome;
        string mz_number;
        IWebDriver mz_driver;

        public SendData(Service service)
        {
            mz_codCliente = service.CodCliente;
            mz_valor = service.Valor;
            mz_referente = service.Referente;
            mz_cpf = service.Cpf;
            mz_vencimento = service.Vencimento;
            mz_nome = service.Nome;
            mz_number = service.Number;

            Console.WriteLine(mz_nome);


        }

        public async Task StartProcessing()
        {
            Service service = new Service().StartLoadingAnimation();

            service.StartLoadingAnimation();
            await Task.Run(() => MzLogin());
            await Task.Run(() => AddNumber());

        }

        public void MzLogin()
        {
            mz_driver = new ChromeDriver();

            mz_driver.Navigate().GoToUrl("https://www.megazap.chat/login.html");
            mz_driver.FindElement(By.Id("email")).SendKeys("lucas.gramnet");
            mz_driver.FindElement(By.Id("password")).SendKeys("Lucas.moreira2");

            IWebElement enterButton = mz_driver.FindElement(By.CssSelector(".btn.btn-lg.btn-block.m-t-20.bgm-black.waves-effect.ng-binding"));
            enterButton.Click();

            WebDriverWait wait = new WebDriverWait(mz_driver, TimeSpan.FromSeconds(8));



            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//button[@ng-click='close()']")));

            mz_driver.FindElement(By.XPath(".//button[@ng-click='close()']")).Click();


        }


        public void AddNumber()
        {
            Thread.Sleep(2000);
            mz_driver.FindElement(By.XPath(".//div[@ng-click='onMostrarModalCriarAtendimentoNovoContato()']")).Click();


            WebDriverWait wait = new WebDriverWait(mz_driver, TimeSpan.FromSeconds(4));

            wait.Until(ExpectedConditions.ElementExists(By.XPath("//select[@ng-change='selectChannelNewTicket()']")));
            IWebElement selectChanel = mz_driver.FindElement(By.XPath("//select[@ng-change='selectChannelNewTicket()']"));


            SelectElement valueInput1 = new SelectElement(selectChanel);


            valueInput1.SelectByValue("0");

            IWebElement teste1 = mz_driver.FindElement(By.Id("novoAtendimentoNovoContatoTelefone"));
            teste1.Click();
            teste1.SendKeys(mz_number);

            IWebElement selectDepartment = mz_driver.FindElement(By.XPath(".//select[@ng-model='departamentoSelecionado.atendenteId']"));
            Thread.Sleep(2000);

            SelectElement valueInput2 = new SelectElement(selectDepartment);
            Thread.Sleep(2000);

            valueInput2.SelectByValue("565207");

            Thread.Sleep(2000);

            mz_driver.FindElement(By.XPath(".//button[@ng-click='onModalCriarAtendimentoNovoContato()']")).Click();

            Thread.Sleep(2000);

            IWebElement textarea = mz_driver.FindElement(By.Id("novaMensagem"));
            Thread.Sleep(5000);


            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)mz_driver;
            string message = $"Gr@mnet Telecom Caro Cliente,\n" +
                             $"segue abaixo boleto referente a visita/suporte efetuado para o cadastro dos dados abaixo:\n" +
                             $"Nome:{mz_nome} \n" +
                             $"Final CPF/CNPJ: {mz_cpf} \n" +
                             $"Protocolo de Atendimento Nº:{mz_codCliente} \n" +
                             $"Em caso de dúvidas, favor iniciar atendimento com nosso suporte! Digitando a opção 1.";

            jsExecutor.ExecuteScript("arguments[0].value = arguments[1];", textarea, message);
            Actions actions = new Actions(mz_driver);
            actions.SendKeys(Keys.Space).Perform();
            Thread.Sleep(2000);

            mz_driver.FindElement(By.XPath(".//span[@ng-click='enviarMensagem()']")).Click();






        }


    }
}
