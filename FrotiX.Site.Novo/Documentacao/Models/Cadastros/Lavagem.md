# Documentação: Lavagem.cs

> **Última Atualização**: 03/02/2026  
> **Versão Atual**: 2.2

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O Model `Lavagem` representa registros de lavagem de veículos. Vinculado a um veículo e motorista, com controle de data e horário único da lavagem.

## Estrutura do Model

```csharp
public class Lavagem
{
    [Key]
    public Guid LavagemId { get; set; }

    [Display(Name = "Data")]
    public DateTime? Data { get; set; }

    [Display(Name = "Horário da Lavagem")]
    public DateTime? HorarioLavagem { get; set; }

    [Display(Name = "Veículo Lavado")]
    public Guid VeiculoId { get; set; }
    [ForeignKey("VeiculoId")]
    public virtual Veiculo? Veiculo { get; set; }

    [Display(Name = "Motorista")]
    public Guid MotoristaId { get; set; }
    [ForeignKey("MotoristaId")]
    public virtual Motorista? Motorista { get; set; }
}
```

## Notas Importantes

1. **Lavadores Múltiplos**: Uma lavagem pode ter múltiplos lavadores (tabela `LavadoresLavagem`)
2. **Horário Único**: A lavagem registra apenas o horário principal (`HorarioLavagem`)

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [03/02/2026] - Unificação de Horário da Lavagem

**Descrição**:
- Removidos HorarioInicio/HorarioFim do model
- Adicionado `HorarioLavagem` (coluna `HorarioLavagem`)

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
