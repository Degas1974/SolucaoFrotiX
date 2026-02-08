# Documentação: ItensContratoController.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O `ItensContratoController` gerencia operações relacionadas a itens de contratos e atas de registro de preços, incluindo listagem de contratos/atas, detalhes, veículos associados e gestão de itens.

**Principais características:**

✅ **Listagem de Contratos/Atas**: Endpoints para dropdowns e seleção  
✅ **Detalhes**: Obtém informações completas de contratos e atas  
✅ **Veículos**: Gerencia veículos associados a contratos  
✅ **Itens**: CRUD de itens de veículos em contratos/atas  
✅ **Repactuações**: Suporta repactuações de contratos

**Nota**: Controller implementado como partial class dividido em múltiplos arquivos.

---

## Endpoints API Principais

### GET `/api/ItensContrato/ListaContratos`

**Descrição**: Lista contratos ativos formatados para dropdowns

**Parâmetros**: `status` (bool, default: true)

**Response**: Lista com `value`, `text` e `tipoContrato`

---

### GET `/api/ItensContrato/ListaAtas`

**Descrição**: Lista atas ativas formatadas para dropdowns

---

### GET `/api/ItensContrato/GetContratoDetalhes`

**Descrição**: Obtém detalhes completos de um contrato

**Parâmetros**: `id` (Guid)

**Response**: Informações completas incluindo flags de terceirização, quantidades e custos mensais

---

### GET `/api/ItensContrato/GetVeiculosContrato`

**Descrição**: Lista veículos associados a um contrato com informações de itens

**Parâmetros**: `contratoId` (Guid)

**Response**: Lista de veículos com informações de itens de repactuação

---

### GET `/api/ItensContrato/GetVeiculosDisponiveis`

**Descrição**: Lista veículos disponíveis (não associados ao contrato)

**Parâmetros**: `contratoId` (Guid)

---

## Interconexões

### Quem Chama Este Controller

- **Pages**: Páginas de gestão de contratos
- **Pages**: Páginas de gestão de atas
- **Pages**: Formulários de repactuação

### O Que Este Controller Chama

- **`_unitOfWork.Contrato`**: CRUD de contratos
- **`_unitOfWork.AtaRegistroPrecos`**: CRUD de atas
- **`_unitOfWork.VeiculoContrato`**: Relacionamentos veículo-contrato
- **`_unitOfWork.ItemVeiculoContrato`**: Itens de veículos
- **`_unitOfWork.RepactuacaoContrato`**: Repactuações

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação da documentação completa do ItensContratoController

**Arquivos Afetados**:
- `Controllers/ItensContratoController.cs`

**Impacto**: Documentação de referência para operações de itens de contratos

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
