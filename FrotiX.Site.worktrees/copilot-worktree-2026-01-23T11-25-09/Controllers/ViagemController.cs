using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Models.DTO;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace FrotiX.Controllers
{
    /*
    *  #################################################################################################
    *  #                                                                                               #
    *  #   ███████╗██████╗  ██████╗ ████████╗██╗██╗  ██╗    ██████╗  ██████╗ ██████╗  ██████╗          #
    *  #   ██╔════╝██╔══██╗██╔═══██╗╚══██╔══╝██║╚██╗██╔╝    ╚════██╗██╔═████╗╚════██╗██╔════╝          #
    *  #   █████╗  ██████╔╝██║   ██║   ██║   ██║ ╚███╔╝      █████╔╝██║██╔██║ █████╔╝███████╗          #
    *  #   ██╔══╝  ██╔══██╗██║   ██║   ██║   ██║ ██╔██╗     ██╔═══╝ ████╔╝██║██╔═══╝ ██╔═══██╗          #
    *  #   ██║     ██║  ██║╚██████╔╝   ██║   ██║██╔╝ ██╗    ███████╗╚██████╔╝███████╗╚██████╔╝          #
    *  #   ╚═╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝╚═╝  ╚═╝    ╚══════╝ ╚═════╝ ╚══════╝ ╚═════╝           #
    *  #                                                                                               #
    *  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                     #
    *  #   MODULO:  CONTROLADOR CENTRAL DE VIAGENS (CORE ENGINE)                                       #
    *  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
    *  #                                                                                               #
    *  #################################################################################################
    */

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: ViagemController                                                  ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Controlador central de viagens (core engine).                             ║
    /// ║    Classe parcial dividida por domínios funcionais.                          ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/Viagem                                                ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public partial class ViagemController : Controller
    {
        private readonly FrotiXDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _log;
        private IWebHostEnvironment hostingEnv;
        private readonly IViagemRepository _viagemRepo;
        private readonly MotoristaFotoService _fotoService;
        private readonly IMemoryCache _cache;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ViagemEstatisticaService _viagemEstatisticaService;
        private readonly VeiculoEstatisticaService _veiculoEstatisticaService;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ViagemController (Construtor)                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Injeta dependências do core engine de viagens.                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • context (FrotiXDbContext): DbContext.                                  ║
        /// ║    • unitOfWork (IUnitOfWork): Unidade de trabalho.                         ║
        /// ║    • viagemRepo (IViagemRepository): Repositório de viagens.                 ║
        /// ║    • webHostEnvironment (IWebHostEnvironment): Ambiente web.                ║
        /// ║    • fotoService (MotoristaFotoService): Serviço de fotos.                  ║
        /// ║    • cache (IMemoryCache): Cache em memória.                                ║
        /// ║    • serviceScopeFactory (IServiceScopeFactory): Escopos DI.                ║
        /// ║    • viagemEstatisticaRepository (IViagemEstatisticaRepository): Estatística.║
        /// ║    • log (ILogService): Serviço de log.                                     ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [ActivatorUtilitiesConstructor]
        public ViagemController(
            FrotiXDbContext context,
            IUnitOfWork unitOfWork,
            IViagemRepository viagemRepo,
            IWebHostEnvironment webHostEnvironment,
            MotoristaFotoService fotoService,
            IMemoryCache cache, 
            IServiceScopeFactory serviceScopeFactory, 
            IViagemEstatisticaRepository viagemEstatisticaRepository,
            ILogService log
        )
        {
            try
            {
                _unitOfWork = unitOfWork;
                _viagemRepo = viagemRepo;
                hostingEnv = webHostEnvironment;
                _fotoService = fotoService;
                _cache = cache;
                _serviceScopeFactory = serviceScopeFactory;
                _context = context;
                _log = log;
                _viagemEstatisticaService = new ViagemEstatisticaService(_context, viagemEstatisticaRepository, unitOfWork);
                _veiculoEstatisticaService = new VeiculoEstatisticaService(_context, cache);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs", "ViagemController", error);
            }
        }

        private static Expression<Func<ViewViagens , bool>> viagemsFilters(
            Guid veiculoId ,
            Guid motoristaId ,
            string dataViagem ,
            string statusId ,
            Guid eventoId
        )
        {
            DateTime? parsedDataViagem = null;

            if (!string.IsNullOrWhiteSpace(dataViagem))
            {
                if (
                    DateTime.TryParseExact(
                        dataViagem ,
                        "dd/MM/yyyy" ,
                        new CultureInfo("pt-BR") ,
                        DateTimeStyles.None ,
                        out var tempDate
                    )
                )
                {
                    parsedDataViagem = tempDate.Date;
                }
            }

            return m =>
                (m.StatusAgendamento == false)
    && (string.IsNullOrEmpty(statusId) || statusId == "Todas" || m.Status == statusId)
    && (motoristaId == Guid.Empty || m.MotoristaId == motoristaId)
    && (veiculoId == Guid.Empty || m.VeiculoId == veiculoId)
    && (
                    parsedDataViagem == null
    || m.DataInicial.HasValue && m.DataInicial.Value.Date == parsedDataViagem.Value
                )
    && (eventoId == Guid.Empty || m.EventoId == eventoId);
        }

        private static Guid GetParsedId(string id)
        {
            Guid parsed = Guid.Empty;
            if (id != null)
            {
                parsed = Guid.Parse(id);
            }
            return parsed;
        }

        // Classe para armazenar informações de progresso
        public class ProgressoCalculoCusto
        {
            public int Total { get; set; }
            public int Processado { get; set; }
            public int Percentual { get; set; }
            public bool Concluido { get; set; }
            public bool Erro { get; set; }
            public string Mensagem { get; set; }
            public DateTime IniciadoEm { get; set; }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   UPLOAD FICHA VISTORIA - SALVA PDF/IMAGEM NO BANCO   |
        * |_______________________________________________________|
        */
        [HttpPost]
        [Route("UploadFichaVistoria")]
        public async Task<IActionResult> UploadFichaVistoria(
            IFormFile arquivo,
            [FromForm] string viagemId
        )
        {
            try
            {
                if (arquivo == null || arquivo.Length == 0)
                {
                    return Json(new
                    {
                        success = false ,
                        message = "Nenhum arquivo foi enviado"
                    });
                }

                if (string.IsNullOrWhiteSpace(viagemId))
                {
                    return Json(
                        new
                        {
                            success = false ,
                            message = "ID da viagem não foi fornecido"
                        }
                    );
                }

                if (!Guid.TryParse(viagemId , out var viagemGuid))
                {
                    return Json(
                        new
                        {
                            success = false ,
                            message = $"ID da viagem inválido: {viagemId}"
                        }
                    );
                }

                var viagem = _unitOfWork.Viagem.GetFirstOrDefault(v => v.ViagemId == viagemGuid);
                if (viagem == null)
                {
                    return Json(
                        new
                        {
                            success = false ,
                            message = $"Viagem não encontrada com ID: {viagemId}" ,
                        }
                    );
                }

                using (var ms = new MemoryStream())
                {
                    await arquivo.CopyToAsync(ms);
                    viagem.FichaVistoria = ms.ToArray();
                }

                var nomeArquivo = arquivo.FileName?.ToLowerInvariant() ?? "";
                var ehFichaPadrao = nomeArquivo.Contains("fichaamarelanova") ||
                                    nomeArquivo.Contains("ficha_amarela") ||
                                    nomeArquivo.Contains("fichapadrao");
                viagem.TemFichaVistoriaReal = !ehFichaPadrao;

                _unitOfWork.Viagem.Update(viagem);
                _unitOfWork.Save();

                _log.Info($"Ficha de vistoria atualizada para a viagem ID: {viagemId}. Arquivo: {nomeArquivo}", "ViagemController", "UploadFichaVistoria");

                var base64 = Convert.ToBase64String(viagem.FichaVistoria);
                return Json(
                    new
                    {
                        success = true ,
                        message = "Ficha de vistoria salva com sucesso" ,
                        imagemBase64 = $"data:image/jpeg;base64,{base64}" ,
                    }
                );
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "UploadFichaVistoria");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "UploadFichaVistoria" , error);
                return Json(
                    new
                    {
                        success = false ,
                        message = $"Erro ao salvar ficha: {error.Message}"
                    }
                );
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   EXISTE FICHA PARA DATA - VERIFICA ECONOMILDO       |
        * |_______________________________________________________|
        */
        [Route("ExisteFichaParaData")]
        public JsonResult ExisteFichaParaData(string data)
        {
            try
            {
                if (!DateTime.TryParseExact(data , "yyyy-MM-dd" , CultureInfo.InvariantCulture , DateTimeStyles.None , out DateTime dataConvertida))
                {
                    return Json(false);
                }

                var existeFicha = _unitOfWork.ViagensEconomildo
                    .GetAll()
                    .Any(v => v.Data.HasValue && v.Data.Value.Date == dataConvertida.Date);

                return Json(existeFicha);
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "ExisteFichaParaData");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ExisteFichaParaData" , error);
                return Json(false);
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   VERIFICA FICHA EXISTE - BUSCA POR NÚMERO            |
        * |_______________________________________________________|
        */
        [Route("VerificaFichaExiste")]
        [HttpGet]
        public IActionResult VerificaFichaExiste(int noFichaVistoria)
        {
            try
            {
                var viagem = _unitOfWork.ViewViagens
                    .GetFirstOrDefault(v => v.NoFichaVistoria == noFichaVistoria);

                if (viagem != null)
                {
                    return Json(new
                    {
                        success = true ,
                        data = new
                        {
                            existe = true ,
                            viagemId = viagem.ViagemId ,
                            fichaId = viagem.NoFichaVistoria
                        }
                    });
                }

                return Json(new { success = true , data = new { existe = false } });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "VerificaFichaExiste");
                return Json(new { success = false , message = error.Message });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   OBTER FICHA VISTORIA - RETORNA IMAGEM EM BASE64     |
        * |_______________________________________________________|
        */
        [HttpGet]
        [Route("ObterFichaVistoria")]
        public IActionResult ObterFichaVistoria(string viagemId)
        {
            try
            {
                if (!Guid.TryParse(viagemId , out var viagemGuid))
                {
                    return Json(new
                    {
                        success = false ,
                        message = "ID inválido"
                    });
                }

                var viagem = _unitOfWork.Viagem.GetFirstOrDefault(v => v.ViagemId == viagemGuid);

                if (viagem == null)
                {
                    return Json(new
                    {
                        success = false ,
                        message = "Viagem não encontrada"
                    });
                }

                if (viagem.FichaVistoria != null && viagem.FichaVistoria.Length > 0)
                {
                    var base64 = Convert.ToBase64String(viagem.FichaVistoria);
                    return Json(
                        new
                        {
                            success = true ,
                            temImagem = true ,
                            imagemBase64 = $"data:image/jpeg;base64,{base64}" ,
                        }
                    );
                }

                return Json(new
                {
                    success = true ,
                    temImagem = false
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "ObterFichaVistoria");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ObterFichaVistoria" , error);
                return Json(new
                {
                    success = false ,
                    message = $"Erro: {error.Message}"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   MONTA DESCRIÇÃO SEM FORMATO - REMOVE HTML TAGS      |
        * |_______________________________________________________|
        */
        [Route("MontaDescricaoSemFormato")]
        [HttpPost]
        public IActionResult MontaDescricaoSemFormato()
        {
            try
            {
                var objViagens = _unitOfWork.Viagem.GetAll(v => v.Descricao != null);

                foreach (var viagem in objViagens)
                {
                    viagem.DescricaoSemFormato = Servicos.ConvertHtml(viagem.Descricao);
                    _unitOfWork.Viagem.Update(viagem);
                }
                _unitOfWork.Save();

                _log.Info("Sincronização de descrições sem formato concluída.", "ViagemController", "MontaDescricaoSemFormato");

                return Json(new
                {
                    success = true
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "MontaDescricaoSemFormato");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "MontaDescricaoSemFormato" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao montar descrição"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   FOTO MOTORISTA - RETORNA FOTO COM CACHE (ETAG)      |
        * |_______________________________________________________|
        */
        [HttpGet]
        [Route("FotoMotorista")]
        public IActionResult FotoMotorista(Guid id)
        {
            try
            {
                var motorista = _unitOfWork.Motorista.GetFirstOrDefault(m => m.MotoristaId == id);

                if (motorista == null || motorista.Foto == null || motorista.Foto.Length == 0)
                {
                    Response.Headers["Cache-Control"] = "public,max-age=600";
                    return Json(new
                    {
                        fotoBase64 = ""
                    });
                }

                string etag;
                using (var sha = SHA256.Create())
                {
                    etag =
                        "\""
                        + Convert.ToBase64String(sha.ComputeHash(motorista.Foto)).TrimEnd('=')
                        + "\"";
                }

                var ifNoneMatch = Request.Headers["If-None-Match"].FirstOrDefault();
                if (
                    !string.IsNullOrEmpty(ifNoneMatch)
        && string.Equals(ifNoneMatch , etag , StringComparison.Ordinal)
                )
                {
                    Response.Headers["ETag"] = etag;
                    Response.Headers["Cache-Control"] = "public,max-age=86400";
                    return StatusCode(StatusCodes.Status304NotModified);
                }

                var base64 = $"data:image/jpeg;base64,{Convert.ToBase64String(motorista.Foto)}";

                Response.Headers["ETag"] = etag;
                Response.Headers["Cache-Control"] = "public,max-age=86400";

                return Json(new
                {
                    fotoBase64 = base64
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "FotoMotorista");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "FotoMotorista" , error);
                return Json(new
                {
                    fotoBase64 = ""
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   PEGAR STATUS VIAGEM - VERIFICA SE ESTÁ ABERTA       |
        * |_______________________________________________________|
        */
        [HttpGet("PegarStatusViagem")]
        public IActionResult PegarStatusViagem(Guid viagemId)
        {
            try
            {
                var viagem = _unitOfWork.Viagem.GetFirstOrDefault(v => v.ViagemId == viagemId);

                if (viagem != null && viagem.Status == "Aberta")
                    return Ok(true);
                else
                    return Ok(false);
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "PegarStatusViagem");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "PegarStatusViagem" , error);
                return StatusCode(500 , $"Erro ao verificar status da viagem: {error.Message}");
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   LISTA DISTINTOS - RETORNA ORIGENS E DESTINOS        |
        * |_______________________________________________________|
        */
        [HttpGet("ListaDistintos")]
        public IActionResult ListaDistintos()
        {
            try
            {
                var origens = _unitOfWork
                    .Viagem.GetAllReduced(
                        selector: v => v.Origem ,
                        filter: v => !string.IsNullOrWhiteSpace(v.Origem)
                    )
                    .Select(o => o?.Trim())
                    .Where(o => !string.IsNullOrEmpty(o))
                    .Distinct()
                    .OrderBy(o => o)
                    .ToList();

                var destinos = _unitOfWork
                    .Viagem.GetAllReduced(
                        selector: v => v.Destino ,
                        filter: v => !string.IsNullOrWhiteSpace(v.Destino)
                    )
                    .Select(d => d?.Trim())
                    .Where(d => !string.IsNullOrEmpty(d))
                    .Distinct()
                    .OrderBy(d => d)
                    .ToList();

                return Ok(new
                {
                    origens ,
                    destinos
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "ListaDistintos");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ListaDistintos" , error);
                return StatusCode(500 , $"Erro ao obter origens/destinos: {error.Message}");
            }
        }

        public class UnificacaoRequest
        {
            public string NovoValor
            {
                get; set;
            }

            public List<string> OrigensSelecionadas
            {
                get; set;
            }

            public List<string> DestinosSelecionados
            {
                get; set;
            }
        }

        [HttpPost]
        [Route("Unificar")]
        public IActionResult Unificar([FromBody] UnificacaoRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.NovoValor))
                    return BadRequest("O novo valor é obrigatório.");

                if (request.OrigensSelecionadas != null && request.OrigensSelecionadas.Any())
                {
                    string Normalize(string text)
                    {
                        if (string.IsNullOrEmpty(text))
                            return string.Empty;
                        text = text.ToLowerInvariant()
                            .Replace("-" , "")
                            .Replace("/" , "")
                            .Replace(" " , "")
                            .Replace("_" , "");
                        text = text.Normalize(System.Text.NormalizationForm.FormD);
                        var chars = text.Where(c =>
                            System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c)
                != System.Globalization.UnicodeCategory.NonSpacingMark
                        );
                        return new string(chars.ToArray());
                    }

                    var origensSelecionadasNormalizadas = request
                        .OrigensSelecionadas.Select(o => Normalize(o))
                        .ToList();

                    var viagensOrigens = _unitOfWork
                        .Viagem.GetAllReduced(selector: v => new { v.ViagemId , v.Origem })
                        .ToList()
                        .Where(v => origensSelecionadasNormalizadas.Contains(Normalize(v.Origem)))
                        .ToList();

                    foreach (var viagem in viagensOrigens)
                    {
                        var entidade = _unitOfWork.Viagem.GetFirstOrDefault(v =>
                            v.ViagemId == viagem.ViagemId
                        );
                        if (entidade != null)
                            entidade.Origem = request.NovoValor;
                        _unitOfWork.Viagem.Update(entidade);
                    }
                    _unitOfWork.Save();
                }

                if (request.DestinosSelecionados != null && request.DestinosSelecionados.Any())
                {
                    string Normalize(string text)
                    {
                        if (string.IsNullOrEmpty(text))
                            return string.Empty;
                        text = text.ToLowerInvariant()
                            .Replace("-" , "")
                            .Replace("/" , "")
                            .Replace(" " , "")
                            .Replace("_" , "");
                        text = text.Normalize(System.Text.NormalizationForm.FormD);
                        var chars = text.Where(c =>
                            System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c)
                != System.Globalization.UnicodeCategory.NonSpacingMark
                        );
                        return new string(chars.ToArray());
                    }

                    var DestinosSelecionadasNormalizadas = request
                        .DestinosSelecionados.Select(o => Normalize(o))
                        .ToList();

                    var viagensDestinos = _unitOfWork
                        .Viagem.GetAllReduced(selector: v => new { v.ViagemId , v.Destino })
                        .ToList()
                        .Where(v => DestinosSelecionadasNormalizadas.Contains(Normalize(v.Destino)))
                        .ToList();

                    foreach (var viagem in viagensDestinos)
                    {
                        var entidade = _unitOfWork.Viagem.GetFirstOrDefault(v =>
                            v.ViagemId == viagem.ViagemId
                        );
                        if (entidade != null)
                            entidade.Destino = request.NovoValor;
                        _unitOfWork.Viagem.Update(entidade);
                    }
                    _unitOfWork.Save();
                }

                _log.Info($"Unificação de endereços concluída. Novo Valor: {request.NovoValor}", "ViagemController", "Unificar");

                return Ok(new
                {
                    mensagem = "Unificação realizada com sucesso."
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "Unificar");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "Unificar" , error);
                return StatusCode(500 , new
                {
                    success = false ,
                    message = "Erro ao unificar"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   GET - LISTAGEM FILTRADA DE VIAGENS                  |
        * |_______________________________________________________|
        */
        [HttpGet]
        public IActionResult Get(
            string veiculoId = null ,
            string motoristaId = null ,
            string statusId = null ,
            string dataViagem = null ,
            string eventoId = null
        )
        {
            try
            {
                var motoristaIdParam = GetParsedId(motoristaId);
                var veiculoIdParam = GetParsedId(veiculoId);
                var eventoIdParam = GetParsedId(eventoId);

                // ✅ OTIMIZAÇÃO: Ordenação e projeção no banco (SQL)
                // Antes: ordenação em memória depois do .ToList()
                // Agora: OrderBy executado como SQL ORDER BY antes de carregar dados
                // AsNoTracking agora é aplicado automaticamente pelo Repository (default = true)
                var result = _unitOfWork.ViewViagens
                    .GetAll(filter: viagemsFilters(
                        veiculoIdParam ,
                        motoristaIdParam ,
                        dataViagem ,
                        statusId ,
                        eventoIdParam
                    ))
                    // Ordenação: NoFichaVistoria = 0 ou null primeiro (para serem preenchidos)
                    // depois por DataInicial DESC, HoraInicio DESC
                    // Registros com NoFichaVistoria > 0 vão depois, ordenados por número DESC
                    .OrderBy(x => x.NoFichaVistoria > 0 ? 1 : 0)  // SQL: CASE WHEN NoFichaVistoria > 0 THEN 1 ELSE 0 END
                    .ThenByDescending(x => x.DataInicial)           // SQL: ORDER BY DataInicial DESC
                    .ThenByDescending(x => x.HoraInicio)            // SQL: ORDER BY HoraInicio DESC
                    .ThenByDescending(x => x.NoFichaVistoria)       // SQL: ORDER BY NoFichaVistoria DESC
                    .Select(x => new
                    {
                        x.CombustivelFinal ,
                        x.CombustivelInicial ,
                        x.DataFinal ,
                        x.DataInicial ,
                        x.Descricao ,
                        x.DescricaoOcorrencia ,
                        x.DescricaoSolucaoOcorrencia ,
                        x.DescricaoVeiculo ,
                        x.Finalidade ,
                        x.HoraFim ,
                        x.HoraInicio ,
                        x.ImagemOcorrencia ,
                        x.KmFinal ,
                        x.KmInicial ,
                        // NoFichaVistoria = 0 retorna como "(mobile)"
                        NoFichaVistoria = x.NoFichaVistoria > 0 ? x.NoFichaVistoria.ToString() : "(mobile)" ,
                        x.NomeMotorista ,
                        x.NomeRequisitante ,
                        x.NomeSetor ,
                        x.ResumoOcorrencia ,
                        x.Status ,
                        x.StatusAgendamento ,
                        x.StatusCartaoAbastecimento ,
                        x.StatusDocumento ,
                        x.StatusOcorrencia ,
                        x.ViagemId ,
                        x.MotoristaId ,
                        x.VeiculoId ,
                    })
                    .ToList(); // UMA ÚNICA chamada ao banco com OrderBy já aplicado

                return Json(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "Get");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "Get" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao carregar viagens"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   APLICAR CORREÇÃO ORIGEM - CORRIGE NOMES EM LOTE     |
        * |_______________________________________________________|
        */
        [HttpPost]
        [Route("AplicarCorrecaoOrigem")]
        public async Task<IActionResult> AplicarCorrecaoOrigem([FromBody] CorrecaoOrigemDto dto)
        {
            try
            {
                if (dto == null || dto.Origens == null || string.IsNullOrWhiteSpace(dto.NovaOrigem))
                    return BadRequest();

                await _viagemRepo.CorrigirOrigemAsync(dto.Origens , dto.NovaOrigem);

                _log.Info($"Correção de Origem aplicada: {dto.NovaOrigem}", "ViagemController", "AplicarCorrecaoOrigem");

                return Ok();
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "AplicarCorrecaoOrigem");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "AplicarCorrecaoOrigem" , error);
                return StatusCode(500);
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   APLICAR CORREÇÃO DESTINO - CORRIGE NOMES EM LOTE    |
        * |_______________________________________________________|
        */
        [HttpPost]
        [Route("AplicarCorrecaoDestino")]
        public async Task<IActionResult> AplicarCorrecaoDestino([FromBody] CorrecaoDestinoDto dto)
        {
            try
            {
                if (dto == null || dto.Destinos == null || string.IsNullOrWhiteSpace(dto.NovoDestino))
                    return BadRequest();

                await _viagemRepo.CorrigirDestinoAsync(dto.Destinos , dto.NovoDestino);

                _log.Info($"Correção de Destino aplicada: {dto.NovoDestino}", "ViagemController", "AplicarCorrecaoDestino");

                return Ok();
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "AplicarCorrecaoDestino");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "AplicarCorrecaoDestino" , error);
                return StatusCode(500);
            }
        }

        public class CorrecaoOrigemDto
        {
            public List<string> Origens
            {
                get; set;
            }

            public string NovaOrigem
            {
                get; set;
            }
        }

        public class CorrecaoDestinoDto
        {
            public List<string> Destinos
            {
                get; set;
            }

            public string NovoDestino
            {
                get; set;
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   FLUXO FILTRADO - LISTAGEM PARA ECONOMILDO           |
        * |_______________________________________________________|
        */
        [Route("FluxoFiltrado")]
        [HttpGet]
        public IActionResult FluxoFiltrado(string veiculoId , string motoristaId , string dataFluxo)
        {
            try
            {
                var query = _unitOfWork
                    .ViewFluxoEconomildo.GetAllReduced(
                        filter: null ,
                        selector: vf => new
                        {
                            vf.ViagemEconomildoId ,
                            vf.MotoristaId ,
                            vf.VeiculoId ,
                            vf.NomeMotorista ,
                            vf.DescricaoVeiculo ,
                            vf.MOB ,
                            vf.Data ,
                            vf.HoraInicio ,
                            vf.HoraFim ,
                            vf.QtdPassageiros ,
                        }
                    )
                    .AsQueryable();

                if (!string.IsNullOrEmpty(veiculoId))
                {
                    if (Guid.TryParse(veiculoId , out var veiculoGuid))
                    {
                        query = query.Where(vf => vf.VeiculoId == veiculoGuid);
                    }
                }

                if (!string.IsNullOrEmpty(motoristaId))
                {
                    if (Guid.TryParse(motoristaId , out var motoristaGuid))
                    {
                        query = query.Where(vf => vf.MotoristaId == motoristaGuid);
                    }
                }

                if (!string.IsNullOrEmpty(dataFluxo))
                {
                    if (DateTime.TryParse(dataFluxo , out var dataConvertida))
                    {
                        query = query.Where(vf => vf.Data == dataConvertida);
                    }
                }

                var resultado = query
                    .Select(x => new
                    {
                        x.ViagemEconomildoId ,
                        x.MotoristaId ,
                        x.VeiculoId ,
                        x.NomeMotorista ,
                        x.DescricaoVeiculo ,
                        x.MOB ,
                        DataFluxo = x.Data.HasValue ? x.Data.Value.ToString("dd/MM/yyyy") : "" ,
                        x.HoraInicio ,
                        x.HoraFim ,
                        x.QtdPassageiros ,
                    })
                    .ToList();

                return Json(new
                {
                    data = resultado
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "FluxoFiltrado");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "FluxoFiltrado" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao filtrar fluxo"
                });
            }
        }

        ///// ╔══════════════════════════════════════════════════════════════════════════════╗
        ///// ║ 📌 NOME: ListaEventos (GET)                                             ║
        ///// ╠══════════════════════════════════════════════════════════════════════════════╣
        ///// ║ 📝 DESCRIÇÃO:                                                              ║
        ///// ║    Lista eventos com custo de viagem calculado (legado).                  ║
        ///// ╠══════════════════════════════════════════════════════════════════════════════╣
        ///// ║ 🔒 STATUS: BLOCO LEGADO COMENTADO                                          ║
        ///// ╚══════════════════════════════════════════════════════════════════════════════╝
        //[HttpGet]
        //[Route("ListaEventos")]
        //public IActionResult ListaEventos()
        //{
        //    var swTotal = System.Diagnostics.Stopwatch.StartNew();

        //    try
        //    {
        //        // ✅ Usa ViewViagens.CustoViagem (mesmo cálculo do Modal)
        //        // Filtra apenas viagens Realizadas
        //        var custosPorEvento = _unitOfWork.ViewViagens
        //            .GetAll(filter: v => v.EventoId != null &&
        //                                v.EventoId != Guid.Empty &&
        //                                v.Status == "Realizada")
        //            .Select(v => new { v.EventoId , v.CustoViagem })
        //            .ToList()
        //            .GroupBy(v => v.EventoId)
        //            .ToDictionary(
        //                g => g.Key ?? Guid.Empty ,
        //                g => g.Sum(v => v.CustoViagem ?? 0)
        //            );

        //        Console.WriteLine($"[ListaEventos] Custos: {swTotal.ElapsedMilliseconds}ms");

        //        // Busca eventos com Include (traz Setor e Requisitante em 1 query só)
        //        var eventos = _context.Evento
        //            .Include(e => e.SetorSolicitante)
        //            .Include(e => e.Requisitante)
        //            .ToList();

        //        Console.WriteLine($"[ListaEventos] Eventos: {swTotal.ElapsedMilliseconds}ms");

        //        // Monta resultado
        //        var resultado = eventos.Select(e =>
        //        {
        //            string nomeSetor = "";
        //            if (e.SetorSolicitante != null)
        //            {
        //                nomeSetor = !string.IsNullOrEmpty(e.SetorSolicitante.Sigla)
        //                    ? $"{e.SetorSolicitante.Nome} ({e.SetorSolicitante.Sigla})"
        //                    : e.SetorSolicitante.Nome ?? "";
        //            }

        //            custosPorEvento.TryGetValue(e.EventoId , out double custoViagem);

        //            return new
        //            {
        //                eventoId = e.EventoId ,
        //                nome = e.Nome ?? "" ,
        //                descricao = e.Descricao ?? "" ,
        //                dataInicial = e.DataInicial ,
        //                dataFinal = e.DataFinal ,
        //                qtdParticipantes = e.QtdParticipantes ?? 0 ,
        //                status = e.Status == "1" ? 1 : 0 ,
        //                nomeSetor = nomeSetor ,
        //                nomeRequisitante = e.Requisitante?.Nome ?? "" ,
        //                nomeRequisitanteHTML = e.Requisitante?.Nome ?? "" ,
        //                custoViagem = custoViagem
        //            };
        //        }).ToList();

        //        swTotal.Stop();
        //        Console.WriteLine($"[ListaEventos] TOTAL: {swTotal.ElapsedMilliseconds}ms - {resultado.Count} eventos");

        //        return Json(new { data = resultado });
        //    }
        //    catch (Exception error)
        //    {
        //        swTotal.Stop();
        //        Alerta.TratamentoErroComLinha("ViagemController.cs" , "ListaEventos" , error);
        //        return Json(new
        //        {
        //            success = false ,
        //            message = "Erro ao listar eventos" ,
        //            data = new List<object>()
        //        });
        //    }
        //}

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   UPDATE STATUS EVENTO - ATIVA/DESATIVA EVENTO        |
        * |_______________________________________________________|
        */
        [Route("UpdateStatusEvento")]
        public JsonResult UpdateStatusEvento(Guid Id)
        {
            try
            {
                if (Id != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Evento.GetFirstOrDefault(u => u.EventoId == Id);
                    string Description = "";
                    int type = 0;

                    if (objFromDb != null)
                    {
                        if (objFromDb.Status == "1")
                        {
                            objFromDb.Status = "0";
                            Description = string.Format(
                                "Atualizado Status do Evento [Nome: {0}] (Inativo)" ,
                                objFromDb.Nome
                            );
                            type = 1;
                        }
                        else
                        {
                            objFromDb.Status = "1";
                            Description = string.Format(
                                "Atualizado Status do Evento  [Nome: {0}] (Ativo)" ,
                                objFromDb.Nome
                            );
                            type = 0;
                        }
                        _unitOfWork.Evento.Update(objFromDb);
                        _unitOfWork.Save();

                        _log.Warning(Description, "ViagemController", "UpdateStatusEvento");
                    }
                    return Json(
                        new
                        {
                            success = true ,
                            message = Description ,
                            type = type ,
                        }
                    );
                }
                return Json(new
                {
                    success = false
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "UpdateStatusEvento");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "UpdateStatusEvento" , error);
                return Json(new
                {
                    success = false
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   APAGA EVENTO - REMOVE EVENTO DO BANCO               |
        * |_______________________________________________________|
        */
        [Route("ApagaEvento")]
        [HttpGet]
        public IActionResult ApagaEvento(string eventoId)
        {
            try
            {
                var objFromDb = _unitOfWork.Evento.GetFirstOrDefault(u =>
                    u.EventoId == Guid.Parse(eventoId)
                );
                if (objFromDb != null)
                {
                    var objViagem = _unitOfWork.Viagem.GetFirstOrDefault(u =>
                        u.EventoId == Guid.Parse(eventoId)
                    );
                    if (objViagem != null)
                    {
                        return Json(
                            new
                            {
                                success = false ,
                                message = "Não foi possível remover o Evento. Ele está associado a uma ou mais viagens!" ,
                            }
                        );
                    }

                    _unitOfWork.Evento.Remove(objFromDb);
                    _unitOfWork.Save();

                    _log.Warning($"Evento removido: {objFromDb.Nome}", "ViagemController", "ApagaEvento");

                    return Json(new
                    {
                        success = true ,
                        message = "Evento removido com sucesso!"
                    });
                }

                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar Evento!"
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "ApagaEvento");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ApagaEvento" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao deletar evento"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   VIAGEM EVENTOS - LISTAGEM DE VIAGENS DE EVENTOS     |
        * |_______________________________________________________|
        */
        [Route("ViagemEventos")]
        [HttpGet]
        public IActionResult ViagemEventos()
        {
            try
            {
                return Json(
                    new
                    {
                        data = _unitOfWork
                            .ViewViagens.GetAll()
                            .Where(v => v.Finalidade == "Evento" && v.StatusAgendamento == false) ,
                    }
                );
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "ViagemEventos");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ViagemEventos" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao carregar viagens de eventos"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   FLUXO - LISTAGEM GERAL PARA ECONOMILDO             |
        * |_______________________________________________________|
        */
        [Route("Fluxo")]
        [HttpGet]
        public IActionResult Fluxo()
        {
            try
            {
                var objFluxo = _unitOfWork
                    .ViewFluxoEconomildo.GetAllReduced(selector: vf => new
                    {
                        vf.ViagemEconomildoId ,
                        vf.MotoristaId ,
                        vf.VeiculoId ,
                        vf.NomeMotorista ,
                        vf.DescricaoVeiculo ,
                        vf.MOB ,
                        DataFluxo = DateTime.Parse(vf.Data.ToString()).ToString("dd/MM/yyyy") ,
                        vf.HoraInicio ,
                        vf.HoraFim ,
                        vf.QtdPassageiros ,
                    })
                    .ToList();

                return Json(new
                {
                    data = objFluxo
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "Fluxo");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "Fluxo" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao carregar fluxo"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   FLUXO VEICULOS - FILTRA FLUXO POR VEICULO           |
        * |_______________________________________________________|
        */
        [Route("FluxoVeiculos")]
        [HttpGet]
        public IActionResult FluxoVeiculos(string Id)
        {
            try
            {
                var objFluxo = _unitOfWork
                    .ViewFluxoEconomildo.GetAllReduced(selector: vf => new
                    {
                        vf.ViagemEconomildoId ,
                        vf.MotoristaId ,
                        vf.VeiculoId ,
                        vf.NomeMotorista ,
                        vf.DescricaoVeiculo ,
                        vf.MOB ,
                        DataFluxo = DateTime.Parse(vf.Data.ToString()).ToString("dd/MM/yyyy") ,
                        vf.HoraInicio ,
                        vf.HoraFim ,
                        vf.QtdPassageiros ,
                    })
                    .Where(vf => vf.VeiculoId == Guid.Parse(Id))
                    .ToList();

                return Json(new
                {
                    data = objFluxo
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "FluxoVeiculos");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "FluxoVeiculos" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao carregar fluxo de veículos"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   FLUXO MOTORISTAS - FILTRA FLUXO POR MOTORISTA       |
        * |_______________________________________________________|
        */
        [Route("FluxoMotoristas")]
        [HttpGet]
        public IActionResult FluxoMotoristas(string Id)
        {
            try
            {
                var objFluxo = _unitOfWork
                    .ViewFluxoEconomildo.GetAllReduced(selector: vf => new
                    {
                        vf.ViagemEconomildoId ,
                        vf.MotoristaId ,
                        vf.VeiculoId ,
                        vf.NomeMotorista ,
                        vf.DescricaoVeiculo ,
                        vf.MOB ,
                        DataFluxo = DateTime.Parse(vf.Data.ToString()).ToString("dd/MM/yyyy") ,
                        vf.HoraInicio ,
                        vf.HoraFim ,
                        vf.QtdPassageiros ,
                    })
                    .Where(vf => vf.MotoristaId == Guid.Parse(Id))
                    .ToList();

                return Json(new
                {
                    data = objFluxo
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "FluxoMotoristas");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "FluxoMotoristas" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao carregar fluxo de motoristas"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   FLUXO DATA - FILTRA FLUXO POR DATA ESPECÍFICA       |
        * |_______________________________________________________|
        */
        [Route("FluxoData")]
        [HttpGet]
        public IActionResult FluxoData(string Id)
        {
            try
            {
                var dataFluxo = DateTime.Parse(Id);

                var objFluxo = _unitOfWork
                    .ViewFluxoEconomildoData.GetAllReduced(selector: vf => new
                    {
                        vf.ViagemEconomildoId ,
                        vf.MotoristaId ,
                        vf.VeiculoId ,
                        vf.NomeMotorista ,
                        vf.DescricaoVeiculo ,
                        vf.MOB ,
                        vf.Data ,
                        DataFluxo = DateTime.Parse(vf.Data.ToString()).ToString("dd/MM/yyyy") ,
                        vf.HoraInicio ,
                        vf.HoraFim ,
                        vf.QtdPassageiros ,
                    })
                    .Where(vf => vf.Data == dataFluxo)
                    .ToList();

                return Json(new
                {
                    data = objFluxo
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "FluxoData");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "FluxoData" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao carregar fluxo por data"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   APAGA FLUXO ECONOMILDO - REMOVE REGISTRO DE FLUXO   |
        * |_______________________________________________________|
        */
        [Route("ApagaFluxoEconomildo")]
        [HttpPost]
        public IActionResult ApagaFluxoEconomildo([FromBody] ViagensEconomildo viagensEconomildo)
        {
            try
            {
                if (viagensEconomildo == null || viagensEconomildo.ViagemEconomildoId == Guid.Empty)
                {
                    return Json(new
                    {
                        success = false,
                        message = "ID da viagem não informado"
                    });
                }

                var objFromDb = _unitOfWork.ViagensEconomildo.GetFirstOrDefault(v =>
                    v.ViagemEconomildoId == viagensEconomildo.ViagemEconomildoId
                );

                if (objFromDb == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Registro não encontrado"
                    });
                }

                _unitOfWork.ViagensEconomildo.Remove(objFromDb);
                _unitOfWork.Save();

                _log.Warning($"Registro de fluxo Economildo removido ID: {objFromDb.ViagemEconomildoId}", "ViagemController", "ApagaFluxoEconomildo");

                return Json(new
                {
                    success = true,
                    message = "Viagem apagada com sucesso"
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "ApagaFluxoEconomildo");
                Alerta.TratamentoErroComLinha("ViagemController.cs", "ApagaFluxoEconomildo", error);
                return Json(new
                {
                    success = false,
                    message = "Erro ao apagar viagem"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   MY UPLOADER - UPLOAD DE FICHA DE VISTORIA           |
        * |_______________________________________________________|
        */
        [Route("MyUploader")]
        [HttpPost]
        public IActionResult MyUploader(IFormFile MyUploader , [FromForm] string ViagemId)
        {
            try
            {
                if (MyUploader != null)
                {
                    var viagemObj = _unitOfWork.Viagem.GetFirstOrDefault(v =>
                        v.ViagemId == Guid.Parse(ViagemId)
                    );
                    using (var ms = new MemoryStream())
                    {
                        MyUploader.CopyTo(ms);
                        viagemObj.FichaVistoria = ms.ToArray();
                    }

                    _unitOfWork.Viagem.Update(viagemObj);
                    _unitOfWork.Save();

                    _log.Info($"Ficha de Vistoria via MyUploader salva para Viagem ID: {ViagemId}", "ViagemController", "MyUploader");

                    return new ObjectResult(new
                    {
                        status = "success"
                    });
                }
                return new ObjectResult(new
                {
                    status = "fail"
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "MyUploader");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "MyUploader" , error);
                return new ObjectResult(new
                {
                    status = "fail"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   CALCULA CUSTO VIAGENS - ACIONA CÁLCULO EM LOTE      |
        * |_______________________________________________________|
        */
        [Route("CalculaCustoViagens")]
        [HttpPost]
        public IActionResult CalculaCustoViagens()
        {
            try
            {
                var cacheKey = "ProgressoCalculoCusto";

                // Verifica se já existe um processamento em andamento
                if (_cache.TryGetValue(cacheKey , out ProgressoCalculoCusto progressoExistente))
                {
                    if (!progressoExistente.Concluido && !progressoExistente.Erro)
                    {
                        return Json(new
                        {
                            success = false ,
                            message = "Já existe um processamento em andamento. Aguarde a conclusão."
                        });
                    }
                }

                // Inicia o processamento em background
                Task.Run(async () => await ProcessarCalculoCustoViagens());

                _log.Warning("Processamento de cálculo de custos iniciado pelo usuário.", "ViagemController", "CalculaCustoViagens");

                return Json(new
                {
                    success = true ,
                    message = "Processamento iniciado com sucesso"
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "CalculaCustoViagens");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "CalculaCustoViagens" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao iniciar cálculo de custos"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   PROCESSAR CÁLCULO CUSTO VIAGENS - BACKGROUND TASK   |
        * |_______________________________________________________|
        */
        private async Task ProcessarCalculoCustoViagens()
        {
            var cacheKey = "ProgressoCalculoCusto";
            var progresso = new ProgressoCalculoCusto
            {
                Total = 0 ,
                Processado = 0 ,
                Percentual = 0 ,
                Concluido = false ,
                Erro = false ,
                Mensagem = "Inicializando..." ,
                IniciadoEm = DateTime.Now
            };

            try
            {
                // Armazena progresso inicial no cache (30 minutos)
                _cache.Set(cacheKey, progresso, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30), Size = 1 });

                // CRÍTICO: Criar um novo scope para ter um novo DbContext
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    // Resolve um novo IUnitOfWork deste scope
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    // Busca todas as viagens a processar
                    var objViagens = unitOfWork.Viagem.GetAll(v =>
                        v.StatusAgendamento == false
                        && v.Status == "Realizada"
                        && (
                            v.Finalidade != "Manutenção"
                            && v.Finalidade != "Devolução à Locadora"
                            && v.Finalidade != "Recebimento da Locadora"
                            && v.Finalidade != "Saída para Manutenção"
                            && v.Finalidade != "Chegada da Manutenção"
                        )
                        && v.NoFichaVistoria != null
                    ).ToList(); // Converte para lista

                    progresso.Total = objViagens.Count;
                    progresso.Mensagem = $"Processando {progresso.Total} viagens...";
                    _cache.Set(cacheKey, progresso, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30), Size = 1 });

                    int contador = 0;

                    foreach (var viagem in objViagens)
                    {
                        try
                        {
                            if (viagem.MotoristaId != null)
                            {
                                int minutos = -1;
                                viagem.CustoMotorista = Servicos.CalculaCustoMotorista(
                                    viagem ,
                                    unitOfWork ,  // ← Usar o unitOfWork do scope
                                    ref minutos
                                );
                                viagem.Minutos = minutos;
                            }
                            if (viagem.VeiculoId != null)
                            {
                                viagem.CustoVeiculo = Servicos.CalculaCustoVeiculo(viagem , unitOfWork);
                                viagem.CustoCombustivel = Servicos.CalculaCustoCombustivel(viagem , unitOfWork);
                            }

                            viagem.CustoOperador = Servicos.CalculaCustoOperador(viagem , unitOfWork);
                            viagem.CustoLavador = Servicos.CalculaCustoLavador(viagem , unitOfWork);

                            unitOfWork.Viagem.Update(viagem);
                            unitOfWork.Save();

                            // Atualiza progresso
                            contador++;
                            progresso.Processado = contador;
                            progresso.Percentual = progresso.Total > 0
                                ? (int)((contador * 100.0) / progresso.Total)
                                : 0;
                            progresso.Mensagem = $"Processando viagem {contador} de {progresso.Total}...";

                            _cache.Set(cacheKey, progresso, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30), Size = 1 });

                            // Pequeno delay para não sobrecarregar (opcional)
                            if (contador % 10 == 0)
                            {
                                await Task.Delay(50);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log do erro mas continua processando as outras viagens
                            Console.WriteLine($"Erro ao processar viagem {viagem.ViagemId}: {ex.Message}");
                        }
                    }

                    // Finaliza com sucesso
                    progresso.Concluido = true;
                    progresso.Percentual = 100;
                    progresso.Mensagem = $"Processamento concluído! {contador} viagens atualizadas.";
                    _cache.Set(cacheKey, progresso, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30), Size = 1 });

                    _log.Info("Cálculo de custos em lote finalizado com sucesso.", "ViagemController", "ProcessarCalculoCustoViagens");
                } // ← O scope é disposed aqui automaticamente
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "ProcessarCalculoCustoViagens");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ProcessarCalculoCustoViagens" , error);

                progresso.Erro = true;
                progresso.Concluido = true;
                progresso.Mensagem = $"Erro durante o processamento: {error.Message}";
                _cache.Set(cacheKey, progresso, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30), Size = 1 });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   OBTER PROGRESSO CÁLCULO CUSTO - POLLING PROGRESS    |
        * |_______________________________________________________|
        */
        [Route("ObterProgressoCalculoCusto")]
        [HttpGet]
        public IActionResult ObterProgressoCalculoCusto()
        {
            try
            {
                var cacheKey = "ProgressoCalculoCusto";

                if (_cache.TryGetValue(cacheKey , out ProgressoCalculoCusto progresso))
                {
                    return Json(new
                    {
                        success = true ,
                        progresso = new
                        {
                            total = progresso.Total ,
                            processado = progresso.Processado ,
                            percentual = progresso.Percentual ,
                            concluido = progresso.Concluido ,
                            erro = progresso.Erro ,
                            mensagem = progresso.Mensagem
                        }
                    });
                }

                // Não há processamento em andamento
                return Json(new
                {
                    success = true ,
                    progresso = new
                    {
                        total = 0 ,
                        processado = 0 ,
                        percentual = 0 ,
                        concluido = false ,
                        erro = false ,
                        mensagem = "Nenhum processamento em andamento"
                    }
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "ObterProgressoCalculoCusto");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ObterProgressoCalculoCusto" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao obter progresso"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   LIMPAR PROGRESSO CÁLCULO CUSTO - RESET CACHE        |
        * |_______________________________________________________|
        */
        [Route("LimparProgressoCalculoCusto")]
        [HttpPost]
        public IActionResult LimparProgressoCalculoCusto()
        {
            try
            {
                var cacheKey = "ProgressoCalculoCusto";
                _cache.Remove(cacheKey);

                return Json(new
                {
                    success = true ,
                    message = "Progresso limpo com sucesso"
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "LimparProgressoCalculoCusto");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "LimparProgressoCalculoCusto" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao limpar progresso"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   VIAGEM VEICULOS - LISTA VIAGENS POR VEICULO         |
        * |_______________________________________________________|
        */
        [Route("ViagemVeiculos")]
        [HttpGet]
        public IActionResult ViagemVeiculos(Guid Id)
        {
            try
            {
                var viagens = _unitOfWork.ViewViagens.GetAllReduced(
                    selector: x => new { x.VeiculoId , x.StatusAgendamento } ,
                    filter: x => x.VeiculoId == Id && x.StatusAgendamento == false
                );
                return Ok(viagens);
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "ViagemVeiculos");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ViagemVeiculos" , error);
                return StatusCode(500);
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   VIAGEM MOTORISTAS - LISTA VIAGENS POR MOTORISTA     |
        * |_______________________________________________________|
        */
        [Route("ViagemMotoristas")]
        [HttpGet]
        public IActionResult ViagemMotoristas(Guid Id)
        {
            try
            {
                var viagens = _unitOfWork.ViewViagens.GetAllReduced(
                    selector: x => new { x.MotoristaId , x.StatusAgendamento } ,
                    filter: x => x.MotoristaId == Id && x.StatusAgendamento == false
                );
                return Ok(viagens);
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "ViagemMotoristas");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ViagemMotoristas" , error);
                return StatusCode(500);
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   VIAGEM STATUS - LISTA VIAGENS POR STATUS            |
        * |_______________________________________________________|
        */
        [Route("ViagemStatus")]
        [HttpGet]
        public IActionResult ViagemStatus(string Id)
        {
            try
            {
                var viagens = _unitOfWork.ViewViagens.GetAllReduced(
                    selector: x => new { x.Status , x.StatusAgendamento } ,
                    filter: x => x.Status == Id && x.StatusAgendamento == false
                );
                return Ok(viagens);
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "ViagemStatus");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ViagemStatus" , error);
                return StatusCode(500);
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   VIAGEM SETORES - LISTA VIAGENS POR SETOR            |
        * |_______________________________________________________|
        */
        [Route("ViagemSetores")]
        [HttpGet]
        public IActionResult ViagemSetores(Guid Id)
        {
            try
            {
                var viagens = _unitOfWork.ViewViagens.GetAllReduced(
                    selector: x => new { x.SetorSolicitanteId , x.StatusAgendamento } ,
                    filter: x => x.SetorSolicitanteId == Id && x.StatusAgendamento == false
                );
                return Ok(viagens);
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "ViagemSetores");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ViagemSetores" , error);
                return StatusCode(500);
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   VIAGEM DATA - LISTA VIAGENS POR DATA INICIAL        |
        * |_______________________________________________________|
        */
        [Route("ViagemData")]
        [HttpGet]
        public IActionResult ViagemData(string Id)
        {
            try
            {
                var viagens = _unitOfWork.ViewViagens.GetAllReduced(
                    selector: x => new { x.DataInicial , x.StatusAgendamento } ,
                    filter: x => x.DataInicial.ToString().Contains(Id) && x.StatusAgendamento == false
                );
                return Ok(viagens);
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "ViagemData");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ViagemData" , error);
                return StatusCode(500);
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   OCORRENCIAS - DETALHES DE OCORRENCIAS DA VIAGEM     |
        * |_______________________________________________________|
        */
        [Route("Ocorrencias")]
        [HttpGet]
        public IActionResult Ocorrencias(Guid Id)
        {
            try
            {
                var ocorrencias = _unitOfWork.ViewViagens.GetAllReduced(
                    selector: x => new { x.ResumoOcorrencia , x.DescricaoOcorrencia } ,
                    filter: x => x.ViagemId == Id
                );
                return Ok(ocorrencias);
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "Ocorrencias");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "Ocorrencias" , error);
                return StatusCode(500);
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   CANCELAR - CANCELA UMA VIAGEM AGENDADA OU EM CURSO  |
        * |_______________________________________________________|
        */
        [Route("Cancelar")]
        [HttpPost]
        public IActionResult Cancelar(ViagemID id)
        {
            try
            {
                var objFromDb = _unitOfWork.Viagem.GetFirstOrDefault(u => u.ViagemId == id.ViagemId);
                if (objFromDb != null)
                {
                    System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                    var currentUserID = currentUser.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value;
                    objFromDb.UsuarioIdCancelamento = currentUserID;
                    objFromDb.DataCancelamento = DateTime.Now;

                    objFromDb.Status = "Cancelada";
                    _unitOfWork.Viagem.Update(objFromDb);
                    _unitOfWork.Save();

                    _log.Warning($"Viagem cancelada. ID: {id.ViagemId}, Usuário: {currentUserID}", "ViagemController", "Cancelar");

                    return Json(new
                    {
                        success = true ,
                        message = "Viagem cancelada com sucesso"
                    });
                }
                return Json(new
                {
                    success = false ,
                    message = "Erro ao cancelar Viagem"
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "Cancelar");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "Cancelar" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao cancelar viagem"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   PEGA FICHA - RETORNA BYTES DA FICHA DE VISTORIA     |
        * |_______________________________________________________|
        */
        [HttpGet]
        [Route("PegaFicha")]
        public JsonResult PegaFicha([FromQuery] Guid id)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Viagem.GetFirstOrDefault(u => u.ViagemId == id);

                    if (objFromDb != null && objFromDb.FichaVistoria != null)
                    {
                        objFromDb.FichaVistoria = this.GetImage(
                            Convert.ToBase64String(objFromDb.FichaVistoria)
                        );
                        return Json(objFromDb);
                    }

                    return Json(false);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "PegaFicha");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "PegaFicha" , error);
                return Json(false);
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   ADICIONAR VIAGENS ECONOMILDO - INSERT INDIVIDUAL    |
        * |_______________________________________________________|
        */
        [Route("AdicionarViagensEconomildo")]
        [Consumes("application/json")]
        public JsonResult AdicionarViagensEconomildo([FromBody] ViagensEconomildo viagensEconomildo)
        {
            try
            {
                _unitOfWork.ViagensEconomildo.Add(viagensEconomildo);
                _unitOfWork.Save();

                _log.Info($"Viagem Economildo adicionada. ID: {viagensEconomildo.ViagemEconomildoId}", "ViagemController", "AdicionarViagensEconomildo");

                return Json(new
                {
                    success = true ,
                    message = "Viagem Adicionada com Sucesso!"
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "AdicionarViagensEconomildo");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "AdicionarViagensEconomildo" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao adicionar viagem"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   EXISTE DATA ECONOMILDO - VALIDA DUPLICIDADE         |
        * |_______________________________________________________|
        */
        [Route("ExisteDataEconomildo")]
        [Consumes("application/json")]
        public JsonResult ExisteDataEconomildo([FromBody] ViagensEconomildo viagensEconomildo)
        {
            try
            {
                viagensEconomildo.Data = viagensEconomildo.Data;

                if (viagensEconomildo.Data != null)
                {
                    var existeData = _unitOfWork.ViagensEconomildo.GetFirstOrDefault(u =>
                        u.Data == viagensEconomildo.Data
            && u.VeiculoId == viagensEconomildo.VeiculoId
            && u.MOB == viagensEconomildo.MOB
            && u.MotoristaId == viagensEconomildo.MotoristaId
                    );

                    if (existeData != null)
                    {
                        return Json(
                            new
                            {
                                success = false ,
                                message = "Já existe registro para essa data!"
                            }
                        );
                    }
                }

                return Json(new
                {
                    success = true ,
                    message = ""
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "ExisteDataEconomildo");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ExisteDataEconomildo" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao verificar data"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   PEGA FICHA MODAL - RETORNA FICHA PARA O MODAL       |
        * |_______________________________________________________|
        */
        [HttpGet]
        [Route("PegaFichaModal")]
        public JsonResult PegaFichaModal(Guid id)
        {
            try
            {
                var objFromDb = _unitOfWork.Viagem.GetFirstOrDefault(u => u.ViagemId == id);
                if (objFromDb.FichaVistoria != null)
                {
                    objFromDb.FichaVistoria = this.GetImage(
                        Convert.ToBase64String(objFromDb.FichaVistoria)
                    );
                    return Json(objFromDb.FichaVistoria);
                }
                return Json(false);
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "PegaFichaModal");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "PegaFichaModal" , error);
                return Json(false);
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   PEGA CATEGORIA - RETORNA CATEGORIA DO VEÍCULO       |
        * |_______________________________________________________|
        */
        [HttpGet]
        [Route("PegaCategoria")]
        public JsonResult PegaCategoria(Guid id)
        {
            try
            {
                var objFromDb = _unitOfWork.Veiculo.GetFirstOrDefault(v => v.VeiculoId == id);
                if (objFromDb.Categoria != null)
                {
                    return Json(objFromDb.Categoria);
                }
                return Json(false);
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "PegaCategoria");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "PegaCategoria" , error);
                return Json(false);
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   ADICIONAR ECONOMILDO LOTE - INSERT EM MASSA         |
        * |_______________________________________________________|
        */
        [Route("AdicionarViagensEconomildoLote")]
        [Consumes("application/json")]
        public JsonResult AdicionarViagensEconomildoLote([FromBody] List<ViagensEconomildo> viagens)
        {
            try
            {
                if (viagens == null || viagens.Count == 0)
                {
                    return Json(new
                    {
                        success = false ,
                        message = "Nenhuma viagem foi informada."
                    });
                }

                // Adicionar todas as viagens de uma vez
                foreach (var viagem in viagens)
                {
                    _unitOfWork.ViagensEconomildo.Add(viagem);
                }

                // Salvar tudo de uma vez - evita múltiplas transações
                _unitOfWork.Save();

                _log.Info($"{viagens.Count} viagens Economildo adicionadas em lote.", "ViagemController", "AdicionarViagensEconomildoLote");

                return Json(new
                {
                    success = true ,
                    message = $"{viagens.Count} viagem(ns) adicionada(s) com sucesso!"
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "AdicionarViagensEconomildoLote");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "AdicionarViagensEconomildoLote" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao adicionar viagens: " + error.Message
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   BUSCAR VIAGEM ECONOMILDO - BUSCA POR ID             |
        * |_______________________________________________________|
        */
        [Route("BuscarViagemEconomildo")]
        public JsonResult BuscarViagemEconomildo(Guid id)
        {
            try
            {
                var viagem = _unitOfWork.ViagensEconomildo.GetFirstOrDefault(
                    v => v.ViagemEconomildoId == id
                );

                if (viagem == null)
                {
                    return Json(new
                    {
                        success = false ,
                        message = "Viagem não encontrada."
                    });
                }

                return Json(new
                {
                    success = true ,
                    data = new
                    {
                        viagemEconomildoId = viagem.ViagemEconomildoId ,
                        data = viagem.Data?.ToString("yyyy-MM-dd") ,
                        veiculoId = viagem.VeiculoId ,
                        motoristaId = viagem.MotoristaId ,
                        mob = viagem.MOB ,
                        responsavel = viagem.Responsavel ,
                        idaVolta = viagem.IdaVolta ,
                        horaInicio = viagem.HoraInicio ,
                        horaFim = viagem.HoraFim ,
                        qtdPassageiros = viagem.QtdPassageiros
                    }
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "BuscarViagemEconomildo");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "BuscarViagemEconomildo" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao buscar viagem: " + error.Message
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   ATUALIZAR VIAGEM ECONOMILDO - UPDATE REGISTRO       |
        * |_______________________________________________________|
        */
        [Route("AtualizarViagemEconomildo")]
        [Consumes("application/json")]
        public JsonResult AtualizarViagemEconomildo([FromBody] ViagensEconomildo viagem)
        {
            try
            {
                if (viagem == null || viagem.ViagemEconomildoId == Guid.Empty)
                {
                    return Json(new
                    {
                        success = false ,
                        message = "Dados inválidos para atualização."
                    });
                }

                // Busca o registro existente
                var objFromDb = _unitOfWork.ViagensEconomildo.GetFirstOrDefault(
                    v => v.ViagemEconomildoId == viagem.ViagemEconomildoId
                );

                if (objFromDb == null)
                {
                    return Json(new
                    {
                        success = false ,
                        message = "Viagem não encontrada."
                    });
                }

                // Atualiza os campos
                objFromDb.Data = viagem.Data;
                objFromDb.VeiculoId = viagem.VeiculoId;
                objFromDb.MotoristaId = viagem.MotoristaId;
                objFromDb.MOB = viagem.MOB;
                objFromDb.Responsavel = viagem.Responsavel;
                objFromDb.IdaVolta = viagem.IdaVolta;
                objFromDb.HoraInicio = viagem.HoraInicio;
                objFromDb.HoraFim = viagem.HoraFim;
                objFromDb.QtdPassageiros = viagem.QtdPassageiros;

                // Salva no banco
                _unitOfWork.ViagensEconomildo.Update(objFromDb);
                _unitOfWork.Save();

                _log.Info($"Viagem Economildo atualizada. ID: {viagem.ViagemEconomildoId}", "ViagemController", "AtualizarViagemEconomildo");

                return Json(new
                {
                    success = true ,
                    message = "Viagem atualizada com sucesso!"
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "AtualizarViagemEconomildo");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "AtualizarViagemEconomildo" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao atualizar viagem: " + error.Message
                });
            }
        }

        public byte[] GetImage(string sBase64String)
        {
            byte[] bytes = null;
            if (!string.IsNullOrEmpty(sBase64String))
            {
                bytes = Convert.FromBase64String(sBase64String);
            }
            return bytes;
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   ADICIONAR EVENTO - CADASTRO RÁPIDO DE EVENTO        |
        * |_______________________________________________________|
        */
        [Route("AdicionarEvento")]
        [Consumes("application/json")]
        public JsonResult AdicionarEvento([FromBody] Evento evento)
        {
            try
            {
                var existeEvento = _unitOfWork.Evento.GetFirstOrDefault(u => (u.Nome == evento.Nome));
                if (existeEvento != null)
                {
                    return Json(new
                    {
                        success = false ,
                        message = "Já existe um evento com este nome"
                    });
                }

                _unitOfWork.Evento.Add(evento);
                _unitOfWork.Save();

                _log.Info($"Novo evento adicionado: {evento.Nome}", "ViagemController", "AdicionarEvento");

                return Json(
                    new
                    {
                        success = true ,
                        message = "Evento Adicionado com Sucesso" ,
                        eventoId = evento.EventoId ,
                        eventoText = evento.Nome ,
                    }
                );
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "AdicionarEvento");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "AdicionarEvento" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao adicionar evento"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   ADICIONAR REQUISITANTE - CADASTRO RÁPIDO            |
        * |_______________________________________________________|
        */
        [Route("AdicionarRequisitante")]
        [Consumes("application/json")]
        public JsonResult AdicionarRequisitante([FromBody] Requisitante requisitante)
        {
            try
            {
                var existeRequisitante = _unitOfWork.Requisitante.GetFirstOrDefault(u =>
                    (u.Ponto == requisitante.Ponto) || (u.Nome == requisitante.Nome)
                );
                if (existeRequisitante != null)
                {
                    return Json(
                        new
                        {
                            success = false ,
                            message = "Já existe um requisitante com este ponto/nome" ,
                        }
                    );
                }

                requisitante.Status = true;
                requisitante.DataAlteracao = DateTime.Now;

                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value;
                requisitante.UsuarioIdAlteracao = currentUserID;

                _unitOfWork.Requisitante.Add(requisitante);
                _unitOfWork.Save();

                _log.Info($"Novo requisitante adicionado: {requisitante.Nome}", "ViagemController", "AdicionarRequisitante");

                return Json(
                    new
                    {
                        success = true ,
                        message = "Requisitante Adicionado com Sucesso" ,
                        requisitanteid = requisitante.RequisitanteId ,
                    }
                );
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "AdicionarRequisitante");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "AdicionarRequisitante" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao adicionar requisitante"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   ADICIONAR SETOR - CADASTRO RÁPIDO DE SETOR          |
        * |_______________________________________________________|
        */
        [Route("AdicionarSetor")]
        [Consumes("application/json")]
        public JsonResult AdicionarSetor([FromBody] SetorSolicitante setorSolicitante)
        {
            try
            {
                if (setorSolicitante.Sigla != null)
                {
                    var existeSigla = _unitOfWork.SetorSolicitante.GetFirstOrDefault(u =>
                        u.Sigla.ToUpper() == setorSolicitante.Sigla.ToUpper()
            && u.SetorPaiId == setorSolicitante.SetorPaiId
                    );
                    if (
                        existeSigla != null
            && existeSigla.SetorSolicitanteId != setorSolicitante.SetorSolicitanteId
            && existeSigla.SetorPaiId == setorSolicitante.SetorPaiId
                    )
                    {
                        return Json(
                            new
                            {
                                success = false ,
                                message = "Já existe um setor com esta sigla neste nível hierárquico" ,
                            }
                        );
                    }
                }

                var existeSetor = _unitOfWork.SetorSolicitante.GetFirstOrDefault(u =>
                    u.Nome.ToUpper() == setorSolicitante.Nome.ToUpper()
        && u.SetorPaiId != setorSolicitante.SetorPaiId
                );
                if (
                    existeSetor != null
        && existeSetor.SetorSolicitanteId != setorSolicitante.SetorSolicitanteId
                )
                {
                    if (existeSetor.SetorPaiId == setorSolicitante.SetorPaiId)
                    {
                        return Json(
                            new
                            {
                                success = false ,
                                message = "Já existe um setor com este nome"
                            }
                        );
                    }
                }

                setorSolicitante.Status = true;
                setorSolicitante.DataAlteracao = DateTime.Now;

                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value;
                setorSolicitante.UsuarioIdAlteracao = currentUserID;

                _unitOfWork.SetorSolicitante.Add(setorSolicitante);
                _unitOfWork.Save();

                _log.Info($"Novo setor adicionado: {setorSolicitante.Nome}", "ViagemController", "AdicionarSetor");

                return Json(
                    new
                    {
                        success = true ,
                        message = "Setor Solicitante Adicionado com Sucesso" ,
                        setorId = setorSolicitante.SetorSolicitanteId ,
                    }
                );
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "AdicionarSetor");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "AdicionarSetor" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao adicionar setor"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   LISTA VIAGENS EVENTO - LISTAGEM FILTRADA            |
        * |_______________________________________________________|
        */
        [Route("ListaViagensEvento")]
        public async Task<IActionResult> ListaViagensEvento(
            Guid Id ,
            int page = 1 ,
            int pageSize = 50 ,
            bool useCache = true
        )
        {
            var swTotal = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                var swCache = System.Diagnostics.Stopwatch.StartNew();
                var cacheKey = $"viagens_evento_{Id}_{page}_{pageSize}";
                if (useCache && _cache.TryGetValue(cacheKey , out var cachedResult))
                {
                    swCache.Stop();
                    _log.Info($"ListaViagensEvento - Cache Hit: {Id}", "ViagemController", "ListaViagensEvento");
                    return Ok(cachedResult);
                }
                swCache.Stop();

                // ⚡ USA O MÉTODO OTIMIZADO
                var (viagens, totalItems) = await _unitOfWork.Viagem.GetViagensEventoPaginadoAsync(
                    Id ,
                    page ,
                    pageSize
                );

                var result = new
                {
                    data = viagens ,
                    pagination = new
                    {
                        totalItems ,
                        currentPage = page ,
                        pageSize ,
                        totalPages = (int)Math.Ceiling(totalItems / (double)pageSize) ,
                    } ,
                };

                if (useCache && totalItems > 0)
                {
                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(30))
                        .SetSize(1);
                    _cache.Set(cacheKey , result , cacheOptions);
                }

                swTotal.Stop();
                Console.WriteLine($"[TOTAL] {swTotal.ElapsedMilliseconds}ms\n");

                return Ok(result);
            }
            catch (Exception error)
            {
                swTotal.Stop();
                Console.WriteLine($"[ERRO] {swTotal.ElapsedMilliseconds}ms - {error.Message}");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ListaViagensEvento" , error);
                return StatusCode(500 , new { error = "Erro ao carregar viagens" , message = error.Message });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   OBTER TOTAL CUSTO VIAGENS EVENTO - DASHBOARD        |
        * |_______________________________________________________|
        */
        [Route("ObterTotalCustoViagensEvento")]
        [HttpGet]
        public async Task<IActionResult> ObterTotalCustoViagensEvento(Guid Id)
        {
            try
            {
                if (Id == Guid.Empty)
                {
                    return BadRequest(new
                    {
                        success = false ,
                        message = "ID do evento inválido"
                    });
                }

                var evento = _unitOfWork.Evento.GetFirstOrDefault(e => e.EventoId == Id);
                if (evento == null)
                {
                    return NotFound(new
                    {
                        success = false ,
                        message = "Evento não encontrado"
                    });
                }

                var viagens = _unitOfWork.ViewViagens.GetAll(filter: x =>
                    x.EventoId == Id && x.Status == "Realizada"
                ).ToList();

                var estatisticas = new
                {
                    TotalViagens = viagens.Count ,
                    ViagensComCusto = viagens.Where(v => v.CustoViagem.HasValue && v.CustoViagem > 0).ToList() ,
                    ViagensSemCusto = viagens.Where(v => !v.CustoViagem.HasValue || v.CustoViagem == 0).ToList() ,
                };

                decimal custoTotal = estatisticas.ViagensComCusto.Sum(v => (decimal)(v.CustoViagem ?? 0));
                decimal custoMedio = estatisticas.ViagensComCusto.Count > 0 ? custoTotal / estatisticas.ViagensComCusto.Count : 0;

                decimal? custoMinimo = estatisticas.ViagensComCusto.Count > 0 ? (decimal)estatisticas.ViagensComCusto.Min(v => v.CustoViagem ?? 0) : (decimal?)null;
                decimal? custoMaximo = estatisticas.ViagensComCusto.Count > 0 ? (decimal)estatisticas.ViagensComCusto.Max(v => v.CustoViagem ?? 0) : (decimal?)null;

                var culturaBR = new System.Globalization.CultureInfo("pt-BR");

                _log.Info($"Consulta de custos para evento: {evento.Nome}", "ViagemController", "ObterTotalCustoViagensEvento");

                return Ok(new
                {
                    success = true ,
                    eventoId = Id ,
                    nomeEvento = evento?.Nome ,
                    totalViagens = estatisticas.TotalViagens ,
                    totalViagensComCusto = estatisticas.ViagensComCusto.Count ,
                    viagensSemCusto = estatisticas.ViagensSemCusto.Count ,
                    percentualComCusto = estatisticas.TotalViagens > 0 ? (estatisticas.ViagensComCusto.Count * 100.0 / estatisticas.TotalViagens) : 0 ,
                    totalCusto = custoTotal ,
                    custoMedio = custoMedio ,
                    custoMinimo = custoMinimo ?? 0 ,
                    custoMaximo = custoMaximo ?? 0 ,
                    totalCustoFormatado = custoTotal.ToString("C" , culturaBR) ,
                    custoMedioFormatado = custoMedio.ToString("C" , culturaBR) ,
                    custoMinimoFormatado = custoMinimo?.ToString("C" , culturaBR) ?? "R$ 0,00" ,
                    custoMaximoFormatado = custoMaximo?.ToString("C" , culturaBR) ?? "R$ 0,00" ,
                    dataConsulta = DateTime.Now ,
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "ObterTotalCustoViagensEvento");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ObterTotalCustoViagensEvento" , error);
                return StatusCode(500 , new { success = false , message = "Erro ao processar a solicitação" , error = error.Message });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   ESTATÍSTICAS VIAGENS EVENTO - ANALYTICS             |
        * |_______________________________________________________|
        */
        [Route("EstatisticasViagensEvento")]
        [HttpGet]
        public IActionResult EstatisticasViagensEvento(Guid Id)
        {
            try
            {
                var viagens = _unitOfWork.ViewViagens.GetAll(filter: x => x.EventoId == Id && x.Status == "Realizada").ToList();

                var estatisticas = new
                {
                    success = true ,
                    totalViagens = viagens.Count() ,
                    viagensComCusto = viagens.Count(v => v.CustoViagem.HasValue && v.CustoViagem > 0) ,
                    viagensSemCusto = viagens.Count(v => !v.CustoViagem.HasValue || v.CustoViagem == 0) ,
                    custoTotal = viagens.Where(v => v.CustoViagem.HasValue).Sum(v => v.CustoViagem.Value) ,
                    custoMedio = viagens.Where(v => v.CustoViagem.HasValue && v.CustoViagem > 0).Select(v => v.CustoViagem.Value).DefaultIfEmpty(0).Average() ,
                    custoMinimo = viagens.Where(v => v.CustoViagem.HasValue && v.CustoViagem > 0).Select(v => v.CustoViagem.Value).DefaultIfEmpty(0).Min() ,
                    custoMaximo = viagens.Where(v => v.CustoViagem.HasValue && v.CustoViagem > 0).Select(v => v.CustoViagem.Value).DefaultIfEmpty(0).Max() ,
                    motoristas = viagens.Where(v => !string.IsNullOrEmpty(v.NomeMotorista)).GroupBy(v => v.NomeMotorista).Select(g => new { nome = g.Key , viagens = g.Count() , custoTotal = g.Sum(v => v.CustoViagem ?? 0) , }).OrderByDescending(m => m.viagens).ToList() ,
                };

                return Ok(estatisticas);
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "EstatisticasViagensEvento");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "EstatisticasViagensEvento" , error);
                return Ok(new { success = false , error = error.Message });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   SAVE IMAGE - UPLOAD DE ARQUIVOS (GENERAL)           |
        * |_______________________________________________________|
        */
        [HttpPost]
        public IActionResult SaveImage(IList<IFormFile> UploadFiles)
        {
            try
            {
                if (UploadFiles == null || UploadFiles.Count == 0 || UploadFiles[0] == null)
                {
                    return BadRequest("Nenhum arquivo foi enviado");
                }

                foreach (IFormFile file in UploadFiles)
                {
                    if (file?.Length > 0)
                    {
                        string filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        filename = Path.GetFileName(filename);
                        string folderPath = Path.Combine(hostingEnv.WebRootPath , "DadosEditaveis" , "ImagensViagens");

                        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                        string fullPath = Path.Combine(folderPath , filename);
                        using (var stream = new FileStream(fullPath , FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        _log.Info($"Arquivo salvo: {filename}", "ViagemController", "SaveImage");
                    }
                }

                return Ok(new { message = "Imagem(ns) salva(s) com sucesso!" });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "SaveImage");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "SaveImage" , error);
                return StatusCode(500);
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   ESTATÍSTICAS VEÍCULO - VALIDAÇÃO INTELIGENTE        |
        * |_______________________________________________________|
        */
        [HttpGet]
        [Route("EstatisticasVeiculo")]
        public async Task<IActionResult> GetEstatisticasVeiculo([FromQuery] Guid veiculoId)
        {
            try
            {
                if (veiculoId == Guid.Empty)
                {
                    return Json(new { success = false , message = "VeiculoId é obrigatório" });
                }

                var estatisticas = await _veiculoEstatisticaService.ObterEstatisticasAsync(veiculoId);
                return Json(new { success = true , data = estatisticas });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "GetEstatisticasVeiculo");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "GetEstatisticasVeiculo" , error);
                return Json(new { success = false , message = "Erro ao obter estatísticas do veículo" });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   FINALIZA VIAGEM - CONCLUSÃO E CÁLCULOS              |
        * |_______________________________________________________|
        */
        [HttpPost]
        [Route("FinalizaViagem")]
        [Consumes("application/json")]
        public async Task<IActionResult> FinalizaViagemAsync([FromBody] FinalizacaoViagem viagem)
        {
            try
            {
                if (viagem.DataFinal.HasValue && viagem.DataFinal.Value.Date > DateTime.Today)
                {
                    return Json(new { success = false , message = "A Data Final não pode ser superior à data atual." });
                }

                var objViagem = _unitOfWork.Viagem.GetFirstOrDefault(v => v.ViagemId == viagem.ViagemId);
                if (objViagem == null)
                {
                    return Json(new { success = false , message = "Viagem não encontrada" });
                }

                objViagem.DataFinal = viagem.DataFinal;
                objViagem.HoraFim = viagem.HoraFim;
                objViagem.KmFinal = viagem.KmFinal;
                objViagem.CombustivelFinal = viagem.CombustivelFinal;
                objViagem.Descricao = viagem.Descricao;
                objViagem.Status = "Realizada";
                objViagem.StatusDocumento = viagem.StatusDocumento;
                objViagem.StatusCartaoAbastecimento = viagem.StatusCartaoAbastecimento;

                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value;
                var currentUserName = currentUser.Identity?.Name ?? "Sistema";
                objViagem.UsuarioIdFinalizacao = currentUserID;
                objViagem.DataFinalizacao = DateTime.Now;

                _unitOfWork.Viagem.Update(objViagem);

                int ocorrenciasCriadas = 0;
                if (viagem.Ocorrencias != null && viagem.Ocorrencias.Any())
                {
                    foreach (var ocDto in viagem.Ocorrencias)
                    {
                        if (!string.IsNullOrWhiteSpace(ocDto.Resumo))
                        {
                            var ocorrencia = new OcorrenciaViagem
                            {
                                OcorrenciaViagemId = Guid.NewGuid() ,
                                ViagemId = objViagem.ViagemId ,
                                VeiculoId = objViagem.VeiculoId ?? Guid.Empty ,
                                MotoristaId = objViagem.MotoristaId ,
                                Resumo = ocDto.Resumo ?? "" ,
                                Descricao = ocDto.Descricao ?? "" ,
                                ImagemOcorrencia = ocDto.ImagemOcorrencia ?? "" ,
                                Status = "Aberta" ,
                                DataCriacao = DateTime.Now ,
                                UsuarioCriacao = currentUserName
                            };
                            _unitOfWork.OcorrenciaViagem.Add(ocorrencia);
                            ocorrenciasCriadas++;
                        }
                    }
                }

                var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(v => v.VeiculoId == objViagem.VeiculoId);
                if (veiculo != null)
                {
                    veiculo.Quilometragem = viagem.KmFinal;
                    _unitOfWork.Veiculo.Update(veiculo);
                }

                _unitOfWork.Save();

                // Fila de custos recalculada
                // COMENTADO: Método RecalcularCustosBackground não existe em IViagemRepository
                // _unitOfWork.Viagem.RecalcularCustosBackground(objViagem.ViagemId);

                await _viagemEstatisticaService.AtualizarEstatisticasDiaAsync((DateTime)objViagem.DataInicial);

                _log.Info($"Viagem finalizada. ID: {objViagem.ViagemId}. Ocorrências: {ocorrenciasCriadas}", "ViagemController", "FinalizaViagem");

                var mensagem = "Viagem finalizada com sucesso";
                if (ocorrenciasCriadas > 0) mensagem += $" ({ocorrenciasCriadas} ocorrência(s) registrada(s))";

                return Json(new { success = true , message = mensagem , type = 0 , ocorrenciasCriadas = ocorrenciasCriadas });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "FinalizaViagem");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "FinalizaViagem" , error);
                return Json(new { success = false , message = "Erro ao finalizar viagem: " + error.Message });
            }
        }

        [Route("AjustaViagem")]
        [HttpPost]
        [Consumes("application/json")]
        public IActionResult AjustaViagem([FromBody] AjusteViagem viagem)
        {
            try
            {
                // VALIDAÇÃO: Data Final não pode ser superior à data atual
                if (viagem.DataFinal.HasValue && viagem.DataFinal.Value.Date > DateTime.Today)
                {
                    return Json(new
                    {
                        success = false ,
                        message = "A Data Final não pode ser superior à data atual."
                    });
                }

                var objViagem = _unitOfWork.Viagem.GetFirstOrDefault(v =>
                    v.ViagemId == viagem.ViagemId
                );
                objViagem.NoFichaVistoria = viagem.NoFichaVistoria;
                objViagem.DataInicial = viagem.DataInicial;
                objViagem.HoraInicio = viagem.HoraInicial;
                objViagem.KmInicial = viagem.KmInicial;
                objViagem.DataFinal = viagem.DataFinal;
                objViagem.HoraFim = viagem.HoraFim;
                objViagem.KmFinal = viagem.KmFinal;

                objViagem.MotoristaId = viagem.MotoristaId;
                objViagem.VeiculoId = viagem.VeiculoId;
                objViagem.Finalidade = viagem.Finalidade;
                objViagem.SetorSolicitanteId = viagem.SetorSolicitanteId ?? Guid.Empty;

                objViagem.CustoCombustivel = Servicos.CalculaCustoCombustivel(objViagem , _unitOfWork);

                int minutos = -1;
                objViagem.CustoMotorista = Servicos.CalculaCustoMotorista(
                    objViagem ,
                    _unitOfWork ,
                    ref minutos
                );
                objViagem.Minutos = minutos;

                objViagem.CustoVeiculo = Servicos.CalculaCustoVeiculo(objViagem , _unitOfWork);

                objViagem.CustoOperador = 0;
                objViagem.CustoLavador = 0;

                objViagem.EventoId = viagem.EventoId;

                _unitOfWork.Viagem.Update(objViagem);
                _unitOfWork.Save();

                return Json(
                    new
                    {
                        success = true ,
                        message = "Viagem ajustada com sucesso" ,
                        type = 0 ,
                    }
                );
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "AjustaViagem" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao ajustar viagem"
                });
            }
        }

        [HttpPost]
        [Route("FluxoServerSide")]
        public IActionResult FluxoServerSide()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var startStr = Request.Form["start"].FirstOrDefault();
                var lengthStr = Request.Form["length"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = lengthStr != null ? Convert.ToInt32(lengthStr) : 10;
                int skip = startStr != null ? Convert.ToInt32(startStr) : 0;

                var veiculoIdStr = Request.Form["veiculoId"].FirstOrDefault();
                var motoristaIdStr = Request.Form["motoristaId"].FirstOrDefault();
                var dataFluxoDeStr = Request.Form["dataFluxoDe"].FirstOrDefault();
                var dataFluxoAteStr = Request.Form["dataFluxoAte"].FirstOrDefault();

                var query = _unitOfWork.ViewFluxoEconomildo.GetAll().AsQueryable();

                if (
                    !string.IsNullOrEmpty(veiculoIdStr) && Guid.TryParse(veiculoIdStr , out var veicGuid)
                )
                {
                    query = query.Where(v => v.VeiculoId == veicGuid);
                }

                if (
                    !string.IsNullOrEmpty(motoristaIdStr)
        && Guid.TryParse(motoristaIdStr , out var motGuid)
                )
                {
                    query = query.Where(v => v.MotoristaId == motGuid);
                }

                if (!string.IsNullOrEmpty(dataFluxoDeStr))
                {
                    if (DateTime.TryParse(dataFluxoDeStr , out var dataConvDe))
                    {
                        query = query.Where(v => v.Data.HasValue && v.Data.Value.Date >= dataConvDe.Date);
                    }
                }

                if (!string.IsNullOrEmpty(dataFluxoAteStr))
                {
                    if (DateTime.TryParse(dataFluxoAteStr , out var dataConvAte))
                    {
                        query = query.Where(v => v.Data.HasValue && v.Data.Value.Date <= dataConvAte.Date);
                    }
                }

                int recordsTotal = _unitOfWork.ViewFluxoEconomildo.GetAll().Count();

                if (!string.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(v =>
                        v.MOB.Contains(searchValue)
            || v.NomeMotorista.Contains(searchValue)
            || v.DescricaoVeiculo.Contains(searchValue)
                    );
                }

                int recordsFiltered = query.Count();

                query = query.OrderByDescending(v => v.Data);

                var pageData = query.Skip(skip).Take(pageSize).ToList();

                var data = pageData
                    .Select(x => new
                    {
                        x.ViagemEconomildoId ,
                        x.MotoristaId ,
                        x.VeiculoId ,
                        x.NomeMotorista ,
                        x.DescricaoVeiculo ,
                        x.MOB ,
                        DataFluxo = x.Data.HasValue ? x.Data.Value.ToString("dd/MM/yyyy") : "" ,
                        x.HoraInicio ,
                        x.HoraFim ,
                        x.QtdPassageiros ,
                    })
                    .ToList();

                var result = new
                {
                    draw = draw ,
                    recordsTotal = recordsTotal ,
                    recordsFiltered = recordsFiltered ,
                    data = data ,
                };
                return Json(result);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "FluxoServerSide" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao processar fluxo"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   OBTER DOCUMENTO - GET EDITOR JSON (SFDT)            |
        * |_______________________________________________________|
        */
        [HttpGet("ObterDocumento")]
        public IActionResult ObterDocumento(Guid viagemId)
        {
            try
            {
                var viagem = _unitOfWork.Viagem.GetFirstOrDefault(v => v.ViagemId == viagemId);
                if (viagem?.DescricaoViagemWord == null)
                    return NotFound();

                var sfdtJson = System.Text.Encoding.UTF8.GetString(viagem.DescricaoViagemWord);
                return Ok(new
                {
                    sfdt = sfdtJson
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "ObterDocumento");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ObterDocumento" , error);
                return StatusCode(500);
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   OBTER DADOS MOBILE - TELEMETRIA E VISTORIA          |
        * |_______________________________________________________|
        */
        [HttpGet]
        [Route("ObterDadosMobile")]
        public IActionResult ObterDadosMobile(Guid viagemId)
        {
            try
            {
                if (viagemId == Guid.Empty)
                {
                    return Json(new
                    {
                        success = false ,
                        message = "ID da viagem não informado"
                    });
                }

                var viagem = _unitOfWork.Viagem.GetFirstOrDefault(v => v.ViagemId == viagemId);

                if (viagem == null)
                {
                    return Json(new
                    {
                        success = false ,
                        message = "Viagem não encontrada"
                    });
                }

                // Rubrica e RubricaFinal são strings (podem ser base64 ou URL)
                string rubricaInicial = null;
                string rubricaFinal = null;

                if (!string.IsNullOrWhiteSpace(viagem.Rubrica))
                {
                    // Se já for base64 com prefixo, usar direto; senão, adicionar prefixo
                    rubricaInicial = viagem.Rubrica.StartsWith("data:")
                        ? viagem.Rubrica
                        : $"data:image/png;base64,{viagem.Rubrica}";
                }

                if (!string.IsNullOrWhiteSpace(viagem.RubricaFinal))
                {
                    rubricaFinal = viagem.RubricaFinal.StartsWith("data:")
                        ? viagem.RubricaFinal
                        : $"data:image/png;base64,{viagem.RubricaFinal}";
                }

                // Buscar ocorrências da viagem
                var ocorrencias = _unitOfWork.OcorrenciaViagem
                    .GetAll(o => o.ViagemId == viagemId)
                    .OrderByDescending(o => o.DataCriacao)
                    .Select(o => new
                    {
                        o.OcorrenciaViagemId ,
                        o.Resumo ,
                        o.Descricao ,
                        DataOcorrencia = o.DataCriacao.ToString("dd/MM/yyyy HH:mm") ,
                        StatusOcorrencia = o.Status ,
                        Solucao = o.Solucao ?? "" ,
                        TemImagem = !string.IsNullOrWhiteSpace(o.ImagemOcorrencia) && o.ImagemOcorrencia != "semimagem.jpg" ,
                        ImagemBase64 = !string.IsNullOrWhiteSpace(o.ImagemOcorrencia) && o.ImagemOcorrencia != "semimagem.jpg"
                            ? (o.ImagemOcorrencia.StartsWith("data:") ? o.ImagemOcorrencia : $"data:image/jpeg;base64,{o.ImagemOcorrencia}")
                            : null
                    })
                    .ToList();

                return Json(new
                {
                    success = true ,
                    noFichaVistoria = viagem.NoFichaVistoria ,
                    isMobile = viagem.NoFichaVistoria == null || viagem.NoFichaVistoria == 0 ,
                    rubricaInicial = rubricaInicial ,
                    rubricaFinal = rubricaFinal ,
                    temRubricaInicial = rubricaInicial != null ,
                    temRubricaFinal = rubricaFinal != null ,
                    // Documentos/Itens Entregues (Vistoria Inicial)
                    statusDocumento = viagem.StatusDocumento ?? "" ,
                    statusCartaoAbastecimento = viagem.StatusCartaoAbastecimento ?? "" ,
                    cintaEntregue = viagem.CintaEntregue ?? false ,
                    tabletEntregue = viagem.TabletEntregue ?? false ,
                    // Documentos/Itens Devolvidos (Vistoria Final)
                    statusDocumentoFinal = viagem.StatusDocumentoFinal ?? "" ,
                    statusCartaoAbastecimentoFinal = viagem.StatusCartaoAbastecimentoFinal ?? "" ,
                    cintaDevolvida = viagem.CintaDevolvida ?? false ,
                    tabletDevolvido = viagem.TabletDevolvido ?? false ,
                    // Ocorrências
                    ocorrencias = ocorrencias ,
                    totalOcorrencias = ocorrencias.Count
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "ObterDadosMobile");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ObterDadosMobile" , error);
                return Json(new
                {
                    success = false ,
                    message = $"Erro ao obter dados mobile: {error.Message}"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   OBTER IMAGEM OCORRÊNCIA - PREVIEW DE MÍDIA          |
        * |_______________________________________________________|
        */
        [HttpGet]
        [Route("ObterImagemOcorrencia")]
        public IActionResult ObterImagemOcorrencia(Guid ocorrenciaId)
        {
            try
            {
                if (ocorrenciaId == Guid.Empty)
                {
                    return Json(new
                    {
                        success = false ,
                        message = "ID da ocorrência não informado"
                    });
                }

                var ocorrencia = _unitOfWork.OcorrenciaViagem.GetFirstOrDefault(o =>
                    o.OcorrenciaViagemId == ocorrenciaId);

                if (ocorrencia == null)
                {
                    return Json(new
                    {
                        success = false ,
                        message = "Ocorrência não encontrada"
                    });
                }

                if (!string.IsNullOrWhiteSpace(ocorrencia.ImagemOcorrencia) && ocorrencia.ImagemOcorrencia != "semimagem.jpg")
                {
                    var imagemBase64 = ocorrencia.ImagemOcorrencia.StartsWith("data:")
                        ? ocorrencia.ImagemOcorrencia
                        : $"data:image/jpeg;base64,{ocorrencia.ImagemOcorrencia}";

                    return Json(new
                    {
                        success = true ,
                        temImagem = true ,
                        imagemBase64 = imagemBase64
                    });
                }

                return Json(new
                {
                    success = true ,
                    temImagem = false
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "ObterImagemOcorrencia");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ObterImagemOcorrencia" , error);
                return Json(new
                {
                    success = false ,
                    message = $"Erro ao obter imagem: {error.Message}"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   OBTER OCORRÊNCIAS VIAGEM - LISTAGEM DE EVENTOS      |
        * |_______________________________________________________|
        */
        [HttpGet]
        [Route("ObterOcorrenciasViagem")]
        public IActionResult ObterOcorrenciasViagem(Guid viagemId)
        {
            try
            {
                if (viagemId == Guid.Empty)
                {
                    return Json(new
                    {
                        success = false ,
                        message = "ID da viagem não informado"
                    });
                }

                var ocorrencias = _unitOfWork.OcorrenciaViagem
                    .GetAll(o => o.ViagemId == viagemId)
                    .OrderByDescending(o => o.DataCriacao)
                    .Select(o => new
                    {
                        o.OcorrenciaViagemId ,
                        o.Resumo ,
                        o.Descricao ,
                        DataOcorrencia = o.DataCriacao.ToString("dd/MM/yyyy HH:mm") ,
                        StatusOcorrencia = o.Status ,
                        Solucao = o.Solucao ?? "" ,
                        TemImagem = !string.IsNullOrWhiteSpace(o.ImagemOcorrencia) && o.ImagemOcorrencia != "semimagem.jpg"
                    })
                    .ToList();

                return Json(new
                {
                    success = true ,
                    ocorrencias = ocorrencias ,
                    total = ocorrencias.Count
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "ViagemController", "ObterOcorrenciasViagem");
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ObterOcorrenciasViagem" , error);
                return Json(new
                {
                    success = false ,
                    message = $"Erro ao obter ocorrências: {error.Message}"
                });
            }
        }
    }

    /*
    *  _______________________________________________________
    * |                                                       |
    * |   REQUEST SIZE LIMIT - ATRIBUTO DE AUTORIZAÇÃO        |
    * |_______________________________________________________|
    */
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Method ,
        AllowMultiple = false ,
        Inherited = true
    )]
    public class RequestSizeLimitAttribute : Attribute, IAuthorizationFilter, IOrderedFilter
    {
        private readonly FormOptions _formOptions;

        public RequestSizeLimitAttribute(int valueCountLimit)
        {
            _formOptions = new FormOptions()
            {
                KeyLengthLimit = valueCountLimit ,
                ValueCountLimit = valueCountLimit ,
                ValueLengthLimit = valueCountLimit ,
            };
        }

        public int Order { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var contextFeatures = context.HttpContext.Features;
            var formFeature = contextFeatures.Get<IFormFeature>();

            if (formFeature == null || formFeature.Form == null)
            {
                contextFeatures.Set<IFormFeature>(
                    new FormFeature(context.HttpContext.Request , _formOptions)
                );
            }
        }
    }
}
