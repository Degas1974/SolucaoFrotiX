# 🧩 Componentes Compartilhados (Shared)

> **Status**: ✅ **PROSA LEVE**  
> **Área**: Infraestrutura de Interface e Layout  
> **Padrão**: Bootstrap 5.3 + Syncfusion Overlays

---

## 📖 Visão Geral

A pasta `Shared` é o coração visual do FrotiX. Ela contém o layout mestre, os componentes de navegação, sistemas de notificação (Toasts) e scripts base que garantem que o sistema seja rápido, consistente e visualmente agradável.

---

## 🏗️ Estrutura de Layout e Casca (Shell)

### 1. `_Layout.cshtml` (O Molde Mestre)

**O que faz?** Define a estrutura HTML básica de todas as páginas. Ele carrega os fontes (FontAwesome Duotone), os estilos globais (`frotix.css`) e organiza a renderização dos componentes filhos.

- **Destaque:** Gerencia o carregamento assíncrono de scripts no rodapé para máxima performance.

### 2. Cabeçalho e Metadados (`_Head.cshtml`, `_Favicon.cshtml`)

- **\_Head:** Configura viewport, tags SEO e pré-carregamento de bibliotecas críticas (Syncfusion Core).
- **\_Favicon:** Garante que a identidade visual (logotipo FrotiX) apareça corretamente na aba do navegador.

### 3. Esqueleto da Página (`_PageHeader.cshtml`, `_PageFooter.cshtml`, `_PageBreadcrumb.cshtml`)

- **Header:** Mantém a barra superior fixa com acesso rápido ao perfil e notificações.
- **Footer:** Informações de copyright e versão do sistema.
- **Breadcrumb:** O "rastro de migalhas" que ajuda o usuário a saber exatamente onde está na hierarquia de módulos.

---

## 🧭 Navegação e Menus

### 1. Menu Lateral (`_LeftPanel.cshtml`, `_Menu.cshtml`)

**O que faz?** Renderiza a árvore de navegação carregada via `nav.json`.

- **Inteligência:** Suporta níveis de permissão. Se o usuário não tem acesso, o item do menu nem sequer é renderizado.

### 2. Atalhos e Utilidades (`_ShortcutMenu.cshtml`, `_DropdownMenu.cshtml`)

- **Botões Flutuantes:** Oferece acesso rápido a funções como "Novo Abastecimento" ou "Abrir Chamado" de qualquer lugar do sistema.

---

## 🔔 Feedback ao Usuário (Toasts e Alertas)

### 1. `_ToastPartial.cshtml`

**O que faz?** É o mecanismo de notificações flutuantes (tipo celular).

- **Uso:** Exibe "Salvo com sucesso!", "Erro na Importação" ou mensagens de boas-vindas. Ele é disparado via JavaScript (`global-toast.js`) ou via `TempData` do C#.

### 2. `_AlertasSino.cshtml` (Notificações em Tempo Real)

**O que faz?** O ícone de sino no topo da página.

- **Integração:** Conectado ao SignalR. Quando um novo alerta chega (ex: multa vencendo), o sino brilha e o contador aumenta sem precisar dar F5 na página.

---

## ⏳ Carregamento e Bloqueios (Spin Overlays)

### 1. `_PageContentOverlay.cshtml`

**O que faz?** Implementa o famoso "FrotiX Spin Overlay".

- **Por que usar?** Evita que o usuário clique duas vezes em um botão de "Salvar" ou tente interagir com a página enquanto os dados estão sendo processados.
- **Visual:** Fundo escuro com blur e o logo do FrotiX pulsando no centro.

### 2. `_ScriptsLoadingSaving.cshtml`

- **O que faz?** Otimiza o carregamento de scripts de validação de formulários (`jquery.validate`) para que só sejam carregados quando necessários.

---

## 🛠️ Scripts e Plugins Base

- **\_ScriptsBasePlugins.cshtml:** Garante que jQuery, Bootstrap e bibliotecas gráficas (Chart.js) estejam sempre disponíveis na ordem correta, evitando erros de "undefined".

