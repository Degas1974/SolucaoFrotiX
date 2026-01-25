using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using FrotiX.TextNormalization;
using Microsoft.AspNetCore.Mvc;
using System;
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

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: OcorrenciaViagemController (Upsert)                               ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Extensão parcial para fluxos de baixa na tela de Upsert de Viagem.         ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public partial class OcorrenciaViagemController
    {
        #region Métodos para Tela Upsert (Nova Viagem)

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: BaixarOcorrenciaUpsert (POST)                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Baixa ocorrência a partir da tela de Nova Viagem (Upsert).               ║
        /// ║    Permite baixar com ou sem solução normalizada pela IA.                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dto (BaixarOcorrenciaUpsertDTO): Dados da baixa.                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da baixa.                                ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("BaixarOcorrenciaUpsert")]
        [HttpPost]
        public async Task<IActionResult> BaixarOcorrenciaUpsert([FromBody] BaixarOcorrenciaUpsertDTO dto)
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
                var ocorrencia = _unitOfWork.OcorrenciaViagem.GetFirstOrDefault(
                    o => o.OcorrenciaViagemId == dto.OcorrenciaViagemId
                );

                if (ocorrencia == null)
                {
                    // [RETORNO] Ocorrência não encontrada.
                    return new JsonResult(new
                    {
                        success = false,
                        message = "Ocorrência não encontrada"
                    });
                }

                // [REGRA] Se já está baixada, retorna erro.
                if (ocorrencia.StatusOcorrencia == false)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "Esta ocorrência já foi baixada"
                    });
                }

                // [ATUALIZACAO] Atualiza status e dados de baixa.
                ocorrencia.Status = "Baixada";
                ocorrencia.StatusOcorrencia = false;  // false = Baixada
                ocorrencia.DataBaixa = DateTime.Now;
                ocorrencia.UsuarioBaixa = HttpContext.User?.Identity?.Name ?? "Sistema";

                // [IA] Se informou solução, normaliza e grava.
                if (!string.IsNullOrWhiteSpace(dto.SolucaoOcorrencia))
                {
                    ocorrencia.Observacoes = await TextNormalizationHelper.NormalizeAsync(dto.SolucaoOcorrencia);
                }

                // [ACAO] Persiste alteração.
                _unitOfWork.OcorrenciaViagem.Update(ocorrencia);
                _unitOfWork.Save();

                // [LOG] Registro de baixa.
                _log.Info($"OcorrenciaViagemController.BaixarOcorrenciaUpsert: Ocorrência {dto.OcorrenciaViagemId} baixada na tela de Upsert.");

                // [RETORNO] Sucesso.
                return new JsonResult(new
                {
                    success = true,
                    message = "Ocorrência baixada com sucesso"
                });
            }
            catch (Exception ex)
            {
                _log.Error("OcorrenciaViagemController.BaixarOcorrenciaUpsert", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Upsert.cs", "BaixarOcorrenciaUpsert", ex);
                return new JsonResult(new
                {
                    success = false,
                    message = "Erro ao baixar ocorrência: " + ex.Message
                });
            }
        }

        #endregion

        #region DTOs para Tela Upsert

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: BaixarOcorrenciaUpsertDTO (DTO)                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    DTO para baixa de ocorrência na tela Upsert.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public class BaixarOcorrenciaUpsertDTO
        {
            public Guid OcorrenciaViagemId { get; set; }
            public string? SolucaoOcorrencia { get; set; }
        }

        #endregion
    }
}
