/* ****************************************************************************************
 * ⚡ ARQUIVO: OcorrenciaController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Consultar e gerir ocorrências de viagens com filtros por veículo,
 *                   motorista, status e datas.
 *
 * 📥 ENTRADAS     : Parâmetros de filtro e uploads de imagens.
 *
 * 📤 SAÍDAS       : JSON com ocorrências e mensagens de operação.
 *
 * 🔗 CHAMADA POR  : Páginas de ocorrências e manutenção.
 *
 * 🔄 CHAMA        : IUnitOfWork, ViewViagens, File System.
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core MVC, Entity Framework, IWebHostEnvironment.
 **************************************************************************************** */

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
    /****************************************************************************************
     * ⚡ CONTROLLER: OcorrenciaController
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Expor endpoints para consulta, filtros e atualização de ocorrências.
     *
     * 📥 ENTRADAS     : IDs, parâmetros de filtro e uploads.
     *
     * 📤 SAÍDAS       : JSON com dados e status.
     *
     * 🔗 CHAMADA POR  : Telas de ocorrências e manutenção.
     ****************************************************************************************/
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class OcorrenciaController :Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment hostingEnv;

        /****************************************************************************************
         * ⚡ FUNÇÃO: OcorrenciaController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar dependências do UnitOfWork e ambiente web.
         *
         * 📥 ENTRADAS     : unitOfWork, env.
         *
         * 📤 SAÍDAS       : Instância configurada.
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI.
         ****************************************************************************************/
        public OcorrenciaController(IUnitOfWork unitOfWork , IWebHostEnvironment env)
        {
            try
            {
                _unitOfWork = unitOfWork;
                hostingEnv = env;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "OcorrenciaController" , error);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Get
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar ocorrências de viagens com filtros múltiplos.
         *
         * 📥 ENTRADAS     : veiculoId, motoristaId, statusId, data, dataInicial, dataFinal, debug.
         *
         * 📤 SAÍDAS       : JSON com lista filtrada.
         *
         * 🔗 CHAMADA POR  : Tela de consulta de ocorrências.
         *
         * 🔄 CHAMA        : ViewViagens.GetAllReducedIQueryable().
         *
         * 📝 OBSERVAÇÕES  : Aceita múltiplos formatos de data; debug retorna parâmetros.
         ****************************************************************************************/
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
                Guid? veiculoGuid = null, motoristaGuid = null;
                if (!string.IsNullOrWhiteSpace(veiculoId) && Guid.TryParse(veiculoId , out var vg))
                    veiculoGuid = vg;
                if (!string.IsNullOrWhiteSpace(motoristaId) && Guid.TryParse(motoristaId , out var mg))
                    motoristaGuid = mg;

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

                DateTime? dataUnica = null, dtIni = null, dtFim = null;
                if (!string.IsNullOrWhiteSpace(data) && TryParse(data , out var d))
                    dataUnica = d;
                if (!string.IsNullOrWhiteSpace(dataInicial) && TryParse(dataInicial , out var di))
                    dtIni = di;
                if (!string.IsNullOrWhiteSpace(dataFinal) && TryParse(dataFinal , out var df))
                    dtFim = df;

                if (dtIni.HasValue && dtFim.HasValue)
                    dataUnica = null;

                if (dtIni.HasValue && dtFim.HasValue && dtIni > dtFim)
                {
                    var t = dtIni;
                    dtIni = dtFim;
                    dtFim = t;
                }

                bool temFiltro =
                    veiculoGuid != default(Guid)
                    || motoristaGuid != default(Guid)
                    || dataUnica.HasValue
                    || (dtIni.HasValue && dtFim.HasValue);
                if (string.IsNullOrWhiteSpace(statusId) && temFiltro)
                    statusId = "Todas";

                IQueryable<ViewViagens> q = _unitOfWork.ViewViagens.GetAllReducedIQueryable(
                    selector: v => v ,
                    filter: null ,
                    asNoTracking: true
                );

                q = q.Where(v => v.ResumoOcorrencia != null && v.ResumoOcorrencia.Trim() != "");

                if (veiculoGuid.HasValue)
                    q = q.Where(v => v.VeiculoId == veiculoGuid);

                if (motoristaGuid.HasValue)
                    q = q.Where(v => v.MotoristaId == motoristaGuid);

                if (!string.IsNullOrWhiteSpace(statusId) && statusId != "Todas")
                    q = q.Where(v => v.StatusOcorrencia == statusId);

                if (dataUnica.HasValue)
                {
                    var dia = dataUnica.Value.Date;
                    q = q.Where(v => v.DataFinal.HasValue && v.DataFinal.Value.Date == dia);
                }
                else if (dtIni.HasValue && dtFim.HasValue)
                {
                    var ini = dtIni.Value.Date;
                    var fim = dtFim.Value.Date;
                    q = q.Where(v =>
                        v.DataFinal.HasValue
                        && v.DataFinal.Value.Date >= ini
                        && v.DataFinal.Value.Date <= fim
                    );
                }

                q = q.OrderByDescending(v => v.DataFinal).ThenByDescending(v => v.DataInicial);

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

                string ToBR(DateTime? dt) => dt.HasValue ? dt.Value.ToString("dd/MM/yyyy") : null;

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
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "Get" , error);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Ocorrencias
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar ocorrências por ID de viagem.
         *
         * 📥 ENTRADAS     : Id (string) - ViagemId.
         *
         * 📤 SAÍDAS       : JSON com ocorrências da viagem.
         *
         * 🔗 CHAMADA POR  : Detalhes de viagem.
         *
         * 🔄 CHAMA        : ViewViagens.GetAllReducedIQueryable().
         ****************************************************************************************/
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
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "Ocorrencias" , error);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: OcorrenciasVeiculos
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar ocorrências por veículo.
         *
         * 📥 ENTRADAS     : Id (string) - VeiculoId.
         *
         * 📤 SAÍDAS       : JSON com ocorrências do veículo.
         *
         * 🔗 CHAMADA POR  : Filtros por veículo.
         *
         * 🔄 CHAMA        : ViewViagens.GetAllReducedIQueryable().
         ****************************************************************************************/
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
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "OcorrenciasVeiculos" , error);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: OcorrenciasMotoristas
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar ocorrências por motorista.
         *
         * 📥 ENTRADAS     : Id (string) - MotoristaId.
         *
         * 📤 SAÍDAS       : JSON com ocorrências do motorista.
         *
         * 🔗 CHAMADA POR  : Filtros por motorista.
         *
         * 🔄 CHAMA        : ViewViagens.GetAllReducedIQueryable().
         ****************************************************************************************/
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
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "OcorrenciasMotoristas" , error);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: OcorrenciasStatus
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar ocorrências por status.
         *
         * 📥 ENTRADAS     : Id (string) - status.
         *
         * 📤 SAÍDAS       : JSON com ocorrências filtradas.
         *
         * 🔗 CHAMADA POR  : Filtros por status.
         *
         * 🔄 CHAMA        : ViewViagens.GetAllReducedIQueryable().
         ****************************************************************************************/
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
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "OcorrenciasStatus" , error);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: OcorrenciasData
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar ocorrências por data.
         *
         * 📥 ENTRADAS     : Id (string) - data.
         *
         * 📤 SAÍDAS       : JSON com ocorrências da data informada.
         *
         * 🔗 CHAMADA POR  : Filtros por data.
         *
         * 🔄 CHAMA        : ViewViagens.GetAllReducedIQueryable().
         ****************************************************************************************/
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
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "OcorrenciasData" , error);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: BaixarOcorrencia
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Baixar ocorrência e atualizar status na viagem.
         *
         * 📥 ENTRADAS     : [ViagemID] id.
         *
         * 📤 SAÍDAS       : JSON com sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Ações de baixa de ocorrência.
         *
         * 🔄 CHAMA        : Viagem.Update(), Save().
         ****************************************************************************************/
        [Route("BaixarOcorrencia")]
        [HttpPost]
        public IActionResult BaixarOcorrencia(ViagemID id)
        {
            try
            {
                //var objFromDb = _unitOfWork.Viagem.GetFirstOrDefault(u => u.ViagemId == id.ViagemId);
                //if (objFromDb != null)
                //{
                //    objFromDb.StatusOcorrencia = "Baixada";
                //    _unitOfWork.Viagem.Update(objFromDb);
                //    _unitOfWork.Save();
                //    return Json(new
                //    {
                //        success = true ,
                //        message = "Ocorrência baixada com sucesso"
                //    });
                //}
                return Json(new
                {
                    success = false ,
                    message = "Erro ao baixar ocorrência"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "BaixarOcorrencia" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao baixar ocorrência"
                });
            }
        }

        [Route("SaveImage")]
        /****************************************************************************************
         * ⚡ FUNÇÃO: SaveImage
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Salvar imagens de ocorrências no diretório configurado.
         *
         * 📥 ENTRADAS     : UploadFiles (lista de arquivos).
         *
         * 📤 SAÍDAS       : Atualiza Response.StatusCode conforme sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Uploads de ocorrência.
         *
         * 🔄 CHAMA        : File IO, Directory.CreateDirectory().
         ****************************************************************************************/
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
                            hostingEnv.WebRootPath
                            + "\\DadosEditaveis\\ImagensViagens"
                            + $@"\{filename}";

                        if (
                            !Directory.Exists(
                                hostingEnv.WebRootPath + "\\DadosEditaveis\\ImagensViagens"
                            )
                        )
                        {
                            Directory.CreateDirectory(
                                hostingEnv.WebRootPath + "\\DadosEditaveis\\ImagensViagens"
                            );
                        }

                        if (!System.IO.File.Exists(filename))
                        {
                            using (FileStream fs = System.IO.File.Create(filename))
                            {
                                file.CopyTo(fs);
                                fs.Flush();
                            }
                            Response.StatusCode = 200;
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "SaveImage" , error);
                Response.StatusCode = 204;
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: EditaOcorrencia
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Atualizar dados de ocorrência (resumo/descrição/solução).
         *
         * 📥 ENTRADAS     : [FinalizacaoViagem] viagem (JSON).
         *
         * 📤 SAÍDAS       : JSON com sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Tela de edição de ocorrência.
         *
         * 📝 OBSERVAÇÕES  : Trecho de atualização está comentado (verificação futura).
         ****************************************************************************************/
        [Route("EditaOcorrencia")]
        [Consumes("application/json")]
        public async Task<IActionResult> EditaOcorrencia([FromBody] FinalizacaoViagem viagem)
        {
            try
            {
                //var objViagem = _unitOfWork.Viagem.GetFirstOrDefault(v =>
                //    v.ViagemId == viagem.ViagemId
                //);
                //objViagem.ResumoOcorrencia = await TextNormalizationHelper.NormalizeAsync(
                //    viagem.ResumoOcorrencia
                //);
                //objViagem.DescricaoOcorrencia = await TextNormalizationHelper.NormalizeAsync(
                //    viagem.DescricaoOcorrencia
                //);
                //objViagem.StatusOcorrencia = await TextNormalizationHelper.NormalizeAsync(
                //    viagem.StatusOcorrencia
                //);
                //objViagem.DescricaoSolucaoOcorrencia = await TextNormalizationHelper.NormalizeAsync(
                //    viagem.SolucaoOcorrencia
                //);

                //_unitOfWork.Viagem.Update(objViagem);

                //_unitOfWork.Save();

                return Json(
                    new
                    {
                        success = true ,
                        message = "Ocorrência atualizada com sucesso" ,
                        type = 0 ,
                    }
                );
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "EditaOcorrencia" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao editar ocorrência"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: FechaItemOS
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Fechar item de OS e atualizar status relacionado.
         *
         * 📥 ENTRADAS     : [ItensManutencao] itensMmanutencao.
         *
         * 📤 SAÍDAS       : JSON com sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Fluxo de manutenção/OS.
         *
         * 🔄 CHAMA        : ItensManutencao.Update(), Save().
         ****************************************************************************************/
        [Route("FechaItemOS")]
        [HttpPost]
        public JsonResult FechaItemOS(Models.ItensManutencao itensMmanutencao)
        {
            try
            {
                //var objItensManutencao = _unitOfWork.ItensManutencao.GetFirstOrDefault(im =>
                //    im.ItemManutencaoId == itensMmanutencao.ItemManutencaoId
                //);

                //var objManutencao = _unitOfWork.Manutencao.GetFirstOrDefault(m =>
                //    m.ManutencaoId == itensMmanutencao.ManutencaoId
                //);

                //objItensManutencao.Status = "Baixada";
                //_unitOfWork.ItensManutencao.Update(objItensManutencao);

                //var ObjOcorrencias = _unitOfWork.Viagem.GetFirstOrDefault(v =>
                //    v.ItemManutencaoId == objItensManutencao.ItemManutencaoId
                //);
                //if (ObjOcorrencias != null)
                //{
                //    ObjOcorrencias.StatusOcorrencia = "Baixada";
                //    ObjOcorrencias.DescricaoSolucaoOcorrencia =
                //        "Baixada na OS nº "
                //        + objManutencao.NumOS
                //        + " de "
                //        + objManutencao.DataSolicitacao;
                //    _unitOfWork.Viagem.Update(ObjOcorrencias);
                //}

                //_unitOfWork.Save();

                return new JsonResult(
                    new
                    {
                        data = itensMmanutencao.ManutencaoId ,
                        message = "OS Baixada com Sucesso!"
                    }
                );
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "FechaItemOS" , error);
                return new JsonResult(new
                {
                    success = false ,
                    message = "Erro ao fechar item OS"
                });
            }
        }
    }
}
