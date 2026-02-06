# Guia de Operação: Módulo de Viagens e Logística

O Módulo de Viagens é o coração pulsante do FrotiX. Ele orquestra o deslocamento dos ativos da frota, garantindo que cada quilômetro rodado seja registrado, validado e precificado.

## 🏁 O Fluxo de Vida de uma Viagem
A viagem no FrotiX percorre três estados principais:
1.  **Agendamento (Agenda):** Planejamento futuro via calendário interativo Syncfusion. Aqui o foco é a prevenção de conflitos (mesmo veículo/motorista em dois lugares).
2.  **Execução:** Período em que o veículo está em trânsito.
3.  **Finalização e Conferência:** Momento crítico onde o motorista ou operador insere os KMs finais e o sistema aciona a **IA de Conferência** para validar se os dados informados são fisicamente coerentes com o trajeto.

## 🧠 Inteligências Embarcadas

### 1. Motor de Agendamento Inteligente (Pages/Agenda)
O calendário utiliza uma integração profunda com o banco de dados para realizar "Check de Conflito" em tempo real. Se você tenta reservar um veículo que já possui uma manutenção agendada ou outra viagem confirmada, o sistema bloqueia a ação imediatamente.

### 2. IA de Validação de Trajeto
Ao finalizar uma viagem, o FrotiX analisa o tempo decorrido vs. quilometragem. Se os dados divergirem significativamente da média histórica do trajeto, a viagem é marcada para revisão manual no dashboard de inconsistências.

### 3. Ficha de Vistoria Digital
Integrada ao formulário de cadastro (Upsert.cshtml), a ficha de vistoria permite anexar fotos e descrições do estado do veículo antes e após o uso, gerando um histórico inquestionável de zelo pelo patrimônio.

## 📊 Dashboards e Monitoramento
- **Dashboard de Viagens:** Visão consolidada de KMs rodados, viagens por setor e produtividade dos motoristas.
- **Módulo TaxiLeg:** Integração para importação de faturas de transporte terceirizado, garantindo que o custo total da mobilidade (frotas próprias + terceiros) esteja em uma única tela.

## 🛠 Detalhes Técnicos para Desenvolvedores
- **Persistência de Imagens:** As vistorias são salvas como yte[] no SQL Server para garantir portabilidade de backups.
- **Performance de Grid:** As listagens utilizam o componente de Grid da Syncfusion com paginação agressiva via servidor (UrlAdaptor) para suportar bases com milhões de registros sem lentidão.


## 📂 Arquivos do Módulo (Listagem Completa)

### 📅 Agenda
- Pages/Agenda/Index.cshtml & .cs: O calendário mestre Syncfusion para gestão de datas e conflitos.

### 🚗 Viagens e Deslocamentos
- Pages/Viagens/Index.cshtml & .cs: Central de monitoramento e pesquisa de deslocamentos.
- Pages/Viagens/Upsert.cshtml & .cs: Formulário inteligente de entrada, edição e vistoria de veículos.
- Pages/Viagens/DashboardViagens.cshtml & .cs: Painel analítico de performance e custos de rodagem.
- Pages/Viagens/DashboardEventos.cshtml & .cs: Gestão de intercorrências durante o trânsito (paradas, quebras).
- Pages/Viagens/ListaEventos.cshtml & .cs: Listagem técnica de eventos para auditoria.
- Pages/Viagens/UpsertEvento.cshtml & .cs: Registro detalhado de ocorrências na estrada.
- Pages/Viagens/FluxoPassageiros.cshtml & .cs: Gestão de lotação e roteirização de ocupantes.
- Pages/Viagens/GestaoFluxo.cshtml & .cs: Painel tático para despacho de veículos baseado na demanda de passageiros.
- Pages/Viagens/ExportarParaPDF.cshtml & .cs: Motor de geração de vouchers e relatórios de viagem em PDF.
- Pages/Viagens/TaxiLeg.cshtml & .cs: Interface de conciliação para faturas de transporte terceirizado.
- Pages/Viagens/TestGrid.cshtml & .cs: Laboratório de performance para grids complexos.
- Pages/Viagens/_SecaoOcorrenciasFinalizacao.cshtml: Componente parcial para registro de danos na entrega.

### 🚕 TaxiLeg (Integração Terceirizada)
- Pages/TaxiLeg/Importacao.cshtml & .cs: Motor de processamento de planilhas de faturamento externo.
- Pages/TaxiLeg/Canceladas.cshtml & .cs: Gestão e auditoria de corridas faturadas porém não realizadas.
- Pages/TaxiLeg/PBITaxiLeg.cshtml & .cs: Dashboard de Business Intelligence dedicado aos custos do contrato de táxi.


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
