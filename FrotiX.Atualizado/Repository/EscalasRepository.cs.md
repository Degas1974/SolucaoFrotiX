# Repository/EscalasRepository.cs

**Mudanca:** GRANDE | **+150** linhas | **-150** linhas

---

```diff
--- JANEIRO: Repository/EscalasRepository.cs
+++ ATUAL: Repository/EscalasRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -8,6 +7,7 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
 using Microsoft.AspNetCore.Mvc.Rendering;
+using Microsoft.EntityFrameworkCore;
 
 namespace FrotiX.Repository
 {
@@ -34,7 +34,7 @@
 
         public void Update(TipoServico tipoServico)
         {
-            var objFromDb = _db.Set<TipoServico>().AsTracking().FirstOrDefault(s => s.TipoServicoId == tipoServico.TipoServicoId);
+            var objFromDb = _db.Set<TipoServico>().FirstOrDefault(s => s.TipoServicoId == tipoServico.TipoServicoId);
             if (objFromDb != null)
             {
                 objFromDb.NomeServico = tipoServico.NomeServico;
@@ -80,7 +80,7 @@
 
         public void Update(Turno turno)
         {
-            var objFromDb = _db.Set<Turno>().AsTracking().FirstOrDefault(s => s.TurnoId == turno.TurnoId);
+            var objFromDb = _db.Set<Turno>().FirstOrDefault(s => s.TurnoId == turno.TurnoId);
             if (objFromDb != null)
             {
                 objFromDb.NomeTurno = turno.NomeTurno;
@@ -126,7 +126,7 @@
 
         public void Update(VAssociado vAssociado)
         {
-            var objFromDb = _db.Set<VAssociado>().AsTracking().FirstOrDefault(s => s.AssociacaoId == vAssociado.AssociacaoId);
+            var objFromDb = _db.Set<VAssociado>().FirstOrDefault(s => s.AssociacaoId == vAssociado.AssociacaoId);
             if (objFromDb != null)
             {
                 objFromDb.MotoristaId = vAssociado.MotoristaId;
@@ -192,7 +192,7 @@
 
         public void Update(EscalaDiaria escalaDiaria)
         {
-            var objFromDb = _db.Set<EscalaDiaria>().AsTracking().FirstOrDefault(s => s.EscalaDiaId == escalaDiaria.EscalaDiaId);
+            var objFromDb = _db.Set<EscalaDiaria>().FirstOrDefault(s => s.EscalaDiaId == escalaDiaria.EscalaDiaId);
             if (objFromDb != null)
             {
                 objFromDb.AssociacaoId = escalaDiaria.AssociacaoId;
@@ -218,50 +218,50 @@
             var dataEscala = data ?? DateTime.Today;
 
             var query = from ed in _db.Set<EscalaDiaria>()
-                        join va in _db.Set<VAssociado>() on ed.AssociacaoId equals va.AssociacaoId into vaLeft
-                        from va in vaLeft.DefaultIfEmpty()
-                        join m in _db.Set<Motorista>() on va.MotoristaId equals m.MotoristaId into mLeft
-                        from m in mLeft.DefaultIfEmpty()
-                        join v in _db.Set<ViewVeiculos>() on va.VeiculoId equals v.VeiculoId into vLeft
-                        from v in vLeft.DefaultIfEmpty()
-                        join ts in _db.Set<TipoServico>() on ed.TipoServicoId equals ts.TipoServicoId
-                        join t in _db.Set<Turno>() on ed.TurnoId equals t.TurnoId
-                        join r in _db.Set<Requisitante>() on ed.RequisitanteId equals r.RequisitanteId into rLeft
-                        from r in rLeft.DefaultIfEmpty()
-                        where ed.DataEscala == dataEscala && ed.Ativo
-                        orderby ed.HoraInicio
-                        select new ViewEscalasCompletas
-                        {
-                            EscalaDiaId = ed.EscalaDiaId,
-                            DataEscala = ed.DataEscala,
-                            HoraInicio = ed.HoraInicio.ToString(@"hh\:mm"),
-                            HoraFim = ed.HoraFim.ToString(@"hh\:mm"),
-                            HoraIntervaloInicio = ed.HoraIntervaloInicio.HasValue ?
-                                ed.HoraIntervaloInicio.Value.ToString(@"hh\:mm") : null,
-                            HoraIntervaloFim = ed.HoraIntervaloFim.HasValue ?
-                                ed.HoraIntervaloFim.Value.ToString(@"hh\:mm") : null,
-                            NumeroSaidas = ed.NumeroSaidas,
-                            StatusMotorista = ed.StatusMotorista,
-                            Lotacao = ed.Lotacao,
-                            Observacoes = ed.Observacoes,
-                            MotoristaId = m.MotoristaId,
-                            NomeMotorista = m.Nome,
-                            Ponto = m.Ponto,
-                            CPF = m.CPF,
-                            CNH = m.CNH,
-                            Celular01 = m.Celular01,
-                            Foto = m.Foto,
-                            VeiculoId = v.VeiculoId,
-                            VeiculoDescricao = v.Descricao,
-                            Placa = v.Placa,
-                            Modelo = v.MarcaModelo,
-                            TipoServicoId = ts.TipoServicoId,
-                            NomeServico = ts.NomeServico,
-                            TurnoId = t.TurnoId,
-                            NomeTurno = t.NomeTurno,
-                            RequisitanteId = r.RequisitanteId,
-                            NomeRequisitante = r.Nome
-                        };
+                       join va in _db.Set<VAssociado>() on ed.AssociacaoId equals va.AssociacaoId into vaLeft
+                       from va in vaLeft.DefaultIfEmpty()
+                       join m in _db.Set<Motorista>() on va.MotoristaId equals m.MotoristaId into mLeft
+                       from m in mLeft.DefaultIfEmpty()
+                       join v in _db.Set<ViewVeiculos>() on va.VeiculoId equals v.VeiculoId into vLeft
+                       from v in vLeft.DefaultIfEmpty()
+                       join ts in _db.Set<TipoServico>() on ed.TipoServicoId equals ts.TipoServicoId
+                       join t in _db.Set<Turno>() on ed.TurnoId equals t.TurnoId
+                       join r in _db.Set<Requisitante>() on ed.RequisitanteId equals r.RequisitanteId into rLeft
+                       from r in rLeft.DefaultIfEmpty()
+                       where ed.DataEscala == dataEscala && ed.Ativo
+                       orderby ed.HoraInicio
+                       select new ViewEscalasCompletas
+                       {
+                           EscalaDiaId = ed.EscalaDiaId,
+                           DataEscala = ed.DataEscala,
+                           HoraInicio = ed.HoraInicio.ToString(@"hh\:mm"),
+                           HoraFim = ed.HoraFim.ToString(@"hh\:mm"),
+                           HoraIntervaloInicio = ed.HoraIntervaloInicio.HasValue ?
+                               ed.HoraIntervaloInicio.Value.ToString(@"hh\:mm") : null,
+                           HoraIntervaloFim = ed.HoraIntervaloFim.HasValue ?
+                               ed.HoraIntervaloFim.Value.ToString(@"hh\:mm") : null,
+                           NumeroSaidas = ed.NumeroSaidas,
+                           StatusMotorista = ed.StatusMotorista,
+                           Lotacao = ed.Lotacao,
+                           Observacoes = ed.Observacoes,
+                           MotoristaId = m.MotoristaId,
+                           NomeMotorista = m.Nome,
+                           Ponto = m.Ponto,
+                           CPF = m.CPF,
+                           CNH = m.CNH,
+                           Celular01 = m.Celular01,
+                           Foto = m.Foto,
+                           VeiculoId = v.VeiculoId,
+                           VeiculoDescricao = v.Descricao,
+                           Placa = v.Placa,
+                           Modelo = v.MarcaModelo,
+                           TipoServicoId = ts.TipoServicoId,
+                           NomeServico = ts.NomeServico,
+                           TurnoId = t.TurnoId,
+                           NomeTurno = t.NomeTurno,
+                           RequisitanteId = r.RequisitanteId,
+                           NomeRequisitante = r.Nome
+                       };
 
             return await query.ToListAsync();
         }
@@ -269,49 +269,49 @@
         public async Task<ViewEscalasCompletas> GetEscalaCompletaByIdAsync(Guid id)
         {
             var query = from ed in _db.Set<EscalaDiaria>()
-                        join va in _db.Set<VAssociado>() on ed.AssociacaoId equals va.AssociacaoId into vaLeft
-                        from va in vaLeft.DefaultIfEmpty()
-                        join m in _db.Set<Motorista>() on va.MotoristaId equals m.MotoristaId into mLeft
-                        from m in mLeft.DefaultIfEmpty()
-                        join v in _db.Set<ViewVeiculos>() on va.VeiculoId equals v.VeiculoId into vLeft
-                        from v in vLeft.DefaultIfEmpty()
-                        join ts in _db.Set<TipoServico>() on ed.TipoServicoId equals ts.TipoServicoId
-                        join t in _db.Set<Turno>() on ed.TurnoId equals t.TurnoId
-                        join r in _db.Set<Requisitante>() on ed.RequisitanteId equals r.RequisitanteId into rLeft
-                        from r in rLeft.DefaultIfEmpty()
-                        where ed.EscalaDiaId == id && ed.Ativo
-                        select new ViewEscalasCompletas
-                        {
-                            EscalaDiaId = ed.EscalaDiaId,
-                            DataEscala = ed.DataEscala,
-                            HoraInicio = ed.HoraInicio.ToString(@"hh\:mm"),
-                            HoraFim = ed.HoraFim.ToString(@"hh\:mm"),
-                            HoraIntervaloInicio = ed.HoraIntervaloInicio.HasValue ?
-                                ed.HoraIntervaloInicio.Value.ToString(@"hh\:mm") : null,
-                            HoraIntervaloFim = ed.HoraIntervaloFim.HasValue ?
-                                ed.HoraIntervaloFim.Value.ToString(@"hh\:mm") : null,
-                            NumeroSaidas = ed.NumeroSaidas,
-                            StatusMotorista = ed.StatusMotorista,
-                            Lotacao = ed.Lotacao,
-                            Observacoes = ed.Observacoes,
-                            MotoristaId = m.MotoristaId,
-                            NomeMotorista = m.Nome,
-                            Ponto = m.Ponto,
-                            CPF = m.CPF,
-                            CNH = m.CNH,
-                            Celular01 = m.Celular01,
-                            Foto = m.Foto,
-                            VeiculoId = v.VeiculoId,
-                            VeiculoDescricao = v.Descricao,
-                            Placa = v.Placa,
-                            Modelo = v.MarcaModelo,
-                            TipoServicoId = ts.TipoServicoId,
-                            NomeServico = ts.NomeServico,
-                            TurnoId = t.TurnoId,
-                            NomeTurno = t.NomeTurno,
-                            RequisitanteId = r.RequisitanteId,
-                            NomeRequisitante = r.Nome
-                        };
+                       join va in _db.Set<VAssociado>() on ed.AssociacaoId equals va.AssociacaoId into vaLeft
+                       from va in vaLeft.DefaultIfEmpty()
+                       join m in _db.Set<Motorista>() on va.MotoristaId equals m.MotoristaId into mLeft
+                       from m in mLeft.DefaultIfEmpty()
+                       join v in _db.Set<ViewVeiculos>() on va.VeiculoId equals v.VeiculoId into vLeft
+                       from v in vLeft.DefaultIfEmpty()
+                       join ts in _db.Set<TipoServico>() on ed.TipoServicoId equals ts.TipoServicoId
+                       join t in _db.Set<Turno>() on ed.TurnoId equals t.TurnoId
+                       join r in _db.Set<Requisitante>() on ed.RequisitanteId equals r.RequisitanteId into rLeft
+                       from r in rLeft.DefaultIfEmpty()
+                       where ed.EscalaDiaId == id && ed.Ativo
+                       select new ViewEscalasCompletas
+                       {
+                           EscalaDiaId = ed.EscalaDiaId,
+                           DataEscala = ed.DataEscala,
+                           HoraInicio = ed.HoraInicio.ToString(@"hh\:mm"),
+                           HoraFim = ed.HoraFim.ToString(@"hh\:mm"),
+                           HoraIntervaloInicio = ed.HoraIntervaloInicio.HasValue ?
+                               ed.HoraIntervaloInicio.Value.ToString(@"hh\:mm") : null,
+                           HoraIntervaloFim = ed.HoraIntervaloFim.HasValue ?
+                               ed.HoraIntervaloFim.Value.ToString(@"hh\:mm") : null,
+                           NumeroSaidas = ed.NumeroSaidas,
+                           StatusMotorista = ed.StatusMotorista,
+                           Lotacao = ed.Lotacao,
+                           Observacoes = ed.Observacoes,
+                           MotoristaId = m.MotoristaId,
+                           NomeMotorista = m.Nome,
+                           Ponto = m.Ponto,
+                           CPF = m.CPF,
+                           CNH = m.CNH,
+                           Celular01 = m.Celular01,
+                           Foto = m.Foto,
+                           VeiculoId = v.VeiculoId,
+                           VeiculoDescricao = v.Descricao,
+                           Placa = v.Placa,
+                           Modelo = v.MarcaModelo,
+                           TipoServicoId = ts.TipoServicoId,
+                           NomeServico = ts.NomeServico,
+                           TurnoId = t.TurnoId,
+                           NomeTurno = t.NomeTurno,
+                           RequisitanteId = r.RequisitanteId,
+                           NomeRequisitante = r.Nome
+                       };
 
             return await query.FirstOrDefaultAsync();
         }
@@ -319,18 +319,18 @@
         public async Task<List<ViewEscalasCompletas>> GetEscalasPorFiltroAsync(FiltroEscalaViewModel filtro)
         {
             var query = from ed in _db.Set<EscalaDiaria>()
-                        join va in _db.Set<VAssociado>() on ed.AssociacaoId equals va.AssociacaoId into vaLeft
-                        from va in vaLeft.DefaultIfEmpty()
-                        join m in _db.Set<Motorista>() on va.MotoristaId equals m.MotoristaId into mLeft
-                        from m in mLeft.DefaultIfEmpty()
-                        join v in _db.Set<ViewVeiculos>() on va.VeiculoId equals v.VeiculoId into vLeft
-                        from v in vLeft.DefaultIfEmpty()
-                        join ts in _db.Set<TipoServico>() on ed.TipoServicoId equals ts.TipoServicoId
-                        join t in _db.Set<Turno>() on ed.TurnoId equals t.TurnoId
-                        join r in _db.Set<Requisitante>() on ed.RequisitanteId equals r.RequisitanteId into rLeft
-                        from r in rLeft.DefaultIfEmpty()
-                        where ed.Ativo
-                        select new { ed, va, m, v, ts, t, r };
+                       join va in _db.Set<VAssociado>() on ed.AssociacaoId equals va.AssociacaoId into vaLeft
+                       from va in vaLeft.DefaultIfEmpty()
+                       join m in _db.Set<Motorista>() on va.MotoristaId equals m.MotoristaId into mLeft
+                       from m in mLeft.DefaultIfEmpty()
+                       join v in _db.Set<ViewVeiculos>() on va.VeiculoId equals v.VeiculoId into vLeft
+                       from v in vLeft.DefaultIfEmpty()
+                       join ts in _db.Set<TipoServico>() on ed.TipoServicoId equals ts.TipoServicoId
+                       join t in _db.Set<Turno>() on ed.TurnoId equals t.TurnoId
+                       join r in _db.Set<Requisitante>() on ed.RequisitanteId equals r.RequisitanteId into rLeft
+                       from r in rLeft.DefaultIfEmpty()
+                       where ed.Ativo
+                       select new { ed, va, m, v, ts, t, r };
 
             if (filtro.DataFiltro.HasValue)
                 query = query.Where(x => x.ed.DataEscala == filtro.DataFiltro.Value);
@@ -404,21 +404,21 @@
                 .ToListAsync();
 
             var query = from ed in _db.Set<EscalaDiaria>()
-                        join va in _db.Set<VAssociado>() on ed.AssociacaoId equals va.AssociacaoId
-                        join m in _db.Set<Motorista>() on va.MotoristaId equals m.MotoristaId
-                        join v in _db.Set<ViewVeiculos>() on va.VeiculoId equals v.VeiculoId into vLeft
-                        from v in vLeft.DefaultIfEmpty()
-                        where ed.DataEscala == hoje &&
-                              ed.Ativo &&
-                              ed.StatusMotorista == "Disponível" &&
-                              ed.HoraInicio <= agora &&
-                              ed.HoraFim >= agora
-                        select new
-                        {
-                            ed,
-                            m,
-                            v
-                        };
+                       join va in _db.Set<VAssociado>() on ed.AssociacaoId equals va.AssociacaoId
+                       join m in _db.Set<Motorista>() on va.MotoristaId equals m.MotoristaId
+                       join v in _db.Set<ViewVeiculos>() on va.VeiculoId equals v.VeiculoId into vLeft
+                       from v in vLeft.DefaultIfEmpty()
+                       where ed.DataEscala == hoje &&
+                             ed.Ativo &&
+                             ed.StatusMotorista == "Disponível" &&
+                             ed.HoraInicio <= agora &&
+                             ed.HoraFim >= agora
+                       select new
+                       {
+                           ed,
+                           m,
+                           v
+                       };
 
             var escalas = await query.ToListAsync();
 
@@ -451,31 +451,31 @@
             var hoje = DateTime.Today;
 
             var query = from m in _db.Set<Motorista>()
-                        join va in _db.Set<VAssociado>() on m.MotoristaId equals va.MotoristaId into vaLeft
-                        from va in vaLeft.Where(x => x.Ativo).DefaultIfEmpty()
-                        join v in _db.Set<ViewVeiculos>() on va.VeiculoId equals v.VeiculoId into vLeft
-                        from v in vLeft.DefaultIfEmpty()
-                        join ed in _db.Set<EscalaDiaria>() on va.AssociacaoId equals ed.AssociacaoId into edLeft
-                        from ed in edLeft.Where(x => x.DataEscala == hoje && x.Ativo).DefaultIfEmpty()
-                        join fr in _db.Set<FolgaRecesso>() on m.MotoristaId equals fr.MotoristaId into frLeft
-                        from fr in frLeft.Where(x => hoje >= x.DataInicio && hoje <= x.DataFim && x.Ativo).DefaultIfEmpty()
-                        join f in _db.Set<Ferias>() on m.MotoristaId equals f.MotoristaId into fLeft
-                        from f in fLeft.Where(x => hoje >= x.DataInicio && hoje <= x.DataFim && x.Ativo).DefaultIfEmpty()
-                        where m.Status == true
-                        select new ViewStatusMotoristas
-                        {
-                            MotoristaId = m.MotoristaId,
-                            Nome = m.Nome,
-                            Ponto = m.Ponto,
-                            StatusAtual = f != null ? "Férias" :
-                                        fr != null ? fr.Tipo :
-                                        ed != null ? ed.StatusMotorista :
-                                        "Sem Escala",
-                            DataEscala = ed.DataEscala,
-                            NumeroSaidas = ed != null ? ed.NumeroSaidas : 0,
-                            Placa = v.Placa,
-                            Veiculo = v.Descricao
-                        };
+                       join va in _db.Set<VAssociado>() on m.MotoristaId equals va.MotoristaId into vaLeft
+                       from va in vaLeft.Where(x => x.Ativo).DefaultIfEmpty()
+                       join v in _db.Set<ViewVeiculos>() on va.VeiculoId equals v.VeiculoId into vLeft
+                       from v in vLeft.DefaultIfEmpty()
+                       join ed in _db.Set<EscalaDiaria>() on va.AssociacaoId equals ed.AssociacaoId into edLeft
+                       from ed in edLeft.Where(x => x.DataEscala == hoje && x.Ativo).DefaultIfEmpty()
+                       join fr in _db.Set<FolgaRecesso>() on m.MotoristaId equals fr.MotoristaId into frLeft
+                       from fr in frLeft.Where(x => hoje >= x.DataInicio && hoje <= x.DataFim && x.Ativo).DefaultIfEmpty()
+                       join f in _db.Set<Ferias>() on m.MotoristaId equals f.MotoristaId into fLeft
+                       from f in fLeft.Where(x => hoje >= x.DataInicio && hoje <= x.DataFim && x.Ativo).DefaultIfEmpty()
+                       where m.Status == true
+                       select new ViewStatusMotoristas
+                       {
+                           MotoristaId = m.MotoristaId,
+                           Nome = m.Nome,
+                           Ponto = m.Ponto,
+                           StatusAtual = f != null ? "Férias" :
+                                       fr != null ? fr.Tipo :
+                                       ed != null ? ed.StatusMotorista :
+                                       "Sem Escala",
+                           DataEscala = ed.DataEscala,
+                           NumeroSaidas = ed != null ? ed.NumeroSaidas : 0,
+                           Placa = v.Placa,
+                           Veiculo = v.Descricao
+                       };
 
             return await query.ToListAsync();
         }
@@ -484,8 +484,8 @@
         {
             var dataEscala = data ?? DateTime.Today;
 
-            var escala = await _db.Set<EscalaDiaria>().AsTracking()
-                .AsTracking().FirstOrDefaultAsync(ed =>
+            var escala = await _db.Set<EscalaDiaria>()
+                .FirstOrDefaultAsync(ed =>
                     ed.Associacao.MotoristaId == motoristaId &&
                     ed.DataEscala == dataEscala &&
                     ed.Ativo);
@@ -614,12 +614,13 @@
     public class FolgaRecessoRepository : Repository<FrotiX.Models.FolgaRecesso>, IFolgaRecessoRepository
     {
         private readonly FrotiXDbContext _db;
+
         public FolgaRecessoRepository(FrotiXDbContext db) : base(db) => _db = db;
 
         public void Update(FrotiX.Models.FolgaRecesso folgaRecesso)
         {
             var set = _db.Set<FrotiX.Models.FolgaRecesso>();
-            var obj = set.AsTracking().FirstOrDefault(x => x.FolgaId == folgaRecesso.FolgaId);
+            var obj = set.FirstOrDefault(x => x.FolgaId == folgaRecesso.FolgaId);
             if (obj != null)
             {
                 obj.MotoristaId = folgaRecesso.MotoristaId;
@@ -656,12 +657,13 @@
     public class FeriasRepository : Repository<FrotiX.Models.Ferias>, IFeriasRepository
     {
         private readonly FrotiXDbContext _db;
+
         public FeriasRepository(FrotiXDbContext db) : base(db) => _db = db;
 
         public void Update(FrotiX.Models.Ferias ferias)
         {
             var set = _db.Set<FrotiX.Models.Ferias>();
-            var obj = set.AsTracking().FirstOrDefault(x => x.FeriasId == ferias.FeriasId);
+            var obj = set.FirstOrDefault(x => x.FeriasId == ferias.FeriasId);
             if (obj != null)
             {
                 obj.MotoristaId = ferias.MotoristaId;
@@ -702,12 +704,13 @@
     public class CoberturaFolgaRepository : Repository<FrotiX.Models.CoberturaFolga>, ICoberturaFolgaRepository
     {
         private readonly FrotiXDbContext _db;
+
         public CoberturaFolgaRepository(FrotiXDbContext db) : base(db) => _db = db;
 
         public void Update(FrotiX.Models.CoberturaFolga coberturaFolga)
         {
             var set = _db.Set<FrotiX.Models.CoberturaFolga>();
-            var obj = set.AsTracking().FirstOrDefault(x => x.CoberturaId == coberturaFolga.CoberturaId);
+            var obj = set.FirstOrDefault(x => x.CoberturaId == coberturaFolga.CoberturaId);
             if (obj != null)
             {
                 obj.MotoristaFolgaId = coberturaFolga.MotoristaFolgaId;
@@ -748,12 +751,13 @@
     public class ObservacoesEscalaRepository : Repository<FrotiX.Models.ObservacoesEscala>, IObservacoesEscalaRepository
     {
         private readonly FrotiXDbContext _db;
+
         public ObservacoesEscalaRepository(FrotiXDbContext db) : base(db) => _db = db;
 
         public void Update(FrotiX.Models.ObservacoesEscala observacaoEscala)
         {
             var set = _db.Set<FrotiX.Models.ObservacoesEscala>();
-            var obj = set.AsTracking().FirstOrDefault(x => x.ObservacaoId == observacaoEscala.ObservacaoId);
+            var obj = set.FirstOrDefault(x => x.ObservacaoId == observacaoEscala.ObservacaoId);
             if (obj != null)
             {
                 obj.DataEscala = observacaoEscala.DataEscala;
@@ -785,6 +789,7 @@
     public class ViewEscalasCompletasRepository : Repository<FrotiX.Models.ViewEscalasCompletas>, IViewEscalasCompletasRepository
     {
         private readonly FrotiXDbContext _db;
+
         public ViewEscalasCompletasRepository(FrotiXDbContext db) : base(db) => _db = db;
 
         public Task<List<FrotiX.Models.ViewEscalasCompletas>> GetAllAsync()
@@ -813,6 +818,7 @@
     public class ViewMotoristasVezRepository : Repository<FrotiX.Models.ViewMotoristasVez>, IViewMotoristasVezRepository
     {
         private readonly FrotiXDbContext _db;
+
         public ViewMotoristasVezRepository(FrotiXDbContext db) : base(db) => _db = db;
 
         public Task<List<FrotiX.Models.ViewMotoristasVez>> GetTopMotoristasAsync(int quantidade = 5)
@@ -826,6 +832,7 @@
     public class ViewStatusMotoristasRepository : Repository<FrotiX.Models.ViewStatusMotoristas>, IViewStatusMotoristasRepository
     {
         private readonly FrotiXDbContext _db;
+
         public ViewStatusMotoristasRepository(FrotiXDbContext db) : base(db) => _db = db;
 
         public Task<List<FrotiX.Models.ViewStatusMotoristas>> GetStatusAtualizadoAsync()
```
