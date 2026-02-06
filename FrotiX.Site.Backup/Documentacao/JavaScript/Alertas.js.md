# Alertas FrotiX (JavaScript) - Real-time e Notificações

Responsável pela camada de interação do sistema de alertas e notificações em tempo real.

## O Que É?
Localizado em wwwroot/js/alertasfrotix/, este módulo gerencia o "Sino" de notificações na Navbar e a tela de gestão de alertas pendentes.

## Por Que Existe?
Para garantir que eventos críticos (vencimento de CNH, manutenção atrasada, infrações) sejam comunicados ao gestor instantaneamente, sem necessidade de refresh.

## Como Funciona?

### 1. SignalR Integration
Os scripts estabelecem uma conexão com o Hub de Alertas. Quando o servidor processa um novo alerta, ele dispara uma mensagem para o cliente, que invoca carregarAlertasAtivos() para atualizar o contador visual no sino.

### 2. Gestão de Baixas
Em lertas_gestao.js, o processo de dar baixa em um alerta utiliza Alerta.Confirmar. Após a confirmação, o registro é movido para a aba de "Lidos" via atualização de DataTables sem recarga de página.

## Detalhes Técnicos (Desenvolvedor)
- **DataTables:** Utiliza a versão JS para filtragem rápida entre alertas "Meus", "Do Setor" e "Lidos".
- **Debounce de Som:** Lógica de notificação sonora para evitar disparos múltiplos e irritantes em sucessão rápida.
- **Fallback:** Se o SignalR falhar, o script possui um iniciarRecarregamentoAutomatico() que faz polling a cada 5 minutos como contingência.


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
