# Documentação: ViewLavagem.cs

> **Última Atualização**: 03/02/2026  
> **Versão Atual**: 2.2

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice
1. [Visão Geral](#visão-geral)
2. [Estrutura do Model](#estrutura-do-model)
3. [Mapeamento Model ↔ Banco de Dados](#mapeamento-model--banco-de-dados)

---

## Visão Geral

O Model `ViewLavagem` representa uma VIEW do banco de dados que consolida informações de lavagens de veículos com dados relacionados de motoristas, veículos e lavadores.

**Principais características:**

✅ **View do Banco**: Representa uma VIEW SQL  
✅ **Dados Consolidados**: Inclui informações de múltiplas tabelas  
✅ **Lavadores Múltiplos**: Campo `LavadoresId` e `Lavadores` (string concatenada)

---

## Estrutura do Model

```csharp
public class ViewLavagem
{
    public Guid LavagemId { get; set; }
    public Guid MotoristaId { get; set; }
    public Guid VeiculoId { get; set; }
    public string? LavadoresId { get; set; }      // IDs concatenados
    public string? Data { get; set; }             // Formatada
    public string? Horario { get; set; }          // Hora da lavagem (formatada)
    public string? Lavadores { get; set; }        // Nomes concatenados
    public string? DescricaoVeiculo { get; set; }
    public string? Nome { get; set; }             // Nome do motorista
}
```

**Propriedades Principais:**

- **Lavagem**: LavagemId, Data, Horario
- **Lavadores**: LavadoresId (IDs), Lavadores (nomes)
- **Veículo**: VeiculoId, DescricaoVeiculo
- **Motorista**: MotoristaId, Nome

---

## Mapeamento Model ↔ Banco de Dados

### View: `ViewLavagem`

**Tipo**: VIEW (não é tabela)

**Tabelas Envolvidas**:
- `Lavagem` (tabela principal)
- `Motorista` (JOIN)
- `Veiculo` (JOIN)
- `LavadoresLavagem` (JOIN para múltiplos lavadores)

**Cálculos na View**:
- `Horario`: `CONVERT(VARCHAR, Lavagem.HorarioInicio, 8)`
- `Lavadores`: STRING_AGG ou CONCAT de nomes dos lavadores

---

## Notas Importantes

1. **Lavadores Múltiplos**: Uma lavagem pode ter múltiplos lavadores
3. **Datas Formatadas**: Data e horário vêm formatados como string

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [03/02/2026] - Unificação do Horário na View

**Descrição**:
- Removidos HorarioInicio/HorarioFim do model
- Adicionado `Horario` como horário único da lavagem

**Status**: ✅ **Concluído**

**Versão**: 2.1

---

## [03/02/2026] - Remoção de DuracaoMinutos

**Descrição**:
- Removido campo `DuracaoMinutos` do model e da documentação

**Status**: ✅ **Concluído**

**Versão**: 2.2

---

## [08/01/2026] - Documentação Inicial Completa

**Status**: ✅ **Concluído**

**Versão**: 2.0

---

**Última atualização**: 03/02/2026  
**Autor**: Sistema FrotiX  
**Versão**: 2.2
