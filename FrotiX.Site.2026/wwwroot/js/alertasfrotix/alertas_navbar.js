/* ****************************************************************************************
 * ⚡ ARQUIVO: alertas_navbar.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Sistema global de alertas no navbar com atualizações em tempo real
 *                   via SignalR. Gerencia dropdown de alertas, badge de notificações,
 *                   marcação de lidos e notificações de navegador.
 * 📥 ENTRADAS     : Eventos SignalR (NovoAlerta, AtualizarBadgeAlertas), clicks em UI,
 *                   responses de GET /api/AlertasFrotiX/GetAlertasAtivos
 * 📤 SAÍDAS       : Renderização de dropdown, atualização de badges, POST marcação lidos,
 *                   notificações toast/navegador, console.logs
 * 🔗 CHAMADA POR  : _Layout.cshtml (global), DOMContentLoaded auto-init
 * 🔄 CHAMA        : SignalRManager.getConnection, $.ajax, AppToast.show, Notification API,
 *                   TratamentoErroComLinha, Alerta.Confirmar
 * 📦 DEPENDÊNCIAS : signalr_manager.js (REQUIRED antes deste), jQuery, SignalR Client,
 *                   AppToast (opcional), Alerta.js (opcional), Bootstrap dropdown markup
 * 📝 OBSERVAÇÕES  : Requer signalr_manager.js carregado ANTES. Auto-injeta estilos CSS.
 *                   Badge ID: badgeAlertasSino. Dropdown ID: dropdownAlertas. Todas as
 *                   funções já possuem try-catch completo com TratamentoErroComLinha.
 *
 * 📋 ÍNDICE DE FUNÇÕES (23 funções + 1 IIFE + 1 DOMContentLoaded):
 *
 * ┌─ INICIALIZAÇÃO ─────────────────────────────────────────────────────────┐
 * │ 1. DOMContentLoaded handler                                             │
 * │    → Auto-init: chama inicializarAlertasNavbar e inicializarSignalRNavbar│
 * │    → try-catch global na inicialização                                  │
 * │                                                                          │
 * │ 2. inicializarAlertasNavbar()                                           │
 * │    → Carrega alertas não lidos via AJAX                                │
 * │    → Configura event listeners (sino, dropdown, document click)        │
 * │    → Previne fechamento ao clicar dentro do dropdown                   │
 * │                                                                          │
 * │ 3. inicializarSignalRNavbar()                                           │
 * │    → Verifica se SignalRManager está disponível (fatal se não)        │
 * │    → Obtém conexão via SignalRManager.getConnection()                  │
 * │    → Chama configurarEventHandlersSignalR                              │
 * │    → Registra callbacks de reconexão (onReconnected, onReconnecting, onClose)│
 * │    → Retry automático após 5s se falhar                                │
 * │                                                                          │
 * │ 4. configurarEventHandlersSignalR()                                     │
 * │    → SignalR.on("NovoAlerta") - adiciona ao array, atualiza badge, toast│
 * │    → SignalR.on("AtualizarBadgeAlertas") - atualiza contador de badge │
 * │    → Chama mostrarNotificacaoNavegador para notificações nativas      │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ GERENCIAMENTO DE DROPDOWN ──────────────────────────────────────────────┐
 * │ 5. toggleDropdownAlertas()                                              │
 * │    → Alterna exibição do dropdown (visible → close, hidden → open)    │
 * │                                                                          │
 * │ 6. abrirDropdownAlertas()                                               │
 * │    → Fecha outros dropdowns, fadeIn(200)                               │
 * │    → Recarrega alertas via carregarAlertasNaoLidos                     │
 * │                                                                          │
 * │ 7. fecharDropdownAlertas()                                              │
 * │    → fadeOut(200) do dropdown                                          │
 * │                                                                          │
 * │ 8. renderizarDropdownAlertas()                                          │
 * │    → CORREÇÃO: Cria container #listaAlertasNavbar se não existir      │
 * │    → Renderiza lista de alertas com cards HTML (título, mensagem, etc)│
 * │    → Se vazio: mostra mensagem "Nenhum alerta não lido"               │
 * │    → Usa truncarTexto, obterClasseSeveridade, formatarDataHora        │
 * │    → Botão "Marcar como lido" por alerta                              │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ CARREGAMENTO E MARCAÇÃO DE ALERTAS ─────────────────────────────────────┐
 * │ 9. carregarAlertasNaoLidos()                                            │
 * │    → GET /api/AlertasFrotiX/GetAlertasAtivos                           │
 * │    → Atualiza array alertasNaoLidos global                             │
 * │    → Chama atualizarBadgeNavbar e renderizarDropdownAlertas           │
 * │                                                                          │
 * │ 10. marcarComoLidoNavbar(alertaId)                                      │
 * │     → POST /api/AlertasFrotiX/MarcarComoLido { alertaId }             │
 * │     → Remove visualmente com fadeOut(300)                              │
 * │     → Atualiza array alertasNaoLidos (filter)                          │
 * │     → Atualiza badge, re-renderiza se vazio                            │
 * │     → Toast de sucesso/erro                                            │
 * │                                                                          │
 * │ 11. marcarTodosComoLidosNavbar()                                        │
 * │     → Confirma ação com Alerta.Confirmar (se disponível)              │
 * │     → Chama executarMarcarTodosComoLidos se confirmado                │
 * │     → Fallback sem confirmação se Alerta.Confirmar não existe         │
 * │                                                                          │
 * │ 12. executarMarcarTodosComoLidos()                                      │
 * │     → POST /api/AlertasFrotiX/MarcarTodosComoLidos                     │
 * │     → Limpa array alertasNaoLidos = []                                 │
 * │     → atualizarBadgeNavbar(0), renderiza vazio                         │
 * │     → Toast de sucesso                                                 │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ BADGE E NOTIFICAÇÕES ───────────────────────────────────────────────────┐
 * │ 13. atualizarBadgeNavbar(total)                                         │
 * │     → Atualiza elemento #badgeAlertasSino (CORRIGIDO do anterior)     │
 * │     → Se total > 0: display=block, textContent=total                   │
 * │     → Se total = 0: display=none                                       │
 * │     → Aviso console.warn se badge não encontrado                       │
 * │                                                                          │
 * │ 14. mostrarNotificacaoNavegador(alerta)                                 │
 * │     → Verifica suporte: if (!("Notification" in window)) return        │
 * │     → Se permission="granted": cria notificação                        │
 * │     → Se permission="denied": não faz nada                             │
 * │     → Caso contrário: requestPermission, cria se granted               │
 * │                                                                          │
 * │ 15. criarNotificacao(alerta)                                            │
 * │     → new Notification(alerta.titulo, { body, icon, badge })           │
 * │     → onclick: foca window, abre dropdown, fecha notificação           │
 * │     → Ícone: /img/logo-small.png                                       │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ FUNÇÕES AUXILIARES ─────────────────────────────────────────────────────┐
 * │ 16. obterClasseSeveridade(severidade)                                   │
 * │     → Mapeia severidade → classe Bootstrap                             │
 * │     → { Critico: 'danger', Alto: 'warning', Medio: 'info', Baixo: 'secondary' }│
 * │     → Default: 'info'                                                   │
 * │                                                                          │
 * │ 17. formatarDataHora(dataStr)                                           │
 * │     → Calcula diff (agora - data)                                      │
 * │     → < 1 min: "Agora"                                                 │
 * │     → < 60 min: "X min atrás"                                          │
 * │     → < 24h: "X h atrás"                                               │
 * │     → < 7 dias: "X dia(s) atrás"                                       │
 * │     → >= 7 dias: toLocaleDateString('pt-BR')                           │
 * │                                                                          │
 * │ 18. truncarTexto(texto, maxLength)                                      │
 * │     → Se texto.length <= maxLength: retorna texto                      │
 * │     → Caso contrário: texto.substring(0, maxLength) + '...'            │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ INJEÇÃO DE ESTILOS CSS ─────────────────────────────────────────────────┐
 * │ 19. IIFE auto-executável (lines ~740-811)                               │
 * │     → Injeta <style id="estiloAlertasNavbar"> no <head>                │
 * │     → Define estilos para #dropdownAlertas, #listaAlertasNavbar, etc. │
 * │     → Verifica se já existe para evitar duplicação                     │
 * │     → Estilos: dropdown box-shadow, hover effects, badge absoluto     │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 FLUXO DE INICIALIZAÇÃO:
 * 1. DOMContentLoaded dispara
 * 2. inicializarAlertasNavbar: carrega alertas iniciais, configura UI listeners
 * 3. inicializarSignalRNavbar: obtém conexão SignalR, registra event handlers
 * 4. configurarEventHandlersSignalR: escuta "NovoAlerta", "AtualizarBadgeAlertas"
 * 5. IIFE injeta CSS automaticamente
 * 6. Sistema fica aguardando eventos SignalR e interações do usuário
 *
 * 🔄 FLUXO DE NOVO ALERTA (SignalR):
 * 1. SignalR.on("NovoAlerta") dispara
 * 2. alertasNaoLidos.unshift(alerta)
 * 3. atualizarBadgeNavbar(length)
 * 4. Se dropdown visível: renderizarDropdownAlertas
 * 5. AppToast.show (se disponível)
 * 6. mostrarNotificacaoNavegador (se permitido)
 *
 * 🔄 FLUXO DE MARCAR COMO LIDO:
 * 1. Usuário clica "Marcar como lido" no alerta
 * 2. marcarComoLidoNavbar(alertaId) → POST /api
 * 3. Sucesso: fadeOut visual, filter array, atualizar badge
 * 4. Se array vazio: renderiza mensagem "Nenhum alerta"
 * 5. Toast de feedback
 *
 * 📌 VARIÁVEIS GLOBAIS:
 * - connectionAlertasNavbar: Conexão SignalR do navbar
 * - alertasNaoLidos: Array de alertas não lidos (sincronizado com API)
 *
 * 📌 ELEMENTOS DOM REQUERIDOS:
 * - #btnNotificacoes ou #iconeSino: Botão/ícone do sino (click handler)
 * - #dropdownAlertas: Container do dropdown (criado/verificado em runtime)
 * - #listaAlertasNavbar: Container da lista (criado dinamicamente se não existir)
 * - #badgeAlertasSino: Badge de contador (atualizado em tempo real)
 * - #btnMarcarTodosLidosNavbar: Botão marcar todos (criado dinamicamente)
 *
 * 📌 API ENDPOINTS:
 * - GET /api/AlertasFrotiX/GetAlertasAtivos → { sucesso, dados: [ alerta[] ] }
 * - POST /api/AlertasFrotiX/MarcarComoLido { alertaId } → { sucesso, message }
 * - POST /api/AlertasFrotiX/MarcarTodosComoLidos → { sucesso, message }
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - CORREÇÃO IMPORTANTE: linha 588 usa #badgeAlertasSino (não #badgeAlertasNavbar)
 * - CORREÇÃO IMPORTANTE: renderizarDropdownAlertas cria estrutura HTML se não existir
 * - SignalR callbacks de reconexão: recarrega alertas automaticamente
 * - Retry de conexão SignalR: setTimeout 5s se falhar
 * - Todas as funções têm try-catch com TratamentoErroComLinha
 * - Injeção de CSS: evita duplicação com verificação $('#estiloAlertasNavbar').length
 * - Browser notifications: usa Notification API nativa (request permission se needed)
 * - Dropdown fecha ao clicar fora (document click handler)
 * - Dropdown NÃO fecha ao clicar dentro (e.stopPropagation)
 *
 * 🔌 VERSÃO: 2.0 (CORRIGIDA)
 * 📌 ÚLTIMA ATUALIZAÇÃO: 01/02/2026
 **************************************************************************************** */

