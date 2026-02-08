# Documentação: ContratoRepository.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O `ContratoRepository` é um repository específico para a entidade `Contrato`, com método otimizado para dropdowns que suporta filtro por tipo de contrato.

**Principais características:**

✅ **Herança**: Herda de `Repository<Contrato>`  
✅ **Interface Específica**: Implementa `IContratoRepository`  
✅ **Dropdown Otimizado**: Retorna `IQueryable` para composição  
✅ **Filtro por Tipo**: Suporta filtro opcional por `TipoContrato`  
✅ **Formatação Inteligente**: Texto formatado diferente com/sem tipo

---

## Estrutura da Classe

### Herança e Implementação

```csharp
public class ContratoRepository : Repository<Contrato>, IContratoRepository
```

---

## Método Específico

### `GetDropDown(string? tipoContrato = null)`

**Descrição**: **MÉTODO OTIMIZADO** - Retorna query para dropdown de contratos

**Retorno**: `IQueryable<SelectListItem>` - Permite composição adicional

**Parâmetros**:
- `tipoContrato`: Filtro opcional por tipo de contrato

**Características**:
- **Filtro de Status**: Apenas contratos com `Status == true`
- **Filtro por Tipo**: Se `tipoContrato` fornecido, filtra por tipo
- **Ordenação**: Por `AnoContrato` desc, `NumeroContrato` desc, `Fornecedor.DescricaoFornecedor` desc
- **Formatação**: 
  - Com tipo: `"{Ano}/{Numero} - {Fornecedor}"`
  - Sem tipo: `"{Ano}/{Numero} - {Fornecedor} ({TipoContrato})"`
- **AsNoTracking**: Usa `AsNoTracking()` para performance
- **Navegação**: Acessa `Fornecedor.DescricaoFornecedor` diretamente (EF Core faz JOIN automaticamente)

**Uso**:
```csharp
// Sem filtro
var contratos = unitOfWork.Contrato.GetDropDown();

// Com filtro por tipo
var contratosServicos = unitOfWork.Contrato.GetDropDown("Servicos");

// Composição adicional
var top10 = unitOfWork.Contrato.GetDropDown()
    .Take(10)
    .ToList();
```

**Vantagem**: Retorna `IQueryable` permitindo composição antes de materializar

---

## Interconexões

### Quem Usa Este Repository

- **ContratoController**: Para listagem de contratos
- **NotaFiscalController**: Para seleção de contratos em notas fiscais
- **Views Razor**: Para dropdowns de contratos

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação da documentação completa do ContratoRepository

**Arquivos Afetados**:
- `Repository/ContratoRepository.cs`

**Impacto**: Documentação de referência para repository de contratos

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

## [19/01/2026] - Manutencao: Remocao de using duplicado do EF Core

**Descricao**: Removida duplicidade de `using Microsoft.EntityFrameworkCore` no cabecalho para eliminar warnings CS0105.

**Arquivos Afetados**:
- Repository/ContratoRepository.cs

**Mudancas**:
- Remocao do `using Microsoft.EntityFrameworkCore` duplicado.

**Impacto**: Nenhuma mudanca funcional; apenas limpeza de compilacao.

**Status**: Concluido

**Responsavel**: Codex

**Versao**: Incremento de patch

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
