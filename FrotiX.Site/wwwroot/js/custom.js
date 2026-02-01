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
		