var connectionAlertasNavbar;
var alertasNaoLidos = [];

$(document).ready(function ()
{
    try
    {
        console.log("✅ Inicializando alertas_navbar.js...");
        inicializarAlertasNavbar();
        inicializarSignalRNavbar();
    }
    catch (error)
    {
        TratamentoErroComLinha("alertas_navbar.js", "document.ready", error);
    }
});

function inicializarAlertasNavbar()
{
    try
    {
        // Carregar alertas não lidos ao iniciar
        carregarAlertasNaoLidos();

        // Configurar evento de clique no sino
        $('#btnNotificacoes, #iconeSino').on('click', function (e)
        {
            try
            {
                e.preventDefault();
                e.stopPropagation();
                toggleDropdownAlertas();
            }
            catch (error)
            {
                TratamentoErroComLinha("alertas_navbar.js", "btnNotificacoes.click", error);
            }
        });

        // Fechar dropdown ao clicar fora
        $(document).on('click', function (e)
        {
            try
            {
                if (!$(e.target).closest('#dropdownAlertas, #btnNotificacoes, #iconeSino').length)
                {
                    fecharDropdownAlertas();
                }
            }
            catch (error)
            {
                TratamentoErroComLinha("alertas_navbar.js", "document.click", error);
            }
        });

        // Prevenir que cliques dentro do dropdown o fechem
        $('#dropdownAlertas').on('click', function (e)
        {
            try
            {
                e.stopPropagation();
            }
            catch (error)
            {
                TratamentoErroComLinha("alertas_navbar.js", "dropdownAlertas.click", error);
            }
        });

        console.log("✅ Alertas navbar inicializado");
    }
    catch (error)
    {
        TratamentoErroComLinha("alertas_navbar.js", "inicializarAlertasNavbar", error);
    }
}

