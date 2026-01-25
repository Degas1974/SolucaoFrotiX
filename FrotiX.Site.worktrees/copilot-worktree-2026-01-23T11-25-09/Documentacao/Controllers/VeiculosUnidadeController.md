# Documentação: VeiculosUnidadeController.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O `VeiculosUnidadeController` gerencia a relação entre veículos e unidades, permitindo listar veículos de uma unidade específica e remover associação.

**Principais características:**

✅ **Listagem Filtrada**: Lista veículos de uma unidade específica  
✅ **Remoção de Associação**: Remove veículo de unidade (não exclui veículo)

---

## Endpoints API

### GET `/api/VeiculosUnidade`

**Descrição**: Retorna lista de veículos de uma unidade específica

**Parâmetros**: `id` (Guid) - ID da unidade

**Response**: Lista de veículos com informações completas (marca, modelo, combustível, contrato, etc.)

**Quando é chamado**: Para exibir veículos de uma unidade específica

---

### POST `/api/VeiculosUnidade/Delete`

**Descrição**: Remove associação de veículo com unidade

**Request Body**: `VeiculoViewModel` com `VeiculoId`

**Lógica**: Define `Veiculo.UnidadeId = Guid.Empty` (não exclui o veículo)

---

## Interconexões

### Quem Chama Este Controller

- **Pages**: Páginas de gestão de unidades
- **Pages**: Relatórios por unidade

### O Que Este Controller Chama

- **`_unitOfWork.Veiculo`**: CRUD
- **`_unitOfWork.ModeloVeiculo`**: Join para marca/modelo
- **`_unitOfWork.MarcaVeiculo`**: Join para marca
- **`_unitOfWork.Unidade`**: Join para informações de unidade
- **`_unitOfWork.Combustivel`**: Join para tipo de combustível
- **`_unitOfWork.Contrato`**: Join para informações de contrato
- **`_unitOfWork.Fornecedor`**: Join para fornecedor
- **`_unitOfWork.AspNetUsers`**: Join para usuário de alteração

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação da documentação completa do VeiculosUnidadeController

**Arquivos Afetados**:
- `Controllers/VeiculosUnidadeController.cs`

**Impacto**: Documentação de referência para relação veículos-unidades

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
