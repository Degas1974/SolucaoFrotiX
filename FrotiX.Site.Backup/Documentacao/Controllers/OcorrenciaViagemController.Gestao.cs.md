# OcorrenciaViagemController.Gestao.cs — Gestão de ocorrências

> **Arquivo:** `Controllers/OcorrenciaViagemController.Gestao.cs`  
> **Papel:** Endpoints de listagem e gestão (edição/baixa) de ocorrências de viagem.

---

## ✅ Visão Geral

Camada de API usada na tela de gestão de ocorrências, com filtros por veículo, motorista, status e período. Também permite editar dados, finalizar ocorrências e baixar com solução.

---

## 🔧 Endpoints Principais

- `GET ListarGestao` — lista ocorrências com filtros e status normalizado.
- `POST EditarOcorrencia` — atualiza resumo/descrição/solução e imagem.
- `POST BaixarOcorrenciaGestao` — baixa ocorrência sem solução.
- `POST BaixarOcorrenciaComSolucao` — baixa ocorrência com solução normalizada.
- `GET ContarOcorrencias` — contagem total, abertas e baixadas.

---

## 🧩 Snippet Comentado

```csharp
if (!string.IsNullOrWhiteSpace(statusId) && statusId != "Todas")
{
    if (statusId == "Aberta")
    {
        ocorrenciasQuery = ocorrenciasQuery.Where(x =>
            ((x.StatusOcorrencia == null || x.StatusOcorrencia == true || x.Status == "Aberta")
            && x.Status != "Pendente"
            && x.ItemManutencaoId == null));
    }
    else if (statusId == "Baixada")
    {
        ocorrenciasQuery = ocorrenciasQuery.Where(x =>
            x.StatusOcorrencia == false || x.Status == "Baixada");
    }
}
```

---

## ✅ Observações Técnicas

- Datas aceitam formatos BR e ISO, com fallback para `InvariantCulture`.
- Lista limita a 500 registros e enriquece dados via `Viagem`, `ViewVeiculos`, `ViewMotoristas`.
- Status usa regra: `NULL/true = Aberta`, `false = Baixada`, com prioridade para `Pendente`/`Manutenção`.
- Textos passam por normalização via `TextNormalizationHelper`.

---

## 📎 DTOs

- `EditarOcorrenciaDTO` — resumo/descrição/solução, status e imagem.
- `BaixarOcorrenciaDTO` — id da ocorrência.
- `BaixarComSolucaoDTO` — id e solução da ocorrência.


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