function inicializarSignalRNavbar()
{
    try
    {
        console.log("🔧 Configurando SignalR para Navbar...");

        // Verificar se SignalRManager está disponível
        if (typeof SignalRManager === 'undefined')
        {
            console.error("❌ SignalRManager não está carregado!");
            console.error("Certifique-se de que signalr_manager.js está carregado ANTES de alertas_navbar.js");
            return;
        }

        // Obter conexão do gerenciador global
        SignalRManager.getConnection()
            .then(function (conn)
            {
                try
                {
                    connectionAlertasNavbar = conn;
                    console.log("✅ Conexão SignalR obtida para Navbar");

                    // Registrar event handlers usando o gerenciador
                    configurarEventHandlersSignalR();

                    // Registrar callbacks de reconexão
                    SignalRManager.registerCallback({
                        onReconnected: function (connectionId)
                        {
                            try
                            {
                                console.log("🔄 Navbar: SignalR reconectado, recarregando alertas...");
                                carregarAlertasNaoLidos();
                            }
                            catch (error)
                            {
                                TratamentoErroComLinha("alertas_navbar.js", "callback.onReconnected", error);
                            }
                        },
                        onReconnecting: function (error)
                        {
                            console.log("🔄 Navbar: SignalR reconectando...");
                        },
                        onClose: function (error)
                        {
                            console.log("❌ Navbar: Conexão SignalR fechada");
                        }
                    });

                    console.log("✅ SignalR configurado com sucesso para Navbar");
                }
                catch (error)
                {
                    TratamentoErroComLinha("alertas_navbar.js", "getConnection.then", error);
                }
            })
            .catch(function (err)
            {
                try
                {
                    console.error("❌ Erro ao obter conexão SignalR para Navbar:", err);
                    // Tentar novamente após 5 segundos
                    setTimeout(inicializarSignalRNavbar, 5000);
                }
                catch (error)
                {
                    TratamentoErroComLinha("alertas_navbar.js", "getConnection.catch", error);
                }
            });
    }
    catch (error)
    {
        TratamentoErroComLinha("alertas_navbar.js", "inicializarSignalRNavbar", error);
    }
}

