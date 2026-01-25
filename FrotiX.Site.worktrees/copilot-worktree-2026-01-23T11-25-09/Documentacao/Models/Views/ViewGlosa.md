# Documentação: ViewGlosa.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

VIEW complexa que consolida informações de glosas (descontos) em manutenções baseadas em itens de contrato. Calcula dias de glosa e valores de desconto. Usa `[Keyless]` pois é uma view sem chave primária.

## Estrutura do Model

```csharp
[Keyless]
public class ViewGlosa
{
    public string PlacaDescricao { get; set; }
    public Guid ContratoId { get; set; }
    public Guid ManutencaoId { get; set; }
    public string NumOS { get; set; }
    public string ResumoOS { get; set; }
    
    // Datas formatadas (dd/MM/yyyy)
    public string DataSolicitacao { get; set; }
    public string DataDisponibilidade { get; set; }
    public string DataRecolhimento { get; set; }
    public string DataRecebimentoReserva { get; set; }
    public string DataDevolucaoReserva { get; set; }
    public string DataEntrega { get; set; }
    
    // Datas cruas
    public DateTime DataSolicitacaoRaw { get; set; }
    public DateTime? DataDisponibilidadeRaw { get; set; }
    public DateTime? DataDevolucaoRaw { get; set; }
    
    public string StatusOS { get; set; }
    public Guid VeiculoId { get; set; }
    public string DescricaoVeiculo { get; set; }
    public string Sigla { get; set; }
    public string CombustivelDescricao { get; set; }
    public string Placa { get; set; }
    public string Reserva { get; set; }
    
    // Itens do contrato
    public string Descricao { get; set; }
    public int? Quantidade { get; set; }
    public double? ValorUnitario { get; set; }
    public string DataDevolucao { get; set; }
    
    // Cálculos
    public int DiasGlosa { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal ValorGlosa { get; set; }
    public int Dias { get; set; }
    
    // UI
    public string Habilitado { get; set; }
    public string Icon { get; set; }
    public int? NumItem { get; set; }
    public string HabilitadoEditar { get; set; }
}
```

## Interconexões

Controllers de glosa e relatórios financeiros usam esta view para calcular e exibir glosas.

## Notas Importantes

1. **Keyless**: View sem chave primária (`[Keyless]`)
2. **Cálculos**: DiasGlosa e ValorGlosa são calculados na view
3. **Datas Duplas**: Formatadas (string) e raw (DateTime) para diferentes usos

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [08/01/2026] - Documentação Inicial Completa

**Status**: ✅ **Concluído**

**Versão**: 2.0

---

**Última atualização**: 08/01/2026  
**Autor**: Sistema FrotiX  
**Versão**: 2.0


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
