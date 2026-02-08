# Documentação: MarcaVeiculo.cs

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

O Model `MarcaVeiculo` representa marcas de veículos cadastradas no sistema (ex: Ford, Chevrolet, Volkswagen). Tabela de cadastro simples usada em dropdowns e relacionamentos com veículos.

**Principais características:**

✅ **Cadastro Simples**: Apenas descrição e status  
✅ **Ativo/Inativo**: Permite desativar marcas sem deletar  
✅ **Relacionamento com Modelos**: Modelos de veículos pertencem a marcas  
✅ **Relacionamento com Veículos**: Veículos têm marca

---

## Estrutura do Model

```csharp
public class MarcaVeiculo
{
    [Key]
    public Guid MarcaId { get; set; }

    [StringLength(50, ErrorMessage = "A descrição não pode exceder 50 caracteres")]
    [Required(ErrorMessage = "(A descrição da marca é obrigatória)")]
    [Display(Name = "Marca do Veículo")]
    public string? DescricaoMarca { get; set; }

    [Display(Name = "Ativo/Inativo")]
    public bool Status { get; set; }
}
```

**Propriedades:**

- `MarcaId` (Guid): Chave primária
- `DescricaoMarca` (string?): Nome da marca (obrigatório, max 50)
- `Status` (bool): Ativo/Inativo

### Classe: `MarcaVeiculoViewModel`

```csharp
public class MarcaVeiculoViewModel
{
    public Guid MarcaId { get; set; }
}
```

**Uso**: ViewModel simples para formulários.

---

## Mapeamento Model ↔ Banco de Dados

### Tabela: `MarcaVeiculo`

**Tipo**: Tabela

**SQL de Criação**:
```sql
CREATE TABLE [dbo].[MarcaVeiculo] (
    [MarcaId] uniqueidentifier NOT NULL PRIMARY KEY,
    [DescricaoMarca] nvarchar(50) NOT NULL,
    [Status] bit NOT NULL DEFAULT 1
);
```

**Chaves e Índices**:
- **PK**: `MarcaId` (CLUSTERED)
- **IX**: `IX_MarcaVeiculo_DescricaoMarca` (DescricaoMarca) - Para busca

**Tabelas Relacionadas**:
- `ModeloVeiculo` - Modelos pertencem a marcas (FK MarcaId)
- `Veiculo` - Veículos têm marca (FK MarcaId)

---

## Interconexões

### Quem Chama Este Arquivo

#### 1. **VeiculoController** → Lista Marcas para Dropdown

**Quando**: Formulário de veículo precisa de lista de marcas  
**Por quê**: Popular dropdown de seleção

```csharp
var marcas = _unitOfWork.MarcaVeiculo
    .GetAll(m => m.Status == true)
    .OrderBy(m => m.DescricaoMarca)
    .Select(m => new SelectListItem
    {
        Value = m.MarcaId.ToString(),
        Text = m.DescricaoMarca
    })
    .ToList();
```

#### 2. **ModeloVeiculoController** → Lista Marcas

**Quando**: Formulário de modelo precisa de lista de marcas  
**Por quê**: Modelo pertence a uma marca

---

## Notas Importantes

1. **Simplicidade**: Tabela muito simples, apenas descrição e status
2. **Status**: Sempre filtrar por `Status = true` em listagens
3. **Descrição Única**: Considerar constraint UNIQUE na descrição
4. **Hierarquia**: Marca → Modelo → Veículo

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
