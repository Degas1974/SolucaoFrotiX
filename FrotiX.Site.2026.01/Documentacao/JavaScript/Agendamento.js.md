# Agendamento de Viagens (JavaScript) - Motor Principal

O main.js dentro de wwwroot/js/agendamento/ é o coração funcional da agenda de frotas.

## O Que É?
Um script massivo que orquestra a integração entre o calendário Syncfusion, as validações de disponibilidade de veículos e o motor de IA de conferência.

## Por Que Existe?
Para transformar uma simples agenda em uma ferramenta de gestão de conflitos, garantindo que nenhum veículo seja reservado para dois lugares ao mesmo tempo e que as regras de negócio sejam respeitadas.

## Funcionalidades Chave

### 1. Motor de Validação
Antes de confirmar qualquer agendamento, o script realiza verificações assíncronas:
- **Disponibilidade:** Checa se o veículo/motorista já está em viagem no período selecionado.
- **IA Consolidada:** Se for um registro de viagem concluída, envia os dados para o alidarFinalizacaoConsolidadaIA, que analisa se a quilometragem e o tempo fazem sentido para o trajeto.

### 2. Sistema de Recorrência
Implementa lógica complexa para agendamentos semanais ou quinzenais. O JS gera a projeção de datas no cliente para exibição imediata antes de enviar para o banco de dados.

## Detalhes Técnicos (Desenvolvedor)
- **Syncfusion e JS Moderno:** Utiliza sync/await para coordenar múltiplas chamadas de API durante o salvamento.
- **Integridade:** Ao editar, o script bloqueia o botão de confirmação ($btn.prop("disabled", true)) para evitar duplicidade de registros (Race Conditions).


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
