/* ****************************************************************************************
 * ⚡ ARQUIVO: custom.js
 * ================================================================================================
 * 
 * 📋 OBJETIVO:
 *    Funções auxiliares globais para controle de sidebars e painéis laterais via manipulação
 *    direta de CSS width. Gerencia 3 painéis distintos: mySidenav (menu principal 253px),
 *    profile (perfil usuário 300px) e profile2 (configurações 301px). Legacy code sem jQuery,
 *    funções globais expostas para onclick HTML inline. Sem animações CSS (transições
 *    devem ser definidas via CSS se necessário).
 * 
 * 🔢 PARÂMETROS DE ENTRADA:
 *    - Nenhum parâmetro: funções chamadas diretamente via onclick="openNav()" em HTML
 *    - Elementos DOM: IDs fixos hardcoded (mySidenav, profile, profile2)
 * 
 * 📤 SAÍDAS PRODUZIDAS:
 *    - Alteração CSS: document.getElementById("id").style.width = "253px" ou "0px"
 *    - Efeito visual: painel desliza da direita/esquerda (via CSS transition se definido)
 *    - Nenhum retorno (void functions)
 * 
 * 🔗 DEPENDÊNCIAS:
 *    • BIBLIOTECAS: Vanilla JavaScript (sem jQuery)
 *    • HTML REQUIRED: elementos com IDs exatos: #mySidenav, #profile, #profile2
 *    • CSS: transitions em .sidenav (ex: transition: width 0.3s ease) para animação suave
 * 
 * ================================================================================================
 * 📑 ÍNDICE DE FUNÇÕES (6 funções globais)
 * ================================================================================================
 * 
 * ┌─────────────────────────────────────────────────────────────────────────────────────────┐
 * │ 📂 PAINEL PRINCIPAL (mySidenav - Menu Principal 253px)                                   │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • openNav()                                → Abre sidebar #mySidenav (width: 253px)      │
 * │ • closeNav()                               → Fecha sidebar #mySidenav (width: 0px)       │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ 👤 PAINEL PERFIL USUÁRIO (profile - 300px)                                               │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • openNav2()                               → Abre painel #profile (width: 300px)         │
 * │ • closeNav2()                              → Fecha painel #profile (width: 0px)          │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ ⚙️ PAINEL CONFIGURAÇÕES (profile2 - 301px)                                               │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • openNav3()                               → Abre painel #profile2 (width: 301px)        │
 * │ • closeNav3()                              → Fecha painel #profile2 (width: 0px)         │
 * └─────────────────────────────────────────────────────────────────────────────────────────┘
 * 
 * ================================================================================================
 * 🔄 FLUXOS TÍPICOS
 * ================================================================================================
 * 
 * 💡 FLUXO 1: Abrir menu principal (sidebar navegação)
 *    Click botão hamburger → onclick="openNav()" (HTML inline)
 *      → document.getElementById("mySidenav").style.width = "253px"
 *      → Sidebar desliza da esquerda (se CSS transition definido)
 *      → Overlay escurece conteúdo principal (se implementado)
 * 
 * 💡 FLUXO 2: Fechar menu principal (click fora ou botão ×)
 *    Click overlay ou botão × → onclick="closeNav()"
 *      → document.getElementById("mySidenav").style.width = "0px"
 *      → Sidebar oculta (colapsa para largura 0)
 * 
 * 💡 FLUXO 3: Abrir perfil usuário (dropdown header)
 *    Click ícone perfil → onclick="openNav2()"
 *      → document.getElementById("profile").style.width = "300px"
 *      → Painel perfil desliza da direita
 *      → Exibe foto, nome, configurações usuário
 * 
 * ================================================================================================
 * 🔍 OBSERVAÇÕES TÉCNICAS
 * ================================================================================================
 * 
 * 📏 LARGURAS FIXAS:
 *    - mySidenav: 253px (menu principal)
 *    - profile: 300px (perfil usuário)
 *    - profile2: 301px (configurações - 1px diferente para evitar conflito CSS?)
 * 
 * 🎨 ANIMAÇÕES:
 *    - NÃO implementadas em JS (funções apenas setam width instantaneamente)
 *    - Para animação suave: adicionar CSS transition no .sidenav:
 *      .sidenav { transition: width 0.3s ease; }
 *    - Efeito: width 0px → 253px com animação deslizante
 * 
 * 🔒 SEGURANÇA:
 *    - NENHUMA validação (código legacy, assume elementos existem)
 *    - Possível erro: "Cannot read property 'style' of null" se ID não existir
 *    - Recomendado refatorar com try-catch ou verificação:
 *      const el = document.getElementById("mySidenav");
 *      if (el) el.style.width = "253px";
 * 
 * 📱 RESPONSIVIDADE:
 *    - Larguras fixas em px (não responsivas)
 *    - Para mobile: considerar max-width: 100vw ou larguras menores
 *    - Pode sobrepor conteúdo em telas < 768px
 * 
 * ⚠️ ACESSIBILIDADE:
 *    - NENHUM suporte (não gerencia aria-expanded, aria-hidden)
 *    - Não controla foco (tab trap não implementado)
 *    - Recomendado: adicionar aria attributes e gerenciamento de foco
 * 
 * 🗑️ FECHAR PAINÉIS:
 *    - Apenas width: 0px (elemento continua no DOM, visibility: hidden não usado)
 *    - Conteúdo interno permanece renderizado (pode afetar performance se complexo)
 * 
 * 🔄 MÚLTIPLOS PAINÉIS:
 *    - Não há lógica de exclusividade (abrir profile2 NÃO fecha profile automaticamente)
 *    - Usuário pode abrir múltiplos painéis simultaneamente (sobreposição)
 *    - Para exclusividade: adicionar closeNav2() dentro de openNav3() e vice-versa
 * 
 * 🎯 CASOS DE USO TÍPICOS:
 *    - mySidenav: menu navegação principal (Agendamentos, Viagens, Cadastros)
 *    - profile: dropdown perfil (foto, nome, logout, preferências)
 *    - profile2: painel configurações avançadas (tema, notificações)
 * 
 * 🛠️ REFATORAÇÃO SUGERIDA (futuro):
 *    - Encapsular em módulo: const SidebarManager = { open, close, toggle }
 *    - Parâmetros dinâmicos: open(elementId, width)
 *    - Validação elementos: if (!element) return;
 *    - Event listeners: adicionar em JS ao invés de onclick inline
 *    - Animações via classList.add('open') ao invés de style.width
 * 
 * **************************************************************************************** */

