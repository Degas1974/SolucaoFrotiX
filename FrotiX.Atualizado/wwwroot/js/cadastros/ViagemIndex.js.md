# wwwroot/js/cadastros/ViagemIndex.js

**Mudanca:** GRANDE | **+1756** linhas | **-2034** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/ViagemIndex.js
+++ ATUAL: wwwroot/js/cadastros/ViagemIndex.js
@@ -1,7 +1,7 @@
 var CarregandoViagemBloqueada = false;
 
-const FTX_FOTO_PLACEHOLDER = '/images/placeholder-user.png';
-const FTX_FOTO_ENDPOINT = '/api/Viagem/FotoMotorista';
+const FTX_FOTO_PLACEHOLDER = "/images/placeholder-user.png";
+const FTX_FOTO_ENDPOINT = "/api/Viagem/FotoMotorista";
 
 const FtxFotoCache = new Map();
 
@@ -11,7 +11,7 @@
 const FTX_MAX_CONCURRENT = 4;
 let FtxFotoCurrent = 0;
 
-const FtxViagens = (function () {
+const FtxViagens = (function() {
     'use strict';
 
     let _primeiroCarregamento = true;
@@ -36,11 +36,7 @@
                 overlayEl.style.display = 'flex';
             }
         } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'ViagemIndex.js',
-                'FtxViagens.mostrarLoading',
-                error,
-            );
+            Alerta.TratamentoErroComLinha("ViagemIndex.js", "FtxViagens.mostrarLoading", error);
         }
     }
 
@@ -51,21 +47,14 @@
                 overlayEl.style.display = 'none';
             }
         } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'ViagemIndex.js',
