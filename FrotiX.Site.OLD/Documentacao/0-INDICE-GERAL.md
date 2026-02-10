# 📚 Índice Geral da Documentação - FrotiX Web

> **Última Atualização**: 08/01/2026  
> **Versão**: 1.0  
> **Total de Arquivos Documentados**: Em progresso

---

## 📋 Sumário

- [Páginas Razor (Pages)](#-páginas-razor-pages)
- [Controllers](#-controllers)
- [Services](#-services)
- [Helpers](#-helpers)
- [Middlewares](#-middlewares)
- [Models](#-models)
- [Repository/IRepository](#-repositoryirepository)
- [Data](#-data)
- [JavaScript](#-javascript)
- [CSS](#-css)

---

## 📄 Páginas Razor (Pages)

A interface do FrotiX é organizada em módulos funcionais que agrupam diversas páginas Razor para entregar uma experiência de gestão completa.

### 🏛️ Módulos de Gestão
- [x] [Operação: Viagens e Logística](Pages/01_Operacao_Viagens.md) ✅ (Agenda, Viagens, TaxiLeg)
- [x] [Ativos: Frota e Motoristas](Pages/02_Gestao_Frota.md) ✅ (Veículos, Motoristas, Unidades, Marcas/Modelos)
- [x] [Suprimentos: Consumo e Abastecimento](Pages/03_Suprimentos_Consumo.md) ✅ (Abastecimento, Combustível)
- [x] [Financeiro: Contratos e Glosas](Pages/04_Financeiro_Contratos.md) ✅ (Contratos, Atas, Empenhos, Notas Fiscais)
- [x] [Conformidade: Infrações e Multas](Pages/05_Infracoes_Multas.md) ✅ (Multas, Autuações, PDFs)
- [x] [Patrimônio: Ativos e Inventário](Pages/06_Patrimonio_Ativos.md) ✅ (Patrimônio, Movimentações, Setores)
- [x] [Governança: Administração e Segurança](Pages/07_Administracao_Governanca.md) ✅ (Usuários, Alertas, WhatsApp, Logs)
- [x] [Escalonamento: Gestão de Escalas e Turnos](Pages/11_Gestao_Escalas.md) ✅ (Escala Diária, Turnos, Folgas)
- [x] [Apoio: Auxiliares e Cadastros Base](Pages/10_Auxiliares_Suporte.md) ✅ (Fornecedores, Requisitantes, Manutenção, Templates)

### 🧩 Componentes e Relatórios
- [x] [Componentes Compartilhados (Layout/Shell)](Pages/08_Componentes_Shared.md) ✅ (Layouts, Toasts, Sino, Scripts)
- [x] [Dashboards e Monitoramento Intel](Pages/09_Dashboards_Intel.md) ✅ (KPIs, Analytics, BI)## 🎮 Controllers (Regras de Negócio e APIs)

Os Controllers do FrotiX atuam como orquestradores entre a interface Razor e os Repositórios de Dados, expondo endpoints para DataTables, Syncfusion e integrações mobile.

### 🏛️ Grupos de Controladores
- [x] [Operação e Logística](Controllers/01_Operacao_Logistica.md) ✅ **PROSA LEVE** (Viagens, Agenda, TaxiLeg)
- [x] [Cadastro e Ativos](Controllers/02_Cadastros_Ativos.md) ✅ **PROSA LEVE** (Veiculos, Motoristas, Unidades)
- [x] [Financeiro e Auditoria](Controllers/03_Financeiro_Auditoria.md) ✅ **PROSA LEVE** (Contratos, Glosas, Notas Fiscais)
- [x] [Suprimentos e Consumo](Controllers/04_Suprimentos_Consumo.md) ✅ **PROSA LEVE** (Abastecimento, Combustível)
- [x] [Infraestrutura e Logs](Controllers/05_Infra_Logs.md) ✅ **PROSA LEVE** (LogErros, Alertas, WhatsApp)

### 🧩 APIs e Viewers
- [ ] Controllers/PdfViewer/ -> Renderização de documentos em tela.
- [ ] Controllers/Api/ -> Endpoints puros para consumo externo.

## 🔧 Services (Camada de Negócio e Background)

- [x] [Painel Geral de Serviços](Services/00_Modulo_Servicos.md) ✅ **PROSA LEVE**

## 🛠️ Helpers e Utilidades

- [x] [Guia de Helpers UI/Backend](Helpers/00_Modulo_Helpers.md) ✅ **PROSA LEVE** (Alertas, Imagens, SFDT)

## 📦 Repository e Unit of Work

- [x] [Padrões de Acesso a Dados](Repository/01_UnitOfWork.md) ✅ **PROSA LEVE**

## 📂 Data (Entity Framework & DB Context)

- [x] [Estrutura de Contextos de Dados](Data/00_Modulo_Data.md) ✅ **PROSA LEVE**

## 📜 Arquitetura JavaScript

As lógicas de front-end do FrotiX são organizadas em guias de inteligência funcional, separando utilitários globais de regras de negócio específicas.

### 🏛️ Guias de Inteligência
- [x] [Guia: Cadastros e Formulários](JavaScript/Cadastros.js.md) ✅ **PROSA LEVE**
- [x] [Guia: Dashboards e Visualização](JavaScript/Dashboards.js.md) ✅ **PROSA LEVE**
- [x] [Guia: Alertas e Mensageria (SignalR)](JavaScript/Alertas.js.md) ✅ **PROSA LEVE**
- [x] [Guia: Motor de Agendamento](JavaScript/Agendamento.js.md) ✅ **PROSA LEVE**

### 🔧 Utilitários e Core
- [x] [Alerta e Interops (SweetAlert)](JavaScript/alerta.js.md) ✅
- [x] [Configurações Globais FrotiX](JavaScript/frotix.js.md) ✅
- [x] [Sistema de Toasts](JavaScript/global-toast.js.md) ✅
- [x] [Utilitários Syncfusion](JavaScript/syncfusion.utils.md) ✅

### 🧪 Scripts Legados e Higienização
- [x] [Higienização de Viagens](JavaScript/higienizarviagens_054.js.md) ✅

---

## 🎨 CSS

- [x] `wwwroot/css/frotix.css` → `CSS/frotix.css.md` ✅ **COMPLETO** (Padrão FrotiX Simplificado)

---

## 📊 Estatísticas

| Categoria       | Estrutura Documentada | Status          | Progresso |
| --------------- | --------------------- | --------------- | --------- |
| **Pages**       | 9 Módulos Funcionais  | ✅ PROSA LEVE    | 100%      |
| **Controllers** | 5 Grupos de Negócio   | ✅ PROSA LEVE    | 100%      |
| **Services**    | Painel de Serviços    | ✅ PROSA LEVE    | 100%      |
| **Helpers**     | Guia de Utilidades    | ✅ PROSA LEVE    | 100%      |
| **Repository**  | Unit of Work / Repos  | ✅ PROSA LEVE    | 100%      |
| **Data**        | Contextos de Dados    | ✅ PROSA LEVE    | 100%      |
| **JavaScript**  | 4 Guias + Core        | ✅ PROSA LEVE    | 100%      |
| **CSS**         | Estilo Global         | ✅ COMPLETO      | 100%      |
| **TOTAL**       | **Arquitetura Web**   | **PRONTO**      | **100%**  | **~714**          | **9**        | **~705**  | **1.3%**  |

---

## 🔄 Como Atualizar Este Índice

1. Quando criar nova documentação, marque com `[x]` e adicione ✅
2. Quando revisar documentação existente, mantenha `[x]` mas adicione nota de revisão
3. Sempre atualize a data de "Última Atualização" no topo
4. Atualize as estatísticas na seção final

---

**Última atualização**: 08/01/2026  
**Mantido por**: Sistema de Documentação FrotiX

---

## 📌 Notas Importantes

1. **Padrão FrotiX Simplificado**: Todas as documentações seguem formato didático com:

   - Objetivos claros no início
   - Arquivos listados com Problema/Solução/Código
   - Fluxos explicados passo a passo
   - Troubleshooting simplificado

2. **Comentários Visuais**: Todos os arquivos fonte documentados possuem comentários visuais no topo indicando onde está a documentação.

3. **Estrutura de Diretórios**: A documentação está organizada em subdiretórios por tipo de arquivo (Pages, Controllers, Services, etc.).

4. **Padrão de Qualidade**: Documentações devem ser extremamente bem explicadas para leigos em TI, com exemplos generosos e interconexões documentadas.








