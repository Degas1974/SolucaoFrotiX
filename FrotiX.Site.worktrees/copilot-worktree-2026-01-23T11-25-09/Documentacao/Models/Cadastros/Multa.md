# Documentação: Multa.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O Model `Multa` representa multas de trânsito recebidas por veículos/motoristas da frota. Inclui informações da infração, valores, prazos, documentos PDF e controle de pagamento.

**Principais características:**

✅ **Multas de Trânsito**: Registro completo de multas  
✅ **Valores Duplos**: Valor até vencimento e após vencimento  
✅ **PDFs**: Múltiplos documentos PDF (autuação, penalidade, comprovante)  
✅ **Controle de Pagamento**: Campos Paga, DataPagamento, ValorPago  
✅ **Processo**: Campo ProcessoEDoc para processos administrativos

## Estrutura do Model

```csharp
public class Multa
{
    [Key]
    public Guid MultaId { get; set; }

    [Required]
    [Display(Name = "Nº da Infração")]
    public string? NumInfracao { get; set; }

    [Required]
    [Display(Name = "Data Infração")]
    public DateTime? Data { get; set; }

    [Required]
    [Display(Name = "Hora")]
    public DateTime? Hora { get; set; }

    [Required]
    [Display(Name = "Localização da Infração")]
    public string? Localizacao { get; set; }

    [Display(Name = "Data de Vencimento")]
    public DateTime? Vencimento { get; set; }

    [DataType(DataType.Currency)]
    [Display(Name = "Valor Até Vencimento")]
    public double? ValorAteVencimento { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    [Display(Name = "Valor Após Vencimento")]
    public double? ValorPosVencimento { get; set; }

    public string? Observacao { get; set; }

    // PDFs
    public string? AutuacaoPDF { get; set; }
    public string? PenalidadePDF { get; set; }
    public string? ComprovantePDF { get; set; }
    public string? ProcessoEdocPDF { get; set; }
    public string? OutrosDocumentosPDF { get; set; }

    // Status
    public bool? Paga { get; set; }
    public bool? EnviadaSecle { get; set; }
    public string? Fase { get; set; }
    public string? ProcessoEDoc { get; set; }
    public string? Status { get; set; }

    // Viagem relacionada
    [Display(Name = "Nº Ficha Vistoria da Viagem")]
    public int? NoFichaVistoria { get; set; }

    // Prazos
    [Required]
    [Display(Name = "Data Notificação")]
    public DateTime? DataNotificacao { get; set; }

    [Required]
    [Display(Name = "Data Limite Reconhecimento")]
    public DateTime? DataLimite { get; set; }

    // Pagamento
    public double? ValorPago { get; set; }
    public DateTime? DataPagamento { get; set; }
    public string? FormaPagamento { get; set; }

    // Relacionamentos
    public Guid? MotoristaId { get; set; }
    [ForeignKey("MotoristaId")]
    public virtual Motorista? Motorista { get; set; }

    public Guid? VeiculoId { get; set; }
    [ForeignKey("VeiculoId")]
    public virtual Veiculo? Veiculo { get; set; }

    public Guid? OrgaoAutuanteId { get; set; }
    [ForeignKey("OrgaoAutuanteId")]
    public virtual OrgaoAutuante? OrgaoAutuante { get; set; }

    public Guid? TipoMultaId { get; set; }
    [ForeignKey("TipoMultaId")]
    public virtual TipoMulta? TipoMulta { get; set; }

    public Guid? EmpenhoMultaId { get; set; }
    [ForeignKey("EmpenhoMultaId")]
    public virtual EmpenhoMulta? EmpenhoMulta { get; set; }

    // Contratos
    public Guid? ContratoVeiculoId { get; set; }
    public Guid? ContratoMotoristaId { get; set; }
    public Guid? AtaVeiculoId { get; set; }
}
```

## Interconexões

Controllers de multa usam este modelo para CRUD e relatórios.

## Notas Importantes

1. **Valores Duplos**: ValorAteVencimento e ValorPosVencimento
2. **PDFs Múltiplos**: Vários campos para diferentes tipos de documentos
3. **Paga Nullable**: Campo Paga é bool? (pode ser null se não foi processado)

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [16/01/2026 17:15] - Auditoria Global: Campos Obrigatórios (.label-required)

**Descrição**: Adicionado asterisco vermelho em labels de campos mandatórios identificados via lógica de validação (Back/Front).

- Definido campo 'ValorAteVencimento' como [Required] e [ValidaZero] para garantir obrigatoriedade no backend.

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
