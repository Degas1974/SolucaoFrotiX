# Documentação: OcorrenciaViagemController.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O `OcorrenciaViagemController` gerencia operações CRUD de ocorrências de viagens, incluindo criação, listagem, baixa, reabertura e exclusão.

**Principais características:**

✅ **CRUD Completo**: Criação, listagem, atualização, exclusão  
✅ **Múltiplas Ocorrências**: Criação em lote  
✅ **Status**: Sistema de baixa/reabertura  
✅ **Upload de Imagens**: Upload de imagens de ocorrências  
✅ **Filtros**: Por viagem, veículo, status

**Nota**: Controller implementado como partial class dividido em múltiplos arquivos:
- `OcorrenciaViagemController.cs` - Métodos principais
- `OcorrenciaViagemController.Listar.cs` - Listagens
- `OcorrenciaViagemController.Gestao.cs` - Gestão avançada
- `OcorrenciaViagemController.Upsert.cs` - Criação/edição
- `OcorrenciaViagemController.Debug.cs` - Métodos de debug

---

## Endpoints API Principais

### GET `/api/OcorrenciaViagem/ListarPorViagem`

**Descrição**: Lista ocorrências de uma viagem específica

**Parâmetros**: `viagemId` (Guid)

**Response**: Lista ordenada por data de criação (descendente)

---

### GET `/api/OcorrenciaViagem/ListarAbertasPorVeiculo`

**Descrição**: Lista ocorrências abertas de um veículo (para popup)

**Parâmetros**: `veiculoId` (Guid)

---

### GET `/api/OcorrenciaViagem/ContarAbertasPorVeiculo`

**Descrição**: Conta ocorrências abertas de um veículo

**Parâmetros**: `veiculoId` (Guid)

---

### POST `/api/OcorrenciaViagem/Criar`

**Descrição**: Cria nova ocorrência

**Request Body**: `OcorrenciaViagemDTO`

**Campos**: ViagemId, VeiculoId, MotoristaId, Resumo, Descricao, ImagemOcorrencia

---

### POST `/api/OcorrenciaViagem/CriarMultiplas`

**Descrição**: Cria múltiplas ocorrências de uma vez

**Request Body**: Lista de `OcorrenciaViagemDTO`

**Uso**: Ao finalizar viagem com múltiplas ocorrências

---

### POST `/api/OcorrenciaViagem/DarBaixa`

**Descrição**: Dá baixa em ocorrência (marca como resolvida)

**Parâmetros**: `ocorrenciaId` (Guid)

---

### POST `/api/OcorrenciaViagem/Reabrir`

**Descrição**: Reabre ocorrência baixada

**Parâmetros**: `ocorrenciaId` (Guid)

---

### POST `/api/OcorrenciaViagem/Excluir`

**Descrição**: Exclui ocorrência

**Parâmetros**: `ocorrenciaId` (Guid)

---

## Interconexões

### Quem Chama Este Controller

- **Pages**: `Pages/Viagem/*.cshtml` - Gestão de ocorrências
- **Pages**: `Pages/Ocorrencia/*.cshtml` - Páginas específicas

### O Que Este Controller Chama

- **`_unitOfWork.OcorrenciaViagem`**: CRUD
- **`_unitOfWork.ViewOcorrenciasViagem`**: View otimizada
- **`_unitOfWork.ViewOcorrenciasAbertasVeiculo`**: View de ocorrências abertas

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação da documentação completa do OcorrenciaViagemController

**Arquivos Afetados**:
- `Controllers/OcorrenciaViagemController.cs`
- `Controllers/OcorrenciaViagemController.*.cs` (múltiplos arquivos parciais)

**Impacto**: Documentação de referência para operações de ocorrências

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