function configurarEventHandlersSignalR()
{
    try
    {
        // Evento: Novo alerta recebido
        SignalRManager.on("NovoAlerta", function (alerta)
        {
            try
            {
                console.log("📬 Novo alerta recebido no navbar:", alerta);

                // Adicionar ao array no início
                alertasNaoLidos.unshift(alerta);

                // Atualizar badge
                atualizarBadgeNavbar(alertasNaoLidos.length);

                // Se o dropdown estiver aberto, atualizar
                if ($('#dropdownAlertas').is(':visible'))
                {
                    renderizarDropdownAlertas();
                }

                // Mostrar notificação toast
                if (typeof AppToast !== 'undefined')
                {
                    AppToast.show("Amarelo", "Novo alerta: " + alerta.titulo, 3000);
                }

                // Notificação do navegador (opcional)
                mostrarNotificacaoNavegador(alerta);
            }
            catch (error)
            {
                TratamentoErroComLinha("alertas_navbar.js", "SignalR.NovoAlerta", error);
            }
        });

        // Evento: Badge atualizado
        SignalRManager.on("AtualizarBadgeAlertas", function (quantidade)
        {
            try
            {
                console.log("🔢 Atualizar badge navbar:", quantidade);
                atualizarBadgeNavbar(quantidade);
            }
            catch (error)
            {
                TratamentoErroComLinha("alertas_navbar.js", "SignalR.AtualizarBadgeAlertas", error);
            }
        });

        console.log("✅ Event handlers configurados para Navbar");
    }
    catch (error)
    {
        TratamentoErroComLinha("alertas_navbar.js", "configurarEventHandlersSignalR", error);
    }
}

