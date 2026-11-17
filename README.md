# console-selenium-42-net

Automação de cenários reais no [DuckDuckGo](https://duckduckgo.com) com **Selenium WebDriver + MSTest** em .NET 10 — atividade da disciplina Gestão e Qualidade de Software.

## Estrutura da solution

```
ConsoleSelenium42.slnx
├── ConsoleSelenium/          # Console app (.NET 10) — ponto de entrada/instruções
└── ConsoleSelenium.Tests/    # Projeto MSTest com os cenários Selenium
    └── BuscaDesafioTest.cs
```

## Pré-requisitos

- .NET SDK 10+
- Google Chrome instalado (o pacote `Selenium.WebDriver.ChromeDriver` fornece o driver)

## Executando

```bash
dotnet restore
dotnet build
dotnet test                 # abre o Chrome visível
DDG_HEADLESS=1 dotnet test  # (opcional) roda em modo headless
```

## Cenários automatizados

| Exercício | Cenário |
|---|---|
| 1 | Pesquisa básica: digita "Selenium WebDriver C#", submete com Enter e valida o título da página de resultados |
| 2 | Autocompletar: digita "C# unit testing", aguarda as sugestões com `WebDriverWait`, clica na **terceira** e valida a navegação |
| 3 | Histórico: busca "Automated Testing", abre o primeiro link orgânico, valida a página externa e usa `Navigation.Back()` para restaurar os resultados |

Boas práticas aplicadas: esperas explícitas (`WebDriverWait`), seletores com fallback, setup/teardown por teste (`TestInitialize`/`TestCleanup`) e commits semânticos.
