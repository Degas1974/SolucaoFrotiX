/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║  ███████╗██████╗  ██████╗ ████████╗██╗██╗  ██╗    ███████╗██╗████████╗███████╗                        ║
 * ║  ██╔════╝██╔══██╗██╔═══██╗╚══██╔══╝██║╚██╗██╔╝    ██╔════╝██║╚══██╔══╝██╔════╝                        ║
 * ║  █████╗  ██████╔╝██║   ██║   ██║   ██║ ╚███╔╝     ███████╗██║   ██║   █████╗                          ║
 * ║  ██╔══╝  ██╔══██╗██║   ██║   ██║   ██║ ██╔██╗     ╚════██║██║   ██║   ██╔══╝                          ║
 * ║  ██║     ██║  ██║╚██████╔╝   ██║   ██║██╔╝ ██╗    ███████║██║   ██║   ███████╗                        ║
 * ║  ╚═╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝╚═╝  ╚═╝    ╚══════╝╚═╝   ╚═╝   ╚══════╝                        ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Upsert.cshtml.cs                                                                                    ║
 * ║ PageModel para criação/edição de registros                                                          ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ METADATA                                                                                             ║
 * ║ @arquivo    Upsert.cshtml.cs                                                                        ║
 * ║ @local      Pages/Viagens/                                                                                ║
 * ║ @versao     1.0.0                                                                                    ║
 * ║ @data       23/01/2026                                                                               ║
 * ║ @relevancia Gestão de viagens                                                                       ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */
using AspNetCoreHero.ToastNotification.Abstractions;
using FrotiX.Cache;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Syncfusion.Blazor.Data;
using Syncfusion.EJ2.DropDowns;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FrotiX.Pages.Viagens
{
    public class UpsertModel : PageModel
    {
        public byte[] FichaVistoria;
        public static Guid viagemId;
        private static DateTime dataAgendamento;
        private static DateTime dataCancelamento;
        private static DateTime dataCriacao;
        private static string usuarioCorrenteId;
        private static string usuarioCorrenteNome;
        private static string UsuarioIdCancelamento;
        private static string usuarioIdCriacao;
        private static Guid veiculoAtual;
        private readonly MotoristaFotoService _fotoService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<IndexModel> _logger;
        private readonly MotoristaCache _motoristaCache;
        private readonly INotyfService _notyf;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Stopwatch _watch = new Stopwatch();
        private IWebHostEnvironment hostingEnv;
        private readonly ViagemEstatisticaService _estatisticaService;
        private readonly FrotiXDbContext _context;

        public UpsertModel(
        FrotiXDbContext context ,
        IUnitOfWork unitOfWork ,
        ILogger<IndexModel> logger ,
        INotyfService notyf ,
        IWebHostEnvironment env ,
        IWebHostEnvironment hostingEnvironment ,
        MotoristaFotoService fotoService ,
        MotoristaCache motoristaCache ,
        IViagemEstatisticaRepository viagemEstatisticaRepository  // ← ADICIONAR ESTA LINHA
        )
        {
            try
            {
                _context = context;
                _unitOfWork = unitOfWork;
                _logger = logger;
                _notyf = notyf;
                hostingEnv = env;
                _hostingEnvironment = hostingEnvironment;
                _fotoService = fotoService;
                _motoristaCache = motoristaCache;
                _estatisticaService = new ViagemEstatisticaService(_context , viagemEstatisticaRepository , unitOfWork);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "UpsertModel" , error);
            }
        }

        [BindProperty]
        public string DescricaoViagemWordBase64
        {
            get; set;
        }

        public string DescricaoViagemWordJson
        {
            get; set;
        }

        [BindProperty]
        public IFormFile FotoUpload
        {
            get; set;
        }

        [BindProperty]
        public ViagemViewModel ViagemObj
        {
            get; set;
        }

        [BindProperty]
        public string FotoBase64
        {
            get; set;
        }

        [BindProperty]
        public string FichaVistoriaExistente
        {
            get; set;
        }

        [BindProperty]
        public string OcorrenciasJson
        {
            get; set;
        }

        public static void ConvertContentTo(HtmlNode node , TextWriter outText)
        {
            try
            {
                foreach (HtmlNode subnode in node.ChildNodes)
                {
                    ConvertTo(subnode , outText);
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "ConvertContentTo" , error);
                return;
            }
        }

        public static string ConvertHtml(string html)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                StringWriter sw = new StringWriter();
                ConvertTo(doc.DocumentNode , sw);
                sw.Flush();
                return sw.ToString();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "ConvertHtml" , error);
                return html;
            }
        }

        public static void ConvertTo(HtmlNode node , TextWriter outText)
        {
            try
            {
                string html;
                switch (node.NodeType)
                {
                    case HtmlNodeType.Comment:
                        break;

                    case HtmlNodeType.Document:
                        ConvertContentTo(node , outText);
                        break;

                    case HtmlNodeType.Text:
                        string parentName = node.ParentNode.Name;
                        if ((parentName == "script") || (parentName == "style"))
                            break;

                        html = ((HtmlTextNode)node).Text;

                        if (HtmlNode.IsOverlappedClosingElement(html))
                            break;

                        if (html.Trim().Length > 0)
                        {
                            outText.Write(HtmlEntity.DeEntitize(html));
                        }
                        break;

                    case HtmlNodeType.Element:
                        switch (node.Name)
                        {
                            case "p":
                                outText.Write("\r\n");
                                break;
                        }

                        if (node.HasChildNodes)
                        {
                            ConvertContentTo(node , outText);
                        }
                        break;
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "ConvertTo" , error);
                return;
            }
        }

        public IActionResult OnGet(Guid id)
        {
            try
            {
                FichaVistoria = null;

                _watch.Restart();

                Console.WriteLine($">>> [START] OnGet: {DateTime.Now:HH:mm:ss.fff}");

                SetViewModel();

                if (id != Guid.Empty)
                {
                    viagemId = id;
                }

                ViewData["fieldsMotorista"] = new ComboBoxFieldSettings
                {
                    Text = "Nome" ,
                    Value = "MotoristaId" ,
                };
                ViewData["itemTemplate"] =
                @"<div><img class='fotoGrid' src='${Foto}' /> ${Nome}</div>";
                ViewData["valueTemplate"] =
                @"<div><img class='fotoGrid' src='${Foto}' /> ${Nome}</div>";

                ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                usuarioCorrenteId = currentUserID;
                var usuarios = _unitOfWork.AspNetUsers.GetAspNetUsersListForDropDown();
                foreach (var usuario in usuarios)
                {
                    if (usuario.Value == currentUserID)
                    {
                        usuarioCorrenteNome = usuario.Text;
                    }
                }

                if (id != Guid.Empty)
                {
                    var sw1 = Stopwatch.StartNew();
                    ViagemObj.Viagem = _unitOfWork.Viagem.GetFirstOrDefault(u => u.ViagemId == id);
                    if (ViagemObj == null)
                    {
                        return NotFound();
                    }
                    Console.WriteLine($">>> Carregamento Viagem(Id): {sw1.ElapsedMilliseconds}ms");

                    if (ViagemObj.Viagem.DescricaoViagemWord != null)
                    {
                        DescricaoViagemWordJson = Encoding.UTF8.GetString(
                        ViagemObj.Viagem.DescricaoViagemWord
                        );
                    }

                    if (ViagemObj.Viagem.UsuarioIdAgendamento != null && ViagemObj.Viagem.UsuarioIdAgendamento != "")
                    {
                        dataAgendamento = (DateTime)ViagemObj.Viagem.DataAgendamento;

                        var dbUsuarioAgendamento = _unitOfWork.AspNetUsers.GetFirstOrDefault(u =>
                        u.Id == ViagemObj.Viagem.UsuarioIdAgendamento
                        );
                        ViagemObj.NomeUsuarioAgendamento = dbUsuarioAgendamento.NomeCompleto;
                    }

                    if (ViagemObj.Viagem.UsuarioIdCriacao != null && ViagemObj.Viagem.UsuarioIdCriacao != "")
                    {
                        usuarioIdCriacao = ViagemObj.Viagem.UsuarioIdCriacao;
                        dataCriacao = (DateTime)ViagemObj.Viagem.DataCriacao;

                        var dbUsuarioCriacao = _unitOfWork.AspNetUsers.GetFirstOrDefault(u =>
                        u.Id == ViagemObj.Viagem.UsuarioIdCriacao
                        );
                        ViagemObj.NomeUsuarioCriacao = dbUsuarioCriacao.NomeCompleto;
                    }

                    if (ViagemObj.Viagem.UsuarioIdCancelamento != null && ViagemObj.Viagem.UsuarioIdCancelamento != "")
                    {
                        UsuarioIdCancelamento = ViagemObj.Viagem.UsuarioIdCancelamento;
                        dataCancelamento = (DateTime)ViagemObj.Viagem.DataCancelamento;

                        var dbUsuarioCriacao = _unitOfWork.AspNetUsers.GetFirstOrDefault(u =>
                        u.Id == ViagemObj.Viagem.UsuarioIdCancelamento
                        );
                        ViagemObj.NomeUsuarioCancelamento = dbUsuarioCriacao.NomeCompleto;
                    }

                    veiculoAtual = (Guid)ViagemObj.Viagem.VeiculoId;
                }
                else
                {
                    usuarioIdCriacao = usuarioCorrenteId;
                    dataCriacao = DateTime.Now;
                }

                if (ViagemObj.Viagem.DataFinalizacao != null)
                {
                    if (ViagemObj.Viagem.UsuarioIdFinalizacao != null && ViagemObj.Viagem.UsuarioIdFinalizacao != "")
                    {
                        var dbUsuarioFinalizacao = _unitOfWork.AspNetUsers.GetFirstOrDefault(u =>
                        u.Id == ViagemObj.Viagem.UsuarioIdFinalizacao
                        );
                        ViagemObj.NomeUsuarioFinalizacao = dbUsuarioFinalizacao.NomeCompleto;
                    }

                    var dataFinalizacao = ViagemObj.Viagem.DataFinalizacao;

                    ViagemObj.DataFinalizacao = dataFinalizacao?.ToString("dd/MM/yy");
                    ViagemObj.HoraFinalizacao = dataFinalizacao?.ToString("HH:mm");
                }

                var sw2 = Stopwatch.StartNew();
                PreencheListaSetores();
                Console.WriteLine($">>> PreencheDropDownsVeiculo: {sw2.ElapsedMilliseconds}ms");

                var sw3 = Stopwatch.StartNew();
                PreencheListaRequisitantes();
                Console.WriteLine($">>> Preenche ListaRequisitantes: {sw3.ElapsedMilliseconds}ms");

                var sw4 = Stopwatch.StartNew();
                PreencheListaMotoristas();
                Console.WriteLine(
                $">>> Preenche ListaMotoristas sem Redimensionamento: {sw4.ElapsedMilliseconds}ms"
                );

                var sw6 = Stopwatch.StartNew();
                PreencheListaVeiculos();
                Console.WriteLine($">>> Preenche ListaVeiculos: {sw6.ElapsedMilliseconds}ms");

                var sw7 = Stopwatch.StartNew();
                PreencheListaCombustivel();
                Console.WriteLine($">>> Preenche ListaCombustível: {sw7.ElapsedMilliseconds}ms");

                var sw8 = Stopwatch.StartNew();
                PreencheListaFinalidade();
                Console.WriteLine($">>> Preenche ListaFinalidade: {sw8.ElapsedMilliseconds}ms");

                var sw9 = Stopwatch.StartNew();
                PreencheListaEventos();
                Console.WriteLine($">>> Preenche ListaEventos: {sw9.ElapsedMilliseconds}ms");

                FichaVistoria = ViagemObj.Viagem.FichaVistoria;

                var sw10 = Stopwatch.StartNew();
                var listaOrigem = _unitOfWork
                .Viagem.GetAllReduced(selector: v => v.Origem)
                .Where(o => o != null)
                .Distinct()
                .OrderBy(o => o)
                .ToList();
                ViewData["ListaOrigem"] = listaOrigem;
                Console.WriteLine($">>> Preenche ListaOrigem: {sw10.ElapsedMilliseconds}ms");

                var sw11 = Stopwatch.StartNew();
                var listaDestino = _unitOfWork
                .Viagem.GetAllReduced(selector: v => v.Destino)
                .Where(d => d != null)
                .Distinct()
                .OrderBy(d => d)
                .ToList();
                ViewData["ListaDestino"] = listaDestino;
                Console.WriteLine($">>> Preenche ListaDestino: {sw11.ElapsedMilliseconds}ms");

                Console.WriteLine($">>> [END] OnGet TOTAL: {_watch.ElapsedMilliseconds}ms");

                return Page();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "OnGet" , error);
                return Page();
            }
        }

        public JsonResult OnGetAJAXPreencheListaEventos()
        {
            try
            {
                var listaEventos = (
                from e in _unitOfWork.Evento.GetAll(orderBy: e => e.OrderBy(e => e.Nome))
                select new
                {
                    e.Nome ,
                    e.EventoId
                }
                ).ToList();

                var eventoDataSource = new List<EventoData>();

                var eventosList = "";

                foreach (var evento in listaEventos)
                {
                    eventoDataSource.Add(
                    new EventoData { EventoId = evento.EventoId , Nome = evento.Nome }
                    );

                    eventosList =
                    eventosList
                    + "{ EventoId: '"
                    + evento.EventoId
                    + "', Nome: '"
                    + evento.Nome
                    + "'},";
                }

                eventosList = "[" + eventosList.Remove(eventosList.Length - 1) + "]";

                ViewData["dataEvento"] = eventoDataSource;

                return new JsonResult(new
                {
                    data = eventoDataSource
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                "Upsert.cshtml.cs" ,
                "OnGetAJAXPreencheListaEventos" ,
                error
                );
                return new JsonResult(new
                {
                    sucesso = false
                });
            }
        }

        public JsonResult OnGetAJAXPreencheListaRequisitantes()
        {
            try
            {
                // Busca dados sem ordenação no banco (melhor performance)
                var ListaRequisitantes = (
                from vr in _unitOfWork.ViewRequisitantes.GetAll()
                select new
                {
                    vr.Requisitante ,
                    vr.RequisitanteId
                }
                ).ToList();

                // Trim e ordena usando comparador natural (trata números corretamente)
                var ListaOrdenada = ListaRequisitantes
                    .Select(r => new
                    {
                        Requisitante = (r.Requisitante ?? "").Trim(),
                        r.RequisitanteId
                    })
                    .OrderBy(r => r.Requisitante ?? "", new FrotiX.Helpers.NaturalStringComparer())
                    .ToList();

                List<RequisitanteData> RequisitanteDataSource = new List<RequisitanteData>();

                var requisitantesList = "";

                foreach (var requisitante in ListaOrdenada)
                {
                    RequisitanteDataSource.Add(
                    new RequisitanteData
                    {
                        RequisitanteId = (Guid)requisitante.RequisitanteId ,
                        Requisitante = requisitante.Requisitante ,
                    }
                    );

                    requisitantesList =
                    requisitantesList
                    + "{ RequisitanteId: '"
                    + (Guid)requisitante.RequisitanteId
                    + "', Requisitante: '"
                    + requisitante.Requisitante
                    + "'},";
                }

                requisitantesList =
                "[" + requisitantesList.Remove(requisitantesList.Length - 1) + "]";

                ViewData["dataRequisitante"] = RequisitanteDataSource;

                return new JsonResult(new
                {
                    data = RequisitanteDataSource
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                "Upsert.cshtml.cs" ,
                "OnGetAJAXPreencheListaRequisitantes" ,
                error
                );
                return new JsonResult(new
                {
                    sucesso = false
                });
            }
        }

        public JsonResult OnGetAJAXPreencheListaSetores()
        {
            try
            {
                var listaSetores = _unitOfWork.ViewSetores.GetAll();
                var treeDataSource = new List<TreeData>();

                foreach (var setor in listaSetores)
                {
                    var temFilho = listaSetores.Any(u => u.SetorPaiId == setor.SetorSolicitanteId);

                    if (setor.SetorPaiId != Guid.Empty)
                    {
                        if (temFilho)
                        {
                            if (setor.SetorPaiId != Guid.Empty)
                            {
                                treeDataSource.Add(
                                new TreeData
                                {
                                    SetorSolicitanteId = (Guid)setor.SetorSolicitanteId ,
                                    SetorPaiId = setor.SetorPaiId ?? Guid.Empty ,
                                    Nome = setor.Nome ,
                                    HasChild = true ,
                                }
                                );
                            }
                            else
                            {
                                treeDataSource.Add(
                                new TreeData
                                {
                                    SetorSolicitanteId = (Guid)setor.SetorSolicitanteId ,
                                    Nome = setor.Nome ,
                                    HasChild = true ,
                                }
                                );
                            }
                        }
                        else
                        {
                            treeDataSource.Add(
                            new TreeData
                            {
                                SetorSolicitanteId = (Guid)setor.SetorSolicitanteId ,
                                SetorPaiId = setor.SetorPaiId ?? Guid.Empty ,
                                Nome = setor.Nome ,
                            }
                            );
                        }
                    }
                    else
                    {
                        if (temFilho)
                        {
                            treeDataSource.Add(
                            new TreeData
                            {
                                SetorSolicitanteId = (Guid)setor.SetorSolicitanteId ,
                                Nome = setor.Nome ,
                                HasChild = true ,
                            }
                            );
                        }
                    }
                }

                ViewData["dataSetor"] = treeDataSource;

                return new JsonResult(new
                {
                    data = treeDataSource
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                "Upsert.cshtml.cs" ,
                "OnGetAJAXPreencheListaSetores" ,
                error
                );
                return new JsonResult(new
                {
                    sucesso = false
                });
            }
        }

        public JsonResult OnGetFichaExistente(string id)
        {
            try
            {
                int NoFichaVistoria = int.Parse(id);

                var objViagem = _unitOfWork.Viagem.GetFirstOrDefault(n =>
                n.NoFichaVistoria == NoFichaVistoria
                );

                if (objViagem == null)
                {
                    return new JsonResult(new
                    {
                        data = false
                    });
                }

                return new JsonResult(new
                {
                    data = true
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "OnGetFichaExistente" , error);
                return new JsonResult(new
                {
                    sucesso = false
                });
            }
        }

        public JsonResult OnGetMotoristasJson()
        {
            try
            {
                var lista = _motoristaCache.GetMotoristas();
                return new JsonResult(lista);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "OnGetMotoristasJson" , error);
                return new JsonResult(new
                {
                    sucesso = false
                });
            }
        }

        public JsonResult OnGetPegaKmAtualVeiculo(string id)
        {
            try
            {
                Guid guidOutput;
                bool isValid = Guid.TryParse(id , out guidOutput);

                if (id != null && isValid)
                {
                    Guid veiculoid = Guid.Parse(id);
                    var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(v =>
                    (v.VeiculoId == veiculoid)
                    );
                    return new JsonResult(new
                    {
                        data = veiculo.Quilometragem
                    });
                }

                return new JsonResult(new
                {
                    data = 0
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "OnGetPegaKmAtualVeiculo" , error);
                return new JsonResult(new
                {
                    data = 0
                });
            }
        }

        public JsonResult OnGetPegaRamal(string id)
        {
            try
            {
                if (Guid.TryParse(id , out var requisitanteid))
                {
                    var requisitante = _unitOfWork.Requisitante.GetFirstOrDefault(e =>
                    e.RequisitanteId == requisitanteid
                    );
                    return new JsonResult(new
                    {
                        data = requisitante.Ramal
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        data = ""
                    });
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "OnGetPegaRamal" , error);
                return new JsonResult(new
                {
                    sucesso = false
                });
            }
        }

        public JsonResult OnGetPegaSetor(string id)
        {
            try
            {
                if (Guid.TryParse(id , out var requisitanteid))
                {
                    var requisitante = _unitOfWork.Requisitante.GetFirstOrDefault(e =>
                    e.RequisitanteId == requisitanteid
                    );
                    var setorrequisitante = _unitOfWork.SetorSolicitante.GetFirstOrDefault(e =>
                    e.SetorSolicitanteId == requisitante.SetorSolicitanteId
                    );
                    return new JsonResult(new
                    {
                        data = setorrequisitante.SetorSolicitanteId
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        data = ""
                    });
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "OnGetPegaSetor" , error);
                return new JsonResult(new
                {
                    sucesso = false
                });
            }
        }

        public JsonResult OnGetVerificaFicha(string id)
        {
            try
            {
                var objFicha = _unitOfWork
                .Viagem.GetAllReduced(selector: f => new { f.NoFichaVistoria })
                .Max(n => n.NoFichaVistoria);

                return new JsonResult(new
                {
                    data = objFicha.Value
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "OnGetVerificaFicha" , error);
                return new JsonResult(new
                {
                    sucesso = false
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: OnGetVerificaMotoristaViagem
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : [PORQUÊ] Evitar alocar motorista em viagem aberta.
         *                   [O QUE] Verifica se existe viagem em aberto para o motorista.
         *                   [COMO] Valida GUID e consulta viagens com status "Aberta" e HoraFim nula.
         *
         * 📥 ENTRADAS     : id [string] - GUID do motorista.
         *
         * 📤 SAÍDAS       : JSON { data: bool } indicando se ha viagem aberta.
         *
         * 🔗 CHAMADA POR  : /Viagens/Upsert?handler=VerificaMotoristaViagem (AJAX).
         *
         * 🔄 CHAMA        : _unitOfWork.Viagem.GetFirstOrDefault.
         ****************************************************************************************/
        public JsonResult OnGetVerificaMotoristaViagem(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id) || !Guid.TryParse(id , out Guid motoristaId))
                {
                    return new JsonResult(new
                    {
                        data = false
                    });
                }

                bool temViagemAberta = _context.Viagem
                    .AsNoTracking()
                    .Any(e => e.MotoristaId == motoristaId && e.HoraFim == null && e.Status == "Aberta");

                return new JsonResult(new
                {
                    data = temViagemAberta
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                "Upsert.cshtml.cs" ,
                "OnGetVerificaMotoristaViagem" ,
                error
                );
                return new JsonResult(new
                {
                    sucesso = false
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: OnGetVerificaVeiculoViagem
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : [PORQUÊ] Evitar alocar veiculo em viagem aberta.
         *                   [O QUE] Verifica se existe viagem em aberto para o veiculo.
         *                   [COMO] Valida GUID e consulta viagens com status "Aberta" e HoraFim nula.
         *
         * 📥 ENTRADAS     : id [string] - GUID do veiculo.
         *
         * 📤 SAÍDAS       : JSON { data: bool } indicando se ha viagem aberta.
         *
         * 🔗 CHAMADA POR  : /Viagens/Upsert?handler=VerificaVeiculoViagem (AJAX).
         *
         * 🔄 CHAMA        : _unitOfWork.Viagem.GetFirstOrDefault.
         ****************************************************************************************/
        public JsonResult OnGetVerificaVeiculoViagem(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id) || !Guid.TryParse(id , out Guid veiculoId))
                {
                    return new JsonResult(new
                    {
                        data = false
                    });
                }

                bool temViagemAberta = _context.Viagem
                    .AsNoTracking()
                    .Any(e => e.VeiculoId == veiculoId && e.HoraFim == null && e.Status == "Aberta");

                return new JsonResult(new
                {
                    data = temViagemAberta
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                "Upsert.cshtml.cs" ,
                "OnGetVerificaVeiculoViagem" ,
                error
                );
                return new JsonResult(new
                {
                    data = false ,
                    erro = error.Message
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: OnPostEditAsync
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : [PORQUÊ] Atualizar viagem garantindo regras de negocio essenciais.
         *                   [O QUE] Valida limites de km e persiste alteracoes.
         *                   [COMO] Valida modelo e salva via UnitOfWork.
         *
         * 📥 ENTRADAS     : Id [Guid] - Identificador da viagem.
         *
         * 📤 SAÍDAS       : IActionResult (JSON em AJAX ou redirect).
         *
         * 🔗 CHAMADA POR  : Submit do formulario Upsert (edit).
         ****************************************************************************************/
        public async Task<IActionResult> OnPostEditAsync(Guid Id)
        {
            try
            {
                ViagemObj.Viagem.ViagemId = Id;

                // Se NoFichaVistoria não foi informado, gravar 0 (viagem mobile)
                if (ViagemObj.Viagem.NoFichaVistoria == null || ViagemObj.Viagem.NoFichaVistoria == 0)
                {
                    ViagemObj.Viagem.NoFichaVistoria = 0;
                }

                // PROCESSAR IMAGEM BASE64
                if (Request.Form.ContainsKey("FotoBase64"))
                {
                    var fotoBase64 = Request.Form["FotoBase64"].ToString();
                    if (!string.IsNullOrEmpty(fotoBase64))
                    {
                        try
                        {
                            var base64Data = fotoBase64.Contains(",")
                            ? fotoBase64.Split(',')[1]
                            : fotoBase64;

                            ViagemObj.Viagem.FichaVistoria = Convert.FromBase64String(base64Data);
                            System.Diagnostics.Debug.WriteLine(
                            $"Nova imagem processada: {ViagemObj.Viagem.FichaVistoria.Length} bytes"
                            );
                        }
                        catch (Exception exImg)
                        {
                            System.Diagnostics.Debug.WriteLine(
                            $"Erro ao processar imagem: {exImg.Message}"
                            );
                        }
                    }
                }
                else if (Request.Form.ContainsKey("FichaVistoriaExistente"))
                {
                    var fichaExistente = Request.Form["FichaVistoriaExistente"].ToString();
                    if (!string.IsNullOrEmpty(fichaExistente))
                    {
                        try
                        {
                            ViagemObj.Viagem.FichaVistoria = Convert.FromBase64String(
                            fichaExistente
                            );
                            System.Diagnostics.Debug.WriteLine(
                            $"Imagem existente mantida: {ViagemObj.Viagem.FichaVistoria.Length} bytes"
                            );
                        }
                        catch (Exception exImg)
                        {
                            System.Diagnostics.Debug.WriteLine(
                            $"Erro ao processar imagem existente: {exImg.Message}"
                            );
                        }
                    }
                }

                const int maxKm = 1000000;
                if (ViagemObj?.Viagem?.KmInicial != null && ViagemObj.Viagem.KmInicial > maxKm)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return new JsonResult(new
                        {
                            success = false,
                            message = "A quilometragem inicial nao pode ultrapassar 1.000.000."
                        });
                    }

                    AppToast.show("Vermelho", "A quilometragem inicial nao pode ultrapassar 1.000.000.", 3000);
                    return Page();
                }

                if (ViagemObj?.Viagem?.KmFinal != null && ViagemObj.Viagem.KmFinal > maxKm)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return new JsonResult(new
                        {
                            success = false,
                            message = "A quilometragem final nao pode ultrapassar 1.000.000."
                        });
                    }

                    AppToast.show("Vermelho", "A quilometragem final nao pode ultrapassar 1.000.000.", 3000);
                    return Page();
                }

                if (ViagemObj.Viagem.HoraFim == null)
                {
                    ViagemObj.Viagem.UsuarioIdCriacao = usuarioCorrenteId;
                    ViagemObj.Viagem.DataCriacao = DateTime.Now;
                }
                else
                {
                    if (ViagemObj.Viagem.UsuarioIdCriacao == null)
                    {
                        ViagemObj.Viagem.UsuarioIdCriacao = usuarioCorrenteId;
                        ViagemObj.Viagem.DataCriacao = dataCriacao;
                    }

                    ViagemObj.Viagem.UsuarioIdFinalizacao = usuarioCorrenteId;
                    ViagemObj.Viagem.DataFinalizacao = DateTime.Now;
                }

                Guid viagemId =
                ViagemObj.Viagem.ViagemId != Guid.Empty
                ? ViagemObj.Viagem.ViagemId
                : Guid.Empty;

                if (
                (ViagemObj.Viagem.HoraFim != null && ViagemObj.Viagem.KmFinal == null)
                || (ViagemObj.Viagem.HoraFim == null && ViagemObj.Viagem.KmFinal != null)
                )
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return new JsonResult(
                        new
                        {
                            success = false ,
                            message = "Para finalizar a viagem, tanto a Hora Final como a Quilometragem Final precisam estar preenchidas!" ,
                        }
                        );
                    }

                    AppToast.show("Vermelho" , "Para finalizar a viagem, tanto a Hora Final como a Quilometragem Final precisam estar preenchidas!" , 3000);

                    PreencheListaSetores();
                    PreencheListaRequisitantes();
                    PreencheListaMotoristas();
                    PreencheListaVeiculos();
                    PreencheListaCombustivel();
                    PreencheListaFinalidade();
                    PreencheListaEventos();

                    ViagemObj.Viagem.ViagemId = viagemId;
                    return Page();
                }

                if (ViagemObj.Viagem.HoraFim == null)
                {
                    ViagemObj.Viagem.Status = "Aberta";
                }
                else
                {
                    //ViagemObj.Viagem.Status = "Realizada";
                    //ViagemObj.Viagem.CustoCombustivel = Servicos.CalculaCustoCombustivel(
                    //ViagemObj.Viagem ,
                    //_unitOfWork
                    //);

                    //int minutos = -1;
                    //ViagemObj.Viagem.CustoMotorista = Servicos.CalculaCustoMotorista(
                    //ViagemObj.Viagem ,
                    //_unitOfWork ,
                    //ref minutos
                    //);
                    //ViagemObj.Viagem.Minutos = minutos;

                    //ViagemObj.Viagem.CustoOperador = Servicos.CalculaCustoOperador(
                    //ViagemObj.Viagem ,
                    //_unitOfWork
                    //);

                    //ViagemObj.Viagem.CustoLavador = Servicos.CalculaCustoLavador(
                    //ViagemObj.Viagem ,
                    //_unitOfWork
                    //);

                    //ViagemObj.Viagem.CustoVeiculo = Servicos.CalculaCustoVeiculo(
                    //ViagemObj.Viagem ,
                    //_unitOfWork
                    //);
                }

                var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(v =>
                v.VeiculoId == ViagemObj.Viagem.VeiculoId
                );

                if (ViagemObj.Viagem.KmFinal != null && veiculo != null)
                {
                    veiculo.Quilometragem = ViagemObj.Viagem.KmFinal;
                    _unitOfWork.Veiculo.Update(veiculo);
                    _unitOfWork.Save();
                }

                string descricao = ViagemObj.Viagem.Descricao;
                if (!string.IsNullOrEmpty(descricao))
                {
                    descricao = ConvertHtml(descricao);
                }
                ViagemObj.Viagem.DescricaoSemFormato = descricao;

                if (!string.IsNullOrWhiteSpace(DescricaoViagemWordBase64))
                {
                    ViagemObj.Viagem.DescricaoViagemWord = Convert.FromBase64String(
                    DescricaoViagemWordBase64
                    );
                }

                ViagemObj.Viagem.StatusAgendamento = false;

                _unitOfWork.Viagem.Update(ViagemObj.Viagem);
                _unitOfWork.Save();

                // Processar novas ocorrências (se houver)
                int ocorrenciasCriadas = ProcessarOcorrencias(
                    ViagemObj.Viagem.ViagemId,
                    ViagemObj.Viagem.VeiculoId,
                    ViagemObj.Viagem.MotoristaId
                );

                if (ViagemObj.Viagem.Status == "Realizada")
                {
                    await _estatisticaService.AtualizarEstatisticasDiaAsync(ViagemObj.Viagem.DataInicial.Value);
                }

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var mensagem = "Viagem atualizada com sucesso!";
                    if (ocorrenciasCriadas > 0)
                    {
                        mensagem += $" ({ocorrenciasCriadas} ocorrência(s) registrada(s))";
                    }
                    
                    return new JsonResult(
                    new
                    {
                        success = true ,
                        message = mensagem ,
                        redirectUrl = "/Viagens" ,
                        ocorrenciasCriadas = ocorrenciasCriadas
                    }
                    );
                }

                AppToast.show("Verde" , "Viagem atualizada com sucesso!" , 2000);
                return RedirectToPage("./Index");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "OnPostEdit" , error);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return new JsonResult(
                    new
                    {
                        success = false ,
                        message = $"Erro ao salvar: {error.Message}"
                    }
                    );
                }

                return RedirectToPage("./Index");
            }
        }

        public async Task<IActionResult> OnPostSalvarDescricaoAsync()
        {
            try
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();

                var input = JsonSerializer.Deserialize<ViagemDescricaoInput>(body);

                if (input is null || input.ViagemId == Guid.Empty)
                    return new JsonResult(new
                    {
                        message = "Dados inválidos."
                    })
                    {
                        StatusCode = 400 ,
                    };

                var viagem = _unitOfWork.Viagem.GetFirstOrDefault(v =>
                v.ViagemId == input.ViagemId
                );
                if (viagem is null)
                    return new JsonResult(new
                    {
                        message = "Viagem não encontrada."
                    })
                    {
                        StatusCode = 404 ,
                    };

                viagem.DescricaoViagemWord = Encoding.UTF8.GetBytes(input.DescricaoWord ?? "");
                _unitOfWork.Save();

                return new JsonResult(new
                {
                    message = "Descrição salva com sucesso!"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                "Upsert.cshtml.cs" ,
                "OnPostSalvarDescricaoAsync" ,
                error
                );
                return new JsonResult(new
                {
                    message = "Impossível Salvar Descrição!"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: OnPostSubmitAsync
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : [PORQUÊ] Criar nova viagem garantindo regras de negocio essenciais.
         *                   [O QUE] Valida limites de km e persiste novo registro.
         *                   [COMO] Valida modelo e salva via UnitOfWork.
         *
         * 📥 ENTRADAS     : Formulario ViagemObj (POST).
         *
         * 📤 SAÍDAS       : IActionResult (JSON em AJAX ou redirect).
         *
         * 🔗 CHAMADA POR  : Submit do formulario Upsert (create).
         ****************************************************************************************/
        public async Task<IActionResult> OnPostSubmitAsync()
        {
            try
            {
                if (ViagemObj.Viagem.ViagemId == Guid.Empty)
                {
                    ViagemObj.Viagem.ViagemId = Guid.NewGuid();
                }

                // Se NoFichaVistoria não foi informado, gravar 0 (viagem mobile)
                if (ViagemObj.Viagem.NoFichaVistoria == null || ViagemObj.Viagem.NoFichaVistoria == 0)
                {
                    ViagemObj.Viagem.NoFichaVistoria = 0;
                }

                const int maxKm = 1000000;
                if (ViagemObj?.Viagem?.KmInicial != null && ViagemObj.Viagem.KmInicial > maxKm)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return new JsonResult(new
                        {
                            success = false,
                            message = "A quilometragem inicial nao pode ultrapassar 1.000.000."
                        });
                    }

                    AppToast.show("Vermelho", "A quilometragem inicial nao pode ultrapassar 1.000.000.", 3000);
                    return Page();
                }

                if (ViagemObj?.Viagem?.KmFinal != null && ViagemObj.Viagem.KmFinal > maxKm)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return new JsonResult(new
                        {
                            success = false,
                            message = "A quilometragem final nao pode ultrapassar 1.000.000."
                        });
                    }

                    AppToast.show("Vermelho", "A quilometragem final nao pode ultrapassar 1.000.000.", 3000);
                    return Page();
                }

                if (Request.Form.ContainsKey("FotoBase64"))
                {
                    var fotoBase64 = Request.Form["FotoBase64"].ToString();
                    if (!string.IsNullOrEmpty(fotoBase64))
                    {
                        try
                        {
                            var base64Data = fotoBase64.Contains(",")
                            ? fotoBase64.Split(',')[1]
                            : fotoBase64;

                            ViagemObj.Viagem.FichaVistoria = Convert.FromBase64String(base64Data);
                        }
                        catch (Exception exImg)
                        {
                            System.Diagnostics.Debug.WriteLine(
                            $"Erro ao processar imagem: {exImg.Message}"
                            );
                        }
                    }
                }

                var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(v =>
                v.VeiculoId == ViagemObj.Viagem.VeiculoId
                );

                if (ViagemObj.Viagem.KmFinal != null && veiculo != null)
                {
                    veiculo.Quilometragem = ViagemObj.Viagem.KmFinal;
                    _unitOfWork.Veiculo.Update(veiculo);
                    _unitOfWork.Save();
                }

                ViagemObj.Viagem.UsuarioIdCriacao = usuarioCorrenteId;
                ViagemObj.Viagem.DataCriacao = DateTime.Now;
                if (ViagemObj.Viagem.DataFinal == null)
                {
                    ViagemObj.Viagem.Status = "Aberta";
                }
                else
                {
                    ViagemObj.Viagem.Status = "Realizada";

                    //ViagemObj.Viagem.CustoCombustivel = Servicos.CalculaCustoCombustivel(
                    //ViagemObj.Viagem ,
                    //_unitOfWork
                    //);

                    //int minutos = -1;
                    //ViagemObj.Viagem.CustoMotorista = Servicos.CalculaCustoMotorista(
                    //ViagemObj.Viagem ,
                    //_unitOfWork ,
                    //ref minutos
                    //);
                    //ViagemObj.Viagem.Minutos = minutos;

                    //ViagemObj.Viagem.CustoOperador = Servicos.CalculaCustoOperador(
                    //ViagemObj.Viagem ,
                    //_unitOfWork
                    //);

                    //ViagemObj.Viagem.CustoLavador = Servicos.CalculaCustoLavador(
                    //ViagemObj.Viagem ,
                    //_unitOfWork
                    //);

                    //ViagemObj.Viagem.CustoVeiculo = Servicos.CalculaCustoVeiculo(
                    //ViagemObj.Viagem ,
                    //_unitOfWork
                    //);
                }

                ViagemObj.Viagem.StatusAgendamento = false;

                if (!string.IsNullOrEmpty(ViagemObj.Viagem.Descricao))
                {
                    ViagemObj.Viagem.DescricaoSemFormato = ConvertHtml(ViagemObj.Viagem.Descricao);
                }

                _unitOfWork.Viagem.Add(ViagemObj.Viagem);
                _unitOfWork.Save();

                // Processar ocorrências (se houver)
                int ocorrenciasCriadas = ProcessarOcorrencias(
                    ViagemObj.Viagem.ViagemId,
                    ViagemObj.Viagem.VeiculoId,
                    ViagemObj.Viagem.MotoristaId
                );

                if (ViagemObj.Viagem.Status == "Realizada")
                {
                    await _estatisticaService.AtualizarEstatisticasDiaAsync(ViagemObj.Viagem.DataInicial.Value);
                }

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var mensagem = "Viagem criada com sucesso!";
                    if (ocorrenciasCriadas > 0)
                    {
                        mensagem += $" ({ocorrenciasCriadas} ocorrência(s) registrada(s))";
                    }
                    
                    return new JsonResult(
                    new
                    {
                        success = true ,
                        message = mensagem ,
                        redirectUrl = "/Viagens" ,
                        ocorrenciasCriadas = ocorrenciasCriadas
                    }
                    );
                }

                AppToast.show("Verde" , "Viagem criada com sucesso!" , 2000);
                return RedirectToPage("./Index");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "OnPostSubmit" , error);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return new JsonResult(
                    new
                    {
                        success = false ,
                        message = $"Erro ao criar viagem: {error.Message}"
                    }
                    );
                }

                return Page();
            }
        }

        public void PreencheListaCombustivel()
        {
            try
            {
                List<CombustivelData> CombustivelDataSource = new List<CombustivelData>();

                CombustivelDataSource.Add(
                new CombustivelData
                {
                    Nivel = "tanquevazio" ,
                    Descricao = "Vazio" ,
                    Imagem = "../images/tanquevazio.png" ,
                }
                );

                CombustivelDataSource.Add(
                new CombustivelData
                {
                    Nivel = "tanqueumquarto" ,
                    Descricao = "1/4" ,
                    Imagem = "../images/tanqueumquarto.png" ,
                }
                );

                CombustivelDataSource.Add(
                new CombustivelData
                {
                    Nivel = "tanquemeiotanque" ,
                    Descricao = "1/2" ,
                    Imagem = "../images/tanquemeiotanque.png" ,
                }
                );

                CombustivelDataSource.Add(
                new CombustivelData
                {
                    Nivel = "tanquetresquartos" ,
                    Descricao = "3/4" ,
                    Imagem = "../images/tanquetresquartos.png" ,
                }
                );

                CombustivelDataSource.Add(
                new CombustivelData
                {
                    Nivel = "tanquecheio" ,
                    Descricao = "Cheio" ,
                    Imagem = "../images/tanquecheio.png" ,
                }
                );

                ViewData["dataCombustivel"] = CombustivelDataSource;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                "Upsert.cshtml.cs" ,
                "PreencheListaCombustivel" ,
                error
                );
                return;
            }
        }

        public void PreencheListaEventos()
        {
            try
            {
                var ListaEventos = _unitOfWork
                .Evento.GetAll(e => e.Status == "1")
                .OrderBy(e => e.Nome);

                List<EventoData> EventoDataSource = new List<EventoData>();

                foreach (var evento in ListaEventos)
                {
                    EventoDataSource.Add(
                    new EventoData { EventoId = evento.EventoId , Nome = evento.Nome }
                    );
                }

                ViewData["dataEvento"] = EventoDataSource;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "PreencheListaEventos" , error);
                return;
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: PreencheListaFinalidade
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : [PORQUÊ] Garantir que a lista de finalidades seja carregada e
         *                   ordenada alfabeticamente para facilitar a selecao pelo usuario.
         *                   [O QUE] Monta o dataSource com as finalidades fixas do sistema.
         *                   [COMO] Cria a lista base e ordena por descricao com comparador natural.
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : ViewData["dataFinalidade"] preenchido com lista ordenada.
         *
         * ⬅️ CHAMADO POR  : OnGet() [Upsert.cshtml.cs]
         *                   OnPostEditAsync() [Upsert.cshtml.cs]
         *                   OnPostSubmitAsync() [Upsert.cshtml.cs]
         *
         * ➡️ CHAMA        : FrotiX.Helpers.NaturalStringComparer (ordenacao natural)
         *
         * 📝 OBSERVAÇÕES  : Lista fixa de finalidades usada no DropDownList de Viagens.
         ****************************************************************************************/
        public void PreencheListaFinalidade()
        {
            try
            {
                List<FinalidadeData> FinalidadeDataSource = new List<FinalidadeData>();

                FinalidadeDataSource.Add(
                new FinalidadeData
                {
                    FinalidadeId = "Transporte de Funcionários" ,
                    Descricao = "Transporte de Funcionários" ,
                }
                );

                FinalidadeDataSource.Add(
                new FinalidadeData
                {
                    FinalidadeId = "Transporte de Convidados" ,
                    Descricao = "Transporte de Convidados" ,
                }
                );

                FinalidadeDataSource.Add(
                new FinalidadeData
                {
                    FinalidadeId = "Transporte de Materiais/Cargas" ,
                    Descricao = "Transporte de Materiais/Cargas" ,
                }
                );

                FinalidadeDataSource.Add(
                new FinalidadeData
                {
                    FinalidadeId = "Economildo Norte(Cefor)" ,
                    Descricao = "Economildo Norte(Cefor)" ,
                }
                );

                FinalidadeDataSource.Add(
                new FinalidadeData
                {
                    FinalidadeId = "Economildo Sul(PGR)" ,
                    Descricao = "Economildo Sul(PGR)" ,
                }
                );

                FinalidadeDataSource.Add(
                new FinalidadeData
                {
                    FinalidadeId = "Economildo Rodoviária" ,
                    Descricao = "Economildo Rodoviária" ,
                }
                );

                FinalidadeDataSource.Add(
                new FinalidadeData
                {
                    FinalidadeId = "Mesa (carros pretos)" ,
                    Descricao = "Mesa (carros pretos)" ,
                }
                );

                FinalidadeDataSource.Add(
                new FinalidadeData
                {
                    FinalidadeId = "TV/Rádio Câmara" ,
                    Descricao = "TV/Rádio Câmara" ,
                }
                );

                FinalidadeDataSource.Add(
                new FinalidadeData { FinalidadeId = "Aeroporto" , Descricao = "Aeroporto" }
                );

                FinalidadeDataSource.Add(
                new FinalidadeData
                {
                    FinalidadeId = "Saída para Manutenção" ,
                    Descricao = "Saída para Manutenção" ,
                }
                );

                FinalidadeDataSource.Add(
                new FinalidadeData
                {
                    FinalidadeId = "Chegada da Manutenção" ,
                    Descricao = "Chegada da Manutenção" ,
                }
                );

                FinalidadeDataSource.Add(
                new FinalidadeData
                {
                    FinalidadeId = "Abastecimento" ,
                    Descricao = "Abastecimento" ,
                }
                );

                FinalidadeDataSource.Add(
                new FinalidadeData
                {
                    FinalidadeId = "Recebimento da Locadora" ,
                    Descricao = "Recebimento da Locadora" ,
                }
                );

                FinalidadeDataSource.Add(
                new FinalidadeData
                {
                    FinalidadeId = "Devolução à Locadora" ,
                    Descricao = "Devolução à Locadora" ,
                }
                );

                FinalidadeDataSource.Add(
                new FinalidadeData
                {
                    FinalidadeId = "Saída Programada" ,
                    Descricao = "Saída Programada" ,
                }
                );

                FinalidadeDataSource.Add(
                new FinalidadeData { FinalidadeId = "Evento" , Descricao = "Evento" }
                );

                FinalidadeDataSource.Add(
                new FinalidadeData { FinalidadeId = "Ambulância" , Descricao = "Ambulância" }
                );

                FinalidadeDataSource.Add(
                new FinalidadeData
                {
                    FinalidadeId = "Enviado Depol" ,
                    Descricao = "Enviado Depol" ,
                }
                );

                FinalidadeDataSource.Add(
                new FinalidadeData
                {
                    FinalidadeId = "Recebido Depol" ,
                    Descricao = "Recebido Depol" ,
                }
                );

                FinalidadeDataSource.Add(
                new FinalidadeData
                {
                    FinalidadeId = "Demanda Política" ,
                    Descricao = "Demanda Política" ,
                }
                );

                FinalidadeDataSource.Add(
                new FinalidadeData { FinalidadeId = "Passaporte" , Descricao = "Passaporte" }
                );

                FinalidadeDataSource.Add(
                new FinalidadeData { FinalidadeId = "Cursos Depol" , Descricao = "Cursos Depol" }
                );

                FinalidadeDataSource = FinalidadeDataSource
                    .OrderBy(f => (f.Descricao ?? string.Empty).Trim(), new FrotiX.Helpers.NaturalStringComparer())
                    .ToList();

                ViewData["dataFinalidade"] = FinalidadeDataSource;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "PreencheListaFinalidade" , error);
                return;
            }
        }

        public void PreencheListaMotoristas()
        {
            try
            {
                var stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                var ListaMotoristas = _unitOfWork
                .ViewMotoristasViagem.GetAllReduced(
                selector: m => new
                {
                    m.MotoristaId ,
                    m.MotoristaCondutor ,
                    m.Foto ,
                } ,
                orderBy: m => m.OrderBy(m => m.Nome)
                )
                .ToList();

                var motoristaDataSource = new List<object>();

                foreach (var motorista in ListaMotoristas)
                {
                    string fotoBase64 =
                    motorista.Foto != null
                    ? $"data:image/jpeg;base64,{Convert.ToBase64String(motorista.Foto)}"
                    : null;

                    motoristaDataSource.Add(
                    new
                    {
                        MotoristaId = motorista.MotoristaId ,
                        Nome = motorista.MotoristaCondutor ,
                        Foto = fotoBase64 ,
                    }
                    );
                }

                ViewData["dataMotorista"] = motoristaDataSource;

                stopwatch.Stop();
                Console.WriteLine(
                $"[SEM redimensionamento] Tempo: {stopwatch.ElapsedMilliseconds}ms"
                );
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "PreencheListaMotoristas" , error);
                return;
            }
        }

        public void PreencheListaMotoristasComRedimensionamento()
        {
            try
            {
                var stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                var lista = _unitOfWork
                .ViewMotoristasViagem.GetAllReduced(
                selector: m => new
                {
                    m.MotoristaId ,
                    m.MotoristaCondutor ,
                    m.Foto ,
                } ,
                orderBy: m => m.OrderBy(x => x.MotoristaCondutor)
                )
                .ToList();

                var listaComFoto = lista
                .Select(m => new
                {
                    MotoristaId = m.MotoristaId ,
                    Nome = m.MotoristaCondutor ,
                    Foto = _fotoService.ObterFotoBase64(m.MotoristaId , m.Foto) ,
                })
                .ToList();

                ViewData["dataMotorista"] = listaComFoto;

                stopwatch.Stop();
                Console.WriteLine(
                $"[COM redimensionamento] Tempo: {stopwatch.ElapsedMilliseconds}ms"
                );
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                "Upsert.cshtml.cs" ,
                "PreencheListaMotoristasComRedimensionamento" ,
                error
                );
                return;
            }
        }

        public void PreencheListaRequisitantes()
        {
            try
            {
                // Busca dados sem ordenação no banco (melhor performance)
                var listaRequisitantes = _unitOfWork
                .ViewRequisitantes.GetAllReduced(
                selector: vr => new { vr.Requisitante , vr.RequisitanteId }
                )
                .ToList();

                // Trim e ordena usando comparador natural (trata números corretamente)
                var listaOrdenada = listaRequisitantes
                    .Select(r => new
                    {
                        Requisitante = (r.Requisitante ?? "").Trim(),
                        r.RequisitanteId
                    })
                    .OrderBy(r => r.Requisitante ?? "", new FrotiX.Helpers.NaturalStringComparer())
                    .ToList();

                var requisitanteDataSource = new List<RequisitanteData>();

                foreach (var requisitante in listaOrdenada)
                {
                    requisitanteDataSource.Add(
                    new RequisitanteData
                    {
                        RequisitanteId = (Guid)requisitante.RequisitanteId ,
                        Requisitante = requisitante.Requisitante ,
                    }
                    );
                }

                ViewData["dataRequisitante"] = requisitanteDataSource;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                "Upsert.cshtml.cs" ,
                "PreencheListaRequisitantes" ,
                error
                );
                return;
            }
        }

        public void PreencheListaSetores()
        {
            try
            {
                var listaSetores = _unitOfWork.ViewSetores.GetAll();
                var treeDataSource = new List<TreeData>();

                foreach (var setor in listaSetores)
                {
                    var temFilho = listaSetores.Any(u => u.SetorPaiId == setor.SetorSolicitanteId);

                    if (setor.SetorPaiId != Guid.Empty)
                    {
                        if (temFilho)
                        {
                            if (setor.SetorPaiId != Guid.Empty)
                            {
                                treeDataSource.Add(
                                new TreeData
                                {
                                    SetorSolicitanteId = (Guid)setor.SetorSolicitanteId ,
                                    SetorPaiId = setor.SetorPaiId ?? Guid.Empty ,
                                    Nome = setor.Nome ,
                                    HasChild = true ,
                                }
                                );
                            }
                            else
                            {
                                treeDataSource.Add(
                                new TreeData
                                {
                                    SetorSolicitanteId = (Guid)setor.SetorSolicitanteId ,
                                    Nome = setor.Nome ,
                                    HasChild = true ,
                                }
                                );
                            }
                        }
                        else
                        {
                            treeDataSource.Add(
                            new TreeData
                            {
                                SetorSolicitanteId = (Guid)setor.SetorSolicitanteId ,
                                SetorPaiId = setor.SetorPaiId ?? Guid.Empty ,
                                Nome = setor.Nome ,
                            }
                            );
                        }
                    }
                    else
                    {
                        if (temFilho)
                        {
                            treeDataSource.Add(
                            new TreeData
                            {
                                SetorSolicitanteId = (Guid)setor.SetorSolicitanteId ,
                                Nome = setor.Nome ,
                                HasChild = true ,
                            }
                            );
                        }
                    }
                }

                ViewData["dataSetor"] = treeDataSource;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "PreencheListaSetores" , error);
                return;
            }
        }

        public void PreencheListaVeiculos()
        {
            try
            {
                var listaVeiculos = (
                from v in _unitOfWork.Veiculo.GetAllReduced(
                filter: v => v.Status != false ,
                includeProperties: nameof(ModeloVeiculo) + "," + nameof(MarcaVeiculo) ,
                selector: v => new
                {
                    v.VeiculoId ,
                    v.Placa ,
                    v.MarcaVeiculo.DescricaoMarca ,
                    v.ModeloVeiculo.DescricaoModelo ,
                }
                )
                select new
                {
                    v.VeiculoId ,
                    Descricao = v.Placa + " - " + v.DescricaoMarca + "/" + v.DescricaoModelo ,
                }
                ).OrderBy(v => v.Descricao);

                var veiculoDataSource = new List<VeiculoData>();

                foreach (var veiculo in listaVeiculos)
                {
                    veiculoDataSource.Add(
                    new VeiculoData
                    {
                        VeiculoId = veiculo.VeiculoId ,
                        Descricao = veiculo.Descricao ,
                    }
                    );
                }

                ViewData["dataVeiculo"] = veiculoDataSource;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "PreencheListaVeiculos" , error);
                return;
            }
        }

        private void SetViewModel()
        {
            try
            {
                ViagemObj = new ViagemViewModel { Viagem = new Models.Viagem() };
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "SetViewModel" , error);
                return;
            }
        }

        /// <summary>
        /// Processa e salva as ocorrências da viagem a partir do JSON recebido do formulário
        /// </summary>
        private int ProcessarOcorrencias(Guid viagemId, Guid? veiculoId, Guid? motoristaId)
        {
            int ocorrenciasCriadas = 0;
            
            try
            {
                if (string.IsNullOrWhiteSpace(OcorrenciasJson))
                {
                    return 0;
                }

                var ocorrencias = JsonSerializer.Deserialize<List<OcorrenciaUpsertDto>>(
                    OcorrenciasJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                if (ocorrencias == null || !ocorrencias.Any())
                {
                    return 0;
                }

                foreach (var oc in ocorrencias)
                {
                    // Só cria se tiver resumo preenchido
                    if (!string.IsNullOrWhiteSpace(oc.Resumo))
                    {
                        var ocorrencia = new OcorrenciaViagem
                        {
                            OcorrenciaViagemId = Guid.NewGuid(),
                            ViagemId = viagemId,
                            VeiculoId = veiculoId ?? Guid.Empty,
                            MotoristaId = motoristaId,
                            Resumo = oc.Resumo ?? "",
                            Descricao = oc.Descricao ?? "",
                            ImagemOcorrencia = oc.ImagemOcorrencia ?? "",
                            Status = "Aberta",
                            DataCriacao = DateTime.Now,
                            UsuarioCriacao = usuarioCorrenteNome ?? ""
                        };
                        
                        _unitOfWork.OcorrenciaViagem.Add(ocorrencia);
                        ocorrenciasCriadas++;
                    }
                }

                if (ocorrenciasCriadas > 0)
                {
                    _unitOfWork.Save();
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs", "ProcessarOcorrencias", error);
            }

            return ocorrenciasCriadas;
        }

        /// <summary>
        /// DTO para deserializar ocorrências do JSON
        /// </summary>
        public class OcorrenciaUpsertDto
        {
            public string Resumo { get; set; }
            public string Descricao { get; set; }
            public string ImagemOcorrencia { get; set; }
        }

        public class ViagemDescricaoInput
        {
            public string DescricaoWord
            {
                get; set;
            }

            public Guid ViagemId
            {
                get; set;
            }
        }

        private class CombustivelData
        {
            public string Descricao
            {
                get; set;
            }

            public string Imagem
            {
                get; set;
            }

            public string Nivel
            {
                get; set;
            }
        }

        private class EventoData
        {
            public Guid EventoId
            {
                get; set;
            }

            public string Nome
            {
                get; set;
            }
        }

        private class FinalidadeData
        {
            public string Descricao
            {
                get; set;
            }

            public string FinalidadeId
            {
                get; set;
            }
        }

        private class MotoristaData
        {
            public string Foto
            {
                get; set;
            }

            public Guid MotoristaId
            {
                get; set;
            }

            public string Nome
            {
                get; set;
            }
        }

        private class RequisicaoData
        {
            public string Descricao
            {
                get; set;
            }

            public Guid RepactuacaoContratoId
            {
                get; set;
            }
        }

        private class RequisitanteData
        {
            public string Requisitante
            {
                get; set;
            }

            public Guid RequisitanteId
            {
                get; set;
            }
        }

        private class TreeData
        {
            public bool Expanded
            {
                get; set;
            }

            public bool HasChild
            {
                get; set;
            }

            public bool IsSelected
            {
                get; set;
            }

            public string Nome
            {
                get; set;
            }

            public Guid SetorPaiId
            {
                get; set;
            }

            public Guid SetorSolicitanteId
            {
                get; set;
            }

            public string Sigla
            {
                get; set;
            }
        }

        private class VeiculoData
        {
            public string Descricao
            {
                get; set;
            }

            public Guid VeiculoId
            {
                get; set;
            }
        }

        /* ****************************************************************************************
         * 🚨 PROBLEMAS IDENTIFICADOS E MELHORIAS PROPOSTAS
         * --------------------------------------------------------------------------------------
         * Data de Identificação: 10/02/2026
         * Identificado por: GitHub Copilot - GPT-5.2-Codex (Análise Automática)
         * Status: 🟡 PENDENTE (aguardando refatoração)
         *
         * ========================================================================================
         * PROBLEMA #1: Estado compartilhado com campos static
         * ----------------------------------------------------------------------------------------
         * Severidade: 🟡 ALTA
         *
         * Descrição:
         * Campos static armazenam dados de requisições e podem vazar entre usuários,
         * causando inconsistências em cenários concorrentes.
         *
         * Localização:
         * - Linhas: 49-59 (campos static no topo da classe)
         *
         * Impacto:
         * - Concorrência: risco de dados cruzados entre sessões
         * - Debugging difícil em ambientes com múltiplos usuários
         *
         * Solução Proposta:
         * Remover static e armazenar estado por request (propriedades de instância,
         * TempData/Session ou reconsulta controlada).
         *
         * ========================================================================================
         * PROBLEMA #2: Max() sem fallback para sequência vazia
         * ----------------------------------------------------------------------------------------
         * Severidade: 🟡 ALTA
         *
         * Descrição:
         * OnGetVerificaFicha usa Max() sem tratar cenário de tabela vazia.
         *
         * Localização:
         * - Linhas: 771-777 (OnGetVerificaFicha)
         *
         * Impacto:
         * - Exceção em ambientes sem registros
         *
         * Solução Proposta:
         * Usar DefaultIfEmpty/FirstOrDefault com valor padrão.
         *
         * ========================================================================================
         * PROBLEMA #3: Possíveis NullReference em endpoints auxiliares
         * ----------------------------------------------------------------------------------------
         * Severidade: 🟠 MÉDIA
         *
         * Descrição:
         * Falta validação de null em consultas de veículo e setor.
         *
         * Localização:
         * - Linhas: 677-684 (OnGetPegaKmAtualVeiculo)
         * - Linhas: 740-749 (OnGetPegaSetor)
         *
         * Impacto:
         * - Exceções quando registros não são encontrados
         *
         * Solução Proposta:
         * Validar retorno antes de acessar propriedades e retornar fallback seguro.
         *
         * ========================================================================================
         * PROBLEMA #4: Strings de lista montadas e não utilizadas
         * ----------------------------------------------------------------------------------------
         * Severidade: 🟢 BAIXA
         *
         * Descrição:
         * Variáveis eventosList/requisitantesList são montadas e não usadas.
         *
         * Localização:
         * - Linhas: 419-507 (OnGetAJAXPreencheListaEventos/Requisitantes)
         *
         * Impacto:
         * - Código morto e risco de exceção quando lista vazia
         *
         * Solução Proposta:
         * Remover montagem de string ou proteger com validação.
         *
         * ========================================================================================
         * OBSERVAÇÕES GERAIS:
         * Este arquivo possui múltiplos endpoints auxiliares e deve manter validações
         * defensivas para evitar erros intermitentes em produção.
         *
         * REFERÊNCIAS:
         * - ArquivosCriticos.md (entrada completa)
         **************************************************************************************** */
    }
}