function toggleDropdownAlertas()
{
    try
    {
        var dropdown = $('#dropdownAlertas');

        if (dropdown.is(':visible'))
        {
            fecharDropdownAlertas();
        } else
        {
            abrirDropdownAlertas();
        }
    }
    catch (error)
    {
        TratamentoErroComLinha("alertas_navbar.js", "toggleDropdownAlertas", error);
    }
}

function abrirDropdownAlertas()
{
    try
    {
        var dropdown = $('#dropdownAlertas');

        // Fechar outros dropdowns que possam estar abertos
        $('.dropdown-menu').not(dropdown).hide();

        // Mostrar dropdown
        dropdown.fadeIn(200);

        // Recarregar alertas
        carregarAlertasNaoLidos();
    }
    catch (error)
    {
        TratamentoErroComLinha("alertas_navbar.js", "abrirDropdownAlertas", error);
    }
}

function fecharDropdownAlertas()
{
    try
    {
        $('#dropdownAlertas').fadeOut(200);
    }
    catch (error)
    {
        TratamentoErroComLinha("alertas_navbar.js", "fecharDropdownAlertas", error);
    }
}

function carregarAlertasNaoLidos()
{
    try
    {
        $.ajax({
            url: '/api/AlertasFrotiX/GetAlertasAtivos',
            type: 'GET',
            dataType: 'json',
            success: function (response)
            {
                try
                {
                    if (response && response.sucesso)
                    {
                        alertasNaoLidos = response.dados || [];
                        atualizarBadgeNavbar(alertasNaoLidos.length);
                        renderizarDropdownAlertas();
                    }
                }
                catch (error)
                {
                    TratamentoErroComLinha("alertas_navbar.js", "carregarAlertasNaoLidos.success", error);
                }
            },
            error: function (xhr, status, error)
            {
                TratamentoErroComLinha("alertas_navbar.js", "carregarAlertasNaoLidos.error", error);
            }
        });
    }
    catch (error)
    {
        TratamentoErroComLinha("alertas_navbar.js", "carregarAlertasNaoLidos", error);
    }
}

