# Documentação: ViewMotoristas.cs

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

O Model `ViewMotoristas` representa uma VIEW do banco de dados que consolida informações de motoristas com dados relacionados de unidades, contratos e fornecedores. Facilita listagens e consultas sem joins complexos.

**Principais características:**

✅ **View do Banco**: Representa uma VIEW SQL  
✅ **Dados Consolidados**: Inclui informações de múltiplas tabelas  
✅ **Contrato/Fornecedor**: Informações de vinculação  
✅ **Foto**: Inclui foto do motorista

---

## Estrutura do Model

```csharp
public class ViewMotoristas
{
    public Guid MotoristaId { get; set; }
    public string? Nome { get; set; }
    public string? MotoristaCondutor { get; set; }
    public string? Ponto { get; set; }
    public string? CNH { get; set; }
    public string? CategoriaCNH { get; set; }
    public string? Celular01 { get; set; }
    public bool Status { get; set; }
    public string? Sigla { get; set; }              // Sigla da unidade
    public string? AnoContrato { get; set; }
    public string? NumeroContrato { get; set; }
    public string? DescricaoFornecedor { get; set; }
    public string? NomeCompleto { get; set; }       // Usuário que alterou
    public string? TipoCondutor { get; set; }
    public string? EfetivoFerista { get; set; }
    public byte[]? Foto { get; set; }
    public DateTime? DataAlteracao { get; set; }
    public Guid? ContratoId { get; set; }
}
```

**Propriedades Principais:**

- **Motorista**: MotoristaId, Nome, Ponto, CNH, CategoriaCNH, Celular01, Status
- **Unidade**: Sigla
- **Contrato**: ContratoId, AnoContrato, NumeroContrato
- **Fornecedor**: DescricaoFornecedor
- **Tipo**: TipoCondutor, EfetivoFerista
- **Auditoria**: NomeCompleto, DataAlteracao
- **Foto**: Foto (byte[])

---

## Mapeamento Model ↔ Banco de Dados

### View: `ViewMotoristas`

**Tipo**: VIEW (não é tabela)

**Tabelas Envolvidas**:
- `Motorista` (tabela principal)
- `Unidade` (LEFT JOIN)
- `MotoristaContrato` (LEFT JOIN)
- `Contrato` (LEFT JOIN)
- `Fornecedor` (LEFT JOIN)
- `AspNetUsers` (LEFT JOIN)

---

## Interconexões

### Quem Chama Este Arquivo

Controllers de motorista usam esta view para listagens e consultas com dados consolidados.

---

## Notas Importantes

1. **Contrato/Fornecedor**: Informações de vinculação incluídas
2. **Foto**: Campo binário para foto do motorista
3. **Status**: Filtro comum por Status = true

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
