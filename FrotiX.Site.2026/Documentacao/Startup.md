# Documentacao: Startup.cs

> **Ultima Atualizacao**: 04/02/2026
> **Versao Atual**: 1.0

---

## Visao Geral

Arquivo de configuracao principal da aplicacao ASP.NET Core. Define servicos de DI, middlewares e configuracoes globais do FrotiX.Site.

## Responsabilidades

- Configurar servicos e dependencias (DbContext, repositorios, services).
- Configurar middlewares e pipeline HTTP.
- Registrar logging customizado do FrotiX.

## Ponto Critico: Logging Customizado

O provider FrotiXLoggerProvider e registrado via DI sem uso de BuildServiceProvider para evitar o warning ASP0000.

### Trecho Atual

```csharp
services.AddSingleton<ILoggerProvider>(sp =>
    new FrotiXLoggerProvider(
        sp.GetRequiredService<ILogService>(),
        LogLevel.Warning
    )
);
services.AddLogging();
```

## Observacoes

- O LogService e registrado antes do provider de logging.
- O pipeline de logs captura Warning+ e integra com o banco.

---

## Log de Modificacoes

| Versao | Data       | Autor          | Descricao |
|--------|------------|----------------|-----------|
| 1.0    | 04/02/2026 | GitHub Copilot | Remove BuildServiceProvider e registra FrotiXLoggerProvider via DI. |
