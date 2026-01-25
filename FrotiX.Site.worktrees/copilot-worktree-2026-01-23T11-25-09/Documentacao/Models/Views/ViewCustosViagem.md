# Documentação: ViewCustosViagem.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice
1. [Visão Geral](#visão-geral)
2. [Estrutura do Model](#estrutura-do-model)
3. [Mapeamento Model ↔ Banco de Dados](#mapeamento-model--banco-de-dados)
4. [Interconexões](#interconexões)

---

## Visão Geral

O Model `ViewCustosViagem` representa uma VIEW do banco de dados que consolida informações de viagens com custos detalhados (motorista, veículo, combustível). Usada para análises de custos e relatórios financeiros.

**Principais características:**

✅ **View do Banco**: Representa uma VIEW SQL  
✅ **Custos Detalhados**: CustoMotorista, CustoVeiculo, CustoCombustivel  
✅ **Datas Formatadas**: Algumas datas vêm formatadas como string  
✅ **Análise Financeira**: Focada em informações de custos

---

## Estrutura do Model

```csharp
public class ViewCustosViagem
{
    public Guid ViagemId { get; set; }
    public Guid? MotoristaId { get; set; }
    public Guid? VeiculoId { get; set; }
    public Guid? SetorSolicitanteId { get; set; }
    public int? NoFichaVistoria { get; set; }
    
    // Datas formatadas
    public string? DataInicial { get; set; }
    public string? DataFinal { get; set; }
    public string? HoraInicio { get; set; }
    public string? HoraFim { get; set; }
    
    public string? Finalidade { get; set; }
    public int? KmInicial { get; set; }
    public int? KmFinal { get; set; }
    public int? Quilometragem { get; set; }
    public string? Status { get; set; }
    public string? DescricaoVeiculo { get; set; }
    public string? NomeMotorista { get; set; }
    
    // Custos formatados (string)
    public string? CustoMotorista { get; set; }
    public string? CustoVeiculo { get; set; }
    public string? CustoCombustivel { get; set; }
    
    public bool StatusAgendamento { get; set; }
    public long? RowNum { get; set; }
    
    [NotMapped]
    public IFormFile FotoUpload { get; set; }
}
```

**Propriedades Principais:**

- **Custos**: CustoMotorista, CustoVeiculo, CustoCombustivel (formatados como string)
- **Viagem**: ViagemId, NoFichaVistoria, Status, Finalidade
- **Datas**: Formatadas como string para exibição

---

## Interconexões

### Quem Chama Este Arquivo

Controllers de relatórios de custos e análises financeiras usam esta view.

---

## Notas Importantes

1. **Custos Formatados**: Valores vêm como string formatada (R$)
2. **Análise Financeira**: Focada em informações de custos
3. **FotoUpload**: Campo `[NotMapped]` para upload (não vem da view)

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
