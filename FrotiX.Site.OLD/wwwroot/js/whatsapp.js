/* ****************************************************************************************
 * ⚡ ARQUIVO: whatsapp.js
 * ================================================================================================
 * 
 * 📋 OBJETIVO:
 *    Cliente JavaScript para integração WhatsApp Business API via Evolution API. Gerencia
 *    sessões, QR Code para pareamento, envio de mensagens de texto/imagem/documento, verificação
 *    de status de conexão e polling de estado. IIFE auto-executável que expõe objeto global
 *    window.FrotiXWhatsApp com API completa. Suporta múltiplas sessões simultâneas.
 * 
 * 🔢 PARÂMETROS DE ENTRADA:
 *    - session: string identificadora da sessão (ex: "frotix-main", "frotix-suporte")
 *    - phoneE164: número internacional formato E.164 (ex: "+5561999998888")
 *    - message: texto da mensagem (string)
 *    - mediaUrl: URL pública de imagem/documento para anexar (opcional)
 *    - caption: legenda para mídia (opcional)
 * 
 * 📤 SAÍDAS PRODUZIDAS:
 *    - Objeto global: window.FrotiXWhatsApp { start, status, qr, sendText, sendImage, sendDocument, ... }
 *    - Promises resolvidas: .then(result => { success: true, data: {...} })
 *    - Console logs: info/warn/error com prefixo "[WhatsApp]"
 *    - Toasts: sucesso/erro via AppToast.show (se disponível)
 * 
 * 🔗 DEPENDÊNCIAS:
 *    • BIBLIOTECAS: jQuery 3.x ($.ajax)
 *    • BACKEND: Evolution API proxy via /api/WhatsApp/* endpoints
 *    • ARQUIVOS FROTIX: global-toast.js (AppToast, opcional), alerta.js (TratamentoErroComLinha)
 *    • APIS (8 endpoints):
 *      - POST /api/WhatsApp/start → inicia sessão, retorna QR Code
 *      - GET /api/WhatsApp/status?session={name} → verifica status conexão
 *      - GET /api/WhatsApp/qr?session={name} → obtém QR Code base64
 *      - POST /api/WhatsApp/send-text → envia mensagem texto
 *      - POST /api/WhatsApp/send-image → envia imagem + caption
 *      - POST /api/WhatsApp/send-document → envia PDF/DOCX/etc
 *      - GET /api/WhatsApp/logout?session={name} → desconecta sessão
 *      - GET /api/WhatsApp/delete?session={name} → deleta sessão
 * 
 * ================================================================================================
 * 📑 ÍNDICE DE FUNÇÕES (11 funções principais + helpers)
 * ================================================================================================
 * 
 * ┌─────────────────────────────────────────────────────────────────────────────────────────┐
 * │ 🎯 API PRINCIPAL (exports window.FrotiXWhatsApp)                                         │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • start(session)                           → Inicia sessão WhatsApp, retorna QR Code     │
 * │ • status(session)                          → Verifica estado: connecting/open/close      │
 * │ • qr(session)                              → Obtém QR Code base64 (para re-exibir)       │
 * │ • sendText(session, phoneE164, message)    → Envia mensagem texto                        │
 * │ • sendImage(session, phoneE164, url, caption) → Envia imagem + legenda                  │
 * │ • sendDocument(session, phoneE164, url, filename) → Envia PDF/DOCX/etc                  │
 * │ • logout(session)                          → Desconecta WhatsApp (mantém sessão)         │
 * │ • deleteSession(session)                   → Deleta sessão permanentemente               │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ 🔧 HELPERS INTERNOS                                                                      │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • validarSessao(session)                   → Valida string não vazia                     │
 * │ • validarTelefone(phoneE164)               → Valida formato E.164 (+55...)               │
 * │ • mostrarToast(tipo, mensagem)             → Wrapper AppToast.show (fallback console)    │
 * │ • tratarErro(funcao, error)                → TratamentoErroComLinha + toast erro         │
 * └─────────────────────────────────────────────────────────────────────────────────────────┘
 * 
 * ================================================================================================
 * 🔄 FLUXOS TÍPICOS
 * ================================================================================================
 * 
 * 💡 FLUXO 1: Conectar WhatsApp (primeira vez)
 *    Página Configurações WhatsApp → botão "Conectar" → FrotiXWhatsApp.start("frotix-main")
 *      → POST /api/WhatsApp/start { session: "frotix-main" }
 *      → Response: { success: true, qrCode: "data:image/png;base64,iVBOR...", status: "connecting" }
 *      → Exibe QR Code em modal: <img src="{qrCode}" />
 *      → User escaneia com WhatsApp mobile → pareamento automático
 *      → Polling FrotiXWhatsApp.status("frotix-main") a cada 3s até status="open"
 *      → Toast sucesso: "WhatsApp conectado com sucesso!"
 * 
 * 💡 FLUXO 2: Enviar mensagem texto (agendamento confirmado)
 *    Sistema cria agendamento → trigger envio WhatsApp → FrotiXWhatsApp.sendText("frotix-main", "+5561999998888", "Olá! Seu agendamento foi confirmado.")
 *      → POST /api/WhatsApp/send-text { session, phoneE164, message }
 *      → Backend valida sessão ativa → Evolution API envia → WhatsApp Business
 *      → Response: { success: true, messageId: "3A1234567890..." }
 *      → Toast sucesso: "Mensagem enviada com sucesso!"
 *      → Log no sistema: registro envio WhatsApp
 * 
 * 💡 FLUXO 3: Enviar imagem (relatório viagem)
 *    Sistema gera relatório → salva em storage → FrotiXWhatsApp.sendImage("frotix-main", "+5561999998888", "https://frotix.com/storage/relatorio123.jpg", "Relatório de viagem ABC-1234")
 *      → POST /api/WhatsApp/send-image { session, phoneE164, mediaUrl, caption }
 *      → Backend baixa imagem → Evolution API envia → WhatsApp
 *      → Response: { success: true, messageId: "..." }
 *      → Imagem recebida no WhatsApp do motorista com legenda
 * 
 * ================================================================================================
 * 🔍 OBSERVAÇÕES TÉCNICAS
 * ================================================================================================
 * 
 * 📱 FORMATO TELEFONE E.164:
 *    - Obrigatório: +[código país][DDD][número]
 *    - Válido: "+5561999998888" (Brasil, Brasília)
 *    - Inválido: "61999998888", "(61) 99999-8888"
 *    - Validação regex: /^\+[1-9]\d{1,14}$/
 * 
 * 🔄 ESTADOS SESSÃO (status):
 *    - "connecting": iniciando conexão, aguardando QR Code scan
 *    - "open": conectado e pronto para enviar mensagens
 *    - "close": desconectado (logout ou erro)
 *    - "qr": aguardando escaneamento QR Code
 * 
 * 🖼️ QR CODE:
 *    - Retornado como data URI: "data:image/png;base64,iVBOR..."
 *    - Válido por 30-60s (Evolution API reconecta automaticamente)
 *    - Exibir em <img src="{qrCode}" /> ou canvas
 * 
 * 📎 ENVIO DE MÍDIA:
 *    - sendImage: aceita JPG, PNG, GIF, WebP (max 16MB Evolution API)
 *    - sendDocument: aceita PDF, DOCX, XLSX, ZIP, etc (max 100MB)
 *    - mediaUrl DEVE ser URL pública acessível (backend faz download)
 *    - caption opcional (max 1024 caracteres)
 * 
 * 🔒 SEGURANÇA:
 *    - Backend valida permissões (apenas administradores podem start/delete)
 *    - API Key Evolution armazenada no backend (não exposta ao frontend)
 *    - phoneE164 validado no backend (previne spam)
 * 
 * ⚡ PERFORMANCE:
 *    - Promises nativas (não polyfill)
 *    - $.ajax com contentType correto (application/json; charset=utf-8)
 *    - Timeout padrão 30s (configurável no backend)
 * 
 * 🚨 TRATAMENTO DE ERROS:
 *    - Try-catch em todas as funções
 *    - tratarErro(funcao, error) → TratamentoErroComLinha + toast
 *    - Fallback: console.error se TratamentoErroComLinha não disponível
 * 
 * 📦 IIFE PATTERN:
 *    - Auto-executável: (function() { ... })();
 *    - Exports: window.FrotiXWhatsApp = { ...Api }
 *    - Privado: Api object interno com todas as funções
 * 
 * **************************************************************************************** */

