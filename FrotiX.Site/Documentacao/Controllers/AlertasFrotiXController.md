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
