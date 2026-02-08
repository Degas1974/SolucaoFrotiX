# Pages/Operador/Index.cshtml

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Pages/Operador/Index.cshtml
+++ ATUAL: Pages/Operador/Index.cshtml
@@ -1,4 +1,5 @@
 @page
+
 @model FrotiX.Pages.Operador.IndexModel
 
 @{
@@ -320,7 +321,7 @@
                                     <th>Celular</th>
                                     <th>Contrato</th>
                                     <th>Status</th>
-                                    <th>Ações</th>
+                                    <th>Ação</th>
                                 </tr>
                             </thead>
                             <tbody></tbody>
```

### REMOVER do Janeiro

```html
                                    <th>Ações</th>
```


### ADICIONAR ao Janeiro

```html
                                    <th>Ação</th>
```
