// ConsoleSelenium42 — automação Selenium no DuckDuckGo.
// Os cenários vivem no projeto ConsoleSelenium.Tests (MSTest); este app
// apenas apresenta a solução e como executá-los.

Console.WriteLine("ConsoleSelenium42 — Automação DuckDuckGo com Selenium + MSTest");
Console.WriteLine();
Console.WriteLine("Cenários (ConsoleSelenium.Tests/BuscaDesafioTest.cs):");
Console.WriteLine("  1. Pesquisa básica e validação de título");
Console.WriteLine("  2. Autocompletar: clique na terceira sugestão");
Console.WriteLine("  3. Navegação por histórico (voltar aos resultados)");
Console.WriteLine();
Console.WriteLine("Execute os testes com: dotnet test");
Console.WriteLine("Headless (sem abrir janela): DDG_HEADLESS=1 dotnet test");
