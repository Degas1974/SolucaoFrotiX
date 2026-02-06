# SkipModelValidationAttribute.cs

## Visão Geral

Atributo (`IActionFilter`) projetado para pular a verificação de `ModelState.IsValid` (ou ignorar erros específicos) em cenários onde validações parciais são aceitáveis.

## Localização

`Filters/SkipModelValidationAttribute.cs`

## Dependências

- `Microsoft.AspNetCore.Mvc`
- `Microsoft.AspNetCore.Mvc.Filters`

## Funcionalidades Principais

- `OnActionExecuting`: Limpa o `ModelState` (`context.ModelState.Clear()`) antes da action ser executada, efetivamente anulando qualquer erro de binding/validação prévio.

## Uso Típico

- Endpoints que recebem DTOs parciais onde campos obrigatórios podem estar ausentes intencionalmente.
- Cenários de rascunho (salvar sem validar).

---

## PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

---

### 2026-01-19 - Padronização e Validação

- **Alteração**: Adição de cabeçalho ASCII padrão e validação de conformidade (Standardization).
- **Responsável**: Agente Gemini/GitHub Copilot
