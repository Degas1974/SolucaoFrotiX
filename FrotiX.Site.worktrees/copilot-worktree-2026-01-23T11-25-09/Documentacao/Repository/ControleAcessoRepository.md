# Documentação: ControleAcessoRepository.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O `ControleAcessoRepository` é um repository específico para a entidade `ControleAcesso`, que gerencia permissões de usuários a recursos do sistema.

**Principais características:**

✅ **Herança**: Herda de `Repository<ControleAcesso>`  
✅ **Interface Específica**: Implementa `IControleAcessoRepository`  
✅ **Chave Composta**: Usa `UsuarioId` + `RecursoId` como chave composta  
⚠️ **Dropdown Estranho**: Método retorna `RecursoId` como texto e `UsuarioId` como valor

---

## Métodos Específicos

### `GetControleAcessoListForDropDown()`

**Descrição**: ⚠️ **MÉTODO COM LÓGICA INCOMUM** - Retorna lista de controle de acesso

**Formato**: `RecursoId` como texto, `UsuarioId` como valor

**Nota**: ⚠️ Formato incomum - normalmente seria o contrário ou usar descrições

**Uso**: Possivelmente para listar recursos por usuário ou vice-versa

---

### `Update(ControleAcesso controleAcesso)`

**Descrição**: Atualiza controle de acesso com lógica específica

**Busca**: Usa apenas `RecursoId` para buscar (⚠️ pode não encontrar corretamente se chave for composta)

**Nota**: ⚠️ Chama `SaveChanges()` diretamente (inconsistente com padrão)

**Problema Potencial**: Busca apenas por `RecursoId`, mas chave é composta `(UsuarioId, RecursoId)`

---

## Interconexões

### Quem Usa Este Repository

- **ControleAcessoController**: CRUD de permissões de acesso
- **RecursoController**: Para gerenciar permissões de recursos
- **Sistema de Autenticação**: Para verificar permissões de usuários

---

## Observações Importantes

### Chave Composta

⚠️ **Chave Composta**: `ControleAcesso` tem chave composta `(UsuarioId, RecursoId)`

**Implicação**: Método `Update()` pode não funcionar corretamente se buscar apenas por `RecursoId`

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação da documentação completa do ControleAcessoRepository

**Arquivos Afetados**:
- `Repository/ControleAcessoRepository.cs`

**Impacto**: Documentação de referência para repository de controle de acesso

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
