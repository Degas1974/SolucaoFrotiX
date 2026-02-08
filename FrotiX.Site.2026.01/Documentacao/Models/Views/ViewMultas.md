# Documentação: ViewMultas.cs

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

O Model `ViewMultas` representa uma VIEW do banco de dados que consolida informações completas de multas com dados relacionados de motoristas, veículos, órgãos autuantes e informações de pagamento.

**Principais características:**

✅ **View do Banco**: Representa uma VIEW SQL  
✅ **Dados Consolidados**: Inclui informações de múltiplas tabelas  
✅ **PDFs**: Caminhos de PDFs (AutuacaoPDF, PenalidadePDF, ComprovantePDF)  
✅ **Valores**: Valores antes e depois do vencimento  
✅ **Status de Pagamento**: Campo Paga e DataPagamento

---

## Estrutura do Model

```csharp
public class ViewMultas
{
    public Guid MultaId { get; set; }
    public Guid? MotoristaId { get; set; }
    public Guid? VeiculoId { get; set; }
    public Guid? OrgaoAutuanteId { get; set; }
    public Guid? TipoMultaId { get; set; }

    // PDFs
    public string? AutuacaoPDF { get; set; }
    public string? PenalidadePDF { get; set; }
    public string? ComprovantePDF { get; set; }

    // Dados da multa
    public string? NumInfracao { get; set; }
    public string? Data { get; set; }              // Formatada
    public string? Hora { get; set; }               // Formatada
    public string? Nome { get; set; }               // Nome do motorista
    public string? Placa { get; set; }
    public string? Telefone { get; set; }
    public string? Sigla { get; set; }              // Sigla da unidade
    public string? Localizacao { get; set; }
    public string? Artigo { get; set; }
    public string? Vencimento { get; set; }         // Formatada

    // Valores
    public double? ValorAteVencimento { get; set; }
    public double? ValorPosVencimento { get; set; }

    // Processo
    public string? ProcessoEDoc { get; set; }
    public string? Status { get; set; }
    public string? Fase { get; set; }
    public string? Descricao { get; set; }
    public string? Observacao { get; set; }

    // Pagamento
    public bool Paga { get; set; }
    public string? DataPagamento { get; set; }      // Formatada
    public double? ValorPago { get; set; }
}
```

**Propriedades Principais:**

- **Multa**: MultaId, NumInfracao, Data, Hora, Artigo, Descricao
- **Motorista**: MotoristaId, Nome, Telefone
- **Veículo**: VeiculoId, Placa
- **Órgão**: OrgaoAutuanteId
- **Valores**: ValorAteVencimento, ValorPosVencimento, ValorPago
- **Pagamento**: Paga, DataPagamento
- **PDFs**: AutuacaoPDF, PenalidadePDF, ComprovantePDF

---

## Mapeamento Model ↔ Banco de Dados

### View: `ViewMultas`

**Tipo**: VIEW (não é tabela)

**Tabelas Envolvidas**:
- `Multa` (tabela principal)
- `Motorista` (JOIN)
- `Veiculo` (JOIN)
- `OrgaoAutuante` (JOIN)
- `TipoMulta` (JOIN)
- `Unidade` (LEFT JOIN)

---

## Interconexões

### Quem Chama Este Arquivo

Controllers de multas usam esta view para listagens e consultas com dados consolidados.

---

## Notas Importantes

1. **PDFs**: Caminhos de arquivos PDF armazenados
2. **Valores Duplos**: ValorAteVencimento e ValorPosVencimento
3. **Status**: Campo Status indica situação da multa
4. **Pagamento**: Campo Paga indica se foi paga

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
