# Models/Cadastros/Viagem.cs

**Mudanca:** GRANDE | **+12** linhas | **-34** linhas

---

```diff
--- JANEIRO: Models/Cadastros/Viagem.cs
+++ ATUAL: Models/Cadastros/Viagem.cs
@@ -6,28 +6,40 @@
 
 namespace FrotiX.Models
 {
+
     public class AgendamentoViagem
     {
+
         public string? CombustivelFinal { get; set; }
+
         public string? CombustivelInicial { get; set; }
 
         [NotMapped]
         public bool CriarViagemFechada { get; set; }
 
         public DateTime? DataAgendamento { get; set; }
+
         public DateTime? DataCancelamento { get; set; }
+
         public DateTime? DataCriacao { get; set; }
 
         [NotMapped]
         public List<DateTime>? DataEspecifica { get; set; }
 
         public DateTime? DataFinal { get; set; }
+
         public DateTime? DataFinalizacao { get; set; }
+
         public DateTime? DataFinalRecorrencia { get; set; }
+
         public DateTime? DataInicial { get; set; }
+
         public List<DateTime>? DatasSelecionadas { get; set; }
+
         public string? Descricao { get; set; }
+
         public string? Destino { get; set; }
+
         public int? DiaMesRecorrencia { get; set; }
 
         [NotMapped]
@@ -37,96 +49,126 @@
         public bool editarTodosRecorrentes { get; set; }
 
         public Guid? EventoId { get; set; }
+
         public string? Finalidade { get; set; }
+
         public bool FoiAgendamento { get; set; }
+
         public bool? Friday { get; set; }
+
         public DateTime? HoraFim { get; set; }
+
         public DateTime? HoraInicio { get; set; }
+
         public string? Intervalo { get; set; }
+
         public int? KmAtual { get; set; }
+
         public int? KmFinal { get; set; }
+
         public int? KmInicial { get; set; }
+
         public bool? Monday { get; set; }
+
         public Guid? MotoristaId { get; set; }
+
         public int? NoFichaVistoria { get; set; }
 
-        public bool? CintaEntregue { get; set; }
-        public bool? CintaDevolvida { get; set; }
-        public bool? TabletEntregue { get; set; }
-        public bool? TabletDevolvido { get; set; }
-        public bool? CaboEntregue { get; set; }
-        public bool? CaboDevolvido { get; set; }
-        public bool? ArlaEntregue { get; set; }
-        public bool? ArlaDevolvido { get; set; }
-
         [NotMapped]
         public bool OperacaoBemSucedida { get; set; }
 
         public string? Origem { get; set; }
+
         public string? RamalRequisitante { get; set; }
+
         public Guid? RecorrenciaViagemId { get; set; }
+
         public string? Recorrente { get; set; }
+
         public Guid? RequisitanteId { get; set; }
+
         public bool? Saturday { get; set; }
+
         public Guid? SetorSolicitanteId { get; set; }
+
         public string? Status { get; set; }
+
         public bool StatusAgendamento { get; set; }
+
         public bool? Sunday { get; set; }
+
         public bool? Thursday { get; set; }
+
         public bool? Tuesday { get; set; }
+
         public string? UsuarioIdAgendamento { get; set; }
+
         public string? UsuarioIdCancelamento { get; set; }
+
         public string? UsuarioIdCriacao { get; set; }
+
         public string? UsuarioIdFinalizacao { get; set; }
+
         public Guid? VeiculoId { get; set; }
+
         public Guid ViagemId { get; set; }
+
         public bool? Wednesday { get; set; }
     }
 
     public class AjusteViagem
     {
+
         [NotMapped]
         public IFormFile? ArquivoFoto { get; set; }
 
         public DateTime? DataFinal { get; set; }
+
         public DateTime? DataInicial { get; set; }
+
         public Guid? EventoId { get; set; }
+
         public string? Finalidade { get; set; }
+
         public DateTime? HoraFim { get; set; }
+
         public DateTime? HoraInicial { get; set; }
+
         public int? KmFinal { get; set; }
+
         public int? KmInicial { get; set; }
-        public bool? CintaEntregue { get; set; }
-        public bool? CintaDevolvida { get; set; }
-        public bool? TabletEntregue { get; set; }
-        public bool? TabletDevolvido { get; set; }
-        public bool? CaboEntregue { get; set; }
-        public bool? CaboDevolvido { get; set; }
-        public bool? ArlaEntregue { get; set; }
-        public bool? ArlaDevolvido { get; set; }
+
         public Guid? MotoristaId { get; set; }
+
         public int? NoFichaVistoria { get; set; }
+
         public Guid? SetorSolicitanteId { get; set; }
+
         public Guid? VeiculoId { get; set; }
+
         public Guid ViagemId { get; set; }
     }
 
     public class FinalizacaoViagem
     {
+
         [NotMapped]
         public IFormFile? ArquivoFoto { get; set; }
 
         public string? CombustivelFinal { get; set; }
+
         public DateTime? DataFinal { get; set; }
+
         public string? Descricao { get; set; }
+
         public DateTime? HoraFim { get; set; }
+
         public int? KmFinal { get; set; }
-        public bool? CintaDevolvida { get; set; }
-        public bool? TabletDevolvido { get; set; }
-        public bool? CaboDevolvido { get; set; }
-        public bool? ArlaDevolvido { get; set; }
+
         public string? StatusCartaoAbastecimento { get; set; }
+
         public string? StatusDocumento { get; set; }
+
         public Guid ViagemId { get; set; }
 
         public List<OcorrenciaFinalizacaoDTO>? Ocorrencias { get; set; }
@@ -134,22 +176,31 @@
 
     public class OcorrenciaFinalizacaoDTO
     {
+
         public string? Resumo { get; set; }
+
         public string? Descricao { get; set; }
+
         public string? ImagemOcorrencia { get; set; }
     }
 
     public class ProcuraViagemViewModel
     {
+
         public string? Data { get; set; }
+
         public string? Hora { get; set; }
+
         public int? NoFichaVistoria { get; set; }
+
         public Guid? VeiculoId { get; set; }
+
         public Viagem? Viagem { get; set; }
     }
 
     public class Viagem
     {
+
         [NotMapped]
         public IFormFile? ArquivoFoto { get; set; }
 
@@ -210,6 +261,7 @@
         public string? Finalidade { get; set; }
 
         public bool? FoiAgendamento { get; set; }
+
         public bool? Friday { get; set; }
 
         [DataType(DataType.DateTime)]
@@ -235,6 +287,7 @@
         public int? KmRodado { get; set; }
 
         public int? Minutos { get; set; }
+
         public bool? Monday { get; set; }
 
         [ForeignKey("MotoristaId")]
@@ -246,9 +299,6 @@
         [Display(Name = "Nº Ficha Vistoria")]
         public int? NoFichaVistoria { get; set; }
 
-        [Display(Name = "Tem Ficha Real")]
-        public bool? TemFichaVistoriaReal { get; set; }
-
         [Display(Name = "Nome do Evento")]
         public string? NomeEvento { get; set; }
 
@@ -262,6 +312,7 @@
         public string? RamalRequisitante { get; set; }
 
         public Guid? RecorrenciaViagemId { get; set; }
+
         public string? Recorrente { get; set; }
 
         [ForeignKey("RequisitanteId")]
@@ -279,18 +330,29 @@
         public Guid? SetorSolicitanteId { get; set; }
 
         public string? Status { get; set; }
+
         public bool? StatusAgendamento { get; set; }
+
         public string? StatusCartaoAbastecimento { get; set; }
+
         public string? StatusCartaoAbastecimentoFinal { get; set; }
+
         public string? StatusDocumento { get; set; }
+
         public string? StatusDocumentoFinal { get; set; }
 
         public bool? Sunday { get; set; }
+
         public bool? Thursday { get; set; }
+
         public bool? Tuesday { get; set; }
+
         public string? UsuarioIdAgendamento { get; set; }
+
         public string? UsuarioIdCancelamento { get; set; }
+
         public string? UsuarioIdCriacao { get; set; }
+
         public string? UsuarioIdFinalizacao { get; set; }
 
         [ForeignKey("VeiculoId")]
@@ -316,18 +378,30 @@
         [Display(Name = "Tablet Devolvido")]
         public bool? TabletDevolvido { get; set; }
 
+        [Display(Name = "Documento Entregue")]
+        public bool? DocumentoEntregue { get; set; }
+
+        [Display(Name = "Documento Devolvido")]
+        public bool? DocumentoDevolvido { get; set; }
+
+        [Display(Name = "Cartão Abastecimento Entregue")]
+        public bool? CartaoAbastecimentoEntregue { get; set; }
+
+        [Display(Name = "Cartão Abastecimento Devolvido")]
+        public bool? CartaoAbastecimentoDevolvido { get; set; }
+
+        [Display(Name = "Arla Entregue")]
+        public bool? ArlaEntregue { get; set; }
+
+        [Display(Name = "Arla Devolvido")]
+        public bool? ArlaDevolvido { get; set; }
+
         [Display(Name = "Cabo Entregue")]
         public bool? CaboEntregue { get; set; }
 
         [Display(Name = "Cabo Devolvido")]
         public bool? CaboDevolvido { get; set; }
 
-        [Display(Name = "Arla Entregue")]
-        public bool? ArlaEntregue { get; set; }
-
-        [Display(Name = "Arla Devolvido")]
-        public bool? ArlaDevolvido { get; set; }
-
         [Display(Name = "Vistoriador Inicial")]
         public string? VistoriadorInicialId { get; set; }
 
@@ -335,6 +409,7 @@
         public string? VistoriadorFinalId { get; set; }
 
         public string? Rubrica { get; set; }
+
         public string? RubricaFinal { get; set; }
 
         [Display(Name = "Foi Normalizada")]
@@ -406,14 +481,6 @@
                 this.RecorrenciaViagemId = viagem.RecorrenciaViagemId;
                 this.Intervalo = viagem.Intervalo;
                 this.DataFinalRecorrencia = viagem.DataFinalRecorrencia;
-                this.CintaEntregue = viagem.CintaEntregue;
-                this.CintaDevolvida = viagem.CintaDevolvida;
-                this.TabletEntregue = viagem.TabletEntregue;
-                this.TabletDevolvido = viagem.TabletDevolvido;
-                this.CaboEntregue = viagem.CaboEntregue;
-                this.CaboDevolvido = viagem.CaboDevolvido;
-                this.ArlaEntregue = viagem.ArlaEntregue;
-                this.ArlaDevolvido = viagem.ArlaDevolvido;
                 this.Monday = viagem.Monday;
                 this.Tuesday = viagem.Tuesday;
                 this.Wednesday = viagem.Wednesday;
@@ -430,21 +497,33 @@
 
     public class ViagemID
     {
+
         public Guid ViagemId { get; set; }
     }
 
     public class ViagemViewModel
     {
+
         public DateTime? DataCancelamento { get; set; }
+
         public string? DataFinalizacao { get; set; }
+
         public byte[]? FichaVistoria { get; set; }
+
         public string? HoraFinalizacao { get; set; }
+
         public string? NomeUsuarioAgendamento { get; set; }
+
         public string? NomeUsuarioCancelamento { get; set; }
+
         public string? NomeUsuarioCriacao { get; set; }
+
         public string? NomeUsuarioFinalizacao { get; set; }
+
         public string? UsuarioIdCancelamento { get; set; }
+
         public Viagem? Viagem { get; set; }
+
         public Guid ViagemId { get; set; }
     }
 }
```

