/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║                                                                          ║
 * ║     ███████╗██████╗  ██████╗ ████████╗██╗██╗  ██╗    ██████╗  ██████╗    ║
 * ║     ██╔════╝██╔══██╗██╔═══██╗╚══██╔══╝██║╚██╗██╔╝    ╚════██╗██╔═████╗   ║
 * ║     █████╗  ██████╔╝██║   ██║   ██║   ██║ ╚███╔╝      █████╔╝██║██╔██║   ║
 * ║     ██╔══╝  ██╔══██╗██║   ██║   ██║   ██║ ██╔██╗     ██╔═══╝ ████╔╝██║   ║
 * ║     ██║     ██║  ██║╚██████╔╝   ██║   ██║██╔╝ ██╗    ███████╗╚██████╔╝   ║
 * ║     ╚═╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝╚═╝  ╚═╝    ╚══════╝ ╚═════╝    ║
 * ║                                                                          ║
 * ║                      SOLUÇÃO FROTIX - GESTÃO DE FROTAS                   ║
 * ║                                                                          ║
 * ╠══════════════════════════════════════════════════════════════════════════╣
 * ║ 📄 ARQUIVO: whatsapp.js                                                  ║
 * ║ 📍 LOCAL: wwwroot/js/                                                    ║
 * ║ 📋 VERSÃO: 1.0                                                           ║
 * ║ 📅 ATUALIZAÇÃO: 23/01/2026                                               ║
 * ╠══════════════════════════════════════════════════════════════════════════╣
 * ║ ❓ POR QUE EXISTO?                                                       ║
 * ║    Integração com WhatsApp Business API para notificações.               ║
 * ║    • Autenticação via QR Code                                            ║
 * ║    • Envio de mensagens de texto                                         ║
 * ║    • Envio de mídia (imagens, documentos)                                ║
 * ║    • Monitoramento de status da sessão                                   ║
 * ║    • Interface modal para gerenciamento                                  ║
 * ║                                                                          ║
 * ║ 🔗 RELEVÂNCIA: Média (Notificações - WhatsApp)                           ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