function renderizarDropdownAlertas()
{
    try
    {
        var container = $('#listaAlertasNavbar');

        // ✅ CORREÇÃO: Verificar se container existe, criar se não existir
        if (container.length === 0)
        {
            try
            {
                console.warn('⚠️ Container #listaAlertasNavbar não encontrado, criando...');

                var dropdown = $('#dropdownAlertas');
                if (dropdown.length === 0)
                {
                    console.error('❌ Dropdown #dropdownAlertas também não existe!');
                    return;
                }

                // Criar estrutura se não existir
                dropdown.html(`
                    <div class="dropdown-header">
                        <div class="d-flex justify-content-between align-items-center">
                            <h6 class="mb-0">Alertas</h6>
                            <button id="btnMarcarTodosLidosNavbar" class="btn btn-sm btn-link text-primary p-0">
                                Marcar todos como lidos
                            </button>
                        </div>
                    </div>
                    <div id="listaAlertasNavbar"></div>
                `);

                container = $('#listaAlertasNavbar');

                // Configurar evento do botão marcar todos
                $('#btnMarcarTodosLidosNavbar').on('click', function ()
                {
                    try
                    {
                        marcarTodosComoLidosNavbar();
                    }
                    catch (error)
                    {
                        TratamentoErroComLinha("alertas_navbar.js", "btnMarcarTodosLidosNavbar.click", error);
                    }
                });
            }
            catch (error)
            {
                TratamentoErroComLinha("alertas_navbar.js", "renderizarDropdownAlertas.criarContainer", error);
                return;
            }
        }

        container.empty();

        if (alertasNaoLidos.length === 0)
        {
            container.html(`
                <div class="p-4 text-center text-muted">
                    <i class="fal fa-check-circle fa-2x mb-2"></i>
                    <p class="mb-0">Nenhum alerta não lido</p>
                </div>
            `);
            return;
        }

        // Renderizar alertas
        alertasNaoLidos.forEach(function (alerta)
        {
            try
            {
                var alertaHtml = `
                    <div class="alerta-item p-3 border-bottom hover-bg-light" data-alerta-id="${alerta.alertaId}">
                        <div class="d-flex">
                            <div class="flex-grow-1">
                                <div class="d-flex justify-content-between align-items-start mb-1">
                                    <h6 class="mb-0">${truncarTexto(alerta.titulo, 50)}</h6>
                                    <span class="badge badge-${obterClasseSeveridade(alerta.severidade)} ml-2">
                                        ${alerta.severidade}
                                    </span>
                                </div>
                                <p class="text-muted small mb-2">${truncarTexto(alerta.mensagem, 100)}</p>
                                <div class="d-flex justify-content-between align-items-center">
                                    <small class="text-muted">
                                        <i class="fal fa-clock mr-1"></i>
                                        ${formatarDataHora(alerta.dataInsercao)}
                                    </small>
                                    <button class="btn btn-sm btn-link text-primary p-0" 
                                            onclick="marcarComoLidoNavbar('${alerta.alertaId}')">
                                        Marcar como lido
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                `;
                container.append(alertaHtml);
            }
            catch (error)
            {
                TratamentoErroComLinha("alertas_navbar.js", "renderizarDropdownAlertas.forEach", error);
            }
        });

        console.log("✅ Dropdown renderizado com", alertasNaoLidos.length, "alertas");
    }
    catch (error)
    {
        TratamentoErroComLinha("alertas_navbar.js", "renderizarDropdownAlertas", error);
    }
}

function marcarComoLidoNavbar(alertaId)
{
    try
    {
        $.ajax({
            url: '/api/AlertasFrotiX/MarcarComoLido',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ alertaId: alertaId }),
            success: function (response)
            {
                try
                {
                    if (response && response.sucesso)
                    {
                        // Remover visualmente o alerta
                        $('.alerta-item[data-alerta-id="' + alertaId + '"]').fadeOut(300, function ()
                        {
                            try
                            {
                                $(this).remove();

                                // Atualizar array
                                alertasNaoLidos = alertasNaoLidos.filter(function (a)
                                {
                                    return a.alertaId !== alertaId;
                                });

                                // Atualizar badge
                                atualizarBadgeNavbar(alertasNaoLidos.length);

                                // Se não houver mais alertas, mostrar mensagem
                                if (alertasNaoLidos.length === 0)
                                {
                                    renderizarDropdownAlertas();
                                }
                            }
                            catch (error)
                            {
                                TratamentoErroComLinha("alertas_navbar.js", "marcarComoLidoNavbar.fadeOut", error);
                            }
                        });

                        // Toast de sucesso
                        if (typeof AppToast !== 'undefined')
                        {
                            AppToast.show("Verde", "Alerta marcado como lido", 2000);
                        }
                    }
                }
                catch (error)
                {
                    TratamentoErroComLinha("alertas_navbar.js", "marcarComoLidoNavbar.success", error);
                }
            },
            error: function (xhr, status, error)
            {
                TratamentoErroComLinha("alertas_navbar.js", "marcarComoLidoNavbar.error", error);

                if (typeof AppToast !== 'undefined')
                {
                    AppToast.show("Vermelho", "Erro ao marcar alerta como lido", 2000);
                }
            }
        });
    }
    catch (error)
    {
        TratamentoErroComLinha("alertas_navbar.js", "marcarComoLidoNavbar", error);
    }
}