/* ****************************************************************************************
 * ⚡ ARQUIVO: custom.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Funções auxiliares para abrir/fechar sidebars e painéis laterais
 *                   via manipulação direta de CSS width. Controla 3 painéis distintos:
 *                   mySidenav (253px), profile (300px) e profile2 (301px).
 * 📥 ENTRADAS     : Chamadas de funções via onclick em elementos HTML
 * 📤 SAÍDAS       : Alteração de estilo CSS (width) em elementos DOM específicos
 * 🔗 CHAMADA POR  : Event handlers onclick em botões de menu/perfil (elementos HTML)
 * 🔄 CHAMA        : document.getElementById(), style.width manipulation
 * 📦 DEPENDÊNCIAS : Vanilla JavaScript (sem jQuery), elementos DOM com IDs específicos
 * 📝 OBSERVAÇÕES  : Legacy code, funções globais (não encapsuladas), sem animações CSS
 *
 * 📋 ÍNDICE DE FUNÇÕES (6 funções principais):
 *
 * ┌─ PAINEL PRINCIPAL (mySidenav) ─────────────────────────────────────────────┐
 * │ 1. openNav()                                                                │
 * │    → Abre sidebar principal #mySidenav com width 253px                     │
 * │    → Manipulação direta: document.getElementById("mySidenav").style.width  │
 * │                                                                             │
 * │ 2. closeNav()                                                               │
 * │    → Fecha sidebar principal #mySidenav com width 0px                      │
 * └─────────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ PAINEL DE PERFIL (profile) ───────────────────────────────────────────────┐
 * │ 3. openNav2()                                                               │
 * │    → Abre painel lateral #profile com width 300px                          │
 * │                                                                             │
 * │ 4. closeNav2()                                                              │
 * │    → Fecha painel lateral #profile com width 0px                           │
 * └─────────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ PAINEL DE PERFIL 2 (profile2) ────────────────────────────────────────────┐
 * │ 5. openNav3()                                                               │
 * │    → Abre painel lateral #profile2 com width 301px                         │
 * │                                                                             │
 * │ 6. closeNav3()                                                              │
 * │    → Fecha painel lateral #profile2 com width 0px                          │
 * └─────────────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 ELEMENTOS DOM ESPERADOS:
 * - #mySidenav (sidebar principal - 253px)
 * - #profile (painel perfil 1 - 300px)
 * - #profile2 (painel perfil 2 - 301px)
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - Funções expostas globalmente (window.openNav, etc.)
 * - Sem validação de existência dos elementos DOM
 * - Sem animações CSS (transição deve estar no CSS)
 * - Widths hardcoded (253px, 300px, 301px)
 **************************************************************************************** */

function openNav() {
    try {
        document.getElementById("mySidenav").style.width = "253px";
    } catch (erro) {
        console.error('Erro em openNav:', erro);
        Alerta.TratamentoErroComLinha('custom.js', 'openNav', erro);
    }
}

function closeNav() {
    try {
        document.getElementById("mySidenav").style.width = "0";
    } catch (erro) {
        console.error('Erro em closeNav:', erro);
        Alerta.TratamentoErroComLinha('custom.js', 'closeNav', erro);
    }
}

/*---------------------------------------------------------------*/

function openNav2() {
    try {
        document.getElementById("profile").style.width = "300px";
    } catch (erro) {
        console.error('Erro em openNav2:', erro);
        Alerta.TratamentoErroComLinha('custom.js', 'openNav2', erro);
    }
}

function closeNav2() {
    try {
        document.getElementById("profile").style.width = "0";
    } catch (erro) {
        console.error('Erro em closeNav2:', erro);
        Alerta.TratamentoErroComLinha('custom.js', 'closeNav2', erro);
    }
}

/*---------------------------------------------------------------*/

function openNav3() {
    try {
        document.getElementById("profile2").style.width = "301px";
    } catch (erro) {
        console.error('Erro em openNav3:', erro);
        Alerta.TratamentoErroComLinha('custom.js', 'openNav3', erro);
    }
}

function closeNav3() {
    try {
        document.getElementById("profile2").style.width = "0";
    } catch (erro) {
        console.error('Erro em closeNav3:', erro);
        Alerta.TratamentoErroComLinha('custom.js', 'closeNav3', erro);
    }
}
		