### REMOVER do Janeiro

```csharp
        public bool? CintaEntregue { get; set; }
        public bool? CintaDevolvida { get; set; }
        public bool? TabletEntregue { get; set; }
        public bool? TabletDevolvido { get; set; }
        public bool? CaboEntregue { get; set; }
        public bool? CaboDevolvido { get; set; }
        public bool? ArlaEntregue { get; set; }
        public bool? ArlaDevolvido { get; set; }
        public bool? CintaEntregue { get; set; }
        public bool? CintaDevolvida { get; set; }
        public bool? TabletEntregue { get; set; }
        public bool? TabletDevolvido { get; set; }
        public bool? CaboEntregue { get; set; }
        public bool? CaboDevolvido { get; set; }
        public bool? ArlaEntregue { get; set; }
        public bool? ArlaDevolvido { get; set; }
        public bool? CintaDevolvida { get; set; }
        public bool? TabletDevolvido { get; set; }
        public bool? CaboDevolvido { get; set; }
        public bool? ArlaDevolvido { get; set; }
        [Display(Name = "Tem Ficha Real")]
        public bool? TemFichaVistoriaReal { get; set; }
        [Display(Name = "Arla Entregue")]
        public bool? ArlaEntregue { get; set; }
        [Display(Name = "Arla Devolvido")]
        public bool? ArlaDevolvido { get; set; }
                this.CintaEntregue = viagem.CintaEntregue;
                this.CintaDevolvida = viagem.CintaDevolvida;
                this.TabletEntregue = viagem.TabletEntregue;
                this.TabletDevolvido = viagem.TabletDevolvido;
                this.CaboEntregue = viagem.CaboEntregue;
                this.CaboDevolvido = viagem.CaboDevolvido;
                this.ArlaEntregue = viagem.ArlaEntregue;
                this.ArlaDevolvido = viagem.ArlaDevolvido;
```


### ADICIONAR ao Janeiro

```csharp
        [Display(Name = "Documento Entregue")]
        public bool? DocumentoEntregue { get; set; }
        [Display(Name = "Documento Devolvido")]
        public bool? DocumentoDevolvido { get; set; }
        [Display(Name = "Cartão Abastecimento Entregue")]
        public bool? CartaoAbastecimentoEntregue { get; set; }
        [Display(Name = "Cartão Abastecimento Devolvido")]
        public bool? CartaoAbastecimentoDevolvido { get; set; }
        [Display(Name = "Arla Entregue")]
        public bool? ArlaEntregue { get; set; }
        [Display(Name = "Arla Devolvido")]
        public bool? ArlaDevolvido { get; set; }
```
