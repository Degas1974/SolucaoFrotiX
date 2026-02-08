# Controllers/DashboardEventosController.cs

**Mudanca:** GRANDE | **+152** linhas | **-191** linhas

---

```diff
--- JANEIRO: Controllers/DashboardEventosController.cs
+++ ATUAL: Controllers/DashboardEventosController.cs
@@ -7,68 +7,48 @@
 using System;
 using System.Collections.Generic;
 using System.Linq;
+using System.Text.Json;
 using System.Threading.Tasks;
 using FrotiX.Helpers;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers
 {
-
     [Authorize]
     public partial class DashboardEventosController : Controller
     {
         private readonly FrotiXDbContext _context;
-        private readonly UserManager<IdentityUser> _user;
-        private readonly ILogService _log;
-
-        public DashboardEventosController(FrotiXDbContext context, UserManager<IdentityUser> user, ILogService log)
-        {
-            try
-            {
-                _context = context;
-                _user = user;
-                _log = log;
-            }
-            catch (Exception error)
-            {
-                Alerta.TratamentoErroComLinha("DashboardEventosController.cs", "DashboardEventosController", error);
-            }
+        private readonly UserManager<IdentityUser> _userManager;
+
+        public DashboardEventosController(FrotiXDbContext context , UserManager<IdentityUser> userManager)
+        {
+            _context = context;
+            _userManager = userManager;
         }
 
         [HttpGet]
         [Route("DashboardEventos")]
         public IActionResult Index()
         {
-            try
-            {
-                return View("/Pages/Eventos/DashboardEventos.cshtml");
-            }
-            catch (Exception error)
-            {
-                _log.Error(error.Message, error, "DashboardEventosController.cs", "Index");
-                Alerta.TratamentoErroComLinha("DashboardEventosController.cs", "Index", error);
-                return View("Error");
-            }
+            return View("/Pages/Eventos/DashboardEventos.cshtml");
         }
 
         [HttpGet]
         [Route("api/DashboardEventos/ObterEstatisticasGerais")]
-        public async Task<IActionResult> ObterEstatisticasGerais(DateTime? dataInicio, DateTime? dataFim)
-        {
-            try
-            {
-
-                if (!dataInicio.HasValue || !dataFim.HasValue)
-                {
-                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
-                    dataInicio = dataFim.Value.AddDays(-30);
-                }
-
-                var eventos = await _context.Evento
-                    .AsNoTracking()
+        public async Task<IActionResult> ObterEstatisticasGerais(DateTime? DataInicial , DateTime? dataFim)
+        {
+            try
+            {
+
+                if (!DataInicial.HasValue || !dataFim.HasValue)
+                {
+                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
+                    DataInicial = dataFim.Value.AddDays(-30);
+                }
+
+                var eventos = await _context.Evento
                     .Include(e => e.SetorSolicitante)
                     .Include(e => e.Requisitante)
-                    .Where(e => e.DataInicial >= dataInicio && e.DataInicial <= dataFim)
+                    .Where(e => e.DataInicial >= DataInicial && e.DataInicial <= dataFim)
                     .ToListAsync();
 
                 var totalEventos = eventos.Count;
@@ -80,12 +60,12 @@
                 var totalParticipantes = eventos.Sum(e => e.QtdParticipantes ?? 0);
                 var mediaParticipantesPorEvento = totalEventos > 0 ? (double)totalParticipantes / totalEventos : 0;
 
-                var diasPeriodo = (dataFim.Value - dataInicio.Value).Days;
-                var dataInicioAnterior = dataInicio.Value.AddDays(-(diasPeriodo + 1));
-                var dataFimAnterior = dataInicio.Value.AddSeconds(-1);
+                var diasPeriodo = (dataFim.Value - DataInicial.Value).Days;
+                var DataInicialAnterior = DataInicial.Value.AddDays(-(diasPeriodo + 1));
+                var dataFimAnterior = DataInicial.Value.AddSeconds(-1);
 
                 var eventosAnteriores = await _context.Evento
-                    .Where(e => e.DataInicial >= dataInicioAnterior && e.DataInicial <= dataFimAnterior)
+                    .Where(e => e.DataInicial >= DataInicialAnterior && e.DataInicial <= dataFimAnterior)
                     .ToListAsync();
 
                 var totalEventosAnteriores = eventosAnteriores.Count;
@@ -93,263 +73,251 @@
 
                 return Json(new
                 {
-                    success = true,
-                    totalEventos,
-                    eventosAtivos,
-                    eventosConcluidos,
-                    eventosCancelados,
-                    eventosAgendados,
-                    totalParticipantes,
-                    mediaParticipantesPorEvento = Math.Round(mediaParticipantesPorEvento, 1),
+                    success = true ,
+
+                    totalEventos ,
+                    eventosAtivos ,
+                    eventosConcluidos ,
+                    eventosCancelados ,
+                    eventosAgendados ,
+                    totalParticipantes ,
+                    mediaParticipantesPorEvento = Math.Round(mediaParticipantesPorEvento , 1) ,
+
                     periodoAnterior = new
                     {
-                        totalEventos = totalEventosAnteriores,
+                        totalEventos = totalEventosAnteriores ,
                         totalParticipantes = totalParticipantesAnteriores
                     }
                 });
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardEventosController.cs", "ObterEstatisticasGerais");
-                Alerta.TratamentoErroComLinha("DashboardEventosController.cs", "ObterEstatisticasGerais", ex);
-                return Json(new { success = false, message = ex.Message });
+                return Json(new { success = false , message = ex.Message });
             }
         }
 
         [HttpGet]
         [Route("api/DashboardEventos/ObterEventosPorStatus")]
-        public async Task<IActionResult> ObterEventosPorStatus(DateTime? dataInicio, DateTime? dataFim)
-        {
-            try
-            {
-                if (!dataInicio.HasValue || !dataFim.HasValue)
-                {
-                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
-                    dataInicio = dataFim.Value.AddDays(-30);
-                }
-
-                var eventos = await _context.Evento
-                    .AsNoTracking()
-                    .Where(e => e.DataInicial >= dataInicio && e.DataInicial <= dataFim)
+        public async Task<IActionResult> ObterEventosPorStatus(DateTime? DataInicial , DateTime? dataFim)
+        {
+            try
+            {
+
+                if (!DataInicial.HasValue || !dataFim.HasValue)
+                {
+                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
+                    DataInicial = dataFim.Value.AddDays(-30);
+                }
+
+                var eventos = await _context.Evento
+                    .Where(e => e.DataInicial >= DataInicial && e.DataInicial <= dataFim)
                     .GroupBy(e => e.Status)
                     .Select(g => new
                     {
-                        status = g.Key ?? "Sem Status",
-                        quantidade = g.Count(),
+                        status = g.Key ?? "Sem Status" ,
+                        quantidade = g.Count() ,
                         participantes = g.Sum(e => e.QtdParticipantes ?? 0)
                     })
                     .OrderByDescending(x => x.quantidade)
                     .ToListAsync();
 
-                return Json(new { success = true, dados = eventos });
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardEventosController.cs", "ObterEventosPorStatus");
-                Alerta.TratamentoErroComLinha("DashboardEventosController.cs", "ObterEventosPorStatus", ex);
-                return Json(new { success = false, message = ex.Message });
+                return Json(new { success = true , dados = eventos });
+            }
+            catch (Exception ex)
+            {
+                return Json(new { success = false , message = ex.Message });
             }
         }
 
         [HttpGet]
         [Route("api/DashboardEventos/ObterEventosPorSetor")]
-        public async Task<IActionResult> ObterEventosPorSetor(DateTime? dataInicio, DateTime? dataFim)
-        {
-            try
-            {
-                if (!dataInicio.HasValue || !dataFim.HasValue)
-                {
-                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
-                    dataInicio = dataFim.Value.AddDays(-30);
-                }
-
-                var eventos = await _context.Evento
-                    .AsNoTracking()
+        public async Task<IActionResult> ObterEventosPorSetor(DateTime? DataInicial , DateTime? dataFim)
+        {
+            try
+            {
+
+                if (!DataInicial.HasValue || !dataFim.HasValue)
+                {
+                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
+                    DataInicial = dataFim.Value.AddDays(-30);
+                }
+
+                var eventos = await _context.Evento
                     .Include(e => e.SetorSolicitante)
-                    .Where(e => e.DataInicial >= dataInicio && e.DataInicial <= dataFim)
+                    .Where(e => e.DataInicial >= DataInicial && e.DataInicial <= dataFim)
                     .ToListAsync();
 
                 var eventosPorSetor = eventos
                     .GroupBy(e => e.SetorSolicitante != null ? e.SetorSolicitante.Nome : "Sem Setor")
                     .Select(g => new
                     {
-                        setor = g.Key,
-                        quantidade = g.Count(),
-                        participantes = g.Sum(e => e.QtdParticipantes ?? 0),
-                        concluidos = g.Count(e => e.Status == "Concluído" || e.Status == "Finalizado"),
-                        taxaConclusao = g.Count() > 0 ? Math.Round((double)g.Count(e => e.Status == "Concluído" || e.Status == "Finalizado") / g.Count() * 100, 1) : 0
+                        setor = g.Key ,
+                        quantidade = g.Count() ,
+                        participantes = g.Sum(e => e.QtdParticipantes ?? 0) ,
+                        concluidos = g.Count(e => e.Status == "Concluído" || e.Status == "Finalizado") ,
+                        taxaConclusao = g.Count() > 0 ? Math.Round((double)g.Count(e => e.Status == "Concluído" || e.Status == "Finalizado") / g.Count() * 100 , 1) : 0
                     })
                     .OrderByDescending(x => x.quantidade)
                     .Take(10)
                     .ToList();
 
-                return Json(new { success = true, dados = eventosPorSetor });
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardEventosController.cs", "ObterEventosPorSetor");
-                Alerta.TratamentoErroComLinha("DashboardEventosController.cs", "ObterEventosPorSetor", ex);
-                return Json(new { success = false, message = ex.Message });
+                return Json(new { success = true , dados = eventosPorSetor });
+            }
+            catch (Exception ex)
+            {
+                return Json(new { success = false , message = ex.Message });
             }
         }
 
         [HttpGet]
         [Route("api/DashboardEventos/ObterEventosPorRequisitante")]
-        public async Task<IActionResult> ObterEventosPorRequisitante(DateTime? dataInicio, DateTime? dataFim)
-        {
-            try
-            {
-                if (!dataInicio.HasValue || !dataFim.HasValue)
-                {
-                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
-                    dataInicio = dataFim.Value.AddDays(-30);
-                }
-
-                var eventos = await _context.Evento
-                    .AsNoTracking()
+        public async Task<IActionResult> ObterEventosPorRequisitante(DateTime? DataInicial , DateTime? dataFim)
+        {
+            try
+            {
+
+                if (!DataInicial.HasValue || !dataFim.HasValue)
+                {
+                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
+                    DataInicial = dataFim.Value.AddDays(-30);
+                }
+
+                var eventos = await _context.Evento
                     .Include(e => e.Requisitante)
-                    .Where(e => e.DataInicial >= dataInicio && e.DataInicial <= dataFim)
+                    .Where(e => e.DataInicial >= DataInicial && e.DataInicial <= dataFim)
                     .ToListAsync();
 
                 var eventosPorRequisitante = eventos
                     .GroupBy(e => e.Requisitante != null ? e.Requisitante.Nome : "Sem Requisitante")
                     .Select(g => new
                     {
-                        requisitante = g.Key,
-                        quantidade = g.Count(),
+                        requisitante = g.Key ,
+                        quantidade = g.Count() ,
                         participantes = g.Sum(e => e.QtdParticipantes ?? 0)
                     })
                     .OrderByDescending(x => x.quantidade)
                     .Take(10)
                     .ToList();
 
-                return Json(new { success = true, dados = eventosPorRequisitante });
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardEventosController.cs", "ObterEventosPorRequisitante");
-                Alerta.TratamentoErroComLinha("DashboardEventosController.cs", "ObterEventosPorRequisitante", ex);
-                return Json(new { success = false, message = ex.Message });
+                return Json(new { success = true , dados = eventosPorRequisitante });
+            }
+            catch (Exception ex)
+            {
+                return Json(new { success = false , message = ex.Message });
             }
         }
 
         [HttpGet]
         [Route("api/DashboardEventos/ObterEventosPorMes")]
-        public async Task<IActionResult> ObterEventosPorMes(DateTime? dataInicio, DateTime? dataFim)
-        {
-            try
-            {
-                if (!dataInicio.HasValue || !dataFim.HasValue)
-                {
-                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
-                    dataInicio = dataFim.Value.AddMonths(-12);
-                }
-
-                var eventos = await _context.Evento
-                    .AsNoTracking()
-                    .Where(e => e.DataInicial >= dataInicio && e.DataInicial <= dataFim)
+        public async Task<IActionResult> ObterEventosPorMes(DateTime? DataInicial , DateTime? dataFim)
+        {
+            try
+            {
+
+                if (!DataInicial.HasValue || !dataFim.HasValue)
+                {
+                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
+                    DataInicial = dataFim.Value.AddMonths(-12);
+                }
+
+                var eventos = await _context.Evento
+                    .Where(e => e.DataInicial >= DataInicial && e.DataInicial <= dataFim)
                     .ToListAsync();
 
                 var eventosPorMes = eventos
                     .Where(e => e.DataInicial.HasValue)
-                    .GroupBy(e => new { Ano = e.DataInicial.Value.Year, Mes = e.DataInicial.Value.Month })
-                    .Select(g => new
-                    {
-                        mes = $"{g.Key.Ano}-{g.Key.Mes:D2}",
-                        mesNome = new DateTime(g.Key.Ano, g.Key.Mes, 1).ToString("MM/yyyy"),
-                        quantidade = g.Count(),
+                    .GroupBy(e => new { Ano = e.DataInicial.Value.Year , Mes = e.DataInicial.Value.Month })
+                    .Select(g => new
+                    {
+                        mes = $"{g.Key.Ano}-{g.Key.Mes:D2}" ,
+                        mesNome = new DateTime(g.Key.Ano , g.Key.Mes , 1).ToString("MM/yyyy") ,
+                        quantidade = g.Count() ,
                         participantes = g.Sum(e => e.QtdParticipantes ?? 0)
                     })
                     .OrderBy(x => x.mes)
                     .ToList();
 
-                return Json(new { success = true, dados = eventosPorMes });
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardEventosController.cs", "ObterEventosPorMes");
-                Alerta.TratamentoErroComLinha("DashboardEventosController.cs", "ObterEventosPorMes", ex);
-                return Json(new { success = false, message = ex.Message });
+                return Json(new { success = true , dados = eventosPorMes });
+            }
+            catch (Exception ex)
+            {
+                return Json(new { success = false , message = ex.Message });
             }
         }
 
         [HttpGet]
         [Route("api/DashboardEventos/ObterTop10EventosMaiores")]
-        public async Task<IActionResult> ObterTop10EventosMaiores(DateTime? dataInicio, DateTime? dataFim)
-        {
-            try
-            {
-                if (!dataInicio.HasValue || !dataFim.HasValue)
-                {
-                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
-                    dataInicio = dataFim.Value.AddDays(-30);
-                }
-
-                var eventos = await _context.Evento
-                    .AsNoTracking()
+        public async Task<IActionResult> ObterTop10EventosMaiores(DateTime? DataInicial , DateTime? dataFim)
+        {
+            try
+            {
+
+                if (!DataInicial.HasValue || !dataFim.HasValue)
+                {
+                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
+                    DataInicial = dataFim.Value.AddDays(-30);
+                }
+
+                var eventos = await _context.Evento
                     .Include(e => e.SetorSolicitante)
                     .Include(e => e.Requisitante)
-                    .Where(e => e.DataInicial >= dataInicio && e.DataInicial <= dataFim)
+                    .Where(e => e.DataInicial >= DataInicial && e.DataInicial <= dataFim)
                     .OrderByDescending(e => e.QtdParticipantes)
                     .Take(10)
                     .Select(e => new
                     {
-                        e.EventoId,
-                        e.Nome,
-                        DataInicial = e.DataInicial.HasValue ? e.DataInicial.Value.ToString("dd/MM/yyyy HH:mm") : "Não definido",
-                        DataFinal = e.DataFinal.HasValue ? e.DataFinal.Value.ToString("dd/MM/yyyy HH:mm") : "Não definido",
-                        participantes = e.QtdParticipantes ?? 0,
-                        setor = e.SetorSolicitante != null ? e.SetorSolicitante.Nome : "Sem Setor",
-                        requisitante = e.Requisitante != null ? e.Requisitante.Nome : "Sem Requisitante",
+                        e.EventoId ,
+                        e.Nome ,
+                        DataInicial = e.DataInicial.HasValue ? e.DataInicial.Value.ToString("dd/MM/yyyy HH:mm") : "Não definido" ,
+                        dataFim = e.DataFinal.HasValue ? e.DataFinal.Value.ToString("dd/MM/yyyy HH:mm") : "Não definido" ,
+                        participantes = e.QtdParticipantes ?? 0 ,
+                        setor = e.SetorSolicitante != null ? e.SetorSolicitante.Nome : "Sem Setor" ,
+                        requisitante = e.Requisitante != null ? e.Requisitante.Nome : "Sem Requisitante" ,
                         e.Status
                     })
                     .ToListAsync();
 
-                return Json(new { success = true, dados = eventos });
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardEventosController.cs", "ObterTop10EventosMaiores");
-                Alerta.TratamentoErroComLinha("DashboardEventosController.cs", "ObterTop10EventosMaiores", ex);
-                return Json(new { success = false, message = ex.Message });
+                return Json(new { success = true , dados = eventos });
+            }
+            catch (Exception ex)
+            {
+                return Json(new { success = false , message = ex.Message });
             }
         }
 
         [HttpGet]
         [Route("api/DashboardEventos/ObterEventosPorDia")]
-        public async Task<IActionResult> ObterEventosPorDia(DateTime? dataInicio, DateTime? dataFim)
-        {
-            try
-            {
-                if (!dataInicio.HasValue || !dataFim.HasValue)
-                {
-                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
-                    dataInicio = dataFim.Value.AddDays(-30);
-                }
-
-                var eventos = await _context.Evento
-                    .AsNoTracking()
-                    .Where(e => e.DataInicial >= dataInicio && e.DataInicial <= dataFim)
+        public async Task<IActionResult> ObterEventosPorDia(DateTime? DataInicial , DateTime? dataFim)
+        {
+            try
+            {
+
+                if (!DataInicial.HasValue || !dataFim.HasValue)
+                {
+                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
+                    DataInicial = dataFim.Value.AddDays(-30);
+                }
+
+                var eventos = await _context.Evento
+                    .Where(e => e.DataInicial >= DataInicial && e.DataInicial <= dataFim)
                     .ToListAsync();
 
                 var eventosPorDia = eventos
                     .GroupBy(e => e.DataInicial)
                     .Select(g => new
                     {
-                        data = g.Key.HasValue ? g.Key.Value.Date.ToString("dd/MM/yyyy") : "",
-                        quantidade = g.Count(),
+                        data = g.Key.HasValue ? g.Key.Value.Date.ToString("dd/MM/yyyy") : "" ,
+                        quantidade = g.Count() ,
                         participantes = g.Sum(e => e.QtdParticipantes ?? 0)
                     })
                     .OrderBy(x => x.data)
                     .ToList();
 
-                return Json(new { success = true, dados = eventosPorDia });
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardEventosController.cs", "ObterEventosPorDia");
-                Alerta.TratamentoErroComLinha("DashboardEventosController.cs", "ObterEventosPorDia", ex);
-                return Json(new { success = false, message = ex.Message });
+                return Json(new { success = true , dados = eventosPorDia });
+            }
+            catch (Exception ex)
+            {
+                return Json(new { success = false , message = ex.Message });
             }
         }
 
```
