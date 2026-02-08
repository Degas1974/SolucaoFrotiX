# wwwroot/js/custom.js

**Mudanca:** GRANDE | **+36** linhas | **-6** linhas

---

```diff
--- JANEIRO: wwwroot/js/custom.js
+++ ATUAL: wwwroot/js/custom.js
@@ -1,23 +1,53 @@
 function openNav() {
-    document.getElementById('mySidenav').style.width = '253px';
+    try {
+        document.getElementById("mySidenav").style.width = "253px";
+    } catch (erro) {
+        console.error('Erro em openNav:', erro);
+        Alerta.TratamentoErroComLinha('custom.js', 'openNav', erro);
+    }
 }
 
 function closeNav() {
-    document.getElementById('mySidenav').style.width = '0';
+    try {
+        document.getElementById("mySidenav").style.width = "0";
+    } catch (erro) {
+        console.error('Erro em closeNav:', erro);
+        Alerta.TratamentoErroComLinha('custom.js', 'closeNav', erro);
+    }
 }
 
 function openNav2() {
-    document.getElementById('profile').style.width = '300px';
+    try {
+        document.getElementById("profile").style.width = "300px";
+    } catch (erro) {
+        console.error('Erro em openNav2:', erro);
+        Alerta.TratamentoErroComLinha('custom.js', 'openNav2', erro);
+    }
 }
 
 function closeNav2() {
-    document.getElementById('profile').style.width = '0';
+    try {
+        document.getElementById("profile").style.width = "0";
+    } catch (erro) {
+        console.error('Erro em closeNav2:', erro);
+        Alerta.TratamentoErroComLinha('custom.js', 'closeNav2', erro);
+    }
 }
 
 function openNav3() {
-    document.getElementById('profile2').style.width = '301px';
+    try {
+        document.getElementById("profile2").style.width = "301px";
+    } catch (erro) {
+        console.error('Erro em openNav3:', erro);
+        Alerta.TratamentoErroComLinha('custom.js', 'openNav3', erro);
+    }
 }
 
 function closeNav3() {
-    document.getElementById('profile2').style.width = '0';
+    try {
+        document.getElementById("profile2").style.width = "0";
+    } catch (erro) {
+        console.error('Erro em closeNav3:', erro);
+        Alerta.TratamentoErroComLinha('custom.js', 'closeNav3', erro);
+    }
 }
```

### REMOVER do Janeiro

```javascript
    document.getElementById('mySidenav').style.width = '253px';
    document.getElementById('mySidenav').style.width = '0';
    document.getElementById('profile').style.width = '300px';
    document.getElementById('profile').style.width = '0';
    document.getElementById('profile2').style.width = '301px';
    document.getElementById('profile2').style.width = '0';
```


### ADICIONAR ao Janeiro

```javascript
    try {
        document.getElementById("mySidenav").style.width = "253px";
    } catch (erro) {
        console.error('Erro em openNav:', erro);
        Alerta.TratamentoErroComLinha('custom.js', 'openNav', erro);
    }
    try {
        document.getElementById("mySidenav").style.width = "0";
    } catch (erro) {
        console.error('Erro em closeNav:', erro);
        Alerta.TratamentoErroComLinha('custom.js', 'closeNav', erro);
    }
    try {
        document.getElementById("profile").style.width = "300px";
    } catch (erro) {
        console.error('Erro em openNav2:', erro);
        Alerta.TratamentoErroComLinha('custom.js', 'openNav2', erro);
    }
    try {
        document.getElementById("profile").style.width = "0";
    } catch (erro) {
        console.error('Erro em closeNav2:', erro);
        Alerta.TratamentoErroComLinha('custom.js', 'closeNav2', erro);
    }
    try {
        document.getElementById("profile2").style.width = "301px";
    } catch (erro) {
        console.error('Erro em openNav3:', erro);
        Alerta.TratamentoErroComLinha('custom.js', 'openNav3', erro);
    }
    try {
        document.getElementById("profile2").style.width = "0";
    } catch (erro) {
        console.error('Erro em closeNav3:', erro);
        Alerta.TratamentoErroComLinha('custom.js', 'closeNav3', erro);
    }
```
