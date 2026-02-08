# wwwroot/js/agendamento/utils/date.utils.js

**Mudanca:** GRANDE | **+101** linhas | **-93** linhas

---

```diff
--- JANEIRO: wwwroot/js/agendamento/utils/date.utils.js
+++ ATUAL: wwwroot/js/agendamento/utils/date.utils.js
@@ -1,176 +1,184 @@
-window.arredondarHora = function (hora, intervaloMinutos = 10) {
-    try {
+window.arredondarHora = function (hora, intervaloMinutos = 10)
+{
+    try
+    {
         const m = moment(hora);
         const minutos = m.minutes();
         const resto = minutos % intervaloMinutos;
 
-        if (resto !== 0) {
+        if (resto !== 0)
+        {
             m.add(intervaloMinutos - resto, 'minutes');
         }
 
         m.seconds(0);
         m.milliseconds(0);
 
-        return m.format('HH:mm');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('date.utils.js', 'arredondarHora', error);
-        return '00:00';
+        return m.format("HH:mm");
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("date.utils.js", "arredondarHora", error);
+        return "00:00";
     }
 };
 
-window.toDateOnlyString = function (d) {
-    try {
+window.toDateOnlyString = function (d)
+{
+    try
+    {
         const dt = d instanceof Date ? d : new Date(d);
         const y = dt.getFullYear();
-        const m = String(dt.getMonth() + 1).padStart(2, '0');
-        const dd = String(dt.getDate()).padStart(2, '0');
+        const m = String(dt.getMonth() + 1).padStart(2, "0");
+        const dd = String(dt.getDate()).padStart(2, "0");
         return `${y}-${m}-${dd}`;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'date.utils.js',
-            'toDateOnlyString',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("date.utils.js", "toDateOnlyString", error);
         return null;
     }
 };
 
-window.toLocalDateOnly = function (date) {
-    try {
+window.toLocalDateOnly = function (date)
+{
+    try
+    {
         const d = new Date(date);
         return new Date(d.getFullYear(), d.getMonth(), d.getDate());
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'date.utils.js',
-            'toLocalDateOnly',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("date.utils.js", "toLocalDateOnly", error);
         return null;
     }
 };
 
-window.toLocalDateTimeString = function (date, timeStr) {
-    try {
+window.toLocalDateTimeString = function (date, timeStr)
+{
+    try
+    {
         if (!date) return null;
-        const [hh, mm] = (timeStr || '').split(':').map(Number);
+        const [hh, mm] = (timeStr || "").split(":").map(Number);
         const d = new Date(date);
-        if (!isNaN(hh) && !isNaN(mm)) {
+        if (!isNaN(hh) && !isNaN(mm))
+        {
             d.setHours(hh, mm, 0, 0);
-            return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}T${String(hh).padStart(2, '0')}:${String(mm).padStart(2, '0')}:00`;
+            return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, "0")}-${String(d.getDate()).padStart(2, "0")}T${String(hh).padStart(2, "0")}:${String(mm).padStart(2, "0")}:00`;
         }
         return null;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'date.utils.js',
-            'toLocalDateTimeString',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("date.utils.js", "toLocalDateTimeString", error);
         return null;
     }
 };
 
-window.formatDate = function (dateObj) {
-    try {
-        const day = ('0' + dateObj.getDate()).slice(-2);
-        const month = ('0' + (dateObj.getMonth() + 1)).slice(-2);
+window.formatDate = function (dateObj)
+{
+    try
+    {
+        const day = ("0" + dateObj.getDate()).slice(-2);
+        const month = ("0" + (dateObj.getMonth() + 1)).slice(-2);
         const year = dateObj.getFullYear();
         return `${day}/${month}/${year}`;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('date.utils.js', 'formatDate', error);
-        return '';
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("date.utils.js", "formatDate", error);
+        return "";
     }
 };
 
-window.fmtDateLocal = function (d) {
-    try {
+window.fmtDateLocal = function (d)
+{
+    try
+    {
         const dt = new Date(d);
         const y = dt.getFullYear();
-        const m = String(dt.getMonth() + 1).padStart(2, '0');
-        const day = String(dt.getDate()).padStart(2, '0');
+        const m = String(dt.getMonth() + 1).padStart(2, "0");
+        const day = String(dt.getDate()).padStart(2, "0");
         return `${y}-${m}-${day}`;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('date.utils.js', 'fmtDateLocal', error);
-        return '';
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("date.utils.js", "fmtDateLocal", error);
+        return "";
     }
 };
 
-window.makeLocalDateTime = function (yyyyMMdd, hhmm) {
-    try {
-        const [hh, mm] = String(hhmm || '00:00').split(':');
-        return `${yyyyMMdd}T${String(hh).padStart(2, '0')}:${String(mm).padStart(2, '0')}:00`;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'date.utils.js',
-            'makeLocalDateTime',
-            error,
-        );
-        return '';
+window.makeLocalDateTime = function (yyyyMMdd, hhmm)
+{
+    try
+    {
+        const [hh, mm] = String(hhmm || "00:00").split(":");
+        return `${yyyyMMdd}T${String(hh).padStart(2, "0")}:${String(mm).padStart(2, "0")}:00`;
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("date.utils.js", "makeLocalDateTime", error);
+        return "";
     }
 };
 
-window.parseDate = function (d) {
-    try {
+window.parseDate = function (d)
+{
+    try
+    {
         if (!d) return null;
 
-        if (d instanceof Date && !isNaN(d)) {
+        if (d instanceof Date && !isNaN(d))
+        {
             return d;
         }
 
         const s = String(d).trim();
 
-        if (/^\d{1,2}\/\d{1,2}\/\d{4}$/.test(s)) {
-            const [dia, mes, ano] = s.split('/');
+        if (/^\d{1,2}\/\d{1,2}\/\d{4}$/.test(s))
+        {
+            const [dia, mes, ano] = s.split("/");
             return new Date(Number(ano), Number(mes) - 1, Number(dia));
         }
 
-        if (/^\d{4}-\d{1,2}-\d{1,2}$/.test(s)) {
-            const [ano, mes, dia] = s.split('-');
+        if (/^\d{4}-\d{1,2}-\d{1,2}$/.test(s))
+        {
+            const [ano, mes, dia] = s.split("-");
             return new Date(Number(ano), Number(mes) - 1, Number(dia));
         }
 
         const parsed = Date.parse(s);
-        if (!isNaN(parsed)) {
+        if (!isNaN(parsed))
+        {
             return new Date(parsed);
         }
 
         return null;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('date.utils.js', 'parseDate', error);
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("date.utils.js", "parseDate", error);
         return null;
     }
 };
 
-window.addDaysLocal = function (dateString, days) {
-    try {
+window.addDaysLocal = function (dateString, days)
+{
+    try
+    {
         if (!dateString) return null;
         const d = new Date(dateString);
         if (isNaN(d)) return null;
         d.setDate(d.getDate() + (Number.isFinite(days) ? days : 0));
         const pad = (n) => String(n).padStart(2, '0');
-        return (
-            d.getFullYear() +
-            '-' +
-            pad(d.getMonth() + 1) +
-            '-' +
-            pad(d.getDate()) +
-            'T' +
-            pad(d.getHours()) +
-            ':' +
-            pad(d.getMinutes()) +
-            ':' +
-            pad(d.getSeconds())
-        );
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('date.utils.js', 'addDaysLocal', error);
+        return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()) + 'T' + pad(d.getHours()) + ':' + pad(d.getMinutes()) + ':' + pad(d.getSeconds());
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("date.utils.js", "addDaysLocal", error);
         return null;
     }
 };
 
-window.delay = function (ms) {
-    try {
-        return new Promise((resolve) => setTimeout(resolve, ms));
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('date.utils.js', 'delay', error);
+window.delay = function (ms)
+{
+    try
+    {
+        return new Promise(resolve => setTimeout(resolve, ms));
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("date.utils.js", "delay", error);
         return Promise.resolve();
     }
 };
```
