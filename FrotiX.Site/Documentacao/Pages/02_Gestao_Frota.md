# Guia de Ativos: Veículos, Motoristas e Unidades

Este módulo gerencia os protagonistas da operação: os veículos e as pessoas que os conduzem. É o repositório central de cadastros fundamentais do FrotiX.

## 🚗 Gestão de Veículos (Pages/Veiculo)
O cadastro de veículos vai muito além de placa e chassi. Nele controlamos:
- **Natureza jurídica:** Próprio, Locado ou Cedido.
- **Documentação Digital:** Sistema de upload e visualização de CRLV com alertas de vencimento.
- **Vida Útil:** Registro de KM atual que serve de base para todo o cálculo de consumo e alertas de manutenção preventiva.

## 👨‍✈️ Gestão de Motoristas (Pages/Motorista)
O foco aqui é a segurança e conformidade.
- **Controle de CNH:** Upload da imagem da carteira e acompanhamento automático de validade e pontuação.
- **Lotação Dinâmica:** Capacidade de associar motoristas a unidades específicas ou setores administrativos.

## 🏢 Unidades e Lotação (Pages/Unidade)
As unidades representam a estrutura organizacional (Secretarias, Departamentos, Filiais).
- **Lotação de Ativos:** Define qual unidade é "dona" de qual veículo.
- **Hierarquia:** Permite que gestores de uma unidade vejam apenas os seus ativos, enquanto a administração central tem visão global (Multitenancy básico).

## 🛠 Detalhes Técnicos
- **Views Otimizadas:** Para listagens e filtros, o sistema utiliza as ViewVeiculos e ViewMotoristas, que já trazem os nomes das unidades e marcas resolvidos via SQL, evitando o problema de consulta N+1.
- **Upload de Fotos:** As fotos dos motoristas são redimensionadas no cliente para salvar espaço e processadas via ImageHelper.cs no servidor.


## 📂 Arquivos do Módulo (Listagem Completa)

### 🚗 Gestão de Veículos (Frota)
- Pages/Veiculo/Index.cshtml & .cs: Listagem central da frota com filtros dinâmicos de status.
- Pages/Veiculo/Upsert.cshtml & .cs: Administração completa de veículos (Próprio/Locado).
- Pages/Veiculo/DashboardVeiculos.cshtml & .cs: Painel tático de disponibilidade e idade da frota.
- Pages/Veiculo/UploadCRLV.cshtml & .cs: Interface especializada para digitalização de documentos veiculares.
- Pages/MarcaVeiculo/Index.cshtml & .cs / Upsert.cshtml & .cs: Cadastro de fabricantes (Chevrolet, Volkswagen, etc).
- Pages/ModeloVeiculo/Index.cshtml & .cs / Upsert.cshtml & .cs: Cadastro técnico de modelos e especificações.

### 👨‍✈️ Gestão de Motoristas
- Pages/Motorista/Index.cshtml & .cs: Quadro geral de condutores e status de CNH.
- Pages/Motorista/Upsert.cshtml & .cs: Formulário de cadastro de motoristas, lotação e contatos.
- Pages/Motorista/DashboardMotoristas.cshtml & .cs: Analytics de produtividade e segurança do condutor.
- Pages/Motorista/UploadCNH.cshtml & .cs: Ferramenta para captura e armazenamento de documentos de habilitação.
- Pages/Motorista/PBILotacaoMotorista.cshtml & .cs: Visão gerencial de distribuição de motoristas por unidade.

### 🏢 Unidades Administrativas
- Pages/Unidade/Index.cshtml & .cs: Gestão da árvore organizacional (Secretarias/Setores).
- Pages/Unidade/Upsert.cshtml & .cs: Cadastro de novas unidades e parametrização de emails.
- Pages/Unidade/LotacaoMotoristas.cshtml & .cs: Ferramenta de drag-and-drop para mover motoristas entre unidades.
- Pages/Unidade/VeiculosUnidade.cshtml & .cs: Relacionamento mestre-detalhe de ativos por departamento.
- Pages/Unidade/VisualizaLotacoes.cshtml & .cs: Mapa visual da ocupação e distribuição de recursos.
