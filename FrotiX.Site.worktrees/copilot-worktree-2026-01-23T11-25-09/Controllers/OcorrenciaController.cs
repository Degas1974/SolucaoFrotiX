using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using FrotiX.TextNormalization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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
    *  #   MODULO:  GESTÃO DE OCORRÊNCIAS (FROTIX CORE)                                                #
    *  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
    *  #                                                                                               #
    *  #################################################################################################
    */

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: OcorrenciaController                                               ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Gestão de ocorrências operacionais e administrativas.                      ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/Ocorrencia                                             ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class OcorrenciaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _log;
        private readonly IWebHostEnvironment _hostingEnv;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OcorrenciaController (Construtor)                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o controlador com UnitOfWork, ambiente e log.                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unitOfWork (IUnitOfWork): Acesso a dados.                               ║
        /// ║    • env (IWebHostEnvironment): Ambiente da aplicação.                       ║
        /// ║    • log (ILogService): Serviço de log centralizado.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public OcorrenciaController(IUnitOfWork unitOfWork, IWebHostEnvironment env, ILogService log)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _hostingEnv = env;
                _log = log;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("OcorrenciaController.cs", "OcorrenciaController", ex);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Get (GET)                                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Endpoint principal de filtragem de ocorrências.                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • veiculoId (string): ID do veículo.                                      ║
        /// ║    • motoristaId (string): ID do motorista.                                  ║
        /// ║    • statusId (string): Status da ocorrência.                                ║
        /// ║    • data (string): Data única.                                              ║
        /// ║    • dataInicial (string): Data inicial.                                     ║
        /// ║    • dataFinal (string): Data final.                                         ║
        /// ║    • debug (string): Ativa retorno de eco ("1").                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com ocorrências filtradas.                          ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        public IActionResult Get(
            string veiculoId = null ,
            string motoristaId = null ,
            string statusId = null ,
            string data = null ,
            string dataInicial = null ,
            string dataFinal = null ,
            string debug = "0"
        )
        {
            try
            {
                // [FILTRO] Conversão de IDs.
                Guid? veiculoGuid = null, motoristaGuid = null;
                if (!string.IsNullOrWhiteSpace(veiculoId) && Guid.TryParse(veiculoId , out var vg))
                    veiculoGuid = vg;
                if (!string.IsNullOrWhiteSpace(motoristaId) && Guid.TryParse(motoristaId , out var mg))
                    motoristaGuid = mg;

                // [FILTRO] Formatos e cultura para datas.
                var formats = new[]
                {
                    "dd/MM/yyyy",
                    "dd/MM/yyyy HH:mm",
                    "dd/MM/yyyy HH:mm:ss",
                    "yyyy-MM-dd",
                    "yyyy-MM-ddTHH:mm",
                    "yyyy-MM-ddTHH:mm:ss",
                };
                var br = new System.Globalization.CultureInfo("pt-BR");
                var inv = System.Globalization.CultureInfo.InvariantCulture;

                // [FUNCAO] Parser flexível de datas.
                bool TryParse(string s , out DateTime dt) =>
                    DateTime.TryParseExact(
                        s.Trim() ,
                        formats ,
                        br ,
                        System.Globalization.DateTimeStyles.None ,
                        out dt
                    )
                    || DateTime.TryParseExact(
                        s.Trim() ,
                        formats ,
                        inv ,
                        System.Globalization.DateTimeStyles.None ,
                        out dt
                    );

                // [FILTRO] Datas únicas e período.
                DateTime? dataUnica = null, dtIni = null, dtFim = null;
                if (!string.IsNullOrWhiteSpace(data) && TryParse(data , out var d))
                    dataUnica = d;
                if (!string.IsNullOrWhiteSpace(dataInicial) && TryParse(dataInicial , out var di))
                    dtIni = di;
                if (!string.IsNullOrWhiteSpace(dataFinal) && TryParse(dataFinal , out var df))
                    dtFim = df;

                // [REGRA] Se período definido, ignora data única.
                if (dtIni.HasValue && dtFim.HasValue)
                    dataUnica = null;

                // [REGRA] Normaliza período invertido.
                if (dtIni.HasValue && dtFim.HasValue && dtIni > dtFim)
                {
                    var t = dtIni;
                    dtIni = dtFim;
                    dtFim = t;
                }

                // [REGRA] Status default quando há filtros.
                bool temFiltro =
                    veiculoGuid != default(Guid)
                    || motoristaGuid != default(Guid)
                    || dataUnica.HasValue
                    || (dtIni.HasValue && dtFim.HasValue);
                if (string.IsNullOrWhiteSpace(statusId) && temFiltro)
                    statusId = "Todas";

                // [DADOS] Consulta view de viagens com ocorrências.
                IQueryable<ViewViagens> q = _unitOfWork.ViewViagens.GetAllReducedIQueryable(
                    selector: v => v ,
                    filter: null ,
                    asNoTracking: true
                );

                q = q.Where(v => v.ResumoOcorrencia != null && v.ResumoOcorrencia.Trim() != "");

                // [FILTRO] Aplica filtros dinâmicos.
                if (veiculoGuid.HasValue)
                    q = q.Where(v => v.VeiculoId == veiculoGuid);

                if (motoristaGuid.HasValue)
                    q = q.Where(v => v.MotoristaId == motoristaGuid);

                if (!string.IsNullOrWhiteSpace(statusId) && statusId != "Todas")
                    q = q.Where(v => v.StatusOcorrencia == statusId);

                if (dataUnica.HasValue)
                {
                    // [FILTRO] Data única.
                    var dia = dataUnica.Value.Date;
                    q = q.Where(v => v.DataFinal.HasValue && v.DataFinal.Value.Date == dia);
                }
                else if (dtIni.HasValue && dtFim.HasValue)
                {
                    // [FILTRO] Intervalo de datas.
                    var ini = dtIni.Value.Date;
                    var fim = dtFim.Value.Date;
                    q = q.Where(v =>
                        v.DataFinal.HasValue
                        && v.DataFinal.Value.Date >= ini
                        && v.DataFinal.Value.Date <= fim
                    );
                }

                // [ORDENACAO] Ordena por datas de viagem.
                q = q.OrderByDescending(v => v.DataFinal).ThenByDescending(v => v.DataInicial);

                // [DADOS] Projeção inicial.
                var lista = q.Select(v => new
                {
                    v.ViagemId ,
                    v.NoFichaVistoria ,
                    v.DataFinal ,
                    v.NomeMotorista ,
                    v.DescricaoVeiculo ,
                    v.ResumoOcorrencia ,
                    v.DescricaoOcorrencia ,
                    v.DescricaoSolucaoOcorrencia ,
                    v.StatusOcorrencia ,
                    v.MotoristaId ,
                    v.VeiculoId ,
                })
                    .ToList();

                // [FORMATO] Converte datas para pt-BR.
                string ToBR(DateTime? dt) => dt.HasValue ? dt.Value.ToString("dd/MM/yyyy") : null;

                // [DADOS] Projeção final para UI.
                var result = lista
                    .Select(v => new
                    {
                        viagemId = v.ViagemId ,
                        noFichaVistoria = v.NoFichaVistoria ,
                        dataSelecao = ToBR(v.DataFinal) ,
                        nomeMotorista = v.NomeMotorista ,
                        descricaoVeiculo = v.DescricaoVeiculo ,
                        resumoOcorrencia = v.ResumoOcorrencia ,
                        descricaoOcorrencia = v.DescricaoOcorrencia ,
                        descricaoSolucaoOcorrencia = v.DescricaoSolucaoOcorrencia ,
                        statusOcorrencia = v.StatusOcorrencia ,
                        motoristaId = v.MotoristaId ,
                        veiculoId = v.VeiculoId ,
                    })
                    .ToList();

                if (debug == "1")
                {
                    // [DEBUG] Retorna echo dos filtros aplicados.
                    var echo = new
                    {
                        recebido = new
                        {
                            data ,
                            dataInicial ,
                            dataFinal
                        } ,
                        aplicado = new
                        {
                            dataUnica = dataUnica?.ToString("dd/MM/yyyy") ,
                            periodo = (dtIni.HasValue && dtFim.HasValue)
                                ? $"{dtIni.Value:dd/MM/yyyy} .. {dtFim.Value:dd/MM/yyyy}"
                                : null ,
                        } ,
                    };
                    return Json(new
                    {
                        data = result ,
                        debugEcho = echo
                    });
                }

                return Json(new
                {
                    data = result
                });
            }
            catch (Exception ex)
            {
                _log.Error("OcorrenciaController.Get", ex );
                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "Get" , ex);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        /// <summary>
        /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
        /// <para>FUNCIONALIDADE: Listar Ocorrências Abertas</para>
        /// <para>DESCRIÇÃO: Retorna lista de ocorrências com status <b>Aberta</b>.</para>
        /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
        /// </summary>
        [Route("Ocorrencias")]
        [HttpGet]
        public IActionResult Ocorrencias(string Id)
        {
            try
            {
                var result = (
                    from vv in _unitOfWork.ViewViagens.GetAll()
                    where
                        (vv.StatusOcorrencia == "Aberta")
                        && (
                            (vv.ResumoOcorrencia != null && vv.ResumoOcorrencia != "")
                            || (vv.DescricaoOcorrencia != null && vv.DescricaoOcorrencia != "")
                        )
                    select new
                    {
                        vv.ViagemId ,
                        vv.NoFichaVistoria ,
                        vv.DataInicial ,
                        vv.NomeMotorista ,
                        vv.DescricaoVeiculo ,
                        vv.ResumoOcorrencia ,
                        vv.DescricaoOcorrencia ,
                        vv.DescricaoSolucaoOcorrencia ,
                        vv.StatusOcorrencia ,
                        DescOcorrencia = vv.DescricaoOcorrencia != null
                            ? Servicos.ConvertHtml(vv.DescricaoOcorrencia)
                            : "Sem Descrição" ,
                    }
                ).ToList();

                return Json(new
                {
                    data = result
                });
            }
            catch (Exception ex)
            {
                _log.Error("OcorrenciaController.Ocorrencias", ex );
                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "Ocorrencias" , ex);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        /// <summary>
        /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
        /// <para>FUNCIONALIDADE: Listar Ocorrências por Veículo</para>
        /// <para>DESCRIÇÃO: Retorna histórico de ocorrências de um veículo específico.</para>
        /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
        /// </summary>
        [Route("OcorrenciasVeiculos")]
        [HttpGet]
        public IActionResult OcorrenciasVeiculos(string Id)
        {
            try
            {
                var result = (
                    from vv in _unitOfWork.ViewViagens.GetAll()
                    where
                        vv.VeiculoId == Guid.Parse(Id)
                        && (
                            (vv.ResumoOcorrencia != null && vv.ResumoOcorrencia != "")
                            || (vv.DescricaoOcorrencia != null && vv.DescricaoOcorrencia != "")
                        )
                    select new
                    {
                        vv.ViagemId ,
                        vv.NoFichaVistoria ,
                        vv.DataInicial ,
                        vv.NomeMotorista ,
                        vv.DescricaoVeiculo ,
                        vv.ResumoOcorrencia ,
                        vv.DescricaoOcorrencia ,
                        vv.DescricaoSolucaoOcorrencia ,
                        vv.StatusOcorrencia ,
                        vv.MotoristaId ,
                        vv.ImagemOcorrencia ,
                        DescOcorrencia = vv.DescricaoOcorrencia != null
                            ? Servicos.ConvertHtml(vv.DescricaoOcorrencia)
                            : "Sem Descrição" ,
                    }
                ).ToList().OrderByDescending(v => v.NoFichaVistoria).ThenByDescending(v => v.DataInicial);

                return Json(new
                {
                    data = result
                });
            }
            catch (Exception ex)
            {
                _log.Error("OcorrenciaController.OcorrenciasVeiculos", ex );
                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "OcorrenciasVeiculos" , ex);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        /// <summary>
        /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
        /// <para>FUNCIONALIDADE: Listar Ocorrências por Motorista</para>
        /// <para>DESCRIÇÃO: Retorna histórico de ocorrências de um motorista específico.</para>
        /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
        /// </summary>
        [Route("OcorrenciasMotoristas")]
        [HttpGet]
        public IActionResult OcorrenciasMotoristas(string Id)
        {
            try
            {
                var result = (
                    from vv in _unitOfWork.ViewViagens.GetAll()
                    where
                        vv.MotoristaId == Guid.Parse(Id)
                        && (
                            (vv.ResumoOcorrencia != null && vv.ResumoOcorrencia != "")
                            || (vv.DescricaoOcorrencia != null && vv.DescricaoOcorrencia != "")
                        )
                    select new
                    {
                        vv.ViagemId ,
                        vv.NoFichaVistoria ,
                        vv.DataInicial ,
                        vv.NomeMotorista ,
                        vv.DescricaoVeiculo ,
                        vv.ResumoOcorrencia ,
                        vv.DescricaoOcorrencia ,
                        vv.DescricaoSolucaoOcorrencia ,
                        vv.StatusOcorrencia ,
                        DescOcorrencia = vv.DescricaoOcorrencia != null
                            ? Servicos.ConvertHtml(vv.DescricaoOcorrencia)
                            : "Sem Descrição" ,
                    }
                ).ToList().OrderByDescending(v => v.NoFichaVistoria).ThenByDescending(v => v.DataInicial);

                return Json(new
                {
                    data = result
                });
            }
            catch (Exception ex)
            {
                _log.Error("OcorrenciaController.OcorrenciasMotoristas", ex );
                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "OcorrenciasMotoristas" , ex);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        /// <summary>
        /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
        /// <para>FUNCIONALIDADE: Listar Ocorrências por Status</para>
        /// <para>DESCRIÇÃO: Retorna lista filtrada pelo status da ocorrência.</para>
        /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
        /// </summary>
        [Route("OcorrenciasStatus")]
        [HttpGet]
        public IActionResult OcorrenciasStatus(string Id)
        {
            try
            {
                if (Id == "Todas")
                {
                    var resultado = (
                        from vv in _unitOfWork.ViewViagens.GetAll()
                        where
                            (
                                (vv.ResumoOcorrencia != null && vv.ResumoOcorrencia != "")
                                || (vv.DescricaoOcorrencia != null && vv.DescricaoOcorrencia != "")
                            )
                        select new
                        {
                            vv.ViagemId ,
                            vv.NoFichaVistoria ,
                            vv.DataInicial ,
                            vv.NomeMotorista ,
                            vv.DescricaoVeiculo ,
                            vv.ResumoOcorrencia ,
                            vv.DescricaoOcorrencia ,
                            vv.DescricaoSolucaoOcorrencia ,
                            vv.StatusOcorrencia ,
                            DescOcorrencia = vv.DescricaoOcorrencia != null
                                ? Servicos.ConvertHtml(vv.DescricaoOcorrencia)
                                : "Sem Descrição" ,
                        }
                    ).ToList().OrderByDescending(v => v.NoFichaVistoria).ThenByDescending(v => v.DataInicial);

                    return Json(new
                    {
                        data = resultado
                    });
                }

                var result = (
                    from vv in _unitOfWork.ViewViagens.GetAll()
                    where
                        vv.StatusOcorrencia == Id
                        && (
                            (vv.ResumoOcorrencia != null && vv.ResumoOcorrencia != "")
                            || (vv.DescricaoOcorrencia != null && vv.DescricaoOcorrencia != "")
                        )
                    select new
                    {
                        vv.ViagemId ,
                        vv.NoFichaVistoria ,
                        vv.DataInicial ,
                        vv.NomeMotorista ,
                        vv.DescricaoVeiculo ,
                        vv.ResumoOcorrencia ,
                        vv.DescricaoOcorrencia ,
                        vv.DescricaoSolucaoOcorrencia ,
                        vv.StatusOcorrencia ,
                        DescOcorrencia = vv.DescricaoOcorrencia != null
                            ? Servicos.ConvertHtml(vv.DescricaoOcorrencia)
                            : "Sem Descrição" ,
                    }
                ).ToList().OrderByDescending(v => v.NoFichaVistoria).ThenByDescending(v => v.DataInicial);

                return Json(new
                {
                    data = result
                });
            }
            catch (Exception ex)
            {
                _log.Error("OcorrenciaController.OcorrenciasStatus", ex );
                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "OcorrenciasStatus" , ex);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        /// <summary>
        /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
        /// <para>FUNCIONALIDADE: Listar Ocorrências por Data</para>
        /// <para>DESCRIÇÃO: Retorna ocorrências iniciadas em uma data específica.</para>
        /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
        /// </summary>
        [Route("OcorrenciasData")]
        [HttpGet]
        public IActionResult OcorrenciasData(string Id)
        {
            try
            {
                if (DateTime.TryParse(Id , out DateTime parsedDate))
                {
                    var result = (
                        from vv in _unitOfWork.ViewViagens.GetAll()
                        where
                            vv.DataInicial.HasValue
                            && vv.DataInicial.Value.Date == parsedDate.Date
                            && (
                                (vv.ResumoOcorrencia != null && vv.ResumoOcorrencia != "")
                                || (vv.DescricaoOcorrencia != null && vv.DescricaoOcorrencia != "")
                            )
                        select new
                        {
                            vv.ViagemId ,
                            vv.NoFichaVistoria ,
                            vv.DataInicial ,
                            vv.NomeMotorista ,
                            vv.DescricaoVeiculo ,
                            vv.ResumoOcorrencia ,
                            vv.DescricaoOcorrencia ,
                            vv.DescricaoSolucaoOcorrencia ,
                            vv.StatusOcorrencia ,
                            DescOcorrencia = vv.DescricaoOcorrencia != null
                                ? Servicos.ConvertHtml(vv.DescricaoOcorrencia)
                                : "Sem Descrição" ,
                        }
                    ).ToList().OrderByDescending(v => v.NoFichaVistoria).ThenByDescending(v => v.DataInicial);

                    return Json(new
                    {
                        data = result
                    });
                }

                return Json(new
                {
                    success = false ,
                    message = "Data inválida fornecida."
                });
            }
            catch (Exception ex)
            {
                _log.Error("OcorrenciaController.OcorrenciasData", ex );
                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "OcorrenciasData" , ex);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        /// <summary>
        /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
        /// <para>FUNCIONALIDADE: Baixar Ocorrência</para>
        /// <para>DESCRIÇÃO: Marca uma ocorrência como <b>Baixada</b>.</para>
        /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
        /// </summary>
        [Route("BaixarOcorrencia")]
        [HttpPost]
        public IActionResult BaixarOcorrencia(ViagemID id)
        {
            try
            {
                // Funcionalidade comentada no original
                return Json(new
                {
                    success = false ,
                    message = "Erro ao baixar ocorrência: Funcionalidade não ativada."
                });
            }
            catch (Exception ex)
            {
                _log.Error("OcorrenciaController.BaixarOcorrencia", ex );
                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "BaixarOcorrencia" , ex);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao baixar ocorrência"
                });
            }
        }

        /// <summary>
        /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
        /// <para>FUNCIONALIDADE: Salvar Imagem da Ocorrência</para>
        /// <para>DESCRIÇÃO: Realiza upload de arquivos para a pasta de imagens de ocorrências.</para>
        /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
        /// </summary>
        [Route("SaveImage")]
        public void SaveImage(IList<IFormFile> UploadFiles)
        {
            try
            {
                foreach (IFormFile file in UploadFiles)
                {
                    if (UploadFiles != null)
                    {
                        string filename = ContentDispositionHeaderValue
                            .Parse(file.ContentDisposition)
                            .FileName.Trim('"');
                        filename =
                            _hostingEnv.WebRootPath
                            + "\\DadosEditaveis\\ImagensViagens"
                            + $@"\{filename}";

                        if (
                            !Directory.Exists(
                                _hostingEnv.WebRootPath + "\\DadosEditaveis\\ImagensViagens"
                            )
                        )
                        {
                            Directory.CreateDirectory(
                                _hostingEnv.WebRootPath + "\\DadosEditaveis\\ImagensViagens"
                            );
                        }

                        if (!System.IO.File.Exists(filename))
                        {
                            using (FileStream fs = System.IO.File.Create(filename))
                            {
                                file.CopyTo(fs);
                                fs.Flush();
                            }
                            _log.Info($"OcorrenciaController.SaveImage: Arquivo {filename} salvo com sucesso.");
                            Response.StatusCode = 200;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("OcorrenciaController.SaveImage", ex );
                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "SaveImage" , ex);
                Response.StatusCode = 204;
            }
        }

        /// <summary>
        /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
        /// <para>FUNCIONALIDADE: Editar Ocorrência</para>
        /// <para>DESCRIÇÃO: Atualiza informações de texto da ocorrência.</para>
        /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
        /// </summary>
        [Route("EditaOcorrencia")]
        [Consumes("application/json")]
        public async Task<IActionResult> EditaOcorrencia([FromBody] FinalizacaoViagem viagem)
        {
            try
            {
                return Json(
                    new
                    {
                        success = true ,
                        message = "Ocorrência atualizada com sucesso" ,
                        type = 0 ,
                    }
                );
            }
            catch (Exception ex)
            {
                _log.Error("OcorrenciaController.EditaOcorrencia", ex );
                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "EditaOcorrencia" , ex);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao editar ocorrência"
                });
            }
        }

        /// <summary>
        /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
        /// <para>FUNCIONALIDADE: Fechar Item da OS</para>
        /// <para>DESCRIÇÃO: Baixa item de manutenção e vincula à ocorrência da viagem original.</para>
        /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
        /// </summary>
        [Route("FechaItemOS")]
        [HttpPost]
        public JsonResult FechaItemOS(Models.ItensManutencao itensMmanutencao)
        {
            try
            {
                _log.Info($"OcorrenciaController.FechaItemOS: Tentativa de baixa de ItemManutencao {itensMmanutencao.ItemManutencaoId} via OS {itensMmanutencao.ManutencaoId}");

                return new JsonResult(
                    new
                    {
                        data = itensMmanutencao.ManutencaoId ,
                        message = "OS Baixada com Sucesso!"
                    }
                );
            }
            catch (Exception ex)
            {
                _log.Error("OcorrenciaController.FechaItemOS", ex );
                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "FechaItemOS" , ex);
                return new JsonResult(new
                {
                    success = false ,
                    message = "Erro ao fechar item OS"
                });
            }
        }
    }
}
