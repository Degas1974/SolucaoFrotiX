# OcorrenciaViagemController.Listar.cs — Listagens e exclusão

> **Arquivo:** `Controllers/OcorrenciaViagemController.Listar.cs`  
> **Papel:** Endpoints auxiliares para listagens em modal, verificação por veículo e exclusão.

---

## ✅ Visão Geral

Fornece consultas rápidas de ocorrências para modal de viagem, listagem por veículo e verificação de pendências, além de exclusão direta por ID.

---

## 🔧 Endpoints Principais

- `GET ListarOcorrenciasModal` — lista ocorrências de uma viagem para o modal.
- `GET ListarOcorrenciasVeiculo` — lista ocorrências abertas/pendentes por veículo.
- `GET VerificarOcorrenciasVeiculo` — retorna quantidade de ocorrências abertas.
- `POST ExcluirOcorrencia` — remove ocorrência específica.

---

## 🧩 Snippet Comentado

```csharp
var ocorrencias = _unitOfWork.OcorrenciaViagem
    .GetAll(o => o.VeiculoId == veiculoId
              && o.StatusOcorrencia == true
              && (o.Status == "Aberta" || o.Status == "Pendente"))
    .OrderByDescending(o => o.DataCriacao)
    .ToList();
```

---

## ✅ Observações Técnicas

- `ListarOcorrenciasVeiculo` filtra apenas abertas/pendentes com `StatusOcorrencia == true`.
- `VerificarOcorrenciasVeiculo` retorna `temOcorrencias` com base no contador.
- `ExcluirOcorrencia` usa DTO simples com `OcorrenciaViagemId`.

---

## 📎 DTOs

- `ExcluirOcorrenciaDTO` — contém `OcorrenciaViagemId`.


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
