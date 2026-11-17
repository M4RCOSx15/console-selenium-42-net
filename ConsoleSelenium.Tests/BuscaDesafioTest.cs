using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace ConsoleSelenium.Tests;

/// <summary>
/// Cenários de automação no DuckDuckGo (https://duckduckgo.com).
/// Cada teste abre um Chrome real via Selenium.WebDriver.ChromeDriver.
/// </summary>
[TestClass]
public sealed class BuscaDesafioTest
{
    private const string BaseUrl = "https://duckduckgo.com";
    private const string ResultadosSelector = "a[data-testid='result-title-a'], .result__a, h2 a";
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(15);

    private IWebDriver _driver = null!;
    private WebDriverWait _wait = null!;

    [TestInitialize]
    public void Setup()
    {
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");
        options.AddArgument("--disable-search-engine-choice-screen");
        // Headless sob demanda: DDG_HEADLESS=1 dotnet test
        if (Environment.GetEnvironmentVariable("DDG_HEADLESS") == "1")
            options.AddArgument("--headless=new");
        _driver = new ChromeDriver(options);
        _wait = new WebDriverWait(_driver, Timeout);
    }

    [TestCleanup]
    public void Teardown()
    {
        _driver?.Quit();
        _driver?.Dispose();
    }

    /// <summary>Campo de pesquisa visível da página inicial (name="q").</summary>
    private IWebElement CampoPesquisa() =>
        _wait.Until(d => d.FindElement(By.Name("q")).Displayed ? d.FindElement(By.Name("q")) : null!);

    /// <summary>Aguarda a página de resultados carregar (links orgânicos no DOM).</summary>
    private void AguardarResultados() =>
        _wait.Until(d => d.FindElements(By.CssSelector(ResultadosSelector)).Any(e => e.Displayed));

    // Exercício 1 — Experiência de Pesquisa Básica e Validação de Título
    [TestMethod]
    public void PesquisaBasica_DeveExibirResultadosComTermoNoTitulo()
    {
        _driver.Navigate().GoToUrl(BaseUrl);

        var campo = CampoPesquisa();
        campo.SendKeys("Selenium WebDriver C#");
        campo.SendKeys(Keys.Enter);

        _wait.Until(d =>
            d.Title.Contains("Selenium WebDriver C#", StringComparison.OrdinalIgnoreCase)
            || d.Url.Contains("q=", StringComparison.OrdinalIgnoreCase));

        Assert.IsTrue(
            _driver.Title.Contains("Selenium WebDriver", StringComparison.OrdinalIgnoreCase),
            $"Título inesperado da página de resultados: {_driver.Title}");
    }

    // Exercício 2 — Interagindo com o Autocompletar (Suggestions)
    [TestMethod]
    public void Autocompletar_DeveNavegarParaResultadosDaTerceiraSugestao()
    {
        _driver.Navigate().GoToUrl(BaseUrl);

        CampoPesquisa().SendKeys("C# unit testing");

        // Espera explícita pela lista dinâmica de sugestões do autocompletar.
        var sugestoes = _wait.Until(d =>
        {
            var itens = d.FindElements(By.CssSelector(
                "[role='option'], .sbox-suggestions li, .search-suggestions-list li, .searchbox_suggestion"));
            return itens.Count >= 3 ? itens : null;
        });

        sugestoes[2].Click();

        AguardarResultados();
        Assert.IsTrue(
            _driver.Url.Contains("duckduckgo.com", StringComparison.OrdinalIgnoreCase)
            && _driver.Url.Contains("q=", StringComparison.OrdinalIgnoreCase),
            $"Navegação inesperada após clicar na sugestão: {_driver.Url}");
    }

    // Exercício 3 — Navegação por Histórico e Botões de Ação
    [TestMethod]
    public void NavegacaoHistorico_DeveRestaurarResultadosAposVoltar()
    {
        _driver.Navigate().GoToUrl(BaseUrl);

        var campo = CampoPesquisa();
        campo.SendKeys("Automated Testing");
        campo.SendKeys(Keys.Enter);

        var primeiroLink = _wait.Until(d =>
            d.FindElements(By.CssSelector(ResultadosSelector)).FirstOrDefault(e => e.Displayed));
        Assert.IsNotNull(primeiroLink, "Nenhum link orgânico retornado na busca.");
        var urlResultados = _driver.Url;

        primeiroLink.Click();

        // O link pode abrir na mesma aba ou em uma nova — cobre os dois casos.
        _wait.Until(d => d.WindowHandles.Count > 1 || !d.Url.Contains("duckduckgo.com", StringComparison.OrdinalIgnoreCase));
        if (_driver.WindowHandles.Count > 1)
            _driver.SwitchTo().Window(_driver.WindowHandles[^1]);

        _wait.Until(d => d.Url.StartsWith("http", StringComparison.OrdinalIgnoreCase));
        Assert.IsFalse(
            _driver.Url.Contains("duckduckgo.com", StringComparison.OrdinalIgnoreCase),
            $"A página externa não foi carregada: {_driver.Url}");

        _driver.Navigate().Back();

        _wait.Until(d => d.Url.Contains("duckduckgo.com", StringComparison.OrdinalIgnoreCase));
        AguardarResultados();
        Assert.AreEqual(urlResultados, _driver.Url, "A página de resultados não foi restaurada corretamente.");
    }
}
