# Documentação: Requisitante.cs

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

O Model `Requisitante` representa usuários que solicitam viagens no sistema. Vinculado a um setor solicitante e possui informações de contato (ponto, ramal, email).

**Principais características:**

✅ **Solicitante de Viagens**: Usuário que solicita viagens  
✅ **Vinculação com Setor**: Pertence a um SetorSolicitante  
✅ **Contato**: Ponto, ramal e email  
✅ **Status**: Ativo/Inativo

---

## Estrutura do Model

```csharp
public class Requisitante
{
    [Key]
    public Guid RequisitanteId { get; set; }

    [Required(ErrorMessage = "(O nome do requisitante é obrigatório)")]
    [Display(Name = "Requisitante")]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "(O ponto é obrigatório)")]
    [Display(Name = "Ponto")]
    public string? Ponto { get; set; }

    [ValidaZero(ErrorMessage = "(O ramal é obrigatório)")]
    [Required(ErrorMessage = "(O ramal é obrigatório)")]
    [Display(Name = "Ramal")]
    public int? Ramal { get; set; }

    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Display(Name = "Ativo/Inativo")]
    public bool Status { get; set; }

    public DateTime? DataAlteracao { get; set; }
    public string? UsuarioIdAlteracao { get; set; }

    [Display(Name = "Setor Solicitante")]
    public Guid SetorSolicitanteId { get; set; }

    [ForeignKey("SetorSolicitanteId")]
    public virtual SetorSolicitante SetorSolicitante { get; set; }
}
```

**Propriedades Principais:**

- `RequisitanteId` (Guid): Chave primária
- `Nome` (string?): Nome do requisitante (obrigatório)
- `Ponto` (string?): Ponto do requisitante (obrigatório)
- `Ramal` (int?): Ramal telefônico (obrigatório)
- `Email` (string?): Email (opcional)
- `Status` (bool): Ativo/Inativo
- `SetorSolicitanteId` (Guid): FK para SetorSolicitante (obrigatório)

---

## Mapeamento Model ↔ Banco de Dados

### Tabela: `Requisitante`

**Tipo**: Tabela

**Chaves e Índices**:
- **PK**: `RequisitanteId` (CLUSTERED)
- **FK**: `SetorSolicitanteId` → `SetorSolicitante(SetorSolicitanteId)`

**Tabelas Relacionadas**:
- `SetorSolicitante` - Setor do requisitante
- `Viagem` - Viagens solicitadas pelo requisitante
- `Evento` - Eventos do requisitante

---

## Interconexões

### Quem Chama Este Arquivo

Controllers de viagem e evento usam este modelo para vincular solicitantes.

---

## Notas Importantes

1. **Setor Obrigatório**: Sempre vinculado a um setor
2. **Ramal**: Campo numérico para ramal telefônico
3. **Status**: Filtrar por Status = true em listagens

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
