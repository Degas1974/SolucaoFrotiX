# Documentação: Shared - \_CookieConsentPartial

> **Última Atualização**: 23/01/2026
> **Versão Atual**: 1.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

Partial responsável por exibir o banner de consentimento de cookies nas páginas de login e registro, utilizando `ITrackingConsentFeature`.

## Estrutura

- Bloco de consentimento exibido apenas quando a página é `login` ou `register`.
- Botão de aceite grava o cookie de consentimento.

## Dependências

- `ITrackingConsentFeature` (ASP.NET Core).
- Script JavaScript com `try-catch` e `Alerta.TratamentoErroComLinha`.

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [23/01/2026 10:40] - Card de identidade e tratamento de erros

**Descrição**:
Inclusão de card ASCII no topo do partial e `try-catch` no script de consentimento.

**Arquivos Afetados**:

- `Pages/Shared/_CookieConsentPartial.cshtml`

**Impacto**: Conformidade com padrão de documentação e tratamento de erros.

**Status**: ✅ Concluído

**Versão**: 1.0