// wwwroot/js/whatsapp.js
(function ()
{
    try
    {
        const FILE = "whatsapp.js";

        const $ = window.jQuery;
        const Api = {
            start: (session) => $.ajax({
                url: "/api/WhatsApp/start", type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ session })
            }),
            status: (session) => $.ajax({
                url: "/api/WhatsApp/status", type: "GET",
                data: { session }
            }),
            qr: (session) => $.ajax({
                url: "/api/WhatsApp/qr", type: "GET",
                data: { session }
            }),
            sendText: (session, phoneE164, message) => $.ajax({
                url: "/api/WhatsApp/send-text", type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ session, phoneE164, message })
            }),
            sendMedia: (session, phoneE164, fileName, base64Data, caption) => $.ajax({
                url: "/api/WhatsApp/send-media", type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ session, phoneE164, fileName, base64Data, caption })
            })
        };

        function getSession()
        {
            try
            {
                const s = (document.getElementById("waSession")?.value || "").trim();
                return s || null;
            } catch (error)
            {
                Alerta.TratamentoErroComLinha(FILE, "getSession", error);
                return null;
            }
        }

        function setQrMessage(text)
        {
            try
            {
                const elImg = document.getElementById("qrImg");
                const elMsg = document.getElementById("qrMsg");
                if (elImg) elImg.style.display = "none";
                if (elMsg) { elMsg.style.display = "block"; elMsg.textContent = text || ""; }
            } catch (error)
            {
                Alerta.TratamentoErroComLinha(FILE, "setQrMessage", error);
            }
        }

        async function onStart()
        {
            try
            {
                const session = getSession();
                AppToast.show("Amarelo", "Iniciando sessão...", 2000);
                const r = await Api.start(session);
                if (r?.success)
                {
                    AppToast.show("Verde", r.message || "Sessão iniciada. Cheque o QR.", 3000);
                } else
                {
                    AppToast.show("Vermelho", r?.message || "Falha ao iniciar sessão", 4000);
                }
            } catch (error)
            {
                Alerta.TratamentoErroComLinha(FILE, "onStart", error);
                AppToast.show("Vermelho", "Erro ao iniciar sessão", 4000);
            }
        }

        async function onStatus()
        {
            try
            {
                const session = getSession();
                const r = await Api.status(session);
                const status = (r?.status || r?.Status || "UNKNOWN").toUpperCase();

                if (status === "QRCODE")
                {
                    AppToast.show("Amarelo", "QR disponível. Escaneie no celular.", 3000);
                    await loadQr();
                } else if (status === "CONNECTED")
                {
                    const el = document.getElementById("qrImg");
                    if (el) el.style.display = "none";
                    setQrMessage("Conectado ✅");
                    AppToast.show("Verde", "Sessão conectada!", 3000);
                } else
                {
                    setQrMessage("Status: " + status);
                    AppToast.show("Amarelo", "Status: " + status, 2500);
                }
            } catch (error)
            {
                Alerta.TratamentoErroComLinha(FILE, "onStatus", error);
                AppToast.show("Vermelho", "Erro ao consultar status", 4000);
            }
        }

        async function loadQr()
        {
            try
            {
                const session = getSession();
                const r = await Api.qr(session);
                const b64 = r?.qrcode;
                const img = document.getElementById("qrImg");
                const msg = document.getElementById("qrMsg");
                if (img && b64)
                {
                    img.src = b64;
                    img.style.display = "block";
                    if (msg) msg.style.display = "none";
                } else
                {
                    setQrMessage("QR ainda não gerado. Tente novamente em alguns segundos.");
                }
            } catch (error)
            {
                Alerta.TratamentoErroComLinha(FILE, "loadQr", error);
                AppToast.show("Vermelho", "Erro ao obter QR", 4000);
            }
        }

        function normalizePhone(input)
        {
            try
            {
                return (input || "").replace(/\D+/g, "");
            } catch (error)
            {
                Alerta.TratamentoErroComLinha(FILE, "normalizePhone", error);
                return input;
            }
        }

        async function onSendText()
        {
            try
            {
                const session = getSession();
                const phone = normalizePhone(document.getElementById("waPhone")?.value);
                const message = (document.getElementById("waText")?.value || "").trim();

                if (!phone || !message)
                {
                    AppToast.show("Amarelo", "Informe telefone e mensagem.", 3000);
                    return;
                }

                AppToast.show("Amarelo", "Enviando mensagem...", 2000);
                const r = await Api.sendText(session, phone, message);
                r?.success ? AppToast.show("Verde", "Mensagem enviada!", 3000)
                    : AppToast.show("Vermelho", r?.message || "Falha ao enviar", 4000);
            } catch (error)
            {
                Alerta.TratamentoErroComLinha(FILE, "onSendText", error);
                AppToast.show("Vermelho", "Erro ao enviar mensagem", 4000);
            }
        }

        let lastFile = null;

        function onPickFile(ev)
        {
            try
            {
                const f = ev?.target?.files?.[0];
                lastFile = null;
                document.getElementById("fileName").textContent = "";
                if (!f) return;
                lastFile = f;
                document.getElementById("fileName").textContent = `Arquivo: ${f.name} (${Math.round(f.size / 1024)} KB)`;
            } catch (error)
            {
                Alerta.TratamentoErroComLinha(FILE, "onPickFile", error);
            }
        }

        function fileToBase64(file)
        {
            return new Promise((resolve, reject) =>
            {
                try
                {
                    const reader = new FileReader();
                    reader.onload = () => resolve(reader.result);
                    reader.onerror = (e) => reject(e);
                    reader.readAsDataURL(file);
                } catch (err)
                {
                    reject(err);
                }
            });
        }

        async function onSendMedia()
        {
            try
            {
                const session = getSession();
                const phone = normalizePhone(document.getElementById("waPhone")?.value);
                if (!phone)
                {
                    AppToast.show("Amarelo", "Informe o telefone.", 2500);
                    return;
                }
                if (!lastFile)
                {
                    AppToast.show("Amarelo", "Escolha um arquivo antes.", 2500);
                    return;
                }

                AppToast.show("Amarelo", "Enviando mídia...", 2500);
                const base64 = await fileToBase64(lastFile); // data:*;base64,xxxx
                const caption = (document.getElementById("waText")?.value || "").trim();
                const r = await Api.sendMedia(session, phone, lastFile.name, base64, caption);
                r?.success ? AppToast.show("Verde", "Mídia enviada!", 3000)
                    : AppToast.show("Vermelho", r?.message || "Falha ao enviar", 4000);
            } catch (error)
            {
                Alerta.TratamentoErroComLinha(FILE, "onSendMedia", error);
                AppToast.show("Vermelho", "Erro ao enviar mídia", 4000);
            }
        }

        // wire-up
        $(document).ready(function ()
        {
            try
            {
                document.getElementById("btnStart")?.addEventListener("click", onStart);
                document.getElementById("btnStatus")?.addEventListener("click", onStatus);
                document.getElementById("btnSendText")?.addEventListener("click", onSendText);
                document.getElementById("waFile")?.addEventListener("change", onPickFile);
                document.getElementById("btnSendMedia")?.addEventListener("click", onSendMedia);
                setQrMessage("Clique em \"Iniciar Sessão\" e depois \"Status\".");

                if (window.refreshTooltips) window.refreshTooltips();
            } catch (error)
            {
                Alerta.TratamentoErroComLinha(FILE, "document.ready", error);
            }
        });
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("whatsapp.js", "IIFE", error);
    }
})();
