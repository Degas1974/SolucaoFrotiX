# Documentação: Fornecedor.cs

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

O Model `Fornecedor` representa fornecedores de serviços (ex: empresas de locação de veículos, fornecedores de combustível). Cadastro simples com informações de contato e CNPJ.

**Principais características:**

✅ **Cadastro de Fornecedores**: Empresas fornecedoras de serviços  
✅ **CNPJ**: Campo obrigatório para identificação  
✅ **Múltiplos Contatos**: Suporta até 2 contatos  
✅ **Status**: Ativo/Inativo

---

## Estrutura do Model

```csharp
public class Fornecedor
{
    [Key]
    public Guid FornecedorId { get; set; }

    [Required(ErrorMessage = "(O nome do fornecedor é obrigatório)")]
    [Display(Name = "Nome do Fornecedor")]
    public string DescricaoFornecedor { get; set; }

    [Required(ErrorMessage = "(O CNPJ do fornecedor é obrigatório)")]
    [Display(Name = "CNPJ")]
    public string CNPJ { get; set; }

    [Display(Name = "Endereço")]
    public string? Endereco { get; set; }

    [Required(ErrorMessage = "(O contato é obrigatório)")]
    [Display(Name = "Contato (1º)")]
    public string Contato01 { get; set; }

    [ValidaZero(ErrorMessage = "(O telefone é obrigatório)")]
    [Required(ErrorMessage = "(O telefone é obrigatório)")]
    [Display(Name = "Telefone/Celular (1º)")]
    public string Telefone01 { get; set; }

    [Display(Name = "Contato (2º)")]
    public string? Contato02 { get; set; }

    [Display(Name = "Telefone/Celular (2º)")]
    public string? Telefone02 { get; set; }

    [Display(Name = "Ativo/Inativo")]
    public bool Status { get; set; }
}
```

**Propriedades Principais:**

- `FornecedorId` (Guid): Chave primária
- `DescricaoFornecedor` (string): Nome do fornecedor (obrigatório)
- `CNPJ` (string): CNPJ do fornecedor (obrigatório)
- `Endereco` (string?): Endereço (opcional)
- `Contato01`/`Telefone01` (string): Primeiro contato (obrigatório)
- `Contato02`/`Telefone02` (string?): Segundo contato (opcional)
- `Status` (bool): Ativo/Inativo

---

## Mapeamento Model ↔ Banco de Dados

### Tabela: `Fornecedor`

**Tipo**: Tabela

**Chaves e Índices**:
- **PK**: `FornecedorId` (CLUSTERED)
- **IX**: `IX_Fornecedor_CNPJ` (CNPJ) - Para busca por CNPJ

**Tabelas Relacionadas**:
- `Contrato` - Contratos podem ter FornecedorId
- `AtaRegistroPrecos` - Atas podem ter FornecedorId

---

## Interconexões

### Quem Chama Este Arquivo

Controllers de fornecedor e contratos usam este modelo.

---

## Notas Importantes

1. **CNPJ Único**: Considerar constraint UNIQUE no CNPJ
2. **Validação CNPJ**: Validar formato de CNPJ (14 dígitos)
3. **Contatos**: Primeiro contato obrigatório, segundo opcional

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
