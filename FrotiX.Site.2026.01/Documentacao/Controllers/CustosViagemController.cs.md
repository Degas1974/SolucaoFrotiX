# CustosViagemController.cs — Custos de viagens

> **Arquivo:** `Controllers/CustosViagemController.cs`  
> **Papel:** listar custos de viagens e recalcular custos.

---

## ✅ Visão Geral

Controller API que retorna dados de custos por viagem e permite recalcular custos em lote para viagens realizadas.

---

## 🔧 Endpoints Principais

- `Get`: lista custos reduzidos (`ViewCustosViagem`).
- `CalculaCustoViagens`: recalcula custos de viagens realizadas.
- Filtros: `ViagemVeiculos`, `ViagemMotoristas`, `ViagemStatus`, `ViagemFinalidade`, `ViagemSetores`.

---

## 🧩 Snippet Comentado

```csharp
[Route("CalculaCustoViagens")]
[HttpPost]
public IActionResult CalculaCustoViagens()
{
    var objViagens = _unitOfWork.Viagem.GetAll(v => v.StatusAgendamento == false && v.Status == "Realizada");

    foreach (var viagem in objViagens)
    {
        if (viagem.MotoristaId != null)
            viagem.CustoMotorista = Servicos.CalculaCustoMotorista(viagem, _unitOfWork, ref minutos);

        if (viagem.VeiculoId != null)
        {
            viagem.CustoVeiculo = Servicos.CalculaCustoVeiculo(viagem, _unitOfWork);
            viagem.CustoCombustivel = Servicos.CalculaCustoCombustivel(viagem, _unitOfWork);
        }

        _unitOfWork.Viagem.Update(viagem);
    }

    _unitOfWork.Save();
    return Json(new { success = true });
}
```

---

## ✅ Observações Técnicas

- Usa `Servicos` para cálculo de custos por motorista, veículo e combustível.
- Respostas retornam listas vazias em caso de erro para o frontend.


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
