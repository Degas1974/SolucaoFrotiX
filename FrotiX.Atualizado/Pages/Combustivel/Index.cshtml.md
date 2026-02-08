# Pages/Combustivel/Index.cshtml

**Mudanca:** MEDIA | **+1** linhas | **-12** linhas

---

```diff
--- JANEIRO: Pages/Combustivel/Index.cshtml
+++ ATUAL: Pages/Combustivel/Index.cshtml
@@ -1,18 +1,4 @@
 @page
-
-/*
-    ═══════════════════════════════════════════════════════════════════════════════
-    📄 DOCUMENTAÇÃO COMPLETA DISPONÍVEL
-    ═══════════════════════════════════════════════════════════════════════════════
-
-    📍 Localização: Documentacao/Pages/Combustivel - Index.md
-    📅 Última Atualização: 08/01/2026
-    📋 Versão: 2.0 (Padrão FrotiX Simplificado)
-
-    Este arquivo contém a View Razor da página de listagem de Combustíveis.
-    Para entender completamente a funcionalidade, consulte a documentação acima.
-    ═══════════════════════════════════════════════════════════════════════════════
-*/
 
 @model FrotiX.Models.Combustivel
 
@@ -64,7 +50,7 @@
                                 <tr>
                                     <th>Descrição do Combustível</th>
                                     <th>Status</th>
-                                    <th>Ações</th>
+                                    <th>Ação</th>
                                 </tr>
                             </thead>
                             <tbody>
```

### REMOVER do Janeiro

```html
/*
    ═══════════════════════════════════════════════════════════════════════════════
    📄 DOCUMENTAÇÃO COMPLETA DISPONÍVEL
    ═══════════════════════════════════════════════════════════════════════════════
    📍 Localização: Documentacao/Pages/Combustivel - Index.md
    📅 Última Atualização: 08/01/2026
    📋 Versão: 2.0 (Padrão FrotiX Simplificado)
    Este arquivo contém a View Razor da página de listagem de Combustíveis.
    Para entender completamente a funcionalidade, consulte a documentação acima.
    ═══════════════════════════════════════════════════════════════════════════════
*/
                                    <th>Ações</th>
```


### ADICIONAR ao Janeiro

```html
                                    <th>Ação</th>
```
