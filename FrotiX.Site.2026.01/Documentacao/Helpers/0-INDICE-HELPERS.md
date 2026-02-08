# Índice: Documentação de Helpers

> **Última Atualização**: 08/01/2026  
> **Versão**: 1.0

---

## 📋 Status da Documentação

**Total de Arquivos**: 6 arquivos  
**Documentados**: 6/6 (100%)  
**Status**: ✅ **Completo**

---

## ✅ Arquivos Documentados

- [x] [`Alerta.md`](./Alerta.md) - Utilitário de alertas visuais e tratamento de erros
- [x] [`AlertaBackend.md`](./AlertaBackend.md) - Helper backend-only para logging estruturado
- [x] [`ErroHelper.md`](./ErroHelper.md) - Geração de scripts JavaScript para alertas
- [x] [`ImageHelper.md`](./ImageHelper.md) - Validação e redimensionamento de imagens
- [x] [`ListasCompartilhadas.md`](./ListasCompartilhadas.md) - Classes helper para listas compartilhadas
- [x] [`SfdtHelper.md`](./SfdtHelper.md) - Conversão de documentos DOCX para PNG

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
