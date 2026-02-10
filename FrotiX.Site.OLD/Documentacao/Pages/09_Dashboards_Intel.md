# 📊 Dashboards e Inteligência de Dados (Intel)

> **Status**: ✅ **PROSA LEVE**  
> **Área**: Business Intelligence e KPIs  
> **Padrão**: Chart.js + Syncfusion Grid + Web API

---

## 📖 Visão Geral

O módulo **Intel** é a torre de comando do FrotiX. Aqui, os dados brutos de viagens, abastecimentos e custos são transformados em gráficos e indicadores de performance (KPIs) para auxiliar na tomada de decisão gerencial.

---

## 🚀 Principais Dashboards

### 1. `PaginaPrincipal.cshtml` (Dashboard Executivo)

**O que faz?** É a primeira tela que o gestor vê ao logar. Traz um resumo de 360 graus da operação.

- **Indicadores Chave (Cards):**
  - Total de Viagens Ativas.
  - Custo Total de Combustível (Mês Atual).
  - Alertas de Manutenção Pendentes.
- **Gráficos:** Evolução mensal de custos e ocupação da frota.

### 2. `AnalyticsDashboard.cshtml` (Deep Dive de Dados)

**O que faz?** Focado em análise estatística e tendências de longo prazo.

- **Uso:** Ideal para identificar comportamentos anômalos, como veículos com consumo muito acima da média ou motoristas com muitas infrações.
- **Tecnologia:** Utiliza integração pesada com `AbastecimentoController.DashboardAPI` para carregar dados filtráveis por período, unidade e tipo de veículo.

### 3. `MarketingDashboard.cshtml` (Fidelização e Uso)

**O que faz?** Embora o nome sugira marketing, no contexto FrotiX ele é usado para analisar a "adesão" ao sistema e a distribuição geográfica das operações.

- **Destaque:** Mapas de calor de rotas e frequência de uso por unidade administrativa.

---

## 🧠 Lógica de Funcionamento

### 1. Camada de API (Backend)

Os dashboards não acessam o banco de dados diretamente via Razor. Eles fazem chamadas AJAX para controllers dedicados (ex: `AbastecimentoImportController` ou métodos `Dashboard` nos controllers principais).

- **Vantagem:** A página carrega instantaneamente ("Skeleton loading") e os gráficos aparecem conforme os dados chegam.

### 2. Renderização de Gráficos (Frontend)

Utilizamos prioritariamente a biblioteca **Chart.js**.

- **Dinamicidade:** Ao clicar em uma legenda do gráfico, o gráfico se ajusta automaticamente para ocultar aquela série de dados.
- **Temas:** Os gráficos seguem a paleta de cores FrotiX (Laranja `#ff6b35` e Azul Petróleo).

### 3. Filtros Consolidados

Os dashboards possuem uma barra de filtros superior que permite segmentar toda a visão por:

- **Unidade de Negócio.**
- **Intervalo de Datas.**
- **Centro de Custo.**

---

## 📝 Notas para Desenvolvedores

1. **Performance é Prioridade:** Nunca faça consultas pesadas (LINQ) diretamente na Action do dashboard. Use os métodos do repositório que retornam objetos DTO simplificados.
2. **Responsividade:** Verifique sempre se os gráficos se ajustam corretamente em telas de notebook (1366x768) e monitores ultrawide.
3. **Novos Dashboards:** Devem seguir o padrão de estrutura de cards com ícones FontAwesome Duotone e sombras suaves (`shadow-sm`).


## 📂 Arquivos do Módulo (Listagem Completa)

### 📊 Dashboards de Inteligência (Core)
- Pages/Intel/PaginaPrincipal.cshtml & .cs: O dashboard 'Home' do sistema.
- Pages/Intel/AnalyticsDashboard.cshtml & .cs: Deep dive analítico de custos e frota.
- Pages/Intel/MarketingDashboard.cshtml & .cs: Gráficos de adesão e mapas de calor.
- Pages/Intel/Introduction.cshtml & .cs: Página de boas-vindas e tutorial de KPIs.
- Pages/Intel/Privacy.cshtml & .cs: Termos de uso e privacidade de dados analíticos.

### 🏛️ Dashboards de Outros Módulos (Consolidados aqui)
- Pages/Frota/DashboardEconomildo.cshtml & .cs: Dashboard focado em economia de escala.
- Pages/Manutencao/DashboardLavagem.cshtml & .cs: Indicadores de higiene e conservação.
- Pages/Manutencao/PBILavagem.cshtml & .cs: Relatório de PowerBI para custos de lavagem.
