# Program.cs — Inicialização do Host

> **Arquivo:** `Program.cs`  
> **Papel:** ponto de entrada do ASP.NET Core, criação do host e configuração de handlers globais.

---

## ✅ Visão Geral

O `Program.cs` é o coração do bootstrap da aplicação. Ele:

- Cria o host com `CreateHostBuilder`.
- Registra handlers globais para exceções não tratadas.
- Garante log de emergência em arquivo caso o DI falhe.

---

## 🔧 Responsabilidades Principais

1. **Inicializar o Host** usando `CreateHostBuilder`.
2. **Configurar handlers globais** (`AppDomain` e `TaskScheduler`).
3. **Registrar log de emergência** em arquivo se o serviço de log não existir.
4. **Delegar o pipeline** para `Startup`.

---

## 🧠 Fluxo de Inicialização (resumo)

1. `Main` chama `CreateHostBuilder`.
2. O host é **buildado**.
3. `ConfigureGlobalExceptionHandlers` liga os listeners globais.
4. `host.Run()` inicia o servidor.

---

## 🧩 Trechos-Chave (comentados)

```csharp
// 1) Build do Host e handlers globais
var host = CreateHostBuilder(args).Build();
ConfigureGlobalExceptionHandlers(host.Services);

// 2) Início da aplicação
host.Run();
```

```csharp
// Handler global de exceções de AppDomain e Tasks não observadas
AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
{
    logService.Error("ERRO NÃO TRATADO (AppDomain)",
        e.ExceptionObject as Exception,
        "AppDomain",
        "UnhandledException");
};
```

```csharp
// Log de emergência em arquivo (fallback sem DI)
var logPath = Path.Combine(logDir, $"frotix_log_{DateTime.Now:yyyy-MM-dd}.txt");
File.AppendAllText(logPath, logMessage);
```

---

## 🛡️ Tratamento de Erros

Todo o `Main` é envolvido em **try/catch** e usa o padrão FrotiX:

- `Alerta.TratamentoErroComLinha` para rastreio centralizado.
- Log de emergência para cenários críticos.

---

## 🔗 Dependências Importantes

- `ILogService` (DI) — centraliza logs.
- `Alerta.TratamentoErroComLinha` — padrão FrotiX.
- `Startup` — define todo o pipeline de serviços e middlewares.

---

## ✅ Observações Técnicas

- O método `EnableTracing` está presente como utilitário opcional.
- Mesmo em erro, o código garante `CreateHostBuilder` retornar um host válido.


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
