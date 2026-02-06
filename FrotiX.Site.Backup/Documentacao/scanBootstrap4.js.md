# scanBootstrap4.js — Scanner de Bootstrap 4

> **Arquivo:** `scanBootstrap4.js`  
> **Papel:** localizar referências de Bootstrap 4 no projeto e gerar relatório HTML.

---

## ✅ Visão Geral

Este script percorre arquivos estáticos procurando padrões do Bootstrap 4. O resultado é consolidado em um relatório `bootstrap4-report.html`.

---

## 🔧 Responsabilidades Principais

1. **Escanear arquivos** com `fast-glob`.
2. **Aplicar regexes** de detecção (classes, links e versões).
3. **Gerar relatório HTML** resumindo os achados.

---

## 🧩 Snippets Comentados

```javascript
const entries = await fg(["**/*.{html,htm,js,ts,css,scss}"]);
```

```javascript
bootstrap4Regexes.forEach(({ pattern, label }) => {
    if (pattern.test(content)) {
        fileMatches.push(label);
    }
});
```

---

## ✅ Observações Técnicas

- Script utilitário para **auditoria de legado**.
- Não faz alterações no código; apenas relatório.
- Ignora `node_modules`, `.git`, `dist` e `build`.


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
