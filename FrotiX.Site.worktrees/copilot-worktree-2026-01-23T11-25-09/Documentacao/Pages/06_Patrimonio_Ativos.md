# Guia Patrimonial: Inventário e Movimentação

Focado em ativos auxiliares (equipamentos, móveis, rádios, sobressalentes) que não são veículos, mas pertencem à frota.

## 📦 Inventário (Pages/Patrimonio)
- **Tagueamento:** Cada item possui um código patrimonial único.
- **Categorização:** Organização por Seções e Setores Patrimoniais para facilitar auditorias anuais.

## 🔄 Movimentação (Pages/MovimentacaoPatrimonio)
- **Termo de Responsabilidade:** Geração automática de documento de cautela quando um item é movimentado entre unidades ou entregue a um colaborador.
- **Histórico de Posse:** Rastro completo de por onde o equipamento passou e quem foi o último responsável.

## 🛠 Detalhes Técnicos
- **Hierarquia de Localização:** Utiliza um sistema de Setores/Seções que reflete a estrutura física (ex: Almoxarifado -> Prateleira A).


## 📂 Arquivos do Módulo (Listagem Completa)

### 📦 Gestão de Itens Patrimoniais
- Pages/Patrimonio/Index.cshtml & .cs: Listagem e busca de bens inventariados.
- Pages/Patrimonio/Upsert.cshtml & .cs: Cadastro técnico, marca e número de série de ativos.

### 🔄 Movimentações e Transferências
- Pages/MovimentacaoPatrimonio/Index.cshtml & .cs: Histórico de trocas de guarda e transferências.
- Pages/MovimentacaoPatrimonio/Upsert.cshtml & .cs: Registro de novas movimentações com geração de termo.

### 🏢 Estrutura de Localização
- Pages/SecaoPatrimonial/Index.cshtml & .cs / Upsert.cshtml & .cs: Divisão física de nível 1.
- Pages/SetorPatrimonial/Index.cshtml & .cs / Upsert.cshtml & .cs: Divisão física de nível 2 (Sub-setor).


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
