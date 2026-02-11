# Copilot Instructions - FrotiX.Mobile

## INSTRUCAO OBRIGATORIA

Leia COMPLETAMENTE o arquivo `RegrasDesenvolvimentoFrotiXMobile.md` antes de qualquer sugestao de codigo.

Este arquivo contem TODAS as regras de desenvolvimento dos projetos mobile FrotiX.

## Contexto do Projeto

- **Plataforma:** .NET MAUI Blazor Hybrid (net10.0-android)
- **Projetos:** FrotiX.Mobile.Shared, FrotiX.Mobile.Economildo, FrotiX.Mobile.Vistorias
- **Comunicacao:** Azure Relay via RelayApiService
- **UI:** Syncfusion Blazor + MudBlazor (Economildo) + Radzen (Vistorias)

## Regras Criticas

1. **Try-catch** obrigatorio em TODAS as funcoes (C# e JS)
2. **AlertaJs** via JS Interop para alertas (NUNCA alert() nativo)
3. **fa-duotone** para icones (NUNCA fa-solid)
4. **Services** no Shared = equivalente a Controllers do web
5. **GC.Collect()** antes de navegacao entre paginas
6. **Syncfusion Blazor** para componentes UI (SfComboBox, SfDatePicker, etc.)
7. Commits: feat:, fix:, refactor:, docs:, style:, chore:
