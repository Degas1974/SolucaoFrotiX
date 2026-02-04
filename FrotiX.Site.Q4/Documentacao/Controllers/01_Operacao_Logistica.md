# Guia de Engenharia: Controllers de Operação e Logística

Este grupo coordena a entrada e saída de dados das viagens, agendamentos e integrações de transporte.

## 🎛 Controladores Principais
- **ViagemController**: O cérebro da operação. Gerencia o CRUD de viagens, o motor de cálculo de custos em lote e a integração com vistorias.
- **AgendaController**: Orquestra o calendário. Possui lógica específica para validação de sobreposição de horários e gestão de recorrências (semanal/mensal).
- **TaxiLegController**: Especializado em processar faturas de terceiros. Cruza dados de empresas parceiras com o orçamento da unidade solicitante.

## ⚡ Padronizações de Performance
Todos os controladores de operação utilizam o UrlAdaptor do Syncfusion, permitindo que filtros complexos de placa, motorista e data sejam resolvidos no SQL Server via ApplyFilters antes de retornar ao cliente.
