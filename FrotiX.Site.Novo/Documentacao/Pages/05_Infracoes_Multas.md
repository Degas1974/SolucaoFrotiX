# Guia de Infrações: Gestão de Multas e Penalidades

Este módulo é dedicado ao acompanhamento de infrações de trânsito, desde a autuação inicial até o pagamento e identificação do condutor.

## 📑 Ciclo da Infração (Pages/Multa)
1.  **Autuação:** Registro inicial da infração. O sistema permite o upload do PDF da notificação.
2.  **Identificação:** Vinculação automática do motorista que estava em posse do veículo no dia e hora exata da infração (cruzamento com o módulo de Viagens).
3.  **Penalidade:** Transformação da autuação em multa real com código de barras e valor.

## 🔍 Visualização e Eficiência
- **PDF Viewer Integrado:** O FrotiX possui um componente de visualização de PDF que permite ao gestor ler a notificação e o comprovante de pagamento sem baixar o arquivo.
- **Órgãos Autuantes:** Cadastro centralizado de prefeituras, Detran e órgãos federais para padronização de destinos de pagamento.

## 🛠 Detalhes Técnicos
- **Cross-Reference:** A lógica de VincularViagemId busca na tabela de Viagens quem era o motorista logado no momento da infração, reduzindo o trabalho manual do setor jurídico.
- **Gestão de Prazos:** Alertas SignalR avisam os gestores sobre multas próximas ao vencimento do desconto de 20%/40%.


## 📂 Arquivos do Módulo (Listagem Completa)

### 📑 Gestão de Multas (Core)
- Pages/Multa/ListaAutuacao.cshtml & .cs: Central de gestão de notificações de infrações.
- Pages/Multa/UpsertAutuacao.cshtml & .cs: Registro detalhado e identificação automático do condutor.
- Pages/Multa/ListaPenalidade.cshtml & .cs: Controle de multas impostas e faturadas.
- Pages/Multa/UpsertPenalidade.cshtml & .cs: Detalhamento de valores, descontos e vencimentos.
- Pages/Multa/PreencheListas.cshtml & .cs: Utilitário para carga de dados rápidos e correções em massa.

### 📄 Documentos e PDFs
- Pages/Multa/UploadPDF.cshtml & .cs: Lógica de processamento e armazenamento de anexos fiscais.
- Pages/Multa/ExibePDFAutuacao.cshtml & .cs: Visualizador de notificação.
- Pages/Multa/ExibePDFPenalidade.cshtml & .cs: Visualizador de multa.
- Pages/Multa/ExibePDFComprovante.cshtml & .cs: Visualizador de pagamento.

### ⚙️ Parametrização e Suporte
- Pages/Multa/ListaTiposMulta.cshtml & .cs / UpsertTipoMulta.cshtml & .cs: Cadastro de códigos de infração (CTB).
- Pages/Multa/ListaOrgaosAutuantes.cshtml & .cs / UpsertOrgaoAutuante.cshtml & .cs: Cadastro de órgãos emissores (Detran, PRF).
- Pages/Multa/ListaEmpenhosMulta.cshtml & .cs / UpsertEmpenhosMulta.cshtml & .cs: Vínculo financeiro para quitação de multas de frota própria.
