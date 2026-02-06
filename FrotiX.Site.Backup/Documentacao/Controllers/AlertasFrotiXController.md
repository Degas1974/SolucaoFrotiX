# Documentação: Sistema de Notificações e Alertas em Tempo Real (AlertasFrotiXController)

O \AlertasFrotiXController\ é o centro de comunicações do FrotiX. Mais do que um simples sistema de mensagens, ele integra um barramento de eventos em tempo real (**SignalR**) com um rastreamento rigoroso de leitura, garantindo que informações críticas (avisos de manutenção, viagens agendadas ou anúncios da empresa) cheguem aos destinatários corretos.

## 1. O Pipeline de Notificação (SignalR)

Toda vez que um alerta é criado ou alterado, o controller não apenas salva os dados no banco, mas também "acorda" a interface do usuário através do \AlertasHub\. Isso permite que o badge de notificações no menu lateral pulse instantaneamente para o usuário, sem que ele precise recarregar a página.

\\\csharp
// Exemplo de como o sistema busca detalhes para a tela de monitoramento
var alerta = await _unitOfWork.AlertasFrotiX.GetFirstOrDefaultAsync(
    a => a.AlertasFrotiXId == id,
    includeProperties: "AlertasUsuarios,Viagem,Manutencao,Veiculo,Motorista"
);
\\\

## 2. Rastreamento e Auditoria (Quem Leu?)

Uma das funcionalidades mais poderosas deste controller é o endpoint \GetDetalhesAlerta\. Ele fornece ao administrador uma visão de 360 graus sobre a eficácia de uma comunicação:
- **Total de Destinatários:** Quantas pessoas deveriam receber.
- **Percentual de Leitura:** Quantos clicaram e leram de fato.
- **Tempo "No Ar":** Cálculo automático de quanto tempo a mensagem ficou ativa antes de expirar ou ser lida.

O sistema diferencia entre **Notificado** (o alerta apareceu no browser), **Lido** (o usuário clicou para detalhes) e **Apagado** (o usuário removeu da sua lista pessoal).

## 3. Identidade Visual e Semântica

O controller automatiza a atribuição de ícones e cores (**FontAwesome Duotone**) baseada no tipo de alerta (\TipoAlerta\) e prioridade. Isso garante consistência visual em todo o sistema, onde uma manutenção sempre será representada por uma chave de fenda laranja (\a-wrench\) e um anúncio crítico por um megafone vermelho (\a-bullhorn\).

## 4. Segurança e Privacidade

O sistema utiliza o \ClaimTypes.NameIdentifier\ para filtrar rigorosamente o que cada usuário pode ver. Um alerta destinado a um motorista específico nunca será vazado para outro, mantendo a integridade dos dados e a privacidade da operação.

---

### Notas de Implementação (Padrão FrotiX)

*   **IgnoreAntiforgeryToken:** Utilizado em alguns endpoints de API para permitir integrações rápidas, mantendo a segurança via tokens de autenticação bearer ou cookies de sessão.
*   **Tratamento de Erros:** Todas as falhas são capturadas e enviadas para o \Alerta.TratamentoErroComLinha\, evitando que o Hub de SignalR trave em caso de inconsistência no banco.
*   **DTOs Otimizados:** O controller utiliza classes de transferência específicas (\AlertaExportDto\) para evitar o envio de objetos de modelo pesados para o frontend.

---
*Documentação atualizada em 2026.01.14 conforme novo padrão de Prosa Leve.*


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
