# GlobalUsings.TryCatchHelper.g.cs — Usings Globais

> **Arquivo:** `GlobalUsings.TryCatchHelper.g.cs`  
> **Papel:** registrar `global using` para helpers de tratamento de erro.

---

## ✅ Visão Geral

Este arquivo é **gerado automaticamente** pelo `TryCatchHelper` e centraliza o namespace `FrotiX.Helpers` como `global using`.

Isso evita repetição de `using` em arquivos C# e garante que o padrão de alertas esteja sempre acessível.

---

## 🧩 Snippet Comentado

```csharp
// Gerado automaticamente pelo TryCatchHelper
global using FrotiX.Helpers;
```

---

## 📌 Por que isso existe?

- Padroniza o acesso ao helper `Alerta` em toda a solução.
- Evita esquecer `using FrotiX.Helpers` em classes novas.
- Mantém consistência com a regra FrotiX de **try/catch obrigatório**.

---

## ✅ Observações Técnicas

- Arquivo **gerado**, não editar manualmente.
- Deve existir enquanto o helper de injeção de try/catch estiver ativo.


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
