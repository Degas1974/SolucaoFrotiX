# Documentação: Empenho.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice
1. [Visão Geral](#visão-geral)
2. [Estrutura do Model](#estrutura-do-model)
3. [Mapeamento Model ↔ Banco de Dados](#mapeamento-model--banco-de-dados)
4. [Interconexões](#interconexões)
5. [Lógica de Negócio](#lógica-de-negócio)

---

## Visão Geral

O Model `Empenho` representa empenhos orçamentários vinculados a contratos ou atas de registro de preços. Controla saldos disponíveis para despesas e movimentações financeiras.

**Principais características:**

✅ **Controle Orçamentário**: Gerencia saldos de empenhos  
✅ **Vinculação**: Pode estar vinculado a Contrato ou Ata  
✅ **Vigência**: Período de vigência do empenho  
✅ **Saldo**: Saldo inicial e final controlados

---

## Estrutura do Model

```csharp
public class Empenho
{
    [Key]
    public Guid EmpenhoId { get; set; }

    [Required(ErrorMessage = "(A nota de Empenho é obrigatória)")]
    [MinLength(12), MaxLength(12)]
    [Display(Name = "Nota de Empenho")]
    public string? NotaEmpenho { get; set; }

    [DataType(DataType.DateTime)]
    [Required(ErrorMessage = "(A data de emissão é obrigatória)")]
    [Display(Name = "Data de Emissão")]
    public DateTime? DataEmissao { get; set; }

    [DataType(DataType.DateTime)]
    [Required(ErrorMessage = "(A data de vigência inicial é obrigatória)")]
    [Display(Name = "Vigência Inicial")]
    public DateTime? VigenciaInicial { get; set; }

    [DataType(DataType.DateTime)]
    [Required(ErrorMessage = "(A data de vigência final é obrigatória)")]
    [Display(Name = "Vigência Final")]
    public DateTime? VigenciaFinal { get; set; }

    [ValidaZero(ErrorMessage = "(O ano de vigência é obrigatório)")]
    [Required(ErrorMessage = "(O ano de vigência é obrigatório)")]
    [Display(Name = "Ano de Vigência")]
    public int? AnoVigencia { get; set; }

    [ValidaZero(ErrorMessage = "(O saldo inicial é obrigatório)")]
    [Required(ErrorMessage = "(O saldo inicial é obrigatório)")]
    [DataType(DataType.Currency)]
    [Display(Name = "Saldo Inicial (R$)")]
    public double? SaldoInicial { get; set; }

    [DataType(DataType.Currency)]
    [Display(Name = "Saldo Final (R$)")]
    public double? SaldoFinal { get; set; }

    [Display(Name = "Contrato")]
    public Guid? ContratoId { get; set; }

    [ForeignKey("ContratoId")]
    public virtual Contrato Contrato { get; set; }

    [Display(Name = "Ata de Registro de Preços")]
    public Guid? AtaId { get; set; }

    [ForeignKey("AtaId")]
    public virtual AtaRegistroPrecos AtaRegistroPrecos { get; set; }
}
```

**Propriedades Principais:**

- `EmpenhoId` (Guid): Chave primária
- `NotaEmpenho` (string): Número da nota de empenho (12 caracteres)
- `DataEmissao` (DateTime?): Data de emissão
- `VigenciaInicial`/`VigenciaFinal` (DateTime?): Período de vigência
- `AnoVigencia` (int?): Ano de vigência
- `SaldoInicial` (double?): Saldo inicial do empenho
- `SaldoFinal` (double?): Saldo final (calculado)
- `ContratoId` (Guid?): FK para Contrato (opcional)
- `AtaId` (Guid?): FK para AtaRegistroPrecos (opcional)

**Validações:**

- `NotaEmpenho`: Deve ter exatamente 12 caracteres
- `SaldoInicial`: Deve ser > 0 (ValidaZero)

---

## Mapeamento Model ↔ Banco de Dados

### Tabela: `Empenho`

**Tipo**: Tabela

**Chaves e Índices**:
- **PK**: `EmpenhoId` (CLUSTERED)
- **FK**: `ContratoId` → `Contrato(ContratoId)` (opcional)
- **FK**: `AtaId` → `AtaRegistroPrecos(AtaId)` (opcional)

---

## Interconexões

### Quem Chama Este Arquivo

Controllers de empenho e relatórios financeiros usam este modelo.

---

## Lógica de Negócio

### Cálculo de Saldo Final

Saldo final é calculado subtraindo movimentações e notas:

```csharp
var movimentacoes = _unitOfWork.MovimentacaoEmpenho
    .GetAll(m => m.EmpenhoId == empenhoId)
    .Sum(m => m.Valor);

var notas = _unitOfWork.NotaFiscal
    .GetAll(n => n.EmpenhoId == empenhoId)
    .Sum(n => n.ValorNF);

empenho.SaldoFinal = empenho.SaldoInicial - movimentacoes - notas;
```

---

## Notas Importantes

1. **Vinculação Opcional**: Pode estar vinculado a Contrato OU Ata (não ambos obrigatoriamente)
2. **Saldo Final**: Calculado automaticamente
3. **Nota de Empenho**: Formato fixo de 12 caracteres

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