function marcarTodosComoLidosNavbar()
{
    try
    {
        if (alertasNaoLidos.length === 0)
        {
            if (typeof AppToast !== 'undefined')
            {
                AppToast.show("Amarelo", "Não há alertas para marcar como lidos", 2000);
            }
            return;
        }

        // Confirmar ação
        if (typeof Alerta !== 'undefined' && typeof Alerta.Confirmar === 'function')
        {
            Alerta.Confirmar(
                "Confirmar Ação",
                "Deseja marcar todos os " + alertasNaoLidos.length + " alertas como lidos?",
                "Sim, marcar todos",
                "Cancelar"
            ).then(function (confirmed)
            {
                try
                {
                    if (confirmed)
                    {
                        executarMarcarTodosComoLidos();
                    }
                }
                catch (error)
                {
                    TratamentoErroComLinha("alertas_navbar.js", "marcarTodosComoLidosNavbar.confirmar", error);
                }
            });
        } else
        {
            // Fallback sem confirmação
            executarMarcarTodosComoLidos();
        }
    }
    catch (error)
    {
        TratamentoErroComLinha("alertas_navbar.js", "marcarTodosComoLidosNavbar", error);
    }
}

function executarMarcarTodosComoLidos()
{
    try
    {
        $.ajax({
            url: '/api/AlertasFrotiX/MarcarTodosComoLidos',
            type: 'POST',
            contentType: 'application/json',
            success: function (response)
            {
                try
                {
                    if (response && response.sucesso)
                    {
                        alertasNaoLidos = [];
                        atualizarBadgeNavbar(0);
                        renderizarDropdownAlertas();

                        if (typeof AppToast !== 'undefined')
                        {
                            AppToast.show("Verde", "Todos os alertas foram marcados como lidos", 2000);
                        }
                    }
                }
                catch (error)
                {
                    TratamentoErroComLinha("alertas_navbar.js", "executarMarcarTodosComoLidos.success", error);
                }
            },
            error: function (xhr, status, error)
            {
                TratamentoErroComLinha("alertas_navbar.js", "executarMarcarTodosComoLidos.error", error);

                if (typeof AppToast !== 'undefined')
                {
                    AppToast.show("Vermelho", "Erro ao marcar todos os alertas como lidos", 2000);
                }
            }
        });
    }
    catch (error)
    {
        TratamentoErroComLinha("alertas_navbar.js", "executarMarcarTodosComoLidos", error);
    }
}

function atualizarBadgeNavbar(total)
{
    const badge = document.getElementById('badgeAlertasSino'); // ← CORRIGIDO

    if (!badge)
    {
        console.warn('⚠️ Badge #badgeAlertasSino não encontrado');
        return;
    }

    if (total > 0)
    {
        badge.textContent = total;
        badge.style.display = 'block';
    } else
    {
        badge.style.display = 'none';
    }
}
function mostrarNotificacaoNavegador(alerta)
{
    try
    {
        // Verificar se o navegador suporta notificações
        if (!("Notification" in window))
        {
            return;
        }

        // Verificar permissão
        if (Notification.permission === "granted")
        {
            criarNotificacao(alerta);
        } else if (Notification.permission !== "denied")
        {
            Notification.requestPermission().then(function (permission)
            {
                try
                {
                    if (permission === "granted")
                    {
                        criarNotificacao(alerta);
                    }
                }
                catch (error)
                {
                    TratamentoErroComLinha("alertas_navbar.js", "mostrarNotificacaoNavegador.requestPermission", error);
                }
            });
        }
    }
    catch (error)
    {
        TratamentoErroComLinha("alertas_navbar.js", "mostrarNotificacaoNavegador", error);
    }
}

