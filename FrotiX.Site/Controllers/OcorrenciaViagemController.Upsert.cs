/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: OcorrenciaViagemController.Upsert.cs                                                    ║
   ║ 📂 CAMINHO: /Controllers                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Partial para baixa de ocorrência específica da tela de Nova Viagem.                    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: BaixarUpsert() - baixa ocorrência durante criação/edição de viagem                       ║
   ║ 🔗 DEPS: IUnitOfWork, TextNormalizationHelper | 📅 28/01/2026 | 👤 Copilot | 📝 v2.0                ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

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
     * 🎯 OBJETIVO     : Método de baixa de ocorrência específico para tela de Nova Viagem
     * 📥 ENTRADAS     : BaixarOcorrenciaUpsertDTO (OcorrenciaViagemId, SolucaoOcorrencia)
     * 📤 SAÍDAS       : JSON com success e message
     * 🔗 CHAMADA POR  : Tela Upsert de Viagem (botão baixar ocorrência)
     * 🔄 CHAMA        : _unitOfWork.OcorrenciaViagem, TextNormalizationHelper
     * 📦 DEPENDÊNCIAS : TextNormalizationHelper.NormalizeAsync, Alerta.TratamentoErroComLinha
     ****************************************************************************************/

    /// <summary>
    /// Partial class para adicionar métodos de baixa na tela Upsert
    /// </summary>
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

    /// <summary>
    /// DTO para baixa de ocorrência na tela Upsert
    /// </summary>
    public class BaixarOcorrenciaUpsertDTO
    {
        public Guid OcorrenciaViagemId { get; set; }
        public string? SolucaoOcorrencia { get; set; }
    }

    #endregion
}
