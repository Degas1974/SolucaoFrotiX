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
