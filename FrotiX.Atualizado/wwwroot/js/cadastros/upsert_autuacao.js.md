# wwwroot/js/cadastros/upsert_autuacao.js

**Mudanca:** MEDIA | **+6** linhas | **-14** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/upsert_autuacao.js
+++ ATUAL: wwwroot/js/cadastros/upsert_autuacao.js
@@ -349,9 +349,7 @@
                                 'Já existe uma Multa inserida com esta numeração'
                             );
                         } else {
-                            alert(
-                                'Já existe uma Multa inserida com esta numeração'
-                            );
+                            console.error('[upsert_autuacao.js] Já existe uma Multa inserida com esta numeração');
                         }
                     }
                 } catch (error) {
@@ -466,9 +464,7 @@
                                 'O veículo escolhido não possui contrato ou ata'
                             );
                         } else {
-                            alert(
-                                'O veículo escolhido não possui contrato ou ata'
-                            );
+                            console.error('[upsert_autuacao.js] O veículo escolhido não possui contrato ou ata');
                         }
                     }
                 } catch (innerError) {
@@ -544,9 +540,7 @@
                             'O veículo escolhido não pertence a esse contrato'
                         );
                     } else {
-                        alert(
-                            'O veículo escolhido não pertence a esse contrato'
-                        );
+                        console.error('[upsert_autuacao.js] O veículo escolhido não pertence a esse contrato');
                     }
 
                     const lstV =
@@ -613,7 +607,7 @@
                             'O veículo escolhido não pertence a essa ata'
                         );
                     } else {
-                        alert('O veículo escolhido não pertence a essa ata');
+                        console.error('[upsert_autuacao.js] O veículo escolhido não pertence a essa ata');
                     }
 
                     const lstV =
@@ -674,7 +668,7 @@
                             'O motorista escolhido não possui contrato'
                         );
                     } else {
-                        alert('O motorista escolhido não possui contrato');
+                        console.error('[upsert_autuacao.js] O motorista escolhido não possui contrato');
                     }
                 }
             },
@@ -730,9 +724,7 @@
                             'O motorista escolhido não pertence a esse contrato'
                         );
                     } else {
-                        alert(
-                            'O motorista escolhido não pertence a esse contrato'
-                        );
+                        console.error('[upsert_autuacao.js] O motorista escolhido não pertence a esse contrato');
                     }
 
                     const lstM =
```

### REMOVER do Janeiro

```javascript
                            alert(
                                'Já existe uma Multa inserida com esta numeração'
                            );
                            alert(
                                'O veículo escolhido não possui contrato ou ata'
                            );
                        alert(
                            'O veículo escolhido não pertence a esse contrato'
                        );
                        alert('O veículo escolhido não pertence a essa ata');
                        alert('O motorista escolhido não possui contrato');
                        alert(
                            'O motorista escolhido não pertence a esse contrato'
                        );
```


### ADICIONAR ao Janeiro

```javascript
                            console.error('[upsert_autuacao.js] Já existe uma Multa inserida com esta numeração');
                            console.error('[upsert_autuacao.js] O veículo escolhido não possui contrato ou ata');
                        console.error('[upsert_autuacao.js] O veículo escolhido não pertence a esse contrato');
                        console.error('[upsert_autuacao.js] O veículo escolhido não pertence a essa ata');
                        console.error('[upsert_autuacao.js] O motorista escolhido não possui contrato');
                        console.error('[upsert_autuacao.js] O motorista escolhido não pertence a esse contrato');
```
