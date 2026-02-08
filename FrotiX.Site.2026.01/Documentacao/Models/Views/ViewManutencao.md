# Documentação: ViewManutencao.cs

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

O Model `ViewManutencao` representa uma VIEW do banco de dados que consolida informações de manutenções com dados relacionados de veículos, contratos e veículos reserva. Inclui datas formatadas e informações de status da OS (Ordem de Serviço).

**Principais características:**

✅ **View do Banco**: Representa uma VIEW SQL  
✅ **Dados Consolidados**: Inclui informações de múltiplas tabelas  
✅ **Datas Formatadas**: Algumas datas vêm formatadas como string  
✅ **Veículo Reserva**: Informações do veículo reserva incluídas

---

## Estrutura do Model

```csharp
public class ViewManutencao
{
    // Chaves
    public Guid ManutencaoId { get; set; }
    public Guid? ContratoId { get; set; }
    public Guid VeiculoId { get; set; }

    // Strings básicas
    public string? NumOS { get; set; }
    public string? ResumoOS { get; set; }
    public string? StatusOS { get; set; }

    // Datas formatadas (char(10) no banco)
    public string? DataSolicitacao { get; set; }        // Formatada dd/MM/yyyy
    public string? DataDisponibilidade { get; set; }
    public string? DataRecolhimento { get; set; }
    public string? DataRecebimentoReserva { get; set; }
    public string? DataDevolucaoReserva { get; set; }
    public string? DataEntrega { get; set; }
    public string? DataDevolucao { get; set; }

    // Datas cruas (datetime no banco)
    public DateTime? DataSolicitacaoRaw { get; set; }
    // ... outros campos raw

    // Veículo Reserva
    public Guid? VeiculoReservaId { get; set; }
    public string? PlacaReserva { get; set; }
    // ... outros campos de reserva
}
```

**Propriedades Principais:**

- **Manutenção**: ManutencaoId, NumOS, ResumoOS, StatusOS
- **Datas Formatadas**: DataSolicitacao, DataDisponibilidade, etc. (string)
- **Datas Raw**: DataSolicitacaoRaw, etc. (DateTime?) para ordenação/filtros
- **Veículo**: VeiculoId
- **Veículo Reserva**: VeiculoReservaId, PlacaReserva
- **Contrato**: ContratoId

---

## Mapeamento Model ↔ Banco de Dados

### View: `ViewManutencao`

**Tipo**: VIEW (não é tabela)

**Tabelas Envolvidas**:
- `Manutencao` (tabela principal)
- `Veiculo` (JOIN - veículo principal)
- `Veiculo` (LEFT JOIN - veículo reserva)
- `Contrato` (LEFT JOIN)

**Formatação de Datas**:
```sql
CONVERT(VARCHAR, DataSolicitacao, 103) AS DataSolicitacao,  -- dd/MM/yyyy
DataSolicitacao AS DataSolicitacaoRaw                       -- datetime raw
```

---

## Interconexões

### Quem Chama Este Arquivo

Controllers de manutenção usam esta view para listagens e consultas.

---

## Notas Importantes

1. **Datas Duplas**: Campos formatados (string) e raw (DateTime?) para diferentes usos
2. **Veículo Reserva**: Informações do veículo reserva incluídas
3. **Status OS**: Campo StatusOS indica status da ordem de serviço

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
