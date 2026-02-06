# PageExceptionFilter.cs

## Visão Geral

Filtro específico para captura de exceções em Razor Pages (`OnGet`, `OnPost` etc.), atuando onde o `GlobalExceptionFilter` (que foca em Controllers) não alcança.

## Localização

`Filters/PageExceptionFilter.cs`

## Dependências

- `FrotiX.Services.ILogService`
- `Microsoft.Extensions.Logging.ILogger`

## Funcionalidades Principais

- `OnPageHandlerExecuted`: Verifica se houve exceção não tratada na execução do handler.
- `OnPageHandlerExecutionAsync`: Wrapper async para capturar exceções em tasks.
- `LogPageException`: Centraliza a lógica de extração de metadados e registro no `ILogService`.

## Diferenças do GlobalFilter

Não altera o `Result` da resposta (não força JSON), apenas registra o erro. O redirecionamento para página de erro é gerenciado pelo middleware de ExceptionHandler padrão do Razor Pages.

---

## PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

---

### 2026-01-19 - Padronização e Validação

- **Alteração**: Adição de cabeçalho ASCII padrão e validação de conformidade (Standardization).
- **Responsável**: Agente Gemini/GitHub Copilot