---

## 📝 Notas para Desenvolvedores

1. **Evite Editar o \_Layout Diretamente:** Se precisar adicionar algo a apenas uma página, use a `@section Scripts { ... }`.
2. **Novos Toasts:** Utilize sempre o helper `Alerta.Sucesso()` que disparará a lógica contida nestes componentes compartilhados.
3. **Consistência:** Mantenha os nomes de classes `ftx-` para garantir que o estilo visual não quebre com atualizações do Bootstrap.


## 📂 Arquivos de Infraestrutura (Listagem Completa)

### 🏗️ Layout e Frameset
- Pages/Shared/_Layout.cshtml: O template mestre do sistema.
- Pages/Shared/_Head.cshtml: Injeção de MetaTags, CSS e Fontes.
- Pages/Shared/_Favicon.cshtml: Logos de aba de navegador.
- Pages/Shared/_LeftPanel.cshtml: Contêiner da barra lateral.
- Pages/Shared/_Menu.cshtml: Renderizador recursivo do 
av.json.
- Pages/Shared/_Logo.cshtml: Componente de marca flutuante.

### 🧭 Navegação Superior
- Pages/Shared/_PageHeader.cshtml: Barra de topo com perfil e busca.
- Pages/Shared/_PageBreadcrumb.cshtml: Indicador de caminho hierárquico.
- Pages/Shared/_PageHeading.cshtml: Título e subtítulo da página atual.
- Pages/Shared/_DropdownApp.cshtml: Hub de aplicações integradas.
- Pages/Shared/_DropdownMenu.cshtml: Menu de perfil do usuário.
- Pages/Shared/_DropdownNotification.cshtml: Painel detalhado de notificações.

### 🔔 Notificações e Interatividade
- Pages/Shared/_AlertasSino.cshtml & .cs: O sino SignalR de alertas vivos.
- Pages/Shared/_ToastPartial.cshtml & .cs: Sistema de popups flutuantes.
- Pages/Shared/_PageContentOverlay.cshtml: O Spin Overlay de processamento.
- Pages/Shared/_ShortcutMenu.cshtml: Botões de ação rápida (Fab).
- Pages/Shared/_ShortcutMessenger.cshtml: Chat rápido de suporte.
- Pages/Shared/_ShortcutModal.cshtml: Modal genérico para atalhos.
- Pages/Shared/_TabMsgr.cshtml: Painel de mensagens em abas.

### 🛠️ Configurações e Scripts
- Pages/Shared/_ScriptsBasePlugins.cshtml: Core de bibliotecas (jQuery/BS).
- Pages/Shared/_ScriptsLoadingSaving.cshtml: Script de controle de Spinners.
- Pages/Shared/_ValidationScriptsPartial.cshtml: Validação Client-Side.
- Pages/Shared/_PageSettings.cshtml: Painel de personalização de cores.
- Pages/Shared/_TabSettings.cshtml: Configurações de UI em tabs.
- Pages/Shared/_ColorProfileReference.cshtml: Paleta de cores dinâmica.

### 🧩 Componentes Especializados
- Pages/Shared/Components/Navigation/Default.cshtml: Template padrão de menu.
- Pages/Shared/Components/Navigation/TreeView.cshtml: Menu estilo árvore (Treeview).
- Pages/Shared/_Signature.cshtml: Padronização de assinaturas eletrônicas.
- Pages/Shared/_GoogleAnalytics.cshtml: Tag de monitoramento de tráfego.
- Pages/Shared/_Contact.cshtml: Card de contatos de suporte.
- Pages/Shared/_Compose.cshtml / _ComposeLayout.cshtml: Estrutura de edição de documentos.
- Pages/Shared/_CookieConsentPartial.cshtml: Gestão de cookies (LGPD).
- Pages/Shared/_ImagemFichaVistoriaAmarela.cshtml: Template visual para laudos de vistoria.
