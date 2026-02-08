# Models/Views/ViewGlosa.cs

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Models/Views/ViewGlosa.cs
+++ ATUAL: Models/Views/ViewGlosa.cs
@@ -7,40 +7,56 @@
     {
 
     [Keyless]
+
     public class ViewGlosa
-        {
+    {
 
         public string PlacaDescricao { get; set; }
 
         public Guid ContratoId { get; set; }
 
         public Guid ManutencaoId { get; set; }
+
         public string NumOS { get; set; }
+
         public string ResumoOS { get; set; }
 
         public string DataSolicitacao { get; set; }
+
         public string DataDisponibilidade { get; set; }
+
         public string DataRecolhimento { get; set; }
+
         public string DataRecebimentoReserva { get; set; }
+
         public string DataDevolucaoReserva { get; set; }
+
         public string DataEntrega { get; set; }
 
         public DateTime DataSolicitacaoRaw { get; set; }
+
         public DateTime? DataDisponibilidadeRaw { get; set; }
+
         public DateTime? DataDevolucaoRaw { get; set; }
 
         public string StatusOS { get; set; }
+
         public Guid VeiculoId { get; set; }
 
         public string DescricaoVeiculo { get; set; }
+
         public string Sigla { get; set; }
+
         public string CombustivelDescricao { get; set; }
+
         public string Placa { get; set; }
 
         public string Reserva { get; set; }
 
         public string Descricao { get; set; }
+
         public int? Quantidade { get; set; }
+
         public double? ValorUnitario { get; set; }
 
         public string DataDevolucao { get; set; }
@@ -53,21 +69,29 @@
         public int Dias { get; set; }
 
         public string Habilitado { get; set; }
+
         public string Icon { get; set; }
 
         public int? NumItem { get; set; }
 
         public string HabilitadoEditar { get; set; }
+
         public string OpacityEditar { get; set; }
+
         public string OpacityTooltipEditarEditar { get; set; }
 
         public string HabilitadoBaixar { get; set; }
+
         public string ModalBaixarAttrs { get; set; }
+
         public string OpacityBaixar { get; set; }
+
         public string Tooltip { get; set; }
 
         public string HabilitadoCancelar { get; set; }
+
         public string OpacityCancelar { get; set; }
+
         public string TooltipCancelar { get; set; }
         }
     }
```

### REMOVER do Janeiro

```csharp
        {
```


### ADICIONAR ao Janeiro

```csharp
    {
```
