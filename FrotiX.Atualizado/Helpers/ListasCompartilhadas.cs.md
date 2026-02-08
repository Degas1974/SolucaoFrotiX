# Helpers/ListasCompartilhadas.cs

**Mudanca:** GRANDE | **+431** linhas | **-649** linhas

---

```diff
--- JANEIRO: Helpers/ListasCompartilhadas.cs
+++ ATUAL: Helpers/ListasCompartilhadas.cs
@@ -2,32 +2,31 @@
 using FrotiX.Repository.IRepository;
 using System;
 using System.Collections.Generic;
-using System.ComponentModel.DataAnnotations;
 using System.Globalization;
 using System.Linq;
 
 namespace FrotiX.Helpers
 {
 
-    internal sealed class PtBrComparer : IComparer<string>
+    internal sealed class PtBrComparer :IComparer<string>
     {
         private static readonly CompareInfo Cmp = new CultureInfo("pt-BR").CompareInfo;
 
+        public int Compare(string x , string y)
+        {
+        return Cmp.Compare(
+            x ?? string.Empty ,
+            y ?? string.Empty ,
+            CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
+        );
+        }
+    }
+
+    public class NaturalStringComparer : IComparer<string>
+    {
+
         public int Compare(string x, string y)
         {
-            return Cmp.Compare(
-                x ?? string.Empty,
-                y ?? string.Empty,
-                CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
-            );
-        }
-    }
-
-    public class NaturalStringComparer : IComparer<string>
-    {
-        public int Compare(string x, string y)
-        {
-
             if (x == null && y == null) return 0;
             if (x == null) return -1;
             if (y == null) return 1;
@@ -60,9 +59,9 @@
                     if (xNum != yNum)
                         return xNum.CompareTo(yNum);
                 }
-
                 else
                 {
+
                     int charComparison = string.Compare(
                         x[ix].ToString(),
                         y[iy].ToString(),
@@ -101,15 +100,14 @@
 
         public ListaFinalidade(IUnitOfWork unitOfWork)
         {
-            _unitOfWork = unitOfWork;
+        _unitOfWork = unitOfWork;
         }
 
         public List<ListaFinalidade> FinalidadesList()
         {
-            try
-            {
-
-                List<ListaFinalidade> finalidades = new List<ListaFinalidade>
+        try
+        {
+        List<ListaFinalidade> finalidades = new List<ListaFinalidade>
                 {
                     new ListaFinalidade { FinalidadeId = "Transporte de Funcionários", Descricao = "Transporte de Funcionários" },
                     new ListaFinalidade { FinalidadeId = "Transporte de Convidados", Descricao = "Transporte de Convidados" },
@@ -135,14 +133,14 @@
                     new ListaFinalidade { FinalidadeId = "Cursos Depol", Descricao = "Cursos Depol" }
                 };
 
-                return finalidades.OrderBy(f => f.Descricao, new PtBrComparer()).ToList();
-            }
-            catch (Exception ex)
-            {
-
-                System.Diagnostics.Debug.WriteLine($"Erro em FinalidadesList - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
-                return new List<ListaFinalidade>();
-            }
+        return finalidades.OrderBy(f => f.Descricao , new PtBrComparer()).ToList();
+        }
+        catch (Exception ex)
+        {
+
+        System.Diagnostics.Debug.WriteLine($"Erro em FinalidadesList - Linha: {new System.Diagnostics.StackTrace(ex , true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
+        return new List<ListaFinalidade>();
+        }
         }
     }
 
@@ -169,15 +167,14 @@
 
         public ListaNivelCombustivel(IUnitOfWork unitOfWork)
         {
-            _unitOfWork = unitOfWork;
+        _unitOfWork = unitOfWork;
         }
 
         public List<ListaNivelCombustivel> NivelCombustivelList()
         {
-            try
-            {
-
-                return new List<ListaNivelCombustivel>
+        try
+        {
+        return new List<ListaNivelCombustivel>
                 {
                     new ListaNivelCombustivel { Nivel = "tanquevazio", Descricao = "Vazio", Imagem = "../images/tanquevazio.png" },
                     new ListaNivelCombustivel { Nivel = "tanqueumquarto", Descricao = "1/4", Imagem = "../images/tanqueumquarto.png" },
@@ -185,76 +182,253 @@
                     new ListaNivelCombustivel { Nivel = "tanquetresquartos", Descricao = "3/4", Imagem = "../images/tanquetresquartos.png" },
                     new ListaNivelCombustivel { Nivel = "tanquecheio", Descricao = "Cheio", Imagem = "../images/tanquecheio.png" }
                 };
+        }
+        catch (Exception ex)
+        {
+        System.Diagnostics.Debug.WriteLine($"Erro em NivelCombustivelList - Linha: {new System.Diagnostics.StackTrace(ex , true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
+        return new List<ListaNivelCombustivel>();
+        }
+        }
+    }
+
+    public class ListaVeiculos
+    {
+        public string Descricao
+        {
+            get; set;
+        }
+        public Guid VeiculoId
+        {
+            get; set;
+        }
+
+        private readonly IUnitOfWork _unitOfWork;
+
+        public ListaVeiculos()
+        {
+        }
+
+        public ListaVeiculos(IUnitOfWork unitOfWork)
+        {
+        _unitOfWork = unitOfWork;
+        }
+
+        public IEnumerable<ListaVeiculos> VeiculosList()
+        {
+        try
+        {
+        var veiculos = (
+            from v in _unitOfWork.Veiculo.GetAllReduced(
+                includeProperties: nameof(ModeloVeiculo) + "," + nameof(MarcaVeiculo) ,
+                selector: v => new
+                {
+                    v.VeiculoId ,
+                    v.Placa ,
+                    v.MarcaVeiculo.DescricaoMarca ,
+                    v.ModeloVeiculo.DescricaoModelo ,
+                    v.Status ,
+                }
+            )
+            where v.Status == true
+            select new ListaVeiculos
+            {
+                VeiculoId = v?.VeiculoId ?? Guid.Empty ,
+                Descricao = $"{v.Placa} - {v.DescricaoMarca}/{v.DescricaoModelo}" ,
             }
-            catch (Exception ex)
-            {
-                System.Diagnostics.Debug.WriteLine($"Erro em NivelCombustivelList - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
-                return new List<ListaNivelCombustivel>();
+        ).OrderBy(v => v.Descricao);
+
+        return veiculos;
+        }
+        catch (Exception ex)
+        {
+        System.Diagnostics.Debug.WriteLine($"Erro em VeiculosList - Linha: {new System.Diagnostics.StackTrace(ex , true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
+        return Enumerable.Empty<ListaVeiculos>();
+        }
+        }
+    }
+
+    public class ListaMotorista
+    {
+        public Guid MotoristaId
+        {
+            get; set;
+        }
+        public string Nome
+        {
+            get; set;
+        }
+        public string FotoBase64
+        {
+            get; set;
+        }
+        public bool Status
+        {
+            get; set;
+        }
+
+        private readonly IUnitOfWork _unitOfWork;
+
+        public ListaMotorista()
+        {
+        }
+
+        public ListaMotorista(IUnitOfWork unitOfWork)
+        {
+        _unitOfWork = unitOfWork;
+        }
+
+        public IEnumerable<ListaMotorista> MotoristaList()
+        {
+        try
+        {
+        var motoristas = _unitOfWork.ViewMotoristas.GetAllReduced(
+            orderBy: m => m.OrderBy(m => m.Nome) ,
+            selector: motorista => new ListaMotorista
+            {
+                MotoristaId = motorista.MotoristaId ,
+                Nome = motorista.MotoristaCondutor ,
+                FotoBase64 = motorista.Foto != null
+                    ? $"data:image/jpeg;base64,{Convert.ToBase64String(motorista.Foto)}"
+                    : null ,
+                Status = motorista.Status ,
             }
-        }
-    }
-
-    public class ListaVeiculos
-    {
-        public string Descricao
-        {
-            get; set;
-        }
-        public Guid VeiculoId
-        {
-            get; set;
-        }
-
-        private readonly IUnitOfWork _unitOfWork;
-
-        public ListaVeiculos()
-        {
-        }
-
-        public ListaVeiculos(IUnitOfWork unitOfWork)
-        {
-            _unitOfWork = unitOfWork;
-        }
-
-        public IEnumerable<ListaVeiculos> VeiculosList()
-        {
-            try
-            {
-
-                var veiculos = (
-                    from v in _unitOfWork.Veiculo.GetAllReduced(
-                        includeProperties: nameof(ModeloVeiculo) + "," + nameof(MarcaVeiculo),
-                        selector: v => new
-                        {
-                            v.VeiculoId,
-                            v.Placa,
-                            v.MarcaVeiculo.DescricaoMarca,
-                            v.ModeloVeiculo.DescricaoModelo,
-                            v.Status,
-                        }
-                    )
-
-                    select new ListaVeiculos
-                    {
-                        VeiculoId = v?.VeiculoId ?? Guid.Empty,
-                        Descricao = $"{v.Placa} - {v.DescricaoMarca}/{v.DescricaoModelo}",
-                    }
-
-                ).OrderBy(v => v.Descricao);
-
-                return veiculos;
+        );
+
+        return motoristas.Where(m => m.Status == true);
+        }
+        catch (Exception ex)
+        {
+        System.Diagnostics.Debug.WriteLine($"Erro em MotoristaList - Linha: {new System.Diagnostics.StackTrace(ex , true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
+        return Enumerable.Empty<ListaMotorista>();
+        }
+        }
+    }
+
+    public class ListaRequisitante
+    {
+        public string Requisitante
+        {
+            get; set;
+        }
+        public Guid RequisitanteId
+        {
+            get; set;
+        }
+
+        private readonly IUnitOfWork _unitOfWork;
+
+        public ListaRequisitante()
+        {
+        }
+
+        public ListaRequisitante(IUnitOfWork unitOfWork)
+        {
+        _unitOfWork = unitOfWork;
+        }
+
+        public IEnumerable<ListaRequisitante> RequisitantesList()
+        {
+        try
+        {
+
+        var requisitantes = _unitOfWork.ViewRequisitantes.GetAllReduced(
+            selector: r => new ListaRequisitante
+            {
+                Requisitante = r.Requisitante ,
+                RequisitanteId = (Guid)r.RequisitanteId ,
             }
-            catch (Exception ex)
-            {
-                System.Diagnostics.Debug.WriteLine($"Erro em VeiculosList - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
-                return Enumerable.Empty<ListaVeiculos>();
+        ).ToList();
+
+        return requisitantes
+            .Select(r => new ListaRequisitante
+            {
+                Requisitante = (r.Requisitante ?? "").Trim(),
+                RequisitanteId = r.RequisitanteId
+            })
+            .OrderBy(r => r.Requisitante ?? "", new NaturalStringComparer())
+            .ToList();
+        }
+        catch (Exception ex)
+        {
+        System.Diagnostics.Debug.WriteLine($"Erro em RequisitantesList - Linha: {new System.Diagnostics.StackTrace(ex , true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
+        return Enumerable.Empty<ListaRequisitante>();
+        }
+        }
+    }
+
+    public class ListaEvento
+    {
+        public string Evento
+        {
+            get; set;
+        }
+        public Guid EventoId
+        {
+            get; set;
+        }
+        public string Status
+        {
+            get; set;
+        }
+
+        private readonly IUnitOfWork _unitOfWork;
+
+        public ListaEvento()
+        {
+        }
+
+        public ListaEvento(IUnitOfWork unitOfWork)
+        {
+        _unitOfWork = unitOfWork;
+        }
+
+        public IEnumerable<ListaEvento> EventosList()
+        {
+        try
+        {
+        var eventos = _unitOfWork.Evento.GetAllReduced(
+            orderBy: n => n.OrderBy(n => n.Nome) ,
+            selector: n => new ListaEvento
+            {
+                Evento = n.Nome ,
+                EventoId = n.EventoId ,
+                Status = n.Status ,
             }
-        }
-    }
-
-    public class ListaMotorista
-    {
-        public Guid MotoristaId
+        );
+
+        return eventos.Where(e => e.Status == "1");
+        }
+        catch (Exception ex)
+        {
+        System.Diagnostics.Debug.WriteLine($"Erro em EventosList - Linha: {new System.Diagnostics.StackTrace(ex , true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
+        return Enumerable.Empty<ListaEvento>();
+        }
+        }
+    }
+
+    public class ListaSetores
+    {
+        public string SetorSolicitanteId
+        {
+            get; set;
+        }
+        public string SetorPaiId
+        {
+            get; set;
+        }
+        public bool HasChild
+        {
+            get; set;
+        }
+        public string Sigla
+        {
+            get; set;
+        }
+        public bool Expanded
+        {
+            get; set;
+        }
+        public bool IsSelected
         {
             get; set;
         }
@@ -262,186 +436,67 @@
         {
             get; set;
         }
-        public string FotoBase64
-        {
-            get; set;
-        }
-        public bool Status
-        {
-            get; set;
-        }
-
-        private readonly IUnitOfWork _unitOfWork;
-
-        public ListaMotorista()
-        {
-        }
-
-        public ListaMotorista(IUnitOfWork unitOfWork)
-        {
-            _unitOfWork = unitOfWork;
-        }
-
-        public IEnumerable<ListaMotorista> MotoristaList()
-        {
-            try
-            {
-
-                var motoristas = _unitOfWork.ViewMotoristas.GetAllReduced(
-                    orderBy: m => m.OrderBy(m => m.Nome),
-                    selector: motorista => new ListaMotorista
-                    {
-                        MotoristaId = motorista.MotoristaId,
-                        Nome = motorista.MotoristaCondutor,
-
-                        FotoBase64 = motorista.Foto != null
-                            ? $"data:image/jpeg;base64,{Convert.ToBase64String(motorista.Foto)}"
-                            : null,
-                        Status = motorista.Status,
-                    }
-                );
-
-                return motoristas;
-            }
-            catch (Exception ex)
-            {
-                System.Diagnostics.Debug.WriteLine($"Erro em MotoristaList - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
-                return Enumerable.Empty<ListaMotorista>();
-            }
-        }
-    }
-
-    public class ListaRequisitante
-    {
-        public string Requisitante
-        {
-            get; set;
-        }
-        public Guid RequisitanteId
-        {
-            get; set;
-        }
-
-        private readonly IUnitOfWork _unitOfWork;
-
-        public ListaRequisitante()
-        {
-        }
-
-        public ListaRequisitante(IUnitOfWork unitOfWork)
-        {
-            _unitOfWork = unitOfWork;
-        }
-
-        public IEnumerable<ListaRequisitante> RequisitantesList()
-        {
-            try
-            {
-
-                var requisitantes = _unitOfWork.ViewRequisitantes.GetAllReduced(
-                    selector: r => new ListaRequisitante
-                    {
-                        Requisitante = r.Requisitante,
-                        RequisitanteId = (Guid)r.RequisitanteId,
-                    }
-                ).ToList();
-
-                return requisitantes
-                    .Select(r => new ListaRequisitante
-                    {
-
-                        Requisitante = (r.Requisitante ?? "").Trim(),
-                        RequisitanteId = r.RequisitanteId
-                    })
-
-                    .OrderBy(r => r.Requisitante ?? "", new NaturalStringComparer())
-                    .ToList();
-            }
-            catch (Exception ex)
-            {
-                System.Diagnostics.Debug.WriteLine($"Erro em RequisitantesList - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
-                return Enumerable.Empty<ListaRequisitante>();
-            }
-        }
-    }
-
-    public class ListaEvento
-    {
-        public string Evento
-        {
-            get; set;
-        }
-        public Guid EventoId
-        {
-            get; set;
-        }
-        public string Status
-        {
-            get; set;
-        }
-
-        private readonly IUnitOfWork _unitOfWork;
-
-        public ListaEvento()
-        {
-        }
-
-        public ListaEvento(IUnitOfWork unitOfWork)
-        {
-            _unitOfWork = unitOfWork;
-        }
-
-        public IEnumerable<ListaEvento> EventosList()
-        {
-            try
-            {
-
-                var eventos = _unitOfWork.Evento.GetAllReduced(
-                    orderBy: n => n.OrderBy(n => n.Nome),
-                    selector: n => new ListaEvento
-                    {
-                        Evento = n.Nome,
-                        EventoId = n.EventoId,
-                        Status = n.Status,
-                    }
-                );
-
-                return eventos;
-            }
-            catch (Exception ex)
-            {
-                System.Diagnostics.Debug.WriteLine($"Erro em EventosList - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
-                return Enumerable.Empty<ListaEvento>();
-            }
-        }
-    }
-
-    public class ListaSetores
+
+        private readonly IUnitOfWork _unitOfWork;
+
+        public ListaSetores()
+        {
+        }
+
+        public ListaSetores(IUnitOfWork unitOfWork)
+        {
+        _unitOfWork = unitOfWork;
+        }
+
+        public List<ListaSetores> SetoresList()
+        {
+        try
+        {
+        var objSetores = _unitOfWork.ViewSetores.GetAll();
+
+        if (objSetores == null || !objSetores.Any())
+        {
+        System.Diagnostics.Debug.WriteLine("⚠️ ATENÇÃO: Nenhum setor encontrado no banco de dados.");
+        return new List<ListaSetores>();
+        }
+
+        List<ListaSetores> treeDataSource = new List<ListaSetores>();
+
+        foreach (var setor in objSetores)
+        {
+        bool temFilho = _unitOfWork.ViewSetores.GetFirstOrDefault(u =>
+            u.SetorPaiId == setor.SetorSolicitanteId
+        ) != null;
+
+        treeDataSource.Add(new ListaSetores
+        {
+            SetorSolicitanteId = setor.SetorSolicitanteId.ToString() ,
+            SetorPaiId = setor.SetorPaiId == null || setor.SetorPaiId == Guid.Empty
+                ? null
+                : setor.SetorPaiId.ToString() ,
+            Nome = setor.Nome ,
+            HasChild = temFilho
+        });
+        }
+
+        System.Diagnostics.Debug.WriteLine($"✅ SetoresList carregou {treeDataSource.Count} setores");
+        return treeDataSource;
+        }
+        catch (Exception ex)
+        {
+        System.Diagnostics.Debug.WriteLine($"❌ ERRO CRÍTICO em SetoresList: {ex.Message}");
+        System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
+        throw;
+        }
+        }
+    }
+
+    public class ListaSetoresEvento
     {
         public string SetorSolicitanteId
         {
             get; set;
         }
-        public string SetorPaiId
-        {
-            get; set;
-        }
-        public bool HasChild
-        {
-            get; set;
-        }
-        public string Sigla
-        {
-            get; set;
-        }
-        public bool Expanded
-        {
-            get; set;
-        }
-        public bool IsSelected
-        {
-            get; set;
-        }
         public string Nome
         {
             get; set;
@@ -449,63 +504,49 @@
 
         private readonly IUnitOfWork _unitOfWork;
 
-        public ListaSetores()
-        {
-        }
-
-        public ListaSetores(IUnitOfWork unitOfWork)
-        {
-            _unitOfWork = unitOfWork;
-        }
-
-        public List<ListaSetores> SetoresList()
-        {
-            try
-            {
-
-                var objSetores = _unitOfWork.ViewSetores.GetAll();
-
-                if (objSetores == null || !objSetores.Any())
-                {
-                    System.Diagnostics.Debug.WriteLine("⚠️ ATENÇÃO: Nenhum setor encontrado no banco de dados.");
-                    return new List<ListaSetores>();
-                }
-
-                List<ListaSetores> treeDataSource = new List<ListaSetores>();
-
-                foreach (var setor in objSetores)
-                {
-
-                    bool temFilho = _unitOfWork.ViewSetores.GetFirstOrDefault(u =>
-                        u.SetorPaiId == setor.SetorSolicitanteId
-                    ) != null;
-
-                    treeDataSource.Add(new ListaSetores
-                    {
-                        SetorSolicitanteId = setor.SetorSolicitanteId.ToString(),
-
-                        SetorPaiId = setor.SetorPaiId == null || setor.SetorPaiId == Guid.Empty
-                            ? null
-                            : setor.SetorPaiId.ToString(),
-                        Nome = setor.Nome,
-                        HasChild = temFilho
-                    });
-                }
-
-                System.Diagnostics.Debug.WriteLine($"✅ SetoresList carregou {treeDataSource.Count} setores");
-                return treeDataSource;
-            }
-            catch (Exception ex)
-            {
-
-                System.Diagnostics.Debug.WriteLine($"❌ ERRO CRÍTICO em SetoresList: {ex.Message}");
-                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
-                throw;
-            }
-        }
-    }
-
-    public class ListaSetoresEvento
+        public ListaSetoresEvento()
+        {
+        }
+
+        public ListaSetoresEvento(IUnitOfWork unitOfWork)
+        {
+        _unitOfWork = unitOfWork;
+        }
+
+        public List<ListaSetoresEvento> SetoresEventoList()
+        {
+        try
+        {
+        var objSetores = _unitOfWork.SetorSolicitante.GetAll();
+
+        if (objSetores == null || !objSetores.Any())
+        {
+        System.Diagnostics.Debug.WriteLine("Nenhum setor encontrado para eventos.");
+        return new List<ListaSetoresEvento>();
+        }
+
+        List<ListaSetoresEvento> treeDataSource = new List<ListaSetoresEvento>();
+
+        foreach (var setor in objSetores)
+        {
+        treeDataSource.Add(new ListaSetoresEvento
+        {
+            SetorSolicitanteId = setor.SetorSolicitanteId.ToString() ,
+            Nome = $"{setor.Nome} ({setor.Sigla})" ,
+        });
+        }
+
+        return treeDataSource.OrderBy(s => s.Nome).ToList();
+        }
+        catch (Exception ex)
+        {
+        System.Diagnostics.Debug.WriteLine($"Erro em SetoresEventoList - Linha: {new System.Diagnostics.StackTrace(ex , true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
+        return new List<ListaSetoresEvento>();
+        }
+        }
+    }
+
+    public class ListaSetoresFlat
     {
         public string SetorSolicitanteId
         {
@@ -515,63 +556,6 @@
         {
             get; set;
         }
-
-        private readonly IUnitOfWork _unitOfWork;
-
-        public ListaSetoresEvento()
-        {
-        }
-
-        public ListaSetoresEvento(IUnitOfWork unitOfWork)
-        {
-            _unitOfWork = unitOfWork;
-        }
-
-        public List<ListaSetoresEvento> SetoresEventoList()
-        {
-            try
-            {
-
-                var objSetores = _unitOfWork.SetorSolicitante.GetAll();
-
-                if (objSetores == null || !objSetores.Any())
-                {
-                    System.Diagnostics.Debug.WriteLine("Nenhum setor encontrado para eventos.");
-                    return new List<ListaSetoresEvento>();
-                }
-
-                List<ListaSetoresEvento> treeDataSource = new List<ListaSetoresEvento>();
-
-                foreach (var setor in objSetores)
-                {
-                    treeDataSource.Add(new ListaSetoresEvento
-                    {
-                        SetorSolicitanteId = setor.SetorSolicitanteId.ToString(),
-
-                        Nome = $"{setor.Nome} ({setor.Sigla})",
-                    });
-                }
-
-                return treeDataSource.OrderBy(s => s.Nome).ToList();
-            }
-            catch (Exception ex)
-            {
-                System.Diagnostics.Debug.WriteLine($"Erro em SetoresEventoList - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
-                return new List<ListaSetoresEvento>();
-            }
-        }
-    }
-
-    public class ListaSetoresFlat
-    {
-        public string SetorSolicitanteId
-        {
-            get; set;
-        }
-        public string Nome
-        {
-            get; set;
-        }
         public int Nivel
         {
             get; set;
@@ -585,7 +569,7 @@
 
         public ListaSetoresFlat(IUnitOfWork unitOfWork)
         {
-            _unitOfWork = unitOfWork;
+        _unitOfWork = unitOfWork;
         }
 
         private class SetorHierarquia
@@ -606,90 +590,86 @@
 
         public List<ListaSetoresFlat> SetoresListFlat()
         {
-            try
-            {
-
-                var objSetores = _unitOfWork.ViewSetores.GetAllReduced(
-                    selector: x => new SetorHierarquia
-                    {
-                        SetorSolicitanteId = x.SetorSolicitanteId,
-                        SetorPaiId = x.SetorPaiId ?? Guid.Empty,
-                        Nome = x.Nome,
-                    }
-                ).ToList();
-
-                if (objSetores == null || !objSetores.Any())
-                {
-                    System.Diagnostics.Debug.WriteLine("Nenhum setor encontrado para lista flat.");
-                    return new List<ListaSetoresFlat>();
-                }
-
-                var resultado = objSetores.Select(setor =>
-                {
-
-                    int nivel = CalcularNivel(setor.SetorSolicitanteId, setor.SetorPaiId, objSetores);
-
-                    string indentacao = new string('—', nivel);
-
-                    return new ListaSetoresFlat
-                    {
-                        SetorSolicitanteId = setor.SetorSolicitanteId.ToString(),
-
-                        Nome = $"{indentacao} {setor.Nome}",
-                        Nivel = nivel
-                    };
-                })
-
-                .OrderBy(s => s.Nome)
-                .ToList();
-
-                return resultado;
+        try
+        {
+
+        var objSetores = _unitOfWork.ViewSetores.GetAllReduced(
+            selector: x => new SetorHierarquia
+            {
+                SetorSolicitanteId = x.SetorSolicitanteId ,
+                SetorPaiId = x.SetorPaiId ?? Guid.Empty ,
+                Nome = x.Nome ,
             }
-            catch (Exception ex)
-            {
-                System.Diagnostics.Debug.WriteLine(
-                    $"Erro em SetoresListFlat - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}"
-                );
-                return new List<ListaSetoresFlat>();
-            }
-        }
-
-        private int CalcularNivel(Guid setorId, Guid setorPaiId, List<SetorHierarquia> objSetores)
-        {
-            int nivel = 0;
-            Guid paiAtual = setorPaiId;
-            HashSet<Guid> visitados = new HashSet<Guid>();
-            int maxNivel = 50;
-
-            while (paiAtual != Guid.Empty && nivel < maxNivel)
-            {
-
-                if (visitados.Contains(paiAtual))
-                {
-                    System.Diagnostics.Debug.WriteLine($"⚠️ Loop circular detectado no setor {paiAtual}");
-                    break;
-                }
-
-                visitados.Add(paiAtual);
-                nivel++;
-
-                var pai = objSetores.FirstOrDefault(s => s.SetorSolicitanteId == paiAtual);
-
-                if (pai == null)
-                {
-                    System.Diagnostics.Debug.WriteLine($"⚠️ Setor pai {paiAtual} não encontrado");
-                    break;
-                }
-
-                paiAtual = pai.SetorPaiId;
-            }
-
-            if (nivel >= maxNivel)
-            {
-                System.Diagnostics.Debug.WriteLine($"⚠️ Nível máximo atingido para setor {setorId}");
-            }
-
-            return nivel;
+        ).ToList();
+
+        if (objSetores == null || !objSetores.Any())
+        {
+        System.Diagnostics.Debug.WriteLine("Nenhum setor encontrado para lista flat.");
+        return new List<ListaSetoresFlat>();
+        }
+
+        var resultado = objSetores.Select(setor =>
+        {
+        int nivel = CalcularNivel(setor.SetorSolicitanteId , setor.SetorPaiId , objSetores);
+        string indentacao = new string('—' , nivel);
+
+        return new ListaSetoresFlat
+        {
+            SetorSolicitanteId = setor.SetorSolicitanteId.ToString() ,
+            Nome = $"{indentacao} {setor.Nome}" ,
+            Nivel = nivel
+        };
+        })
+        .OrderBy(s => s.Nome)
+        .ToList();
+
+        return resultado;
+        }
+        catch (Exception ex)
+        {
+        System.Diagnostics.Debug.WriteLine(
+            $"Erro em SetoresListFlat - Linha: {new System.Diagnostics.StackTrace(ex , true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}"
+        );
+        return new List<ListaSetoresFlat>();
+        }
+        }
+
+        private int CalcularNivel(Guid setorId , Guid setorPaiId , List<SetorHierarquia> objSetores)
+        {
+        int nivel = 0;
+        Guid paiAtual = setorPaiId;
+        HashSet<Guid> visitados = new HashSet<Guid>();
+        int maxNivel = 50;
+
+        while (paiAtual != Guid.Empty && nivel < maxNivel)
+        {
+
+        if (visitados.Contains(paiAtual))
+        {
+        System.Diagnostics.Debug.WriteLine($"⚠️ Loop circular detectado no setor {paiAtual}");
+        break;
+        }
+
+        visitados.Add(paiAtual);
+        nivel++;
+
+        var pai = objSetores.FirstOrDefault(s => s.SetorSolicitanteId == paiAtual);
+
+        if (pai == null)
+        {
+        System.Diagnostics.Debug.WriteLine($"⚠️ Setor pai {paiAtual} não encontrado");
+        break;
+        }
+
+        paiAtual = pai.SetorPaiId;
+        }
+
+        if (nivel >= maxNivel)
+        {
+        System.Diagnostics.Debug.WriteLine($"⚠️ Nível máximo atingido para setor {setorId}");
+        }
+
+        return nivel;
         }
     }
 
@@ -712,15 +692,14 @@
 
         public ListaDias(IUnitOfWork unitOfWork)
         {
-            _unitOfWork = unitOfWork;
+        _unitOfWork = unitOfWork;
         }
 
         public List<ListaDias> DiasList()
         {
-            try
-            {
-
-                return new List<ListaDias>
+        try
+        {
+        return new List<ListaDias>
                 {
                     new ListaDias { DiaId = "Monday", Dia = "Segunda" },
                     new ListaDias { DiaId = "Tuesday", Dia = "Terça" },
@@ -730,12 +709,12 @@
                     new ListaDias { DiaId = "Saturday", Dia = "Sábado" },
                     new ListaDias { DiaId = "Sunday", Dia = "Domingo" }
                 };
-            }
-            catch (Exception ex)
-            {
-                System.Diagnostics.Debug.WriteLine($"Erro em DiasList - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
-                return new List<ListaDias>();
-            }
+        }
+        catch (Exception ex)
+        {
+        System.Diagnostics.Debug.WriteLine($"Erro em DiasList - Linha: {new System.Diagnostics.StackTrace(ex , true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
+        return new List<ListaDias>();
+        }
         }
     }
 
@@ -756,22 +735,21 @@
 
         public List<ListaPeriodos> PeriodosList()
         {
-            try
-            {
-
-                return new List<ListaPeriodos>
+        try
+        {
+        return new List<ListaPeriodos>
                 {
                     new ListaPeriodos { PeriodoId = "D", Periodo = "Diário" },
                     new ListaPeriodos { PeriodoId = "S", Periodo = "Semanal" },
                     new ListaPeriodos { PeriodoId = "Q", Periodo = "Quinzenal" },
                     new ListaPeriodos { PeriodoId = "M", Periodo = "Mensal" }
                 };
-            }
-            catch (Exception ex)
-            {
-                System.Diagnostics.Debug.WriteLine($"Erro em PeriodosList - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
-                return new List<ListaPeriodos>();
-            }
+        }
+        catch (Exception ex)
+        {
+        System.Diagnostics.Debug.WriteLine($"Erro em PeriodosList - Linha: {new System.Diagnostics.StackTrace(ex , true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
+        return new List<ListaPeriodos>();
+        }
         }
     }
 
@@ -792,20 +770,19 @@
 
         public List<ListaRecorrente> RecorrenteList()
         {
-            try
-            {
-
-                return new List<ListaRecorrente>
+        try
+        {
+        return new List<ListaRecorrente>
             {
                 new ListaRecorrente { RecorrenteId = "N", Descricao = "Não" },
                 new ListaRecorrente { RecorrenteId = "S", Descricao = "Sim" }
             };
-            }
-            catch (Exception ex)
-            {
-                System.Diagnostics.Debug.WriteLine($"Erro em RecorrenteList - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
-                return new List<ListaRecorrente>();
-            }
+        }
+        catch (Exception ex)
+        {
+        System.Diagnostics.Debug.WriteLine($"Erro em RecorrenteList - Linha: {new System.Diagnostics.StackTrace(ex , true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
+        return new List<ListaRecorrente>();
+        }
         }
     }
 
@@ -828,290 +805,27 @@
 
         public ListaStatus(IUnitOfWork unitOfWork)
         {
-            _unitOfWork = unitOfWork;
+        _unitOfWork = unitOfWork;
         }
 
         public List<ListaStatus> StatusList()
         {
-            try
-            {
-
-                return new List<ListaStatus>
+        try
+        {
+        return new List<ListaStatus>
                 {
                     new ListaStatus { Status = "Todas", StatusId = "Todas" },
                     new ListaStatus { Status = "Abertas", StatusId = "Aberta" },
                     new ListaStatus { Status = "Realizadas", StatusId = "Realizada" },
                     new ListaStatus { Status = "Canceladas", StatusId = "Cancelada" }
                 };
-            }
-            catch (Exception ex)
-            {
-                System.Diagnostics.Debug.WriteLine($"Erro em StatusList - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
-                return new List<ListaStatus>();
-            }
-        }
-    }
-
-    public class ListaTipoServico
-    {
-        public Guid TipoServicoId
-        {
-            get; set;
-        }
-        public string NomeServico
-        {
-            get; set;
-        }
-
-        private readonly IUnitOfWork _unitOfWork;
-
-        public ListaTipoServico()
-        {
-        }
-        public ListaTipoServico(IUnitOfWork unitOfWork)
-        {
-            _unitOfWork = unitOfWork;
-        }
-
-        public IEnumerable<ListaTipoServico> ServicoList()
-        {
-            try
-            {
-                var servicos = _unitOfWork.ViewEscalasCompletas.GetAllReduced(
-                    orderBy: ts => ts.OrderBy(ts => ts.NomeServico),
-                    selector: servicos => new ListaTipoServico
-                    {
-                        TipoServicoId = servicos.TipoServicoId ?? Guid.Empty,
-                        NomeServico = servicos.NomeServico,
-                    }
-                );
-
-                return servicos;
-            }
-            catch (Exception ex)
-            {
-                System.Diagnostics.Debug.WriteLine($"Erro em TipoServicoList - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
-                return Enumerable.Empty<ListaTipoServico>();
-            }
-        }
-
-    }
-
-    public class ListaTurno
-    {
-        public Guid TurnoId { get; set; }
-
-        public string NomeTurno { get; set; }
-
-        public TurnoEnum? TurnoTipo { get; set; }
-
-        private readonly IUnitOfWork _unitOfWork;
-
-        public ListaTurno()
-        {
-        }
-
-        public ListaTurno(IUnitOfWork unitOfWork)
-        {
-            _unitOfWork = unitOfWork;
-        }
-
-        public IEnumerable<ListaTurno> TurnoList()
-        {
-            try
-            {
-                var turnos = _unitOfWork.ViewEscalasCompletas.GetAllReduced(
-                    orderBy: tr => tr.OrderBy(tr => tr.NomeTurno),
-                    selector: turnos => new ListaTurno
-                    {
-                        TurnoId = turnos.TurnoId ?? Guid.Empty,
-                        NomeTurno = turnos.NomeTurno ?? "",
-                        TurnoTipo = MapearEnum(turnos.NomeTurno)
-                    }
-                );
-                return turnos;
-            }
-            catch (Exception ex)
-            {
-                System.Diagnostics.Debug.WriteLine($"Erro em TurnoList - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
-                return Enumerable.Empty<ListaTurno>();
-            }
-        }
-        private static TurnoEnum? MapearEnum(string? nomeTurno)
-        {
-            if (string.IsNullOrWhiteSpace(nomeTurno))
-                return null;
-
-            return nomeTurno switch
-            {
-                "Matutino" => TurnoEnum.Matutino,
-                "Vespertino" => TurnoEnum.Vespertino,
-                "Noturno" => TurnoEnum.Noturno,
-                _ => null
-            };
-        }
-
-        public string ObterNomeDisplay()
-        {
-            if (!TurnoTipo.HasValue)
-                return NomeTurno;
-
-            var field = typeof(TurnoEnum).GetField(TurnoTipo.Value.ToString());
-            var displayAttr = field?.GetCustomAttributes(typeof(DisplayAttribute), false)
-                .FirstOrDefault() as DisplayAttribute;
-
-            return displayAttr?.Name ?? NomeTurno;
-        }
-
-        public bool EhTurnoValido()
-        {
-            return TurnoTipo.HasValue;
-        }
-    }
-
-    public class ListaLotacao
-    {
-        public string? Lotacao
-        {
-            get; set;
-        }
-
-        public LotacaoEnum? TipoLotacao { get; set; }
-
-        private readonly IUnitOfWork _unitOfWork;
-
-        public ListaLotacao()
-        {
-        }
-
-        public ListaLotacao(IUnitOfWork unitOfWork)
-        {
-            _unitOfWork = unitOfWork;
-        }
-
-        public IEnumerable<ListaLotacao> LotacaoList()
-        {
-            try
-            {
-                var lotacoes = _unitOfWork.ViewEscalasCompletas.GetAllReduced(
-                    orderBy: lt => lt.OrderBy(lt => lt.Lotacao),
-                    selector: lotacoes => new ListaLotacao
-                    {
-                        Lotacao = lotacoes.Lotacao ?? "",
-                        TipoLotacao = MapearEnum(lotacoes.Lotacao),
-                    }
-                );
-
-                return lotacoes;
-            }
-            catch (Exception ex)
-            {
-                System.Diagnostics.Debug.WriteLine($"Erro em LotacaoList - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
-                return Enumerable.Empty<ListaLotacao>();
-            }
-        }
-
-        private static LotacaoEnum? MapearEnum(string? lotacao)
-        {
-            if (string.IsNullOrWhiteSpace(lotacao))
-                return null;
-
-            return lotacao switch
-            {
-                "Aeroporto" => LotacaoEnum.Aeroporto,
-                "PGR" => LotacaoEnum.PGR,
-                "Rodoviária" => LotacaoEnum.Rodoviaria,
-                "Setor de Obras" => LotacaoEnum.SetorObras,
-                "CEFOR" => LotacaoEnum.Cefor,
-                "Outros" => LotacaoEnum.Outros,
-                _ => null
-            };
-        }
-
-        public string ObterNomeDisplay()
-        {
-            if (!TipoLotacao.HasValue)
-                return Lotacao;
-
-            var field = typeof(LotacaoEnum).GetField(TipoLotacao.Value.ToString());
-            var displayAttr = field?.GetCustomAttributes(typeof(DisplayAttribute), false)
-                .FirstOrDefault() as DisplayAttribute;
-
-            return displayAttr?.Name ?? Lotacao;
-        }
-
-    }
-
-    public class ListaStatusMotorista
-    {
-        public string? StatusMotorista
-        {
-            get; set;
-        }
-
-        public StatusMotoristaEnum? TipoStatusM { get; set; }
-
-        private readonly IUnitOfWork _unitOfWork;
-
-        public ListaStatusMotorista()
-        {
-        }
-
-        public ListaStatusMotorista(IUnitOfWork unitOfWork)
-        {
-            _unitOfWork = unitOfWork;
-        }
-
-        public IEnumerable<ListaStatusMotorista> StatusMList()
-        {
-            try
-            {
-                var statusmotorista = _unitOfWork.ViewEscalasCompletas.GetAllReduced(
-                    orderBy: sm => sm.OrderBy(sm => sm.StatusMotorista),
-                    selector: statusmotorista => new ListaStatusMotorista
-                    {
-                        StatusMotorista = statusmotorista.StatusMotorista ?? "",
-                        TipoStatusM = MapearEnum(statusmotorista.StatusMotorista),
-                    }
-                );
-
-                return statusmotorista;
-            }
-            catch (Exception ex)
-            {
-                System.Diagnostics.Debug.WriteLine($"Erro em ListaStatusMotorista - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
-                return Enumerable.Empty<ListaStatusMotorista>();
-            }
-        }
-
-        private static StatusMotoristaEnum? MapearEnum(string? statusM)
-        {
-            if (string.IsNullOrWhiteSpace(statusM))
-                return null;
-
-            return statusM switch
-            {
-                "Disponível" => StatusMotoristaEnum.Disponivel,
-                "Em Viagem" => StatusMotoristaEnum.EmViagem,
-                "Em Serviço" => StatusMotoristaEnum.EmServico,
-                "Indisponível" => StatusMotoristaEnum.Indisponivel,
-                "Economildo" => StatusMotoristaEnum.Economildo,
-                _ => null
-            };
-        }
-
-        public string ObterNomeDisplay()
-        {
-            if (!TipoStatusM.HasValue)
-                return StatusMotorista;
-
-            var field = typeof(StatusMotoristaEnum).GetField(TipoStatusM.Value.ToString());
-            var displayAttr = field?.GetCustomAttributes(typeof(DisplayAttribute), false)
-                .FirstOrDefault() as DisplayAttribute;
-
-            return displayAttr?.Name ?? StatusMotorista;
-        }
-
+        }
+        catch (Exception ex)
+        {
+        System.Diagnostics.Debug.WriteLine($"Erro em StatusList - Linha: {new System.Diagnostics.StackTrace(ex , true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
+        return new List<ListaStatus>();
+        }
+        }
     }
 
 }
```
