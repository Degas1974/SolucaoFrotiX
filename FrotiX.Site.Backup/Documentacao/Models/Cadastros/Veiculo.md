# Documentação: Veiculo.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O Model `Veiculo` representa veículos da frota. É uma das entidades principais do sistema, com múltiplos relacionamentos e campos importantes para gestão da frota.

**Principais características:**

✅ **Cadastro Completo**: Placa, marca, modelo, quilometragem, etc.  
✅ **Múltiplos Relacionamentos**: Marca, Modelo, Unidade, Combustível, Contrato, Ata  
✅ **Tipos de Veículo**: Próprio/Locação, Reserva/Efetivo, Economildo  
✅ **Documentos**: CRLV digitalizado  
✅ **Patrimônio**: Número de patrimônio

## Estrutura do Model

```csharp
public class Veiculo
{
    [Key]
    public Guid VeiculoId { get; set; }

    [Required]
    [StringLength(10)]
    [Display(Name = "Placa")]
    public string? Placa { get; set; }

    [Display(Name = "Quilometragem")]
    public int? Quilometragem { get; set; }

    [StringLength(20)]
    [Display(Name = "Renavam")]
    public string? Renavam { get; set; }

    [StringLength(20)]
    [Display(Name = "Placa Vinculada")]
    public string? PlacaVinculada { get; set; }

    [Required]
    [Display(Name = "Ano de Fabricacao")]
    public int? AnoFabricacao { get; set; }

    [Required]
    [Display(Name = "Ano do Modelo")]
    public int? AnoModelo { get; set; }

    [Display(Name = "Carro Reserva")]
    public bool Reserva { get; set; }

    [Display(Name = "Ativo/Inativo")]
    public bool Status { get; set; }

    [Display(Name = "Veículo Próprio")]
    public bool VeiculoProprio { get; set; }

    [Display(Name = "Nº Patrimônio")]
    public string? Patrimonio { get; set; }

    [Display(Name = "Categoria")]
    public string? Categoria { get; set; }

    public byte[]? CRLV { get; set; }

    public DateTime? DataAlteracao { get; set; }
    public string? UsuarioIdAlteracao { get; set; }

    // Relacionamentos
    public Guid? PlacaBronzeId { get; set; }
    [ForeignKey("PlacaBronzeId")]
    public virtual PlacaBronze? PlacaBronze { get; set; }

    [Required]
    public Guid? MarcaId { get; set; }
    [ForeignKey("MarcaId")]
    public virtual MarcaVeiculo? MarcaVeiculo { get; set; }

    public Guid? ModeloId { get; set; }
    [ForeignKey("ModeloId")]
    public virtual ModeloVeiculo? ModeloVeiculo { get; set; }

    public Guid? UnidadeId { get; set; }
    [ForeignKey("UnidadeId")]
    public virtual Unidade? Unidade { get; set; }

    public Guid? CombustivelId { get; set; }
    [ForeignKey("CombustivelId")]
    public virtual Combustivel? Combustivel { get; set; }

    // Contratos e Atas (através de tabelas intermediárias)
    public Guid? ItemVeiculoId { get; set; }
    public Guid? ItemVeiculoAtaId { get; set; }
}
```

## Interconexões

Controllers de veículo, viagem, abastecimento e manutenção usam extensivamente este modelo.

## Notas Importantes

1. **Placa Única**: Considerar constraint UNIQUE na Placa
2. **Reserva**: Campo Reserva indica se é veículo reserva ou efetivo
3. **VeiculoProprio**: Indica se é próprio da instituição ou locado
4. **CRLV**: Documento digitalizado armazenado como byte[]

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
