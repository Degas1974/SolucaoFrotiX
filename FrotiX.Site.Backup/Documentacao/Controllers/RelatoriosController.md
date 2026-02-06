# Documentação: RelatoriosController.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O `RelatoriosController` gerencia exportação de PDFs do Dashboard Economildo, incluindo múltiplos tipos de relatórios (heatmaps, distribuições, comparativos).

**Principais características:**

✅ **Exportação PDF**: Geração de relatórios em PDF  
✅ **Múltiplos Tipos**: Heatmap viagens, heatmap passageiros, usuários por mês/turno, comparativos, etc.  
✅ **Filtros**: Por MOB, mês, ano  
✅ **Serviço PDF**: Usa `RelatorioEconomildoPdfService` para geração

---

## Endpoints API Principais

### GET `/api/Relatorios/ExportarEconomildo`

**Descrição**: Exporta relatório Economildo para PDF

**Parâmetros**:
- `tipo` (TipoRelatorioEconomildo) - Tipo de relatório
- `mob` (string opcional) - MOB do veículo
- `mes` (int opcional)
- `ano` (int opcional)

**Tipos Suportados**:
- `HeatmapViagens` - Heatmap de distribuição de viagens
- `HeatmapPassageiros` - Heatmap de distribuição de passageiros
- `UsuariosMes` - Usuários por mês
- `UsuariosTurno` - Usuários por turno
- `ComparativoMob` - Comparativo entre MOBs
- `UsuariosDiaSemana` - Usuários por dia da semana
- `DistribuicaoHorario` - Distribuição por horário
- `TopVeiculos` - Top veículos

**Response**: Arquivo PDF

---

## Interconexões

### Quem Chama Este Controller

- **Pages**: `Pages/Dashboard/Economildo.cshtml`
- **JavaScript**: Botões de exportação

### O Que Este Controller Chama

- **`_context.ViagensEconomildo`**: Dados de viagens Economildo
- **`_pdfService`**: Geração de PDFs
- **`_unitOfWork`**: Acesso a dados

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação da documentação completa do RelatoriosController

**Arquivos Afetados**:
- `Controllers/RelatoriosController.cs`

**Impacto**: Documentação de referência para exportação de relatórios

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
