using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using FrotiX.TextNormalization;
using FrotiX.Helpers; // Added
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
    *  #   MODULO:  GESTÃO DE VIAGENS (OCORRÊNCIAS)                                                  #
    *  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
    *  #                                                                                               #
    *  #################################################################################################
    */

    /// <summary>
    /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
    /// <para>CLASSE: <c>OcorrenciaViagemController</c> (Gestão)</para>
    /// <para>DESCRIÇÃO: Extensão parcial para rotas da página de Gestão de Ocorrências.</para>
    /// <para>PADRÃO: FrotiX 2026 - (IA) Documented & Modernized </para>
    /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
    /// </summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OcorrenciaViagemController (Gestão)                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Extensão parcial para rotas da página de Gestão de Ocorrências.            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public partial class OcorrenciaViagemController
    {
        #region LISTAR PARA GESTÃO

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ListarGestao (GET)                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista ocorrências para a página de gestão com filtros.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • veiculoId (string): ID do veículo.                                      ║
        /// ║    • motoristaId (string): ID do motorista.                                  ║
        /// ║    • statusId (string): Status da ocorrência.                                ║
        /// ║    • data (string): Data única.                                              ║
        /// ║    • dataInicial (string): Data inicial.                                     ║
        /// ║    • dataFinal (string): Data final.                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com ocorrências filtradas.                          ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("ListarGestao")]
        public IActionResult ListarGestao(
            string veiculoId = null ,
            string motoristaId = null ,
            string statusId = null ,
            string data = null ,
            string dataInicial = null ,
            string dataFinal = null)
        {
            try
            {
                // [FILTRO] Conversão de IDs.
                Guid? veiculoGuid = null, motoristaGuid = null;
                if (!string.IsNullOrWhiteSpace(veiculoId) && Guid.TryParse(veiculoId , out var vg))
                    veiculoGuid = vg;
                if (!string.IsNullOrWhiteSpace(motoristaId) && Guid.TryParse(motoristaId , out var mg))
                    motoristaGuid = mg;

                // [FILTRO] Formatos e cultura de datas.
                var formats = new[]
                {
                    "dd/MM/yyyy",
                    "dd/MM/yyyy HH:mm",
                    "dd/MM/yyyy HH:mm:ss",
                    "yyyy-MM-dd",
                    "yyyy-MM-ddTHH:mm",
                    "yyyy-MM-ddTHH:mm:ss",
                };
                var br = new CultureInfo("pt-BR");
                var inv = CultureInfo.InvariantCulture;

                // [FUNCAO] Parser flexível de datas.
                bool TryParseDt(string s , out DateTime dt) =>
                    DateTime.TryParseExact(s?.Trim() ?? "" , formats , br , DateTimeStyles.None , out dt)
                    || DateTime.TryParseExact(s?.Trim() ?? "" , formats , inv , DateTimeStyles.None , out dt);

                // [FILTRO] Datas únicas e período.
                DateTime? dataUnica = null, dtIni = null, dtFim = null;
                if (!string.IsNullOrWhiteSpace(data) && TryParseDt(data , out var d))
                    dataUnica = d;
                if (!string.IsNullOrWhiteSpace(dataInicial) && TryParseDt(dataInicial , out var di))
                    dtIni = di;
                if (!string.IsNullOrWhiteSpace(dataFinal) && TryParseDt(dataFinal , out var df))
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
                bool temFiltro = veiculoGuid.HasValue || motoristaGuid.HasValue || dataUnica.HasValue || (dtIni.HasValue && dtFim.HasValue);
                if (string.IsNullOrWhiteSpace(statusId) && temFiltro)
                    statusId = "Todas";

                // [DADOS] Consulta base de ocorrências.
                var ocorrenciasQuery = _unitOfWork.OcorrenciaViagem.GetAll().AsQueryable();

                // [FILTRO] Aplica filtros dinâmicos.
                if (veiculoGuid.HasValue)
                    ocorrenciasQuery = ocorrenciasQuery.Where(x => x.VeiculoId == veiculoGuid);

                if (motoristaGuid.HasValue)
                    ocorrenciasQuery = ocorrenciasQuery.Where(x => x.MotoristaId == motoristaGuid);

                // CORREÇÃO: Tratar StatusOcorrencia NULL como "Aberta"
                // No banco: NULL ou true = Aberta, false = Baixada
                // Em SQL: NULL != false retorna NULL, não true. Precisamos ser explícitos.
                if (!string.IsNullOrWhiteSpace(statusId) && statusId != "Todas")
                {
                    if (statusId == "Aberta")
                    {
                        // Aberta = StatusOcorrencia é NULL ou true, OU Status == "Aberta"
                        // Exclui Pendente e items em Manutenção
                        ocorrenciasQuery = ocorrenciasQuery.Where(x =>
                            ((x.StatusOcorrencia == null || x.StatusOcorrencia == true || x.Status == "Aberta")
                            && x.Status != "Pendente"
                            && x.ItemManutencaoId == null));
                    }
                    else if (statusId == "Baixada")
                    {
                        // Baixada = StatusOcorrencia == false OU Status == "Baixada"
                        ocorrenciasQuery = ocorrenciasQuery.Where(x =>
                            x.StatusOcorrencia == false ||
                            x.Status == "Baixada");
                    }
                    else if (statusId == "Pendente")
                    {
                        // Pendente = Status == "Pendente"
                        ocorrenciasQuery = ocorrenciasQuery.Where(x => x.Status == "Pendente");
                    }
                    else if (statusId == "Manutenção")
                    {
                        // Manutenção = tem ItemManutencaoId preenchido e não está Baixada
                        ocorrenciasQuery = ocorrenciasQuery.Where(x => 
                            x.ItemManutencaoId != null && 
                            x.StatusOcorrencia != false &&
                            x.Status != "Baixada" &&
                            x.Status != "Pendente");
                    }
                    else
                    {
                        ocorrenciasQuery = ocorrenciasQuery.Where(x => x.Status == statusId);
                    }
                }

                if (dataUnica.HasValue)
                {
                    // [FILTRO] Data única.
                    var dia = dataUnica.Value.Date;
                    ocorrenciasQuery = ocorrenciasQuery.Where(x => x.DataCriacao.Date == dia);
                }
                else if (dtIni.HasValue && dtFim.HasValue)
                {
                    // [FILTRO] Intervalo de datas.
                    var ini = dtIni.Value.Date;
                    var fim = dtFim.Value.Date;
                    ocorrenciasQuery = ocorrenciasQuery.Where(x => x.DataCriacao.Date >= ini && x.DataCriacao.Date <= fim);
                }

                // [ORDENACAO] Ordena por data de criação.
                ocorrenciasQuery = ocorrenciasQuery.OrderByDescending(x => x.DataCriacao);

                // [LIMITACAO] Limita volume para performance.
                var ocorrenciasFiltradas = ocorrenciasQuery.Take(500).ToList();

                if (!ocorrenciasFiltradas.Any())
                {
                    // [RETORNO] Lista vazia.
                    return new JsonResult(new { data = new List<object>() });
                }

                // [DADOS] Coleta IDs relacionados.
                var viagemIds = ocorrenciasFiltradas
                    .Where(o => o.ViagemId != Guid.Empty)
                    .Select(o => o.ViagemId)
                    .Distinct()
                    .ToList();

                var veiculoIds = ocorrenciasFiltradas
                    .Where(o => o.VeiculoId != Guid.Empty)
                    .Select(o => o.VeiculoId)
                    .Distinct()
                    .ToList();

                var motoristaIds = ocorrenciasFiltradas
                    .Where(o => o.MotoristaId.HasValue && o.MotoristaId != Guid.Empty)
                    .Select(o => o.MotoristaId.Value)
                    .Distinct()
                    .ToList();

                // [DADOS] Carrega dicionários de apoio.
                var viagens = viagemIds.Any()
                    ? _unitOfWork.Viagem.GetAll(v => viagemIds.Contains(v.ViagemId))
                        .ToDictionary(v => v.ViagemId)
                    : new Dictionary<Guid , Viagem>();

                var veiculos = veiculoIds.Any()
                    ? _unitOfWork.ViewVeiculos.GetAll(v => veiculoIds.Contains(v.VeiculoId))
                        .ToDictionary(v => v.VeiculoId)
                    : new Dictionary<Guid , ViewVeiculos>();

                var motoristas = motoristaIds.Any()
                    ? _unitOfWork.ViewMotoristas.GetAll(m => motoristaIds.Contains(m.MotoristaId))
                        .ToDictionary(m => m.MotoristaId)
                    : new Dictionary<Guid , ViewMotoristas>();

                // [MONTAGEM] Projeção final para UI.
                var result = ocorrenciasFiltradas.Select(oc =>
                {
                    viagens.TryGetValue(oc.ViagemId , out var viagem);
                    veiculos.TryGetValue(oc.VeiculoId , out var veiculo);
                    ViewMotoristas motorista = null;
                    if (oc.MotoristaId.HasValue)
                        motoristas.TryGetValue(oc.MotoristaId.Value , out motorista);

                    // CORREÇÃO: Determinar status corretamente
                    // Prioridade: campo Status se for "Pendente" ou "Manutenção"
                    // Senão: StatusOcorrencia (false = Baixada, NULL ou true = Aberta)
                    string statusFinal;
                    if (!string.IsNullOrEmpty(oc.Status) && (oc.Status == "Pendente" || oc.Status == "Manutenção"))
                    {
                        statusFinal = oc.Status;
                    }
                    else if (oc.StatusOcorrencia == false || oc.Status == "Baixada")
                    {
                        statusFinal = "Baixada";
                    }
                    else
                    {
                        statusFinal = "Aberta";
                    }

                    return new
                    {
                        ocorrenciaViagemId = oc.OcorrenciaViagemId ,
                        viagemId = oc.ViagemId ,
                        noFichaVistoria = viagem?.NoFichaVistoria ,
                        data = oc.DataCriacao.ToString("dd/MM/yyyy") ,
                        nomeMotorista = motorista?.Nome ?? "" ,
                        descricaoVeiculo = veiculo?.VeiculoCompleto ?? "" ,
                        resumoOcorrencia = oc.Resumo ?? "" ,
                        descricaoOcorrencia = oc.Descricao ?? "" ,
                        descricaoSolucaoOcorrencia = oc.Observacoes ?? "" ,
                        statusOcorrencia = statusFinal ,
                        imagemOcorrencia = oc.ImagemOcorrencia ?? "" ,
                        motoristaId = oc.MotoristaId ,
                        veiculoId = oc.VeiculoId ,
                        dataBaixa = oc.DataBaixa.HasValue ? oc.DataBaixa.Value.ToString("dd/MM/yyyy") : "" ,
                        usuarioCriacao = oc.UsuarioCriacao ?? "" ,
                        usuarioBaixa = oc.UsuarioBaixa ?? ""
                    };
                }).ToList();

                return new JsonResult(new { data = result });
            }
            catch (Exception ex)
            {
                _log.Error("OcorrenciaViagemController.ListarGestao", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Gestao.cs" , "ListarGestao" , ex);
                return new JsonResult(new { data = new List<object>() });
            }
        }

        #endregion LISTAR PARA GESTÃO

        #region EDITAR OCORRÊNCIA

        /// <summary>
        /// <para><b>FUNCIONALIDADE:</b> <c>EditarOcorrencia</c></para>
        /// <para><b>DESCRIÇÃO:</b> Edita uma ocorrência existente (chamado pelo modal de edição).</para>
        /// </summary>
        [HttpPost]
        [Route("EditarOcorrencia")]
        public async Task<IActionResult> EditarOcorrencia([FromBody] EditarOcorrenciaDTO dto)
        {
            try
            {
                if (dto == null || dto.OcorrenciaViagemId == Guid.Empty)
                {
                    return new JsonResult(new
                    {
                        success = false ,
                        message = "ID da ocorrência não informado"
                    });
                }

                var ocorrencia = _unitOfWork.OcorrenciaViagem
                    .GetFirstOrDefault(o => o.OcorrenciaViagemId == dto.OcorrenciaViagemId);

                if (ocorrencia == null)
                {
                    return new JsonResult(new
                    {
                        success = false ,
                        message = "Ocorrência não encontrada"
                    });
                }

                ocorrencia.Resumo = await TextNormalizationHelper.NormalizeAsync(dto.ResumoOcorrencia ?? ocorrencia.Resumo);
                ocorrencia.Descricao = await TextNormalizationHelper.NormalizeAsync(dto.DescricaoOcorrencia ?? ocorrencia.Descricao);
                ocorrencia.Observacoes = await TextNormalizationHelper.NormalizeAsync(dto.SolucaoOcorrencia ?? ocorrencia.Observacoes);

                if (dto.ImagemOcorrencia != null)
                {
                    ocorrencia.ImagemOcorrencia = dto.ImagemOcorrencia;
                }

                if (!string.IsNullOrWhiteSpace(dto.StatusOcorrencia))
                {
                    var novoStatus = dto.StatusOcorrencia.Trim();
                    // NULL ou true = Aberta, false = Baixada
                    var statusAtualAberta = ocorrencia.StatusOcorrencia != false;

                    if (novoStatus == "Baixada" && statusAtualAberta)
                    {
                        ocorrencia.Status = "Baixada";
                        ocorrencia.StatusOcorrencia = false;
                        ocorrencia.DataBaixa = DateTime.Now;
                        ocorrencia.UsuarioBaixa = HttpContext.User.Identity?.Name ?? "Sistema";
                    }
                    else if (novoStatus == "Aberta" && !statusAtualAberta)
                    {
                        ocorrencia.Status = "Aberta";
                        ocorrencia.StatusOcorrencia = true;
                        ocorrencia.DataBaixa = null;
                        ocorrencia.UsuarioBaixa = "";
                    }
                }

                _unitOfWork.OcorrenciaViagem.Update(ocorrencia);
                _unitOfWork.Save();

                _log.Info($"OcorrenciaViagemController.EditarOcorrencia: Ocorrência {dto.OcorrenciaViagemId} atualizada.");

                return new JsonResult(new
                {
                    success = true ,
                    message = "Ocorrência atualizada com sucesso"
                });
            }
            catch (Exception ex)
            {
                _log.Error("OcorrenciaViagemController.EditarOcorrencia", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Gestao.cs" , "EditarOcorrencia" , ex);
                return new JsonResult(new
                {
                    success = false ,
                    message = "Erro ao editar ocorrência: " + ex.Message
                });
            }
        }

        #endregion EDITAR OCORRÊNCIA

        #region BAIXAR OCORRÊNCIA

        /// <summary>
        /// <para><b>FUNCIONALIDADE:</b> <c>BaixarOcorrenciaGestao</c></para>
        /// <para><b>DESCRIÇÃO:</b> Dá baixa em uma ocorrência (botão finalizar na grid ou no modal).</para>
        /// </summary>
        [HttpPost]
        [Route("BaixarOcorrenciaGestao")]
        public IActionResult BaixarOcorrenciaGestao([FromBody] BaixarOcorrenciaDTO dto)
        {
            try
            {
                if (dto == null || dto.OcorrenciaViagemId == Guid.Empty)
                {
                    return new JsonResult(new
                    {
                        success = false ,
                        message = "ID da ocorrência não informado"
                    });
                }

                var ocorrencia = _unitOfWork.OcorrenciaViagem
                    .GetFirstOrDefault(o => o.OcorrenciaViagemId == dto.OcorrenciaViagemId);

                if (ocorrencia == null)
                {
                    return new JsonResult(new
                    {
                        success = false ,
                        message = "Ocorrência não encontrada"
                    });
                }

                // CORREÇÃO: Verificar status considerando NULL como Aberta
                // NULL ou true = Aberta, false = Baixada
                var jaEstaBaixada = ocorrencia.StatusOcorrencia == false;
                if (jaEstaBaixada)
                {
                    return new JsonResult(new
                    {
                        success = false ,
                        message = "Esta ocorrência já está baixada"
                    });
                }

                ocorrencia.Status = "Baixada";
                ocorrencia.StatusOcorrencia = false;
                ocorrencia.DataBaixa = DateTime.Now;
                ocorrencia.UsuarioBaixa = HttpContext.User.Identity?.Name ?? "Sistema";

                _unitOfWork.OcorrenciaViagem.Update(ocorrencia);
                _unitOfWork.Save();

                _log.Info($"OcorrenciaViagemController.BaixarOcorrenciaGestao: Ocorrência {dto.OcorrenciaViagemId} baixada via gestão.");

                return new JsonResult(new
                {
                    success = true ,
                    message = "Ocorrência baixada com sucesso"
                });
            }
            catch (Exception ex)
            {
                _log.Error("OcorrenciaViagemController.BaixarOcorrenciaGestao", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Gestao.cs" , "BaixarOcorrenciaGestao" , ex);
                return new JsonResult(new
                {
                    success = false ,
                    message = "Erro ao baixar ocorrência: " + ex.Message
                });
            }
        }

        #endregion BAIXAR OCORRÊNCIA

        #region BAIXAR COM SOLUÇÃO

        /// <summary>
        /// <para><b>FUNCIONALIDADE:</b> <c>BaixarOcorrenciaComSolucao</c></para>
        /// <para><b>DESCRIÇÃO:</b> Dá baixa em uma ocorrência com solução (modal de baixa rápida).</para>
        /// </summary>
        [HttpPost]
        [Route("BaixarOcorrenciaComSolucao")]
        public async Task<IActionResult> BaixarOcorrenciaComSolucao([FromBody] BaixarComSolucaoDTO dto)
        {
            try
            {
                if (dto == null || dto.OcorrenciaViagemId == Guid.Empty)
                {
                    return new JsonResult(new
                    {
                        success = false ,
                        message = "ID da ocorrência não informado"
                    });
                }

                var ocorrencia = _unitOfWork.OcorrenciaViagem
                    .GetFirstOrDefault(o => o.OcorrenciaViagemId == dto.OcorrenciaViagemId);

                if (ocorrencia == null)
                {
                    return new JsonResult(new
                    {
                        success = false ,
                        message = "Ocorrência não encontrada"
                    });
                }

                // Verificar se já está baixada
                var jaEstaBaixada = ocorrencia.StatusOcorrencia == false;
                if (jaEstaBaixada)
                {
                    return new JsonResult(new
                    {
                        success = false ,
                        message = "Esta ocorrência já está baixada"
                    });
                }

                // Atualiza status
                ocorrencia.Status = "Baixada";
                ocorrencia.StatusOcorrencia = false;
                ocorrencia.DataBaixa = DateTime.Now;
                ocorrencia.UsuarioBaixa = HttpContext.User.Identity?.Name ?? "Sistema";

                // Atualiza solução se informada
                if (!string.IsNullOrWhiteSpace(dto.SolucaoOcorrencia))
                {
                    ocorrencia.Observacoes = await TextNormalizationHelper.NormalizeAsync(dto.SolucaoOcorrencia);
                }

                _unitOfWork.OcorrenciaViagem.Update(ocorrencia);
                _unitOfWork.Save();

                _log.Info($"OcorrenciaViagemController.BaixarOcorrenciaComSolucao: Ocorrência {dto.OcorrenciaViagemId} baixada com solução rápida.");

                return new JsonResult(new
                {
                    success = true ,
                    message = "Ocorrência baixada com sucesso"
                });
            }
            catch (Exception ex)
            {
                _log.Error("OcorrenciaViagemController.BaixarOcorrenciaComSolucao", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Gestao.cs" , "BaixarOcorrenciaComSolucao" , ex);
                return new JsonResult(new
                {
                    success = false ,
                    message = "Erro ao baixar ocorrência: " + ex.Message
                });
            }
        }

        #endregion BAIXAR COM SOLUÇÃO

        /// <summary>
        /// <para><b>FUNCIONALIDADE:</b> <c>ContarOcorrencias</c></para>
        /// <para><b>DESCRIÇÃO:</b> Retorna contagem de ocorrências para debug.</para>
        /// </summary>
        [HttpGet]
        [Route("ContarOcorrencias")]
        public IActionResult ContarOcorrencias()
        {
            try
            {
                var total = _unitOfWork.OcorrenciaViagem.GetAll().Count();
                // NULL ou true = Aberta, false = Baixada
                var abertas = _unitOfWork.OcorrenciaViagem
                    .GetAll(x => x.StatusOcorrencia == null || x.StatusOcorrencia == true)
                    .Count();
                var baixadas = _unitOfWork.OcorrenciaViagem
                    .GetAll(x => x.StatusOcorrencia == false)
                    .Count();

                return new JsonResult(new
                {
                    success = true ,
                    total = total ,
                    abertas = abertas ,
                    baixadas = baixadas
                });
            }
            catch (Exception ex)
            {
                _log.Error("OcorrenciaViagemController.ContarOcorrencias", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Gestao.cs" , "ContarOcorrencias" , ex);
                return new JsonResult(new
                {
                    success = false ,
                    message = ex.Message
                });
            }
        }
    }

    #region DTOs

    /// <summary>
    /// <para>DTO para edição de ocorrência</para>
    /// </summary>
    public class EditarOcorrenciaDTO
    {
        public Guid OcorrenciaViagemId { get; set; }
        public string? ResumoOcorrencia { get; set; }
        public string? DescricaoOcorrencia { get; set; }
        public string? SolucaoOcorrencia { get; set; }
        public string? StatusOcorrencia { get; set; }
        public string? ImagemOcorrencia { get; set; }
    }

    /// <summary>
    /// <para>DTO para baixa de ocorrência</para>
    /// </summary>
    public class BaixarOcorrenciaDTO
    {
        public Guid OcorrenciaViagemId { get; set; }
    }

    /// <summary>
    /// <para>DTO para baixa de ocorrência com solução</para>
    /// </summary>
    public class BaixarComSolucaoDTO
    {
        public Guid OcorrenciaViagemId { get; set; }
        public string? SolucaoOcorrencia { get; set; }
    }

    #endregion DTOs
}
