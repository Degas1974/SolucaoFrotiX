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
*  #   PROJETO: FROTIX - GESTÃO DE FROTAS                                                          #
*  #   MODULO:  OPERAÇÕES (OCORRÊNCIAS DE VIAGEM)                                                  #
*  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
*  #                                                                                               #
*  #################################################################################################
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FrotiX.Repository.IRepository;
using FrotiX.Models;
using FrotiX.Helpers;
using FrotiX.Services;

namespace FrotiX.Controllers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: OcorrenciaViagemController                                         ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Gestão de ocorrências registradas durante viagens (manutenção, multas).    ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/OcorrenciaViagem                                       ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    public partial class OcorrenciaViagemController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OcorrenciaViagemController (Construtor)                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o controlador com UnitOfWork e Log centralizado.              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unitOfWork (IUnitOfWork): Acesso a dados.                               ║
        /// ║    • log (ILogService): Serviço de log centralizado.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public OcorrenciaViagemController(IUnitOfWork unitOfWork, ILogService log)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _log = log;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "Constructor", ex);
            }
        }

        #region LISTAR

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ListarPorViagem                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna o histórico de ocorrências de uma viagem específica.              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • viagemId: ID da viagem.                                                 ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista de ocorrências.                           ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("ListarPorViagem")]
        public IActionResult ListarPorViagem(Guid viagemId)
        {
            try
            {
                // [DADOS] Consulta histórico de ocorrências por viagem.
                var ocorrencias = _unitOfWork.ViewOcorrenciasViagem
                    .GetAll(o => o.ViagemId == viagemId)
                    .OrderByDescending(o => o.DataCriacao)
                    .Select(o => new
                    {
                        o.OcorrenciaViagemId ,
                        o.ViagemId ,
                        o.VeiculoId ,
                        o.MotoristaId ,
                        o.Resumo ,
                        o.Descricao ,
                        o.ImagemOcorrencia ,
                        o.Status ,
                        DataCriacao = o.DataCriacao.ToString("dd/MM/yyyy HH:mm") ,
                        DataBaixa = o.DataBaixa.HasValue ? o.DataBaixa.Value.ToString("dd/MM/yyyy HH:mm") : "" ,
                        o.UsuarioCriacao ,
                        o.UsuarioBaixa ,
                        o.Placa ,
                        o.VeiculoCompleto ,
                        o.NomeMotorista ,
                        o.DiasEmAberto ,
                        o.Urgencia ,
                        o.CorUrgencia
                    })
                    .ToList();

                // [RETORNO] Lista de ocorrências.
                return Ok(new { success = true , data = ocorrencias });
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "OcorrenciaViagemController.cs", "ListarPorViagem");
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "ListarPorViagem", error);
                return Ok(new { success = false , message = "Erro ao listar ocorrências: " + error.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ListarAbertasPorVeiculo                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista ocorrências ABERTAS de um veículo específico (para popup).          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • veiculoId: ID do veículo.                                               ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista de ocorrências em aberto.                 ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("ListarAbertasPorVeiculo")]
        public IActionResult ListarAbertasPorVeiculo(Guid veiculoId)
        {
            try
            {
                // [DADOS] Consulta ocorrências abertas por veículo.
                var ocorrencias = _unitOfWork.ViewOcorrenciasAbertasVeiculo
                    .GetAll(o => o.VeiculoId == veiculoId)
                    .OrderByDescending(o => o.DataCriacao)
                    .Select(o => new
                    {
                        o.OcorrenciaViagemId ,
                        o.ViagemId ,
                        o.VeiculoId ,
                        o.Resumo ,
                        o.Descricao ,
                        o.ImagemOcorrencia ,
                        DataCriacao = o.DataCriacao.ToString("dd/MM/yyyy HH:mm") ,
                        DataViagem = o.DataViagem.HasValue ? o.DataViagem.Value.ToString("dd/MM/yyyy") : "" ,
                        o.NoFichaVistoria ,
                        o.NomeMotorista ,
                        o.DiasEmAberto ,
                        o.Urgencia ,
                        o.CorUrgencia
                    })
                    .ToList();

                // [RETORNO] Lista de ocorrências abertas.
                return Ok(new { success = true , data = ocorrencias });
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "OcorrenciaViagemController.cs", "ListarAbertasPorVeiculo");
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "ListarAbertasPorVeiculo", error);
                return Ok(new { success = false , message = "Erro ao listar ocorrências abertas: " + error.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ContarAbertasPorVeiculo                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Conta ocorrências abertas de um veículo.                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • veiculoId: ID do veículo.                                               ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com contagem total.                                 ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("ContarAbertasPorVeiculo")]
        public IActionResult ContarAbertasPorVeiculo(Guid veiculoId)
        {
            try
            {
                // [DADOS] Contagem de ocorrências abertas.
                var count = _unitOfWork.ViewOcorrenciasAbertasVeiculo
                    .GetAll(o => o.VeiculoId == veiculoId)
                    .Count();

                // [RETORNO] Total de ocorrências.
                return Ok(new { success = true , count = count });
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "OcorrenciaViagemController.cs", "ContarAbertasPorVeiculo");
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "ContarAbertasPorVeiculo", error);
                return Ok(new { success = false , message = "Erro ao contar ocorrências: " + error.Message });
            }
        }

        #endregion LISTAR

        #region CRIAR

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Criar                                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Cria uma nova ocorrência vinculada a uma viagem.                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dto: Objeto OcorrenciaViagemDTO com os dados da ocorrência.             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost]
        [Route("Criar")]
        public IActionResult Criar([FromBody] OcorrenciaViagemDTO dto)
        {
            try
            {
                // [DADOS] Monta entidade de ocorrência.
                var ocorrencia = new OcorrenciaViagem
                {
                    OcorrenciaViagemId = Guid.NewGuid() ,
                    ViagemId = dto.ViagemId ,
                    VeiculoId = dto.VeiculoId ,
                    MotoristaId = dto.MotoristaId != Guid.Empty ? dto.MotoristaId : null ,
                    Resumo = dto.Resumo ?? "" ,
                    Descricao = dto.Descricao ?? "" ,
                    ImagemOcorrencia = dto.ImagemOcorrencia ?? "" ,
                    Status = "Aberta" ,
                    DataCriacao = DateTime.Now ,
                    UsuarioCriacao = User.Identity?.Name ?? "Sistema"
                };

                // [ACAO] Persiste ocorrência.
                _unitOfWork.OcorrenciaViagem.Add(ocorrencia);
                _unitOfWork.Save();

                // [LOG] Registro de criação.
                _log.Info($"OcorrenciaViagemController.Criar: Ocorrência {ocorrencia.OcorrenciaViagemId} criada para viagem {dto.ViagemId}.");

                // [RETORNO] Sucesso na criação.
                return Ok(new { success = true , message = "Ocorrência criada com sucesso!" , id = ocorrencia.OcorrenciaViagemId });
            }
            catch (Exception ex)
            {
                _log.Error("OcorrenciaViagemController.Criar", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "Criar", ex);
                return Ok(new { success = false , message = "Erro ao criar ocorrência: " + ex.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: CriarMultiplas                                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Cria múltiplas ocorrências de uma vez (ex: na finalização da viagem).     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dtos: Lista de DTOs de ocorrência.                                      ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost]
        [Route("CriarMultiplas")]
        public IActionResult CriarMultiplas([FromBody] List<OcorrenciaViagemDTO> dtos)
        {
            try
            {
                var criadas = 0;
                foreach (var dto in dtos)
                {
                    var ocorrencia = new OcorrenciaViagem
                    {
                        OcorrenciaViagemId = Guid.NewGuid() ,
                        ViagemId = dto.ViagemId ,
                        VeiculoId = dto.VeiculoId ,
                        MotoristaId = dto.MotoristaId != Guid.Empty ? dto.MotoristaId : null ,
                        Resumo = dto.Resumo ?? "" ,
                        Descricao = dto.Descricao ?? "" ,
                        ImagemOcorrencia = dto.ImagemOcorrencia ?? "" ,
                        Status = "Aberta" ,
                        DataCriacao = DateTime.Now ,
                        UsuarioCriacao = User.Identity?.Name ?? "Sistema"
                    };

                    _unitOfWork.OcorrenciaViagem.Add(ocorrencia);
                    criadas++;
                }

                _unitOfWork.Save();

                _log.Info($"OcorrenciaViagemController.CriarMultiplas: {criadas} ocorrências criadas.");

                return Ok(new { success = true , message = $"{criadas} ocorrência(s) criada(s) com sucesso!" });
            }
            catch (Exception ex)
            {
                _log.Error("OcorrenciaViagemController.CriarMultiplas", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "CriarMultiplas", ex);
                return Ok(new { success = false , message = "Erro ao criar ocorrências: " + ex.Message });
            }
        }

        #endregion CRIAR

        #region ATUALIZAR STATUS

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DarBaixa                                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Marca uma ocorrência como resolvida ('Baixada').                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • ocorrenciaId: ID da ocorrência a ser baixada.                           ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost]
        [Route("DarBaixa")]
        public IActionResult DarBaixa(Guid ocorrenciaId)
        {
            try
            {
                var ocorrencia = _unitOfWork.OcorrenciaViagem.GetFirstOrDefault(o => o.OcorrenciaViagemId == ocorrenciaId);
                if (ocorrencia == null)
                    return Ok(new { success = false , message = "Ocorrência não encontrada." });

                ocorrencia.Status = "Baixada";
                ocorrencia.DataBaixa = DateTime.Now;
                ocorrencia.UsuarioBaixa = User.Identity?.Name ?? "Sistema";

                _unitOfWork.OcorrenciaViagem.Update(ocorrencia);
                _unitOfWork.Save();

                _log.Info($"OcorrenciaViagemController.DarBaixa: Ocorrência {ocorrenciaId} baixada.");

                return Ok(new { success = true , message = "Ocorrência baixada com sucesso!" });
            }
            catch (Exception ex)
            {
                _log.Error("OcorrenciaViagemController.DarBaixa", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "DarBaixa", ex);
                return Ok(new { success = false , message = "Erro ao dar baixa: " + ex.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Reabrir                                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Reabre uma ocorrência previamente baixada para novas tratativas.          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • ocorrenciaId: ID da ocorrência a ser reaberta.                          ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost]
        [Route("Reabrir")]
        public IActionResult Reabrir(Guid ocorrenciaId)
        {
            try
            {
                var ocorrencia = _unitOfWork.OcorrenciaViagem.GetFirstOrDefault(o => o.OcorrenciaViagemId == ocorrenciaId);
                if (ocorrencia == null)
                    return Ok(new { success = false , message = "Ocorrência não encontrada." });

                ocorrencia.Status = "Aberta";
                ocorrencia.DataBaixa = null;
                ocorrencia.UsuarioBaixa = "";

                _unitOfWork.OcorrenciaViagem.Update(ocorrencia);
                _unitOfWork.Save();

                _log.Info($"OcorrenciaViagemController.Reabrir: Ocorrência {ocorrenciaId} reaberta.");

                return Ok(new { success = true , message = "Ocorrência reaberta com sucesso!" });
            }
            catch (Exception ex)
            {
                _log.Error("OcorrenciaViagemController.Reabrir", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "Reabrir", ex);
                return Ok(new { success = false , message = "Erro ao reabrir: " + ex.Message });
            }
        }

        #endregion ATUALIZAR STATUS

        #region EXCLUIR

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Excluir                                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Remove fisicamente uma ocorrência do banco de dados.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • ocorrenciaId: ID da ocorrência a ser excluída.                          ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpDelete]
        [Route("Excluir")]
        public IActionResult Excluir(Guid ocorrenciaId)
        {
            try
            {
                var ocorrencia = _unitOfWork.OcorrenciaViagem.GetFirstOrDefault(o => o.OcorrenciaViagemId == ocorrenciaId);
                if (ocorrencia == null)
                    return Ok(new { success = false , message = "Ocorrência não encontrada." });

                _unitOfWork.OcorrenciaViagem.Remove(ocorrencia);
                _unitOfWork.Save();

                _log.Info($"OcorrenciaViagemController.Excluir: Ocorrência {ocorrenciaId} excluída.");

                return Ok(new { success = true , message = "Ocorrência excluída com sucesso!" });
            }
            catch (Exception ex)
            {
                _log.Error("OcorrenciaViagemController.Excluir", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "Excluir", ex);
                return Ok(new { success = false , message = "Erro ao excluir: " + ex.Message });
            }
        }

        #endregion EXCLUIR

        #region ATUALIZAR

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Atualizar                                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Atualiza campos de texto e imagem de uma ocorrência existente.            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dto: DTO com os novos dados.                                            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPut]
        [Route("Atualizar")]
        public IActionResult Atualizar([FromBody] OcorrenciaViagemDTO dto)
        {
            try
            {
                var ocorrencia = _unitOfWork.OcorrenciaViagem.GetFirstOrDefault(o => o.OcorrenciaViagemId == dto.OcorrenciaViagemId);
                if (ocorrencia == null)
                    return Ok(new { success = false , message = "Ocorrência não encontrada." });

                ocorrencia.Resumo = dto.Resumo ?? ocorrencia.Resumo;
                ocorrencia.Descricao = dto.Descricao ?? ocorrencia.Descricao;
                ocorrencia.ImagemOcorrencia = !string.IsNullOrEmpty(dto.ImagemOcorrencia) ? dto.ImagemOcorrencia : ocorrencia.ImagemOcorrencia;
                ocorrencia.Observacoes = dto.Observacoes ?? ocorrencia.Observacoes;

                if (dto.ItemManutencaoId != Guid.Empty)
                    ocorrencia.ItemManutencaoId = dto.ItemManutencaoId;

                _unitOfWork.OcorrenciaViagem.Update(ocorrencia);
                _unitOfWork.Save();

                _log.Info($"OcorrenciaViagemController.Atualizar: Ocorrência {dto.OcorrenciaViagemId} atualizada.");

                return Ok(new { success = true , message = "Ocorrência atualizada com sucesso!" });
            }
            catch (Exception ex)
            {
                _log.Error("OcorrenciaViagemController.Atualizar", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "Atualizar", ex);
                return Ok(new { success = false , message = "Erro ao atualizar: " + ex.Message });
            }
        }

        #endregion ATUALIZAR

        #region UPLOAD IMAGEM

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: UploadImagem                                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Realiza o upload de arquivos de imagem/vídeo para ocorrências.            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • arquivo: IFormFile enviado via form-data.                               ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost]
        [Route("UploadImagem")]
        public async Task<IActionResult> UploadImagem(IFormFile arquivo)
        {
            try
            {
                if (arquivo == null || arquivo.Length == 0)
                    return Ok(new { success = false , message = "Nenhum arquivo enviado." });

                var extensao = Path.GetExtension(arquivo.FileName).ToLower();
                var extensoesPermitidas = new[] { ".jpg" , ".jpeg" , ".png" , ".gif" , ".webp" , ".mp4" , ".webm" };

                if (!extensoesPermitidas.Contains(extensao))
                    return Ok(new { success = false , message = "Tipo de arquivo não permitido." });

                var pastaUpload = Path.Combine(Directory.GetCurrentDirectory() , "wwwroot" , "uploads" , "ocorrencias");
                if (!Directory.Exists(pastaUpload))
                    Directory.CreateDirectory(pastaUpload);

                var nomeArquivo = Guid.NewGuid().ToString() + extensao;
                var caminhoCompleto = Path.Combine(pastaUpload , nomeArquivo);

                using (var stream = new FileStream(caminhoCompleto , FileMode.Create))
                {
                    await arquivo.CopyToAsync(stream);
                }

                var urlRelativa = "/uploads/ocorrencias/" + nomeArquivo;

                _log.Info($"OcorrenciaViagemController.UploadImagem: Upload realizado: {urlRelativa}");

                return Ok(new { success = true , url = urlRelativa });
            }
            catch (Exception ex)
            {
                _log.Error("OcorrenciaViagemController.UploadImagem", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "UploadImagem", ex);
                return Ok(new { success = false , message = "Erro no upload: " + ex.Message });
            }
        }

        #endregion UPLOAD IMAGEM
    }

    /// <summary>
    /// DTO para transferência de dados de ocorrência
    /// </summary>
    public class OcorrenciaViagemDTO
    {
        public Guid OcorrenciaViagemId { get; set; }
        public Guid ViagemId { get; set; }
        public Guid VeiculoId { get; set; }
        public Guid MotoristaId { get; set; }
        public string? Resumo { get; set; }
        public string? Descricao { get; set; }
        public string? ImagemOcorrencia { get; set; }
        public string? Observacoes { get; set; }
        public Guid ItemManutencaoId { get; set; }
    }
}
