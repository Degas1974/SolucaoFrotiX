# wwwroot/js/agendamento/utils/calendario-config.js

**Mudanca:** GRANDE | **+32** linhas | **-31** linhas

---

```diff
--- JANEIRO: wwwroot/js/agendamento/utils/calendario-config.js
+++ ATUAL: wwwroot/js/agendamento/utils/calendario-config.js
@@ -1,21 +1,24 @@
-function configurarCalendarioPtBR() {
+function configurarCalendarioPtBR()
+{
 
-    if (typeof ej !== 'undefined' && ej.base && ej.base.L10n) {
+    if (typeof ej !== 'undefined' && ej.base && ej.base.L10n)
+    {
         ej.base.L10n.load({
             'pt-BR': {
-                calendar: {
-                    today: 'Hoje',
+                'calendar': {
+                    today: 'Hoje'
                 },
-                datepicker: {
+                'datepicker': {
                     placeholder: 'Selecione uma data',
-                    today: 'Hoje',
-                },
-            },
+                    today: 'Hoje'
+                }
+            }
         });
     }
 }
 
-function criarCalendario(elementoId, dataInicial) {
+function criarCalendario(elementoId, dataInicial)
+{
 
     var calendario = new ej.calendars.Calendar({
         value: dataInicial || new Date(),
@@ -23,49 +26,47 @@
 
         dayHeaderFormat: 'Short',
 
-        created: function () {
+        created: function ()
+        {
             traduzirCalendario(elementoId);
         },
-        navigated: function () {
+        navigated: function ()
+        {
             traduzirCalendario(elementoId);
-        },
+        }
     });
 
     calendario.appendTo('#' + elementoId);
     return calendario;
 }
 
-function traduzirCalendario(elementoId) {
+function traduzirCalendario(elementoId)
+{
 
     var diasSemana = ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb'];
     var meses = [
-        'Janeiro',
-        'Fevereiro',
-        'Março',
-        'Abril',
-        'Maio',
-        'Junho',
-        'Julho',
-        'Agosto',
-        'Setembro',
-        'Outubro',
-        'Novembro',
-        'Dezembro',
+        'Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho',
+        'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'
     ];
 
-    setTimeout(function () {
+    setTimeout(function ()
+    {
 
         var elemento = document.getElementById(elementoId);
-        if (elemento) {
+        if (elemento)
+        {
             var headers = elemento.querySelectorAll('.e-calendar th');
-            headers.forEach(function (header, index) {
-                if (index < diasSemana.length) {
+            headers.forEach(function (header, index)
+            {
+                if (index < diasSemana.length)
+                {
                     header.textContent = diasSemana[index];
                 }
             });
 
             var titulo = elemento.querySelector('.e-title');
-            if (titulo) {
+            if (titulo)
+            {
                 var textoOriginal = titulo.textContent;
 
                 var dataAtual = titulo.getAttribute('aria-label');
@@ -78,5 +79,5 @@
 window.CalendarioConfig = {
     configurar: configurarCalendarioPtBR,
     criar: criarCalendario,
-    traduzir: traduzirCalendario,
+    traduzir: traduzirCalendario
 };
```

### REMOVER do Janeiro

```javascript
function configurarCalendarioPtBR() {
    if (typeof ej !== 'undefined' && ej.base && ej.base.L10n) {
                calendar: {
                    today: 'Hoje',
                datepicker: {
                    today: 'Hoje',
                },
            },
function criarCalendario(elementoId, dataInicial) {
        created: function () {
        navigated: function () {
        },
function traduzirCalendario(elementoId) {
        'Janeiro',
        'Fevereiro',
        'Março',
        'Abril',
        'Maio',
        'Junho',
        'Julho',
        'Agosto',
        'Setembro',
        'Outubro',
        'Novembro',
        'Dezembro',
    setTimeout(function () {
        if (elemento) {
            headers.forEach(function (header, index) {
                if (index < diasSemana.length) {
            if (titulo) {
    traduzir: traduzirCalendario,
```


### ADICIONAR ao Janeiro

```javascript
function configurarCalendarioPtBR()
{
    if (typeof ej !== 'undefined' && ej.base && ej.base.L10n)
    {
                'calendar': {
                    today: 'Hoje'
                'datepicker': {
                    today: 'Hoje'
                }
            }
function criarCalendario(elementoId, dataInicial)
{
        created: function ()
        {
        navigated: function ()
        {
        }
function traduzirCalendario(elementoId)
{
        'Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho',
        'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'
    setTimeout(function ()
    {
        if (elemento)
        {
            headers.forEach(function (header, index)
            {
                if (index < diasSemana.length)
                {
            if (titulo)
            {
    traduzir: traduzirCalendario
```
