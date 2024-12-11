using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace envioBoletos.MegaZap
{
    internal class SendData
    {
        private int currentID;

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
            mz_driver = service.Driver;
            mz_codCliente = service.CodCliente;
            mz_valor = service.Valor;
            mz_referente = service.Referente;
            mz_cpf = service.Cpf;
            mz_vencimento = service.Vencimento;
            mz_nome = service.Nome;
            mz_number = service.Number;
        }

        public async Task StartProcessing()
        {
            Service service = new Service().StartLoadingAnimation();

            service.StartLoadingAnimation();
            await Task.Run(() => MzLogin());
            await Task.Run(() => AddNumber());

        }

        IWebDriver tempo;
        public void MzLogin()
        {

            tempo = new ChromeDriver();
            tempo.Navigate().GoToUrl("https://www.megazap.chat/login.html");
            tempo.FindElement(By.Id("email")).SendKeys("lucas.gramnet");
            tempo.FindElement(By.Id("password")).SendKeys("Lucas.moreira2");

            IWebElement enterButton = tempo.FindElement(By.CssSelector(".btn.btn-lg.btn-block.m-t-20.bgm-black.waves-effect.ng-binding"));
            enterButton.Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5);


            wait.Until(tempo => tempo.FindElement(By.CssSelector("[@ng-click='close()']")).Displayed);


            tempo.FindElement(By.CssSelector("[@ng-click='close()']")).Click();


        }


        public void AddNumber()
        {


            WebDriverWait wait = new WebDriverWait(tempo, TimeSpan.FromSeconds(8));

            wait.Until(tempo => tempo.FindElement(By.CssSelector("[//select[@ng-model='novoAtendimentoNovoContato.canalChave']")).Displayed);

            IWebElement selectChanel = driver.FindElement(By.XPath("//select[@ng-model='novoAtendimentoNovoContato.canalChave']"));

            SelectElement valueInput1 = new SelectElement(selectChanel);

            valueInput1.SelectByValue("0");

            string testeN = "47997306396";
            IWebElement teste1 = tempo.FindElement(By.Id("novoAtendimentoNovoContatoTelefone"));
            teste1.Click();
            teste1.SendKeys(testeN);

            IWebElement selectDepartment = tempo.FindElement(By.XPath("//select[@ng-model='departamentoSelecionado.atendenteId']"));

            SelectElement valueInput2 = new SelectElement(selectDepartment);

            valueInput2.SelectByValue("565207");


            tempo.FindElement(By.XPath(".//button[@ng-click='onModalCriarAtendimentoNovoContato()']")).Click();




        }


    }
}
