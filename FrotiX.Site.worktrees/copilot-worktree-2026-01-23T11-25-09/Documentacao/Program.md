# Program.cs — Inicialização do Host

**Arquivo:** `Program.cs`  
**Papel:** ponto de entrada do ASP.NET Core, criação do host e configuração de handlers globais.

---

## ✅ Visão Geral

O `Program.cs` é o bootstrap da aplicação FrotiX. Ele constrói o host, ativa listeners globais de exceção e garante log de emergência caso o DI ainda não esteja pronto.

---

## 🔧 Responsabilidades Principais

1. **Criar o Host** com `CreateHostBuilder`.
2. **Configurar handlers globais** (`AppDomain` e `TaskScheduler`).
3. **Fallback de log em arquivo** caso a injeção falhe.
4. **Delegar o pipeline** para `Startup`.

---

## 🧠 Fluxo de Inicialização (resumo)

1. `Main` cria o host.
2. `ConfigureGlobalExceptionHandlers` registra listeners globais.
3. `host.Run()` inicia o servidor web.

---

## 🧩 Snippets Comentados

```csharp
// Build do host + handlers globais de exceção
var host = CreateHostBuilder(args).Build();
ConfigureGlobalExceptionHandlers(host.Services);

// Inicia o pipeline web
host.Run();
```

```csharp
// Handler de exceções não tratadas do AppDomain
AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
{
    logService.Error(
        "ERRO NÃO TRATADO (AppDomain)",
        e.ExceptionObject as Exception,
        "AppDomain",
        "UnhandledException"
    );
};
```

```csharp
// Fallback: log em arquivo quando o DI não está disponível
var logPath = Path.Combine(logDir, $"frotix_log_{DateTime.Now:yyyy-MM-dd}.txt");
File.AppendAllText(logPath, logMessage);
```

---

## 🛡️ Tratamento de Erros

Todo o `Main` está envolvido em `try/catch` e usa o padrão FrotiX (`Alerta.TratamentoErroComLinha`). Isso garante rastreabilidade mesmo em falhas críticas de startup.

---

## 🔗 Dependências Importantes

- `ILogService` — centraliza logging via DI.
- `Alerta.TratamentoErroComLinha` — padrão FrotiX de rastreio.
- `Startup` — define serviços e middlewares.

---

## ✅ Observações Técnicas

- `EnableTracing` existe como utilitário de diagnóstico opcional.
- O fallback de `CreateHostBuilder` assegura que a aplicação não quebre por erro de configuração.

---

## 🎨 Nota Visual

Esta documentação segue a paleta FrotiX (caramelo suave) em títulos e ícones, mantendo legibilidade no Markdown.


---

# PARTE 2: LOG DE MODIFICAÃ‡Ã•ES/CORREÃ‡Ã•ES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [19/01/2026] - AtualizaÃ§Ã£o: ImplementaÃ§Ã£o de MÃ©todos com Tracking Seletivo

**DescriÃ§Ã£o**: MigraÃ§Ã£o de chamadas .AsTracking() para novos mÃ©todos GetWithTracking() e GetFirstOrDefaultWithTracking() como parte da otimizaÃ§Ã£o de performance do Entity Framework Core.

**Arquivos Afetados**:
- Este arquivo (uso dos novos mÃ©todos do repositÃ³rio)
- Repository/IRepository/IRepository.cs (definiÃ§Ã£o dos novos mÃ©todos)
- Repository/Repository.cs (implementaÃ§Ã£o)
- RegrasDesenvolvimentoFrotiX.md (seÃ§Ã£o 4.2 - nova regra permanente)

**MudanÃ§as**:
- âŒ **ANTES**: _unitOfWork.Entity.AsTracking().Get(id) ou _unitOfWork.Entity.AsTracking().GetFirstOrDefault(filter)
- âœ… **AGORA**: _unitOfWork.Entity.GetWithTracking(id) ou _unitOfWork.Entity.GetFirstOrDefaultWithTracking(filter)

**Motivo**: 
- OtimizaÃ§Ã£o de memÃ³ria e performance
- Tracking seletivo (apenas quando necessÃ¡rio para Update/Delete)
- PadrÃ£o mais limpo e explÃ­cito
- Conformidade com nova regra permanente (RegrasDesenvolvimentoFrotiX.md seÃ§Ã£o 4.2)

**Impacto**: 
- Melhoria de performance em operaÃ§Ãµes de leitura (usa AsNoTracking por padrÃ£o)
- Tracking correto em operaÃ§Ãµes de escrita (Update/Delete)
- Zero impacto funcional (comportamento mantido)

**Status**: âœ… **ConcluÃ­do**

**ResponsÃ¡vel**: Sistema (AtualizaÃ§Ã£o AutomÃ¡tica)

**VersÃ£o**: Incremento de patch
