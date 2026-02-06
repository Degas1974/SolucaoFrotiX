# AlertasFrotiXController.cs — Alertas e notificações

> **Arquivo:** `Controllers/AlertasFrotiXController.cs`  
> **Papel:** endpoints para alertas do sistema e métricas de leitura.

---

## ✅ Visão Geral

Controller API que consulta alertas, monta estatísticas de leitura e retorna detalhes completos para o frontend.

---

## 🔧 Endpoints Principais

- `GetDetalhesAlerta/{id}`: carrega alerta, destinatários e estatísticas.

---

## 🧩 Snippet Comentado

```csharp
[HttpGet("GetDetalhesAlerta/{id}")]
public async Task<IActionResult> GetDetalhesAlerta(Guid id)
{
    var alerta = await _unitOfWork.AlertasFrotiX.GetFirstOrDefaultAsync(
        a => a.AlertasFrotiXId == id,
        includeProperties: "AlertasUsuarios,Viagem,Manutencao,Veiculo,Motorista");

    if (alerta == null)
    {
        return NotFound(new { success = false, message = "Alerta não encontrado" });
    }

    // ... monta estatísticas de leitura
    return Ok(new { success = true, data = new { alertaId = alerta.AlertasFrotiXId } });
}
```

---

## ✅ Observações Técnicas

- Usa `AlertasHub` para integrações em tempo real.
- Mapeia ícones/cores com `fa-duotone` por tipo e prioridade.


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
