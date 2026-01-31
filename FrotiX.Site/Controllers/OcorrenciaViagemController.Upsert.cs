/* ****************************************************************************************
 * ⚡ ARQUIVO: OcorrenciaViagemController.Upsert.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Baixar ocorrência a partir da tela de Nova Viagem (Upsert).
 *
 * 📥 ENTRADAS     : BaixarOcorrenciaUpsertDTO.
 *
 * 📤 SAÍDAS       : JSON com sucesso/erro.
 *
 * 🔗 CHAMADA POR  : Modal de baixa na tela de Upsert.
 *
 * 🔄 CHAMA        : IUnitOfWork.OcorrenciaViagem, TextNormalizationHelper.
 **************************************************************************************** */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.TextNormalization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER PARTIAL: OcorrenciaViagemController.Upsert
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Métodos para baixa de ocorrência na tela de Upsert.
     *
     * 📥 ENTRADAS     : DTOs de baixa.
     *
     * 📤 SAÍDAS       : JSON com status da operação.
     ****************************************************************************************/
    public partial class OcorrenciaViagemController
    {
        #region Métodos para Tela Upsert (Nova Viagem)

        /****************************************************************************************
         * ⚡ FUNÇÃO: BaixarOcorrenciaUpsert
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Baixa ocorrência durante criação/edição de viagem (com/sem solução)
         * 📥 ENTRADAS     : BaixarOcorrenciaUpsertDTO (OcorrenciaViagemId, SolucaoOcorrencia opcional)
         * 📤 SAÍDAS       : JSON com success e message
         * 🔗 CHAMADA POR  : Modal de baixa em tela Upsert via POST /BaixarOcorrenciaUpsert
         * 🔄 CHAMA        : _unitOfWork.OcorrenciaViagem, TextNormalizationHelper.NormalizeAsync
         * 📦 DEPENDÊNCIAS : TextNormalizationHelper, Alerta.TratamentoErroComLinha
         * 📝 OBSERVAÇÃO   : [DOC] Impede baixar ocorrência já baixada (StatusOcorrencia=false)
         *                   [DOC] Atualiza Status(string) e StatusOcorrencia(bool) simultaneamente
         ****************************************************************************************/
        [Route("BaixarOcorrenciaUpsert")]
        [HttpPost]
        public async Task<IActionResult> BaixarOcorrenciaUpsert([FromBody] BaixarOcorrenciaUpsertDTO dto)
        {
            try
            {
                if (dto == null || dto.OcorrenciaViagemId == Guid.Empty)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "ID da ocorrência não informado"
                    });
                }

                var ocorrencia = _unitOfWork.OcorrenciaViagem.GetFirstOrDefault(
                    o => o.OcorrenciaViagemId == dto.OcorrenciaViagemId
                );

                if (ocorrencia == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "Ocorrência não encontrada"
                    });
                }

                // Se já está baixada, retorna erro
                if (ocorrencia.StatusOcorrencia == false)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "Esta ocorrência já foi baixada"
                    });
                }

                // Atualiza o status (string E bool)
                ocorrencia.Status = "Baixada";
                ocorrencia.StatusOcorrencia = false;  // false = Baixada
                ocorrencia.DataBaixa = DateTime.Now;
                ocorrencia.UsuarioBaixa = HttpContext.User?.Identity?.Name ?? "Sistema";

                // Se informou solução, normaliza e grava
                if (!string.IsNullOrWhiteSpace(dto.SolucaoOcorrencia))
                {
                    ocorrencia.Observacoes = await TextNormalizationHelper.NormalizeAsync(dto.SolucaoOcorrencia);
                }

                _unitOfWork.OcorrenciaViagem.Update(ocorrencia);
                _unitOfWork.Save();

                return new JsonResult(new
                {
                    success = true,
                    message = "Ocorrência baixada com sucesso"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "BaixarOcorrenciaUpsert", error);
                return new JsonResult(new
                {
                    success = false,
                    message = "Erro ao baixar ocorrência: " + error.Message
                });
            }
        }

        #endregion
    }

    #region DTOs para Tela Upsert

    /****************************************************************************************
     * ⚡ DTO: BaixarOcorrenciaUpsertDTO
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Transportar dados necessários para baixar ocorrência na tela Upsert.
     *
     * 📥 ENTRADAS     : OcorrenciaViagemId, SolucaoOcorrencia (opcional).
     *
     * 📤 SAÍDAS       : Nenhuma (apenas transporte de dados).
     *
     * 🔗 CHAMADA POR  : BaixarOcorrenciaUpsert (POST /BaixarOcorrenciaUpsert).
     *
     * 📝 OBSERVAÇÕES  : Solução é normalizada antes de persistir.
     ****************************************************************************************/
    public class BaixarOcorrenciaUpsertDTO
    {
        public Guid OcorrenciaViagemId { get; set; }
        public string? SolucaoOcorrencia { get; set; }
    }

    #endregion
}
