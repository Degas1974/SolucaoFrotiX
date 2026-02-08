# Índice: Documentação de IRepository

> **Última Atualização**: 08/01/2026  
> **Versão**: 1.0

---

## 📋 Status da Documentação

**Total de Arquivos**: ~103 interfaces  
**Documentados (Principais)**: 2/103  
**Padrão Documentado**: ✅ Sim

---

## ✅ Interfaces Base Documentadas

- [x] [`IRepository.md`](./IRepository.md) - Interface genérica base
- [x] [`IUnitOfWork.md`](./IUnitOfWork.md) - Interface Unit of Work (principal + extensões)

---

## 📝 Interfaces Específicas

**Total**: ~100 interfaces específicas seguindo o padrão `I{Entidade}Repository : IRepository<{Entidade}>`

### Padrão de Nomenclatura

- `I{Entidade}Repository.cs`
- Herda de `IRepository<{Entidade}>`
- Define métodos específicos quando necessário

### Exemplos

- `IVeiculoRepository : IRepository<Veiculo>`
- `IMotoristaRepository : IRepository<Motorista>`
- `IViewAbastecimentosRepository : IRepository<ViewAbastecimentos>`

---

## 📚 Documentação de Referência

Para entender como as interfaces específicas funcionam, consulte:
- [`IRepository.md`](./IRepository.md) - Contrato base
- [`../PADRAO-REPOSITORIES-ESPECIFICOS.md`](../PADRAO-REPOSITORIES-ESPECIFICOS.md) - Padrão completo

---

**Última atualização**: 08/01/2026  
**Versão**: 1.0


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
