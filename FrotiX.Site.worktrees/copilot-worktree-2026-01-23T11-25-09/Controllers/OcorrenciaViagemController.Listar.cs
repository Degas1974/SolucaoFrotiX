using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

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

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: OcorrenciaViagemController (Listar)                               ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Extensão parcial para rotas de listagem de ocorrências.                   ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public partial class OcorrenciaViagemController
    {
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ListarOcorrenciasModal (GET)                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista ocorrências de uma viagem específica para o modal.                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • viagemId (Guid): ID da viagem.                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com ocorrências.                                   ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("ListarOcorrenciasModal")]
        public IActionResult ListarOcorrenciasModal(Guid viagemId)
        {
            try
            {
                // [VALIDACAO] Verifica ID da viagem.
                if (viagemId == Guid.Empty)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "ID da viagem não informado"
                    });
                }

                // [DADOS] Consulta ocorrências da viagem.
                var ocorrencias = _unitOfWork.OcorrenciaViagem
                    .GetAll(o => o.ViagemId == viagemId)
                    .OrderBy(o => o.DataCriacao)
                    .Select(o => new
                    {
                        o.OcorrenciaViagemId,
                        o.ViagemId,
                        o.Resumo,
                        o.Descricao,
                        o.ImagemOcorrencia,
                        o.DataCriacao,
                        o.Status,
                        o.StatusOcorrencia
                    })
                    .ToList();

                // [RETORNO] Lista para o modal.
                return new JsonResult(new
                {
                    success = true,
                    data = ocorrencias,
                    total = ocorrencias.Count
                });
            }
            catch (Exception ex)
            {
                _log.Error("OcorrenciaViagemController.ListarOcorrenciasModal", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Listar.cs", "ListarOcorrenciasModal", ex);
                return new JsonResult(new
                {
                    success = false,
                    message = "Erro ao listar ocorrências: " + ex.Message
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ListarOcorrenciasVeiculo (GET)                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista ocorrências em aberto de um veículo específico.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • veiculoId (Guid): ID do veículo.                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com ocorrências abertas.                            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("ListarOcorrenciasVeiculo")]
        public IActionResult ListarOcorrenciasVeiculo(Guid veiculoId)
        {
            try
            {
                // [VALIDACAO] Verifica ID do veículo.
                if (veiculoId == Guid.Empty)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "ID do veículo não informado"
                    });
                }

                // [DADOS] Consulta ocorrências em aberto por veículo.
                var ocorrencias = _unitOfWork.OcorrenciaViagem
                    .GetAll(o => o.VeiculoId == veiculoId 
                              && o.StatusOcorrencia == true
                              && (o.Status == "Aberta" || o.Status == "Pendente"))
                    .OrderByDescending(o => o.DataCriacao)
                    .Select(o => new
                    {
                        o.OcorrenciaViagemId,
                        o.ViagemId,
                        o.VeiculoId,
                        o.Resumo,
                        o.Descricao,
                        o.ImagemOcorrencia,
                        o.DataCriacao,
                        o.Status,
                        o.StatusOcorrencia
                    })
                    .ToList();

                // [RETORNO] Lista de ocorrências do veículo.
                return new JsonResult(new
                {
                    success = true,
                    data = ocorrencias,
                    total = ocorrencias.Count,
                    temOcorrencias = ocorrencias.Count > 0
                });
            }
            catch (Exception ex)
            {
                _log.Error("OcorrenciaViagemController.ListarOcorrenciasVeiculo", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Listar.cs", "ListarOcorrenciasVeiculo", ex);
                return new JsonResult(new
                {
                    success = false,
                    message = "Erro ao listar ocorrências: " + ex.Message
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: VerificarOcorrenciasVeiculo (GET)                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Verifica se um veículo possui ocorrências em aberto (contagem).          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • veiculoId (Guid): ID do veículo.                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com contagem.                                      ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("VerificarOcorrenciasVeiculo")]
        public IActionResult VerificarOcorrenciasVeiculo(Guid veiculoId)
        {
            try
            {
                // [VALIDACAO] Verifica ID do veículo.
                if (veiculoId == Guid.Empty)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "ID do veículo não informado"
                    });
                }

                // [DADOS] Conta ocorrências em aberto.
                var quantidade = _unitOfWork.OcorrenciaViagem
                    .GetAll(o => o.VeiculoId == veiculoId 
                              && (o.Status == "Aberta" || o.Status == "Pendente"))
                    .Count();

                // [RETORNO] Contagem de ocorrências.
                return new JsonResult(new
                {
                    success = true,
                    quantidade = quantidade,
                    temOcorrencias = quantidade > 0
                });
            }
            catch (Exception ex)
            {
                _log.Error("OcorrenciaViagemController.VerificarOcorrenciasVeiculo", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Listar.cs", "VerificarOcorrenciasVeiculo", ex);
                return new JsonResult(new
                {
                    success = false,
                    message = "Erro ao verificar ocorrências: " + ex.Message
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ExcluirOcorrencia (POST)                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Exclui uma ocorrência específica do banco de dados.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dto (ExcluirOcorrenciaDTO): Dados com ID da ocorrência.                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da exclusão.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost]
        [Route("ExcluirOcorrencia")]
        public IActionResult ExcluirOcorrencia([FromBody] ExcluirOcorrenciaDTO dto)
        {
            try
            {
                // [VALIDACAO] Verifica payload e ID.
                if (dto == null || dto.OcorrenciaViagemId == Guid.Empty)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "ID da ocorrência não informado"
                    });
                }

                // [DADOS] Busca ocorrência.
                var ocorrencia = _unitOfWork.OcorrenciaViagem
                    .GetFirstOrDefault(o => o.OcorrenciaViagemId == dto.OcorrenciaViagemId);

                if (ocorrencia == null)
                {
                    // [RETORNO] Ocorrência não encontrada.
                    return new JsonResult(new
                    {
                        success = false,
                        message = "Ocorrência não encontrada"
                    });
                }

                // [ACAO] Remove e persiste.
                _unitOfWork.OcorrenciaViagem.Remove(ocorrencia);
                _unitOfWork.Save();

                // [LOG] Registro de exclusão.
                _log.Info($"OcorrenciaViagemController.ExcluirOcorrencia: Ocorrência {dto.OcorrenciaViagemId} removida.");

                // [RETORNO] Sucesso.
                return new JsonResult(new
                {
                    success = true,
                    message = "Ocorrência excluída com sucesso"
                });
            }
            catch (Exception ex)
            {
                _log.Error("OcorrenciaViagemController.ExcluirOcorrencia", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Listar.cs", "ExcluirOcorrencia", ex);
                return new JsonResult(new
                {
                    success = false,
                    message = "Erro ao excluir ocorrência: " + ex.Message
                });
            }
        }

        public class ExcluirOcorrenciaDTO
        {
            public Guid OcorrenciaViagemId { get; set; }
        }
    }
}

