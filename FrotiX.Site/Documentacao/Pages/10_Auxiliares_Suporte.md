# 🛠 Módulos Auxiliares, Suporte e Cadastros Base

> **Status**: ✅ **PROSA LEVE**  
> **Área**: Apoio Operacional e Cadastros Periféricos

---

## 📖 Visão Geral

Este guia consolida os módulos de apoio que alimentam os processos principais do FrotiX. São cadastros de parceiros (Fornecedores), personas (Requisitantes, Operadores) e utilitários de sistema.

---

## 🤝 Gestão de Parceiros e Personas

### 1. Fornecedores (`Pages/Fornecedor`)

**O que faz?** Cadastro de todas as empresas prestadoras de serviço (Locadoras, Postos, Oficinas).

- **Vínculo:** Essencial para o módulo Financeiro e de Abastecimento.

### 2. Requisitantes e Solicitantes (`Pages/Requisitante`, `Pages/SetorSolicitante`)

- **Requisitantes:** Cadastro de servidores ou funcionários que solicitam viagens.
- **Setor Solicitante:** Organiza as demandas por centros de custo (ex: Secretaria de Saúde, Educação).

### 3. Operadores, Encarregados e Lavadores

- **Encarregados (`Pages/Encarregado`):** Gestores locais responsáveis por liberar chaves ou vistorias.
- **Operadores (`Pages/Operador`):** Perfis técnicos que alimentam dados de campo.
- **Lavadores (`Pages/Lavador`):** Controle de execução de serviços de limpeza veicular.

---

## 🔧 Manutenção e Ocorrências

### 1. Manutenção (`Pages/Manutencao`)

**O que faz?** Além das glosas, gerencia a listagem de entradas em oficina e controles preventivos.

- **Controle de Lavagem:** Acompanhamento de frequência de limpeza para preservação do ativo.

### 2. Ocorrências (`Pages/Ocorrencia`)

- Central de registro de fatos atípicos (pequenos furtos, perdas de chaves, avarias estacionado).

---

## 📂 Arquivos do Módulo (Listagem Completa)

### 🤝 Cadastros de Apoio

- `Pages/Fornecedor/Index.cshtml` & `.cs` / `Upsert.cshtml` & `.cs`: Parceiros externos.
- `Pages/Requisitante/Index.cshtml` & `.cs` / `Upsert.cshtml` & `.cs`: Quem pede a viagem.
- `Pages/SetorSolicitante/Index.cshtml` & `.cs` / `Upsert.cshtml` & `.cs`: Origem da demanda.
- `Pages/Encarregado/Index.cshtml` & `.cs` / `Upsert.cshtml` & `.cs`: Gestão de chaves/Pátio.
- `Pages/Operador/Index.cshtml` & `.cs` / `Upsert.cshtml` & `.cs`: Perfis operacionais.
- `Pages/Lavador/Index.cshtml` & `.cs` / `Upsert.cshtml` & `.cs`: Cadastro de prestadores de limpeza.

### 🔧 Manutenção Operacional

- `Pages/Manutencao/ListaManutencao.cshtml` & `.cs`: Quadro geral de ordens de serviço.
- `Pages/Manutencao/Upsert.cshtml` & `.cs`: Registro de entrada/saída de oficina.
- `Pages/Manutencao/ControleLavagem.cshtml` & `.cs`: Lançamento de tickets de lavagem.
- `Pages/Ocorrencia/Ocorrencias.cshtml` & `.cs`: Gestão de eventos patrimoniais.

### 📜 Templates de Sistema e Layouts (`Pages/Page`)

_Arquivos base de design e templates padrão da administração._

- `Pages/Page/Index.cshtml` & `.cs`: Template de Dashboard Base.
- `Pages/Page/Login.cshtml` / `LoginAlt.cshtml` & `.cs`: Telas de autenticação.
- `Pages/Page/Register.cshtml` & `.cs`: Tela de registro de novo usuário.
- `Pages/Page/Profile.cshtml` & `.cs`: Gestão de perfil pessoal do usuário.
- `Pages/Page/Chat.cshtml` & `.cs`: Template de interface de chat interno.
- `Pages/Page/InboxGeneral.cshtml` / `InboxRead.cshtml` / `InboxWrite.cshtml`: Sistema de correio interno.
- `Pages/Page/Contacts.cshtml` & `.cs`: Agenda de contatos do sistema.
- `Pages/Page/Projects.cshtml` & `.cs`: Visualizador de tarefas e projetos.
- `Pages/Page/Search.cshtml` & `.cs`: Template de resultados de busca global.
- `Pages/Page/Invoice.cshtml` & `.cs`: Template de fatura/recibo visual.
- `Pages/Page/Forget.cshtml` & `.cs`: Recuperação de senha.
- `Pages/Page/Locked.cshtml` & `.cs`: Tela de bloqueio por inatividade.
- `Pages/Page/Confirmation.cshtml` & `.cs`: Página genérica de sucesso/confirmação.
- `Pages/Page/Error.cshtml` & `.cs`: Página de erro 404/500 amigável.
- `Pages/Page/ForumList.cshtml` / `ForumThreads.cshtml` / `ForumDiscussion.cshtml`: Templates de colaboração.

### 🧪 Utilitários e Temporários

- `Pages/Uploads/UploadPDF.cshtml` & `.cs`: HUB genérico de upload de arquivos.
- `Pages/Uploads/UpsertAutuacao.cshtml` & `.cs`: Shorthand para carga de multas via upload.
- `Pages/PlacaBronze/Index.cshtml` & `.cs` / `Upsert.cshtml` & `.cs`: Gestão de placas comemorativas/identificação física.
- `Pages/Relatorio/TesteRelatorio.cshtml` & `.cs`: Sandbox para validação de templates Stimulsoft.
- `Pages/Temp/Index.cshtml` & `.cs`: Área de rascunho para novas funcionalidades.
- `Pages/_ViewImports.cshtml`: Definições globais de namespaces e TagHelpers.
- `Pages/_ViewStart.cshtml`: Definição de layout padrão para todas as páginas.
