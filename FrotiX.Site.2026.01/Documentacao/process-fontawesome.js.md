# process-fontawesome.js — Normalização de Ícones

> **Arquivo:** `process-fontawesome.js`  
> **Papel:** traduz categorias e termos de ícones FontAwesome para português.

---

## ✅ Visão Geral

Este script Node.js processa listas de ícones FontAwesome e aplica:

- Tradução de categorias (ex.: `Accessibility → Acessibilidade`).
- Normalização de termos comuns (ex.: `calendar → calendário`).
- Padronização terminológica para relatórios internos.

---

## 🔧 Responsabilidades Principais

1. **Mapear categorias** usando `categoryTranslations`.
2. **Normalizar nomes** usando `translations`.
3. **Gerar saída consistente** para catálogos FrotiX.

---

## 🧩 Snippets Comentados

```javascript
const categoryTranslations = {
  'Accessibility': 'Acessibilidade',
  'Alert': 'Alertas',
  'Automotive': 'Automotivo'
};
```

```javascript
const translations = {
  'calendar': 'calendário',
  'vehicle': 'veículo',
  'alert': 'alerta'
};
```

---

## ✅ Observações Técnicas

- Script de apoio (não usado no runtime web).
- Ideal para gerar **catálogos internos** e documentação de ícones.
- Usa `fs` e `yaml` para leitura e normalização.


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
