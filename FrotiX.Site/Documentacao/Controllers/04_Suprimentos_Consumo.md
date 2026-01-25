# Guia de Engenharia: Controllers de Suprimentos e Consumo

Gerencia o fluxo de energia da frota.

## 🎛 Controladores Principais
- **AbastecimentoImportController**: Motor de importação massiva. Lida com arquivos Excel pesados e utiliza SignalR para feedback de progresso.
- **CombustivelController**: Cadastro de preços e postos credenciados.
- **AbastecimentoController.DashboardAPI**: Fornece os pontos de dados para os gráficos de consumo e custo/km.
