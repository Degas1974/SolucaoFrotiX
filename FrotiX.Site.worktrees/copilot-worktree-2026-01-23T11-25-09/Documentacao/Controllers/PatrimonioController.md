# Documentação: PatrimonioController.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O `PatrimonioController` gerencia operações CRUD de patrimônio e movimentações patrimoniais, incluindo filtros avançados e prevenção de duplicação.

**Principais características:**

✅ **CRUD Completo**: Listagem, criação e consulta de movimentações  
✅ **Filtros Avançados**: Por marca, modelo, setor, seção, situação  
✅ **Movimentações**: Gestão de movimentações entre setores/seções  
✅ **Prevenção de Duplicação**: Sistema de lock para evitar requisições duplicadas  
✅ **Cache**: Usa `IMemoryCache` para otimização

**⚠️ CRÍTICO**: Sistema de movimentação patrimonial é complexo e requer validações rigorosas.

---

## Endpoints API Principais

### GET `/api/Patrimonio/Get`

**Descrição**: Lista patrimônios com filtros múltiplos

**Parâmetros**:
- `marca` (string) - Filtro por marca(s) separadas por vírgula
- `modelo` (string) - Filtro por modelo(s) separados por vírgula
- `setor` (string) - Filtro por setor(es) separados por vírgula
- `secao` (string) - Filtro por seção(ões) separadas por vírgula
- `situacao` (string) - Filtro por situação(ões) separadas por vírgula

**Response**: Lista de patrimônios ordenados por NPR

---

### GET `/api/Patrimonio/GetMovimentacao`

**Descrição**: Obtém detalhes de uma movimentação patrimonial

**Parâmetros**: `id` (Guid) - ID da movimentação

**Response**: Dados completos da movimentação com informações de origem e destino

---

### POST `/api/Patrimonio/CreateMovimentacao`

**Descrição**: **ENDPOINT CRÍTICO** - Cria movimentação patrimonial

**Request Body**: `MovimentacaoPatrimonioDto`

**Validações**:
- Patrimônio selecionado
- Data informada
- Setor destino informado
- Seção destino informada

**Prevenção de Duplicação**:
- Usa `HashSet<string>` com lock para evitar requisições simultâneas
- Chave única: `{PatrimonioId}_{DataMovimentacao}`

**Lógica**:
1. Valida dados
2. Busca patrimônio atual
3. Cria movimentação
4. Atualiza patrimônio (setor/seção destino)
5. Registra histórico

---

## Interconexões

### Quem Chama Este Controller

- **Pages**: Páginas de gestão patrimonial
- **Pages**: Páginas de movimentação patrimonial

### O Que Este Controller Chama

- **`_unitOfWork.Patrimonio`**: CRUD de patrimônio
- **`_unitOfWork.MovimentacaoPatrimonio`**: CRUD de movimentações
- **`_unitOfWork.SetorPatrimonial`**: Validação de setores
- **`_unitOfWork.SecaoPatrimonial`**: Validação de seções
- **`_unitOfWork.ViewPatrimonioConferencia`**: View otimizada
- **`_cache`**: Cache de memória

---

## Notas Importantes

1. **Prevenção de Duplicação**: Sistema robusto com lock para evitar movimentações duplicadas
2. **Filtros Múltiplos**: Suporta múltiplos valores separados por vírgula
3. **Movimentação**: Atualiza automaticamente setor/seção do patrimônio

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação da documentação completa do PatrimonioController

**Arquivos Afetados**:
- `Controllers/PatrimonioController.cs`

**Impacto**: Documentação de referência para operações patrimoniais

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
