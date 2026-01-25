using Microsoft.AspNetCore.Mvc;
using System;

namespace FrotiX.Controllers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: ViagemController (Partial: DesassociarEvento)                       ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Desvinculação entre viagens e eventos.                                     ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rotas: /api/Viagem/*                                                   ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public partial class ViagemController
    {
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DesassociarViagemEvento (POST)                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Desassocia viagem de evento e altera finalidade.                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • request (DesassociarViagemRequest): Dados da desvinculação.            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da operação.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("DesassociarViagemEvento")]
        [HttpPost]
        public IActionResult DesassociarViagemEvento([FromBody] DesassociarViagemRequest request)
        {
            try
            {
                // [VALIDACAO] Requisição e ID.
                if (request == null || request.ViagemId == Guid.Empty)
                {
                    return Json(new
                    {
                        success = false,
                        message = "ID da viagem não informado"
                    });
                }

                // [VALIDACAO] Finalidade.
                if (string.IsNullOrWhiteSpace(request.NovaFinalidade))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Nova finalidade não informada"
                    });
                }

                // [DADOS] Busca viagem.
                var viagem = _unitOfWork.Viagem.GetFirstOrDefault(v => v.ViagemId == request.ViagemId);

                // [VALIDACAO] Viagem encontrada.
                if (viagem == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Viagem não encontrada"
                    });
                }

                // [DADOS] Evento anterior.
                var eventoAntigoId = viagem.EventoId;

                // [ACAO] Remove associação com evento.
                viagem.EventoId = null;
                viagem.NomeEvento = null;

                // [ACAO] Atualiza finalidade.
                viagem.Finalidade = request.NovaFinalidade;

                // [ACAO] Persiste alterações.
                _unitOfWork.Viagem.Update(viagem);
                _unitOfWork.Save();

                // [CACHE] Invalida cache do evento.
                if (eventoAntigoId.HasValue && _cache != null)
                {
                    _cache.Remove($"viagens_evento_{eventoAntigoId.Value}_1_50");
                    _cache.Remove($"viagens_evento_{eventoAntigoId.Value}_1_100");
                }

                // [RETORNO] Sucesso.
                return Json(new
                {
                    success = true,
                    message = "Viagem desassociada com sucesso!"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs", "DesassociarViagemEvento", error);
                return Json(new
                {
                    success = false,
                    message = "Erro ao desassociar viagem do evento"
                });
            }
        }
    }

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: DesassociarViagemRequest                                           ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Contrato de dados para desvincular evento da viagem.                       ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📦 PROPRIEDADES:                                                             ║
    /// ║    • ViagemId (Guid): ID da viagem.                                           ║
    /// ║    • NovaFinalidade (string): Finalidade após desvincular.                    ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public class DesassociarViagemRequest
    {
        public Guid ViagemId { get; set; }
        public string NovaFinalidade { get; set; }
    }
}
