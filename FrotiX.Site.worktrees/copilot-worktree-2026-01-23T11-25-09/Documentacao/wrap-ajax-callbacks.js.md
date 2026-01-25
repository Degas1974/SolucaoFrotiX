# wrap-ajax-callbacks.js — Codemod de Try/Catch

> **Arquivo:** `wrap-ajax-callbacks.js`  
> **Papel:** codemod para envolver callbacks de `$.ajax` em try/catch automático.

---

## ✅ Visão Geral

Este script usa `jscodeshift` para localizar chamadas `$.ajax` e reescrever callbacks `success` e `error` com `try/catch`, garantindo o padrão FrotiX de tratamento de erros.

---

## 🔧 Responsabilidades Principais

1. **Localizar chamadas `$.ajax`.**
2. **Extrair o endpoint** para nomear o log.
3. **Encapsular callbacks** em `try/catch`.

---

## 🧩 Snippets Comentados

```javascript
root.find(j.CallExpression, {
  callee: { object: { name: '$' }, property: { name: 'ajax' } }
}).forEach(path => {
  // ... reescrita automática
});
```

```javascript
fn.body = j.blockStatement([
  j.tryStatement(
    j.blockStatement(originalStmts),
    j.catchClause(
      j.identifier('error'),
      null,
      j.blockStatement([
        j.expressionStatement(
          j.callExpression(
            j.identifier('TratamentoErroComLinha'),
            [__scriptName, `ajax.${endpoint}.${cbName}`, error]
          )
        )
      ])
    )
  )
]);
```

---

## ✅ Observações Técnicas

- Codemod **não altera lógica de negócio**, apenas tratamento de erros.
- Ajuda a aplicar a regra FrotiX de **try/catch obrigatório** em JS.
- Requer `jscodeshift` para execução.


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