function criarNotificacao(alerta)
{
    try
    {
        var notification = new Notification(alerta.titulo, {
            body: alerta.mensagem,
            icon: '/img/logo-small.png',
            badge: '/img/badge.png'
        });

        notification.onclick = function ()
        {
            try
            {
                window.focus();
                abrirDropdownAlertas();
                notification.close();
            }
            catch (error)
            {
                TratamentoErroComLinha("alertas_navbar.js", "notification.onclick", error);
            }
        };
    }
    catch (error)
    {
        TratamentoErroComLinha("alertas_navbar.js", "criarNotificacao", error);
    }
}

// ========== FUNÇÕES AUXILIARES ==========

function obterClasseSeveridade(severidade)
{
    try
    {
        var classes = {
            'Critico': 'danger',
            'Crítico': 'danger',
            'Alto': 'warning',
            'Medio': 'info',
            'Médio': 'info',
            'Baixo': 'secondary'
        };
        return classes[severidade] || 'info';
    }
    catch (error)
    {
        TratamentoErroComLinha("alertas_navbar.js", "obterClasseSeveridade", error);
        return 'info';
    }
}

function formatarDataHora(dataStr)
{
    try
    {
        var data = new Date(dataStr);
        var agora = new Date();
        var diff = agora - data;
        var minutos = Math.floor(diff / 60000);

        if (minutos < 1) return 'Agora';
        if (minutos < 60) return minutos + ' min atrás';

        var horas = Math.floor(minutos / 60);
        if (horas < 24) return horas + ' h atrás';

        var dias = Math.floor(horas / 24);
        if (dias < 7) return dias + ' dia' + (dias > 1 ? 's' : '') + ' atrás';

        return data.toLocaleDateString('pt-BR');
    }
    catch (error)
    {
        TratamentoErroComLinha("alertas_navbar.js", "formatarDataHora", error);
        return dataStr;
    }
}

function truncarTexto(texto, maxLength)
{
    try
    {
        if (!texto) return '';
        if (texto.length <= maxLength) return texto;
        return texto.substring(0, maxLength) + '...';
    }
    catch (error)
    {
        TratamentoErroComLinha("alertas_navbar.js", "truncarTexto", error);
        return texto || '';
    }
}

// ========== INJEÇÃO DE ESTILOS CSS ==========

(function ()
{
    try
    {
        var estiloAlertas = `
        <style id="estiloAlertasNavbar">
        #dropdownAlertas {
            display: none;
            position: absolute;
            right: 0;
            top: 100%;
            margin-top: 0.5rem;
            width: 400px;
            max-width: 90vw;
            max-height: 500px;
            background: white;
            border-radius: 0.5rem;
            box-shadow: 0 10px 30px rgba(0,0,0,0.2);
            z-index: 9999;
            overflow: hidden;
        }

        #dropdownAlertas .dropdown-header {
            padding: 1rem;
            border-bottom: 1px solid #e5e7eb;
            background: #f9fafb;
        }

        #listaAlertasNavbar {
            max-height: 400px;
            overflow-y: auto;
        }

        .hover-bg-light:hover {
            background-color: #f9fafb;
            cursor: pointer;
        }

        .alerta-item {
            transition: all 0.2s;
        }

        #badgeAlertasNavbar {
            position: absolute;
            top: -5px;
            right: -5px;
            background-color: #ef4444;
            color: white;
            font-size: 0.7rem;
            font-weight: bold;
            padding: 0.15rem 0.4rem;
            border-radius: 50%;
            min-width: 18px;
            height: 18px;
            display: flex;
            align-items: center;
            justify-content: center;
        }
        </style>
        `;

        // Injetar estilos na página (apenas uma vez)
        if ($('#estiloAlertasNavbar').length === 0)
        {
            $('head').append(estiloAlertas);
        }
    }
    catch (error)
    {
        TratamentoErroComLinha("alertas_navbar.js", "injecaoEstilos", error);
    }
})();

console.log("✅ alertas_navbar.js carregado completamente");
