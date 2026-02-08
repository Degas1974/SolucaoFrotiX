# Documentação: ViagemEventoDto.cs

**📅 Última Atualização:** 08/01/2026  
**📋 Versão:** 2.0 (Padrão FrotiX Simplificado)

---

## 🎯 Objetivos

O DTO `ViagemEventoDto` transfere dados de viagens vinculadas a eventos, incluindo informações do evento, viagem, veículo, motorista e custos, usado principalmente em listagens e dashboards.

**Principais objetivos:**

✅ Transferir dados agregados de viagem + evento  
✅ Incluir informações formatadas para exibição  
✅ Otimizar queries evitando múltiplos JOINs repetidos

---

## 🏗️ Estrutura do Model

```csharp
public class ViagemEventoDto
{
    public Guid EventoId { get; set; }
    public Guid ViagemId { get; set; }
    public int NoFichaVistoria { get; set; }
    public string NomeRequisitante { get; set; }
    public string NomeSetor { get; set; }
    public string NomeMotorista { get; set; }
    public string DescricaoVeiculo { get; set; }
    public decimal CustoViagem { get; set; }
    public DateTime DataInicial { get; set; }
    public DateTime? HoraInicio { get; set; } // ✅ DateTime? (não TimeSpan?)
    public string Placa { get; set; }
}
```

---

## 🔗 Quem Chama e Por Quê

### ViagemEventoController.cs → Listar Viagens de Evento

```csharp
[HttpGet("ListarViagensEvento/{eventoId}")]
public IActionResult ListarViagensEvento(Guid eventoId)
{
    var viagens = _context.Viagem
        .Where(v => v.EventoId == eventoId)
        .Select(v => new ViagemEventoDto
        {
            EventoId = eventoId,
            ViagemId = v.ViagemId,
            NoFichaVistoria = v.NoFichaVistoria ?? 0,
            NomeRequisitante = v.Requisitante.Nome,
            NomeSetor = v.SetorSolicitante.Nome,
            NomeMotorista = v.Motorista.Nome,
            DescricaoVeiculo = $"{v.Veiculo.Placa} - {v.Veiculo.ModeloVeiculo.DescricaoModelo}",
            CustoViagem = v.CustoTotal ?? 0,
            DataInicial = v.DataInicial ?? DateTime.MinValue,
            HoraInicio = v.HoraInicio,
            Placa = v.Veiculo.Placa
        })
        .ToList();
    
    return Json(new { data = viagens });
}
```

---

## 📝 Notas Importantes

1. **HoraInicio como DateTime?** - Comentário no código indica que deve ser `DateTime?` e não `TimeSpan?`.

2. **Dados agregados** - Inclui informações de múltiplas entidades relacionadas.

---

**📅 Documentação criada em:** 08/01/2026


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