-                'FtxViagens.esconderLoading',
-                error,
-            );
+            Alerta.TratamentoErroComLinha("ViagemIndex.js", "FtxViagens.esconderLoading", error);
         }
     }
 
     function adicionarAvisoFiltro() {
         try {
             const filterWrapper = $('#tblViagem_wrapper .dataTables_filter');
-            if (
-                filterWrapper.length &&
-                !filterWrapper.find('.ftx-filter-hint').length
-            ) {
+            if (filterWrapper.length && !filterWrapper.find('.ftx-filter-hint').length) {
                 const avisoHtml = `
                     <span class="ftx-filter-hint" title="Digite qualquer termo para filtrar em todas as colunas visíveis">
                         <i class="fa-duotone fa-lightbulb"></i>
@@ -75,11 +64,7 @@
                 filterWrapper.append(avisoHtml);
             }
         } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'ViagemIndex.js',
-                'FtxViagens.adicionarAvisoFiltro',
-                error,
-            );
+            Alerta.TratamentoErroComLinha("ViagemIndex.js", "FtxViagens.adicionarAvisoFiltro", error);
         }
     }
 
@@ -88,11 +73,7 @@
             mostrarLoading('Filtrando viagens...');
             ListaTodasViagens();
         } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'ViagemIndex.js',
-                'FtxViagens.filtrar',
-                error,
-            );
+            Alerta.TratamentoErroComLinha("ViagemIndex.js", "FtxViagens.filtrar", error);
             esconderLoading();
         }
     }
@@ -102,20 +83,18 @@
         esconderLoading: esconderLoading,
         adicionarAvisoFiltro: adicionarAvisoFiltro,
         filtrar: filtrar,
-        isPrimeiroCarregamento: function () {
-            return _primeiroCarregamento;
-        },
-        setPrimeiroCarregamento: function (val) {
-            _primeiroCarregamento = val;
-        },
+        isPrimeiroCarregamento: function() { return _primeiroCarregamento; },
+        setPrimeiroCarregamento: function(val) { _primeiroCarregamento = val; }
     };
 })();
 
 window.FtxViagens = FtxViagens;
 
-$(document).on('click', '#tblViagem .btn-fundo-laranja', function (e) {
+$(document).on('click', '#tblViagem .btn-fundo-laranja', function (e)
+{
     const $btn = $(this);
-    if ($btn.hasClass('disabled')) {
+    if ($btn.hasClass('disabled'))
+    {
         e.preventDefault();
         return false;
     }
@@ -130,24 +109,45 @@
     modal.show();
 });
 
-$(document).on('click', '#tblViagem .btn-imprimir', function (e) {
+$(document).on('click', '#tblViagem .btn-imprimir', function (e)
+{
     e.preventDefault();
     e.stopPropagation();
 
     const viagemId = $(this).data('viagem-id');
+    console.log('🖨️ Abrindo modal de impressão para viagem:', viagemId);
+
     const modalEl = document.getElementById('modalPrint');
-    if (!modalEl || !viagemId) return;
+    if (!modalEl)
+    {
+        console.error('❌ Modal #modalPrint não encontrado!');
+        return;
+    }
+
+    if (!viagemId)
+    {
+        console.error('❌ ID da viagem não informado!');
+        return;
+    }
 
     modalEl.setAttribute('data-viagem-id', String(viagemId));
 
     $('#txtViagemId').val(viagemId);
+
+    console.log('✅ ID armazenado:', {
+        modal: modalEl.getAttribute('data-viagem-id'),
+        hidden: $('#txtViagemId').val()
+    });
 
     const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
     modal.show();
+    console.log('✅ Modal aberto');
 });
 
-$(document).on('click', '#tblViagem .btn-custos-viagem', function (e) {
-    try {
+$(document).on('click', '#tblViagem .btn-custos-viagem', function (e)
+{
+    try
+    {
         e.preventDefault();
 
         if ($(this).hasClass('disabled')) return;
@@ -161,25 +161,26 @@
 
         const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
         modal.show();
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'click.btn-custos-viagem',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "click.btn-custos-viagem", error);
     }
 });
 
-$('#modalCustosViagem').on('shown.bs.modal', function (e) {
-    try {
+$('#modalCustosViagem').on('shown.bs.modal', function (e)
+{
+    try
+    {
         const modalEl = this;
         let viagemId = modalEl.getAttribute('data-viagem-id');
 
-        if (!viagemId || viagemId === 'undefined') {
+        if (!viagemId || viagemId === 'undefined')
+        {
             viagemId = $('#hiddenCustosViagemId').val();
         }
 
-        if (!viagemId || viagemId === 'undefined') {
+        if (!viagemId || viagemId === 'undefined')
+        {
             console.error('ViagemId não encontrado');
             return;
         }
@@ -187,17 +188,16 @@
         resetarModalCustos();
 
         carregarCustosViagem(viagemId);
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'modalCustosViagem.shown',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "modalCustosViagem.shown", error);
     }
 });
 
-function resetarModalCustos() {
-    try {
+function resetarModalCustos()
+{
+    try
+    {
         $('#spanNumeroViagem').text('-');
         $('#spanInfoViagem').text('Carregando...');
         $('#spanDuracao').text('-');
@@ -211,291 +211,266 @@
         $('#spanCustoOperador').text('R$ 0,00');
         $('#spanCustoLavador').text('R$ 0,00');
         $('#spanCustoTotal').text('R$ 0,00');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'resetarModalCustos',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "resetarModalCustos", error);
     }
 }
 
-function carregarCustosViagem(viagemId) {
-    try {
+function carregarCustosViagem(viagemId)
+{
+    try
+    {
         $.ajax({
             url: '/api/Viagem/ObterCustosViagem',
             type: 'GET',
             data: { viagemId: viagemId },
-            success: function (response) {
-                try {
-                    if (response.success && response.data) {
+            success: function (response)
+            {
+                try
+                {
+                    if (response.success && response.data)
+                    {
                         preencherModalCustos(response.data);
-                    } else {
-                        $('#spanInfoViagem').text(
-                            response.message || 'Erro ao carregar custos',
-                        );
-                        AppToast.show(
-                            'Vermelho',
-                            response.message || 'Erro ao carregar custos',
-                            4000,
-                        );
+                    } else
+                    {
+                        $('#spanInfoViagem').text(response.message || 'Erro ao carregar custos');
+                        AppToast.show('Vermelho', response.message || 'Erro ao carregar custos', 4000);
                     }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'ViagemIndex.js',
-                        'carregarCustosViagem.success',
-                        error,
-                    );
+                } catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("ViagemIndex.js", "carregarCustosViagem.success", error);
                 }
             },
-            error: function (xhr, status, error) {
+            error: function (xhr, status, error)
+            {
                 console.error('Erro ao carregar custos:', error);
                 $('#spanInfoViagem').text('Erro ao carregar custos');
-                AppToast.show(
-                    'Vermelho',
-                    'Erro ao carregar custos da viagem',
-                    4000,
-                );
-            },
+                AppToast.show('Vermelho', 'Erro ao carregar custos da viagem', 4000);
+            }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'carregarCustosViagem',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "carregarCustosViagem", error);
     }
 }
 
-function preencherModalCustos(data) {
-    try {
+function preencherModalCustos(data)
+{
+    try
+    {
 
         $('#spanNumeroViagem').text(data.noFichaVistoria || '-');
 
         $('#spanInfoViagem').text(data.infoViagem || '-');
 
         $('#spanDuracao').text(data.duracaoFormatada || '-');
-        $('#spanKm').text(
-            data.kmPercorrido > 0
-                ? `${data.kmPercorrido.toLocaleString('pt-BR')} km`
-                : '-',
-        );
-        $('#spanLitros').text(
-            data.litrosGastos > 0
-                ? `${data.litrosGastos.toFixed(2).replace('.', ',')} L`
-                : '-',
-        );
+        $('#spanKm').text(data.kmPercorrido > 0 ? `${data.kmPercorrido.toLocaleString('pt-BR')} km` : '-');
+        $('#spanLitros').text(data.litrosGastos > 0 ? `${data.litrosGastos.toFixed(2).replace('.', ',')} L` : '-');
         $('#spanConsumo').text(data.consumoFormatado || '-');
         $('#spanTipoCombustivel').text(data.tipoCombustivel || '-');
 
         $('#spanCustoMotorista').text(formatarMoedaCustos(data.custoMotorista));
         $('#spanCustoVeiculo').text(formatarMoedaCustos(data.custoVeiculo));
-        $('#spanCustoCombustivel').text(
-            formatarMoedaCustos(data.custoCombustivel),
-        );
+        $('#spanCustoCombustivel').text(formatarMoedaCustos(data.custoCombustivel));
         $('#spanCustoOperador').text(formatarMoedaCustos(data.custoOperador));
         $('#spanCustoLavador').text(formatarMoedaCustos(data.custoLavador));
 
         $('#spanCustoTotal').text(formatarMoedaCustos(data.custoTotal));
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'preencherModalCustos',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "preencherModalCustos", error);
     }
 }
 
-function formatarMoedaCustos(valor) {
-    try {
+function formatarMoedaCustos(valor)
+{
+    try
+    {
         if (valor === null || valor === undefined) return 'R$ 0,00';
         return valor.toLocaleString('pt-BR', {
             style: 'currency',
-            currency: 'BRL',
+            currency: 'BRL'
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'formatarMoedaCustos',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "formatarMoedaCustos", error);
         return 'R$ 0,00';
     }
 }
 
-$('#modalCustosViagem').on('hidden.bs.modal', function () {
-    try {
+$('#modalCustosViagem').on('hidden.bs.modal', function ()
+{
+    try
+    {
         this.removeAttribute('data-viagem-id');
         $('#hiddenCustosViagemId').val('');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'modalCustosViagem.hidden',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "modalCustosViagem.hidden", error);
     }
 });
 
-function ftxQueueFotoFetch(motoristaId) {
-    try {
-        if (!motoristaId) return Promise.resolve('');
-
-        if (FtxFotoCache.has(motoristaId)) {
+function ftxQueueFotoFetch(motoristaId)
+{
+    try
+    {
+        if (!motoristaId) return Promise.resolve("");
+
+        if (FtxFotoCache.has(motoristaId))
+        {
             return Promise.resolve(FtxFotoCache.get(motoristaId));
         }
 
-        if (FtxFotoInflight.has(motoristaId)) {
+        if (FtxFotoInflight.has(motoristaId))
+        {
             return FtxFotoInflight.get(motoristaId);
         }
 
-        const promise = new Promise((resolve) => {
+        const promise = new Promise((resolve) =>
+        {
             FtxFotoQueue.push({ motoristaId, resolve });
             ftxDrainFotoQueue();
         });
 
         FtxFotoInflight.set(motoristaId, promise);
         return promise;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'ftxQueueFotoFetch',
-            error,
-        );
-        return Promise.resolve('');
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "ftxQueueFotoFetch", error);
+        return Promise.resolve("");
     }
 }
 
-function ftxDrainFotoQueue() {
-    try {
-        while (FtxFotoCurrent < FTX_MAX_CONCURRENT && FtxFotoQueue.length > 0) {
+function ftxDrainFotoQueue()
+{
+    try
+    {
+        while (FtxFotoCurrent < FTX_MAX_CONCURRENT && FtxFotoQueue.length > 0)
+        {
             const { motoristaId, resolve } = FtxFotoQueue.shift();
             FtxFotoCurrent++;
 
             $.get(FTX_FOTO_ENDPOINT, { id: motoristaId })
-                .done(function (res) {
-                    const src = res && res.fotoBase64 ? res.fotoBase64 : '';
+                .done(function (res)
+                {
+                    const src = (res && res.fotoBase64) ? res.fotoBase64 : "";
                     FtxFotoCache.set(motoristaId, src);
                     resolve(src);
                 })
-                .fail(function () {
-                    FtxFotoCache.set(motoristaId, '');
-                    resolve('');
+                .fail(function ()
+                {
+                    FtxFotoCache.set(motoristaId, "");
+                    resolve("");
                 })
-                .always(function () {
+                .always(function ()
+                {
                     FtxFotoCurrent--;
                     FtxFotoInflight.delete(motoristaId);
                     ftxDrainFotoQueue();
                 });
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'ftxDrainFotoQueue',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "ftxDrainFotoQueue", error);
     }
 }
 
-const FtxFotoObserver = (function () {
-    try {
+const FtxFotoObserver = (function ()
+{
+    try
+    {
         if (!('IntersectionObserver' in window)) return null;
 
         let rootEl = null;
-        try {
-            const cand1 = document.querySelector(
-                '#tblViagem_wrapper .dataTables_scrollBody',
-            );
-            const cand2 = document.querySelector(
-                '#tblViagem_wrapper .dataTables_scroll',
-            );
+        try
+        {
+            const cand1 = document.querySelector('#tblViagem_wrapper .dataTables_scrollBody');
+            const cand2 = document.querySelector('#tblViagem_wrapper .dataTables_scroll');
             rootEl = cand1 || cand2 || null;
-        } catch (e) {
-            rootEl = null;
-        }
-
-        return new IntersectionObserver(
-            (entries, obs) => {
-                entries.forEach((entry) => {
-                    if (!entry.isIntersecting) return;
-
-                    const img = entry.target;
-                    const motId = img.getAttribute('data-mot-id');
-                    const ico =
-                        img.nextElementSibling &&
-                        img.nextElementSibling.tagName === 'I'
-                            ? img.nextElementSibling
-                            : null;
-
-                    ftxQueueFotoFetch(motId).then((src) => {
-                        if (img.getAttribute('data-mot-id') !== motId) return;
-
-                        if (src) {
-                            img.src = src;
-                            img.classList.add('is-visible');
-                            if (ico) ico.style.display = 'none';
-                        } else {
-                            img.removeAttribute('src');
-                            img.classList.remove('is-visible');
-                            if (ico) ico.style.display = 'inline-block';
-                        }
-                    });
-
-                    obs.unobserve(img);
+        } catch (e) { rootEl = null; }
+
+        return new IntersectionObserver((entries, obs) =>
+        {
+            entries.forEach(entry =>
+            {
+                if (!entry.isIntersecting) return;
+
+                const img = entry.target;
+                const motId = img.getAttribute('data-mot-id');
+                const ico = img.nextElementSibling && img.nextElementSibling.tagName === 'I'
+                    ? img.nextElementSibling
+                    : null;
+
+                ftxQueueFotoFetch(motId).then(src =>
+                {
+                    if (img.getAttribute('data-mot-id') !== motId) return;
+
+                    if (src)
+                    {
+                        img.src = src;
+                        img.classList.add('is-visible');
+                        if (ico) ico.style.display = 'none';
+                    } else
+                    {
+                        img.removeAttribute('src');
+                        img.classList.remove('is-visible');
+                        if (ico) ico.style.display = 'inline-block';
+                    }
                 });
-            },
-            { root: rootEl, rootMargin: '120px', threshold: 0.01 },
-        );
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'FtxFotoObserver',
-            error,
-        );
+
+                obs.unobserve(img);
+            });
+        }, { root: rootEl, rootMargin: '120px', threshold: 0.01 });
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "FtxFotoObserver", error);
         return null;
     }
 })();
 
-function ftxBindLazyImgsInRow(row) {
-    try {
+function ftxBindLazyImgsInRow(row)
+{
+    try
+    {
         const $imgs = $(row).find('img[data-mot-id]');
         if (!$imgs.length) return;
 
-        $imgs.each(function () {
+        $imgs.each(function ()
+        {
             const img = this;
             const motId = img.getAttribute('data-mot-id');
 
-            if (motId && FtxFotoCache.has(motId)) {
+            if (motId && FtxFotoCache.has(motId))
+            {
                 img.src = FtxFotoCache.get(motId) || FTX_FOTO_PLACEHOLDER;
                 return;
             }
 
-            if (FtxFotoObserver) {
+            if (FtxFotoObserver)
+            {
                 FtxFotoObserver.observe(img);
-            } else {
-                ftxQueueFotoFetch(motId).then((src) => {
-                    img.src = src;
-                });
+            } else
+            {
+                ftxQueueFotoFetch(motId).then(src => { img.src = src; });
             }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'ftxBindLazyImgsInRow',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "ftxBindLazyImgsInRow", error);
     }
 }
 
-function ftxRenderMotorista(data, type, row, meta) {
-    try {
+function ftxRenderMotorista(data, type, row, meta)
+{
+    try
+    {
         const id = row.motoristaId || row.MotoristaId || '';
         const nome = row.nomeMotorista || row.NomeMotorista || '';
 
         if (type !== 'display') return nome || '';
 
-        const safeNome = $('<div>')
-            .text(nome || '')
-            .html();
+        const safeNome = $('<div>').text(nome || '').html();
         const imgId = `ftx_foto_${id}_${meta.row}_${meta.col}`;
         const icoId = `${imgId}_ico`;
 
@@ -509,138 +484,137 @@
           </div>
         `;
 
-        setTimeout(() => {
+        setTimeout(() =>
+        {
             const img = document.getElementById(imgId);
             const ico = document.getElementById(icoId);
             if (!img) return;
 
             const cached = FtxFotoCache.get(id);
-            const apply = (src) => {
-                if (src) {
+            const apply = (src) =>
+            {
+                if (src)
+                {
                     img.src = src;
                     img.classList.add('is-visible');
                     if (ico) ico.style.display = 'none';
-                } else {
+                } else
+                {
                     img.removeAttribute('src');
                     img.classList.remove('is-visible');
                     if (ico) ico.style.display = 'block';
                 }
             };
 
-            if (cached !== undefined) {
+            if (cached !== undefined)
+            {
                 apply(cached);
-            } else if (FtxFotoObserver) {
+            } else if (FtxFotoObserver)
+            {
                 FtxFotoObserver.observe(img);
-            } else {
+            } else
+            {
                 ftxQueueFotoFetch(id).then(apply);
             }
         }, 0);
 
         return html;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'ftxRenderMotorista',
-            error,
-        );
-        return (row && (row.nomeMotorista || row.NomeMotorista)) || '';
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "ftxRenderMotorista", error);
+        return row && (row.nomeMotorista || row.NomeMotorista) || '';
     }
 }
 
-$(function () {
-    try {
+$(function ()
+{
+    try
+    {
 
         var modalEl = document.getElementById('modalLoadingViagens');
         if (modalEl && !modalEl.classList.contains('show')) {
-            FtxViagens.mostrarLoading(
-                'Aguarde enquanto carregamos as viagens...',
-            );
+            FtxViagens.mostrarLoading('Aguarde enquanto carregamos as viagens...');
         }
 
         ListaTodasViagens();
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('ViagemIndex.js', '$(function)', error);
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "$(function)", error);
         FtxViagens.esconderLoading();
     }
 });
 
-document.addEventListener('DOMContentLoaded', function () {
-    try {
+document.addEventListener("DOMContentLoaded", function ()
+{
+    try
+    {
 
         const tooltipDuracao = new ej.popups.Tooltip({
-            content:
-                'Se a <span style="color: #A0522D; font-weight: bold; text-decoration: underline;">Duração da Viagem</span> estiver muito longa, verifique se ela está <strong>Correta</strong>',
-            opensOn: 'Hover',
-            cssClass: 'custom-orange-tooltip',
-            position: 'TopCenter',
-            beforeOpen: (args) => {
-                try {
+            content: 'Se a <span style="color: #A0522D; font-weight: bold; text-decoration: underline;">Duração da Viagem</span> estiver muito longa, verifique se ela está <strong>Correta</strong>',
+            opensOn: "Hover",
+            cssClass: "custom-orange-tooltip",
+            position: "TopCenter",
+            beforeOpen: (args) =>
+            {
+                try
+                {
                     if (CarregandoViagemBloqueada) args.cancel = true;
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'ViagemIndex.js',
-                        'tooltipDuracao.beforeOpen',
-                        error,
-                    );
-                }
-            },
+                } catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("ViagemIndex.js", "tooltipDuracao.beforeOpen", error);
+                }
+            }
         });
-        tooltipDuracao.appendTo('#txtDuracao');
+        tooltipDuracao.appendTo("#txtDuracao");
 
         const tooltipKm = new ej.popups.Tooltip({
-            content:
-                'Se a <span style="color: #A0522D; font-weight: bold; text-decoration: underline;">Quilometragem Percorrida</span> estiver muito grande, verifique se ela está <strong>Correta</strong>',
-            opensOn: 'Hover',
-            cssClass: 'custom-orange-tooltip',
-            position: 'TopCenter',
-            beforeOpen: (args) => {
-                try {
+            content: 'Se a <span style="color: #A0522D; font-weight: bold; text-decoration: underline;">Quilometragem Percorrida</span> estiver muito grande, verifique se ela está <strong>Correta</strong>',
+            opensOn: "Hover",
+            cssClass: "custom-orange-tooltip",
+            position: "TopCenter",
+            beforeOpen: (args) =>
+            {
+                try
+                {
                     if (CarregandoViagemBloqueada) args.cancel = true;
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'ViagemIndex.js',
-                        'tooltipKm.beforeOpen',
-                        error,
-                    );
-                }
-            },
+                } catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("ViagemIndex.js", "tooltipKm.beforeOpen", error);
+                }
+            }
         });
-        tooltipKm.appendTo('#txtKmPercorrido');
+        tooltipKm.appendTo("#txtKmPercorrido");
 
         window.tooltipDuracao = tooltipDuracao;
         window.tooltipKm = tooltipKm;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'DOMContentLoaded',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "DOMContentLoaded", error);
     }
 });
 
-function salvarFichaVistoria() {
-    try {
+function salvarFichaVistoria()
+{
+    try
+    {
         const fileInput = document.getElementById('txtFile');
         const file = fileInput.files[0];
 
-        if (!file) {
+        if (!file)
+        {
             AppToast.show('Amarelo', 'Selecione um arquivo', 3000);
             return;
         }
 
-        let viagemId =
-            $('#hiddenViagemId').val() ||
+        let viagemId = $('#hiddenViagemId').val() ||
             window.viagemIdAtual ||
             $('#modalFicha').data('viagem-id');
 
         console.log('ID final para salvar:', viagemId);
 
-        if (!viagemId || viagemId === 'undefined' || viagemId === '') {
-            AppToast.show(
-                'Vermelho',
-                'ID da viagem perdido. Feche o modal e tente novamente',
-                3000,
-            );
+        if (!viagemId || viagemId === 'undefined' || viagemId === '')
+        {
+            AppToast.show('Vermelho', 'ID da viagem perdido. Feche o modal e tente novamente', 3000);
             return;
         }
 
@@ -658,83 +632,74 @@
             data: formData,
             processData: false,
             contentType: false,
-            success: function (response) {
+            success: function (response)
+            {
                 $('#loadingSpinner').hide();
                 $('#imageContainer').show();
                 $('#btnSalvarFicha').prop('disabled', false);
 
-                if (response.success) {
-                    AppToast.show(
-                        'Verde',
-                        response.message || 'Ficha salva com sucesso',
-                        3000,
-                    );
+                if (response.success)
+                {
+                    AppToast.show('Verde', response.message || 'Ficha salva com sucesso', 3000);
 
                     const table = $('#tblViagem').DataTable();
-                    if (table) {
+                    if (table)
+                    {
                         table.ajax.reload(null, false);
                     }
 
-                    setTimeout(() => {
-                        const modalFicha = bootstrap.Modal.getInstance(
-                            document.getElementById('modalFicha'),
-                        );
+                    setTimeout(() =>
+                    {
+                        const modalFicha = bootstrap.Modal.getInstance(document.getElementById('modalFicha'));
                         if (modalFicha) modalFicha.hide();
                         $('.modal-backdrop').remove();
                         $('body').removeClass('modal-open');
                     }, 1500);
-                } else {
-                    AppToast.show(
-                        'Vermelho',
-                        response.message || 'Erro ao salvar ficha',
-                        3000,
-                    );
+                } else
+                {
+                    AppToast.show('Vermelho', response.message || 'Erro ao salvar ficha', 3000);
                 }
             },
-            error: function (xhr) {
+            error: function (xhr)
+            {
                 $('#loadingSpinner').hide();
                 $('#imageContainer').show();
                 $('#btnSalvarFicha').prop('disabled', false);
 
-                const errorMsg =
-                    xhr.responseJSON?.message || 'Erro ao enviar arquivo';
+                const errorMsg = xhr.responseJSON?.message || 'Erro ao enviar arquivo';
                 AppToast.show('Vermelho', errorMsg, 4000);
-            },
+            }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'salvarFichaVistoria',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "salvarFichaVistoria", error);
     }
 }
 
-function resetModalFicha() {
-    try {
-        console.log('[DEBUG] Resetando modal de ficha...');
+function resetModalFicha()
+{
+    try
+    {
         $('#txtFile').val('');
         $('#imgFichaViewer').attr('src', '').hide();
         $('#noImageContainer').hide();
-        $('#loadingSpinner').removeClass('d-block').addClass('d-none');
+        $('#loadingSpinner').hide();
         $('#imageContainer').show();
         $('#uploadContainer').show();
         $('#btnSalvarFicha').hide();
         $('#btnAlterarFicha').hide();
-        console.log('[DEBUG] Modal resetado com sucesso');
-    } catch (error) {
-        console.error('[ERROR] Erro ao resetar modal:', error);
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'resetModalFicha',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "resetModalFicha", error);
     }
 }
 
-function carregarFichaVistoria(viagemId) {
-    try {
-        if (!viagemId) {
+function carregarFichaVistoria(viagemId)
+{
+    try
+    {
+        if (!viagemId)
+        {
             console.error('ViagemId não fornecido');
             return;
         }
@@ -748,19 +713,20 @@
             url: '/api/Viagem/ObterFichaVistoria',
             type: 'GET',
             data: { viagemId: viagemId },
-            success: function (response) {
+            success: function (response)
+            {
                 $('#loadingSpinner').hide();
                 $('#imageContainer').show();
 
-                if (response.success && response.temImagem) {
-                    $('#imgFichaViewer')
-                        .attr('src', response.imagemBase64)
-                        .show();
+                if (response.success && response.temImagem)
+                {
+                    $('#imgFichaViewer').attr('src', response.imagemBase64).show();
                     $('#noImageContainer').hide();
                     $('#uploadContainer').hide();
                     $('#btnAlterarFicha').show();
                     $('#btnSalvarFicha').hide();
-                } else {
+                } else
+                {
                     $('#imgFichaViewer').hide();
                     $('#noImageContainer').show();
                     $('#uploadContainer').show();
@@ -768,121 +734,75 @@
                     $('#btnSalvarFicha').hide();
                 }
             },
-            error: function (xhr) {
+            error: function (xhr)
+            {
                 $('#loadingSpinner').hide();
                 $('#imageContainer').show();
                 $('#noImageContainer').show();
-                AppToast.show(
-                    'Vermelho',
-                    'Erro ao carregar ficha de vistoria',
-                    3000,
-                );
-            },
+                AppToast.show('Vermelho', 'Erro ao carregar ficha de vistoria', 3000);
+            }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'carregarFichaVistoria',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "carregarFichaVistoria", error);
     }
 }
 
-function abrirModalFicha(viagemId) {
-    try {
-
-        if (!viagemId || viagemId === 'undefined') {
-            Alerta.Erro('Erro', 'ID da viagem não informado');
-            return;
-        }
-
-        $('#hiddenViagemId').val(viagemId);
-        $('#modalFicha').data('viagem-id', viagemId);
-        window.viagemIdAtual = viagemId;
-
-        console.log('[DEBUG] Abrindo modal para viagem:', viagemId);
-
-        resetModalFicha();
-
-        carregarFichaVistoria(viagemId);
-
-        const modalElement = document.getElementById('modalFicha');
-        if (modalElement) {
-            const modal = new bootstrap.Modal(modalElement);
-            modal.show();
-        } else {
-            console.error('[ERROR] Modal #modalFicha não encontrado no DOM');
-            Alerta.Erro('Erro', 'Modal de ficha não encontrado');
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'abrirModalFicha',
-            error,
-        );
-    }
-}
-
-$(document).ready(function () {
-    try {
-
-        $('#modalFicha .btn-secondary, #modalFicha .close').on(
-            'click',
-            function () {
-                const modalFicha = bootstrap.Modal.getInstance(
-                    document.getElementById('modalFicha'),
-                );
+$(document).ready(function ()
+{
+    try
+    {
+
+        $('#modalFicha .btn-secondary, #modalFicha .close').on('click', function ()
+        {
+            const modalFicha = bootstrap.Modal.getInstance(document.getElementById('modalFicha'));
+            if (modalFicha) modalFicha.hide();
+            setTimeout(() =>
+            {
+                $('.modal-backdrop').remove();
+                $('body').removeClass('modal-open').css('padding-right', '');
+            }, 300);
+        });
+
+        $(document).on('keydown', function (e)
+        {
+            if (e.key === 'Escape' && $('#modalFicha').hasClass('show'))
+            {
+                const modalFicha = bootstrap.Modal.getInstance(document.getElementById('modalFicha'));
                 if (modalFicha) modalFicha.hide();
-                setTimeout(() => {
-                    $('.modal-backdrop').remove();
-                    $('body')
-                        .removeClass('modal-open')
-                        .css('padding-right', '');
-                }, 300);
-            },
-        );
-
-        $(document).on('keydown', function (e) {
-            if (e.key === 'Escape' && $('#modalFicha').hasClass('show')) {
-                const modalFicha = bootstrap.Modal.getInstance(
-                    document.getElementById('modalFicha'),
-                );
-                if (modalFicha) modalFicha.hide();
             }
         });
 
-        $('#modalFicha').on('hidden.bs.modal', function () {
+        $('#modalFicha').on('hidden.bs.modal', function ()
+        {
             resetModalFicha();
             window.viagemIdAtual = null;
             $('#hiddenViagemId').val('');
             $('#modalFicha').removeData('viagem-id');
         });
 
-        $('#txtFile').on('change', function (e) {
+        $('#txtFile').on('change', function (e)
+        {
             const file = e.target.files[0];
-            if (file) {
-                if (!file.type.match('image.*')) {
-                    AppToast.show(
-                        'Vermelho',
-                        'Por favor, selecione apenas arquivos de imagem',
-                        3000,
-                    );
+            if (file)
+            {
+                if (!file.type.match('image.*'))
+                {
+                    AppToast.show('Vermelho', 'Por favor, selecione apenas arquivos de imagem', 3000);
                     this.value = '';
                     return;
                 }
 
-                if (file.size > 5 * 1024 * 1024) {
-                    AppToast.show(
-                        'Vermelho',
-                        'O arquivo não pode ser maior que 5MB',
-                        3000,
-                    );
+                if (file.size > 5 * 1024 * 1024)
+                {
+                    AppToast.show('Vermelho', 'O arquivo não pode ser maior que 5MB', 3000);
                     this.value = '';
                     return;
                 }
 
                 const reader = new FileReader();
-                reader.onload = function (e) {
+                reader.onload = function (e)
+                {
                     $('#imgFichaViewer').attr('src', e.target.result).show();
                     $('#noImageContainer').hide();
                     $('#btnSalvarFicha').show();
@@ -892,356 +812,319 @@
             }
         });
 
-        $('#btnSalvarFicha').on('click', function () {
-            console.log(
-                'Clique no botão salvar. ID atual:',
-                window.viagemIdAtual,
-            );
+        $('#btnSalvarFicha').on('click', function ()
+        {
+            console.log('Clique no botão salvar. ID atual:', window.viagemIdAtual);
             salvarFichaVistoria();
         });
 
-        $('#btnAlterarFicha').on('click', function () {
+        $('#btnAlterarFicha').on('click', function ()
+        {
             $('#uploadContainer').show();
             $('#btnAlterarFicha').hide();
             $('#txtFile').val('').trigger('click');
         });
 
-        if (
-            document.getElementById('ddtCombustivelInicial') &&
-            document.getElementById('ddtCombustivelInicial').ej2_instances
-        ) {
-            document
-                .getElementById('ddtCombustivelInicial')
-                .ej2_instances[0].showPopup();
-            document
-                .getElementById('ddtCombustivelInicial')
-                .ej2_instances[0].hidePopup();
-            console.log('Mostrei/Escondi Popup');
-        }
-    } catch (error) {
-        TratamentoErroComLinha('ViagemIndex.js', 'document.ready', error);
+        if (document.getElementById("ddtCombustivelInicial") &&
+            document.getElementById("ddtCombustivelInicial").ej2_instances)
+        {
+            document.getElementById("ddtCombustivelInicial").ej2_instances[0].showPopup();
+            document.getElementById("ddtCombustivelInicial").ej2_instances[0].hidePopup();
+            console.log("Mostrei/Escondi Popup");
+        }
+    } catch (error)
+    {
+        TratamentoErroComLinha("ViagemIndex.js", "document.ready", error);
     }
 });
 
-$(document).on('click', '.btn-cancela-viagem', async function () {
-    try {
-        const id = $(this).data('id');
+$(document).on("click", ".btn-cancela-viagem", async function ()
+{
+    try
+    {
+        const id = $(this).data("id");
 
         const confirmacao = await window.SweetAlertInterop.ShowConfirm(
-            'Cancelar Viagem',
-            'Você tem certeza que deseja cancelar esta viagem? Não será possível desfazer a operação!',
-            'Cancelar a Viagem',
-            'Desistir',
+            "Cancelar Viagem",
+            "Você tem certeza que deseja cancelar esta viagem? Não será possível desfazer a operação!",
+            "Cancelar a Viagem",
+            "Desistir"
         );
 
-        if (confirmacao) {
+        if (confirmacao)
+        {
             const dataToPost = JSON.stringify({ ViagemId: id });
             $.ajax({
-                url: '/api/Viagem/Cancelar',
-                type: 'POST',
+                url: "/api/Viagem/Cancelar",
+                type: "POST",
                 data: dataToPost,
-                contentType: 'application/json; charset=utf-8',
-                dataType: 'json',
+                contentType: "application/json; charset=utf-8",
+                dataType: "json"
             })
-                .done(function (data) {
-                    try {
-                        if (data.success) {
+                .done(function (data)
+                {
+                    try
+                    {
+                        if (data.success)
+                        {
                             AppToast.show('Verde', data.message);
-                            $('#tblViagem').DataTable().ajax.reload();
-                        } else {
+                            $("#tblViagem").DataTable().ajax.reload();
+                        } else
+                        {
                             AppToast.show('Vermelho', data.message);
                         }
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'ViagemIndex.js',
-                            'ajax.done.cancelar',
-                            error,
-                        );
+                    } catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("ViagemIndex.js", "ajax.done.cancelar", error);
                     }
                 })
-                .fail(function (err) {
-                    try {
-                        TratamentoErroComLinha(
-                            'ViagemIndex.js',
-                            'ajax.error',
-                            err,
-                        );
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'ViagemIndex.js',
-                            'ajax.fail.cancelar',
-                            error,
-                        );
+                .fail(function (err)
+                {
+                    try
+                    {
+                        TratamentoErroComLinha("ViagemIndex.js", "ajax.error", err);
+                    } catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("ViagemIndex.js", "ajax.fail.cancelar", error);
                     }
                 });
         }
-    } catch (error) {
-        TratamentoErroComLinha('ViagemIndex.js', 'click.btn-cancelar', error);
+    } catch (error)
+    {
+        TratamentoErroComLinha("ViagemIndex.js", "click.btn-cancelar", error);
     }
 });
 
-$('#modalFinalizaViagem').on('shown.bs.modal', function (event) {
-    try {
-
-        if (
-            window.ValidadorFinalizacaoIA &&
-            window.ValidadorFinalizacaoIA.resetarConfirmacoes
-        ) {
+$("#modalFinalizaViagem").on("shown.bs.modal", function (event)
+{
+    try
+    {
+
+        if (window.ValidadorFinalizacaoIA && window.ValidadorFinalizacaoIA.resetarConfirmacoes)
+        {
             window.ValidadorFinalizacaoIA.resetarConfirmacoes();
-            console.log('🔄 ValidadorFinalizacaoIA: confirmações resetadas');
+            console.log("🔄 ValidadorFinalizacaoIA: confirmações resetadas");
         }
 
         let $btn = $(event.relatedTarget || []);
 
         const modalEl = this;
         const fallbackId = modalEl.getAttribute('data-trigger-id');
-        const viagemId = ($btn.length ? $btn.data('id') : null) || fallbackId;
-
-        if (!viagemId) {
-            console.warn(
-                'Não foi possível determinar o viagemId para preencher o modal.',
-            );
+        const viagemId = ($btn.length ? $btn.data("id") : null) || fallbackId;
+
+        if (!viagemId)
+        {
+            console.warn("Não foi possível determinar o viagemId para preencher o modal.");
             return;
         }
 
-        $('#txtId').val(viagemId);
-
-        const dt = $('#tblViagem').DataTable();
-        let idx = dt
-            .rows((i, r) => String(r.viagemId) === String(viagemId))
-            .indexes()[0];
-        let data = idx !== undefined ? dt.row(idx).data() : null;
-
-        if (!data) {
-
-            if ($btn.length) {
-                data = dt.row($btn.closest('tr')).data();
-                if (!data) {
-                    const $parent = $btn.closest('tr.child').prev('.parent');
+        $("#txtId").val(viagemId);
+
+        const dt = $("#tblViagem").DataTable();
+        let idx = dt.rows((i, r) => String(r.viagemId) === String(viagemId)).indexes()[0];
+        let data = (idx !== undefined) ? dt.row(idx).data() : null;
+
+        if (!data)
+        {
+
+            if ($btn.length)
+            {
+                data = dt.row($btn.closest("tr")).data();
+                if (!data)
+                {
+                    const $parent = $btn.closest("tr.child").prev(".parent");
                     if ($parent.length) data = dt.row($parent).data();
                 }
             }
         }
 
-        let dataInicial = data?.dataInicial
-            ? new Date(data.dataInicial).toLocaleDateString('pt-BR')
-            : '';
-        let horaInicio = data?.horaInicio
-            ? new Date(data.horaInicio).toLocaleTimeString('pt-BR', {
-                  hour: '2-digit',
-                  minute: '2-digit',
-              })
-            : '';
-
-        $('#txtDataInicial').val(dataInicial).prop('disabled', true);
-        $('#txtHoraInicial').val(horaInicio).prop('disabled', true);
-        $('#txtKmInicial').val(data.kmInicial).prop('disabled', true);
-
-        const combInicial = document.getElementById('ddtCombustivelInicial');
-        const combFinal = document.getElementById('ddtCombustivelFinal');
-        const rteDescricao = document.getElementById('rteDescricao');
-
-        if (combInicial?.ej2_instances?.length) {
+        let dataInicial = data?.dataInicial ? new Date(data.dataInicial).toLocaleDateString("pt-BR") : "";
+        let horaInicio = data?.horaInicio ? new Date(data.horaInicio).toLocaleTimeString("pt-BR", { hour: "2-digit", minute: "2-digit" }) : "";
+
+        $("#txtDataInicial").val(dataInicial).prop("disabled", true);
+        $("#txtHoraInicial").val(horaInicio).prop("disabled", true);
+        $("#txtKmInicial").val(data.kmInicial).prop("disabled", true);
+
+        const combInicial = document.getElementById("ddtCombustivelInicial");
+        const combFinal = document.getElementById("ddtCombustivelFinal");
+        const rteDescricao = document.getElementById("rteDescricao");
+
+        if (combInicial?.ej2_instances?.length)
+        {
             combInicial.ej2_instances[0].value = [data.combustivelInicial];
             combInicial.ej2_instances[0].enabled = false;
         }
 
-        $('#h3Titulo').html(
-            'Finalizar a Viagem - Ficha nº ' +
-                data.noFichaVistoria +
-                ' de ' +
-                data.nomeMotorista,
-        );
-
-        console.log('📋 Dados da viagem disponíveis:', data);
-        console.log(' Campos:', Object.keys(data));
-
-        var veiculoIdValue =
-            data.veiculoId || data.VeiculoId || data.veiculo_id || '';
-        var motoristaIdValue =
-            data.motoristaId || data.MotoristaId || data.motorista_id || '';
-
-        console.log(' veiculoId encontrado:', veiculoIdValue);
-        console.log(' motoristaId encontrado:', motoristaIdValue);
+        $("#h3Titulo").html("Finalizar a Viagem - Ficha nº " + data.noFichaVistoria + " de " + data.nomeMotorista);
+
+        console.log("📋 Dados da viagem disponíveis:", data);
+        console.log(" Campos:", Object.keys(data));
+
+        var veiculoIdValue = data.veiculoId || data.VeiculoId || data.veiculo_id || '';
+        var motoristaIdValue = data.motoristaId || data.MotoristaId || data.motorista_id || '';
+
+        console.log(" veiculoId encontrado:", veiculoIdValue);
+        console.log(" motoristaId encontrado:", motoristaIdValue);
 
         modalEl.setAttribute('data-veiculo-id', veiculoIdValue);
         modalEl.setAttribute('data-motorista-id', motoristaIdValue);
 
-        if (data.dataFinal != null) {
+        if (data.dataFinal != null)
+        {
             CarregandoViagemBloqueada = true;
 
-            const formattedDate = new Date(data.dataFinal).toLocaleDateString(
-                'pt-BR',
-            );
-            $('#txtDataFinal')
-                .removeAttr('type')
-                .val(formattedDate)
-                .attr('readonly', true);
-
-            const isoHour = data.horaFim || '2025-08-05T09:50:00';
+            const formattedDate = new Date(data.dataFinal).toLocaleDateString("pt-BR");
+            $("#txtDataFinal").removeAttr("type").val(formattedDate).attr("readonly", true);
+
+            const isoHour = data.horaFim || "2025-08-05T09:50:00";
             const dateHour = new Date(isoHour);
-            const timeFormatted = `${String(dateHour.getHours()).padStart(2, '0')}:${String(dateHour.getMinutes()).padStart(2, '0')}`;
-            $('#txtHoraFinal').val(timeFormatted).attr('readonly', true);
-
-            $('#txtKmFinal').val(data.kmFinal).attr('readonly', true);
-
-            if (combFinal?.ej2_instances?.length) {
+            const timeFormatted = `${String(dateHour.getHours()).padStart(2, "0")}:${String(dateHour.getMinutes()).padStart(2, "0")}`;
+            $("#txtHoraFinal").val(timeFormatted).attr("readonly", true);
+
+            $("#txtKmFinal").val(data.kmFinal).attr("readonly", true);
+
+            if (combFinal?.ej2_instances?.length)
+            {
                 combFinal.ej2_instances[0].value = [data.combustivelFinal];
                 combFinal.ej2_instances[0].enabled = false;
             }
-            if (rteDescricao?.ej2_instances?.length) {
+            if (rteDescricao?.ej2_instances?.length)
+            {
                 rteDescricao.ej2_instances[0].value = data.descricao;
                 rteDescricao.ej2_instances[0].readonly = true;
             }
 
-            $('#chkStatusDocumento')
-                .prop('checked', data.statusDocumento)
-                .prop('disabled', true);
-            $('#chkStatusCartaoAbastecimento')
-                .prop('checked', data.statusCartaoAbastecimento)
-                .prop('disabled', true);
+            $("#chkStatusDocumento").prop("checked", data.statusDocumento).prop("disabled", true);
+            $("#chkStatusCartaoAbastecimento").prop("checked", data.statusCartaoAbastecimento).prop("disabled", true);
 
             calcularDistanciaViagem();
             calcularDuracaoViagem();
 
-            $('#btnFinalizarViagem').hide();
-        } else {
+            $("#btnFinalizarViagem").hide();
+        } else
+        {
             const agora = new Date();
             const dataAtual = `${agora.getFullYear()}-${String(agora.getMonth() + 1).padStart(2, '0')}-${String(agora.getDate()).padStart(2, '0')}`;
             const horaAtual = `${String(agora.getHours()).padStart(2, '0')}:${String(agora.getMinutes()).padStart(2, '0')}`;
 
-            $('#txtDataFinal')
-                .removeAttr('type')
-                .attr('type', 'date')
-                .val(dataAtual);
-            $('#txtHoraFinal').val(horaAtual);
-            $('#txtKmFinal').val('');
+            $("#txtDataFinal").removeAttr("type").attr("type", "date").val(dataAtual);
+            $("#txtHoraFinal").val(horaAtual);
+            $("#txtKmFinal").val("");
 
             calcularDuracaoViagem();
 
-            if (combFinal?.ej2_instances?.length) {
-                combFinal.ej2_instances[0].value = '';
+            if (combFinal?.ej2_instances?.length)
+            {
+                combFinal.ej2_instances[0].value = "";
                 combFinal.ej2_instances[0].enabled = true;
             }
-            if (rteDescricao?.ej2_instances?.length) {
-                rteDescricao.ej2_instances[0].value = data.descricao || '';
+            if (rteDescricao?.ej2_instances?.length)
+            {
+                rteDescricao.ej2_instances[0].value = data.descricao || "";
                 rteDescricao.ej2_instances[0].readonly = false;
             }
 
-            $('#chkStatusDocumento')
-                .prop('checked', true)
-                .attr('readonly', false);
-            $('#chkStatusCartaoAbastecimento')
-                .prop('checked', true)
-                .attr('readonly', false);
-
-            $('#btnFinalizarViagem').show();
-
-            if (
-                typeof OcorrenciaViagem !== 'undefined' &&
-                OcorrenciaViagem.limparOcorrencias
-            ) {
+            $("#chkStatusDocumento").prop("checked", true).attr("readonly", false);
+            $("#chkStatusCartaoAbastecimento").prop("checked", true).attr("readonly", false);
+
+            $("#btnFinalizarViagem").show();
+
+            if (typeof OcorrenciaViagem !== 'undefined' && OcorrenciaViagem.limparOcorrencias) {
                 OcorrenciaViagem.limparOcorrencias();
             }
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'shown.bs.modal',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "shown.bs.modal", error);
     }
 });
 
-$('#modalFinalizaViagem').on('hide.bs.modal', function () {
-    try {
-        $(
-            '#txtId, #txtDataInicial, #txtHoraInicial, #txtKmInicial, #txtDataFinal, #txtHoraFinal, #txtKmFinal',
-        )
-            .val('')
-            .removeAttr('readonly');
-        $('#txtDataFinal').attr('type', 'date');
-
-        const combInicial = document.getElementById('ddtCombustivelInicial');
-        if (
-            combInicial &&
-            combInicial.ej2_instances &&
-            combInicial.ej2_instances.length > 0
-        ) {
-            combInicial.ej2_instances[0].value = '';
+$("#modalFinalizaViagem").on("hide.bs.modal", function ()
+{
+    try
+    {
+        $("#txtId, #txtDataInicial, #txtHoraInicial, #txtKmInicial, #txtDataFinal, #txtHoraFinal, #txtKmFinal")
+            .val("")
+            .removeAttr("readonly");
+        $("#txtDataFinal").attr("type", "date");
+
+        const combInicial = document.getElementById("ddtCombustivelInicial");
+        if (combInicial && combInicial.ej2_instances && combInicial.ej2_instances.length > 0)
+        {
+            combInicial.ej2_instances[0].value = "";
             combInicial.ej2_instances[0].enabled = true;
         }
 
-        const combFinal = document.getElementById('ddtCombustivelFinal');
-        if (
-            combFinal &&
-            combFinal.ej2_instances &&
-            combFinal.ej2_instances.length > 0
-        ) {
-            combFinal.ej2_instances[0].value = '';
+        const combFinal = document.getElementById("ddtCombustivelFinal");
+        if (combFinal && combFinal.ej2_instances && combFinal.ej2_instances.length > 0)
+        {
+            combFinal.ej2_instances[0].value = "";
             combFinal.ej2_instances[0].enabled = true;
         }
 
-        const combKm = document.getElementById('txtKmPercorrido');
-        if (combKm && combKm.ej2_instances && combKm.ej2_instances.length > 0) {
-            combKm.ej2_instances[0].value = '';
+        const combKm = document.getElementById("txtKmPercorrido");
+        if (combKm && combKm.ej2_instances && combKm.ej2_instances.length > 0)
+        {
+            combKm.ej2_instances[0].value = "";
             combKm.ej2_instances[0].enabled = true;
         }
 
-        const rteDescricao = document.getElementById('rteDescricao');
-        if (
-            rteDescricao &&
-            rteDescricao.ej2_instances &&
-            rteDescricao.ej2_instances.length > 0
-        ) {
-            rteDescricao.ej2_instances[0].value = '';
+        const rteDescricao = document.getElementById("rteDescricao");
+        if (rteDescricao && rteDescricao.ej2_instances && rteDescricao.ej2_instances.length > 0)
+        {
+            rteDescricao.ej2_instances[0].value = "";
             rteDescricao.ej2_instances[0].readonly = false;
         }
 
-        document.getElementById('txtKmPercorrido').value = '';
-        document.getElementById('txtDuracao').value = '';
-
-        $('#chkStatusDocumento, #chkStatusCartaoAbastecimento')
-            .prop('checked', false)
-            .attr('readonly', false);
-        $('#btnFinalizarViagem').show();
-
-        if (
-            typeof OcorrenciaViagem !== 'undefined' &&
-            OcorrenciaViagem.limparOcorrencias
-        ) {
+        document.getElementById("txtKmPercorrido").value = "";
+        document.getElementById("txtDuracao").value = "";
+
+        $("#chkStatusDocumento, #chkStatusCartaoAbastecimento")
+            .prop("checked", false)
+            .attr("readonly", false);
+        $("#btnFinalizarViagem").show();
+
+        if (typeof OcorrenciaViagem !== 'undefined' && OcorrenciaViagem.limparOcorrencias) {
             OcorrenciaViagem.limparOcorrencias();
         }
 
         this.removeAttribute('data-veiculo-id');
         this.removeAttribute('data-motorista-id');
-    } catch (error) {
-        TratamentoErroComLinha('ViagemIndex.js', 'hide.bs.modal', error);
+    } catch (error)
+    {
+        TratamentoErroComLinha("ViagemIndex.js", "hide.bs.modal", error);
     }
 });
 
-function formatDateBR(dateStr) {
-    try {
-        if (dateStr.includes('/')) {
+function formatDateBR(dateStr)
+{
+    try
+    {
+        if (dateStr.includes("/"))
+        {
             return dateStr;
         }
-        const [year, month, day] = dateStr.split('-');
+        const [year, month, day] = dateStr.split("-");
         return `${day}/${month}/${year}`;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('ViagemIndex.js', 'formatDateBR', error);
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "formatDateBR", error);
         return dateStr;
     }
 }
 
-function parseDataBR(dataBR) {
+function parseDataBR(dataBR)
+{
     if (!dataBR) return null;
     const partes = dataBR.split('/');
 
     return new Date(partes[2], partes[1] - 1, partes[0]);
 }
 
-$('#txtDataFinal').focusout(async function () {
-    try {
+$("#txtDataFinal").focusout(async function ()
+{
+    try
+    {
         const rawDataFinal = $(this).val();
         if (!rawDataFinal) return;
 
@@ -1249,75 +1132,74 @@
 
         const modalEl = document.getElementById('modalFinalizaViagem');
         const veiculoId = modalEl?.getAttribute('data-veiculo-id') || '';
-        const DataInicial = $('#txtDataInicial').val();
-        const HoraInicial = $('#txtHoraInicial').val();
-        const HoraFinal = $('#txtHoraFinal').val();
-
-        if (typeof window.ValidadorFinalizacaoIA !== 'undefined') {
+        const DataInicial = $("#txtDataInicial").val();
+        const HoraInicial = $("#txtHoraInicial").val();
+        const HoraFinal = $("#txtHoraFinal").val();
+
+        if (typeof window.ValidadorFinalizacaoIA !== 'undefined')
+        {
             const validador = window.ValidadorFinalizacaoIA;
 
-            const validacaoData =
-                await validador.validarDataNaoFutura(rawDataFinal);
-            if (!validacaoData.valido) {
-                $('#txtDataFinal').val('');
-                $('#txtDuracao').val('');
+            const validacaoData = await validador.validarDataNaoFutura(rawDataFinal);
+            if (!validacaoData.valido)
+            {
+                $("#txtDataFinal").val("");
+                $("#txtDuracao").val("");
                 await mostrarErroValidacaoIA(validacaoData.mensagem);
-                $('#txtDataFinal').focus();
+                $("#txtDataFinal").focus();
                 return;
             }
 
-            if (DataInicial && HoraInicial && HoraFinal) {
+            if (DataInicial && HoraInicial && HoraFinal)
+            {
                 const analiseDatas = await validador.analisarDatasHoras({
                     dataInicial: DataInicial,
                     horaInicial: HoraInicial,
                     dataFinal: rawDataFinal,
                     horaFinal: HoraFinal,
-                    veiculoId: veiculoId,
+                    veiculoId: veiculoId
                 });
 
-                if (!analiseDatas.valido) {
-                    $('#txtDataFinal').val('');
-                    $('#txtDuracao').val('');
+                if (!analiseDatas.valido)
+                {
+                    $("#txtDataFinal").val("");
+                    $("#txtDuracao").val("");
                     await mostrarErroValidacaoIA(analiseDatas.mensagem);
                     return;
                 }
 
-                if (analiseDatas.requerConfirmacao) {
-                    const confirmou = await mostrarConfirmacaoValidacaoIA(
-                        analiseDatas.mensagem,
-                        analiseDatas.nivel,
-                    );
-                    if (!confirmou) {
-                        $('#txtDataFinal').val('');
-                        $('#txtDuracao').val('');
+                if (analiseDatas.requerConfirmacao)
+                {
+                    const confirmou = await mostrarConfirmacaoValidacaoIA(analiseDatas.mensagem, analiseDatas.nivel);
+                    if (!confirmou)
+                    {
+                        $("#txtDataFinal").val("");
+                        $("#txtDuracao").val("");
                         return;
                     }
                     validador._duracaoConfirmada = true;
                 }
             }
         }
-    } catch (error) {
-        TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'focusout.txtDataFinal',
-            error,
-        );
+    } catch (error)
+    {
+        TratamentoErroComLinha("ViagemIndex.js", "focusout.txtDataFinal", error);
     }
 });
 
-$('#txtHoraFinal').focusout(async function () {
-    try {
+$("#txtHoraFinal").focusout(async function ()
+{
+    try
+    {
         const horaFinal = $(this).val();
         if (!horaFinal) return;
 
-        const dataFinal = $('#txtDataFinal').val();
-
-        if (!dataFinal) {
-            $('#txtHoraFinal').val('');
-            await Alerta.Erro(
-                'Campo Obrigatório',
-                'Preencha a <strong>Data Final</strong> antes de preencher a Hora Final.',
-            );
+        const dataFinal = $("#txtDataFinal").val();
+
+        if (!dataFinal)
+        {
+            $("#txtHoraFinal").val("");
+            await Alerta.Erro("Campo Obrigatório", "Preencha a <strong>Data Final</strong> antes de preencher a Hora Final.");
             return;
         }
 
@@ -1325,14 +1207,11 @@
 
         const modalEl = document.getElementById('modalFinalizaViagem');
         const veiculoId = modalEl?.getAttribute('data-veiculo-id') || '';
-        const DataInicial = $('#txtDataInicial').val();
-        const HoraInicial = $('#txtHoraInicial').val();
-
-        if (
-            typeof window.ValidadorFinalizacaoIA !== 'undefined' &&
-            DataInicial &&
-            HoraInicial
-        ) {
+        const DataInicial = $("#txtDataInicial").val();
+        const HoraInicial = $("#txtHoraInicial").val();
+
+        if (typeof window.ValidadorFinalizacaoIA !== 'undefined' && DataInicial && HoraInicial)
+        {
             const validador = window.ValidadorFinalizacaoIA;
 
             const analiseDatas = await validador.analisarDatasHoras({
@@ -1340,161 +1219,161 @@
                 horaInicial: HoraInicial,
                 dataFinal: dataFinal,
                 horaFinal: horaFinal,
-                veiculoId: veiculoId,
+                veiculoId: veiculoId
             });
 
-            if (!analiseDatas.valido) {
-                $('#txtHoraFinal').val('');
-                $('#txtDuracao').val('');
+            if (!analiseDatas.valido)
+            {
+                $("#txtHoraFinal").val("");
+                $("#txtDuracao").val("");
                 await mostrarErroValidacaoIA(analiseDatas.mensagem);
                 return;
             }
 
-            if (
-                analiseDatas.requerConfirmacao &&
-                !validador._duracaoConfirmada
-            ) {
-                const confirmou = await mostrarConfirmacaoValidacaoIA(
-                    analiseDatas.mensagem,
-                    analiseDatas.nivel,
-                );
-                if (!confirmou) {
-                    $('#txtHoraFinal').val('');
-                    $('#txtDuracao').val('');
+            if (analiseDatas.requerConfirmacao && !validador._duracaoConfirmada)
+            {
+                const confirmou = await mostrarConfirmacaoValidacaoIA(analiseDatas.mensagem, analiseDatas.nivel);
+                if (!confirmou)
+                {
+                    $("#txtHoraFinal").val("");
+                    $("#txtDuracao").val("");
                     return;
                 }
                 validador._duracaoConfirmada = true;
             }
         }
-    } catch (error) {
-        TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'focusout.txtHoraFinal',
-            error,
-        );
+    } catch (error)
+    {
+        TratamentoErroComLinha("ViagemIndex.js", "focusout.txtHoraFinal", error);
     }
 });
 
-$('#txtKmInicial').focusout(function () {
-    try {
-        const kmInicialStr = $('#txtKmInicial').val();
-        const kmAtualStr = $('#txtKmAtual').val();
-
-        if (!kmInicialStr || !kmAtualStr) {
-            $('#txtKmPercorrido').val('');
+$("#txtKmInicial").focusout(function ()
+{
+    try
+    {
+        const kmInicialStr = $("#txtKmInicial").val();
+        const kmAtualStr = $("#txtKmAtual").val();
+
+        if (!kmInicialStr || !kmAtualStr)
+        {
+            $("#txtKmPercorrido").val("");
             return;
         }
 
-        const kmInicial = parseFloat(kmInicialStr.replace(',', '.'));
-        const kmAtual = parseFloat(kmAtualStr.replace(',', '.'));
-
-        if (isNaN(kmInicial) || isNaN(kmAtual)) {
-            $('#txtKmPercorrido').val('');
+        const kmInicial = parseFloat(kmInicialStr.replace(",", "."));
+        const kmAtual = parseFloat(kmAtualStr.replace(",", "."));
+
+        if (isNaN(kmInicial) || isNaN(kmAtual))
+        {
+            $("#txtKmPercorrido").val("");
             return;
         }
 
-        if (kmInicial < 0) {
-            $('#txtKmInicial').val('');
-            $('#txtKmPercorrido').val('');
-            Alerta.Erro(
-                'Erro na Quilometragem',
-                'A quilometragem <strong>inicial</strong> deve ser maior que <strong>zero</strong>!',
-            );
+        if (kmInicial < 0)
+        {
+            $("#txtKmInicial").val("");
+            $("#txtKmPercorrido").val("");
+            Alerta.Erro("Erro na Quilometragem", "A quilometragem <strong>inicial</strong> deve ser maior que <strong>zero</strong>!");
             return;
         }
 
-        if (kmInicial < kmAtual) {
-            $('#txtKmAtual').val('');
-            $('#txtKmPercorrido').val('');
-            Alerta.Erro(
-                'Erro na Quilometragem',
-                'A quilometragem <strong>inicial</strong> deve ser maior que a <strong>atual</strong>!',
-            );
+        if (kmInicial < kmAtual)
+        {
+            $("#txtKmAtual").val("");
+            $("#txtKmPercorrido").val("");
+            Alerta.Erro("Erro na Quilometragem", "A quilometragem <strong>inicial</strong> deve ser maior que a <strong>atual</strong>!");
             return;
         }
 
         calcularDistanciaViagem();
-    } catch (error) {
-        TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'focusout.txtKmInicial',
-            error,
-        );
+    } catch (error)
+    {
+        TratamentoErroComLinha("ViagemIndex.js", "focusout.txtKmInicial", error);
     }
 });
 
-$('#txtKmFinal').focusout(async function () {
-    try {
+$("#txtKmFinal").focusout(async function ()
+{
+    try
+    {
         const kmFinalStr = $(this).val();
         if (!kmFinalStr) return;
 
-        const kmInicialStr = $('#txtKmInicial').val();
-        if (!kmInicialStr) {
-            $('#txtKmPercorrido').val('');
+        const kmInicialStr = $("#txtKmInicial").val();
+        if (!kmInicialStr)
+        {
+            $("#txtKmPercorrido").val("");
             return;
         }
 
-        const kmInicial = parseFloat(kmInicialStr.replace(',', '.'));
-        const kmFinal = parseFloat(kmFinalStr.replace(',', '.'));
-
-        if (isNaN(kmInicial) || isNaN(kmFinal)) {
-            $('#txtKmPercorrido').val('');
+        const kmInicial = parseFloat(kmInicialStr.replace(",", "."));
+        const kmFinal = parseFloat(kmFinalStr.replace(",", "."));
+
+        if (isNaN(kmInicial) || isNaN(kmFinal))
+        {
+            $("#txtKmPercorrido").val("");
             return;
         }
 
         const kmPercorrido = (kmFinal - kmInicial).toFixed(2);
-        $('#txtKmPercorrido').val(kmPercorrido);
+        $("#txtKmPercorrido").val(kmPercorrido);
         calcularDistanciaViagem();
 
         const modalEl = document.getElementById('modalFinalizaViagem');
         const veiculoId = modalEl?.getAttribute('data-veiculo-id') || '';
 
-        if (typeof window.ValidadorFinalizacaoIA !== 'undefined') {
+        if (typeof window.ValidadorFinalizacaoIA !== 'undefined')
+        {
             const validador = window.ValidadorFinalizacaoIA;
 
             const analiseKm = await validador.analisarKm({
                 kmInicial: kmInicialStr,
                 kmFinal: kmFinalStr,
-                veiculoId: veiculoId,
+                veiculoId: veiculoId
             });
 
-            if (!analiseKm.valido) {
-                $('#txtKmFinal').val('');
-                $('#txtKmPercorrido').val('');
+            if (!analiseKm.valido)
+            {
+                $("#txtKmFinal").val("");
+                $("#txtKmPercorrido").val("");
                 await mostrarErroValidacaoIA(analiseKm.mensagem);
                 return;
             }
 
-            if (analiseKm.requerConfirmacao && !validador._kmConfirmado) {
-                const confirmou = await mostrarConfirmacaoValidacaoIA(
-                    analiseKm.mensagem,
-                    analiseKm.nivel,
-                );
-                if (!confirmou) {
-                    $('#txtKmFinal').val('');
-                    $('#txtKmPercorrido').val('');
+            if (analiseKm.requerConfirmacao && !validador._kmConfirmado)
+            {
+                const confirmou = await mostrarConfirmacaoValidacaoIA(analiseKm.mensagem, analiseKm.nivel);
+                if (!confirmou)
+                {
+                    $("#txtKmFinal").val("");
+                    $("#txtKmPercorrido").val("");
                     return;
                 }
                 validador._kmConfirmado = true;
             }
         }
-    } catch (error) {
-        TratamentoErroComLinha('ViagemIndex.js', 'focusout.txtKmFinal', error);
+    } catch (error)
+    {
+        TratamentoErroComLinha("ViagemIndex.js", "focusout.txtKmFinal", error);
     }
 });
 
-function calcularDistanciaViagem() {
-    try {
-        var elKmIni = document.getElementById('txtKmInicial');
-        var elKmFim = document.getElementById('txtKmFinal');
-        var elPerc = document.getElementById('txtKmPercorrido');
+function calcularDistanciaViagem()
+{
+    try
+    {
+        var elKmIni = document.getElementById("txtKmInicial");
+        var elKmFim = document.getElementById("txtKmFinal");
+        var elPerc = document.getElementById("txtKmPercorrido");
         if (!elKmIni || !elKmFim || !elPerc) return;
 
-        var ini = parseFloat((elKmIni.value || '').replace(',', '.'));
-        var fim = parseFloat((elKmFim.value || '').replace(',', '.'));
-
-        if (isNaN(ini) || isNaN(fim)) {
-            elPerc.value = '';
+        var ini = parseFloat((elKmIni.value || '').replace(",", "."));
+        var fim = parseFloat((elKmFim.value || '').replace(",", "."));
+
+        if (isNaN(ini) || isNaN(fim))
+        {
+            elPerc.value = "";
             FieldUX.setInvalid(elPerc, false);
             FieldUX.setHigh(elPerc, false);
             FieldUX.tooltipOnTransition(elPerc, false, 1, 'tooltipKm');
@@ -1502,70 +1381,70 @@
         }
 
         var diff = +(fim - ini).toFixed(2);
-        elPerc.value = isFinite(diff) ? diff : '';
-
-        var invalid = diff < 0 || diff > 100;
-        var high = !invalid && diff >= 50 && diff <= 100;
+        elPerc.value = isFinite(diff) ? diff : "";
+
+        var invalid = (diff < 0 || diff > 100);
+        var high = (!invalid && diff >= 50 && diff <= 100);
 
         FieldUX.setInvalid(elPerc, invalid);
         FieldUX.setHigh(elPerc, high);
-        FieldUX.tooltipOnTransition(
-            elPerc,
-            invalid && !window.CarregandoViagemBloqueada,
-            1200,
-            'tooltipKm',
-        );
-    } catch (error) {
-        if (typeof TratamentoErroComLinha === 'function') {
-            TratamentoErroComLinha(
-                'ViagemIndex.js',
-                'calcularDistanciaViagem',
-                error,
-            );
-        } else {
-            console.error(error);
-        }
+        FieldUX.tooltipOnTransition(elPerc, invalid && !window.CarregandoViagemBloqueada, 1200, 'tooltipKm');
+    } catch (error)
+    {
+        if (typeof TratamentoErroComLinha === 'function')
+        {
+            TratamentoErroComLinha("ViagemIndex.js", "calcularDistanciaViagem", error);
+        } else { console.error(error); }
     }
 }
 
-window.calcularKmPercorrido =
-    window.calcularKmPercorrido || calcularDistanciaViagem;
-
-function parseDate(d) {
-    try {
+window.calcularKmPercorrido = window.calcularKmPercorrido || calcularDistanciaViagem;
+
+function parseDate(d)
+{
+    try
+    {
         if (!d) return null;
-        if (d.includes('/')) {
-            const [dia, mes, ano] = d.split('/');
+        if (d.includes("/"))
+        {
+            const [dia, mes, ano] = d.split("/");
             return new Date(ano, mes - 1, dia);
         }
-        if (d.includes('-')) {
-            const [ano, mes, dia] = d.split('-');
+        if (d.includes("-"))
+        {
+            const [ano, mes, dia] = d.split("-");
             return new Date(ano, mes - 1, dia);
         }
         return null;
-    } catch (error) {
-        TratamentoErroComLinha('ViagemIndex.js', 'parseDate', error);
+    } catch (error)
+    {
+        TratamentoErroComLinha("ViagemIndex.js", "parseDate", error);
         return null;
     }
 }
 
-async function validarDatasSimples() {
-    try {
-        const dataInicialStr = $('#txtDataInicial').val();
-        const dataFinalInput = $('#txtDataFinal');
+async function validarDatasSimples()
+{
+    try
+    {
+        const dataInicialStr = $("#txtDataInicial").val();
+        const dataFinalInput = $("#txtDataFinal");
         const dataFinalStr = dataFinalInput.val();
 
-        if (dataInicialStr === '') {
-            Alerta.Erro('Erro na Data', 'A data inicial é obrigatória!');
+        if (dataInicialStr === "")
+        {
+            Alerta.Erro("Erro na Data", "A data inicial é obrigatória!");
             return false;
         }
 
-        if (dataInicialStr !== '' && dataFinalStr !== '') {
+        if (dataInicialStr !== "" && dataFinalStr !== "")
+        {
             const dtInicial = parseDate(dataInicialStr);
             const dtFinal = parseDate(dataFinalStr);
 
-            if (!dtInicial || !dtFinal) {
-                Alerta.Erro('Erro na Data', 'Formato de data inválido!');
+            if (!dtInicial || !dtFinal)
+            {
+                Alerta.Erro("Erro na Data", "Formato de data inválido!");
                 return false;
             }
 
@@ -1574,30 +1453,22 @@
 
             const diferencaDias = (dtFinal - dtInicial) / (1000 * 60 * 60 * 24);
 
-            if (diferencaDias >= 5) {
-                const mensagem =
-                    'A Data Final está 5 dias ou mais após a Data Inicial. Tem certeza?';
-                const confirmado =
-                    await window.SweetAlertInterop.ShowPreventionAlert(
-                        mensagem,
-                    );
-
-                if (confirmado) {
-                    showSyncfusionToast(
-                        'Confirmação feita pelo usuário!',
-                        'success',
-                        '💪🏼',
-                    );
-                } else {
-                    showSyncfusionToast(
-                        'Ação cancelada pelo usuário',
-                        'danger',
-                        '😟',
-                    );
-
-                    const campo = document.getElementById('txtDataFinal');
-                    if (campo) {
-                        campo.value = '';
+            if (diferencaDias >= 5)
+            {
+                const mensagem = "A Data Final está 5 dias ou mais após a Data Inicial. Tem certeza?";
+                const confirmado = await window.SweetAlertInterop.ShowPreventionAlert(mensagem);
+
+                if (confirmado)
+                {
+                    showSyncfusionToast("Confirmação feita pelo usuário!", "success", "💪🏼");
+                } else
+                {
+                    showSyncfusionToast("Ação cancelada pelo usuário", "danger", "😟");
+
+                    const campo = document.getElementById("txtDataFinal");
+                    if (campo)
+                    {
+                        campo.value = "";
                         campo.focus();
                         return false;
                     }
@@ -1606,43 +1477,45 @@
         }
 
         return true;
-    } catch (error) {
-        TratamentoErroComLinha('ViagemIndex.js', 'validarDatasSimples', error);
+    } catch (error)
+    {
+        TratamentoErroComLinha("ViagemIndex.js", "validarDatasSimples", error);
         return false;
     }
 }
 
-async function validarKmAtualFinal() {
-    try {
-        const kmInicial = $('#txtKmInicial').val();
-        const kmAtual = $('#txtKmAtual').val();
+async function validarKmAtualFinal()
+{
+    try
+    {
+        const kmInicial = $("#txtKmInicial").val();
+        const kmAtual = $("#txtKmAtual").val();
 
         if (!kmInicial || !kmAtual) return true;
 
-        const ini = parseFloat(kmAtual.replace(',', '.'));
-        const fim = parseFloat(kmInicial.replace(',', '.'));
-
-        if (fim < ini) {
-            Alerta.Erro(
-                'Erro',
-                'A quilometragem <strong>inicial</strong> deve ser maior que a <strong>atual</strong>.',
+        const ini = parseFloat(kmAtual.replace(",", "."));
+        const fim = parseFloat(kmInicial.replace(",", "."));
+
+        if (fim < ini)
+        {
+            Alerta.Erro("Erro", "A quilometragem <strong>inicial</strong> deve ser maior que a <strong>atual</strong>.");
+            return false;
+        }
+
+        const diff = fim - ini;
+
+        if (diff > 100)
+        {
+            const confirmado = await Alerta.Confirmar(
+                "Quilometragem Alta",
+                'A quilometragem <span style="color: #A0522D; font-weight: bold; text-decoration: underline;">Inicial</span> excede em 100km a <span style="color: #A0522D; font-weight: bold; text-decoration: underline;">Atual</span>. Tem certeza?',
+                "Tenho certeza! 💪🏼",
+                "Me enganei! 😟"
             );
-            return false;
-        }
-
-        const diff = fim - ini;
-
-        if (diff > 100) {
-            const confirmado = await Alerta.Confirmar(
-                'Quilometragem Alta',
-                'A quilometragem <span style="color: #A0522D; font-weight: bold; text-decoration: underline;">Inicial</span> excede em 100km a <span style="color: #A0522D; font-weight: bold; text-decoration: underline;">Atual</span>. Tem certeza?',
-                'Tenho certeza! 💪🏼',
-                'Me enganei! 😟',
-            );
-
-            if (!confirmado) {
-                const txtKmInicialElement =
-                    document.getElementById('txtKmInicial');
+
+            if (!confirmado)
+            {
+                const txtKmInicialElement = document.getElementById("txtKmInicial");
                 txtKmInicialElement.value = null;
                 txtKmInicialElement.focus();
                 return false;
@@ -1650,41 +1523,44 @@
         }
 
         return true;
-    } catch (error) {
-        TratamentoErroComLinha('ViagemIndex.js', 'validarKmAtualFinal', error);
+    } catch (error)
+    {
+        TratamentoErroComLinha("ViagemIndex.js", "validarKmAtualFinal", error);
         return false;
     }
 }
 
-async function validarKmInicialFinal() {
-    try {
-        const kmInicial = $('#txtKmInicial').val();
-        const kmFinal = $('#txtKmFinal').val();
+async function validarKmInicialFinal()
+{
+    try
+    {
+        const kmInicial = $("#txtKmInicial").val();
+        const kmFinal = $("#txtKmFinal").val();
 
         if (!kmInicial || !kmFinal) return true;
 
-        const ini = parseFloat(kmInicial.replace(',', '.'));
-        const fim = parseFloat(kmFinal.replace(',', '.'));
-
-        if (fim < ini) {
-            Alerta.Erro(
-                'Erro',
-                'A quilometragem final deve ser maior que a inicial.',
+        const ini = parseFloat(kmInicial.replace(",", "."));
+        const fim = parseFloat(kmFinal.replace(",", "."));
+
+        if (fim < ini)
+        {
+            Alerta.Erro("Erro", "A quilometragem final deve ser maior que a inicial.");
+            return false;
+        }
+
+        const diff = fim - ini;
+        if (diff > 100)
+        {
+            const confirmado = await Alerta.Confirmar(
+                "Quilometragem Alta",
+                'A quilometragem <span style="color: #A0522D; font-weight: bold; text-decoration: underline;">Final</span> excede em 100km a <span style="color: #A0522D; font-weight: bold; text-decoration: underline;">Inicial</span>. Tem certeza?',
+                "Tenho certeza! 💪🏼",
+                "Me enganei! 😟"
             );
-            return false;
-        }
-
-        const diff = fim - ini;
-        if (diff > 100) {
-            const confirmado = await Alerta.Confirmar(
-                'Quilometragem Alta',
-                'A quilometragem <span style="color: #A0522D; font-weight: bold; text-decoration: underline;">Final</span> excede em 100km a <span style="color: #A0522D; font-weight: bold; text-decoration: underline;">Inicial</span>. Tem certeza?',
-                'Tenho certeza! 💪🏼',
-                'Me enganei! 😟',
-            );
-
-            if (!confirmado) {
-                const txtKmFinalElement = document.getElementById('txtKmFinal');
+
+            if (!confirmado)
+            {
+                const txtKmFinalElement = document.getElementById("txtKmFinal");
                 txtKmFinalElement.value = null;
                 txtKmFinalElement.focus();
                 return false;
@@ -1692,301 +1568,267 @@
         }
 
         return true;
-    } catch (error) {
-        TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'validarKmInicialFinal',
-            error,
-        );
+    } catch (error)
+    {
+        TratamentoErroComLinha("ViagemIndex.js", "validarKmInicialFinal", error);
         return false;
     }
 }
 
-function ListaTodasViagens() {
-    try {
-        let veiculoId = '';
-        const veiculosCombo = document.getElementById('lstVeiculos');
-        if (
-            veiculosCombo &&
-            veiculosCombo.ej2_instances &&
-            veiculosCombo.ej2_instances.length > 0
-        ) {
+function ListaTodasViagens()
+{
+    try
+    {
+        let veiculoId = "";
+        const veiculosCombo = document.getElementById("lstVeiculos");
+        if (veiculosCombo && veiculosCombo.ej2_instances && veiculosCombo.ej2_instances.length > 0)
+        {
             const combo = veiculosCombo.ej2_instances[0];
-            if (combo.value != null && combo.value !== '') {
+            if (combo.value != null && combo.value !== "")
+            {
                 veiculoId = combo.value;
             }
         }
 
-        let motoristaId = '';
-        const motoristasCombo = document.getElementById('lstMotorista');
-        if (
-            motoristasCombo &&
-            motoristasCombo.ej2_instances &&
-            motoristasCombo.ej2_instances.length > 0
-        ) {
+        let motoristaId = "";
+        const motoristasCombo = document.getElementById("lstMotorista");
+        if (motoristasCombo && motoristasCombo.ej2_instances && motoristasCombo.ej2_instances.length > 0)
+        {
             const combo = motoristasCombo.ej2_instances[0];
-            if (combo.value != null && combo.value !== '') {
+            if (combo.value != null && combo.value !== "")
+            {
                 motoristaId = combo.value;
             }
         }
 
-        let eventoId = '';
-        const eventosCombo = document.getElementById('lstEventos');
-        if (
-            eventosCombo &&
-            eventosCombo.ej2_instances &&
-            eventosCombo.ej2_instances.length > 0
-        ) {
+        let eventoId = "";
+        const eventosCombo = document.getElementById("lstEventos");
+        if (eventosCombo && eventosCombo.ej2_instances && eventosCombo.ej2_instances.length > 0)
+        {
             const combo = eventosCombo.ej2_instances[0];
-            if (combo.value != null && combo.value !== '') {
+            if (combo.value != null && combo.value !== "")
+            {
                 eventoId = combo.value;
             }
         }
 
-        let statusId = 'Aberta';
-        const statusCombo = document.getElementById('lstStatus');
-        if (
-            statusCombo &&
-            statusCombo.ej2_instances &&
-            statusCombo.ej2_instances.length > 0
-        ) {
+        let statusId = "Aberta";
+        const statusCombo = document.getElementById("lstStatus");
+        if (statusCombo && statusCombo.ej2_instances && statusCombo.ej2_instances.length > 0)
+        {
             const status = statusCombo.ej2_instances[0];
-            if (status.value === '' || status.value === null) {
-                if (
-                    motoristaId ||
-                    veiculoId ||
-                    eventoId ||
-                    ($('#txtData').val() != null && $('#txtData').val() !== '')
-                ) {
-                    statusId = 'Todas';
-                }
-            } else {
+            if (status.value === "" || status.value === null)
+            {
+                if (motoristaId || veiculoId || eventoId || ($("#txtData").val() != null && $("#txtData").val() !== ""))
+                {
+                    statusId = "Todas";
+                }
+            } else
+            {
                 statusId = status.value;
             }
         }
 
-        const dateVal = $('#txtData').val();
-        const date = dateVal ? dateVal.split('-') : null;
-        const dataViagem =
-            date && date.length === 3 ? `${date[2]}/${date[1]}/${date[0]}` : '';
-
-        const URLapi = '/api/viagem';
-
-        let dataTableViagens = $('#tblViagem').DataTable();
+        const dateVal = $("#txtData").val();
+        const date = dateVal ? dateVal.split("-") : null;
+        const dataViagem = date && date.length === 3 ? `${date[2]}/${date[1]}/${date[0]}` : "";
+
+        const URLapi = "/api/viagem";
+
+        let dataTableViagens = $("#tblViagem").DataTable();
         dataTableViagens.destroy();
-        $('#tblViagem tbody').empty();
-
-        dataTableViagens = $('#tblViagem').DataTable({
+        $("#tblViagem tbody").empty();
+
+        dataTableViagens = $("#tblViagem").DataTable({
             autoWidth: false,
-            dom: 'Bfrtip',
+            dom: "Bfrtip",
             deferRender: true,
             stateSave: false,
             lengthMenu: [
                 [10, 25, 50, -1],
-                ['10 linhas', '25 linhas', '50 linhas', 'Todas as Linhas'],
+                ["10 linhas", "25 linhas", "50 linhas", "Todas as Linhas"]
             ],
             buttons: [
-                'pageLength',
-                'excel',
-                {
-                    extend: 'pdfHtml5',
-                    orientation: 'landscape',
-                    pageSize: 'LEGAL',
-                },
+                "pageLength",
+                "excel",
+                {
+                    extend: "pdfHtml5",
+                    orientation: "landscape",
+                    pageSize: "LEGAL"
+                }
             ],
             order: [],
             columnDefs: [
-                { targets: 0, className: 'text-center', width: '3%' },
-                { targets: 1, className: 'text-center', width: '3%' },
-                { targets: 2, className: 'text-center', width: '3%' },
-                { targets: 3, className: 'text-left', width: '10%' },
-                { targets: 4, className: 'text-left', width: '10%' },
-                { targets: 5, className: 'text-left', width: '10%' },
-                { targets: 6, className: 'text-left', width: '10%' },
-                { targets: 7, className: 'text-center', width: '4%' },
-                { targets: 8, className: 'text-center', width: '6%' },
-                {
-                    targets: 9,
-                    className: 'text-center',
-                    width: '1%',
-                    visible: false,
-                },
-                { targets: 10, className: 'text-center', visible: false },
-                { targets: 11, className: 'text-center', visible: false },
-                { targets: 12, className: 'text-center', visible: false },
-                { targets: 13, className: 'text-center', visible: false },
-                { targets: 14, className: 'text-center', visible: false },
-                { targets: 15, className: 'text-center', visible: false },
-                { targets: 16, className: 'text-center', visible: false },
-                { targets: 17, className: 'text-center', visible: false },
-                { targets: 18, className: 'text-center', visible: false },
-                { targets: 19, className: 'text-center', visible: false },
-                { targets: 20, className: 'text-center', visible: false },
+                { targets: 0, className: "text-center", width: "3%" },
+                { targets: 1, className: "text-center", width: "3%" },
+                { targets: 2, className: "text-center", width: "3%" },
+                { targets: 3, className: "text-left", width: "10%" },
+                { targets: 4, className: "text-left", width: "10%" },
+                { targets: 5, className: "text-left", width: "10%" },
+                { targets: 6, className: "text-left", width: "10%" },
+                { targets: 7, className: "text-center", width: "4%" },
+                { targets: 8, className: "text-center", width: "6%" },
+                { targets: 9, className: "text-center", width: "1%", visible: false },
+                { targets: 10, className: "text-center", visible: false },
+                { targets: 11, className: "text-center", visible: false },
+                { targets: 12, className: "text-center", visible: false },
+                { targets: 13, className: "text-center", visible: false },
+                { targets: 14, className: "text-center", visible: false },
+                { targets: 15, className: "text-center", visible: false },
+                { targets: 16, className: "text-center", visible: false },
+                { targets: 17, className: "text-center", visible: false },
+                { targets: 18, className: "text-center", visible: false },
+                { targets: 19, className: "text-center", visible: false },
+                { targets: 20, className: "text-center", visible: false }
             ],
             responsive: true,
             ajax: {
                 url: URLapi,
-                type: 'GET',
+                type: "GET",
                 data: {
                     veiculoId: veiculoId,
                     motoristaId: motoristaId,
                     statusId: statusId,
                     dataviagem: dataViagem,
-                    eventoId: eventoId,
+                    eventoId: eventoId
                 },
-                datatype: 'json',
+                datatype: "json"
             },
             columns: [
-                { data: 'noFichaVistoria' },
-                {
-                    data: 'dataInicial',
-                    render: function (data) {
-                        try {
-                            if (!data) return '';
+                { data: "noFichaVistoria" },
+                {
+                    data: "dataInicial",
+                    render: function (data)
+                    {
+                        try
+                        {
+                            if (!data) return "";
                             var date = new Date(data);
-                            return date.toLocaleDateString('pt-BR');
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'ViagemIndex.js',
-                                'render.dataInicial',
-                                error,
-                            );
-                            return '';
+                            return date.toLocaleDateString("pt-BR");
+                        } catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("ViagemIndex.js", "render.dataInicial", error);
+                            return "";
                         }
-                    },
+                    }
                 },
                 {
-                    data: 'horaInicio',
-                    render: function (data) {
-                        try {
-                            if (!data) return '';
+                    data: "horaInicio",
+                    render: function (data)
+                    {
+                        try
+                        {
+                            if (!data) return "";
                             var date = new Date(data);
-                            return date.toLocaleTimeString('pt-BR', {
-                                hour: '2-digit',
-                                minute: '2-digit',
+                            return date.toLocaleTimeString("pt-BR", {
+                                hour: "2-digit",
+                                minute: "2-digit"
                             });
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'ViagemIndex.js',
-                                'render.horaInicio',
-                                error,
-                            );
-                            return '';
+                        } catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("ViagemIndex.js", "render.horaInicio", error);
+                            return "";
                         }
-                    },
+                    }
                 },
-                { data: 'nomeRequisitante' },
-                { data: 'nomeSetor' },
+                { data: "nomeRequisitante" },
+                { data: "nomeSetor" },
                 {
                     data: null,
-                    name: 'nomeMotorista',
-                    render: ftxRenderMotorista,
+                    name: "nomeMotorista",
+                    render: ftxRenderMotorista
                 },
-                { data: 'descricaoVeiculo' },
-
-                {
-                    data: 'status',
-                    render: function (data, type, row, meta) {
-                        try {
-                            if (row.status === 'Aberta') {
-                                return `<span class="ftx-viagem-badge ftx-viagem-badge-aberta">
-                                            <i class="fa-duotone fa-circle-check"></i> Aberta
+                { data: "descricaoVeiculo" },
+
+                {
+                    data: "status",
+                    render: function (data, type, row, meta)
+                    {
+                        try
+                        {
+
+                            const statusNormalizado = (row.status || "").trim().toLowerCase();
+
+                            if (statusNormalizado === "cancelada")
+                            {
+                                return `<span class="ftx-viagem-badge ftx-viagem-badge-cancelada">
+                                            <i class="fa-solid fa-xmark"></i> Cancelada
                                         </span>`;
                             }
-                            if (row.status === 'Realizada') {
+
+                            if (statusNormalizado === "realizada")
+                            {
                                 return `<span class="ftx-viagem-badge ftx-viagem-badge-realizada">
-                                            <i class="fa-duotone fa-lock"></i> Realizada
+                                            <i class="fa-solid fa-lock"></i> Realizada
                                         </span>`;
                             }
 
-                            return `<span class="ftx-viagem-badge ftx-viagem-badge-cancelada">
-                                        <i class="fa-duotone fa-xmark"></i> Cancelada
+                            return `<span class="ftx-viagem-badge ftx-viagem-badge-aberta">
+                                        <i class="fa-solid fa-circle-check"></i> Aberta
                                     </span>`;
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'ViagemIndex.js',
-                                'render.status',
-                                error,
-                            );
-                            return '';
+                        } catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("ViagemIndex.js", "render.status", error);
+                            return "";
                         }
-                    },
+                    }
                 },
 
                 {
-                    data: 'viagemId',
+                    data: "viagemId",
                     orderable: false,
                     searchable: false,
-                    render: function (data, type, row, meta) {
-                        try {
-                            const isAberta = row.status === 'Aberta';
-                            const disabledClass = isAberta ? '' : 'disabled';
-
-                            const temFicha =
-                                row.temFichaVistoriaReal === true ||
-                                row.temFichaVistoriaReal === 1;
-                            const fichaDisabledClass = !temFicha
-                                ? 'disabled'
-                                : '';
-
-                            const tooltipEditar = 'Editar a Viagem';
-                            const tooltipFinalizar = isAberta
-                                ? 'Finalizar a Viagem'
-                                : 'Somente viagens Abertas podem ser finalizadas';
-                            const tooltipCancelar = isAberta
-                                ? 'Cancelar a Viagem'
-                                : 'Somente viagens Abertas podem ser canceladas';
-                            const tooltipImprimir = 'Imprimir Ficha da Viagem';
-                            const tooltipFichaVistoria = !temFicha
-                                ? 'Sem ficha de vistoria cadastrada'
-                                : 'Visualizar Ficha de Vistoria';
-                            const tooltipCustos = isAberta
-                                ? 'Disponível após finalizar a viagem'
-                                : 'Custos da Viagem';
-                            const tooltipOcorrencias = isAberta
-                                ? 'Disponível após finalizar a viagem'
-                                : 'Ocorrências da Viagem';
+                    render: function (data, type, row, meta)
+                    {
+                        try
+                        {
+
+                            const statusNormalizado = (row.status || "").trim().toLowerCase();
+
+                            const isAberta = statusNormalizado === "aberta" || statusNormalizado === "";
+                            const disabledAttrs = isAberta ? "" : 'class="disabled" aria-disabled="true" tabindex="-1"';
+                            const disabledClass = isAberta ? "" : "disabled";
 
                             return `
                                     <div class="text-center ftx-actions">
                                         <!-- Editar -->
                                         <a href="/Viagens/Upsert?id=${data}"
                                            class="btn btn-azul text-white btn-icon-28"
-                                           data-ejtip="${tooltipEditar}">
+                                           data-ejtip="Editar a Viagem">
                                             <i class="fad fa-pen-to-square"></i>
                                         </a>
 
                                         <!-- Finalizar (abre modal Bootstrap 5) -->
                                         <a class="btn btn-fundo-laranja text-white btn-icon-28 ${disabledClass}"
                                            href="javascript:void(0)"
-                                           ${isAberta ? 'data-bs-toggle="modal" data-bs-target="#modalFinalizaViagem"' : ''}
+                                           data-bs-toggle="modal" data-bs-target="#modalFinalizaViagem"
                                            data-id="${data}"
-                                           data-ejtip="${tooltipFinalizar}">
+                                           data-ejtip="Finalizar a Viagem">
                                             <i class="fad fa-flag-checkered"></i>
                                         </a>
 
                                         <!-- Cancelar (desabilitado se não estiver Aberta) -->
                                         <a class="btn btn-cancela-viagem btn-vinho text-white btn-icon-28 ${disabledClass}"
-                                           href="javascript:void(0)"
+                                           href="javascript:void(0)" ${disabledAttrs}
                                            data-id="${data}"
-                                           data-ejtip="${tooltipCancelar}">
+                                           data-ejtip="Cancelar a Viagem">
                                             <i class="fad fa-rectangle-xmark"></i>
                                         </a>
 
-                                        <!-- Imprimir -->
                                         <a class="btn btn-imprimir text-white btn-icon-28 btn-glow"
                                            href="javascript:void(0)"
                                            role="button"
                                            data-viagem-id="${data}"
-                                           data-ejtip="${tooltipImprimir}">
+                                           data-ejtip="Ficha da Viagem">
                                             <i class="fad fa-print"></i>
                                         </a>
 
-                                        <!-- Ficha de Vistoria (desabilitado se não tem ficha) -->
-                                        <a class="btn btn-foto text-white btn-icon-28 ${fichaDisabledClass}"
+                                        <!-- Ficha de Vistoria (desabilitado se aberta) -->
+                                        <a class="btn btn-foto text-white btn-icon-28 ${isAberta ? 'disabled' : ''}"
                                            href="javascript:void(0)"
-                                           ${temFicha ? `onclick="abrirModalFicha('${data}')"` : ''}
-                                           data-ejtip="${tooltipFichaVistoria}">
+                                           ${isAberta ? '' : `onclick="abrirModalFicha('${data}')"`}
+                                           ${isAberta ? 'tabindex="-1" aria-disabled="true"' : ''}
+                                           data-ejtip="Ficha de Vistoria">
                                             <i class="fad fa-clipboard-check"></i>
                                         </a>
 
@@ -1994,7 +1836,8 @@
                                         <a class="btn btn-custos-viagem text-white btn-icon-28 ${isAberta ? 'disabled' : ''}"
                                            href="javascript:void(0)"
                                            data-id="${data}"
-                                           data-ejtip="${tooltipCustos}">
+                                           ${isAberta ? 'tabindex="-1" aria-disabled="true"' : ''}
+                                           data-ejtip="Custos da Viagem">
                                             <i class="fad fa-money-bill-wave"></i>
                                         </a>
 
@@ -2003,103 +1846,93 @@
                                            href="javascript:void(0)"
                                            data-id="${data}"
                                            data-noficha="${row.noFichaVistoria || ''}"
-                                           data-ejtip="${tooltipOcorrencias}">
+                                           ${isAberta ? 'tabindex="-1" aria-disabled="true"' : ''}
+                                           data-ejtip="Ocorrências da Viagem">
                                             <i class="fad fa-car-burst"></i>
                                         </a>
                                     </div>`;
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'ViagemIndex.js',
-                                'render.viagemId',
-                                error,
-                            );
-                            return '';
+                        } catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("ViagemIndex.js", "render.viagemId", error);
+                            return "";
                         }
-                    },
+                    }
                 },
-                { data: 'kmInicial' },
-                { data: 'combustivelInicial' },
-                { data: 'dataFinal' },
-                { data: 'horaFim' },
-                { data: 'kmFinal' },
-                { data: 'combustivelFinal' },
-                { data: 'resumoOcorrencia' },
-                { data: 'descricaoOcorrencia' },
-                { data: 'statusDocumento' },
-                { data: 'statusCartaoAbastecimento' },
-                { data: 'descricao' },
-                { data: 'imagemOcorrencia' },
+                { data: "kmInicial" },
+                { data: "combustivelInicial" },
+                { data: "dataFinal" },
+                { data: "horaFim" },
+                { data: "kmFinal" },
+                { data: "combustivelFinal" },
+                { data: "resumoOcorrencia" },
+                { data: "descricaoOcorrencia" },
+                { data: "statusDocumento" },
+                { data: "statusCartaoAbastecimento" },
+                { data: "descricao" },
+                { data: "imagemOcorrencia" }
             ],
-            rowCallback: function (row) {
-                try {
+            rowCallback: function (row)
+            {
+                try
+                {
                     ftxBindLazyImgsInRow(row);
-                } catch (e) {
-                    console.error('Erro no rowCallback:', e);
+                } catch (e)
+                {
+                    console.error("Erro no rowCallback:", e);
                 }
             },
             language: {
-                emptyTable: 'Nenhum registro encontrado',
-                info: 'Mostrando de _START_ até _END_ de _TOTAL_ registros',
-                infoEmpty: 'Mostrando 0 até 0 de 0 registros',
-                infoFiltered: '(Filtrados de _MAX_ registros)',
-                loadingRecords: 'Carregando...',
-                processing: 'Processando...',
-                zeroRecords: 'Nenhum registro encontrado',
-                search: 'Pesquisar',
+                emptyTable: "Nenhum registro encontrado",
+                info: "Mostrando de _START_ até _END_ de _TOTAL_ registros",
+                infoEmpty: "Mostrando 0 até 0 de 0 registros",
+                infoFiltered: "(Filtrados de _MAX_ registros)",
+                loadingRecords: "Carregando...",
+                processing: "Processando...",
+                zeroRecords: "Nenhum registro encontrado",
+                search: "Pesquisar",
                 paginate: {
-                    next: 'Próximo',
-                    previous: 'Anterior',
-                    first: 'Primeiro',
-                    last: 'Último',
+                    next: "Próximo",
+                    previous: "Anterior",
+                    first: "Primeiro",
+                    last: "Último"
                 },
-                lengthMenu: 'Exibir _MENU_ resultados por página',
+                lengthMenu: "Exibir _MENU_ resultados por página"
             },
-            drawCallback: function (settings) {
-                try {
-
-                    var tooltipElements =
-                        document.querySelectorAll('[data-ejtip]');
-                    tooltipElements.forEach(function (element) {
-
-                        if (
-                            !element.ej2_instances ||
-                            element.ej2_instances.length === 0
-                        ) {
-
+            drawCallback: function (settings)
+            {
+                try
+                {
+
+                    var tooltipElements = document.querySelectorAll('[data-ejtip]');
+                    tooltipElements.forEach(function (element)
+                    {
+                        if (!element.ej2_instances || element.ej2_instances.length === 0)
+                        {
                             new ej.popups.Tooltip({
                                 content: element.getAttribute('data-ejtip'),
-                                position: 'TopCenter',
-                                opensOn: 'Hover',
-
-                                cssClass: 'ftx-tooltip-viagens',
+                                position: 'TopCenter'
                             }).appendTo(element);
                         }
                     });
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'ViagemIndex.js',
-                        'DataTable.drawCallback',
-                        error,
-                    );
+                } catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("ViagemIndex.js", "DataTable.drawCallback", error);
                 }
             },
 
-            initComplete: function (settings, json) {
+            initComplete: function(settings, json) {
                 try {
 
                     FtxViagens.adicionarAvisoFiltro();
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'ViagemIndex.js',
-                        'DataTable.initComplete',
-                        error,
-                    );
-                }
-            },
+                    Alerta.TratamentoErroComLinha("ViagemIndex.js", "DataTable.initComplete", error);
+                }
+            }
         });
 
         let loadingHidden = false;
-        function safeHideLoading() {
+        function safeHideLoading()
+        {
             if (loadingHidden) return;
             loadingHidden = true;
 
@@ -2123,62 +1956,71 @@
         dataTableViagens.one('draw.dt', safeHideLoading);
         dataTableViagens.on('error.dt', safeHideLoading);
 
-        dataTableViagens.on('draw.dt', function () {
-            try {
-                $('#tblViagem tbody tr').each(function () {
+        dataTableViagens.on('draw.dt', function ()
+        {
+            try
+            {
+                $('#tblViagem tbody tr').each(function ()
+                {
                     ftxBindLazyImgsInRow(this);
                 });
-            } catch (e) {
-                console.error('Erro no draw.dt:', e);
+            } catch (e)
+            {
+                console.error("Erro no draw.dt:", e);
             }
         });
 
         setTimeout(safeHideLoading, 20000);
-    } catch (error) {
-        TratamentoErroComLinha('ViagemIndex.js', 'ListaTodasViagens', error);
+
+    } catch (error)
+    {
+        TratamentoErroComLinha("ViagemIndex.js", "ListaTodasViagens", error);
         FtxViagens.esconderLoading();
     }
 }
 
-$('#btnFinalizarViagem').click(async function (e) {
-    try {
+$("#btnFinalizarViagem").click(async function (e)
+{
+    try
+    {
         e.preventDefault();
-        console.log('🔵 [1/6] Botão Finalizar Viagem clicado');
-
-        const DataFinal = $('#txtDataFinal').val();
-        console.log('🔵 [2/6] Verificando Data Final:', DataFinal);
-        if (DataFinal === '') {
-            console.log('❌ Data Final vazia - parando execução');
-            Alerta.Erro('Erro na Data', 'A data final é obrigatória!');
+        console.log("🔵 [1/6] Botão Finalizar Viagem clicado");
+
+        const DataFinal = $("#txtDataFinal").val();
+        console.log("🔵 [2/6] Verificando Data Final:", DataFinal);
+        if (DataFinal === "")
+        {
+            console.log("❌ Data Final vazia - parando execução");
+            Alerta.Erro("Erro na Data", "A data final é obrigatória!");
             return;
         }
 
-        const HoraFinal = $('#txtHoraFinal').val();
-        console.log('🔵 [3/6] Verificando Hora Final:', HoraFinal);
-        if (HoraFinal === '') {
-            console.log('❌ Hora Final vazia - parando execução');
-            Alerta.Erro('Erro na Hora', 'A hora final é obrigatória!');
+        const HoraFinal = $("#txtHoraFinal").val();
+        console.log("🔵 [3/6] Verificando Hora Final:", HoraFinal);
+        if (HoraFinal === "")
+        {
+            console.log("❌ Hora Final vazia - parando execução");
+            Alerta.Erro("Erro na Hora", "A hora final é obrigatória!");
             return;
         }
 
-        const KmFinal = $('#txtKmFinal').val();
-        console.log('🔵 [4/6] Verificando KM Final:', KmFinal);
-        if (KmFinal === '') {
-            console.log('❌ KM Final vazio - parando execução');
-            Alerta.Erro(
-                'Erro na Quilometragem',
-                'A quilometragem final é obrigatória!',
-            );
+        const KmFinal = $("#txtKmFinal").val();
+        console.log("🔵 [4/6] Verificando KM Final:", KmFinal);
+        if (KmFinal === "")
+        {
+            console.log("❌ KM Final vazio - parando execução");
+            Alerta.Erro("Erro na Quilometragem", "A quilometragem final é obrigatória!");
             return;
         }
 
-        if (typeof window.validarFinalizacaoConsolidadaIA === 'function') {
-            console.log('🔵 [5/7] Executando validação consolidada IA...');
+        if (typeof window.validarFinalizacaoConsolidadaIA === 'function')
+        {
+            console.log("🔵 [5/7] Executando validação consolidada IA...");
             const modalEl = document.getElementById('modalFinalizaViagem');
             const veiculoId = modalEl?.getAttribute('data-veiculo-id') || '';
-            const DataInicial = $('#txtDataInicial').val();
-            const HoraInicial = $('#txtHoraInicial').val();
-            const KmInicial = $('#txtKmInicial').val();
+            const DataInicial = $("#txtDataInicial").val();
+            const HoraInicial = $("#txtHoraInicial").val();
+            const KmInicial = $("#txtKmInicial").val();
 
             const iaValida = await validarFinalizacaoConsolidadaIA({
                 dataInicial: DataInicial,
@@ -2187,884 +2029,831 @@
                 horaFinal: HoraFinal,
                 kmInicial: KmInicial,
                 kmFinal: KmFinal,
-                veiculoId: veiculoId,
+                veiculoId: veiculoId
             });
 
-            if (!iaValida) {
-                console.log(
-                    '❌ Validação consolidada IA falhou - usuário optou por corrigir',
-                );
+            if (!iaValida)
+            {
+                console.log("❌ Validação consolidada IA falhou - usuário optou por corrigir");
                 return;
             }
-            console.log('✅ Validação consolidada IA passou!');
-        }
-
-        console.log('🔵 [6/7] Verificando nível de combustível...');
-        var niveisElement = document.getElementById('ddtCombustivelFinal');
-        console.log('🔵 [6/7] Elemento ddtCombustivelFinal:', niveisElement);
-
-        if (!niveisElement) {
-            console.log(
-                '❌ ERRO CRÍTICO: Elemento ddtCombustivelFinal não encontrado no DOM!',
-            );
-            Alerta.Erro(
-                'Erro',
-                'Componente de combustível não foi encontrado. Recarregue a página.',
-            );
+            console.log("✅ Validação consolidada IA passou!");
+        }
+
+        console.log("🔵 [6/7] Verificando nível de combustível...");
+        var niveisElement = document.getElementById("ddtCombustivelFinal");
+        console.log("🔵 [6/7] Elemento ddtCombustivelFinal:", niveisElement);
+
+        if (!niveisElement)
+        {
+            console.log("❌ ERRO CRÍTICO: Elemento ddtCombustivelFinal não encontrado no DOM!");
+            Alerta.Erro("Erro", "Componente de combustível não foi encontrado. Recarregue a página.");
             return;
         }
 
         var niveis = niveisElement.ej2_instances?.[0];
-        console.log('🔵 [6/7] Instância Syncfusion niveis:', niveis);
-        console.log('🔵 [6/7] Valor do nível:', niveis?.value);
-
-        if (!niveis) {
-            console.log(
-                '❌ ERRO CRÍTICO: Componente Syncfusion ddtCombustivelFinal não está inicializado!',
-            );
-            Alerta.Erro(
-                'Erro',
-                'Componente de combustível não está inicializado. Recarregue a página.',
-            );
+        console.log("🔵 [6/7] Instância Syncfusion niveis:", niveis);
+        console.log("🔵 [6/7] Valor do nível:", niveis?.value);
+
+        if (!niveis)
+        {
+            console.log("❌ ERRO CRÍTICO: Componente Syncfusion ddtCombustivelFinal não está inicializado!");
+            Alerta.Erro("Erro", "Componente de combustível não está inicializado. Recarregue a página.");
             return;
         }
 
-        if (
-            niveis.value === null ||
-            niveis.value === undefined ||
-            niveis.value === ''
-        ) {
-            console.log('❌ Nível de combustível vazio - parando execução');
-            Alerta.Erro(
-                'Atenção',
-                'O nível final de combustível é obrigatório!',
-            );
+        if (niveis.value === null || niveis.value === undefined || niveis.value === "")
+        {
+            console.log("❌ Nível de combustível vazio - parando execução");
+            Alerta.Erro("Atenção", "O nível final de combustível é obrigatório!");
             return;
         }
 
         var nivelcombustivel = niveis.value.toString();
-        console.log(
-            '🔵 [6/7] Nível de combustível validado:',
-            nivelcombustivel,
-        );
-
-        console.log('🔵 [7/7] Validando ocorrências múltiplas...');
-        if (
-            typeof OcorrenciaViagem !== 'undefined' &&
-            OcorrenciaViagem.temOcorrencias &&
-            OcorrenciaViagem.temOcorrencias()
-        ) {
-            if (!OcorrenciaViagem.validarOcorrencias()) {
-                console.log(
-                    '❌ Validação de ocorrências múltiplas falhou - parando execução',
-                );
-                AppToast.show(
-                    'Vermelho',
-                    'Preencha o resumo de todas as ocorrências!',
-                    3000,
-                );
+        console.log("🔵 [6/7] Nível de combustível validado:", nivelcombustivel);
+
+        console.log("🔵 [7/7] Validando ocorrências múltiplas...");
+        if (typeof OcorrenciaViagem !== 'undefined' && OcorrenciaViagem.temOcorrencias && OcorrenciaViagem.temOcorrencias())
+        {
+            if (!OcorrenciaViagem.validarOcorrencias())
+            {
+                console.log("❌ Validação de ocorrências múltiplas falhou - parando execução");
+                AppToast.show('Vermelho', 'Preencha o resumo de todas as ocorrências!', 3000);
                 return;
             }
         }
 
-        console.log(
-            '✅ Todas as validações passaram! Preparando dados para gravação...',
-        );
-
-        const statusDocumento = $('#chkStatusDocumento').prop('checked')
-            ? 'Entregue'
-            : 'Ausente';
-        const statusCartaoAbastecimento = $(
-            '#chkStatusCartaoAbastecimento',
-        ).prop('checked')
-            ? 'Entregue'
-            : 'Ausente';
-
-        var descricaoElement = document.getElementById('rteDescricao');
+        console.log("✅ Todas as validações passaram! Preparando dados para gravação...");
+
+        const statusDocumento = $("#chkStatusDocumento").prop("checked") ? "Entregue" : "Ausente";
+        const statusCartaoAbastecimento = $("#chkStatusCartaoAbastecimento").prop("checked") ? "Entregue" : "Ausente";
+
+        var descricaoElement = document.getElementById("rteDescricao");
         var descricao = descricaoElement?.ej2_instances?.[0];
 
-        if (!descricao) {
-            console.log(
-                '⚠️ Componente rteDescricao não inicializado, usando valor vazio',
-            );
+        if (!descricao)
+        {
+            console.log("⚠️ Componente rteDescricao não inicializado, usando valor vazio");
         }
 
         var ocorrenciasParaEnviar = [];
-        if (
-            typeof OcorrenciaViagem !== 'undefined' &&
-            OcorrenciaViagem.temOcorrencias &&
-            OcorrenciaViagem.temOcorrencias()
-        ) {
-            ocorrenciasParaEnviar =
-                OcorrenciaViagem.coletarOcorrenciasSimples();
-            console.log('📋 Ocorrências coletadas:', ocorrenciasParaEnviar);
+        if (typeof OcorrenciaViagem !== 'undefined' && OcorrenciaViagem.temOcorrencias && OcorrenciaViagem.temOcorrencias())
+        {
+            ocorrenciasParaEnviar = OcorrenciaViagem.coletarOcorrenciasSimples();
+            console.log("📋 Ocorrências coletadas:", ocorrenciasParaEnviar);
         }
 
         const objViagem = {
-            ViagemId: $('#txtId').val(),
-            DataFinal: $('#txtDataFinal').val(),
-            HoraFim: $('#txtHoraFinal').val(),
-            KmFinal: $('#txtKmFinal').val(),
+            ViagemId: $("#txtId").val(),
+            DataFinal: $("#txtDataFinal").val(),
+            HoraFim: $("#txtHoraFinal").val(),
+            KmFinal: $("#txtKmFinal").val(),
             CombustivelFinal: nivelcombustivel,
             StatusDocumento: statusDocumento,
             StatusCartaoAbastecimento: statusCartaoAbastecimento,
-            Descricao: descricao?.value || '',
-            Ocorrencias: ocorrenciasParaEnviar,
+            Descricao: descricao?.value || "",
+            Ocorrencias: ocorrenciasParaEnviar
         };
 
-        console.log('📤 Dados preparados para envio:', objViagem);
-        console.log('📋 Total de ocorrências:', ocorrenciasParaEnviar.length);
-        console.log('🚀 Iniciando requisição AJAX...');
+        console.log("📤 Dados preparados para envio:", objViagem);
+        console.log("📋 Total de ocorrências:", ocorrenciasParaEnviar.length);
+        console.log("🚀 Iniciando requisição AJAX...");
 
         mostrarSpinnerFinalizacao();
 
         $.ajax({
-            type: 'POST',
-            url: '/api/Viagem/FinalizaViagem',
-            contentType: 'application/json; charset=utf-8',
-            dataType: 'json',
+            type: "POST",
+            url: "/api/Viagem/FinalizaViagem",
+            contentType: "application/json; charset=utf-8",
+            dataType: "json",
             data: JSON.stringify(objViagem),
-            beforeSend: function () {
-                console.log('⏳ AJAX: Enviando requisição...');
+            beforeSend: function ()
+            {
+                console.log("⏳ AJAX: Enviando requisição...");
             },
-            success: function (data) {
+            success: function (data)
+            {
 
                 esconderSpinnerFinalizacao();
 
-                console.log('✅ AJAX: Resposta recebida com sucesso!', data);
-
-                if (data.success === false) {
-                    console.log('❌ Operação falhou:', data.message);
-                    AppToast.show(
-                        'Amarelo',
-                        data.message || 'Erro ao finalizar viagem',
-                        4000,
-                    );
-
-                    if (data.message && data.message.includes('Data Final')) {
-                        $('#txtDataFinal').val('');
-                        $('#txtDataFinal').focus();
+                console.log("✅ AJAX: Resposta recebida com sucesso!", data);
+
+                if (data.success === false)
+                {
+                    console.log("❌ Operação falhou:", data.message);
+                    AppToast.show("Amarelo", data.message || "Erro ao finalizar viagem", 4000);
+
+                    if (data.message && data.message.includes("Data Final"))
+                    {
+                        $("#txtDataFinal").val("");
+                        $("#txtDataFinal").focus();
                     }
                     return;
                 }
 
                 if (data.ocorrenciasCriadas > 0) {
-                    console.log(
-                        '✅ Ocorrências criadas:',
-                        data.ocorrenciasCriadas,
-                    );
-                }
-
-                if (
-                    typeof OcorrenciaViagem !== 'undefined' &&
-                    OcorrenciaViagem.limparOcorrencias
-                ) {
+                    console.log("✅ Ocorrências criadas:", data.ocorrenciasCriadas);
+                }
+
+                if (typeof OcorrenciaViagem !== 'undefined' && OcorrenciaViagem.limparOcorrencias) {
                     OcorrenciaViagem.limparOcorrencias();
                 }
 
-                try {
-                    AppToast.show('Verde', data.message, 3000);
-
-                    console.log('🔄 Recarregando tabela de viagens...');
-                    $('#tblViagem').DataTable().ajax.reload(null, false);
-
-                    console.log('❌ Fechando modal...');
-                    $('#modalFinalizaViagem').hide();
-                    $('div').removeClass('modal-backdrop');
-                    $('body').removeClass('modal-open');
-
-                    console.log('✅ Viagem finalizada com sucesso!');
-                } catch (error) {
-                    console.error('❌ Erro no callback de sucesso:', error);
-                    Alerta.TratamentoErroComLinha(
-                        'ViagemIndex.js',
-                        'success.finalizarViagem',
-                        error,
-                    );
+                try
+                {
+                    AppToast.show("Verde", data.message, 3000);
+
+                    console.log("🔄 Recarregando tabela de viagens...");
+                    $("#tblViagem").DataTable().ajax.reload(null, false);
+
+                    console.log("❌ Fechando modal...");
+                    $("#modalFinalizaViagem").hide();
+                    $("div").removeClass("modal-backdrop");
+                    $("body").removeClass("modal-open");
+
+                    console.log("✅ Viagem finalizada com sucesso!");
+                }
+                catch (error)
+                {
+                    console.error("❌ Erro no callback de sucesso:", error);
+                    Alerta.TratamentoErroComLinha("ViagemIndex.js", "success.finalizarViagem", error);
                 }
             },
-            error: function (xhr, status, error) {
-                try {
+            error: function (xhr, status, error)
+            {
+                try
+                {
 
                     esconderSpinnerFinalizacao();
 
-                    console.error('❌ AJAX: Erro na requisição', {
+                    console.error("❌ AJAX: Erro na requisição", {
                         status: status,
                         error: error,
                         responseText: xhr.responseText,
-                        statusCode: xhr.status,
+                        statusCode: xhr.status
                     });
 
-                    let mensagem = 'Erro ao finalizar viagem';
-
-                    if (xhr.responseJSON?.message) {
+                    let mensagem = "Erro ao finalizar viagem";
+
+                    if (xhr.responseJSON?.message)
+                    {
                         mensagem = xhr.responseJSON.message;
-                    } else if (xhr.responseText) {
-                        try {
+                    }
+                    else if (xhr.responseText)
+                    {
+                        try
+                        {
                             const response = JSON.parse(xhr.responseText);
                             mensagem = response.message || mensagem;
-                        } catch (e) {
-                            console.error(
-                                '❌ Erro ao parsear responseText:',
-                                e,
-                            );
+                        }
+                        catch (e)
+                        {
+                            console.error("❌ Erro ao parsear responseText:", e);
                         }
                     }
 
-                    AppToast.show('Vermelho', mensagem, 4000);
-                    Alerta.TratamentoErroComLinha(
-                        'ViagemIndex.js',
-                        'ajax.error.finalizarViagem',
-                        error,
-                    );
-                } catch (err) {
+                    AppToast.show("Vermelho", mensagem, 4000);
+                    Alerta.TratamentoErroComLinha("ViagemIndex.js", "ajax.error.finalizarViagem", error);
+                } catch (err)
+                {
 
                     esconderSpinnerFinalizacao();
 
-                    console.error('❌ Erro no handler de erro do AJAX:', err);
-                    Alerta.TratamentoErroComLinha(
-                        'ViagemIndex.js',
-                        'error.finalizarViagem',
-                        err,
-                    );
+                    console.error("❌ Erro no handler de erro do AJAX:", err);
+                    Alerta.TratamentoErroComLinha("ViagemIndex.js", "error.finalizarViagem", err);
                 }
             },
-            complete: function () {
-                console.log('🏁 AJAX: Requisição finalizada (sucesso ou erro)');
-            },
+            complete: function ()
+            {
+                console.log("🏁 AJAX: Requisição finalizada (sucesso ou erro)");
+            }
         });
-    } catch (error) {
-        console.error('❌ ERRO CRÍTICO na função de finalizar viagem:', error);
-        console.error('❌ Stack trace:', error.stack);
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'click.btnFinalizarViagem',
-            error,
-        );
+    } catch (error)
+    {
+        console.error("❌ ERRO CRÍTICO na função de finalizar viagem:", error);
+        console.error("❌ Stack trace:", error.stack);
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "click.btnFinalizarViagem", error);
     }
 });
 
-function calcularDuracaoViagem() {
-    try {
-        var elDuracao = document.getElementById('txtDuracao');
-        var elDataIni = document.getElementById('txtDataInicial');
-        var elHoraIni = document.getElementById('txtHoraInicial');
-        var elDataFim = document.getElementById('txtDataFinal');
-        var elHoraFim = document.getElementById('txtHoraFinal');
+function calcularDuracaoViagem()
+{
+    try
+    {
+        var elDuracao = document.getElementById("txtDuracao");
+        var elDataIni = document.getElementById("txtDataInicial");
+        var elHoraIni = document.getElementById("txtHoraInicial");
+        var elDataFim = document.getElementById("txtDataFinal");
+        var elHoraFim = document.getElementById("txtHoraFinal");
         if (!elDuracao) return;
 
         var DUR_LIMIAR_MIN = 720;
 
-        var dIni = (elDataIni?.value || '').trim();
-        var hIni = (elHoraIni?.value || '').trim();
-        var dFim = (elDataFim?.value || '').trim();
-        var hFim = (elHoraFim?.value || '').trim();
-
-        if (!dIni || !hIni || !dFim || !hFim) {
-            elDuracao.value = '';
+        var dIni = (elDataIni?.value || "").trim();
+        var hIni = (elHoraIni?.value || "").trim();
+        var dFim = (elDataFim?.value || "").trim();
+        var hFim = (elHoraFim?.value || "").trim();
+
+        if (!dIni || !hIni || !dFim || !hFim)
+        {
+            elDuracao.value = "";
             FieldUX.setInvalid(elDuracao, false);
             FieldUX.tooltipOnTransition(elDuracao, false, 1, 'tooltipDuracao');
             return;
         }
 
-        if (typeof moment !== 'function')
-            throw new Error('moment.js não carregado');
-
-        var F = ['DD/MM/YYYYTHH:mm', 'YYYY-MM-DDTHH:mm'];
-        var inicio = moment(dIni + 'T' + hIni, F, true);
-        var fim = moment(dFim + 'T' + hFim, F, true);
-        if (!inicio.isValid() || !fim.isValid()) {
-            elDuracao.value = '';
+        if (typeof moment !== "function") throw new Error("moment.js não carregado");
+
+        var F = ["DD/MM/YYYYTHH:mm", "YYYY-MM-DDTHH:mm"];
+        var inicio = moment(dIni + "T" + hIni, F, true);
+        var fim = moment(dFim + "T" + hFim, F, true);
+        if (!inicio.isValid() || !fim.isValid())
+        {
+            elDuracao.value = "";
             FieldUX.setInvalid(elDuracao, false);
             FieldUX.tooltipOnTransition(elDuracao, false, 1, 'tooltipDuracao');
             return;
         }
 
-        var minutos = fim.diff(inicio, 'minutes');
-        if (!Number.isFinite(minutos) || minutos < 0) {
-            elDuracao.value = '';
+        var minutos = fim.diff(inicio, "minutes");
+        if (!Number.isFinite(minutos) || minutos < 0)
+        {
+            elDuracao.value = "";
             FieldUX.setInvalid(elDuracao, true);
-            FieldUX.tooltipOnTransition(
-                elDuracao,
-                true,
-                1200,
-                'tooltipDuracao',
-            );
+            FieldUX.tooltipOnTransition(elDuracao, true, 1200, 'tooltipDuracao');
             return;
         }
 
         var dias = Math.floor(minutos / 1440);
         var horas = Math.floor((minutos % 1440) / 60);
-        elDuracao.value = `${dias} dia${dias !== 1 ? 's' : ''} e ${horas} hora${horas !== 1 ? 's' : ''}`;
+        elDuracao.value = `${dias} dia${dias !== 1 ? "s" : ""} e ${horas} hora${horas !== 1 ? "s" : ""}`;
 
         var invalida = minutos > DUR_LIMIAR_MIN;
         FieldUX.setInvalid(elDuracao, invalida);
-        FieldUX.tooltipOnTransition(
-            elDuracao,
-            invalida && !window.CarregandoViagemBloqueada,
-            1200,
-            'tooltipDuracao',
-        );
-    } catch (error) {
-        if (typeof TratamentoErroComLinha === 'function') {
-            TratamentoErroComLinha(
-                'ViagemIndex.js',
-                'calcularDuracaoViagem',
-                error,
-            );
-        } else {
-            console.error(error);
-        }
+        FieldUX.tooltipOnTransition(elDuracao, invalida && !window.CarregandoViagemBloqueada, 1200, 'tooltipDuracao');
+    } catch (error)
+    {
+        if (typeof TratamentoErroComLinha === 'function')
+        {
+            TratamentoErroComLinha("ViagemIndex.js", "calcularDuracaoViagem", error);
+        } else { console.error(error); }
     }
 }
 
-function determinarRelatorio(data) {
-    try {
-        if (!data) return 'FichaAberta.trdp';
-
-        let relatorio = 'FichaAberta.trdp';
-
-        if (data.status === 'Cancelada') {
-            $('#btnCancela').hide();
+function determinarRelatorio(data)
+{
+    try
+    {
+        if (!data) return "FichaAberta.trdp";
+
+        let relatorio = "FichaAberta.trdp";
+
+        let statusEfetivo = (data.status || "").trim().toLowerCase();
+
+        if (!statusEfetivo || statusEfetivo === "")
+        {
+            if (data.dataCancelamento || data.DataCancelamento)
+            {
+                statusEfetivo = "cancelada";
+                console.warn('⚠️ Status vazio mas viagem tem DataCancelamento - inferido como Cancelada');
+            }
+            else if (data.dataFinalizacao || data.DataFinalizacao || data.dataFinal || data.DataFinal)
+            {
+                statusEfetivo = "realizada";
+                console.warn('⚠️ Status vazio mas viagem tem DataFinalizacao/DataFinal - inferido como Realizada');
+            }
+            else
+            {
+                statusEfetivo = "aberta";
+            }
+        }
+
+        if (statusEfetivo === "cancelada")
+        {
+            $("#btnCancela").hide();
             window.CarregandoViagemBloqueada = true;
-            relatorio =
-                data.finalidade !== 'Evento'
-                    ? 'FichaCancelada.trdp'
-                    : 'FichaEventoCancelado.trdp';
-        } else if (
-            data.finalidade === 'Evento' &&
-            data.status !== 'Cancelada'
-        ) {
-            relatorio = 'FichaEvento.trdp';
-        } else if (data.status === 'Aberta' && data.finalidade !== 'Evento') {
-            relatorio = 'FichaAberta.trdp';
-        } else if (data.status === 'Realizada') {
+            relatorio = data.finalidade !== "Evento" ? "FichaCancelada.trdp" : "FichaEventoCancelado.trdp";
+        } else if (data.finalidade === "Evento" && statusEfetivo !== "cancelada")
+        {
+            relatorio = "FichaEvento.trdp";
+        } else if (statusEfetivo === "aberta" && data.finalidade !== "Evento")
+        {
+            relatorio = "FichaAberta.trdp";
+        } else if (statusEfetivo === "realizada")
+        {
             window.CarregandoViagemBloqueada = true;
-            relatorio =
-                data.finalidade !== 'Evento'
-                    ? 'FichaRealizada.trdp'
-                    : 'FichaEventoRealizado.trdp';
-        } else if (data.statusAgendamento === true) {
-            relatorio =
-                data.finalidade !== 'Evento'
-                    ? 'FichaAgendamento.trdp'
-                    : 'FichaEventoAgendado.trdp';
-        }
-
-        console.log('Relatório selecionado:', relatorio);
+            relatorio = data.finalidade !== "Evento" ? "FichaRealizada.trdp" : "FichaEventoRealizado.trdp";
+        } else if (data.statusAgendamento === true)
+        {
+            relatorio = data.finalidade !== "Evento" ? "FichaAgendamento.trdp" : "FichaEventoAgendado.trdp";
+        }
+
+        console.log('Relatório selecionado:', relatorio, '(statusEfetivo:', statusEfetivo, ')');
         return relatorio;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'determinarRelatorio',
-            error,
-        );
-        return 'FichaAberta.trdp';
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "determinarRelatorio", error);
+        return "FichaAberta.trdp";
     }
 }
 
-function initViewer(viagemId) {
-    try {
-        if (!viagemId) {
+function initViewer(viagemId)
+{
+    try
+    {
+        if (!viagemId)
+        {
             console.error('ViagemId não fornecido para o Report Viewer');
-            $('#reportViewer1').html(
-                '<div class="alert alert-warning">ID da viagem não informado.</div>',
-            );
+            $("#reportViewer1").html('<div class="alert alert-warning">ID da viagem não informado.</div>');
             return;
         }
 
-        console.log('Inicializando Report Viewer para viagem:', viagemId);
-
-        if (typeof $ === 'undefined') {
-            console.error('jQuery não carregado!');
-            $('#reportViewer1').html(
-                '<div class="alert alert-danger">jQuery não carregado.</div>',
-            );
+        console.log('📊 Inicializando Report Viewer para viagem:', viagemId);
+
+        if (typeof $ === 'undefined')
+        {
+            console.error('❌ jQuery não carregado!');
+            $("#reportViewer1").html('<div class="alert alert-danger">jQuery não carregado.</div>');
             return;
         }
 
-        if (typeof kendo === 'undefined') {
-            console.error('Kendo UI não carregado!');
-            $('#reportViewer1').html(
-                '<div class="alert alert-danger">Kendo UI não carregado.</div>',
-            );
+        if (typeof kendo === 'undefined')
+        {
+            console.error('❌ Kendo UI não carregado!');
+            $("#reportViewer1").html('<div class="alert alert-danger">Kendo UI não carregado.</div>');
             return;
         }
 
-        if (typeof telerikReportViewer === 'undefined') {
-            console.error('telerikReportViewer não está definido.');
-            $('#reportViewer1').html(
-                '<div class="alert alert-danger">Telerik Report Viewer não carregado.</div>',
-            );
+        if (typeof telerikReportViewer === 'undefined')
+        {
+            console.error('❌ telerikReportViewer não está definido.');
+            $("#reportViewer1").html('<div class="alert alert-danger">Telerik Report Viewer não carregado.</div>');
             return;
         }
 
-        const $viewer = $('#reportViewer1');
-
-        const existingInstance = $viewer.data('telerik_ReportViewer');
-        if (existingInstance) {
-            try {
-                if (typeof existingInstance.dispose === 'function') {
+        console.log('✅ Todas as dependências estão carregadas');
+
+        const $viewer = $("#reportViewer1");
+        console.log('📊 Viewer element encontrado:', $viewer.length > 0 ? '✅' : '❌');
+
+        const existingInstance = $viewer.data("telerik_ReportViewer");
+        if (existingInstance)
+        {
+            try
+            {
+                if (typeof existingInstance.dispose === 'function')
+                {
                     existingInstance.dispose();
-                } else if (typeof existingInstance.destroy === 'function') {
+                } else if (typeof existingInstance.destroy === 'function')
+                {
                     existingInstance.destroy();
                 }
-            } catch (e) {
+            } catch (e)
+            {
                 console.error('Erro ao destruir Report Viewer anterior:', e);
             }
-            $viewer.removeData('telerik_ReportViewer');
+            $viewer.removeData("telerik_ReportViewer");
             $viewer.empty();
         }
 
-        $('#ReportContainer')
-            .css({
-                height: '700px',
-                display: 'block',
-            })
-            .addClass('visible');
-
+        $("#ReportContainer").css({
+            height: "700px",
+            display: "block"
+        }).addClass("visible");
+
+        console.log('📡 Fazendo requisição AJAX para /api/Agenda/RecuperaViagem...');
         $.ajax({
-            type: 'GET',
-            url: '/api/Agenda/RecuperaViagem',
+            type: "GET",
+            url: "/api/Agenda/RecuperaViagem",
             data: { id: viagemId },
-            contentType: 'application/json',
-            dataType: 'json',
+            contentType: "application/json",
+            dataType: "json"
         })
-            .done(function (response) {
-                try {
+            .done(function (response)
+            {
+                console.log('✅ Requisição AJAX concluída:', response);
+                try
+                {
                     const data = response && response.data ? response.data : {};
                     const relatorioNome = determinarRelatorio(data);
 
-                    console.log('Configurando Report Viewer com:', {
+                    console.log('📊 Configurando Report Viewer com:', {
                         relatorio: relatorioNome,
                         viagemId: viagemId,
+                        serviceUrl: '/api/reports/'
                     });
 
                     kendo.ui.progress($viewer, true);
 
-                    try {
+                    try
+                    {
+                        console.log('🔧 Chamando telerik_ReportViewer()...');
 
                         $viewer.telerik_ReportViewer({
-                            serviceUrl: '/api/reports/',
+                            serviceUrl: "/api/reports/",
                             reportSource: {
                                 report: relatorioNome,
                                 parameters: {
-                                    ViagemId: viagemId.toString().toUpperCase(),
-                                },
+                                    ViagemId: viagemId.toString().toUpperCase()
+                                }
                             },
-                            viewMode:
-                                telerikReportViewer.ViewModes.PRINT_PREVIEW,
+                            viewMode: telerikReportViewer.ViewModes.PRINT_PREVIEW,
                             scaleMode: telerikReportViewer.ScaleModes.SPECIFIC,
                             scale: 1.0,
+                            width: "100%",
+                            height: "640px",
                             enableAccessibility: false,
                             sendEmail: {
-                                enabled: false,
+                                enabled: false
                             },
+                            error: function (e, args)
+                            {
+                                console.error('❌ Erro no Report Viewer:', args);
+                                kendo.ui.progress($viewer, false);
+                                const errorMsg = args.message || 'Erro desconhecido ao carregar relatório';
+                                console.error('Mensagem de erro:', errorMsg);
+                                $("#reportViewer1").html('<div class="alert alert-danger"><strong>Erro:</strong> ' + errorMsg + '</div>');
+                            },
+                            ready: function ()
+                            {
+                                console.log('✅ Report Viewer carregado com sucesso!');
+                                kendo.ui.progress($viewer, false);
+                            }
                         });
 
-                    } catch (error) {
+                        console.log('✅ telerik_ReportViewer() chamado com sucesso');
+
+                    }
+                    catch (error)
+                    {
                         console.error('Erro ao inicializar viewer:', error);
                         kendo.ui.progress($viewer, false);
-                        $('#reportViewer1').html(
-                            '<div class="alert alert-danger">Erro: ' +
-                                error.message +
-                                '</div>',
-                        );
+                        $("#reportViewer1").html('<div class="alert alert-danger">Erro: ' + error.message + '</div>');
                     }
-                } catch (error) {
+                } catch (error)
+                {
                     console.error('Erro ao configurar Report Viewer:', error);
-                    $('#reportViewer1').html(
-                        '<div class="alert alert-danger">Erro ao configurar o relatório.</div>',
-                    );
-                    Alerta.TratamentoErroComLinha(
-                        'ViagemIndex.js',
-                        'initViewer.done',
-                        error,
-                    );
+                    $("#reportViewer1").html('<div class="alert alert-danger">Erro ao configurar o relatório.</div>');
+                    Alerta.TratamentoErroComLinha("ViagemIndex.js", "initViewer.done", error);
                 }
             })
-            .fail(function (xhr) {
-                console.error('Erro ao carregar dados da viagem:', xhr);
-                const errorMsg =
-                    xhr.responseJSON?.message ||
-                    'Não foi possível carregar os dados da viagem';
-                $('#reportViewer1').html(
-                    '<div class="alert alert-danger">' + errorMsg + '</div>',
-                );
+            .fail(function (xhr)
+            {
+                console.error("❌ Erro ao carregar dados da viagem:", xhr);
+                console.error("Status:", xhr.status);
+                console.error("Status Text:", xhr.statusText);
+                console.error("Response:", xhr.responseText);
+                const errorMsg = xhr.responseJSON?.message || 'Não foi possível carregar os dados da viagem';
+                $("#reportViewer1").html('<div class="alert alert-danger"><strong>Erro na API:</strong> ' + errorMsg + '</div>');
             });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('ViagemIndex.js', 'initViewer', error);
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "initViewer", error);
     }
 }
 
-$(function () {
-    try {
-        const $modal = $('#modalPrint');
-
-        $modal.on('show.bs.modal', function (event) {
+$(function ()
+{
+    try
+    {
+        const $modal = $("#modalPrint");
+
+        $modal.on("show.bs.modal", function (event)
+        {
             const modalEl = this;
             const $button = $(event.relatedTarget || []);
-            const fromBtn = $button.data('viagem-id') || $button.data('id');
+            const fromBtn = $button.data("viagem-id") || $button.data("id");
             const fromAttr = modalEl.getAttribute('data-viagem-id');
-            const fromHidden = $('#txtViagemId').val();
+            const fromHidden = $("#txtViagemId").val();
 
             const viagemId = fromBtn || fromAttr || fromHidden;
 
             console.log('Modal abrindo com ViagemId:', viagemId);
 
-            if (!viagemId) {
+            if (!viagemId)
+            {
                 console.error('ViagemId não encontrado no botão/modal/hidden');
                 return;
             }
 
-            $('#txtViagemId').val(viagemId);
+            $("#txtViagemId").val(viagemId);
         });
 
-        $modal.on('shown.bs.modal', function () {
-
-            $(document).off('focusin.modal');
-
-            const viagemId = $('#txtViagemId').val();
-
-            if (!viagemId) {
-                $('#reportViewer1').html(
-                    '<div class="alert alert-warning">ID da viagem não informado.</div>',
-                );
+        $modal.on("shown.bs.modal", function ()
+        {
+            console.log('📊 Evento shown.bs.modal disparado');
+
+            $(document).off("focusin.modal");
+
+            const viagemId = $("#txtViagemId").val();
+            console.log('📊 ViagemId no shown:', viagemId);
+
+            if (!viagemId)
+            {
+                console.error('❌ ID da viagem não encontrado no shown.bs.modal');
+                $("#reportViewer1").html('<div class="alert alert-warning">ID da viagem não informado.</div>');
                 return;
             }
 
-            setTimeout(function () {
-                if (
-                    typeof kendo !== 'undefined' &&
-                    typeof telerikReportViewer !== 'undefined'
-                ) {
+            console.log('🔍 Verificando dependências...');
+            console.log(' - jQuery:', typeof $ !== 'undefined' ? '✅' : '❌');
+            console.log(' - Kendo:', typeof kendo !== 'undefined' ? '✅' : '❌');
+            console.log(' - telerikReportViewer:', typeof telerikReportViewer !== 'undefined' ? '✅' : '❌');
+
+            setTimeout(function ()
+            {
+                if (typeof kendo !== 'undefined' && typeof telerikReportViewer !== 'undefined')
+                {
+                    console.log('✅ Dependências OK. Chamando initViewer...');
                     initViewer(viagemId);
-                } else {
-                    console.error('Dependências não carregadas. Aguardando...');
-                    setTimeout(function () {
+                } else
+                {
+                    console.error('⚠️ Dependências não carregadas. Aguardando mais 1 segundo...');
+                    setTimeout(function ()
+                    {
+                        console.log('🔄 Segunda tentativa de inicializar o viewer...');
                         initViewer(viagemId);
                     }, 1000);
                 }
             }, 300);
         });
 
-        $modal.on('hidden.bs.modal', function () {
+        $modal.on("hidden.bs.modal", function ()
+        {
             console.log('Limpando Report Viewer...');
 
             this.removeAttribute('data-viagem-id');
 
-            const $viewer = $('#reportViewer1');
-            const instance = $viewer.data('telerik_ReportViewer');
-
-            if (instance) {
-                try {
-                    if (typeof instance.dispose === 'function') {
+            const $viewer = $("#reportViewer1");
+            const instance = $viewer.data("telerik_ReportViewer");
+
+            if (instance)
+            {
+                try
+                {
+                    if (typeof instance.dispose === 'function')
+                    {
                         instance.dispose();
-                    } else if (typeof instance.destroy === 'function') {
+                    } else if (typeof instance.destroy === 'function')
+                    {
                         instance.destroy();
                     }
-                } catch (e) {
+                } catch (e)
+                {
                     console.error('Erro ao limpar Report Viewer:', e);
                 }
             }
 
-            $viewer.replaceWith(
-                '<div id="reportViewer1" style="width:100%" class="pb-3">Loading...</div>',
-            );
+            $viewer.replaceWith('<div id="reportViewer1" style="width:100%" class="pb-3">Loading...</div>');
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'modalPrint.config',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "modalPrint.config", error);
     }
 });
 
-(function () {
-    function ensureTooltip(el, globalName) {
-        try {
-            if (
-                globalName &&
-                window[globalName] &&
-                typeof window[globalName].open === 'function'
-            )
-                return window[globalName];
-            if (el && el.ej2_instances && el.ej2_instances.length) {
-                for (var i = 0; i < el.ej2_instances.length; i++) {
+(function ()
+{
+    function ensureTooltip(el, globalName)
+    {
+        try
+        {
+            if (globalName && window[globalName] && typeof window[globalName].open === 'function') return window[globalName];
+            if (el && el.ej2_instances && el.ej2_instances.length)
+            {
+                for (var i = 0; i < el.ej2_instances.length; i++)
+                {
                     var inst = el.ej2_instances[i];
-                    if (
-                        inst &&
-                        typeof inst.open === 'function' &&
-                        typeof inst.close === 'function'
-                    ) {
+                    if (inst && typeof inst.open === 'function' && typeof inst.close === 'function')
+                    {
                         if (globalName) window[globalName] = inst;
                         return inst;
                     }
                 }
             }
-            if (
-                window.ej &&
-                ej.popups &&
-                typeof ej.popups.Tooltip === 'function'
-            ) {
+            if (window.ej && ej.popups && typeof ej.popups.Tooltip === 'function')
+            {
                 var inst = new ej.popups.Tooltip({
-                    content:
-                        el.getAttribute('data-ejtip') ||
-                        'Valor acima do limite.',
+                    content: el.getAttribute('data-ejtip') || 'Valor acima do limite.',
                     opensOn: 'Custom',
-                    position: 'TopCenter',
+                    position: 'TopCenter'
                 });
                 inst.appendTo(el);
                 if (globalName) window[globalName] = inst;
                 return inst;
             }
             return null;
-        } catch (error) {
-            console.error('Erro em ensureTooltip:', error);
+        } catch (error)
+        {
+            console.error("Erro em ensureTooltip:", error);
             return null;
         }
     }
 
-    function toggleClass(el, cls, on) {
-        try {
+    function toggleClass(el, cls, on)
+    {
+        try
+        {
             if (!el) return;
-            if (el.classList) {
+            if (el.classList)
+            {
                 el.classList.toggle(cls, !!on);
-            } else {
-                var c = el.className || '',
-                    has = new RegExp('\\b' + cls + '\\b').test(c);
+            } else
+            {
+                var c = el.className || '', has = new RegExp('\\b' + cls + '\\b').test(c);
                 if (on && !has) el.className = (c + ' ' + cls).trim();
-                if (!on && has)
-                    el.className = c
-                        .replace(new RegExp('\\b' + cls + '\\b'), '')
-                        .replace(/\s{2,}/g, ' ')
-                        .trim();
-            }
-        } catch (error) {
-            console.error('Erro em toggleClass:', error);
-        }
-    }
-
-    function setInvalid(el, invalid) {
-        try {
+                if (!on && has) el.className = c.replace(new RegExp('\\b' + cls + '\\b'), '').replace(/\s{2,}/g, ' ').trim();
+            }
+        } catch (error)
+        {
+            console.error("Erro em toggleClass:", error);
+        }
+    }
+
+    function setInvalid(el, invalid)
+    {
+        try
+        {
             if (!el) return;
             toggleClass(el, 'is-invalid', invalid);
-            try {
-                el.setAttribute('aria-invalid', String(!!invalid));
-            } catch (e) {}
-            try {
-                el.style.color = invalid
-                    ? 'var(--ftx-invalid,#dc3545)'
-                    : 'black';
-            } catch (e) {}
-            try {
-                var wrap = el.closest(
-                    '.e-input-group, .e-float-input, .e-control-wrapper',
-                );
+            try { el.setAttribute('aria-invalid', String(!!invalid)); } catch (e) { }
+            try { el.style.color = invalid ? 'var(--ftx-invalid,#dc3545)' : 'black'; } catch (e) { }
+            try
+            {
+                var wrap = el.closest('.e-input-group, .e-float-input, .e-control-wrapper');
                 if (wrap) toggleClass(wrap, 'is-invalid', invalid);
-            } catch (e) {}
+            } catch (e) { }
             setHigh(el, false);
-        } catch (error) {
-            console.error('Erro em setInvalid:', error);
-        }
-    }
-
-    function setHigh(el, high) {
-        try {
+        } catch (error)
+        {
+            console.error("Erro em setInvalid:", error);
+        }
+    }
+
+    function setHigh(el, high)
+    {
+        try
+        {
             if (!el) return;
             toggleClass(el, 'is-high', high);
-            try {
-                var wrap = el.closest(
-                    '.e-input-group, .e-float-input, .e-control-wrapper',
-                );
+            try
+            {
+                var wrap = el.closest('.e-input-group, .e-float-input, .e-control-wrapper');
                 if (wrap) toggleClass(wrap, 'is-high', high);
-            } catch (e) {}
-        } catch (error) {
-            console.error('Erro em setHigh:', error);
-        }
-    }
-
-    function tooltipOnTransition(el, condition, ms, globalName) {
-        try {
+            } catch (e) { }
+        } catch (error)
+        {
+            console.error("Erro em setHigh:", error);
+        }
+    }
+
+    function tooltipOnTransition(el, condition, ms, globalName)
+    {
+        try
+        {
             if (!el) return;
             var key = '_prevCond_' + (globalName || 'tt');
-            var prev = !!el[key],
-                now = !!condition;
-            if (now && !prev) {
+            var prev = !!el[key], now = !!condition;
+            if (now && !prev)
+            {
                 var tip = ensureTooltip(el, globalName);
-                if (tip && typeof tip.open === 'function') {
+                if (tip && typeof tip.open === 'function')
+                {
                     tip.open(el);
                     clearTimeout(el._tipTimer);
-                    el._tipTimer = setTimeout(function () {
-                        try {
-                            tip.close();
-                        } catch (e) {}
+                    el._tipTimer = setTimeout(function ()
+                    {
+                        try { tip.close(); } catch (e) { }
                     }, ms || 1000);
                 }
             }
             el[key] = now;
-        } catch (error) {
-            console.error('Erro em tooltipOnTransition:', error);
-        }
-    }
-
-    window.FieldUX = {
-        ensureTooltip,
-        setInvalid,
-        setHigh,
-        tooltipOnTransition,
-    };
+        } catch (error)
+        {
+            console.error("Erro em tooltipOnTransition:", error);
+        }
+    }
+
+    window.FieldUX = { ensureTooltip, setInvalid, setHigh, tooltipOnTransition };
 })();
 
-['input', 'change', 'focusout'].forEach((evt) => {
-    document
-        .getElementById('txtHoraFinal')
-        ?.addEventListener(evt, calcularDuracaoViagem);
-    document
-        .getElementById('txtDataFinal')
-        ?.addEventListener(evt, calcularDuracaoViagem);
-    document
-        .getElementById('txtKmInicial')
-        ?.addEventListener(evt, calcularDistanciaViagem);
-    document
-        .getElementById('txtKmFinal')
-        ?.addEventListener(evt, calcularDistanciaViagem);
+["input", "change", "focusout"].forEach(evt =>
+{
+    document.getElementById("txtHoraFinal")?.addEventListener(evt, calcularDuracaoViagem);
+    document.getElementById("txtDataFinal")?.addEventListener(evt, calcularDuracaoViagem);
+    document.getElementById("txtKmInicial")?.addEventListener(evt, calcularDistanciaViagem);
+    document.getElementById("txtKmFinal")?.addEventListener(evt, calcularDistanciaViagem);
 });
 
 window.viagemIdAtual = null;
 
-window.abrirModalFicha = function (viagemId) {
-    try {
-        console.log('[DEBUG] ===== ABRINDO MODAL DE FICHA =====');
-        console.log('[DEBUG] ViagemId recebido:', viagemId);
-
-        if (!viagemId || viagemId === 'undefined') {
-            console.error('[ERROR] ViagemId inválido');
+window.abrirModalFicha = function (viagemId)
+{
+    try
+    {
+        console.log('Abrindo modal para viagem:', viagemId);
+
+        if (!viagemId || viagemId === 'undefined')
+        {
             AppToast.show('Vermelho', 'ID da viagem não identificado', 3000);
             return;
         }
 
-        console.log('[DEBUG] Armazenando ViagemId...');
         window.viagemIdAtual = viagemId;
         $('#hiddenViagemId').val(viagemId);
 
-        console.log('[DEBUG] Buscando elemento do modal...');
         const modalEl = document.getElementById('modalFicha');
-        if (!modalEl) {
-            console.error('[ERROR] Modal #modalFicha não encontrado no DOM!');
-            AppToast.show(
-                'Vermelho',
-                'Modal não encontrado. Recarregue a página.',
-                3000,
-            );
+        if (!modalEl)
+        {
+            console.error('Modal #modalFicha não encontrado!');
             return;
         }
-        console.log('[DEBUG] Modal encontrado:', modalEl);
 
         modalEl.setAttribute('data-viagem-id', viagemId);
 
-        console.log('[DEBUG] Resetando modal...');
+        console.log('ID armazenado (global):', window.viagemIdAtual);
+        console.log('ID armazenado (hidden):', $('#hiddenViagemId').val());
+
         resetModalFicha();
 
-        console.log('[DEBUG] Verificando bootstrap...');
-        if (typeof bootstrap === 'undefined') {
-            console.error('[ERROR] Bootstrap não está carregado!');
-            AppToast.show('Vermelho', 'Erro: Bootstrap não carregado.', 3000);
-            return;
-        }
-
-        console.log('[DEBUG] Criando instância do modal...');
         const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
-        console.log('[DEBUG] Instância criada:', modal);
-
-        console.log('[DEBUG] Mostrando modal...');
         modal.show();
-        console.log('[DEBUG] Modal.show() executado');
-
-        setTimeout(() => {
-            console.log('[DEBUG] Carregando ficha de vistoria...');
+
+        setTimeout(() =>
+        {
             carregarFichaVistoria(viagemId);
         }, 300);
-    } catch (error) {
-        console.error('[ERROR] Erro ao abrir modal:', error);
-        console.error('[ERROR] Stack:', error.stack);
-        AppToast.show(
-            'Vermelho',
-            'Erro ao abrir modal: ' + error.message,
-            4000,
-        );
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'abrirModalFicha',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "abrirModalFicha", error);
     }
 };
 
-document.addEventListener('show.bs.modal', function (ev) {
-    try {
-        if (ev.target && ev.target.id === 'modalPrint') {
+document.addEventListener('show.bs.modal', function (ev)
+{
+    try
+    {
+        if (ev.target && ev.target.id === 'modalPrint')
+        {
             const trigger = ev.relatedTarget;
             const id = trigger ? trigger.getAttribute('data-viagem-id') : null;
             const hid = document.getElementById('txtViagemId');
             if (hid && id) hid.value = id;
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'modalPrint.show',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "modalPrint.show", error);
     }
 });
 
-document.getElementById('txtFile')?.addEventListener('change', (e) => {
-    try {
+document.getElementById('txtFile')?.addEventListener('change', e =>
+{
+    try
+    {
         const file = e.target.files?.[0];
         const img = document.getElementById('imgViewer');
         if (!file || !img) return;
         const r = new FileReader();
-        r.onload = (ev) => {
+        r.onload = ev =>
+        {
             img.src = ev.target.result;
             img.classList.remove('d-none');
         };
         r.readAsDataURL(file);
-    } catch (error) {
-        console.error('Erro no preview da imagem:', error);
+    } catch (error)
+    {
+        console.error("Erro no preview da imagem:", error);
     }
 });
 
-function mostrarSpinnerFinalizacao() {
-    try {
+function mostrarSpinnerFinalizacao()
+{
+    try
+    {
         $('#modalSpinnerFinalizacao').modal('show');
         $('#btnFinalizarViagem').prop('disabled', true).addClass('disabled');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'mostrarSpinnerFinalizacao',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "mostrarSpinnerFinalizacao", error);
     }
 }
 
-function esconderSpinnerFinalizacao() {
-    try {
+function esconderSpinnerFinalizacao()
+{
+    try
+    {
         $('#modalSpinnerFinalizacao').modal('hide');
-        $('#btnFinalizarViagem')
-            .prop('disabled', false)
-            .removeClass('disabled');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'esconderSpinnerFinalizacao',
-            error,
-        );
+        $('#btnFinalizarViagem').prop('disabled', false).removeClass('disabled');
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "esconderSpinnerFinalizacao", error);
     }
 }
 
-$(document).on('click', '#tblViagem .btn-ocorrencias-viagem', function (e) {
-    try {
+$(document).on('click', '#tblViagem .btn-ocorrencias-viagem', function (e)
+{
+    try
+    {
         e.preventDefault();
 
         if ($(this).hasClass('disabled')) return;
@@ -3078,120 +2867,103 @@
         modalEl.setAttribute('data-noficha', String(noFicha || ''));
         $('#hiddenOcorrenciasViagemId').val(viagemId);
 
-        const tituloSpan = modalEl.querySelector(
-            '#modalOcorrenciasViagemLabel span',
-        );
-        if (tituloSpan) {
-            tituloSpan.textContent = noFicha
-                ? `Ocorrências da Viagem nº ${noFicha}`
-                : 'Ocorrências da Viagem';
+        const tituloSpan = modalEl.querySelector('#modalOcorrenciasViagemLabel span');
+        if (tituloSpan)
+        {
+            tituloSpan.textContent = noFicha ? `Ocorrências da Viagem nº ${noFicha}` : 'Ocorrências da Viagem';
         }
 
         const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
         modal.show();
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'click.btn-ocorrencias-viagem',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "click.btn-ocorrencias-viagem", error);
     }
 });
 
-$('#modalOcorrenciasViagem').on('shown.bs.modal', function (e) {
-    try {
+$('#modalOcorrenciasViagem').on('shown.bs.modal', function (e)
+{
+    try
+    {
         const modalEl = this;
         let viagemId = modalEl.getAttribute('data-viagem-id');
 
-        if (!viagemId || viagemId === 'undefined') {
+        if (!viagemId || viagemId === 'undefined')
+        {
             viagemId = $('#hiddenOcorrenciasViagemId').val();
         }
 
-        if (!viagemId || viagemId === 'undefined') {
+        if (!viagemId || viagemId === 'undefined')
+        {
             console.error('ViagemId não encontrado');
             return;
         }
 
-        $('#tblOcorrenciasViagem tbody').html(
-            '<tr><td colspan="5" class="text-center"><i class="fa fa-spinner fa-spin"></i> Carregando...</td></tr>',
-        );
+        $('#tblOcorrenciasViagem tbody').html('<tr><td colspan="5" class="text-center"><i class="fa fa-spinner fa-spin"></i> Carregando...</td></tr>');
 
         carregarOcorrenciasViagem(viagemId);
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'modalOcorrenciasViagem.shown',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "modalOcorrenciasViagem.shown", error);
     }
 });
 
-function carregarOcorrenciasViagem(viagemId) {
-    try {
+function carregarOcorrenciasViagem(viagemId)
+{
+    try
+    {
         $.ajax({
             url: '/api/OcorrenciaViagem/ListarOcorrenciasModal',
             type: 'GET',
             data: { viagemId: viagemId },
-            success: function (response) {
-                try {
-                    if (response.success && response.data) {
+            success: function (response)
+            {
+                try
+                {
+                    if (response.success && response.data)
+                    {
                         renderizarTabelaOcorrencias(response.data);
-                    } else {
-                        $('#tblOcorrenciasViagem tbody').html(
-                            '<tr><td colspan="5" class="text-center text-muted">Nenhuma ocorrência registrada</td></tr>',
-                        );
+                    } else
+                    {
+                        $('#tblOcorrenciasViagem tbody').html('<tr><td colspan="5" class="text-center text-muted">Nenhuma ocorrência registrada</td></tr>');
                     }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'ViagemIndex.js',
-                        'carregarOcorrenciasViagem.success',
-                        error,
-                    );
+                } catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("ViagemIndex.js", "carregarOcorrenciasViagem.success", error);
                 }
             },
-            error: function (xhr, status, error) {
+            error: function (xhr, status, error)
+            {
                 console.error('Erro ao carregar ocorrências:', error);
-                $('#tblOcorrenciasViagem tbody').html(
-                    '<tr><td colspan="5" class="text-center text-danger">Erro ao carregar ocorrências</td></tr>',
-                );
-                AppToast.show(
-                    'Vermelho',
-                    'Erro ao carregar ocorrências da viagem',
-                    4000,
-                );
-            },
+                $('#tblOcorrenciasViagem tbody').html('<tr><td colspan="5" class="text-center text-danger">Erro ao carregar ocorrências</td></tr>');
+                AppToast.show('Vermelho', 'Erro ao carregar ocorrências da viagem', 4000);
+            }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'carregarOcorrenciasViagem',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "carregarOcorrenciasViagem", error);
     }
 }
 
-function renderizarTabelaOcorrencias(ocorrencias) {
-    try {
-        if (!ocorrencias || ocorrencias.length === 0) {
-            $('#tblOcorrenciasViagem tbody').html(
-                '<tr><td colspan="6" class="text-center text-muted">Nenhuma ocorrência registrada</td></tr>',
-            );
+function renderizarTabelaOcorrencias(ocorrencias)
+{
+    try
+    {
+        if (!ocorrencias || ocorrencias.length === 0)
+        {
+            $('#tblOcorrenciasViagem tbody').html('<tr><td colspan="6" class="text-center text-muted">Nenhuma ocorrência registrada</td></tr>');
             return;
         }
 
         let html = '';
-        ocorrencias.forEach(function (oc, index) {
+        ocorrencias.forEach(function (oc, index)
+        {
 
             const dataCriacao = oc.dataCriacao || oc.DataCriacao;
-            const dataFormatada = dataCriacao
-                ? new Date(dataCriacao).toLocaleDateString('pt-BR')
-                : '-';
+            const dataFormatada = dataCriacao ? new Date(dataCriacao).toLocaleDateString('pt-BR') : '-';
             const imagem = oc.imagemOcorrencia || oc.ImagemOcorrencia || '';
             const temImagem = imagem && imagem.trim() !== '';
-            const statusOc =
-                oc.statusOcorrencia !== undefined
-                    ? oc.statusOcorrencia
-                    : oc.StatusOcorrencia;
+            const statusOc = oc.statusOcorrencia !== undefined ? oc.statusOcorrencia : oc.StatusOcorrencia;
             const statusStr = oc.status || oc.Status || '';
             const resumo = oc.resumo || oc.Resumo || '';
             const descricao = oc.descricao || oc.Descricao || '';
@@ -3201,16 +2973,18 @@
             let statusFinal = 'Aberta';
             let badgeClass = 'ftx-ocorrencia-badge-aberta';
 
-            if (statusStr === 'Pendente') {
+            if (statusStr === 'Pendente')
+            {
                 statusFinal = 'Pendente';
                 badgeClass = 'ftx-ocorrencia-badge-pendente';
-            } else if (statusStr === 'Baixada' || statusOc === false) {
+            }
+            else if (statusStr === 'Baixada' || statusOc === false)
+            {
                 statusFinal = 'Baixada';
                 badgeClass = 'ftx-ocorrencia-badge-baixada';
-            } else if (
-                itemManutId &&
-                itemManutId !== '00000000-0000-0000-0000-000000000000'
-            ) {
+            }
+            else if (itemManutId && itemManutId !== '00000000-0000-0000-0000-000000000000')
+            {
                 statusFinal = 'Manutenção';
                 badgeClass = 'ftx-ocorrencia-badge-manutencao';
             }
@@ -3251,154 +3025,132 @@
         });
 
         $('#tblOcorrenciasViagem tbody').html(html);
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'renderizarTabelaOcorrencias',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "renderizarTabelaOcorrencias", error);
     }
 }
 
-$(document).on(
-    'click',
-    '.btn-ver-imagem-ocorrencia:not(.disabled)',
-    function (e) {
-        try {
-            e.preventDefault();
-            const imagemPath = $(this).data('imagem');
-
-            if (!imagemPath) {
-                AppToast.show(
-                    'Amarelo',
-                    'Esta ocorrência não possui imagem',
-                    3000,
-                );
-                return;
-            }
-
-            $('#imgOcorrenciaViewer').attr('src', imagemPath).show();
-            $('#noImageOcorrencia').hide();
-
-            const modalImagem = new bootstrap.Modal(
-                document.getElementById('modalImagemOcorrencia'),
-            );
-            modalImagem.show();
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'ViagemIndex.js',
-                'click.btn-ver-imagem-ocorrencia',
-                error,
-            );
-        }
-    },
-);
-
-$(document).on('click', '.btn-excluir-ocorrencia', function (e) {
-    try {
+$(document).on('click', '.btn-ver-imagem-ocorrencia:not(.disabled)', function (e)
+{
+    try
+    {
+        e.preventDefault();
+        const imagemPath = $(this).data('imagem');
+
+        if (!imagemPath)
+        {
+            AppToast.show('Amarelo', 'Esta ocorrência não possui imagem', 3000);
+            return;
+        }
+
+        $('#imgOcorrenciaViewer').attr('src', imagemPath).show();
+        $('#noImageOcorrencia').hide();
+
+        const modalImagem = new bootstrap.Modal(document.getElementById('modalImagemOcorrencia'));
+        modalImagem.show();
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "click.btn-ver-imagem-ocorrencia", error);
+    }
+});
+
+$(document).on('click', '.btn-excluir-ocorrencia', function (e)
+{
+    try
+    {
         e.preventDefault();
         const ocorrenciaId = $(this).data('id');
         const $btn = $(this);
         const $row = $btn.closest('tr');
 
         Alerta.Confirmar(
-            'Deseja realmente excluir esta ocorrência?',
-            'Esta ação não poderá ser desfeita!',
-            'Sim, excluir',
-            'Cancelar',
-        ).then((confirmado) => {
-            if (confirmado) {
+            "Deseja realmente excluir esta ocorrência?",
+            "Esta ação não poderá ser desfeita!",
+            "Sim, excluir",
+            "Cancelar"
+        ).then((confirmado) =>
+        {
+            if (confirmado)
+            {
                 excluirOcorrenciaViagem(ocorrenciaId, $row);
             }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'click.btn-excluir-ocorrencia',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "click.btn-excluir-ocorrencia", error);
     }
 });
 
-function excluirOcorrenciaViagem(ocorrenciaId, $row) {
-    try {
+function excluirOcorrenciaViagem(ocorrenciaId, $row)
+{
+    try
+    {
         $.ajax({
             url: '/api/OcorrenciaViagem/ExcluirOcorrencia',
             type: 'POST',
             contentType: 'application/json',
             data: JSON.stringify({ ocorrenciaViagemId: ocorrenciaId }),
-            success: function (response) {
-                try {
-                    if (response.success) {
-                        AppToast.show(
-                            'Verde',
-                            'Ocorrência excluída com sucesso',
-                            3000,
-                        );
-                        $row.fadeOut(300, function () {
+            success: function (response)
+            {
+                try
+                {
+                    if (response.success)
+                    {
+                        AppToast.show('Verde', 'Ocorrência excluída com sucesso', 3000);
+                        $row.fadeOut(300, function() {
                             $(this).remove();
 
-                            if (
-                                $('#tblOcorrenciasViagem tbody tr').length === 0
-                            ) {
-                                $('#tblOcorrenciasViagem tbody').html(
-                                    '<tr><td colspan="5" class="text-center text-muted">Nenhuma ocorrência registrada</td></tr>',
-                                );
+                            if ($('#tblOcorrenciasViagem tbody tr').length === 0)
+                            {
+                                $('#tblOcorrenciasViagem tbody').html('<tr><td colspan="5" class="text-center text-muted">Nenhuma ocorrência registrada</td></tr>');
                             }
                         });
-                    } else {
-                        AppToast.show(
-                            'Vermelho',
-                            response.message || 'Erro ao excluir ocorrência',
-                            4000,
-                        );
+                    } else
+                    {
+                        AppToast.show('Vermelho', response.message || 'Erro ao excluir ocorrência', 4000);
                     }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'ViagemIndex.js',
-                        'excluirOcorrenciaViagem.success',
-                        error,
-                    );
+                } catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("ViagemIndex.js", "excluirOcorrenciaViagem.success", error);
                 }
             },
-            error: function (xhr, status, error) {
+            error: function (xhr, status, error)
+            {
                 console.error('Erro ao excluir ocorrência:', error);
                 AppToast.show('Vermelho', 'Erro ao excluir ocorrência', 4000);
-            },
+            }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'excluirOcorrenciaViagem',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "excluirOcorrenciaViagem", error);
     }
 }
 
-$('#modalOcorrenciasViagem').on('hidden.bs.modal', function () {
-    try {
+$('#modalOcorrenciasViagem').on('hidden.bs.modal', function ()
+{
+    try
+    {
         this.removeAttribute('data-viagem-id');
         this.removeAttribute('data-noficha');
         $('#hiddenOcorrenciasViagemId').val('');
         $('#tblOcorrenciasViagem tbody').html('');
 
-        const tituloSpan = this.querySelector(
-            '#modalOcorrenciasViagemLabel span',
-        );
-        if (tituloSpan) {
+        const tituloSpan = this.querySelector('#modalOcorrenciasViagemLabel span');
+        if (tituloSpan)
+        {
             tituloSpan.textContent = 'Ocorrências da Viagem';
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'modalOcorrenciasViagem.hidden',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "modalOcorrenciasViagem.hidden", error);
     }
 });
 
-$(document).on('click', '.btn-baixar-ocorrencia:not(.disabled)', function (e) {
-    try {
+$(document).on('click', '.btn-baixar-ocorrencia:not(.disabled)', function (e)
+{
+    try
+    {
         e.preventDefault();
         const ocorrenciaId = $(this).data('id');
         const resumo = $(this).data('resumo') || 'esta ocorrência';
@@ -3407,139 +3159,128 @@
 
         Alerta.Confirmar(
             `Deseja dar baixa em: "${resumo}"?`,
-            'A ocorrência será marcada como resolvida',
-            'Sim, dar baixa',
-            'Cancelar',
-        ).then((confirmado) => {
-            if (confirmado) {
+            "A ocorrência será marcada como resolvida",
+            "Sim, dar baixa",
+            "Cancelar"
+        ).then((confirmado) =>
+        {
+            if (confirmado)
+            {
 
                 Alerta.Confirmar(
-                    'Deseja informar a solução aplicada?',
-                    'Você pode registrar como a ocorrência foi resolvida',
-                    'Sim, informar',
-                    'Não, apenas baixar',
-                ).then((querInformar) => {
-                    if (querInformar) {
+                    "Deseja informar a solução aplicada?",
+                    "Você pode registrar como a ocorrência foi resolvida",
+                    "Sim, informar",
+                    "Não, apenas baixar"
+                ).then((querInformar) =>
+                {
+                    if (querInformar)
+                    {
 
                         $('#hiddenBaixaOcorrenciaId').val(ocorrenciaId);
                         $('#txtSolucaoBaixa').val('');
-                        const modalSolucao = new bootstrap.Modal(
-                            document.getElementById('modalSolucaoOcorrencia'),
-                        );
+                        const modalSolucao = new bootstrap.Modal(document.getElementById('modalSolucaoOcorrencia'));
                         modalSolucao.show();
-                    } else {
+                    }
+                    else
+                    {
 
                         executarBaixaOcorrencia(ocorrenciaId, '', $row, $btn);
                     }
                 });
             }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'click.btn-baixar-ocorrencia',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "click.btn-baixar-ocorrencia", error);
     }
 });
 
-$(document).on('click', '#btnConfirmarBaixaSolucao', function (e) {
-    try {
+$(document).on('click', '#btnConfirmarBaixaSolucao', function (e)
+{
+    try
+    {
         e.preventDefault();
         const ocorrenciaId = $('#hiddenBaixaOcorrenciaId').val();
         const solucao = $('#txtSolucaoBaixa').val().trim();
 
-        const modalSolucao = bootstrap.Modal.getInstance(
-            document.getElementById('modalSolucaoOcorrencia'),
-        );
+        const modalSolucao = bootstrap.Modal.getInstance(document.getElementById('modalSolucaoOcorrencia'));
         if (modalSolucao) modalSolucao.hide();
 
         const $btn = $(`.btn-baixar-ocorrencia[data-id="${ocorrenciaId}"]`);
         const $row = $btn.closest('tr');
 
-        if ($btn.length > 0) {
+        if ($btn.length > 0)
+        {
             executarBaixaOcorrencia(ocorrenciaId, solucao, $row, $btn);
-        } else {
+        }
+        else
+        {
 
             executarBaixaOcorrenciaSemUI(ocorrenciaId, solucao);
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'click.btnConfirmarBaixaSolucao',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "click.btnConfirmarBaixaSolucao", error);
     }
 });
 
-function executarBaixaOcorrenciaSemUI(ocorrenciaId, solucao) {
-    try {
+function executarBaixaOcorrenciaSemUI(ocorrenciaId, solucao)
+{
+    try
+    {
         $.ajax({
             url: '/api/OcorrenciaViagem/BaixarOcorrenciaUpsert',
             type: 'POST',
             contentType: 'application/json',
             data: JSON.stringify({
                 ocorrenciaViagemId: ocorrenciaId,
-                solucaoOcorrencia: solucao || '',
+                solucaoOcorrencia: solucao || ''
             }),
-            success: function (response) {
-                try {
-
-                    const isSuccess =
-                        response.success === true || response.Success === true;
+            success: function (response)
+            {
+                try
+                {
+
+                    const isSuccess = response.success === true || response.Success === true;
                     const mensagem = response.message || response.Message;
 
-                    if (isSuccess) {
-                        AppToast.show(
-                            'Verde',
-                            'Ocorrência baixada com sucesso!',
-                            3000,
-                        );
+                    if (isSuccess)
+                    {
+                        AppToast.show('Verde', 'Ocorrência baixada com sucesso!', 3000);
 
                         const viagemId = $('#hiddenOcorrenciasViagemId').val();
-                        if (viagemId) {
+                        if (viagemId)
+                        {
                             carregarOcorrenciasViagem(viagemId);
                         }
-                    } else {
-                        AppToast.show(
-                            'Vermelho',
-                            mensagem || 'Erro ao baixar ocorrência',
-                            4000,
-                        );
                     }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'ViagemIndex.js',
-                        'executarBaixaOcorrenciaSemUI.success',
-                        error,
-                    );
+                    else
+                    {
+                        AppToast.show('Vermelho', mensagem || 'Erro ao baixar ocorrência', 4000);
+                    }
+                } catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("ViagemIndex.js", "executarBaixaOcorrenciaSemUI.success", error);
                 }
             },
-            error: function (xhr, status, error) {
-                console.error(
-                    'Erro ao baixar ocorrência:',
-                    xhr.responseText || error,
-                );
+            error: function (xhr, status, error)
+            {
+                console.error('Erro ao baixar ocorrência:', xhr.responseText || error);
                 AppToast.show('Vermelho', 'Erro ao baixar ocorrência', 4000);
-            },
+            }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'executarBaixaOcorrenciaSemUI',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "executarBaixaOcorrenciaSemUI", error);
     }
 }
 
-function executarBaixaOcorrencia(ocorrenciaId, solucao, $row, $btn) {
-    try {
-        console.log('executarBaixaOcorrencia chamada:', {
-            ocorrenciaId,
-            solucao,
-            temRow: !!$row,
-            temBtn: !!$btn,
-        });
+function executarBaixaOcorrencia(ocorrenciaId, solucao, $row, $btn)
+{
+    try
+    {
+        console.log('executarBaixaOcorrencia chamada:', { ocorrenciaId, solucao, temRow: !!$row, temBtn: !!$btn });
 
         $.ajax({
             url: '/api/OcorrenciaViagem/BaixarOcorrenciaUpsert',
@@ -3547,93 +3288,75 @@
             contentType: 'application/json',
             data: JSON.stringify({
                 ocorrenciaViagemId: ocorrenciaId,
-                solucaoOcorrencia: solucao || '',
+                solucaoOcorrencia: solucao || ''
             }),
-            success: function (response) {
-                try {
+            success: function (response)
+            {
+                try
+                {
                     console.log('Resposta da API:', response);
 
-                    const isSuccess =
-                        response.success === true || response.Success === true;
-                    const mensagem =
-                        response.message ||
-                        response.Message ||
-                        'Ocorrência baixada com sucesso!';
-
-                    if (isSuccess) {
+                    const isSuccess = response.success === true || response.Success === true;
+                    const mensagem = response.message || response.Message || 'Ocorrência baixada com sucesso!';
+
+                    if (isSuccess)
+                    {
                         AppToast.show('Verde', mensagem, 3000);
 
-                        if ($btn && $btn.length > 0) {
+                        if ($btn && $btn.length > 0)
+                        {
                             $btn.addClass('disabled')
                                 .attr('disabled', true)
                                 .attr('title', 'Já baixada')
                                 .prop('disabled', true);
 
-                            if ($row && $row.length > 0) {
+                            if ($row && $row.length > 0)
+                            {
                                 $row.addClass('table-success');
-                                setTimeout(
-                                    () => $row.removeClass('table-success'),
-                                    2000,
-                                );
+                                setTimeout(() => $row.removeClass('table-success'), 2000);
                             }
-                        } else {
-
-                            const viagemId = $(
-                                '#hiddenOcorrenciasViagemId',
-                            ).val();
-                            if (viagemId) {
+                        }
+                        else
+                        {
+
+                            const viagemId = $('#hiddenOcorrenciasViagemId').val();
+                            if (viagemId)
+                            {
                                 carregarOcorrenciasViagem(viagemId);
                             }
                         }
-                    } else {
-                        AppToast.show(
-                            'Vermelho',
-                            mensagem || 'Erro ao baixar ocorrência',
-                            4000,
-                        );
                     }
-                } catch (error) {
+                    else
+                    {
+                        AppToast.show('Vermelho', mensagem || 'Erro ao baixar ocorrência', 4000);
+                    }
+                } catch (error)
+                {
                     console.error('Erro no success:', error);
-                    Alerta.TratamentoErroComLinha(
-                        'ViagemIndex.js',
-                        'executarBaixaOcorrencia.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("ViagemIndex.js", "executarBaixaOcorrencia.success", error);
                 }
             },
-            error: function (xhr, status, error) {
-                console.error('Erro AJAX:', {
-                    xhr,
-                    status,
-                    error,
-                    responseText: xhr.responseText,
-                });
-                AppToast.show(
-                    'Vermelho',
-                    'Erro ao baixar ocorrência: ' + (xhr.responseText || error),
-                    4000,
-                );
-            },
+            error: function (xhr, status, error)
+            {
+                console.error('Erro AJAX:', { xhr, status, error, responseText: xhr.responseText });
+                AppToast.show('Vermelho', 'Erro ao baixar ocorrência: ' + (xhr.responseText || error), 4000);
+            }
         });
-    } catch (error) {
+    } catch (error)
+    {
         console.error('Erro geral:', error);
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'executarBaixaOcorrencia',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "executarBaixaOcorrencia", error);
     }
 }
 
-$('#modalSolucaoOcorrencia').on('hidden.bs.modal', function () {
-    try {
+$('#modalSolucaoOcorrencia').on('hidden.bs.modal', function ()
+{
+    try
+    {
         $('#hiddenBaixaOcorrenciaId').val('');
         $('#txtSolucaoBaixa').val('');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemIndex.js',
-            'modalSolucaoOcorrencia.hidden',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemIndex.js", "modalSolucaoOcorrencia.hidden", error);
     }
 });
```
