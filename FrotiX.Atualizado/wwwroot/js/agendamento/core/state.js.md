# wwwroot/js/agendamento/core/state.js

**Mudanca:** GRANDE | **+120** linhas | **-188** linhas

---

```diff
--- JANEIRO: wwwroot/js/agendamento/core/state.js
+++ ATUAL: wwwroot/js/agendamento/core/state.js
@@ -1,17 +1,19 @@
-class StateManager {
-    constructor() {
+class StateManager
+{
+    constructor()
+    {
         this.state = {
 
             viagem: {
-                id: '',
-                idAJAX: '',
-                recorrenciaId: '',
-                recorrenciaIdAJAX: '',
-                dataInicial: '',
-                dataInicialList: '',
-                horaInicial: '',
-                status: 'Aberta',
-                dados: null,
+                id: "",
+                idAJAX: "",
+                recorrenciaId: "",
+                recorrenciaIdAJAX: "",
+                dataInicial: "",
+                dataInicialList: "",
+                horaInicial: "",
+                status: "Aberta",
+                dados: null
             },
 
             ui: {
@@ -21,35 +23,40 @@
                 carregandoAgendamento: false,
                 carregandoViagemBloqueada: false,
                 inserindoRequisitante: false,
-                transformandoEmViagem: false,
+                transformandoEmViagem: false
             },
 
             recorrencia: {
                 agendamentos: [],
                 editarTodos: false,
-                datasSelecionadas: [],
+                datasSelecionadas: []
             },
 
             calendario: {
                 instancia: null,
-                selectedDates: [],
-            },
+                selectedDates: []
+            }
         };
 
         this.listeners = new Map();
     }
 
-    get(path) {
-        try {
+    get(path)
+    {
+        try
+        {
             return path.split('.').reduce((obj, key) => obj?.[key], this.state);
-        } catch (error) {
-            Alerta.TratamentoErroComLinha('state.js', 'get', error);
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("state.js", "get", error);
             return null;
         }
     }
 
-    set(path, value) {
-        try {
+    set(path, value)
+    {
+        try
+        {
             const keys = path.split('.');
             const lastKey = keys.pop();
             const target = keys.reduce((obj, key) => obj[key], this.state);
@@ -58,73 +65,88 @@
             target[lastKey] = value;
 
             this.notify(path, value, oldValue);
-        } catch (error) {
-            Alerta.TratamentoErroComLinha('state.js', 'set', error);
-        }
-    }
-
-    update(updates) {
-        try {
-            Object.entries(updates).forEach(([path, value]) => {
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("state.js", "set", error);
+        }
+    }
+
+    update(updates)
+    {
+        try
+        {
+            Object.entries(updates).forEach(([path, value]) =>
+            {
                 this.set(path, value);
             });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha('state.js', 'update', error);
-        }
-    }
-
-    subscribe(path, callback) {
-        try {
-            if (!this.listeners.has(path)) {
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("state.js", "update", error);
+        }
+    }
+
+    subscribe(path, callback)
+    {
+        try
+        {
+            if (!this.listeners.has(path))
+            {
                 this.listeners.set(path, []);
             }
             this.listeners.get(path).push(callback);
 
-            return () => {
+            return () =>
+            {
                 const callbacks = this.listeners.get(path);
                 const index = callbacks.indexOf(callback);
-                if (index > -1) {
+                if (index > -1)
+                {
                     callbacks.splice(index, 1);
                 }
             };
-        } catch (error) {
-            Alerta.TratamentoErroComLinha('state.js', 'subscribe', error);
-            return () => {};
-        }
-    }
-
-    notify(path, newValue, oldValue) {
-        try {
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("state.js", "subscribe", error);
+            return () => { };
+        }
+    }
+
+    notify(path, newValue, oldValue)
+    {
+        try
+        {
             const callbacks = this.listeners.get(path) || [];
-            callbacks.forEach((callback) => {
-                try {
+            callbacks.forEach(callback =>
+            {
+                try
+                {
                     callback(newValue, oldValue);
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'state.js',
-                        'notify_callback',
-                        error,
-                    );
+                } catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("state.js", "notify_callback", error);
                 }
             });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha('state.js', 'notify', error);
-        }
-    }
-
-    reset() {
-        try {
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("state.js", "notify", error);
+        }
+    }
+
+    reset()
+    {
+        try
+        {
             this.state = {
                 viagem: {
-                    id: '',
-                    idAJAX: '',
-                    recorrenciaId: '',
-                    recorrenciaIdAJAX: '',
-                    dataInicial: '',
-                    dataInicialList: '',
-                    horaInicial: '',
-                    status: 'Aberta',
-                    dados: null,
+                    id: "",
+                    idAJAX: "",
+                    recorrenciaId: "",
+                    recorrenciaIdAJAX: "",
+                    dataInicial: "",
+                    dataInicialList: "",
+                    horaInicial: "",
+                    status: "Aberta",
+                    dados: null
                 },
                 ui: {
                     modalAberto: false,
@@ -133,26 +155,28 @@
                     carregandoAgendamento: false,
                     carregandoViagemBloqueada: false,
                     inserindoRequisitante: false,
-                    transformandoEmViagem: false,
+                    transformandoEmViagem: false
                 },
                 recorrencia: {
                     agendamentos: [],
                     editarTodos: false,
-                    datasSelecionadas: [],
+                    datasSelecionadas: []
                 },
                 calendario: {
                     instancia: null,
-                    selectedDates: [],
-                },
+                    selectedDates: []
+                }
             };
 
             this.notify('*', this.state, null);
-        } catch (error) {
-            Alerta.TratamentoErroComLinha('state.js', 'reset', error);
-        }
-    }
-
-    getState() {
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("state.js", "reset", error);
+        }
+    }
+
+    getState()
+    {
         return { ...this.state };
     }
 }
@@ -161,202 +185,95 @@
 
 Object.defineProperty(window, 'viagemId', {
     get: () => window.AppState.get('viagem.id'),
-    set: (value) => window.AppState.set('viagem.id', value),
+    set: (value) => window.AppState.set('viagem.id', value)
 });
 
 Object.defineProperty(window, 'viagemId_AJAX', {
     get: () => window.AppState.get('viagem.idAJAX'),
-    set: (value) => window.AppState.set('viagem.idAJAX', value),
+    set: (value) => window.AppState.set('viagem.idAJAX', value)
 });
 
 Object.defineProperty(window, 'recorrenciaViagemId', {
     get: () => window.AppState.get('viagem.recorrenciaId'),
-    set: (value) => window.AppState.set('viagem.recorrenciaId', value),
+    set: (value) => window.AppState.set('viagem.recorrenciaId', value)
 });
 
 Object.defineProperty(window, 'recorrenciaViagemId_AJAX', {
     get: () => window.AppState.get('viagem.recorrenciaIdAJAX'),
-    set: (value) => window.AppState.set('viagem.recorrenciaIdAJAX', value),
+    set: (value) => window.AppState.set('viagem.recorrenciaIdAJAX', value)
 });
 
 Object.defineProperty(window, 'dataInicial', {
     get: () => window.AppState.get('viagem.dataInicial'),
-    set: (value) => window.AppState.set('viagem.dataInicial', value),
+    set: (value) => window.AppState.set('viagem.dataInicial', value)
 });
 
 Object.defineProperty(window, 'dataInicial_List', {
     get: () => window.AppState.get('viagem.dataInicialList'),
-    set: (value) => window.AppState.set('viagem.dataInicialList', value),
+    set: (value) => window.AppState.set('viagem.dataInicialList', value)
 });
 
 Object.defineProperty(window, 'horaInicial', {
     get: () => window.AppState.get('viagem.horaInicial'),
-    set: (value) => window.AppState.set('viagem.horaInicial', value),
+    set: (value) => window.AppState.set('viagem.horaInicial', value)
 });
 
 Object.defineProperty(window, 'StatusViagem', {
     get: () => window.AppState.get('viagem.status'),
-    set: (value) => window.AppState.set('viagem.status', value),
+    set: (value) => window.AppState.set('viagem.status', value)
 });
 
 Object.defineProperty(window, 'modalLock', {
     get: () => window.AppState.get('ui.modalLock'),
-    set: (value) => window.AppState.set('ui.modalLock', value),
+    set: (value) => window.AppState.set('ui.modalLock', value)
 });
 
 Object.defineProperty(window, 'isSubmitting', {
     get: () => window.AppState.get('ui.isSubmitting'),
-    set: (value) => window.AppState.set('ui.isSubmitting', value),
+    set: (value) => window.AppState.set('ui.isSubmitting', value)
 });
 
 Object.defineProperty(window, 'CarregandoAgendamento', {
     get: () => window.AppState.get('ui.carregandoAgendamento'),
-    set: (value) => window.AppState.set('ui.carregandoAgendamento', value),
+    set: (value) => window.AppState.set('ui.carregandoAgendamento', value)
 });
 
 Object.defineProperty(window, 'CarregandoViagemBloqueada', {
     get: () => window.AppState.get('ui.carregandoViagemBloqueada'),
-    set: (value) => window.AppState.set('ui.carregandoViagemBloqueada', value),
+    set: (value) => window.AppState.set('ui.carregandoViagemBloqueada', value)
 });
 
 Object.defineProperty(window, 'InserindoRequisitante', {
     get: () => window.AppState.get('ui.inserindoRequisitante'),
-    set: (value) => window.AppState.set('ui.inserindoRequisitante', value),
+    set: (value) => window.AppState.set('ui.inserindoRequisitante', value)
 });
 
 Object.defineProperty(window, 'transformandoEmViagem', {
     get: () => window.AppState.get('ui.transformandoEmViagem'),
-    set: (value) => window.AppState.set('ui.transformandoEmViagem', value),
+    set: (value) => window.AppState.set('ui.transformandoEmViagem', value)
 });
 
 Object.defineProperty(window, 'agendamentosRecorrentes', {
     get: () => window.AppState.get('recorrencia.agendamentos'),
-    set: (value) => window.AppState.set('recorrencia.agendamentos', value),
+    set: (value) => window.AppState.set('recorrencia.agendamentos', value)
 });
 
 Object.defineProperty(window, 'editarTodosRecorrentes', {
     get: () => window.AppState.get('recorrencia.editarTodos'),
-    set: (value) => window.AppState.set('recorrencia.editarTodos', value),
+    set: (value) => window.AppState.set('recorrencia.editarTodos', value)
 });
 
 Object.defineProperty(window, 'datasSelecionadas', {
     get: () => window.AppState.get('recorrencia.datasSelecionadas'),
-    set: (value) => window.AppState.set('recorrencia.datasSelecionadas', value),
+    set: (value) => window.AppState.set('recorrencia.datasSelecionadas', value)
 });
 
 Object.defineProperty(window, 'calendar', {
     get: () => window.AppState.get('calendario.instancia'),
-    set: (value) => window.AppState.set('calendario.instancia', value),
+    set: (value) => window.AppState.set('calendario.instancia', value)
 });
 
 Object.defineProperty(window, 'selectedDates', {
     get: () => window.AppState.get('calendario.selectedDates'),
-    set: (value) => window.AppState.set('calendario.selectedDates', value),
-});
-
-window.aplicarConfigRecorrencia = function () {
-    try {
-        const config = window.FrotiXConfig?.recorrencia || {};
-
-        if (typeof config.forcarTextoRecorrencia === 'boolean') {
-            window.forcarTextoRecorrencia = config.forcarTextoRecorrencia;
-        }
-
-        if (typeof config.forcarDatePickerRecorrencia === 'boolean') {
-            window.forcarDatePickerRecorrencia =
-                config.forcarDatePickerRecorrencia;
-        }
-
-        if (
-            config.forcarTextoRecorrencia === true &&
-            config.forcarDatePickerRecorrencia === true
-        ) {
-            window.forcarDatePickerRecorrencia = false;
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'state.js',
-            'aplicarConfigRecorrencia',
-            error,
-        );
-    }
-};
-
-window.inicializarToggleRecorrenciaDev = function () {
-    try {
-        const btnToggle = document.getElementById('btnToggleRecorrenciaDev');
-        const lblToggle = document.getElementById('lblToggleRecorrenciaDev');
-        const config = window.FrotiXConfig?.recorrencia || {};
-
-        if (!btnToggle) {
-            return;
-        }
-
-        const deveMostrar = config.mostrarToggleDev === true;
-        btnToggle.style.display = deveMostrar ? 'inline-flex' : 'none';
-
-        if (!deveMostrar) {
-            return;
-        }
-
-        const atualizarLabel = () => {
-            if (!lblToggle) return;
-            const textoAtual = window.forcarTextoRecorrencia === true;
-            lblToggle.textContent = textoAtual
-                ? 'Recorrência: Texto'
-                : 'Recorrência: DatePicker';
-        };
-
-        atualizarLabel();
-
-        btnToggle.addEventListener('click', function () {
-            try {
-                if (window.forcarTextoRecorrencia === true) {
-                    window.forcarTextoRecorrencia = false;
-                    window.forcarDatePickerRecorrencia = true;
-                } else {
-                    window.forcarTextoRecorrencia = true;
-                    window.forcarDatePickerRecorrencia = false;
-                }
-
-                atualizarLabel();
-
-                if (typeof Alerta !== 'undefined' && Alerta.Warning) {
-                    const modo = window.forcarTextoRecorrencia
-                        ? 'Texto readonly'
-                        : 'DatePicker';
-                    Alerta.Warning(
-                        'Recorrência (DEV)',
-                        `Modo alternado para ${modo}.`,
-                    );
-                }
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'state.js',
-                    'btnToggleRecorrenciaDev.click',
-                    error,
-                );
-            }
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'state.js',
-            'inicializarToggleRecorrenciaDev',
-            error,
-        );
-    }
-};
-
-document.addEventListener('DOMContentLoaded', function () {
-    try {
-        if (typeof window.aplicarConfigRecorrencia === 'function') {
-            window.aplicarConfigRecorrencia();
-        }
-
-        if (typeof window.inicializarToggleRecorrenciaDev === 'function') {
-            window.inicializarToggleRecorrenciaDev();
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('state.js', 'DOMContentLoaded', error);
-    }
-});
+    set: (value) => window.AppState.set('calendario.selectedDates', value)
+});
```
