# Services/Servicos.cs

**Mudanca:** MEDIA | **+7** linhas | **-2** linhas

---

```diff
--- JANEIRO: Services/Servicos.cs
+++ ATUAL: Services/Servicos.cs
@@ -36,18 +36,26 @@
         {
             try
             {
+
                 var veiculoObj = _unitOfWork.ViewVeiculos.GetFirstOrDefault(v => v.VeiculoId == viagemObj.VeiculoId);
 
-                var combustivelObj = _unitOfWork.Abastecimento.GetAll(a => a.VeiculoId == viagemObj.VeiculoId).OrderByDescending(o => o.DataHora);
+                var combustivelObj = _unitOfWork.Abastecimento
+                    .GetAll(a => a.VeiculoId == viagemObj.VeiculoId)
+                    .OrderByDescending(o => o.DataHora);
 
                 double ValorCombustivel = 0;
                 if (combustivelObj.FirstOrDefault() == null)
                 {
-                    var abastecimentoObj = _unitOfWork.MediaCombustivel.GetAll(a => a.CombustivelId == veiculoObj.CombustivelId).OrderByDescending(o => o.Ano).ThenByDescending(o => o.Mes);
+
+                    var abastecimentoObj = _unitOfWork.MediaCombustivel
+                        .GetAll(a => a.CombustivelId == veiculoObj.CombustivelId)
+                        .OrderByDescending(o => o.Ano)
+                        .ThenByDescending(o => o.Mes);
                     ValorCombustivel = (double)abastecimentoObj.FirstOrDefault().PrecoMedio;
                 }
                 else
                 {
+
                     ValorCombustivel = (double)combustivelObj.FirstOrDefault().ValorUnitario;
                 }
 
@@ -75,6 +83,7 @@
         {
             try
             {
+
                 var veiculoObj = _unitOfWork.Veiculo.GetFirstOrDefault(v => v.VeiculoId == viagemObj.VeiculoId);
                 double valorUnitario = ObterValorUnitarioVeiculo(veiculoObj , _unitOfWork);
 
@@ -111,6 +120,7 @@
         {
             try
             {
+
                 var motoristaObj = _unitOfWork.Motorista.GetFirstOrDefault(m => m.MotoristaId == viagemObj.MotoristaId);
 
                 if (motoristaObj.ContratoId == null)
@@ -393,6 +403,7 @@
         {
             try
             {
+
                 System.Diagnostics.Debug.WriteLine($"[MEDIA QUERY] Iniciando cálculo para {dataViagem:dd/MM/yyyy}");
 
                 System.Diagnostics.Debug.WriteLine($"[MEDIA QUERY] Obtendo query (sem materializar)...");
@@ -404,7 +415,6 @@
                 );
 
                 System.Diagnostics.Debug.WriteLine($"[MEDIA QUERY] Executando COUNT no SQL...");
-
                 int totalViagens = await Task.Run(() => query.Count());
 
                 System.Diagnostics.Debug.WriteLine($"[MEDIA QUERY] Total: {totalViagens}");
@@ -416,7 +426,6 @@
                 }
 
                 System.Diagnostics.Debug.WriteLine($"[MEDIA QUERY] Executando MIN no SQL...");
-
                 DateTime primeiraViagem = await Task.Run(() => query.Min(v => v.DataInicial.Value));
 
                 System.Diagnostics.Debug.WriteLine($"[MEDIA QUERY] Primeira viagem: {primeiraViagem:dd/MM/yyyy}");
@@ -453,6 +462,7 @@
             {
                 if (html != null)
                 {
+
                     HtmlDocument doc = new HtmlDocument();
                     doc.LoadHtml(html);
 
@@ -501,6 +511,7 @@
                         break;
 
                     case HtmlNodeType.Document:
+
                         ConvertContentTo(node , outText);
                         break;
 
@@ -517,11 +528,13 @@
 
                         if (html.Trim().Length > 0)
                         {
+
                             outText.Write(HtmlEntity.DeEntitize(html));
                         }
                         break;
 
                     case HtmlNodeType.Element:
+
                         switch (node.Name)
                         {
                             case "p":
@@ -547,8 +560,10 @@
         {
             try
             {
+
                 foreach (HtmlNode subnode in node.ChildNodes)
                 {
+
                     ConvertTo(subnode , outText);
                 }
             }
```

### REMOVER do Janeiro

```csharp
                var combustivelObj = _unitOfWork.Abastecimento.GetAll(a => a.VeiculoId == viagemObj.VeiculoId).OrderByDescending(o => o.DataHora);
                    var abastecimentoObj = _unitOfWork.MediaCombustivel.GetAll(a => a.CombustivelId == veiculoObj.CombustivelId).OrderByDescending(o => o.Ano).ThenByDescending(o => o.Mes);
```


### ADICIONAR ao Janeiro

```csharp
                var combustivelObj = _unitOfWork.Abastecimento
                    .GetAll(a => a.VeiculoId == viagemObj.VeiculoId)
                    .OrderByDescending(o => o.DataHora);
                    var abastecimentoObj = _unitOfWork.MediaCombustivel
                        .GetAll(a => a.CombustivelId == veiculoObj.CombustivelId)
                        .OrderByDescending(o => o.Ano)
                        .ThenByDescending(o => o.Mes);
```
