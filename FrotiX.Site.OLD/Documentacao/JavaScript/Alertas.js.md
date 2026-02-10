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
