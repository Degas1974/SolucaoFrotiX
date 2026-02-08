# Documentação: DashboardViagensController.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O `DashboardViagensController` fornece endpoints para dashboards de viagens, incluindo estatísticas gerais, análises por período, filtros de outliers e comparações com períodos anteriores.

**Principais características:**

✅ **Estatísticas Gerais**: Totais, custos, KM, médias  
✅ **Filtro de Outliers**: Filtra viagens com KM > 2000 (erros)  
✅ **Comparação Períodos**: Compara período atual com anterior  
✅ **Análises**: Por dia da semana, status, veículo, motorista, etc.  
✅ **Exportação PDF**: Geração de relatórios em PDF

**Nota**: Controller implementado como partial class dividido em múltiplos arquivos.

---

## Endpoints API Principais

### GET `/api/DashboardViagens/ObterEstatisticasGerais`

**Descrição**: **ENDPOINT PRINCIPAL** - Estatísticas gerais de viagens

**Parâmetros**: 
- `dataInicio` (DateTime opcional) - Padrão: 30 dias atrás
- `dataFim` (DateTime opcional) - Padrão: hoje

**Filtros de Outliers**:
- KM máximo por viagem: 2000 km
- Filtra viagens com `KmFinal < KmInicial`
- Filtra viagens com diferença > 2000 km

**Response**: Estatísticas completas incluindo comparação com período anterior

---

## Interconexões

### Quem Chama Este Controller

- **Pages**: `Pages/Dashboard/Viagens.cshtml`
- **JavaScript**: Dashboards e gráficos

### O Que Este Controller Chama

- **`_context.Viagem`**: Consultas diretas ao DbContext
- **`_userManager`**: Informações de usuários

---

## Notas Importantes

1. **Filtro de Outliers**: Constante `KM_MAXIMO_POR_VIAGEM = 2000`
2. **Comparação**: Calcula período anterior automaticamente
3. **Custos**: Calculados apenas para viagens "Realizada"

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação da documentação completa do DashboardViagensController

**Arquivos Afetados**:
- `Controllers/DashboardViagensController.cs`
- `Controllers/DashboardViagensController_ExportacaoPDF.cs`

**Impacto**: Documentação de referência para dashboards de viagens

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
