/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║  📄 DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md                  ║
 * ║  Seção: ViagemController.cs                                              ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

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
    /****************************************************************************************
     * ⚡ CONTROLLER: Viagem API (Partial - ARQUIVO PRINCIPAL)
     * 🎯 OBJETIVO: Controller principal de viagens - gerenciar CRUD completo de viagens
     * 📋 ROTAS PRINCIPAIS:
     *    - GET /api/Viagem - Listar viagens
     *    - GET /api/Viagem/{id} - Obter viagem por ID
     *    - POST /api/Viagem/Upsert - Criar ou atualizar viagem
     *    - DELETE /api/Viagem/Delete - Deletar viagem
     *    + MUITAS outras rotas (estatísticas, dashboards, relatórios, etc.)
     * 🔗 ENTIDADES: Viagem, ViewViagens, Veiculo, Motorista, Evento, Ocorrencia
     * 📦 DEPENDÊNCIAS:
     *    - FrotiXDbContext, IUnitOfWork, IViagemRepository
     *    - MotoristaFotoService, IMemoryCache, IServiceScopeFactory
     *    - ViagemEstatisticaService, VeiculoEstatisticaService
     * 📝 NOTA: Classe parcial com múltiplos arquivos:
     *    - ViagemController.cs (PRINCIPAL - 3.101 linhas)
     *    - ViagemController.DashboardEconomildo.cs
     *    - ViagemController.CalculoCustoBatch.cs
     *    - ViagemController.AtualizarDados.cs
     *    - ViagemController.AtualizarDadosViagem.cs
     *    - ViagemController.CustosViagem.cs
     *    - ViagemController.DesassociarEvento.cs
     *    - ViagemController.HeatmapEconomildo.cs
     *    - ViagemController.HeatmapEconomildoPassageiros.cs
     *    - ViagemController.ListaEventos.cs
     *    - ViagemController.MetodosEstatisticas.cs
     * ⚠️ ARQUIVO MUITO GRANDE: 3.101 linhas com dezenas de métodos
     ****************************************************************************************/
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public partial class ViagemController : Controller
    {
        private readonly FrotiXDbContext _context; // ← ADICIONAR
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment hostingEnv;
        private readonly IViagemRepository _viagemRepo;
        private readonly MotoristaFotoService _fotoService;
        private readonly IMemoryCache _cache;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ViagemEstatisticaService _viagemEstatisticaService;
        private readonly VeiculoEstatisticaService _veiculoEstatisticaService;

        [ActivatorUtilitiesConstructor]
        public ViagemController(
            FrotiXDbContext context ,
            IUnitOfWork unitOfWork ,
            IViagemRepository viagemRepo ,
            IWebHostEnvironment webHostEnvironment ,
            MotoristaFotoService fotoService ,
            IMemoryCache cache , IServiceScopeFactory serviceScopeFactory , IViagemEstatisticaRepository viagemEstatisticaRepository
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
                _viagemEstatisticaService = new ViagemEstatisticaService(_context , viagemEstatisticaRepository , unitOfWork);
                _veiculoEstatisticaService = new VeiculoEstatisticaService(_context , cache);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ViagemController" , error);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: viagemsFilters (PRIVATE STATIC HELPER)
         * 🎯 OBJETIVO: Criar expressão de filtro dinâmico para consultas de viagens
         * 📥 ENTRADAS: veiculoId, motoristaId, dataViagem, statusId, eventoId
         * 📤 SAÍDAS: Expression<Func<ViewViagens, bool>> (expressão LINQ)
         * 🔗 CHAMADA POR: Get(), FluxoFiltrado() e outros métodos de consulta
         * 📊 FILTROS:
         *    - StatusAgendamento == false (sempre)
         *    - Status (opcional)
         *    - MotoristaId (opcional)
         *    - VeiculoId (opcional)
         *    - EventoId (opcional)
         *    - DataInicial (opcional, parseada de dd/MM/yyyy)
         ****************************************************************************************/
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

        [HttpPost]
        [Route("UploadFichaVistoria")]
        public async Task<IActionResult> UploadFichaVistoria(
            IFormFile arquivo ,
            [FromForm] string viagemId
        )
        {
            try
            {
                Console.WriteLine($"Recebido viagemId: {viagemId}");

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

                _unitOfWork.Viagem.Update(viagem);
                _unitOfWork.Save();

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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "UploadFichaVistoria" , error);
                Console.WriteLine($"Erro ao salvar ficha: {error.Message}");
                return Json(
                    new
                    {
                        success = false ,
                        message = $"Erro ao salvar ficha: {error.Message}"
                    }
                );
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ExisteFichaParaData
         * 🎯 OBJETIVO: Verificar se existe viagem Economildo para data específica
         * 📥 ENTRADAS: data (string yyyy-MM-dd)
         * 📤 SAÍDAS: JSON boolean (true se existe, false se não)
         ****************************************************************************************/
        [Route("ExisteFichaParaData")]
        public JsonResult ExisteFichaParaData(string data)
        {
            try
            {
                // Converte string para DateTime
                if (!DateTime.TryParseExact(data , "yyyy-MM-dd" , CultureInfo.InvariantCulture , DateTimeStyles.None , out DateTime dataConvertida))
                {
                    return Json(false);
                }

                // Verifica se existe algum registro para essa data
                var existeFicha = _unitOfWork.ViagensEconomildo
                    .GetAll()
                    .Any(v => v.Data.HasValue && v.Data.Value.Date == dataConvertida.Date);

                return Json(existeFicha);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ExisteFichaParaData" , error);
                return Json(false);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: VerificaFichaExiste
         * 🎯 OBJETIVO: Verificar se ficha de vistoria já existe (evitar duplicidade)
         * 📥 ENTRADAS: noFichaVistoria (int)
         * 📤 SAÍDAS: JSON { success, data: { existe, viagemId, fichaId } }
         ****************************************************************************************/
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
            catch (Exception ex)
            {
                return Json(new { success = false , message = ex.Message });
            }
        }

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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ObterFichaVistoria" , error);
                return Json(new
                {
                    success = false ,
                    message = $"Erro: {error.Message}"
                });
            }
        }

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
                    _unitOfWork.Save();
                }

                return Json(new
                {
                    success = true
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "MontaDescricaoSemFormato" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao montar descrição"
                });
            }
        }

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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "FotoMotorista" , error);
                return Json(new
                {
                    fotoBase64 = ""
                });
            }
        }

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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "PegarStatusViagem" , error);
                return StatusCode(500 , $"Erro ao verificar status da viagem: {error.Message}");
            }
        }

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
                    .Where(o => !string.IsNullOrEmpty(o)) // ✅ FILTRO ADICIONAL
                    .Distinct()
                    .OrderBy(o => o)
                    .ToList();

                var destinos = _unitOfWork
                    .Viagem.GetAllReduced(
                        selector: v => v.Destino ,
                        filter: v => !string.IsNullOrWhiteSpace(v.Destino)
                    )
                    .Select(d => d?.Trim())
                    .Where(d => !string.IsNullOrEmpty(d)) // ✅ FILTRO ADICIONAL
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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ListaDistintos" , error);
                return StatusCode(500 , $"Erro ao obter origens/destinos: {error.Message}");
            }
        }

        /****************************************************************************************
         * 📦 CLASSE: UnificacaoRequest
         * 🎯 OBJETIVO: Request para unificação de origens/destinos
         * 📋 PROPRIEDADES:
         *    - NovoValor: Valor normalizado final
         *    - OrigensSelecionadas: Lista de origens a unificar
         *    - DestinosSelecionados: Lista de destinos a unificar
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: Unificar
         * 🎯 OBJETIVO: Unificar valores de Origem e/ou Destino (correção em massa)
         * 📥 ENTRADAS: request (UnificacaoRequest com NovoValor e listas de valores antigos)
         * 📤 SAÍDAS: OK { mensagem } ou BadRequest/StatusCode 500
         * 🔗 CHAMADA POR: Interface de limpeza de dados (unificação de origens/destinos)
         * 🔄 CHAMA: Viagem.GetAllReduced(), Update(), Save()
         * 📊 ALGORITMO:
         *    1. Normaliza valores selecionados (remove espaços, acentos, caracteres especiais)
         *    2. Busca viagens com origens/destinos normalizados correspondentes
         *    3. Atualiza para NovoValor
         *    4. Salva em batch
         * 📝 NOTA: Função de limpeza de dados - unifica variações do mesmo local
         ****************************************************************************************/
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

                return Ok(new
                {
                    mensagem = "Unificação realizada com sucesso."
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "Unificar" , error);
                return StatusCode(500 , new
                {
                    success = false ,
                    message = "Erro ao unificar"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Get (MÉTODO PRINCIPAL DE CONSULTA)
         * 🎯 OBJETIVO: Listar viagens com filtros opcionais (CRUD Read)
         * 📥 ENTRADAS: veiculoId, motoristaId, statusId, dataViagem, eventoId (todos opcionais)
         * 📤 SAÍDAS: JSON { data: Array<ViewViagens projetado> }
         * 🔗 CHAMADA POR: Frontend (DataTable principal de viagens)
         * 🔄 CHAMA: ViewViagens.GetAll() com viagemsFilters()
         * ⚡ OTIMIZAÇÃO:
         *    - Ordenação no banco (SQL ORDER BY)
         *    - Projeção no banco (SQL SELECT específico)
         *    - AsNoTracking (read-only query)
         * 📊 ORDENAÇÃO ESPECIAL:
         *    1. NoFichaVistoria = 0 ou null primeiro (para preenchimento)
         *    2. DataInicial DESC
         *    3. HoraInicio DESC
         *    4. NoFichaVistoria DESC
         ****************************************************************************************/
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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "Get" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao carregar viagens"
                });
            }
        }

        [HttpPost]
        [Route("AplicarCorrecaoOrigem")]
        public async Task<IActionResult> AplicarCorrecaoOrigem([FromBody] CorrecaoOrigemDto dto)
        {
            try
            {
                if (dto == null || dto.Origens == null || string.IsNullOrWhiteSpace(dto.NovaOrigem))
                    return BadRequest();

                await _viagemRepo.CorrigirOrigemAsync(dto.Origens , dto.NovaOrigem);
                return Ok();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "AplicarCorrecaoOrigem" , error);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("AplicarCorrecaoDestino")]
        public async Task<IActionResult> AplicarCorrecaoDestino([FromBody] CorrecaoDestinoDto dto)
        {
            try
            {
                if (dto == null || dto.Destinos == null || string.IsNullOrWhiteSpace(dto.NovoDestino))
                    return BadRequest();

                await _viagemRepo.CorrigirDestinoAsync(dto.Destinos , dto.NovoDestino);
                return Ok();
            }
            catch (Exception error)
            {
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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "FluxoFiltrado" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao filtrar fluxo"
                });
            }
        }

        ///// <summary>
        ///// Lista todos os eventos COM o campo custoViagem calculado
        ///// Rota: /api/viagem/listaeventos
        ///// </summary>
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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "UpdateStatusEvento" , error);
                return Json(new
                {
                    success = false
                });
            }
        }

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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ApagaEvento" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao deletar evento"
                });
            }
        }

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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ViagemEventos" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao carregar viagens de eventos"
                });
            }
        }

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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "Fluxo" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao carregar fluxo"
                });
            }
        }

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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "FluxoVeiculos" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao carregar fluxo de veículos"
                });
            }
        }

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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "FluxoMotoristas" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao carregar fluxo de motoristas"
                });
            }
        }

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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "FluxoData" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao carregar fluxo por data"
                });
            }
        }

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

                return Json(new
                {
                    success = true,
                    message = "Viagem apagada com sucesso"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs", "ApagaFluxoEconomildo", error);
                return Json(new
                {
                    success = false,
                    message = "Erro ao apagar viagem"
                });
            }
        }

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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "MyUploader" , error);
                return new ObjectResult(new
                {
                    status = "fail"
                });
            }
        }

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

                return Json(new
                {
                    success = true ,
                    message = "Processamento iniciado com sucesso"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "CalculaCustoViagens" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao iniciar cálculo de custos"
                });
            }
        }

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
                _cache.Set(cacheKey , progresso , TimeSpan.FromMinutes(30));

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
                    _cache.Set(cacheKey , progresso , TimeSpan.FromMinutes(30));

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

                            _cache.Set(cacheKey , progresso , TimeSpan.FromMinutes(30));

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
                    _cache.Set(cacheKey , progresso , TimeSpan.FromMinutes(30));
                } // ← O scope é disposed aqui automaticamente
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ProcessarCalculoCustoViagens" , error);

                progresso.Erro = true;
                progresso.Concluido = true;
                progresso.Mensagem = $"Erro durante o processamento: {error.Message}";
                _cache.Set(cacheKey , progresso , TimeSpan.FromMinutes(30));
            }
        }

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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ObterProgressoCalculoCusto" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao obter progresso"
                });
            }
        }

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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "LimparProgressoCalculoCusto" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao limpar progresso"
                });
            }
        }

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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ViagemVeiculos" , error);
                return StatusCode(500);
            }
        }

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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ViagemMotoristas" , error);
                return StatusCode(500);
            }
        }

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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ViagemStatus" , error);
                return StatusCode(500);
            }
        }

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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ViagemSetores" , error);
                return StatusCode(500);
            }
        }

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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ViagemData" , error);
                return StatusCode(500);
            }
        }

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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "Ocorrencias" , error);
                return StatusCode(500);
            }
        }

        [Route("Cancelar")]
        [HttpPost]
        public IActionResult Cancelar(ViagemID id)
        {
            try
            {
                var objFromDb = _unitOfWork.Viagem.GetFirstOrDefault(u => u.ViagemId == id.ViagemId);
                if (objFromDb != null)
                {
                    ClaimsPrincipal currentUser = this.User;
                    var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                    objFromDb.UsuarioIdCancelamento = currentUserID;
                    objFromDb.DataCancelamento = DateTime.Now;

                    objFromDb.Status = "Cancelada";
                    _unitOfWork.Viagem.Update(objFromDb);
                    _unitOfWork.Save();
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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "Cancelar" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao cancelar viagem"
                });
            }
        }

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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "PegaFicha" , error);
                return Json(false);
            }
        }

        [Route("AdicionarViagensEconomildo")]
        [Consumes("application/json")]
        public JsonResult AdicionarViagensEconomildo([FromBody] ViagensEconomildo viagensEconomildo)
        {
            try
            {
                _unitOfWork.ViagensEconomildo.Add(viagensEconomildo);
                _unitOfWork.Save();

                return Json(new
                {
                    success = true ,
                    message = "Viagem Adicionada com Sucesso!"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "AdicionarViagensEconomildo" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao adicionar viagem"
                });
            }
        }

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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ExisteDataEconomildo" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao verificar data"
                });
            }
        }

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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "PegaFichaModal" , error);
                return Json(false);
            }
        }

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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "PegaCategoria" , error);
                return Json(false);
            }
        }

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

                return Json(new
                {
                    success = true ,
                    message = $"{viagens.Count} viagem(ns) adicionada(s) com sucesso!"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "AdicionarViagensEconomildoLote" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao adicionar viagens: " + error.Message
                });
            }
        }

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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "BuscarViagemEconomildo" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao buscar viagem: " + error.Message
                });
            }
        }

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

                return Json(new
                {
                    success = true ,
                    message = "Viagem atualizada com sucesso!"
                });
            }
            catch (Exception error)
            {
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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "AdicionarEvento" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao adicionar evento"
                });
            }
        }

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

                ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                requisitante.UsuarioIdAlteracao = currentUserID;

                _unitOfWork.Requisitante.Add(requisitante);
                _unitOfWork.Save();

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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "AdicionarRequisitante" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao adicionar requisitante"
                });
            }
        }

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

                ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                setorSolicitante.UsuarioIdAlteracao = currentUserID;

                _unitOfWork.SetorSolicitante.Add(setorSolicitante);
                _unitOfWork.Save();

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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "AdicionarSetor" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao adicionar setor"
                });
            }
        }

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
                    Console.WriteLine($"[CACHE HIT] {swCache.ElapsedMilliseconds}ms");
                    return Ok(cachedResult);
                }
                swCache.Stop();
                Console.WriteLine($"[CACHE MISS] {swCache.ElapsedMilliseconds}ms");

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
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
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

        [Route("ObterTotalCustoViagensEvento")]
        [HttpGet]
        public async Task<IActionResult> ObterTotalCustoViagensEvento(Guid Id)
        {
            try
            {
                if (Id == Guid.Empty)
                {
                    return await Task.FromResult(
                        BadRequest(new
                        {
                            success = false ,
                            message = "ID do evento inválido"
                        })
                    );
                }

                var evento = await Task.Run(() =>
                    _unitOfWork.Evento.GetFirstOrDefault(e => e.EventoId == Id)
                );
                if (evento == null)
                {
                    return await Task.FromResult(
                        NotFound(new
                        {
                            success = false ,
                            message = "Evento não encontrado"
                        })
                    );
                }

                var viagens = await Task.Run(() =>
                    _unitOfWork
                        .ViewViagens.GetAll(filter: x =>
                            x.EventoId == Id && x.Status == "Realizada"
                        )
                        .ToList()
                );

                var estatisticas = new
                {
                    TotalViagens = viagens.Count() ,
                    ViagensComCusto = viagens
                        .Where(v => v.CustoViagem.HasValue && v.CustoViagem > 0)
                        .ToList() ,
                    ViagensSemCusto = viagens
                        .Where(v => !v.CustoViagem.HasValue || v.CustoViagem == 0)
                        .ToList() ,
                };

                decimal custoTotal = estatisticas.ViagensComCusto.Sum(v =>
                    (decimal)(v.CustoViagem ?? 0)
                );
                decimal custoMedio =
                    estatisticas.ViagensComCusto.Count > 0
                        ? custoTotal / estatisticas.ViagensComCusto.Count
                        : 0;

                decimal? custoMinimo =
                    estatisticas.ViagensComCusto.Count > 0
                        ? (decimal)estatisticas.ViagensComCusto.Min(v => v.CustoViagem ?? 0)
                        : (decimal?)null;

                decimal? custoMaximo =
                    estatisticas.ViagensComCusto.Count > 0
                        ? (decimal)estatisticas.ViagensComCusto.Max(v => v.CustoViagem ?? 0)
                        : (decimal?)null;

                var culturaBR = new System.Globalization.CultureInfo("pt-BR");

                return await Task.FromResult(
                    Ok(
                        new
                        {
                            success = true ,
                            eventoId = Id ,
                            nomeEvento = evento?.Nome ,
                            totalViagens = estatisticas.TotalViagens ,
                            totalViagensComCusto = estatisticas.ViagensComCusto.Count ,
                            viagensSemCusto = estatisticas.ViagensSemCusto.Count ,
                            percentualComCusto = estatisticas.TotalViagens > 0
                                ? (
                                    estatisticas.ViagensComCusto.Count
                                    * 100.0
                                    / estatisticas.TotalViagens
                                )
                                : 0 ,
                            totalCusto = custoTotal ,
                            custoMedio = custoMedio ,
                            custoMinimo = custoMinimo ?? 0 ,
                            custoMaximo = custoMaximo ?? 0 ,
                            totalCustoFormatado = custoTotal.ToString("C" , culturaBR) ,
                            custoMedioFormatado = custoMedio.ToString("C" , culturaBR) ,
                            custoMinimoFormatado = custoMinimo?.ToString("C" , culturaBR)
                                ?? "R$ 0,00" ,
                            custoMaximoFormatado = custoMaximo?.ToString("C" , culturaBR)
                                ?? "R$ 0,00" ,
                            dataConsulta = DateTime.Now ,
                        }
                    )
                );
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ObterTotalCustoViagensEvento" , error);
                var errorDetails = new
                {
                    Message = error.Message ,
                    StackTrace = error.StackTrace ,
                    InnerException = error.InnerException?.Message ,
                };

                return await Task.FromResult(
                    StatusCode(
                        500 ,
                        new
                        {
                            success = false ,
                            message = "Erro ao processar a solicitação" ,
                            error = errorDetails.Message ,
                        }
                    )
                );
            }
        }

        [Route("EstatisticasViagensEvento")]
        [HttpGet]
        public IActionResult EstatisticasViagensEvento(Guid Id)
        {
            try
            {
                var viagens = _unitOfWork
                    .ViewViagens.GetAll(filter: x => x.EventoId == Id && x.Status == "Realizada")
                    .ToList();

                var estatisticas = new
                {
                    success = true ,
                    totalViagens = viagens.Count() ,
                    viagensComCusto = viagens.Count(v =>
                        v.CustoViagem.HasValue && v.CustoViagem > 0
                    ) ,
                    viagensSemCusto = viagens.Count(v =>
                        !v.CustoViagem.HasValue || v.CustoViagem == 0
                    ) ,
                    custoTotal = viagens
                        .Where(v => v.CustoViagem.HasValue)
                        .Sum(v => v.CustoViagem.Value) ,
                    custoMedio = viagens
                        .Where(v => v.CustoViagem.HasValue && v.CustoViagem > 0)
                        .Select(v => v.CustoViagem.Value)
                        .DefaultIfEmpty(0)
                        .Average() ,
                    custoMinimo = viagens
                        .Where(v => v.CustoViagem.HasValue && v.CustoViagem > 0)
                        .Select(v => v.CustoViagem.Value)
                        .DefaultIfEmpty(0)
                        .Min() ,
                    custoMaximo = viagens
                        .Where(v => v.CustoViagem.HasValue && v.CustoViagem > 0)
                        .Select(v => v.CustoViagem.Value)
                        .DefaultIfEmpty(0)
                        .Max() ,
                    motoristas = viagens
                        .Where(v => !string.IsNullOrEmpty(v.NomeMotorista))
                        .GroupBy(v => v.NomeMotorista)
                        .Select(g => new
                        {
                            nome = g.Key ,
                            viagens = g.Count() ,
                            custoTotal = g.Sum(v => v.CustoViagem ?? 0) ,
                        })
                        .OrderByDescending(m => m.viagens)
                        .ToList() ,
                };

                return Ok(estatisticas);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "EstatisticasViagensEvento" , error);
                return Ok(new
                {
                    success = false ,
                    error = error.Message
                });
            }
        }

        [HttpPost]
        public IActionResult SaveImage(IList<IFormFile> UploadFiles)
        {
            try
            {
                if (UploadFiles == null || UploadFiles.Count == 0)
                {
                    return BadRequest("Nenhum arquivo foi enviado");
                }

                foreach (IFormFile file in UploadFiles)
                {
                    if (file.Length > 0)
                    {
                        string filename = ContentDispositionHeaderValue
                            .Parse(file.ContentDisposition)
                            .FileName.Trim('"');

                        filename = Path.GetFileName(filename);

                        string folderPath = Path.Combine(
                            hostingEnv.WebRootPath ,
                            "DadosEditaveis" ,
                            "ImagensViagens"
                        );

                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        string fullPath = Path.Combine(folderPath , filename);

                        using (var stream = new FileStream(fullPath , FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                    }
                }

                return Ok(new
                {
                    message = "Imagem(ns) salva(s) com sucesso!"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "SaveImage" , error);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Obtém estatísticas de viagens de um veículo para validação inteligente
        /// Usado pela IA evolutiva para calibrar alertas baseados no histórico
        /// </summary>
        [HttpGet]
        [Route("EstatisticasVeiculo")]
        public async Task<IActionResult> GetEstatisticasVeiculo([FromQuery] Guid veiculoId)
        {
            try
            {
                if (veiculoId == Guid.Empty)
                {
                    return Json(new
                    {
                        success = false,
                        message = "VeiculoId é obrigatório"
                    });
                }

                var estatisticas = await _veiculoEstatisticaService.ObterEstatisticasAsync(veiculoId);

                return Json(new
                {
                    success = true,
                    data = estatisticas
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs", "GetEstatisticasVeiculo", error);
                return Json(new
                {
                    success = false,
                    message = "Erro ao obter estatísticas do veículo"
                });
            }
        }

        [HttpPost]
        [Route("FinalizaViagem")]
        [Consumes("application/json")]
        public async Task<IActionResult> FinalizaViagemAsync([FromBody] FinalizacaoViagem viagem)
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

                // 1. BUSCA A VIAGEM NO BANCO
                var objViagem = _unitOfWork.Viagem.GetFirstOrDefault(v =>
                    v.ViagemId == viagem.ViagemId
                );

                if (objViagem == null)
                {
                    return Json(new
                    {
                        success = false ,
                        message = "Viagem não encontrada"
                    });
                }

                // 2. ATUALIZA OS DADOS BÁSICOS DA VIAGEM
                objViagem.DataFinal = viagem.DataFinal;
                objViagem.HoraFim = viagem.HoraFim;
                objViagem.KmFinal = viagem.KmFinal;
                objViagem.CombustivelFinal = viagem.CombustivelFinal;
                objViagem.Descricao = viagem.Descricao;
                objViagem.Status = "Realizada";
                objViagem.StatusDocumento = viagem.StatusDocumento;
                objViagem.StatusCartaoAbastecimento = viagem.StatusCartaoAbastecimento;

                // 3. REGISTRA USUÁRIO E DATA DE FINALIZAÇÃO
                ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                var currentUserName = currentUser.Identity?.Name ?? "Sistema";
                objViagem.UsuarioIdFinalizacao = currentUserID;
                objViagem.DataFinalizacao = DateTime.Now;

                // 4. ✅ CALCULA CUSTOS DE FORMA ASSÍNCRONA (AQUI ESTÁ A CORREÇÃO!)

                //// 4.1 Custo de Combustível
                //objViagem.CustoCombustivel = await ServicosAsync.CalculaCustoCombustivelAsync(
                //    objViagem ,
                //    _unitOfWork
                //);

                //// 4.2 Custo de Motorista (retorna tupla com custo e minutos)
                //var (custoMotorista, minutos) = await ServicosAsync.CalculaCustoMotoristaAsync(
                //    objViagem ,
                //    _unitOfWork
                //);
                //objViagem.CustoMotorista = custoMotorista;
                //objViagem.Minutos = minutos;

                //// 4.3 Custo de Operador (🚨 ESTE ERA O MAIS LENTO)
                //objViagem.CustoOperador = await ServicosAsync.CalculaCustoOperadorAsync(
                //    objViagem ,
                //    _unitOfWork
                //);

                //// 4.4 Custo de Lavador (🚨 ESTE TAMBÉM ERA LENTO)
                //objViagem.CustoLavador = await ServicosAsync.CalculaCustoLavadorAsync(
                //    objViagem ,
                //    _unitOfWork
                //);

                //// 4.5 Custo de Veículo
                //objViagem.CustoVeiculo = await ServicosAsync.CalculaCustoVeiculoAsync(
                //    objViagem ,
                //    _unitOfWork
                //);

                // 5. ATUALIZA VIAGEM NO BANCO
                _unitOfWork.Viagem.Update(objViagem);

                // 6. ✅ SALVA OCORRÊNCIAS MÚLTIPLAS (SE HOUVER)
                int ocorrenciasCriadas = 0;
                if (viagem.Ocorrencias != null && viagem.Ocorrencias.Any())
                {
                    foreach (var ocDto in viagem.Ocorrencias)
                    {
                        // Só cria se tiver resumo preenchido
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

                // 7. ATUALIZA QUILOMETRAGEM DO VEÍCULO
                var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(v =>
                    v.VeiculoId == objViagem.VeiculoId
                );

                if (veiculo != null)
                {
                    veiculo.Quilometragem = viagem.KmFinal;
                    _unitOfWork.Veiculo.Update(veiculo);
                }

                // 8. ✅ SALVA TUDO NO BANCO (AGUARDA CONCLUSÃO)
                _unitOfWork.Save();

                // 9. ATUALIZA ESTATÍSTICAS DO DIA
                await _viagemEstatisticaService.AtualizarEstatisticasDiaAsync((DateTime)objViagem.DataInicial);

                // 10. ✅ RETORNA SUCESSO COM INFO DE OCORRÊNCIAS
                var mensagem = "Viagem finalizada com sucesso";
                if (ocorrenciasCriadas > 0)
                {
                    mensagem += $" ({ocorrenciasCriadas} ocorrência(s) registrada(s))";
                }

                return Json(
                    new
                    {
                        success = true ,
                        message = mensagem ,
                        type = 0 ,
                        ocorrenciasCriadas = ocorrenciasCriadas
                    }
                );
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "FinalizaViagem" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao finalizar viagem: " + error.Message
                });
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
                var dataFluxoStr = Request.Form["dataFluxo"].FirstOrDefault();

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

                if (!string.IsNullOrEmpty(dataFluxoStr))
                {
                    if (DateTime.TryParse(dataFluxoStr , out var dataConv))
                    {
                        query = query.Where(v => v.Data == dataConv);
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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ObterDocumento" , error);
                return StatusCode(500);
            }
        }

        // ===============================================================================
        // INTEGRAÇÃO FROTIX MOBILE - RUBRICAS E OCORRÊNCIAS
        // ===============================================================================

        /// <summary>
        /// Retorna os dados registrados pelo Mobile: Rubrica, RubricaFinal, Documentos/Itens e Ocorrências
        /// </summary>
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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ObterDadosMobile" , error);
                return Json(new
                {
                    success = false ,
                    message = $"Erro ao obter dados mobile: {error.Message}"
                });
            }
        }

        /// <summary>
        /// Retorna a imagem de uma ocorrência específica
        /// </summary>
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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ObterImagemOcorrencia" , error);
                return Json(new
                {
                    success = false ,
                    message = $"Erro ao obter imagem: {error.Message}"
                });
            }
        }

        /// <summary>
        /// Retorna lista de ocorrências de uma viagem específica
        /// </summary>
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
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ObterOcorrenciasViagem" , error);
                return Json(new
                {
                    success = false ,
                    message = $"Erro ao obter ocorrências: {error.Message}"
                });
            }
        }
    }

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

        public int Order
        {
            get; set;
        }

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
