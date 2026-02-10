# Guia de Suprimentos: Abastecimento e Consumo

O módulo de suprimentos é focado em transformar notas fiscais e cupons de posto em indicadores de eficiência energética e controle financeiro.

## ⛽ Ciclo de Abastecimento (Pages/Abastecimento)
O abastecimento pode entrar no sistema de três formas:
1.  **Lançamento Manual:** O operador insere os dados do cupom diretamente na tela.
2.  **Importação Automática:** Processamento de planilhas de frotistas e postos via NPOI. O motor de importação detecta automaticamente duplicidades e normaliza nomes de combustíveis.
3.  **App FrotiX:** (Integração futura/API) Cadastro direto da ponta.

## 📉 Cálculo de Eficiência
A cada novo abastecimento, o sistema recalcula automaticamente:
- **Média (KM/L):** Comparando o KM atual com o abastecimento anterior.
- **Custo por KM:** Cruzando o valor total pago com o deslocamento realizado.
- **Inconsistências:** Alertas de "tanque maior que a capacidade" ou "quilometragem retroativa".

## 🛠 Detalhes Técnicos
- **Importação Resiliente:** O AbastecimentoImportController processa o Excel em lotes. Erros em linhas específicas são reportados ao usuário sem abortar o processamento das linhas válidas.
- **SignalR integration:** Durante importações de arquivos grandes (10.000+ linhas), o progresso é enviado em tempo real para a barra de loading do usuário via WebSockets.


## 📂 Arquivos do Módulo (Listagem Completa)

### ⛽ Gestão de Abastecimentos
- Pages/Abastecimento/Index.cshtml & .cs: Central de auditoria e listagem de todos os cupons registrados.
- Pages/Abastecimento/Importacao.cshtml & .cs: Motor de integração de planilhas de combustíveis (Postos/Ticket Log).
- Pages/Abastecimento/Pendencias.cshtml & .cs: Filtro inteligente de abastecimentos que aguardam validação ou correção.
- Pages/Abastecimento/DashboardAbastecimento.cshtml & .cs: Painel executivo de consumo, médias e gastos totais.
- Pages/Abastecimento/RegistraCupons.cshtml & .cs: Formulário de entrada simplificado para grandes volumes de redigitação.
- Pages/Abastecimento/UpsertCupons.cshtml & .cs: Gestão de itens de cupons fiscais e detalhamento de litros.
- Pages/Abastecimento/PBI.cshtml & .cs: Interface de embed para relatórios de Business Intelligence externos.

### 🔥 Gestão de Combustíveis
- Pages/Combustivel/Index.cshtml & .cs: Cadastro de tipos de combustíveis (Gasolina, Diesel S10, GNV).
- Pages/Combustivel/Upsert.cshtml & .cs: Definição de densidade, padrão e precificação base.
