# Pages/Viagens/Upsert.cshtml

**Mudanca:** GRANDE | **+854** linhas | **-937** linhas

---

```diff
--- JANEIRO: Pages/Viagens/Upsert.cshtml
+++ ATUAL: Pages/Viagens/Upsert.cshtml
@@ -4,57 +4,59 @@
 @using Syncfusion.EJ2;
 @using Syncfusion.Data;
 @using Syncfusion.EJ2.DocumentEditor;
+@using Kendo.Mvc.UI;
+@using Kendo.Mvc.Extensions;
 
 @model FrotiX.Pages.Viagens.UpsertModel
 
 @{
 
-    bool hasBytes = Model.ViagemObj.Viagem.FichaVistoria != null;
-
-    string defaultImg = Url.Content("~/Images/FichaAmarelaNova.JPG")
-    + "?t=" + DateTime.Now.Ticks;
-
-    string imgSrc;
-
-    if (hasBytes)
-    {
-        imgSrc = "data:image/jpeg;base64,"
-                                         + Convert.ToBase64String(Model.ViagemObj.Viagem.FichaVistoria);
-    }
-    else
-    {
-        imgSrc = defaultImg;
-    }
+bool hasBytes = Model.ViagemObj.Viagem.FichaVistoria != null;
+
+string defaultImg = Url.Content("~/Images/FichaAmarelaNova.JPG")
+                    + "?t=" + DateTime.Now.Ticks;
+
+string imgSrc;
+
+if (hasBytes)
+{
+imgSrc = "data:image/jpeg;base64,"
+         + Convert.ToBase64String(Model.ViagemObj.Viagem.FichaVistoria);
 }
+else
+{
+imgSrc = defaultImg;
+}
+}
 
 @{
-    string conteudoSfdt = Model.ViagemObj.Viagem.DescricaoViagemWord != null
-                    ? System.Text.Encoding.UTF8.GetString(Model.ViagemObj.Viagem.DescricaoViagemWord)
+string conteudoSfdt = Model.ViagemObj.Viagem.DescricaoViagemWord != null
+    ? System.Text.Encoding.UTF8.GetString(Model.ViagemObj.Viagem.DescricaoViagemWord)
     : "";
 }
 
 @Html.AntiForgeryToken()
 
 @{
-    ViewData["Title"] = "Viagens";
-    ViewData["PageName"] = "viagens_upsert";
-    ViewData["Heading"] = "";
-    ViewData["Category1"] = "Cadastros";
-    ViewData["PageIcon"] = "fa-duotone fa-route";
-
-    ViewData["fieldsMotorista"] = new ComboBoxFieldSettings
-    {
-        Text = "Nome",
-        Value = "MotoristaId"
-    };
-
-    ViewData["itemTemplate"] = @"
+ViewData["Title"] = "Viagens";
+ViewData["PageName"] = "viagens_upsert";
+ViewData["Heading"] = "";
+ViewData["Category1"] = "Cadastros";
+ViewData["PageIcon"] = "fa-duotone fa-route";
+
+ViewData["fieldsMotorista"] = new ComboBoxFieldSettings
+{
+    Text = "Nome",
+    Value = "MotoristaId"
+};
+
+ViewData["itemTemplate"] = @"
     <div class='d-flex align-items-center'>
         <img src='${Foto}' style='width:40px; height:40px; border-radius:50%; margin-right:10px;' />
         <span>${Nome}</span>
     </div>";
 
-    ViewData["valueTemplate"] = @"
+ViewData["valueTemplate"] = @"
     <div class='d-flex align-items-center'>
         <img src='${Foto}' style='width:30px; height:30px; border-radius:50%; margin-right:10px;' />
         <span>${Nome}</span>
@@ -63,7 +65,7 @@
 
 @section HeadBlock {
     <style>
-        /* ======== OUTLINE BRANCO NO BOTÃO DO HEADER (Padrão FrotiX) ======== */
+/* ======== OUTLINE BRANCO NO BOTÃO DO HEADER (Padrão FrotiX) ======== */
         .ftx-card-header .btn-fundo-laranja {
             outline: 2px solid rgba(255, 255, 255, 0.5) !important;
             outline-offset: 1px;
@@ -74,6 +76,7 @@
             outline: 2px solid rgba(255, 255, 255, 0.8) !important;
             outline-offset: 2px;
         }
+
     </style>
     <link href="~/css/ftx-card-styled.css" rel="stylesheet" asp-append-version="true" />
 
@@ -106,22 +109,7 @@
 }
 
 <script>
-    /**
-     * ═══════════════════════════════════════════════════════════════════════════
-     * UPSERT VIAGEM - FORMULÁRIO PRINCIPAL
-     * ═══════════════════════════════════════════════════════════════════════════
-     * @@description Formulário completo de criação / edição de viagens.
-     * Integra múltiplos combos(motorista, veículo, requisitante, destinos),
-     * validação cruzada e editor de texto rico.
-     * @@requires jQuery, Syncfusion(ComboBox, RichTextEditor), Kendo UI
-        * @@file Viagens / Upsert.cshtml
-        */
-
-    /**
-     * Previne submissão do form ao pressionar Enter
-     * @@param { Event } e - Evento de teclado
-        * @@description Bloqueia Enter exceto em divs contenteditable
-            */
+
     function stopEnterSubmitting(e) {
         try {
             const evt = e || window.event;
@@ -138,11 +126,6 @@
         }
     }
 
-    /**
-     * Handler de clique na toolbar do RichTextEditor
-     * @@param { Object } e - Evento com item.id indicando botão clicado
-        * @@description Injeta token AntiForgery para upload de imagens
-            */
     function toolbarClick(e) {
         try {
             if (e && e.item && e.item.id && e.item.id.indexOf("Image") >= 0) {
@@ -163,56 +146,50 @@
         }
     }
 
-    /**
-     * Callback de criação do ComboBox de Motoristas
-     * @@description Configura templates personalizados com foto do motorista
-        */
     function onCmbMotoristaCreated() {
-            try {
-                const combo = document.getElementById('cmbMotorista').ej2_instances[0];
-
-                combo.itemTemplate = function (data) {
-                    let imgSrc = (data.Foto && data.Foto.startsWith('data:image'))
-                        ? data.Foto
-                        : '/images/barbudo.jpg';
-                    return `
+        try {
+            const combo = document.getElementById('cmbMotorista').ej2_instances[0];
+
+            combo.itemTemplate = function (data) {
+                let imgSrc = (data.Foto && data.Foto.startsWith('data:image'))
+                    ? data.Foto
+                    : '/images/barbudo.jpg';
+                return `
                 <div class="d-flex align-items-center">
                     <img src="${imgSrc}" alt="Foto" style="height:40px; width:40px; border-radius:50%; margin-right:10px;" />
                     <span>${data.Nome}</span>
                 </div>`;
-                };
-
-                combo.valueTemplate = function (data) {
-                    if (!data) return '';
-                    let imgSrc = (data.Foto && data.Foto.startsWith('data:image'))
-                        ? data.Foto
-                        : '/images/barbudo.jpg';
-                    return `
+            };
+
+            combo.valueTemplate = function (data) {
+                if (!data) return '';
+                let imgSrc = (data.Foto && data.Foto.startsWith('data:image'))
+                    ? data.Foto
+                    : '/images/barbudo.jpg';
+                return `
                 <div class="d-flex align-items-center">
                     <img src="${imgSrc}" alt="Foto" style="height:30px; width:30px; border-radius:50%; margin-right:10px;" />
                     <span>${data.Nome}</span>
                 </div>`;
-                };
-            } catch (error) {
-                Alerta.TratamentoErroComLinha("Upsert.cshtml", "onCmbMotoristaCreated", error);
-            }
-        }
-
-    /**
-     * Handler de mudança de Finalidade da viagem
-     * @@description Exibe / oculta seletor de Evento conforme tipo de viagem
-        */
+            };
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("Upsert.cshtml", "onCmbMotoristaCreated", error);
+        }
+    }
+
     function lstFinalidade_Change() {
         try {
             try {
-                const finalidade = document.getElementById('ddtFinalidade').ej2_instances[0]?.value?.[0];
-                const lstEvento = document.getElementById("ddtEventos")?.ej2_instances?.[0];
+                const finalidadeWidget = $("#ddtFinalidade").data("kendoDropDownList");
+                const finalidade = finalidadeWidget?.dataItem()?.Descricao;
+                const lstEventoWidget = $("#ddtEventos").data("kendoDropDownTree");
                 const btnEvento = document.getElementById("btnEvento");
 
+                $("#hiddenFinalidade").val(finalidadeWidget?.value() || "");
+
                 if (finalidade === 'Evento') {
-                    if (lstEvento) {
-                        lstEvento.enabled = true;
-                        lstEvento.dataBind();
+                    if (lstEventoWidget) {
+                        lstEventoWidget.enable(true);
                     }
                     if (btnEvento) {
                         btnEvento.style.display = "block";
@@ -222,9 +199,8 @@
                     }
                     $(".esconde-diveventos").show();
                 } else {
-                    if (lstEvento) {
-                        lstEvento.enabled = false;
-                        lstEvento.dataBind();
+                    if (lstEventoWidget) {
+                        lstEventoWidget.enable(false);
                     }
                     if (btnEvento) btnEvento.style.display = "none";
                     $(".esconde-diveventos").hide();
@@ -237,10 +213,6 @@
         }
     }
 
-    /**
-     * Handler de mudança de Motorista
-     * @@description Desabilita seletor de Setor ao selecionar motorista
-        */
     function MotoristaValueChange() {
         try {
             try {
@@ -260,11 +232,6 @@
         }
     }
 
-    /**
-     * Controla habilitação/desabilitação da seção de ocorrências
-     * @@param { string } veiculoId - GUID do veículo selecionado
-        * @@description Habilita seção apenas se veículo selecionado e viagem não finalizada
-            */
     function controlarSecaoOcorrencias(veiculoId) {
         try {
             const secao = document.getElementById('secaoOcorrenciasUpsert');
@@ -279,10 +246,10 @@
             }
 
             const temVeiculo = veiculoId &&
-                veiculoId !== '' &&
-                veiculoId !== '00000000-0000-0000-0000-000000000000' &&
-                veiculoId !== null &&
-                veiculoId !== undefined;
+                               veiculoId !== '' &&
+                               veiculoId !== '00000000-0000-0000-0000-000000000000' &&
+                               veiculoId !== null &&
+                               veiculoId !== undefined;
 
             if (temVeiculo) {
 
@@ -300,10 +267,6 @@
         }
     }
 
-    /**
-     * Handler de mudança de Veículo
-     * @@description Aciona controle de seção de ocorrências
-        */
     function VeiculoValueChange() {
         try {
             const cmbVeiculo = document.getElementById('cmbVeiculo');
@@ -320,10 +283,6 @@
         }
     }
 
-    /**
-     * Handler de mudança de Requisitante (viagem normal)
-     * @@description Placeholder para lógica futura de integração
-        */
     function RequisitanteValueChange() {
         try {
             try {
@@ -338,10 +297,6 @@
         }
     }
 
-    /**
-     * Handler de mudança de Requisitante (viagem de evento)
-     * @@description Placeholder para lógica futura de integração com eventos
-        */
     function RequisitanteEventoValueChange() {
         try {
             try {
@@ -356,42 +311,33 @@
         }
     }
 
-    /**
-     * Callback de criação do ComboBox de Veículos
-     * @@description Configura templates com imagem do veículo
-        */
     function onVeiculoComboCreated() {
+        try {
             try {
-                try {
-                    var veiculoCombo = document.getElementById('cmbVeiculo').ej2_instances[0];
-
-                    veiculoCombo.itemTemplate = function (data) {
-                        return '<div class="d-flex align-items-center">' +
-                            '<img src="' + (data.Imagem || '/Images/carro.jpg') + '" alt="Imagem" style="height:30px; width:30px; border-radius:5px; margin-right:10px;" />' +
-                            '<span>' + data.Descricao + '</span>' +
-                            '</div>';
-                    };
-
-                    veiculoCombo.valueTemplate = function (data) {
-                        if (!data) return '';
-                        return '<div class="d-flex align-items-center">' +
-                            '<img src="' + (data.Imagem || '/Images/carro.jpg') + '" alt="Imagem" style="height:30px; width:30px; border-radius:5px; margin-right:10px;" />' +
-                            '<span>' + data.Descricao + '</span>' +
-                            '</div>';
-                    };
-                } catch (error) {
-                    TratamentoErroComLinha("Viagem_050", "onVeiculoComboCreated", error);
-                }
+                var veiculoCombo = document.getElementById('cmbVeiculo').ej2_instances[0];
+
+                veiculoCombo.itemTemplate = function (data) {
+                    return '<div class="d-flex align-items-center">' +
+                        '<img src="' + (data.Imagem || '/Images/carro.jpg') + '" alt="Imagem" style="height:30px; width:30px; border-radius:5px; margin-right:10px;" />' +
+                        '<span>' + data.Descricao + '</span>' +
+                        '</div>';
+                };
+
+                veiculoCombo.valueTemplate = function (data) {
+                    if (!data) return '';
+                    return '<div class="d-flex align-items-center">' +
+                        '<img src="' + (data.Imagem || '/Images/carro.jpg') + '" alt="Imagem" style="height:30px; width:30px; border-radius:5px; margin-right:10px;" />' +
+                        '<span>' + data.Descricao + '</span>' +
+                        '</div>';
+                };
             } catch (error) {
-                Alerta.TratamentoErroComLinha("Upsert.cshtml", "onVeiculoComboCreated", error);
+                TratamentoErroComLinha("Viagem_050", "onVeiculoComboCreated", error);
             }
-        }
-
-    /**
-     * Handler de mudança de Motorista com foto
-     * @@param { Object } args - Evento com itemData contendo MotoristaId
-        * @@description Busca e exibe foto do motorista selecionado
-            */
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("Upsert.cshtml", "onVeiculoComboCreated", error);
+        }
+    }
+
     function onMotoristaChange(args) {
         try {
             const motoristaId = args?.itemData?.MotoristaId;
@@ -412,11 +358,6 @@
         }
     }
 
-    /**
-     * Preview de imagem antes do upload
-     * @@param { HTMLInputElement } input - Campo file com imagem selecionada
-        * @@description Usa FileReader para mostrar preview em imgViewer
-            */
     function VisualizaImagem(input) {
         try {
             if (!input.files || !input.files[0]) return;
@@ -435,33 +376,19 @@
         }
     }
 
-    /**
-     * Atualiza imagem no modal de zoom
-     * @@description Copia src de imgViewerItem para imgZoomed
-        */
-    function atualizarImagemModal() {
+    function atualizarImagemModal()
+    {
         const imgSrc = document.getElementById('imgViewerItem').src;
         const modalImg = document.getElementById('imgZoomed');
-        if (modalImg) {
+        if (modalImg)
+        {
             modalImg.src = imgSrc;
         }
     }
 
 </script>
 <script>
-    /**
-     * ═══════════════════════════════════════════════════════════════════════════
-     * VALIDAÇÃO FUZZY DE DUPLICADOS - DESTINOS ORIGEM/DESTINO
-     * ═══════════════════════════════════════════════════════════════════════════
-     * @@description Sistema de detecção de destinos similares usando Levenshtein
-        * para evitar duplicação acidental de origem / destino.
-     */
-
-    /**
-     * Normaliza texto para comparação fuzzy
-     * @@param { string } str - Texto a normalizar
-        * @@returns { string } Texto normalizado(lowercase, sem acentos, sem espaços extras)
-            */
+
     function normalize(str) {
         try {
             if (!str) return "";
@@ -477,13 +404,6 @@
             Alerta.TratamentoErroComLinha("Upsert.cshtml", "normalize", error);
         }
     }
-
-    /**
-     * Calcula distância de Levenshtein entre duas strings
-     * @@param { string } a - Primeira string
-        * @@param { string } b - Segunda string
-            * @@returns { number } Número de edições necessárias
-                */
     function levenshteinRaw(a, b) {
         try {
             const n = a.length, m = b.length;
@@ -507,13 +427,6 @@
             Alerta.TratamentoErroComLinha("Upsert.cshtml", "levenshteinRaw", error);
         }
     }
-
-    /**
-     * Calcula similaridade entre duas strings (0 a 1)
-     * @@param { string } a - Primeira string
-        * @@param { string } b - Segunda string
-            * @@returns { number } Valor entre 0(diferentes) e 1(idênticas)
-                */
     function similarity(a, b) {
         try {
             const na = normalize(a), nb = normalize(b);
@@ -526,17 +439,6 @@
         }
     }
 
-    /**
-     * ═══════════════════════════════════════════════════════════════════════════
-     * HELPERS SYNCFUSION EJ2
-     * ═══════════════════════════════════════════════════════════════════════════
-     */
-
-    /**
-     * Obtém instância de ComboBox/DropDownTree por ID
-     * @@param { string } id - ID do elemento
-        * @@returns { Object| null} Instância EJ2 ou null
-            */
     function getCombo(id) {
         try {
             const host = document.getElementById(id);
@@ -545,13 +447,6 @@
             Alerta.TratamentoErroComLinha("Upsert.cshtml", "getCombo", error);
         }
     }
-
-    /**
-     * Extrai textos de array de itens do combo
-     * @@param { Array } items - Array de objetos ou strings
-        * @@param { Object } fields - Configuração de campos(text, value)
-            * @@returns { Array<string>} Array de textos extraídos
-                */
     function extractTexts(items, fields) {
         try {
             const textField = fields?.text;
@@ -570,12 +465,6 @@
             Alerta.TratamentoErroComLinha("Upsert.cshtml", "extractTexts", error);
         }
     }
-
-    /**
-     * Obtém cache de textos master de um combo
-     * @@param { Object } combo - Instância EJ2 do combo
-        * @@returns { Array<string>} Lista de textos disponíveis
-            */
     function getMasterTexts(combo) {
         try {
             if (Array.isArray(combo.__masterTexts) && combo.__masterTexts.length) return combo.__masterTexts;
@@ -590,35 +479,24 @@
             Alerta.TratamentoErroComLinha("Upsert.cshtml", "getMasterTexts", error);
         }
     }
-
-    /**
-     * Conecta cache de textos ao combo para validação
-     * @@param { Object } combo - Instância EJ2 do combo
-        */
     function wireMasterCache(combo) {
-            try {
+        try {
+            if (!combo.__masterTexts || !combo.__masterTexts.length) {
+                combo.__masterTexts = getMasterTexts(combo);
+            }
+            const prev = combo.dataBound;
+            combo.dataBound = function () {
+                if (typeof prev === "function") prev.apply(combo, arguments);
                 if (!combo.__masterTexts || !combo.__masterTexts.length) {
-                    combo.__masterTexts = getMasterTexts(combo);
+                    combo.__masterTexts = extractTexts(combo.listData, combo.fields);
                 }
-                const prev = combo.dataBound;
-                combo.dataBound = function () {
-                    if (typeof prev === "function") prev.apply(combo, arguments);
-                    if (!combo.__masterTexts || !combo.__masterTexts.length) {
-                        combo.__masterTexts = extractTexts(combo.listData, combo.fields);
-                    }
-                };
-                combo.dataBind();
-            } catch (error) {
-                Alerta.TratamentoErroComLinha("Upsert.cshtml", "wireMasterCache", error);
-            }
-        }
-
-    /**
-     * Exibe alerta informativo
-     * @@param { string } titulo - Título do alerta
-        * @@param { string } texto - Texto do alerta
-            * @@param { string } confirm - Texto do botão de confirmação
-                */
+            };
+            combo.dataBind();
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("Upsert.cshtml", "wireMasterCache", error);
+        }
+    }
+
     function alertInfo(titulo, texto, confirm = "OK") {
         try {
             if (typeof Alerta !== "undefined" && Alerta?.Info) Alerta.Info(titulo, texto, confirm);
@@ -627,13 +505,6 @@
             Alerta.TratamentoErroComLinha("Upsert.cshtml", "alertInfo", error);
         }
     }
-
-    /**
-     * Exibe alerta de advertência
-     * @@param { string } titulo - Título do alerta
-        * @@param { string } texto - Texto do alerta
-            * @@param { string } confirm - Texto do botão de confirmação
-                */
     function alertWarn(titulo, texto, confirm = "OK") {
         try {
             if (typeof Alerta !== "undefined" && Alerta?.Warning) Alerta.Warning(titulo, texto, confirm);
@@ -643,12 +514,6 @@
         }
     }
 
-    /**
-     * Valida duplicados fuzzy na lista de destinos
-     * @@param { Object } combo - Instância EJ2 do combo
-        * @@param { Object } opts - Opções de validação(infoThreshold, warnThreshold, etc.)
-            * @@description Detecta destinos similares digitados manualmente e sugere correção
-                */
     function validarDuplicadoNaLista(combo, opts) {
         try {
             const {
@@ -709,13 +574,6 @@
         }
     }
 
-    /**
-     * Valida cruzamento entre Origem e Destino
-     * @@param { Object } origemCombo - Instância EJ2 do combo de origem
-        * @@param { Object } destinoCombo - Instância EJ2 do combo de destino
-            * @@param { Object } opts - Opções(infoThreshold, warnThreshold)
-                * @@description Alerta se origem e destino são muito similares
-                    */
     function validarCruzado(origemCombo, destinoCombo, opts) {
         try {
             const { infoThreshold = 0.85, warnThreshold = 0.92, confirmarTexto = "OK" } = opts || {};
@@ -740,45 +598,33 @@
         }
     }
 
-    /**
-     * ═══════════════════════════════════════════════════════════════════════════
-     * INICIALIZAÇÃO DOS COMBOS ORIGEM/DESTINO
-     * ═══════════════════════════════════════════════════════════════════════════
-     * @@description IIFE que conecta validação fuzzy aos combos de origem / destino
-        */
-            (function boot() {
-                try {
-            /**
-             * Conecta eventos de blur aos combos para validação
-             * @@param { string } id - ID do combo principal
-                        * @@param { string } peerId - ID do combo pareado(para validação cruzada)
-                            * @@param { Object } opts - Opções de validação
-                                */
-                    function connect(id, peerId, opts) {
-                        const c = getCombo(id);
-                        const p = getCombo(peerId);
-                        if (!c || !c.inputElement) return false;
-                        wireMasterCache(c);
-                        c.inputElement.addEventListener("blur", function () {
-                            validarDuplicadoNaLista(c, opts);
-                            if (p && p.inputElement) validarCruzado(id === "cmbOrigem" ? c : p, id === "cmbOrigem" ? p : c, opts);
-                        });
-                        return true;
-                    }
-                    function tryWire() {
-                        const opts = { infoThreshold: 0.85, warnThreshold: 0.92, autoCanonizar: true };
-                        const okO = connect("cmbOrigem", "cmbDestino", opts);
-                        const okD = connect("cmbDestino", "cmbOrigem", opts);
-                        return okO && okD;
-                    }
-                    if (!tryWire()) {
-                        document.addEventListener("DOMContentLoaded", tryWire, { once: true });
-                        window.addEventListener("load", () => setTimeout(tryWire, 0), { once: true });
-                    }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha("Upsert.cshtml", "boot", error);
-                }
-            })();
+    (function boot() {
+        try {
+            function connect(id, peerId, opts) {
+                const c = getCombo(id);
+                const p = getCombo(peerId);
+                if (!c || !c.inputElement) return false;
+                wireMasterCache(c);
+                c.inputElement.addEventListener("blur", function () {
+                    validarDuplicadoNaLista(c, opts);
+                    if (p && p.inputElement) validarCruzado(id === "cmbOrigem" ? c : p, id === "cmbOrigem" ? p : c, opts);
+                });
+                return true;
+            }
+            function tryWire() {
+                const opts = { infoThreshold: 0.85, warnThreshold: 0.92, autoCanonizar: true };
+                const okO = connect("cmbOrigem", "cmbDestino", opts);
+                const okD = connect("cmbDestino", "cmbOrigem", opts);
+                return okO && okD;
+            }
+            if (!tryWire()) {
+                document.addEventListener("DOMContentLoaded", tryWire, { once: true });
+                window.addEventListener("load", () => setTimeout(tryWire, 0), { once: true });
+            }
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("Upsert.cshtml", "boot", error);
+        }
+    })();
 
     window.Alerta = window.Alerta || {
         Info: (t, x) => alert(`${t}\n\n${x}`),
@@ -810,9 +656,9 @@
         border-radius: 10px;
         padding: 1.25rem 1.5rem;
         margin-bottom: 1.25rem;
-        border: 1px solid rgba(0, 0, 0, 0.06);
+        border: 1px solid rgba(0,0,0,0.06);
         border-left: 4px solid #6c757d;
-        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.04);
+        box-shadow: 0 1px 3px rgba(0,0,0,0.04);
     }
 
     .ftx-section-title {
@@ -822,7 +668,7 @@
         letter-spacing: 0.8px;
         margin-bottom: 1rem;
         padding-bottom: 0.5rem;
-        border-bottom: 1px dashed rgba(0, 0, 0, 0.1);
+        border-bottom: 1px dashed rgba(0,0,0,0.1);
         display: flex;
         align-items: center;
         gap: 0.5rem;
@@ -834,61 +680,26 @@
     }
 
     /* Cores das seções */
-    .ftx-section-periodo {
-        border-left-color: #325d88;
-    }
-
-    .ftx-section-periodo .ftx-section-title {
-        color: #325d88;
-    }
-
-    .ftx-section-trajeto {
-        border-left-color: #2e7d32;
-    }
-
-    .ftx-section-trajeto .ftx-section-title {
-        color: #2e7d32;
-    }
-
-    .ftx-section-evento {
-        border-left-color: #7b1fa2;
-    }
-
-    .ftx-section-evento .ftx-section-title {
-        color: #7b1fa2;
-    }
-
-    .ftx-section-veiculo {
-        border-left-color: #ef6c00;
-    }
-
-    .ftx-section-veiculo .ftx-section-title {
-        color: #ef6c00;
-    }
-
-    .ftx-section-combustivel {
-        border-left-color: #c62828;
-    }
-
-    .ftx-section-combustivel .ftx-section-title {
-        color: #c62828;
-    }
-
-    .ftx-section-solicitante {
-        border-left-color: #00695c;
-    }
-
-    .ftx-section-solicitante .ftx-section-title {
-        color: #00695c;
-    }
-
-    .ftx-section-descricao {
-        border-left-color: #37474f;
-    }
-
-    .ftx-section-descricao .ftx-section-title {
-        color: #37474f;
-    }
+    .ftx-section-periodo { border-left-color: #325d88; }
+    .ftx-section-periodo .ftx-section-title { color: #325d88; }
+
+    .ftx-section-trajeto { border-left-color: #2e7d32; }
+    .ftx-section-trajeto .ftx-section-title { color: #2e7d32; }
+
+    .ftx-section-evento { border-left-color: #7b1fa2; }
+    .ftx-section-evento .ftx-section-title { color: #7b1fa2; }
+
+    .ftx-section-veiculo { border-left-color: #ef6c00; }
+    .ftx-section-veiculo .ftx-section-title { color: #ef6c00; }
+
+    .ftx-section-combustivel { border-left-color: #c62828; }
+    .ftx-section-combustivel .ftx-section-title { color: #c62828; }
+
+    .ftx-section-solicitante { border-left-color: #00695c; }
+    .ftx-section-solicitante .ftx-section-title { color: #00695c; }
+
+    .ftx-section-descricao { border-left-color: #37474f; }
+    .ftx-section-descricao .ftx-section-title { color: #37474f; }
 
     /* ======== Labels e Inputs melhorados ======== */
     .ftx-label {
@@ -912,8 +723,43 @@
         height: 38px !important;
         min-height: 38px !important;
         font-size: 0.9rem !important;
+    }
+
+    /* Correção das bordas dos controles Syncfusion */
+    .e-input-group,
+    .e-input-group.e-control-wrapper,
+    .e-ddl.e-input-group,
+    .e-dropdowntree.e-input-group,
+    .e-ddt.e-input-group {
         border: 1px solid #ced4da !important;
         border-radius: 0.25rem !important;
+    }
+
+    /* Força a linha de borda dos controles Syncfusion */
+    .e-float-line::before,
+    .e-float-line::after {
+        background: #ced4da !important;
+        height: 1px !important;
+    }
+
+    .e-input-group .e-float-line,
+    .e-ddl .e-float-line,
+    .e-dropdowntree .e-float-line,
+    .e-ddt .e-float-line {
+        border-bottom: 1px solid #ced4da !important;
+    }
+
+    .e-input-group:focus-within,
+    .e-ddl.e-input-group:focus-within,
+    .e-dropdowntree.e-input-group:focus-within,
+    .e-ddt.e-input-group:focus-within {
+        border-color: #325d88 !important;
+        box-shadow: 0 0 0 0.2rem rgba(50, 93, 136, 0.15) !important;
+    }
+
+    .e-input-group:focus-within .e-float-line::before,
+    .e-input-group:focus-within .e-float-line::after {
+        background: #325d88 !important;
     }
 
     .ftx-section .form-control:focus {
@@ -956,8 +802,7 @@
     }
 
     .label i {
-        display: none;
-        /* Esconde ícones individuais das labels */
+        display: none; /* Esconde ícones individuais das labels */
     }
 
     input[type=checkbox] {
@@ -1002,17 +847,16 @@
         align-items: center;
         text-decoration: none;
     }
-
-    .btn-vinho:hover,
-    .btn-vinho:focus,
-    .btn-vinho:active,
-    .btn-vinho:focus-visible {
-        background-color: #a71d2a !important;
-        border-color: #a71d2a !important;
-        color: #fff !important;
-        outline: none !important;
-        box-shadow: none !important;
-    }
+        .btn-vinho:hover,
+        .btn-vinho:focus,
+        .btn-vinho:active,
+        .btn-vinho:focus-visible {
+            background-color: #a71d2a !important;
+            border-color: #a71d2a !important;
+            color: #fff !important;
+            outline: none !important;
+            box-shadow: none !important;
+        }
 
     .custom-blue-btn {
         background-color: #6c87a3 !important;
@@ -1023,17 +867,16 @@
         justify-content: center;
         align-items: center;
     }
-
-    .custom-blue-btn:hover,
-    .custom-blue-btn:focus,
-    .custom-blue-btn:active,
-    .custom-blue-btn:focus-visible {
-        background-color: #5c748f !important;
-        color: #fff !important;
-        border-color: #5c748f !important;
-        outline: none !important;
-        box-shadow: none !important;
-    }
+        .custom-blue-btn:hover,
+        .custom-blue-btn:focus,
+        .custom-blue-btn:active,
+        .custom-blue-btn:focus-visible {
+            background-color: #5c748f !important;
+            color: #fff !important;
+            border-color: #5c748f !important;
+            outline: none !important;
+            box-shadow: none !important;
+        }
 
     /* Botão primário usado nos modais desta página */
     .custom-primary-btn {
@@ -1046,17 +889,13 @@
         justify-content: center;
         gap: .5rem;
     }
-
-    .custom-primary-btn:hover,
-    .custom-primary-btn:focus {
-        background-color: #0b5ed7 !important;
-        border-color: #0a58ca !important;
-        color: #fff !important;
-    }
-
-    .custom-primary-btn .btn-inner i {
-        font-size: 1rem;
-    }
+        .custom-primary-btn:hover,
+        .custom-primary-btn:focus {
+            background-color: #0b5ed7 !important;
+            border-color: #0a58ca !important;
+            color: #fff !important;
+        }
+        .custom-primary-btn .btn-inner i { font-size: 1rem; }
 
     /* ======== Syncfusion ComboBox personalizado (motorista) ======== */
     .cmbMotorista_popup .e-dropdownbase .e-list-item {
@@ -1066,40 +905,34 @@
         padding-left: 10px;
         transition: background-color 0.3s;
     }
-
-    .cmbMotorista_popup .e-dropdownbase .e-list-item img {
-        height: 45px;
-        width: 45px;
-        object-fit: cover;
-        border-radius: 50%;
-        margin-right: 10px;
-        transition: transform 0.3s ease;
-    }
-
-    .cmbMotorista_popup .e-dropdownbase .e-list-item:hover {
-        background-color: #f0f8ff;
-    }
-
+        .cmbMotorista_popup .e-dropdownbase .e-list-item img {
+            height: 45px;
+            width: 45px;
+            object-fit: cover;
+            border-radius: 50%;
+            margin-right: 10px;
+            transition: transform 0.3s ease;
+        }
+        .cmbMotorista_popup .e-dropdownbase .e-list-item:hover {
+            background-color: #f0f8ff;
+        }
     #cmbMotorista_popup .e-dropdownbase .e-list-item:hover img {
         transform: scale(1.2);
     }
-
     .cmbMotorista .e-input-group .e-input,
     .cmbMotorista .e-control-wrapper .e-input {
         display: flex;
         align-items: center;
     }
-
-    .cmbMotorista .e-input-group .e-input img,
-    .cmbMotorista .e-control-wrapper .e-input img {
-        height: 30px;
-        width: 30px;
-        object-fit: cover;
-        border-radius: 50%;
-        margin-right: 8px;
-    }
-
-    /* Balão laranja bonitão */
+        .cmbMotorista .e-input-group .e-input img,
+        .cmbMotorista .e-control-wrapper .e-input img {
+            height: 30px;
+            width: 30px;
+            object-fit: cover;
+            border-radius: 50%;
+            margin-right: 8px;
+        }
+            /* Balão laranja bonitão */
     .e-tooltip-wrap,
     .e-tooltip-wrap.custom-orange-tooltip,
     .e-tooltip-wrap.tooltip-laranja {
@@ -1107,61 +940,56 @@
         color: #fff !important;
         border-radius: 8px !important;
         padding: 8px 12px;
-        box-shadow: 0 2px 6px rgba(0, 0, 0, 0.2);
+        box-shadow: 0 2px 6px rgba(0,0,0,0.2);
         font-family: 'Segoe UI', sans-serif;
         font-size: 0.95rem;
         z-index: 99999 !important;
     }
-
-    .e-tooltip-wrap .e-tip-content,
-    .e-tooltip-wrap.custom-orange-tooltip .e-tip-content,
-    .e-tooltip-wrap.tooltip-laranja .e-tip-content {
-        color: #fff !important;
-        font-weight: 500;
-        line-height: 1.4;
-        background-color: transparent !important;
-    }
-
-    .e-tooltip-wrap .e-tip-arrow,
-    .e-tooltip-wrap.custom-orange-tooltip .e-tip-arrow,
-    .e-tooltip-wrap.tooltip-laranja .e-tip-arrow {
-        background: transparent !important;
-        border: none !important;
-        box-shadow: none !important;
-        outline: none !important;
-        width: auto !important;
-        height: auto !important;
-        min-width: 0 !important;
-        min-height: 0 !important;
-        padding: 0 !important;
-        margin: 0 !important;
-    }
-
-    .e-tooltip-wrap .e-arrow-tip-inner,
-    .e-tooltip-wrap.custom-orange-tooltip .e-arrow-tip-inner,
-    .e-tooltip-wrap.tooltip-laranja .e-arrow-tip-inner {
-        background: orange !important;
-        border: none !important;
-        box-shadow: none !important;
-        outline: none !important;
-    }
-
-    .e-tooltip-wrap .e-tip-arrow svg,
-    .e-tooltip-wrap.custom-orange-tooltip .e-tip-arrow svg,
-    .e-tooltip-wrap.tooltip-laranja .e-tip-arrow svg {
-        display: none !important;
-    }
-
-    .e-tooltip-wrap .e-tip-arrow::before,
-    .e-tooltip-wrap .e-tip-arrow::after,
-    .e-tooltip-wrap .e-arrow-tip-inner::before,
-    .e-tooltip-wrap .e-arrow-tip-inner::after {
-        background: none !important;
-        border: none !important;
-        content: none !important;
-        box-shadow: none !important;
-        outline: none !important;
-    }
+        .e-tooltip-wrap .e-tip-content,
+        .e-tooltip-wrap.custom-orange-tooltip .e-tip-content,
+        .e-tooltip-wrap.tooltip-laranja .e-tip-content {
+            color: #fff !important;
+            font-weight: 500;
+            line-height: 1.4;
+            background-color: transparent !important;
+        }
+        .e-tooltip-wrap .e-tip-arrow,
+        .e-tooltip-wrap.custom-orange-tooltip .e-tip-arrow,
+        .e-tooltip-wrap.tooltip-laranja .e-tip-arrow {
+            background: transparent !important;
+            border: none !important;
+            box-shadow: none !important;
+            outline: none !important;
+            width: auto !important;
+            height: auto !important;
+            min-width: 0 !important;
+            min-height: 0 !important;
+            padding: 0 !important;
+            margin: 0 !important;
+        }
+        .e-tooltip-wrap .e-arrow-tip-inner,
+        .e-tooltip-wrap.custom-orange-tooltip .e-arrow-tip-inner,
+        .e-tooltip-wrap.tooltip-laranja .e-arrow-tip-inner {
+            background: orange !important;
+            border: none !important;
+            box-shadow: none !important;
+            outline: none !important;
+        }
+        .e-tooltip-wrap .e-tip-arrow svg,
+        .e-tooltip-wrap.custom-orange-tooltip .e-tip-arrow svg,
+        .e-tooltip-wrap.tooltip-laranja .e-tip-arrow svg {
+            display: none !important;
+        }
+        .e-tooltip-wrap .e-tip-arrow::before,
+        .e-tooltip-wrap .e-tip-arrow::after,
+        .e-tooltip-wrap .e-arrow-tip-inner::before,
+        .e-tooltip-wrap .e-arrow-tip-inner::after {
+            background: none !important;
+            border: none !important;
+            content: none !important;
+            box-shadow: none !important;
+            outline: none !important;
+        }
 
     /* ======== ZOOM – botão na imagem ======== */
     .img-zoom-wrapper {
@@ -1171,53 +999,48 @@
         inline-size: fit-content;
         width: -moz-fit-content;
     }
-
     .zoom-btn {
         position: absolute;
-        left: 8px;
-        bottom: 8px;
+        left: -18px;
+        bottom: 18px;
         width: 44px;
         height: 44px;
-        border: none;
+        border: 3px solid #fff;
         outline: none;
         cursor: pointer;
         background: #ff7a00;
         color: #fff;
         border-radius: 50%;
-        box-shadow: 0 2px 8px rgba(0, 0, 0, .25);
+        box-shadow: 0 2px 8px rgba(0,0,0,.25);
         display: inline-flex;
         align-items: center;
         justify-content: center;
         transition: transform .12s ease, box-shadow .12s ease, background-color .12s ease, filter .12s ease;
         z-index: 2;
     }
-
-    .zoom-btn:hover {
-        transform: translateY(-1px);
-        background: #ff8f26;
-        box-shadow: 0 0 0 3px rgba(255, 122, 0, .25), 0 8px 18px rgba(0, 0, 0, .3);
-        filter: saturate(1.05);
-    }
-
-    .zoom-btn .fa-duotone {
-        --fa-primary-color: #ffffff;
-        --fa-secondary-color: rgba(255, 255, 255, .75);
-        --fa-secondary-opacity: 1;
-        font-size: 1.15rem;
-        line-height: 1;
-    }
+        .zoom-btn:hover {
+            transform: translateY(-1px);
+            background: #ff8f26;
+            box-shadow: 0 0 0 3px rgba(255,122,0,.25), 0 8px 18px rgba(0,0,0,.3);
+            filter: saturate(1.05);
+        }
+        .zoom-btn .fa-duotone {
+            --fa-primary-color: #ffffff;
+            --fa-secondary-color: rgba(255,255,255,.75);
+            --fa-secondary-opacity: 1;
+            font-size: 1.15rem;
+            line-height: 1;
+        }
 
     .modal-zoom {
         max-width: 960px;
     }
-
     #modalZoom .modal-body {
         display: flex;
         justify-content: center;
         align-items: center;
         padding: .75rem;
     }
-
     #imgZoomed {
         max-width: 920px;
         width: 100%;
@@ -1226,64 +1049,57 @@
         object-fit: contain;
         cursor: zoom-out;
     }
-
     .modal-close-floating {
         position: absolute;
         top: .5rem;
         right: .5rem;
         z-index: 5;
-        filter: drop-shadow(0 1px 3px rgba(0, 0, 0, .35));
+        filter: drop-shadow(0 1px 3px rgba(0,0,0,.35));
     }
 
     /* Validação sutil */
     :root {
         --ftx-invalid: #dc3545;
-        --ftx-glow: rgba(220, 53, 69, .18);
-        --ftx-glow-focus: rgba(220, 53, 69, .28);
-    }
-
+        --ftx-glow: rgba(220,53,69,.18);
+        --ftx-glow-focus: rgba(220,53,69,.28);
+    }
     input.is-invalid,
     textarea.is-invalid {
         border-color: var(--ftx-invalid) !important;
         box-shadow: 0 0 0 .2rem var(--ftx-glow);
         color: var(--ftx-invalid) !important;
     }
-
-    input.is-invalid:focus,
-    textarea.is-invalid:focus {
-        box-shadow: 0 0 0 .28rem var(--ftx-glow-focus);
-    }
-
+        input.is-invalid:focus,
+        textarea.is-invalid:focus {
+            box-shadow: 0 0 0 .28rem var(--ftx-glow-focus);
+        }
     .e-input-group.is-invalid,
     .e-float-input.is-invalid,
     .e-control-wrapper.is-invalid {
         box-shadow: 0 0 0 .2rem var(--ftx-glow);
         border-radius: .25rem;
     }
-
-    .e-input-group.is-invalid:focus-within,
-    .e-float-input.is-invalid:focus-within,
-    .e-control-wrapper.is-invalid:focus-within {
-        box-shadow: 0 0 0 .28rem var(--ftx-glow-focus);
-    }
-
-    .e-input-group.is-invalid .e-input,
-    .e-float-input.is-invalid input,
-    .e-control-wrapper.is-invalid input {
-        border-color: var(--ftx-invalid) !important;
-        color: var(--ftx-invalid) !important;
-    }
-
-    .e-float-input.is-invalid label.e-float-text,
-    .e-float-input.is-invalid .e-float-line {
-        color: var(--ftx-invalid) !important;
-        border-color: var(--ftx-invalid) !important;
-    }
+        .e-input-group.is-invalid:focus-within,
+        .e-float-input.is-invalid:focus-within,
+        .e-control-wrapper.is-invalid:focus-within {
+            box-shadow: 0 0 0 .28rem var(--ftx-glow-focus);
+        }
+        .e-input-group.is-invalid .e-input,
+        .e-float-input.is-invalid input,
+        .e-control-wrapper.is-invalid input {
+            border-color: var(--ftx-invalid) !important;
+            color: var(--ftx-invalid) !important;
+        }
+        .e-float-input.is-invalid label.e-float-text,
+        .e-float-input.is-invalid .e-float-line {
+            color: var(--ftx-invalid) !important;
+            border-color: var(--ftx-invalid) !important;
+        }
 
     #imgViewerItem {
         display: block;
         border-radius: 4px;
-        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
+        box-shadow: 0 2px 8px rgba(0,0,0,0.1);
     }
 
     /* ===== BADGES DE OCORRÊNCIA ===== */
@@ -1331,8 +1147,7 @@
                     <div class="panel-container show">
 
                         <div class="ftx-card-header">
-                            @if (Model.ViagemObj.Viagem.Status == "Realizada" || Model.ViagemObj.Viagem.Status ==
-                                                        "Cancelada")
+                            @if (Model.ViagemObj.Viagem.Status == "Realizada" || Model.ViagemObj.Viagem.Status == "Cancelada")
                             {
                                 <h2 class="titulo-paginas">
                                     <i class="fa-duotone fa-suitcase-rolling"></i>
@@ -1347,8 +1162,7 @@
                                 </h2>
                             }
                             <div class="ftx-card-actions">
-                                <a href="javascript:void(0)" class="btn btn-fundo-laranja" id="btnVoltarLista"
-                                    data-ftx-loading>
+                                <a href="javascript:void(0)" class="btn btn-fundo-laranja" id="btnVoltarLista" data-ftx-loading>
                                     <i class="fa-duotone fa-rotate-left icon-space icon-rotate-left"></i>
                                     Voltar à Lista
                                 </a>
@@ -1359,18 +1173,15 @@
                             @{
 
                                 bool viagemFinalizada = Model.ViagemObj?.Viagem?.Status == "Realizada" ||
-                                Model.ViagemObj?.Viagem?.Status == "Cancelada";
-
-                                bool isEdicaoViagemMobile = Model.ViagemObj?.Viagem?.ViagemId != Guid.Empty &&
-                                (Model.ViagemObj?.Viagem?.Rubrica != null ||
-                                Model.ViagemObj?.Viagem?.RubricaFinal != null);
+                                                       Model.ViagemObj?.Viagem?.Status == "Cancelada";
                             }
                             <input type="hidden" asp-for="@Model.ViagemObj.Viagem.ViagemId" id="txtViagemId" />
 
                             @if (Model.ViagemObj?.Viagem?.FichaVistoria != null)
                             {
-                                <input type="hidden" asp-for="FichaVistoria"
-                                    value="@Convert.ToBase64String(Model.ViagemObj.Viagem.FichaVistoria)" />
+                            <input type="hidden"
+                                   asp-for="FichaVistoria"
+                                   value="@Convert.ToBase64String(Model.ViagemObj.Viagem.FichaVistoria)" />
                             }
 
                             <div class="p-3">
@@ -1385,71 +1196,64 @@
                                             <label class="ftx-label" asp-for="ViagemObj.Viagem.NoFichaVistoria">
                                                 Nº Ficha Vistoria
                                             </label>
-                                            <span class="text-danger font-weight-light"
-                                                asp-validation-for="ViagemObj.Viagem.NoFichaVistoria"></span>
+                                            <span class="text-danger font-weight-light" asp-validation-for="ViagemObj.Viagem.NoFichaVistoria"></span>
 
                                             @{
 
                                                 bool isNovaViagem = Model.ViagemObj?.Viagem?.ViagemId == null ||
-                                                Model.ViagemObj.Viagem.ViagemId == Guid.Empty;
+                                                                    Model.ViagemObj.Viagem.ViagemId == Guid.Empty;
 
                                                 bool isViagemMobile = !isNovaViagem &&
-                                                (Model.ViagemObj?.Viagem?.NoFichaVistoria == null ||
-                                                Model.ViagemObj.Viagem.NoFichaVistoria == 0);
+                                                                      (Model.ViagemObj?.Viagem?.NoFichaVistoria == null ||
+                                                                       Model.ViagemObj.Viagem.NoFichaVistoria == 0);
 
                                                 bool mostrarCampoNumerico = isNovaViagem || !isViagemMobile;
                                             }
                                             <div id="containerNoFichaVistoria">
-                                                <input id="txtNoFichaVistoria" class="form-control"
-                                                    asp-for="ViagemObj.Viagem.NoFichaVistoria"
-                                                    style="@(mostrarCampoNumerico ? "" : "display: none;")" />
-                                                <input id="txtNoFichaVistoriaMobile" class="form-control ftx-calculated"
-                                                    value="(mobile)" disabled
-                                                    style="@(mostrarCampoNumerico ? "display: none;" : "")"
-                                                    title="Viagem registrada via FrotiX Mobile" />
+                                                <input id="txtNoFichaVistoria"
+                                                       class="form-control"
+                                                       asp-for="ViagemObj.Viagem.NoFichaVistoria"
+                                                       style="@(mostrarCampoNumerico ? "" : "display: none;")" />
+                                                <input id="txtNoFichaVistoriaMobile"
+                                                       class="form-control ftx-calculated"
+                                                       value="(mobile)"
+                                                       disabled
+                                                       style="@(mostrarCampoNumerico ? "display: none;" : "")"
+                                                       title="Viagem registrada via FrotiX Mobile" />
                                             </div>
                                         </div>
                                         <div class="col-6 col-md-2">
-                                            <label class="ftx-label ftx-required"
-                                                asp-for="ViagemObj.Viagem.DataInicial">
+                                            <label class="ftx-label ftx-required" asp-for="ViagemObj.Viagem.DataInicial">
                                                 Data Inicial
                                             </label>
-                                            <span class="text-danger font-weight-light"
-                                                asp-validation-for="ViagemObj.Viagem.DataInicial"></span>
-                                            <input id="txtDataInicial" class="form-control"
-                                                asp-for="ViagemObj.Viagem.DataInicial" type="date" />
+                                            <span class="text-danger font-weight-light" asp-validation-for="ViagemObj.Viagem.DataInicial"></span>
+                                            <input id="txtDataInicial" class="form-control" asp-for="ViagemObj.Viagem.DataInicial" type="date" />
                                         </div>
                                         <div class="col-6 col-md-2">
                                             <label class="ftx-label ftx-required" asp-for="ViagemObj.Viagem.HoraInicio">
                                                 Hora Início
                                             </label>
-                                            <span class="text-danger font-weight-light"
-                                                asp-validation-for="ViagemObj.Viagem.HoraInicio"></span>
-                                            <input id="txtHoraInicial" class="form-control"
-                                                asp-for="ViagemObj.Viagem.HoraInicio" type="time" />
+                                            <span class="text-danger font-weight-light" asp-validation-for="ViagemObj.Viagem.HoraInicio"></span>
+                                            <input id="txtHoraInicial" class="form-control" asp-for="ViagemObj.Viagem.HoraInicio" type="time" />
                                         </div>
                                         <div class="col-6 col-md-2">
                                             <label class="ftx-label" asp-for="ViagemObj.Viagem.DataFinal">
                                                 Data Final
                                             </label>
-                                            <span class="text-danger font-weight-light"
-                                                asp-validation-for="ViagemObj.Viagem.DataFinal"></span>
-                                            <input id="txtDataFinal" class="form-control"
-                                                asp-for="ViagemObj.Viagem.DataFinal" type="date" />
+                                            <span class="text-danger font-weight-light" asp-validation-for="ViagemObj.Viagem.DataFinal"></span>
+                                            <input id="txtDataFinal" class="form-control" asp-for="ViagemObj.Viagem.DataFinal" type="date" />
                                         </div>
                                         <div class="col-6 col-md-2">
                                             <label class="ftx-label" asp-for="ViagemObj.Viagem.HoraFim">
                                                 Hora Fim
                                             </label>
-                                            <span class="text-danger font-weight-light"
-                                                asp-validation-for="ViagemObj.Viagem.HoraFim"></span>
-                                            <input id="txtHoraFinal" class="form-control"
-                                                asp-for="ViagemObj.Viagem.HoraFim" type="time" />
+                                            <span class="text-danger font-weight-light" asp-validation-for="ViagemObj.Viagem.HoraFim"></span>
+                                            <input id="txtHoraFinal" class="form-control" asp-for="ViagemObj.Viagem.HoraFim" type="time" />
                                         </div>
                                         <div class="col-6 col-md-2">
                                             <label class="ftx-label">Duração</label>
                                             <input id="txtDuracao" class="form-control ftx-calculated" disabled
-                                                data-ejtip="Se a <span style='color: #A0522D; font-weight: bold;'>Duração</span> estiver alta, revise o horário e datas" />
+                                                   data-ejtip="Se a <span style='color: #A0522D; font-weight: bold;'>Duração</span> estiver alta, revise o horário e datas" />
                                         </div>
                                     </div>
                                 </div>
@@ -1461,39 +1265,39 @@
                                     </div>
                                     <div class="row g-3">
                                         <div class="col-12 col-md-4">
-                                            <label class="ftx-label ftx-required"
-                                                asp-for="ViagemObj.Viagem.Finalidade">Finalidade</label>
-                                            <span class="text-danger font-weight-light"
-                                                asp-validation-for="ViagemObj.Viagem.Finalidade"></span>
-                                            <ejs-dropdowntree id="ddtFinalidade" placeholder="Escolha a Finalidade..."
-                                                showCheckBox="false" allowMultiSelection="false" allowFiltering="true"
-                                                filterType="Contains" ejs-for="@Model.ViagemObj.Viagem.Finalidade"
-                                                popupHeight="200px" sortOrder="Ascending" showClearButton="true"
-                                                change="lstFinalidade_Change">
-                                                <e-dropdowntree-fields dataSource="@ViewData["dataFinalidade"]"
-                                                    value="FinalidadeId" text="Descricao"></e-dropdowntree-fields>
-                                            </ejs-dropdowntree>
+                                            <label class="ftx-label ftx-required" asp-for="ViagemObj.Viagem.Finalidade">Finalidade</label>
+                                            <span class="text-danger font-weight-light" asp-validation-for="ViagemObj.Viagem.Finalidade"></span>
+                                            @(Html.Kendo().DropDownList()
+                                                .Name("ddtFinalidade")
+                                                .DataTextField("Descricao")
+                                                .DataValueField("FinalidadeId")
+                                                .OptionLabel("Escolha a Finalidade...")
+                                                .Filter("contains")
+                                                .Height(200)
+                                                .HtmlAttributes(new { style = "width: 100%" })
+                                                .BindTo((System.Collections.IEnumerable)ViewData["dataFinalidade"])
+                                                .Value(Model.ViagemObj.Viagem.Finalidade)
+                                                .Events(e => e.Change("lstFinalidade_Change"))
+                                                .Deferred()
+                                            )
+                                            <input type="hidden" asp-for="ViagemObj.Viagem.Finalidade" id="hiddenFinalidade" />
                                         </div>
                                         <div class="col-12 col-md-4">
-                                            <label class="ftx-label ftx-required"
-                                                asp-for="ViagemObj.Viagem.Origem">Origem</label>
-                                            <span class="text-danger font-weight-light"
-                                                asp-validation-for="ViagemObj.Viagem.Origem"></span>
+                                            <label class="ftx-label ftx-required" asp-for="ViagemObj.Viagem.Origem">Origem</label>
+                                            <span class="text-danger font-weight-light" asp-validation-for="ViagemObj.Viagem.Origem"></span>
                                             <ejs-combobox id="cmbOrigem" ejs-for="ViagemObj.Viagem.Origem"
-                                                dataSource="@ViewData["ListaOrigem"]"
-                                                placeholder="Selecione ou digite a Origem" allowFiltering="true"
-                                                filterType="Contains" allowCustom="true" popupHeight="220px">
+                                                          dataSource="@ViewData["ListaOrigem"]"
+                                                          placeholder="Selecione ou digite a Origem"
+                                                          allowFiltering="true" filterType="Contains"
+                                                          allowCustom="true" popupHeight="220px">
                                             </ejs-combobox>
                                         </div>
                                         <div class="col-12 col-md-4">
-                                            <label class="ftx-label ftx-required"
-                                                asp-for="ViagemObj.Viagem.Destino">Destino</label>
-                                            <span class="text-danger font-weight-light"
-                                                asp-validation-for="ViagemObj.Viagem.Destino"></span>
+                                            <label class="ftx-label ftx-required" asp-for="ViagemObj.Viagem.Destino">Destino</label>
+                                            <span class="text-danger font-weight-light" asp-validation-for="ViagemObj.Viagem.Destino"></span>
                                             <ejs-combobox id="cmbDestino" ejs-for="ViagemObj.Viagem.Destino"
-                                                dataSource="@ViewData["ListaDestino"]"
-                                                placeholder="Selecione ou digite o destino" allowFiltering="true"
-                                                filterType="Contains" allowCustom="true" popupHeight="220px">
+                                                          dataSource="@ViewData["ListaDestino"]" placeholder="Selecione ou digite o destino"
+                                                          allowFiltering="true" filterType="Contains" allowCustom="true" popupHeight="220px">
                                             </ejs-combobox>
                                         </div>
                                     </div>
@@ -1509,20 +1313,20 @@
                                             <label class="ftx-label">Nome do Evento</label>
                                             <div class="d-flex align-items-end gap-2">
                                                 <div class="flex-grow-1">
-                                                    <ejs-dropdowntree id="ddtEventos"
-                                                        placeholder="Selecione um Evento..." showCheckBox="false"
-                                                        allowMultiSelection="false" allowFiltering="true"
-                                                        filterType="Contains" filterBarPlaceholder="Procurar..."
-                                                        ejs-for="@Model.ViagemObj.Viagem.EventoId" popupHeight="200px"
-                                                        sortOrder="Ascending">
-                                                        <e-dropdowntree-fields dataSource="@ViewData["dataEvento"]"
-                                                            value="EventoId" text="Nome"></e-dropdowntree-fields>
+                                                    <ejs-dropdowntree id="ddtEventos" placeholder="Selecione um Evento..."
+                                                                      showCheckBox="false" allowMultiSelection="false"
+                                                                      allowFiltering="true" filterType="Contains"
+                                                                      filterBarPlaceholder="Procurar..."
+                                                                      ejs-for="@Model.ViagemObj.Viagem.EventoId" popupHeight="200px"
+                                                                      sortOrder="Ascending">
+                                                        <e-dropdowntree-fields dataSource="@ViewData["dataEvento"]" value="EventoId" text="Nome"></e-dropdowntree-fields>
                                                     </ejs-dropdowntree>
                                                 </div>
-                                                <a class="btn btn-sm fundo-chocolate text-white" id="btnEvento"
-                                                    style="height: 38px; display: flex; align-items: center;"
-                                                    title="Adicionar Evento" data-bs-toggle="modal"
-                                                    data-bs-target="#modalEvento">
+                                                <a class="btn btn-sm fundo-chocolate text-white"
+                                                   id="btnEvento"
+                                                   style="height: 38px; display: flex; align-items: center;"
+                                                   title="Adicionar Evento"
+                                                   data-bs-toggle="modal" data-bs-target="#modalEvento">
                                                     <i class="fa-duotone fa-calendar-plus"></i>
                                                 </a>
                                             </div>
@@ -1538,11 +1342,17 @@
                                     <div class="row g-3 mb-3">
                                         <div class="col-12 col-md-5">
                                             <label class="ftx-label ftx-required">Motorista</label>
-                                            <ejs-combobox id="cmbMotorista" placeholder="Selecione um Motorista"
-                                                ejs-for="@Model.ViagemObj.Viagem.MotoristaId" allowFiltering="true"
-                                                filterType="Contains" popupHeight="200px" width="100%"
-                                                showClearButton="true" dataSource="@ViewData["dataMotorista"]"
-                                                created="onCmbMotoristaCreated" change="MotoristaValueChange">
+                                            <ejs-combobox id="cmbMotorista"
+                                                          placeholder="Selecione um Motorista"
+                                                          ejs-for="@Model.ViagemObj.Viagem.MotoristaId"
+                                                          allowFiltering="true"
+                                                          filterType="Contains"
+                                                          popupHeight="200px"
+                                                          width="100%"
+                                                          showClearButton="true"
+                                                          dataSource="@ViewData["dataMotorista"]"
+                                                          created="onCmbMotoristaCreated"
+                                                          change="MotoristaValueChange">
                                                 <e-combobox-fields text="Nome" value="MotoristaId"></e-combobox-fields>
                                             </ejs-combobox>
                                         </div>
@@ -1551,24 +1361,25 @@
                                             <div class="d-flex align-items-center gap-2">
                                                 <div class="flex-grow-1">
                                                     <ejs-combobox id="cmbVeiculo" placeholder="Selecione um Veículo"
-                                                        allowFiltering="true" filterType="Contains"
-                                                        dataSource="@ViewData["dataVeiculo"]" popupHeight="200px"
-                                                        width="100%" showClearButton="true"
-                                                        ejs-for="@Model.ViagemObj.Viagem.VeiculoId"
-                                                        change="VeiculoValueChange">
-                                                        <e-combobox-fields text="Descricao"
-                                                            value="VeiculoId"></e-combobox-fields>
+                                                                  allowFiltering="true" filterType="Contains"
+                                                                  dataSource="@ViewData["dataVeiculo"]" popupHeight="200px"
+                                                                  width="100%" showClearButton="true"
+                                                                  ejs-for="@Model.ViagemObj.Viagem.VeiculoId"
+                                                                  change="VeiculoValueChange">
+                                                        <e-combobox-fields text="Descricao" value="VeiculoId"></e-combobox-fields>
                                                     </ejs-combobox>
                                                 </div>
 
-                                                <button type="button" id="btnOcorrenciasVeiculo"
-                                                    class="btn btn-ocorrencias-viagem text-white disabled" disabled
-                                                    title="Nenhuma ocorrência em aberto"
-                                                    style="position: relative; flex-shrink: 0; width: 42px; height: 42px; padding: 0; display: flex; align-items: center; justify-content: center;">
+                                                <button type="button"
+                                                        id="btnOcorrenciasVeiculo"
+                                                        class="btn btn-ocorrencias-viagem text-white disabled"
+                                                        disabled
+                                                        title="Nenhuma ocorrência em aberto"
+                                                        style="position: relative; flex-shrink: 0; width: 42px; height: 42px; padding: 0; display: flex; align-items: center; justify-content: center;">
                                                     <i class="fa-duotone fa-car-burst" style="font-size: 1.2rem;"></i>
                                                     <span id="badgeOcorrenciasVeiculo"
-                                                        class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger"
-                                                        style="display: none; font-size: 0.65rem;">
+                                                          class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger"
+                                                          style="display: none; font-size: 0.65rem;">
                                                         0
                                                     </span>
                                                 </button>
@@ -1577,33 +1388,28 @@
 
                                         <div class="col-4 col-md-2">
                                             <label class="ftx-label" asp-for="ViagemObj.Viagem.KmAtual">Km Atual</label>
-                                            <span class="text-danger font-weight-light"
-                                                asp-validation-for="ViagemObj.Viagem.KmAtual"></span>
-                                            <input readonly id="txtKmAtual" class="form-control ftx-calculated"
-                                                asp-for="ViagemObj.Viagem.KmAtual" />
+                                            <span class="text-danger font-weight-light" asp-validation-for="ViagemObj.Viagem.KmAtual"></span>
+                                            <input readonly id="txtKmAtual" class="form-control ftx-calculated" asp-for="ViagemObj.Viagem.KmAtual" />
                                         </div>
                                     </div>
                                     <div class="row g-3">
                                         <div class="col-4 col-md-2">
-                                            <label class="ftx-label ftx-required"
-                                                asp-for="ViagemObj.Viagem.KmInicial">Km Inicial</label>
-                                            <span class="text-danger font-weight-light"
-                                                asp-validation-for="ViagemObj.Viagem.KmInicial"></span>
-                                            <input id="txtKmInicial" class="form-control"
-                                                asp-for="ViagemObj.Viagem.KmInicial" type="number" />
+                                            <label class="ftx-label ftx-required" asp-for="ViagemObj.Viagem.KmInicial">Km Inicial</label>
+                                            <span class="text-danger font-weight-light" asp-validation-for="ViagemObj.Viagem.KmInicial"></span>
+                                            <input id="txtKmInicial" class="form-control" asp-for="ViagemObj.Viagem.KmInicial" type="number" />
                                         </div>
                                         <div class="col-4 col-md-2">
                                             <label class="ftx-label" asp-for="ViagemObj.Viagem.KmFinal">Km Final</label>
-                                            <span class="text-danger font-weight-light"
-                                                asp-validation-for="ViagemObj.Viagem.KmFinal"></span>
-                                            <input id="txtKmFinal" class="form-control"
-                                                asp-for="ViagemObj.Viagem.KmFinal" type="number" />
+                                            <span class="text-danger font-weight-light" asp-validation-for="ViagemObj.Viagem.KmFinal"></span>
+                                            <input id="txtKmFinal" class="form-control" asp-for="ViagemObj.Viagem.KmFinal" type="number" />
                                         </div>
                                         <div class="col-4 col-md-2">
                                             <label class="ftx-label">Km Percorrido</label>
-                                            <input id="txtKmPercorrido" class="form-control ftx-calculated"
-                                                type="number" disabled
-                                                data-ejtip="Se a <span style='color: #A0522D; font-weight: bold;'>Quilometragem</span> estiver alta, revise os Km Inicial e Final" />
+                                            <input id="txtKmPercorrido"
+                                                   class="form-control ftx-calculated"
+                                                   type="number"
+                                                   disabled
+                                                   data-ejtip="Se a <span style='color: #A0522D; font-weight: bold;'>Quilometragem</span> estiver alta, revise os Km Inicial e Final" />
                                         </div>
                                     </div>
                                 </div>
@@ -1616,27 +1422,49 @@
                                     <div class="row g-3">
                                         <div class="col-6 col-md-3">
                                             <label class="ftx-label ftx-required">Combustível Inicial</label>
-                                            <span class="text-danger font-weight-light"
-                                                asp-validation-for="ViagemObj.Viagem.CombustivelInicial"></span>
-                                            <ejs-dropdowntree id="ddtCombustivelInicial" popupHeight="200px"
-                                                showClearButton="true"
-                                                ejs-for="@Model.ViagemObj.Viagem.CombustivelInicial">
-                                                <e-dropdowntree-fields dataSource="@ViewData["dataCombustivel"]"
-                                                    value="Nivel" text="Descricao"
-                                                    imageURL="Imagem"></e-dropdowntree-fields>
-                                            </ejs-dropdowntree>
+                                            <span class="text-danger font-weight-light" asp-validation-for="ViagemObj.Viagem.CombustivelInicial"></span>
+                                            @(Html.Kendo().DropDownList()
+                                                .Name("ddtCombustivelInicial")
+                                                .DataTextField("Descricao")
+                                                .DataValueField("Nivel")
+                                                .BindTo((System.Collections.IEnumerable)ViewData["dataCombustivel"])
+                                                .Height(200)
+                                                .HtmlAttributes(new { style = "width: 100%" })
+                                                .Value(Model.ViagemObj.Viagem.CombustivelInicial)
+                                                .Template("<div style='display: flex; align-items: center;'>" +
+                                                          "<img src='#: Imagem #' style='width: 24px; height: 24px; margin-right: 8px;' />" +
+                                                          "<span>#: Descricao #</span>" +
+                                                          "</div>")
+                                                .ValueTemplate("<div style='display: flex; align-items: center;'>" +
+                                                               "<img src='#: Imagem #' style='width: 20px; height: 20px; margin-right: 8px;' />" +
+                                                               "<span>#: Descricao #</span>" +
+                                                               "</div>")
+                                                .Deferred()
+                                            )
+                                            <input type="hidden" asp-for="ViagemObj.Viagem.CombustivelInicial" id="hiddenCombustivelInicial" />
                                         </div>
                                         <div class="col-6 col-md-3">
                                             <label class="ftx-label">Combustível Final</label>
-                                            <span class="text-danger font-weight-light"
-                                                asp-validation-for="ViagemObj.Viagem.CombustivelFinal"></span>
-                                            <ejs-dropdowntree id="ddtCombustivelFinal" popupHeight="200px"
-                                                showClearButton="true"
-                                                ejs-for="@Model.ViagemObj.Viagem.CombustivelFinal">
-                                                <e-dropdowntree-fields dataSource="@ViewData["dataCombustivel"]"
-                                                    value="Nivel" text="Descricao"
-                                                    imageURL="Imagem"></e-dropdowntree-fields>
-                                            </ejs-dropdowntree>
+                                            <span class="text-danger font-weight-light" asp-validation-for="ViagemObj.Viagem.CombustivelFinal"></span>
+                                            @(Html.Kendo().DropDownList()
+                                                .Name("ddtCombustivelFinal")
+                                                .DataTextField("Descricao")
+                                                .DataValueField("Nivel")
+                                                .BindTo((System.Collections.IEnumerable)ViewData["dataCombustivel"])
+                                                .Height(200)
+                                                .HtmlAttributes(new { style = "width: 100%" })
+                                                .Value(Model.ViagemObj.Viagem.CombustivelFinal)
+                                                .Template("<div style='display: flex; align-items: center;'>" +
+                                                          "<img src='#: Imagem #' style='width: 24px; height: 24px; margin-right: 8px;' />" +
+                                                          "<span>#: Descricao #</span>" +
+                                                          "</div>")
+                                                .ValueTemplate("<div style='display: flex; align-items: center;'>" +
+                                                               "<img src='#: Imagem #' style='width: 20px; height: 20px; margin-right: 8px;' />" +
+                                                               "<span>#: Descricao #</span>" +
+                                                               "</div>")
+                                                .Deferred()
+                                            )
+                                            <input type="hidden" asp-for="ViagemObj.Viagem.CombustivelFinal" id="hiddenCombustivelFinal" />
                                         </div>
                                     </div>
                                 </div>
@@ -1649,44 +1477,40 @@
                                     <div class="row g-3">
                                         <div class="col-6 col-md-3">
                                             <div class="form-check form-switch">
-                                                <input type="hidden" id="hidStatusDocumento"
-                                                    name="ViagemObj.Viagem.StatusDocumento"
-                                                    value="@(Model.ViagemObj?.Viagem?.StatusDocumento == "Entregue" ? "Entregue" : "")" />
+                                                <input type="hidden" id="hidStatusDocumento" name="ViagemObj.Viagem.StatusDocumento"
+                                                       value="@(Model.ViagemObj?.Viagem?.StatusDocumento == "Entregue" ? "Entregue" : "")" />
                                                 <input class="form-check-input" type="checkbox" id="chkStatusDocumento"
-                                                    @(Model.ViagemObj?.Viagem?.StatusDocumento == "Entregue" ? "checked" : "") @(viagemFinalizada ? "disabled" : "")
-                                                    onchange="document.getElementById('hidStatusDocumento').value = this.checked ? 'Entregue' : '';">
+                                                       @(Model.ViagemObj?.Viagem?.StatusDocumento == "Entregue" ? "checked" : "")
+                                                       @(viagemFinalizada ? "disabled" : "")
+                                                       onchange="document.getElementById('hidStatusDocumento').value = this.checked ? 'Entregue' : '';">
                                                 <label class="form-check-label fw-semibold" for="chkStatusDocumento">
                                                     <i class="fa-duotone fa-file-check me-1 text-success"></i>
-                                                    Documento
+                                                    Documento Entregue
                                                 </label>
                                             </div>
                                         </div>
                                         <div class="col-6 col-md-3">
                                             <div class="form-check form-switch">
-                                                <input type="hidden" id="hidStatusCartaoAbastecimento"
-                                                    name="ViagemObj.Viagem.StatusCartaoAbastecimento"
-                                                    value="@(Model.ViagemObj?.Viagem?.StatusCartaoAbastecimento == "Entregue" ? "Entregue" : "")" />
-                                                <input class="form-check-input" type="checkbox"
-                                                    id="chkStatusCartaoAbastecimento"
-                                                    @(Model.ViagemObj?.Viagem?.StatusCartaoAbastecimento == "Entregue" ?
-                                                                                                        "checked" : "") @(viagemFinalizada ? "disabled" : "")
-                                                    onchange="document.getElementById('hidStatusCartaoAbastecimento').value = this.checked ? 'Entregue' : '';">
-                                                <label class="form-check-label fw-semibold"
-                                                    for="chkStatusCartaoAbastecimento">
+                                                <input type="hidden" id="hidStatusCartaoAbastecimento" name="ViagemObj.Viagem.StatusCartaoAbastecimento"
+                                                       value="@(Model.ViagemObj?.Viagem?.StatusCartaoAbastecimento == "Entregue" ? "Entregue" : "")" />
+                                                <input class="form-check-input" type="checkbox" id="chkStatusCartaoAbastecimento"
+                                                       @(Model.ViagemObj?.Viagem?.StatusCartaoAbastecimento == "Entregue" ? "checked" : "")
+                                                       @(viagemFinalizada ? "disabled" : "")
+                                                       onchange="document.getElementById('hidStatusCartaoAbastecimento').value = this.checked ? 'Entregue' : '';">
+                                                <label class="form-check-label fw-semibold" for="chkStatusCartaoAbastecimento">
                                                     <i class="fa-duotone fa-credit-card me-1 text-success"></i>
-                                                    Cartão Abastecimento
+                                                    Cartão Entregue
                                                 </label>
                                             </div>
                                         </div>
                                         <div class="col-6 col-md-3">
                                             <div class="form-check form-switch">
-                                                <input type="hidden" id="hidCintaEntregue"
-                                                    name="ViagemObj.Viagem.CintaEntregue"
-                                                    value="@(Model.ViagemObj?.Viagem?.CintaEntregue == true ? "true" : "false")" />
+                                                <input type="hidden" id="hidCintaEntregue" name="ViagemObj.Viagem.CintaEntregue"
+                                                       value="@(Model.ViagemObj?.Viagem?.CintaEntregue == true ? "true" : "false")" />
                                                 <input class="form-check-input" type="checkbox" id="chkCintaEntregue"
-                                                    @(Model.ViagemObj?.Viagem?.CintaEntregue == true ? "checked" : "")
-                                                    @(viagemFinalizada ? "disabled" : "")
-                                                    onchange="document.getElementById('hidCintaEntregue').value = this.checked ? 'true' : 'false';">
+                                                       @(Model.ViagemObj?.Viagem?.CintaEntregue == true ? "checked" : "")
+                                                       @(viagemFinalizada ? "disabled" : "")
+                                                       onchange="document.getElementById('hidCintaEntregue').value = this.checked ? 'true' : 'false';">
                                                 <label class="form-check-label fw-semibold" for="chkCintaEntregue">
                                                     <i class="fa-duotone fa-link me-1 text-info"></i>
                                                     Cinta Entregue
@@ -1695,13 +1519,12 @@
                                         </div>
                                         <div class="col-6 col-md-3">
                                             <div class="form-check form-switch">
-                                                <input type="hidden" id="hidTabletEntregue"
-                                                    name="ViagemObj.Viagem.TabletEntregue"
-                                                    value="@(Model.ViagemObj?.Viagem?.TabletEntregue == true ? "true" : "false")" />
+                                                <input type="hidden" id="hidTabletEntregue" name="ViagemObj.Viagem.TabletEntregue"
+                                                       value="@(Model.ViagemObj?.Viagem?.TabletEntregue == true ? "true" : "false")" />
                                                 <input class="form-check-input" type="checkbox" id="chkTabletEntregue"
-                                                    @(Model.ViagemObj?.Viagem?.TabletEntregue == true ? "checked" : "")
-                                                    @(viagemFinalizada ? "disabled" : "")
-                                                    onchange="document.getElementById('hidTabletEntregue').value = this.checked ? 'true' : 'false';">
+                                                       @(Model.ViagemObj?.Viagem?.TabletEntregue == true ? "checked" : "")
+                                                       @(viagemFinalizada ? "disabled" : "")
+                                                       onchange="document.getElementById('hidTabletEntregue').value = this.checked ? 'true' : 'false';">
                                                 <label class="form-check-label fw-semibold" for="chkTabletEntregue">
                                                     <i class="fa-duotone fa-tablet me-1 text-info"></i>
                                                     Tablet Entregue
@@ -1710,61 +1533,54 @@
                                         </div>
                                         <div class="col-6 col-md-3">
                                             <div class="form-check form-switch">
-                                                <input type="hidden" id="hidCaboEntregue"
-                                                    name="ViagemObj.Viagem.CaboEntregue"
-                                                    value="@(Model.ViagemObj?.Viagem?.CaboEntregue == true ? "true" : "false")" />
+                                                <input type="hidden" id="hidArlaEntregue" name="ViagemObj.Viagem.ArlaEntregue"
+                                                       value="@(Model.ViagemObj?.Viagem?.ArlaEntregue == true ? "true" : "false")" />
+                                                <input class="form-check-input" type="checkbox" id="chkArlaEntregue"
+                                                       @(Model.ViagemObj?.Viagem?.ArlaEntregue == true ? "checked" : "")
+                                                       @(viagemFinalizada ? "disabled" : "")
+                                                       onchange="document.getElementById('hidArlaEntregue').value = this.checked ? 'true' : 'false';">
+                                                <label class="form-check-label fw-semibold" for="chkArlaEntregue">
+                                                    <i class="fa-duotone fa-tint me-1 text-info"></i>
+                                                    Arla Entregue
+                                                </label>
+                                            </div>
+                                        </div>
+                                        <div class="col-6 col-md-3">
+                                            <div class="form-check form-switch">
+                                                <input type="hidden" id="hidCaboEntregue" name="ViagemObj.Viagem.CaboEntregue"
+                                                       value="@(Model.ViagemObj?.Viagem?.CaboEntregue == true ? "true" : "false")" />
                                                 <input class="form-check-input" type="checkbox" id="chkCaboEntregue"
-                                                    @(Model.ViagemObj?.Viagem?.CaboEntregue == true ? "checked" : "")
-                                                    @(viagemFinalizada ? "disabled" : "")
-                                                    onchange="document.getElementById('hidCaboEntregue').value = this.checked ? 'true' : 'false';">
+                                                       @(Model.ViagemObj?.Viagem?.CaboEntregue == true ? "checked" : "")
+                                                       @(viagemFinalizada ? "disabled" : "")
+                                                       onchange="document.getElementById('hidCaboEntregue').value = this.checked ? 'true' : 'false';">
                                                 <label class="form-check-label fw-semibold" for="chkCaboEntregue">
-                                                    <i class="fa-duotone fa-plug me-1 text-warning"></i>
+                                                    <i class="fa-duotone fa-charging-station me-1 text-info"></i>
                                                     Cabo Entregue
                                                 </label>
                                             </div>
                                         </div>
-                                        <div class="col-6 col-md-3">
-                                            <div class="form-check form-switch">
-                                                <input type="hidden" id="hidArlaEntregue"
-                                                    name="ViagemObj.Viagem.ArlaEntregue"
-                                                    value="@(Model.ViagemObj?.Viagem?.ArlaEntregue == true ? "true" : "false")" />
-                                                <input class="form-check-input" type="checkbox" id="chkArlaEntregue"
-                                                    @(Model.ViagemObj?.Viagem?.ArlaEntregue == true ? "checked" : "")
-                                                    @(viagemFinalizada ? "disabled" : "")
-                                                    onchange="document.getElementById('hidArlaEntregue').value = this.checked ? 'true' : 'false';">
-                                                <label class="form-check-label fw-semibold" for="chkArlaEntregue">
-                                                    <i class="fa-duotone fa-gas-pump me-1 text-success"></i>
-                                                    Arla Entregue
-                                                </label>
-                                            </div>
-                                        </div>
                                     </div>
                                 </div>
 
                                 @{
                                     bool temVeiculoSelecionado = Model.ViagemObj?.Viagem?.VeiculoId != null &&
-                                    Model.ViagemObj.Viagem.VeiculoId != Guid.Empty;
+                                                                  Model.ViagemObj.Viagem.VeiculoId != Guid.Empty;
                                 }
-                                <div id="secaoOcorrenciasUpsert"
-                                    class="ftx-section ftx-section-ocorrencias @(temVeiculoSelecionado ? "" : "ftx-section-disabled")">
+                                <div id="secaoOcorrenciasUpsert" class="ftx-section ftx-section-ocorrencias @(temVeiculoSelecionado ? "" : "ftx-section-disabled")">
                                     <div class="ftx-section-title">
                                         <i class="fa-duotone fa-triangle-exclamation"></i>
                                         Ocorrências da Viagem
-                                        <span id="badgeOcorrenciasUpsert" class="badge bg-danger ms-2"
-                                            style="display: none;">0</span>
+                                        <span id="badgeOcorrenciasUpsert" class="badge bg-danger ms-2" style="display: none;">0</span>
                                     </div>
 
-                                    <div id="avisoSelecionarVeiculo" class="alert alert-warning py-2 mb-3"
-                                        style="@(temVeiculoSelecionado ? "display: none;" : "")">
+                                    <div id="avisoSelecionarVeiculo" class="alert alert-warning py-2 mb-3" style="@(temVeiculoSelecionado ? "display: none;" : "")">
                                         <i class="fa-duotone fa-info-circle me-1"></i>
                                         Selecione um <strong>veículo</strong> para registrar ocorrências.
                                     </div>
 
                                     <div class="row mb-3">
                                         <div class="col-12">
-                                            <button type="button" class="btn btn-fundo-laranja"
-                                                id="btnAdicionarOcorrenciaUpsert" @(temVeiculoSelecionado &&
-                                                                                                                                  !viagemFinalizada ? "" : "disabled")>
+                                            <button type="button" class="btn btn-fundo-laranja" id="btnAdicionarOcorrenciaUpsert" @(temVeiculoSelecionado && !viagemFinalizada ? "" : "disabled")>
                                                 <i class="fa-duotone fa-plus me-1"></i>
                                                 Registrar Ocorrência
                                             </button>
@@ -1791,51 +1607,45 @@
                                             <label class="ftx-label ftx-required">Requisitante</label>
                                             <div class="d-flex align-items-center gap-2">
                                                 <div class="flex-grow-1">
-                                                    <ejs-combobox id="cmbRequisitante"
-                                                        placeholder="Selecione um Requisitante..." allowFiltering="true"
-                                                        filterType="Contains" dataSource="@ViewData["dataRequisitante"]"
-                                                        popupHeight="200px" width="100%" showClearButton="true"
-                                                        change="RequisitanteValueChange"
-                                                        ejs-for="@Model.ViagemObj.Viagem.RequisitanteId">
-                                                        <e-combobox-fields text="Requisitante"
-                                                            value="RequisitanteId"></e-combobox-fields>
+                                                    <ejs-combobox id="cmbRequisitante" placeholder="Selecione um Requisitante..."
+                                                                  allowFiltering="true" filterType="Contains"
+                                                                  dataSource="@ViewData["dataRequisitante"]" popupHeight="200px"
+                                                                  width="100%" showClearButton="true" change="RequisitanteValueChange"
+                                                                  ejs-for="@Model.ViagemObj.Viagem.RequisitanteId">
+                                                        <e-combobox-fields text="Requisitante" value="RequisitanteId"></e-combobox-fields>
                                                     </ejs-combobox>
                                                 </div>
-                                                <a class="btn btn-sm fundo-chocolate text-white" id="btnRequisitante"
-                                                    style="height: 38px; display: flex; align-items: center;"
-                                                    data-bs-toggle="modal" data-bs-target="#modalRequisitante"
-                                                    title="Adicionar Requisitante">
+                                                <a class="btn btn-sm fundo-chocolate text-white"
+                                                   id="btnRequisitante"
+                                                   style="height: 38px; display: flex; align-items: center;"
+                                                   data-bs-toggle="modal" data-bs-target="#modalRequisitante"
+                                                   title="Adicionar Requisitante">
                                                     <i class="fa-duotone fa-user-plus"></i>
                                                 </a>
                                             </div>
                                         </div>
                                         <div class="col-6 col-md-2">
-                                            <label class="ftx-label ftx-required"
-                                                asp-for="ViagemObj.Viagem.RamalRequisitante">Ramal</label>
-                                            <span class="text-danger font-weight-light"
-                                                asp-validation-for="ViagemObj.Viagem.RamalRequisitante"></span>
-                                            <input id="txtRamalRequisitante" class="form-control"
-                                                asp-for="ViagemObj.Viagem.RamalRequisitante" />
+                                            <label class="ftx-label ftx-required" asp-for="ViagemObj.Viagem.RamalRequisitante">Ramal</label>
+                                            <span class="text-danger font-weight-light" asp-validation-for="ViagemObj.Viagem.RamalRequisitante"></span>
+                                            <input id="txtRamalRequisitante" class="form-control" asp-for="ViagemObj.Viagem.RamalRequisitante" />
                                         </div>
                                         <div class="col-12 col-md-5">
                                             <label class="ftx-label ftx-required">Setor Solicitante</label>
                                             <div class="d-flex align-items-center gap-2">
                                                 <div class="flex-grow-1">
-                                                    <ejs-dropdowntree id="ddtSetor" placeholder="Selecione um Setor"
-                                                        enabled="false" sortOrder="Ascending" showCheckBox="false"
-                                                        allowMultiSelection="false" allowFiltering="true"
-                                                        filterType="Contains" filterBarPlaceholder="Procurar..."
-                                                        ejs-for="@Model.ViagemObj.Viagem.SetorSolicitanteId">
-                                                        <e-dropdowntree-fields dataSource="@ViewData["dataSetor"]"
-                                                            value="SetorSolicitanteId" text="Nome"
-                                                            parentValue="SetorPaiId"
-                                                            hasChildren="HasChild"></e-dropdowntree-fields>
-                                                    </ejs-dropdowntree>
+                                                    <input id="ddtSetor" style="width: 100%" />
+                                                    <input type="hidden" asp-for="ViagemObj.Viagem.SetorSolicitanteId" id="hiddenSetor" />
+                                                    <script>
+                                                        var setorData = @Html.Raw(Newtonsoft.Json.JsonConvert.SerializeObject(ViewData["dataSetor"]));
+                                                        var setorValue = "@(Model.ViagemObj.Viagem.SetorSolicitanteId?.ToString() ?? "")";
+                                                    </script>
                                                 </div>
-                                                <a hidden class="btn btn-verde btn-sm text-white" id="btnSetor"
-                                                    style="height: 38px; display: flex; align-items: center;"
-                                                    data-bs-toggle="modal" data-bs-target="#modalSetor"
-                                                    title="Adicionar Setor">
+                                                <a hidden
+                                                   class="btn btn-verde btn-sm text-white"
+                                                   id="btnSetor"
+                                                   style="height: 38px; display: flex; align-items: center;"
+                                                   data-bs-toggle="modal" data-bs-target="#modalSetor"
+                                                   title="Adicionar Setor">
                                                     <i class="fa-duotone fa-house-medical"></i>
                                                 </a>
                                             </div>
@@ -1851,11 +1661,9 @@
                                     <div class="row">
                                         <div class="col-12">
                                             <label class="ftx-label">Passageiros / Carga</label>
-                                            <input type="hidden" id="DescricaoViagemWordBase64"
-                                                name="DescricaoViagemWordBase64" />
-
-                                            <textarea id="rte" name="ViagemObj.Viagem.Descricao"
-                                                style="height:320px; width:100%;">@Html.Raw(Model.ViagemObj?.Viagem?.Descricao ?? "")</textarea>
+                                            <input type="hidden" id="DescricaoViagemWordBase64" name="DescricaoViagemWordBase64" />
+
+                                            <textarea id="rte" name="ViagemObj.Viagem.Descricao" style="height:320px; width:100%;">@Html.Raw(Model.ViagemObj?.Viagem?.Descricao ?? "")</textarea>
                                             <div id="errorMessage">
                                                 <span asp-validation-for="@Model.ViagemObj.Viagem.Descricao"></span>
                                             </div>
@@ -1867,53 +1675,45 @@
                                     <div class="ftx-section-title">
                                         <i class="fa-duotone fa-box-archive"></i>
                                         Documentos e Itens Devolvidos
-                                        <small class="text-muted fw-normal ms-2">(preencher se a viagem já foi
-                                            finalizada)</small>
+                                        <small class="text-muted fw-normal ms-2">(preencher se a viagem já foi finalizada)</small>
                                     </div>
                                     <div class="row g-3">
                                         <div class="col-6 col-md-3">
                                             <div class="form-check form-switch">
-                                                <input type="hidden" id="hidStatusDocumentoFinal"
-                                                    name="ViagemObj.Viagem.StatusDocumentoFinal"
-                                                    value="@(Model.ViagemObj?.Viagem?.StatusDocumentoFinal == "Devolvido" ? "Devolvido" : "")" />
-                                                <input class="form-check-input" type="checkbox"
-                                                    id="chkStatusDocumentoFinal"
-                                                    @(Model.ViagemObj?.Viagem?.StatusDocumentoFinal == "Devolvido" ?
-                                                                                                        "checked" : "") @(viagemFinalizada ? "disabled" : "")
-                                                    onchange="document.getElementById('hidStatusDocumentoFinal').value = this.checked ? 'Devolvido' : '';">
-                                                <label class="form-check-label fw-semibold"
-                                                    for="chkStatusDocumentoFinal">
+                                                <input type="hidden" id="hidStatusDocumentoFinal" name="ViagemObj.Viagem.StatusDocumentoFinal"
+                                                       value="@(Model.ViagemObj?.Viagem?.StatusDocumentoFinal == "Devolvido" ? "Devolvido" : "")" />
+                                                <input class="form-check-input" type="checkbox" id="chkStatusDocumentoFinal"
+                                                       @(Model.ViagemObj?.Viagem?.StatusDocumentoFinal == "Devolvido" ? "checked" : "")
+                                                       @(viagemFinalizada ? "disabled" : "")
+                                                       onchange="document.getElementById('hidStatusDocumentoFinal').value = this.checked ? 'Devolvido' : '';">
+                                                <label class="form-check-label fw-semibold" for="chkStatusDocumentoFinal">
                                                     <i class="fa-duotone fa-file-check me-1 text-info"></i>
-                                                    Documento
+                                                    Documento Devolvido
                                                 </label>
                                             </div>
                                         </div>
                                         <div class="col-6 col-md-3">
                                             <div class="form-check form-switch">
-                                                <input type="hidden" id="hidStatusCartaoAbastecimentoFinal"
-                                                    name="ViagemObj.Viagem.StatusCartaoAbastecimentoFinal"
-                                                    value="@(Model.ViagemObj?.Viagem?.StatusCartaoAbastecimentoFinal == "Devolvido" ? "Devolvido" : "")" />
-                                                <input class="form-check-input" type="checkbox"
-                                                    id="chkStatusCartaoAbastecimentoFinal"
-                                                    @(Model.ViagemObj?.Viagem?.StatusCartaoAbastecimentoFinal ==
-                                                                                                        "Devolvido" ? "checked" : "") @(viagemFinalizada ? "disabled" : "")
-                                                    onchange="document.getElementById('hidStatusCartaoAbastecimentoFinal').value = this.checked ? 'Devolvido' : '';">
-                                                <label class="form-check-label fw-semibold"
-                                                    for="chkStatusCartaoAbastecimentoFinal">
+                                                <input type="hidden" id="hidStatusCartaoAbastecimentoFinal" name="ViagemObj.Viagem.StatusCartaoAbastecimentoFinal"
+                                                       value="@(Model.ViagemObj?.Viagem?.StatusCartaoAbastecimentoFinal == "Devolvido" ? "Devolvido" : "")" />
+                                                <input class="form-check-input" type="checkbox" id="chkStatusCartaoAbastecimentoFinal"
+                                                       @(Model.ViagemObj?.Viagem?.StatusCartaoAbastecimentoFinal == "Devolvido" ? "checked" : "")
+                                                       @(viagemFinalizada ? "disabled" : "")
+                                                       onchange="document.getElementById('hidStatusCartaoAbastecimentoFinal').value = this.checked ? 'Devolvido' : '';">
+                                                <label class="form-check-label fw-semibold" for="chkStatusCartaoAbastecimentoFinal">
                                                     <i class="fa-duotone fa-credit-card me-1 text-info"></i>
-                                                    Cartão Abastecimento
+                                                    Cartão Devolvido
                                                 </label>
                                             </div>
                                         </div>
                                         <div class="col-6 col-md-3">
                                             <div class="form-check form-switch">
-                                                <input type="hidden" id="hidCintaDevolvida"
-                                                    name="ViagemObj.Viagem.CintaDevolvida"
-                                                    value="@(Model.ViagemObj?.Viagem?.CintaDevolvida == true ? "true" : "false")" />
+                                                <input type="hidden" id="hidCintaDevolvida" name="ViagemObj.Viagem.CintaDevolvida"
+                                                       value="@(Model.ViagemObj?.Viagem?.CintaDevolvida == true ? "true" : "false")" />
                                                 <input class="form-check-input" type="checkbox" id="chkCintaDevolvida"
-                                                    @(Model.ViagemObj?.Viagem?.CintaDevolvida == true ? "checked" : "")
-                                                    @(viagemFinalizada ? "disabled" : "")
-                                                    onchange="document.getElementById('hidCintaDevolvida').value = this.checked ? 'true' : 'false';">
+                                                       @(Model.ViagemObj?.Viagem?.CintaDevolvida == true ? "checked" : "")
+                                                       @(viagemFinalizada ? "disabled" : "")
+                                                       onchange="document.getElementById('hidCintaDevolvida').value = this.checked ? 'true' : 'false';">
                                                 <label class="form-check-label fw-semibold" for="chkCintaDevolvida">
                                                     <i class="fa-duotone fa-link me-1 text-primary"></i>
                                                     Cinta Devolvida
@@ -1922,13 +1722,12 @@
                                         </div>
                                         <div class="col-6 col-md-3">
                                             <div class="form-check form-switch">
-                                                <input type="hidden" id="hidTabletDevolvido"
-                                                    name="ViagemObj.Viagem.TabletDevolvido"
-                                                    value="@(Model.ViagemObj?.Viagem?.TabletDevolvido == true ? "true" : "false")" />
+                                                <input type="hidden" id="hidTabletDevolvido" name="ViagemObj.Viagem.TabletDevolvido"
+                                                       value="@(Model.ViagemObj?.Viagem?.TabletDevolvido == true ? "true" : "false")" />
                                                 <input class="form-check-input" type="checkbox" id="chkTabletDevolvido"
-                                                    @(Model.ViagemObj?.Viagem?.TabletDevolvido == true ? "checked" : "")
-                                                    @(viagemFinalizada ? "disabled" : "")
-                                                    onchange="document.getElementById('hidTabletDevolvido').value = this.checked ? 'true' : 'false';">
+                                                       @(Model.ViagemObj?.Viagem?.TabletDevolvido == true ? "checked" : "")
+                                                       @(viagemFinalizada ? "disabled" : "")
+                                                       onchange="document.getElementById('hidTabletDevolvido').value = this.checked ? 'true' : 'false';">
                                                 <label class="form-check-label fw-semibold" for="chkTabletDevolvido">
                                                     <i class="fa-duotone fa-tablet me-1 text-primary"></i>
                                                     Tablet Devolvido
@@ -1937,39 +1736,57 @@
                                         </div>
                                         <div class="col-6 col-md-3">
                                             <div class="form-check form-switch">
-                                                <input type="hidden" id="hidCaboDevolvido"
-                                                    name="ViagemObj.Viagem.CaboDevolvido"
-                                                    value="@(Model.ViagemObj?.Viagem?.CaboDevolvido == true ? "true" : "false")" />
+                                                <input type="hidden" id="hidArlaDevolvido" name="ViagemObj.Viagem.ArlaDevolvido"
+                                                       value="@(Model.ViagemObj?.Viagem?.ArlaDevolvido == true ? "true" : "false")" />
+                                                <input class="form-check-input" type="checkbox" id="chkArlaDevolvido"
+                                                       @(Model.ViagemObj?.Viagem?.ArlaDevolvido == true ? "checked" : "")
+                                                       @(viagemFinalizada ? "disabled" : "")
+                                                       onchange="document.getElementById('hidArlaDevolvido').value = this.checked ? 'true' : 'false';">
+                                                <label class="form-check-label fw-semibold" for="chkArlaDevolvido">
+                                                    <i class="fa-duotone fa-tint me-1 text-primary"></i>
+                                                    Arla Devolvido
+                                                </label>
+                                            </div>
+                                        </div>
+                                        <div class="col-6 col-md-3">
+                                            <div class="form-check form-switch">
+                                                <input type="hidden" id="hidCaboDevolvido" name="ViagemObj.Viagem.CaboDevolvido"
+                                                       value="@(Model.ViagemObj?.Viagem?.CaboDevolvido == true ? "true" : "false")" />
                                                 <input class="form-check-input" type="checkbox" id="chkCaboDevolvido"
-                                                    @(Model.ViagemObj?.Viagem?.CaboDevolvido == true ? "checked" : "")
-                                                    @(viagemFinalizada ? "disabled" : "")
-                                                    onchange="document.getElementById('hidCaboDevolvido').value = this.checked ? 'true' : 'false';">
+                                                       @(Model.ViagemObj?.Viagem?.CaboDevolvido == true ? "checked" : "")
+                                                       @(viagemFinalizada ? "disabled" : "")
+                                                       onchange="document.getElementById('hidCaboDevolvido').value = this.checked ? 'true' : 'false';">
                                                 <label class="form-check-label fw-semibold" for="chkCaboDevolvido">
-                                                    <i class="fa-duotone fa-plug me-1 text-warning"></i>
+                                                    <i class="fa-duotone fa-charging-station me-1 text-primary"></i>
                                                     Cabo Devolvido
                                                 </label>
                                             </div>
                                         </div>
                                         <div class="col-6 col-md-3">
                                             <div class="form-check form-switch">
-                                                <input type="hidden" id="hidArlaDevolvido"
-                                                    name="ViagemObj.Viagem.ArlaDevolvido"
-                                                    value="@(Model.ViagemObj?.Viagem?.ArlaDevolvido == true ? "true" : "false")" />
+                                                <input type="hidden" id="hidArlaDevolvido" name="ViagemObj.Viagem.ArlaDevolvido"
+                                                       value="@(Model.ViagemObj?.Viagem?.ArlaDevolvido == true ? "true" : "false")" />
                                                 <input class="form-check-input" type="checkbox" id="chkArlaDevolvido"
-                                                    @(Model.ViagemObj?.Viagem?.ArlaDevolvido == true ? "checked" : "")
-                                                    @(viagemFinalizada ? "disabled" : "")
-                                                    onchange="document.getElementById('hidArlaDevolvido').value = this.checked ? 'true' : 'false';">
+                                                       @(Model.ViagemObj?.Viagem?.ArlaDevolvido == true ? "checked" : "")
+                                                       @(viagemFinalizada ? "disabled" : "")
+                                                       onchange="document.getElementById('hidArlaDevolvido').value = this.checked ? 'true' : 'false';">
                                                 <label class="form-check-label fw-semibold" for="chkArlaDevolvido">
-                                                    <i class="fa-duotone fa-gas-pump me-1 text-success"></i>
+                                                    <i class="fa-duotone fa-tint me-1 text-primary"></i>
                                                     Arla Devolvido
                                                 </label>
                                             </div>
                                         </div>
                                     </div>
                                 </div>
+
+                                @{
+
+                                    bool isEdicaoViagemMobile = Model.ViagemObj?.Viagem?.ViagemId != null &&
+                                                                Model.ViagemObj.Viagem.ViagemId != Guid.Empty &&
+                                                                (Model.ViagemObj?.Viagem?.NoFichaVistoria == null ||
+                                                                 Model.ViagemObj.Viagem.NoFichaVistoria == 0);
                                 }
-                                <div id="secaoMobile" class="ftx-section ftx-section-mobile p-3"
-                                    style="@(isEdicaoViagemMobile ? "" : "display: none;")">
+                                <div id="secaoMobile" class="ftx-section ftx-section-mobile p-3" style="@(isEdicaoViagemMobile ? "" : "display: none;")">
                                     <div class="ftx-section-title" style="color: #9c27b0;">
                                         <i class="fa-duotone fa-mobile-screen-button"></i>
                                         Dados do FrotiX Mobile
@@ -1986,8 +1803,10 @@
                                                     <i class="fa-duotone fa-pen fa-2x text-muted"></i>
                                                     <p class="text-muted mb-0 mt-2">Sem rubrica inicial</p>
                                                 </div>
-                                                <img id="imgRubricaInicial" class="rubrica-imagem"
-                                                    style="display: none;" alt="Rubrica Inicial" />
+                                                <img id="imgRubricaInicial"
+                                                     class="rubrica-imagem"
+                                                     style="display: none;"
+                                                     alt="Rubrica Inicial" />
                                             </div>
                                         </div>
 
@@ -2000,8 +1819,10 @@
                                                     <i class="fa-duotone fa-pen-fancy fa-2x text-muted"></i>
                                                     <p class="text-muted mb-0 mt-2">Sem rubrica final</p>
                                                 </div>
-                                                <img id="imgRubricaFinal" class="rubrica-imagem" style="display: none;"
-                                                    alt="Rubrica Final" />
+                                                <img id="imgRubricaFinal"
+                                                     class="rubrica-imagem"
+                                                     style="display: none;"
+                                                     alt="Rubrica Final" />
                                             </div>
                                         </div>
                                     </div>
@@ -2058,121 +1879,137 @@
                                     </div>
                                 </div>
 
-                                <div class="form-group row">
-                                    <div class="col-12">
-                                        <div class="row justify-content-start">
-
-                                            <div id="divSubmit" class="col-12 col-md-4 col-lg-3 pb-2"
-                                                style="@(viagemFinalizada ? "display:none;" : "")">
-                                                @if (Model.ViagemObj.Viagem.ViagemId != Guid.Empty)
-                                                {
-                                                    <button id="btnSubmit" type="submit" asp-page-handler="Submit"
-                                                        class="btn btn-azul btn-submit-spin w-100">
-                                                        <span class="d-flex justify-content-center align-items-center">
-                                                            <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i>
-                                                            <span>Atualizar Viagem</span>
-                                                        </span>
-                                                    </button>
-                                                }
-                                                else
-                                                {
-                                                    <button id="btnSubmit" type="submit" value="Submit"
-                                                        asp-page-handler="Submit"
-                                                        class="btn btn-azul btn-submit-spin w-100">
-                                                        <span class="d-flex justify-content-center align-items-center">
-                                                            <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i>
-                                                            <span>Criar Viagem</span>
-                                                        </span>
-                                                    </button>
-                                                }
-                                            </div>
-
-                                            <div class="col-12 col-md-4 col-lg-3 pb-2">
-                                                <a href="javascript:void(0)"
-                                                    class="btn btn-vinho w-100 btn-voltar-lista" data-ftx-loading>
-                                                    <span class="d-flex justify-content-center align-items-center">
-                                                        <i class="fa-duotone fa-circle-xmark icon-space icon-pulse"></i>
-                                                        <span class="ms-2">Cancelar Operação</span>
-                                                    </span>
-                                                </a>
-                                            </div>
-
-                                            <div class="col-12" hidden>
-                                                @if (Model.ViagemObj.Viagem.ViagemId != Guid.Empty)
-                                                {
-                                                    <button id="btnEscondido" type="button" data-handler="Edit"
-                                                        data-id="@Model.ViagemObj.Viagem.ViagemId"
-                                                        class="btn btn-azul form-control">
-                                                        <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i>
-                                                        Atualizar Viagem
-                                                    </button>
-                                                }
-                                                else
-                                                {
-                                                    <button id="btnEscondido" type="button" data-handler="Submit"
-                                                        class="btn btn-azul form-control">
-                                                        <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i>
-                                                        Criar Viagem
-                                                    </button>
-                                                }
-                                            </div>
+                            <div class="form-group row">
+                                <div class="col-12">
+                                    <div class="row justify-content-start">
+
+                                        <div id="divSubmit" class="col-12 col-md-4 col-lg-3 pb-2" style="@(viagemFinalizada ? "display:none;" : "")">
+                                            @if (Model.ViagemObj.Viagem.ViagemId != Guid.Empty)
+                                            {
+                                            <button id="btnSubmit"
+                                                    type="submit"
+                                                    asp-page-handler="Submit"
+                                                    class="btn btn-azul btn-submit-spin w-100">
+                                                <span class="d-flex justify-content-center align-items-center">
+                                                    <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i>
+                                                    <span>Atualizar Viagem</span>
+                                                </span>
+                                            </button>
+                                            }
+                                            else
+                                            {
+                                            <button id="btnSubmit"
+                                                    type="submit"
+                                                    value="Submit"
+                                                    asp-page-handler="Submit"
+                                                    class="btn btn-azul btn-submit-spin w-100">
+                                                <span class="d-flex justify-content-center align-items-center">
+                                                    <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i>
+                                                    <span>Criar Viagem</span>
+                                                </span>
+                                            </button>
+                                            }
+                                        </div>
+
+                                        <div class="col-12 col-md-4 col-lg-3 pb-2">
+                                            <a href="javascript:void(0)" class="btn btn-vinho w-100 btn-voltar-lista" data-ftx-loading>
+                                                <span class="d-flex justify-content-center align-items-center">
+                                                    <i class="fa-duotone fa-circle-xmark icon-space icon-pulse"></i>
+                                                    <span class="ms-2">Cancelar Operação</span>
+                                                </span>
+                                            </a>
+                                        </div>
+
+                                        <div class="col-12" hidden>
+                                            @if (Model.ViagemObj.Viagem.ViagemId != Guid.Empty)
+                                            {
+                                            <button id="btnEscondido"
+                                                    type="button"
+                                                    data-handler="Edit"
+                                                    data-id="@Model.ViagemObj.Viagem.ViagemId"
+                                                    class="btn btn-azul form-control">
+                                                <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i>
+                                                Atualizar Viagem
+                                            </button>
+                                            }
+                                            else
+                                            {
+                                            <button id="btnEscondido"
+                                                    type="button"
+                                                    data-handler="Submit"
+                                                    class="btn btn-azul form-control">
+                                                <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i>
+                                                Criar Viagem
+                                            </button>
+                                            }
                                         </div>
                                     </div>
                                 </div>
-
                             </div>
+
                         </div>
                     </div>
                 </div>
             </div>
         </div>
+    </div>
 </form>
 
 @{
 
     bool isNovaViagemFicha = Model.ViagemObj?.Viagem?.ViagemId == null ||
-    Model.ViagemObj.Viagem.ViagemId == Guid.Empty;
+                             Model.ViagemObj.Viagem.ViagemId == Guid.Empty;
     bool isViagemMobileFicha = !isNovaViagemFicha &&
-    (Model.ViagemObj?.Viagem?.NoFichaVistoria == null ||
-    Model.ViagemObj.Viagem.NoFichaVistoria == 0);
+                               (Model.ViagemObj?.Viagem?.NoFichaVistoria == null ||
+                                Model.ViagemObj.Viagem.NoFichaVistoria == 0);
 
     bool mostrarFichaVistoria = isNovaViagemFicha || !isViagemMobileFicha;
 }
 
 <form method="post" asp-page-handler="InsereFicha" enctype="multipart/form-data" id="formFichaVistoria">
-    <div class="container-fluid" id="secaoFichaVistoria"
-        style="padding-left: 0; margin-left: 0; @(mostrarFichaVistoria ? "" : "display: none;")">
+    <div class="container-fluid" id="secaoFichaVistoria" style="padding-left: 0; margin-left: 0; @(mostrarFichaVistoria ? "" : "display: none;")">
         <div class="row bottom-space" style="margin-top: 2rem;">
 
             <div class="col-md-6 col-lg-5">
                 <label class="d-block label font-weight-bold">Ficha de Vistoria</label>
-                <input asp-for="FotoUpload" type="file" id="txtFile" class="form-control mb-3" accept="image/*"
-                    onchange="VisualizaImagem(this)" />
+                <input asp-for="FotoUpload"
+                       type="file"
+                       id="txtFile"
+                       class="form-control mb-3"
+                       accept="image/*"
+                       onchange="VisualizaImagem(this)" />
 
                 <input type="hidden" id="hiddenFoto" name="FotoBase64" value="" />
 
                 @if (Model.ViagemObj?.Viagem?.FichaVistoria != null && Model.ViagemObj.Viagem.FichaVistoria.Length > 0)
                 {
-                    <input type="hidden" id="hiddenFichaExistente" name="FichaVistoriaExistente"
-                        value="@Convert.ToBase64String(Model.ViagemObj.Viagem.FichaVistoria)" />
+                <input type="hidden"
+                       id="hiddenFichaExistente"
+                       name="FichaVistoriaExistente"
+                       value="@Convert.ToBase64String(Model.ViagemObj.Viagem.FichaVistoria)" />
                 }
             </div>
 
             <div class="col-md-6 col-lg-7 d-flex flex-column align-items-start">
                 <div class="img-zoom-wrapper">
                     @{
-                        string imageSrc = "/images/FichaAmarelaNova.jpg";
-                        if (Model.ViagemObj?.Viagem?.FichaVistoria != null && Model.ViagemObj.Viagem.FichaVistoria.Length >
-                        0)
-                        {
-                            imageSrc = $"data:image/jpeg;base64,{Convert.ToBase64String(Model.ViagemObj.Viagem.FichaVistoria)}";
-                        }
+                    string imageSrc = "/images/FichaAmarelaNova.jpg";
+                    if (Model.ViagemObj?.Viagem?.FichaVistoria != null && Model.ViagemObj.Viagem.FichaVistoria.Length > 0)
+                    {
+                    imageSrc = $"data:image/jpeg;base64,{Convert.ToBase64String(Model.ViagemObj.Viagem.FichaVistoria)}";
                     }
-                    <img id="imgViewerItem" src="@imageSrc" alt="Ficha de Vistoria"
-                        style="max-width: 500px; max-height: 500px; border: 1px solid #ddd; margin-top: .5rem; object-fit: contain;"
-                        class="img-thumbnail" />
-                    <button type="button" class="zoom-btn" data-bs-toggle="modal" data-bs-target="#modalZoom"
-                        aria-label="Ampliar imagem" onclick="atualizarImagemModal()">
+                    }
+                    <img id="imgViewerItem"
+                         src="@imageSrc"
+                         alt="Ficha de Vistoria"
+                         style="max-width: 500px; max-height: 500px; border: 1px solid #ddd; margin-top: .5rem; object-fit: contain;"
+                         class="img-thumbnail" />
+                    <button type="button"
+                            class="zoom-btn"
+                            data-bs-toggle="modal"
+                            data-bs-target="#modalZoom"
+                            aria-label="Ampliar imagem"
+                            onclick="atualizarImagemModal()">
                         <i class="fa-duotone fa-magnifying-glass-plus"></i>
                     </button>
                 </div>
@@ -2186,12 +2023,13 @@
         <div class="modal-content">
             <div class="modal-header modal-header-azul">
                 <h5 class="modal-title" id="modalZoomLabel">Ficha de Vistoria</h5>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
             <div class="modal-body text-center">
-                <img id="imgZoomed" src="/images/FichaAmarelaNova.jpg" alt="Ficha de Vistoria Ampliada"
-                    style="max-width: 100%; height: auto;" />
+                <img id="imgZoomed"
+                     src="/images/FichaAmarelaNova.jpg"
+                     alt="Ficha de Vistoria Ampliada"
+                     style="max-width: 100%; height: auto;" />
             </div>
         </div>
     </div>
@@ -2248,35 +2086,40 @@
                         <div class="col-5 form-control-xs" style="margin: 10px; width: 200px;">
                             <label class="label font-weight-bold">Requisitante do Evento</label>
                             <ejs-dropdowntree id="lstRequisitanteEvento" placeholder="Selecione um Requisitante"
-                                showCheckBox="false" allowMultiSelection="false" allowFiltering="true"
-                                filterType="Contains" filterBarPlaceholder="Procurar..." popupHeight="200px"
-                                change="RequisitanteEventoValueChange">
-                                <e-dropdowntree-fields dataSource="@ViewData["dataRequisitante"]" value="RequisitanteId"
-                                    text="Requisitante">
+                                              showCheckBox="false" allowMultiSelection="false" allowFiltering="true"
+                                              filterType="Contains" filterBarPlaceholder="Procurar..."
+                                              popupHeight="200px" change="RequisitanteEventoValueChange">
+                                <e-dropdowntree-fields dataSource="@ViewData["dataRequisitante"]"
+                                                       value="RequisitanteId" text="Requisitante">
                                 </e-dropdowntree-fields>
                             </ejs-dropdowntree>
                         </div>
                         <div class="col-5 form-control-xs" style="margin: 10px; width: 200px;">
                             <label class="label font-weight-bold">Setor do Requisitante</label>
                             <ejs-dropdowntree id="ddtSetorRequisitanteEvento" placeholder="Selecione um Setor"
-                                sortOrder="Ascending" showCheckBox="false" allowMultiSelection="false"
-                                allowFiltering="true" filterType="Contains" filterBarPlaceholder="Procurar...">
-                                <e-dropdowntree-fields dataSource="@ViewData["dataSetor"]" value="SetorSolicitanteId"
-                                    text="Nome" parentValue="SetorPaiId" hasChildren="HasChild">
+                                              sortOrder="Ascending" showCheckBox="false" allowMultiSelection="false"
+                                              allowFiltering="true" filterType="Contains" filterBarPlaceholder="Procurar...">
+                                <e-dropdowntree-fields dataSource="@ViewData["dataSetor"]"
+                                                       value="SetorSolicitanteId" text="Nome"
+                                                       parentValue="SetorPaiId" hasChildren="HasChild">
                                 </e-dropdowntree-fields>
                             </ejs-dropdowntree>
                         </div>
                     </div>
                     <br /><br /><br />
                     <div class="modal-footer justify-content-between">
-                        <button id="btnInserirEvento" type="submit" value="SUBMIT" class="btn custom-primary-btn">
+                        <button id="btnInserirEvento"
+                                type="submit"
+                                value="SUBMIT"
+                                class="btn custom-primary-btn">
                             <span class="btn-inner">
                                 <i class="fa-duotone fa-down-to-bracket"></i>
                                 <span>Inserir Evento</span>
                             </span>
                         </button>
-                        <button type="button" class="btn btn-vinho ml-auto ms-auto" data-bs-dismiss="modal"
-                            data-bs-dismiss="modal" aria-label="Fechar">
+                        <button type="button"
+                                class="btn btn-vinho ml-auto ms-auto"
+                                data-bs-dismiss="modal" data-bs-dismiss="modal" aria-label="Fechar">
                             <span class="btn-inner">
                                 <i class="fa-duotone fa-circle-xmark icon-spin" aria-hidden="true"></i>
                                 <span>Fechar</span>
@@ -2299,8 +2142,7 @@
                     <i class="fa-duotone fa-user-plus"></i>
                     Inserir um novo Requisitante
                 </h4>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
 
             <div class="modal-divider"></div>
@@ -2333,10 +2175,11 @@
                         <div class="col-md-6">
                             <label for="ddtSetorRequisitante" class="font-weight-bold">Setor do Requisitante</label>
                             <ejs-dropdowntree id="ddtSetorRequisitante" placeholder="Selecione um Setor"
-                                sortOrder="Ascending" showCheckBox="false" allowMultiSelection="false"
-                                allowFiltering="true" filterType="Contains" filterBarPlaceholder="Procurar...">
-                                <e-dropdowntree-fields dataSource="@ViewData["dataSetor"]" value="SetorSolicitanteId"
-                                    text="Nome" parentValue="SetorPaiId" hasChildren="HasChild">
+                                              sortOrder="Ascending" showCheckBox="false" allowMultiSelection="false"
+                                              allowFiltering="true" filterType="Contains" filterBarPlaceholder="Procurar...">
+                                <e-dropdowntree-fields dataSource="@ViewData["dataSetor"]"
+                                                       value="SetorSolicitanteId" text="Nome"
+                                                       parentValue="SetorPaiId" hasChildren="HasChild">
                                 </e-dropdowntree-fields>
                             </ejs-dropdowntree>
                         </div>
@@ -2346,15 +2189,19 @@
 
                     <div class="modal-footer justify-content-between">
 
-                        <button id="btnInserirRequisitante" class="btn btn-azul" type="submit" value="SUBMIT">
+                        <button id="btnInserirRequisitante"
+                                class="btn btn-azul"
+                                type="submit"
+                                value="SUBMIT">
                             <span class="btn-inner">
                                 <i class="fa-duotone fa-down-to-bracket"></i>
                                 <span>Inserir Requisitante</span>
                             </span>
                         </button>
 
-                        <button type="button" class="btn fundo-vermelho" data-bs-dismiss="modal" data-bs-dismiss="modal"
-                            aria-label="Fechar modal">
+                        <button type="button"
+                                class="btn fundo-vermelho"
+                                data-bs-dismiss="modal" data-bs-dismiss="modal" aria-label="Fechar modal">
                             <span class="btn-inner">
                                 <i class="fa-duotone fa-circle-xmark icon-spin" aria-hidden="true"></i>
                                 <span>Fechar</span>
@@ -2373,8 +2220,7 @@
 
             <div class="modal-header modal-header-azul">
                 <h4 class="modal-title">Inserir um novo Setor Solicitante</h4>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
 
             <div class="modal-divider"></div>
@@ -2388,8 +2234,7 @@
                         </div>
                         <div class="col-md-6">
                             <label for="txtNomeSetor" class="font-weight-bold">Nome do Setor</label>
-                            <input id="txtNomeSetor" class="form-control"
-                                placeholder="Insira o nome completo do setor" />
+                            <input id="txtNomeSetor" class="form-control" placeholder="Insira o nome completo do setor" />
                         </div>
                         <div class="col-md-3">
                             <label for="txtRamalSetor" class="font-weight-bold">Ramal</label>
@@ -2400,11 +2245,12 @@
                     <div class="form-row mb-4">
                         <div class="col-md-6">
                             <label for="ddtSetorPai" class="font-weight-bold">Setor Pai (se houver)</label>
-                            <ejs-dropdowntree id="ddtSetorPai" placeholder="Selecione um Setor" sortOrder="Ascending"
-                                showCheckBox="false" allowMultiSelection="false" allowFiltering="true"
-                                filterType="Contains" filterBarPlaceholder="Procurar...">
-                                <e-dropdowntree-fields dataSource="@ViewData["dataSetor"]" value="SetorSolicitanteId"
-                                    text="Nome" parentValue="SetorPaiId" hasChildren="HasChild">
+                            <ejs-dropdowntree id="ddtSetorPai" placeholder="Selecione um Setor"
+                                              sortOrder="Ascending" showCheckBox="false" allowMultiSelection="false"
+                                              allowFiltering="true" filterType="Contains" filterBarPlaceholder="Procurar...">
+                                <e-dropdowntree-fields dataSource="@ViewData["dataSetor"]"
+                                                       value="SetorSolicitanteId" text="Nome"
+                                                       parentValue="SetorPaiId" hasChildren="HasChild">
                                 </e-dropdowntree-fields>
                             </ejs-dropdowntree>
                         </div>
@@ -2413,15 +2259,19 @@
                     <div class="modal-divider"></div>
 
                     <div class="modal-footer justify-content-between">
-                        <button id="btnInserirSetor" type="submit" value="SUBMIT" class="btn custom-primary-btn">
+                        <button id="btnInserirSetor"
+                                type="submit"
+                                value="SUBMIT"
+                                class="btn custom-primary-btn">
                             <span class="btn-inner">
                                 <i class="fa-duotone fa-down-to-bracket"></i>
                                 <span>Inserir Setor</span>
                             </span>
                         </button>
 
-                        <button type="button" class="btn btn-vinho ml-auto ms-auto" data-bs-dismiss="modal"
-                            data-bs-dismiss="modal" aria-label="Fechar">
+                        <button type="button"
+                                class="btn btn-vinho ml-auto ms-auto"
+                                data-bs-dismiss="modal" data-bs-dismiss="modal" aria-label="Fechar">
                             <span class="btn-inner">
                                 <i class="fa-duotone fa-circle-xmark icon-spin" aria-hidden="true"></i>
                                 <span>Fechar</span>
@@ -2438,8 +2288,7 @@
 
 <div id="syncfusion-toast"></div>
 
-<div class="modal fade" id="modalOcorrenciasVeiculoUpsert" tabindex="-1"
-    aria-labelledby="modalOcorrenciasVeiculoUpsertLabel" aria-hidden="true">
+<div class="modal fade" id="modalOcorrenciasVeiculoUpsert" tabindex="-1" aria-labelledby="modalOcorrenciasVeiculoUpsertLabel" aria-hidden="true">
     <div class="modal-dialog modal-xl" role="document">
         <div class="modal-content">
             <div class="modal-header modal-header-terracota">
@@ -2447,8 +2296,7 @@
                     <i class="fa-duotone fa-car-burst" aria-hidden="true"></i>
                     <span>Ocorrências em Aberto do Veículo</span>
                 </h4>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
 
             <div class="modal-body">
@@ -2481,8 +2329,7 @@
     </div>
 </div>
 
-<div class="modal fade" id="modalImagemOcorrenciaUpsert" tabindex="-1"
-    aria-labelledby="modalImagemOcorrenciaUpsertLabel" aria-hidden="true">
+<div class="modal fade" id="modalImagemOcorrenciaUpsert" tabindex="-1" aria-labelledby="modalImagemOcorrenciaUpsertLabel" aria-hidden="true">
     <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
         <div class="modal-content">
             <div class="modal-header modal-header-terracota">
@@ -2490,13 +2337,14 @@
                     <i class="fa-duotone fa-image" aria-hidden="true"></i>
                     <span>Imagem da Ocorrência</span>
                 </h5>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
 
             <div class="modal-body text-center p-3">
-                <img id="imgOcorrenciaViewerUpsert" class="img-fluid rounded shadow"
-                    style="max-height: 500px; display: none;" alt="Imagem da Ocorrência" />
+                <img id="imgOcorrenciaViewerUpsert"
+                     class="img-fluid rounded shadow"
+                     style="max-height: 500px; display: none;"
+                     alt="Imagem da Ocorrência" />
                 <div id="noImageOcorrenciaUpsert" class="p-4 bg-light rounded" style="display: none;">
                     <i class="fa-duotone fa-image fa-4x text-muted mb-3"></i>
                     <p class="text-muted">Nenhuma imagem disponível</p>
@@ -2512,8 +2360,7 @@
     </div>
 </div>
 
-<div class="modal fade" id="modalSolucaoOcorrenciaUpsert" tabindex="-1"
-    aria-labelledby="modalSolucaoOcorrenciaUpsertLabel" aria-hidden="true">
+<div class="modal fade" id="modalSolucaoOcorrenciaUpsert" tabindex="-1" aria-labelledby="modalSolucaoOcorrenciaUpsertLabel" aria-hidden="true">
     <div class="modal-dialog modal-dialog-centered" role="document">
         <div class="modal-content">
             <div class="modal-header modal-header-verde">
@@ -2521,8 +2368,7 @@
                     <i class="fa-duotone fa-circle-check" aria-hidden="true"></i>
                     <span>Informar Solução da Ocorrência</span>
                 </h5>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
 
             <div class="modal-body">
@@ -2533,7 +2379,7 @@
                         Solução aplicada (opcional)
                     </label>
                     <textarea id="txtSolucaoOcorrenciaUpsert" class="form-control" rows="4"
-                        placeholder="Descreva a solução aplicada para resolver esta ocorrência..."></textarea>
+                              placeholder="Descreva a solução aplicada para resolver esta ocorrência..."></textarea>
                 </div>
             </div>
 
@@ -2551,16 +2397,14 @@
 
 <div id="loadingOverlaySalvando" class="ftx-spin-overlay" style="z-index: 999999; cursor: wait; display: none;">
     <div class="ftx-spin-box" style="text-align: center; min-width: 300px;">
-        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo"
-            style="display: block;" />
+        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo" style="display: block;" />
         <div class="ftx-loading-bar"></div>
         <div class="ftx-loading-text">Gravando Viagem...</div>
         <div class="ftx-loading-subtext">Aguarde, por favor</div>
     </div>
 </div>
 
-<div class="modal fade" id="modalVerOcorrenciaViagem" tabindex="-1" aria-labelledby="modalVerOcorrenciaViagemLabel"
-    aria-hidden="true">
+<div class="modal fade" id="modalVerOcorrenciaViagem" tabindex="-1" aria-labelledby="modalVerOcorrenciaViagemLabel" aria-hidden="true">
     <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
         <div class="modal-content">
             <div class="modal-header modal-header-terracota">
@@ -2568,8 +2412,7 @@
                     <i class="fa-duotone fa-triangle-exclamation" aria-hidden="true"></i>
                     <span>Detalhes da Ocorrência</span>
                 </h5>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
 
             <div class="modal-body">
@@ -2588,19 +2431,19 @@
                     </div>
                     <div class="col-12">
                         <label class="fw-bold text-muted small">Descrição</label>
-                        <div id="txtOcorrenciaDescricao" class="form-control-plaintext border-bottom"
-                            style="min-height: 60px;"></div>
+                        <div id="txtOcorrenciaDescricao" class="form-control-plaintext border-bottom" style="min-height: 60px;"></div>
                     </div>
                     <div class="col-12" id="divSolucaoOcorrencia" style="display: none;">
                         <label class="fw-bold text-muted small">Solução Aplicada</label>
-                        <div id="txtOcorrenciaSolucao"
-                            class="form-control-plaintext border-bottom bg-light p-2 rounded"></div>
+                        <div id="txtOcorrenciaSolucao" class="form-control-plaintext border-bottom bg-light p-2 rounded"></div>
                     </div>
                     <div class="col-12">
                         <label class="fw-bold text-muted small">Imagem</label>
                         <div class="text-center p-3 bg-light rounded">
-                            <img id="imgOcorrenciaViagem" class="img-fluid rounded shadow"
-                                style="max-height: 400px; display: none;" alt="Imagem da Ocorrência" />
+                            <img id="imgOcorrenciaViagem"
+                                 class="img-fluid rounded shadow"
+                                 style="max-height: 400px; display: none;"
+                                 alt="Imagem da Ocorrência" />
                             <div id="semImagemOcorrenciaViagem" class="p-4">
                                 <i class="fa-duotone fa-image fa-3x text-muted mb-2"></i>
                                 <p class="text-muted mb-0">Nenhuma imagem disponível</p>
@@ -2619,8 +2462,7 @@
     </div>
 </div>
 
-<div class="modal fade" id="modalInserirOcorrenciaUpsert" tabindex="-1"
-    aria-labelledby="modalInserirOcorrenciaUpsertLabel" aria-hidden="true">
+<div class="modal fade" id="modalInserirOcorrenciaUpsert" tabindex="-1" aria-labelledby="modalInserirOcorrenciaUpsertLabel" aria-hidden="true">
     <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
         <div class="modal-content">
             <div class="modal-header modal-header-terracota">
@@ -2628,8 +2470,7 @@
                     <i class="fa-duotone fa-triangle-exclamation" aria-hidden="true"></i>
                     <span>Registrar Nova Ocorrência</span>
                 </h5>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
 
             <div class="modal-body">
@@ -2640,8 +2481,10 @@
                             <i class="fa-duotone fa-file-signature me-1 text-warning"></i>
                             Resumo da Ocorrência <span class="text-danger">*</span>
                         </label>
-                        <input type="text" id="txtResumoOcorrenciaUpsert" class="form-control"
-                            placeholder="Ex: Arranhão na lateral, Pneu furado, Vidro trincado..." maxlength="100" />
+                        <input type="text" id="txtResumoOcorrenciaUpsert"
+                               class="form-control"
+                               placeholder="Ex: Arranhão na lateral, Pneu furado, Vidro trincado..."
+                               maxlength="100" />
                     </div>
 
                     <div class="mb-3">
@@ -2649,8 +2492,10 @@
                             <i class="fa-duotone fa-message-lines me-1 text-info"></i>
                             Descrição Detalhada
                         </label>
-                        <textarea id="txtDescricaoOcorrenciaUpsert" class="form-control" rows="3"
-                            placeholder="Descreva os detalhes da ocorrência..."></textarea>
+                        <textarea id="txtDescricaoOcorrenciaUpsert"
+                                  class="form-control"
+                                  rows="3"
+                                  placeholder="Descreva os detalhes da ocorrência..."></textarea>
                     </div>
 
                     <div class="mb-3">
@@ -2659,14 +2504,18 @@
                             Foto da Ocorrência (opcional)
                         </label>
                         <div class="input-group">
-                            <input type="file" id="fileImagemOcorrenciaUpsert" class="form-control" accept="image/*" />
+                            <input type="file" id="fileImagemOcorrenciaUpsert"
+                                   class="form-control"
+                                   accept="image/*" />
                             <button type="button" class="btn btn-vinho" id="btnLimparImagemOcorrenciaUpsert">
                                 <i class="fa-duotone fa-xmark"></i>
                             </button>
                         </div>
                         <div id="previewImagemOcorrenciaUpsert" class="mt-2 text-center" style="display: none;">
-                            <img id="imgPreviewOcorrenciaUpsert" class="img-thumbnail" style="max-height: 150px;"
-                                alt="Preview" />
+                            <img id="imgPreviewOcorrenciaUpsert"
+                                 class="img-thumbnail"
+                                 style="max-height: 150px;"
+                                 alt="Preview" />
                         </div>
                     </div>
                 </form>
@@ -2692,12 +2541,13 @@
                     <i class="fa-duotone fa-image" aria-hidden="true"></i>
                     <span>Imagem da Ocorrência</span>
                 </h5>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
             <div class="modal-body text-center p-4">
-                <img id="imgViewerOcorrenciaUpsert" class="img-fluid rounded shadow" style="max-height: 500px;"
-                    alt="Imagem da Ocorrência" />
+                <img id="imgViewerOcorrenciaUpsert"
+                     class="img-fluid rounded shadow"
+                     style="max-height: 500px;"
+                     alt="Imagem da Ocorrência" />
             </div>
             <div class="modal-footer justify-content-end">
                 <button type="button" class="btn btn-vinho" data-bs-dismiss="modal">
@@ -2931,32 +2781,101 @@
 @section ScriptsBlock {
 
     <script>
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * VARIÁVEIS GLOBAIS DE VIAGEM
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Parâmetros passados do servidor para uso em JavaScript
-        */
-
-        /** @@type { string } ViagemId - ID da viagem atual(GUID) */
+
         const ViagemId = "@(Model.ViagemObj.Viagem?.ViagemId.ToString() ?? "")";
         window.viagemId = ViagemId;
 
-        /** @@type { string } Status da viagem para controle de desabilitação */
         window.viagemStatus = "@(Model.ViagemObj?.Viagem?.Status ?? "")";
-        /** @@type { boolean } Indica se viagem está finalizada(Realizada / Cancelada) */
         window.viagemFinalizada = @Json.Serialize(Model.ViagemObj?.Viagem?.Status == "Realizada" || Model.ViagemObj?.Viagem?.Status == "Cancelada");
 
+        function initKendoWidgets() {
+            try {
+
+                var combInicialWidget = $("#ddtCombustivelInicial").data("kendoDropDownList");
+                if (combInicialWidget) {
+                    combInicialWidget.bind("change", function() {
+                        $("#hiddenCombustivelInicial").val(this.value() || "");
+                    });
+                }
+
+                var combFinalWidget = $("#ddtCombustivelFinal").data("kendoDropDownList");
+                if (combFinalWidget) {
+                    combFinalWidget.bind("change", function() {
+                        $("#hiddenCombustivelFinal").val(this.value() || "");
+                    });
+                }
+
+                if (typeof setorData !== 'undefined' && typeof kendo !== 'undefined') {
+
+                    var hierarchicalData = buildHierarchy(setorData);
+
+                    $("#ddtSetor").kendoDropDownTree({
+                        placeholder: "Selecione um Setor",
+                        dataTextField: "Nome",
+                        dataValueField: "SetorSolicitanteId",
+                        enabled: false,
+                        dataSource: {
+                            data: hierarchicalData,
+                            schema: {
+                                model: {
+                                    id: "SetorSolicitanteId",
+                                    hasChildren: "HasChild",
+                                    children: "items"
+                                }
+                            }
+                        },
+                        value: setorValue,
+                        change: function(e) {
+                            $("#hiddenSetor").val(this.value() || "");
+                        }
+                    });
+                }
+            } catch (error) {
+                console.error('[Viagem Upsert] Erro ao inicializar widgets Kendo:', error);
+            }
+        }
+
+        function buildHierarchy(flatData) {
+            var roots = [];
+            var lookup = {};
+
+            flatData.forEach(function(item) {
+                lookup[item.SetorSolicitanteId] = { ...item, items: [] };
+            });
+
+            flatData.forEach(function(item) {
+                if (item.SetorPaiId && item.SetorPaiId !== "00000000-0000-0000-0000-000000000000") {
+                    var parent = lookup[item.SetorPaiId];
+                    if (parent) {
+                        parent.items.push(lookup[item.SetorSolicitanteId]);
+                    } else {
+                        roots.push(lookup[item.SetorSolicitanteId]);
+                    }
+                } else {
+                    roots.push(lookup[item.SetorSolicitanteId]);
+                }
+            });
+
+            return roots;
+        }
+
     </script>
 
-    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
-        crossorigin="anonymous" />
-    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
-        crossorigin="anonymous"></script>
-
-    <link href="https://kendo.cdn.telerik.com/2022.1.412/styles/kendo.common.min.css" rel="stylesheet" />
-    <link href="https://kendo.cdn.telerik.com/2022.1.412/styles/kendo.default.min.css" rel="stylesheet" />
-    <script src="https://kendo.cdn.telerik.com/2022.1.412/js/kendo.all.min.js"></script>
+    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
+    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
+
+    @(Html.Kendo().DeferredScripts())
+
+    <script>
+        $(document).ready(function() {
+
+            setTimeout(function() {
+                if (typeof initKendoWidgets === 'function') {
+                    initKendoWidgets();
+                }
+            }, 100);
+        });
+    </script>
 
     <script src="~/js/viagens/kendo-editor-upsert.js" asp-append-version="true"></script>
 
@@ -2965,24 +2884,13 @@
     <script src="~/js/cadastros/ViagemUpsert.js" asp-append-version="true"></script>
 
     <script>
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * DETECÇÃO DE MUDANÇAS E CONFIRMAÇÃO AO SAIR
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Sistema de rastreamento de alterações no formulário.
-         * Solicita confirmação antes de sair se houver dados não salvos.
-         */
+
         $(document).ready(function () {
             try {
-                /** @@type { string } Estado serializado inicial do formulário */
+
                 var estadoInicial = $('form').serialize();
-                /** @@type { boolean } Flag de formulário alterado */
                 var formularioAlterado = false;
 
-                /**
-                 * Handler: Detecta mudanças no formulário
-                 * @@description Compara estado atual com inicial para detectar alterações
-                */
                 $('form').on('change input', 'input, select, textarea', function () {
                     try {
                         formularioAlterado = ($('form').serialize() !== estadoInicial);
@@ -2991,10 +2899,6 @@
                     }
                 });
 
-                /**
-                 * Verifica alterações e redireciona com confirmação
-                 * @@description Se há alterações não salvas, exibe dialog de confirmação
-                */
                 function verificarEVoltar() {
                     try {
                         if (formularioAlterado) {
@@ -3020,9 +2924,6 @@
                     }
                 }
 
-                /**
-                 * Handler: Botão Voltar à Lista no header
-                 */
                 $('#btnVoltarLista').on('click', function (e) {
                     try {
                         e.preventDefault();
@@ -3032,9 +2933,6 @@
                     }
                 });
 
-                /**
-                 * Handler: Botão Voltar à Lista no rodapé
-                 */
                 $('.btn-voltar-lista').on('click', function (e) {
                     try {
                         e.preventDefault();
```