// wwwroot/js/whatsapp.js
(function () {
    try {
        const FILE = 'whatsapp.js';

        const $ = window.jQuery;
        const Api = {
            start: (session) =>
                $.ajax({
                    url: '/api/WhatsApp/start',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ session }),
                }),
            status: (session) =>
                $.ajax({
                    url: '/api/WhatsApp/status',
                    type: 'GET',
                    data: { session },
                }),
            qr: (session) =>
                $.ajax({
                    url: '/api/WhatsApp/qr',
                    type: 'GET',
                    data: { session },
                }),
            sendText: (session, phoneE164, message) =>
                $.ajax({
                    url: '/api/WhatsApp/send-text',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ session, phoneE164, message }),
                }),
            sendMedia: (session, phoneE164, fileName, base64Data, caption) =>
                $.ajax({
                    url: '/api/WhatsApp/send-media',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({
                        session,
                        phoneE164,
                        fileName,
                        base64Data,
                        caption,
                    }),
                }),
        };

        function getSession() {
            try {
                const s = (
                    document.getElementById('waSession')?.value || ''
                ).trim();
                return s || null;
            } catch (error) {
                Alerta.TratamentoErroComLinha(FILE, 'getSession', error);
                return null;
            }
        }

        function setQrMessage(text) {
            try {
                const elImg = document.getElementById('qrImg');
                const elMsg = document.getElementById('qrMsg');
                if (elImg) elImg.style.display = 'none';
                if (elMsg) {
                    elMsg.style.display = 'block';
                    elMsg.textContent = text || '';
                }
            } catch (error) {
                Alerta.TratamentoErroComLinha(FILE, 'setQrMessage', error);
            }
        }

        async function onStart() {
            try {
                const session = getSession();
                AppToast.show('Amarelo', 'Iniciando sessão...', 2000);
                const r = await Api.start(session);
                if (r?.success) {
                    AppToast.show(
                        'Verde',
                        r.message || 'Sessão iniciada. Cheque o QR.',
                        3000,
                    );
                } else {
                    AppToast.show(
                        'Vermelho',
                        r?.message || 'Falha ao iniciar sessão',
                        4000,
                    );
                }
            } catch (error) {
                Alerta.TratamentoErroComLinha(FILE, 'onStart', error);
                AppToast.show('Vermelho', 'Erro ao iniciar sessão', 4000);
            }
        }

        async function onStatus() {
            try {
                const session = getSession();
                const r = await Api.status(session);
                const status = (
                    r?.status ||
                    r?.Status ||
                    'UNKNOWN'
                ).toUpperCase();

                if (status === 'QRCODE') {
                    AppToast.show(
                        'Amarelo',
                        'QR disponível. Escaneie no celular.',
                        3000,
                    );
                    await loadQr();
                } else if (status === 'CONNECTED') {
                    const el = document.getElementById('qrImg');
                    if (el) el.style.display = 'none';
                    setQrMessage('Conectado ✅');
                    AppToast.show('Verde', 'Sessão conectada!', 3000);
                } else {
                    setQrMessage('Status: ' + status);
                    AppToast.show('Amarelo', 'Status: ' + status, 2500);
                }
            } catch (error) {
                Alerta.TratamentoErroComLinha(FILE, 'onStatus', error);
                AppToast.show('Vermelho', 'Erro ao consultar status', 4000);
            }
        }

        async function loadQr() {
            try {
                const session = getSession();
                const r = await Api.qr(session);
                const b64 = r?.qrcode;
                const img = document.getElementById('qrImg');
                const msg = document.getElementById('qrMsg');
                if (img && b64) {
                    img.src = b64;
                    img.style.display = 'block';
                    if (msg) msg.style.display = 'none';
                } else {
                    setQrMessage(
                        'QR ainda não gerado. Tente novamente em alguns segundos.',
                    );
                }
            } catch (error) {
                Alerta.TratamentoErroComLinha(FILE, 'loadQr', error);
                AppToast.show('Vermelho', 'Erro ao obter QR', 4000);
            }
        }

        function normalizePhone(input) {
            try {
                return (input || '').replace(/\D+/g, '');
            } catch (error) {
                Alerta.TratamentoErroComLinha(FILE, 'normalizePhone', error);
                return input;
            }
        }

        async function onSendText() {
            try {
                const session = getSession();
                const phone = normalizePhone(
                    document.getElementById('waPhone')?.value,
                );
                const message = (
                    document.getElementById('waText')?.value || ''
                ).trim();

                if (!phone || !message) {
                    AppToast.show(
                        'Amarelo',
                        'Informe telefone e mensagem.',
                        3000,
                    );
                    return;
                }

                AppToast.show('Amarelo', 'Enviando mensagem...', 2000);
                const r = await Api.sendText(session, phone, message);
                r?.success
                    ? AppToast.show('Verde', 'Mensagem enviada!', 3000)
                    : AppToast.show(
                          'Vermelho',
                          r?.message || 'Falha ao enviar',
                          4000,
                      );
            } catch (error) {
                Alerta.TratamentoErroComLinha(FILE, 'onSendText', error);
                AppToast.show('Vermelho', 'Erro ao enviar mensagem', 4000);
            }
        }

        let lastFile = null;

        function onPickFile(ev) {
            try {
                const f = ev?.target?.files?.[0];
                lastFile = null;
                document.getElementById('fileName').textContent = '';
                if (!f) return;
                lastFile = f;
                document.getElementById('fileName').textContent =
                    `Arquivo: ${f.name} (${Math.round(f.size / 1024)} KB)`;
            } catch (error) {
                Alerta.TratamentoErroComLinha(FILE, 'onPickFile', error);
            }
        }

        function fileToBase64(file) {
            return new Promise((resolve, reject) => {
                try {
                    const reader = new FileReader();
                    reader.onload = () => resolve(reader.result);
                    reader.onerror = (e) => reject(e);
                    reader.readAsDataURL(file);
                } catch (err) {
                    reject(err);
                }
            });
        }

        async function onSendMedia() {
            try {
                const session = getSession();
                const phone = normalizePhone(
                    document.getElementById('waPhone')?.value,
                );
                if (!phone) {
                    AppToast.show('Amarelo', 'Informe o telefone.', 2500);
                    return;
                }
                if (!lastFile) {
                    AppToast.show('Amarelo', 'Escolha um arquivo antes.', 2500);
                    return;
                }

                AppToast.show('Amarelo', 'Enviando mídia...', 2500);
                const base64 = await fileToBase64(lastFile); // data:*;base64,xxxx
                const caption = (
                    document.getElementById('waText')?.value || ''
                ).trim();
                const r = await Api.sendMedia(
                    session,
                    phone,
                    lastFile.name,
                    base64,
                    caption,
                );
                r?.success
                    ? AppToast.show('Verde', 'Mídia enviada!', 3000)
                    : AppToast.show(
                          'Vermelho',
                          r?.message || 'Falha ao enviar',
                          4000,
                      );
            } catch (error) {
                Alerta.TratamentoErroComLinha(FILE, 'onSendMedia', error);
                AppToast.show('Vermelho', 'Erro ao enviar mídia', 4000);
            }
        }

        // wire-up
        $(document).ready(function () {
            try {
                document
                    .getElementById('btnStart')
                    ?.addEventListener('click', onStart);
                document
                    .getElementById('btnStatus')
                    ?.addEventListener('click', onStatus);
                document
                    .getElementById('btnSendText')
                    ?.addEventListener('click', onSendText);
                document
                    .getElementById('waFile')
                    ?.addEventListener('change', onPickFile);
                document
                    .getElementById('btnSendMedia')
                    ?.addEventListener('click', onSendMedia);
                setQrMessage('Clique em "Iniciar Sessão" e depois "Status".');

                if (window.refreshTooltips) window.refreshTooltips();
            } catch (error) {
                Alerta.TratamentoErroComLinha(FILE, 'document.ready', error);
            }
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha('whatsapp.js', 'IIFE', error);
    }
})();
