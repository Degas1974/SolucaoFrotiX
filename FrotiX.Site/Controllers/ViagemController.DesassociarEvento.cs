/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║  📄 DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md                  ║
 * ║  Seção: ViagemController.DesassociarEvento.cs                            ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using Microsoft.AspNetCore.Mvc;
using System;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: Viagem API (Partial - DesassociarEvento)
     * 🎯 OBJETIVO: Desassociar viagem de evento, alterando finalidade e limpando cache
     * 📋 ROTAS: /api/viagem/DesassociarViagemEvento [POST]
     * 🔗 ENTIDADES: Viagem
     * 📦 DEPENDÊNCIAS: IUnitOfWork, IMemoryCache
     * 📝 NOTA: Classe parcial - ver ViagemController.cs principal
     ****************************************************************************************/
    public partial class ViagemController
    {
        /****************************************************************************************
         * ⚡ FUNÇÃO: DesassociarViagemEvento
         * 🎯 OBJETIVO: Desassociar viagem de evento, limpar EventoId/NomeEvento e alterar finalidade
         * 📥 ENTRADAS: DesassociarViagemRequest { ViagemId, NovaFinalidade }
         * 📤 SAÍDAS: JSON { success, message }
         * 🔗 CHAMADA POR: Modal de desassociação de viagem em evento
         * 🔄 CHAMA: Viagem.GetFirstOrDefault(), Viagem.Update(), IMemoryCache.Remove()
         * 🗑️ CACHE: Invalida cache do evento antigo (chaves: viagens_evento_{id}_1_50/100)
         ****************************************************************************************/
        [Route("DesassociarViagemEvento")]
        [HttpPost]
        public IActionResult DesassociarViagemEvento([FromBody] DesassociarViagemRequest request)
        {
            try
            {
                if (request == null || request.ViagemId == Guid.Empty)
                {
                    return Json(new
                    {
                        success = false,
                        message = "ID da viagem não informado"
                    });
                }

                if (string.IsNullOrWhiteSpace(request.NovaFinalidade))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Nova finalidade não informada"
                    });
                }

                var viagem = _unitOfWork.Viagem.GetFirstOrDefault(v => v.ViagemId == request.ViagemId);

                if (viagem == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Viagem não encontrada"
                    });
                }

                // [DOC] Guarda o evento antigo para invalidar cache depois da desassociação
                var eventoAntigoId = viagem.EventoId;

                // [DOC] Remove a associação com o evento
                viagem.EventoId = null;
                viagem.NomeEvento = null;

                // [DOC] Altera a finalidade da viagem para a nova informada
                viagem.Finalidade = request.NovaFinalidade;

                // [DOC] Atualiza a viagem no banco de dados
                _unitOfWork.Viagem.Update(viagem);
                _unitOfWork.Save();

                // [DOC] Invalida cache do evento antigo (lista de viagens com paginação 50 e 100)
                if (eventoAntigoId.HasValue && _cache != null)
                {
                    _cache.Remove($"viagens_evento_{eventoAntigoId.Value}_1_50");
                    _cache.Remove($"viagens_evento_{eventoAntigoId.Value}_1_100");
                }

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

    /****************************************************************************************
     * 📦 DTO: DesassociarViagemRequest
     * 🎯 OBJETIVO: Request para desassociar viagem de evento
     * 📋 PROPRIEDADES:
     *    - ViagemId: Identificador da viagem
     *    - NovaFinalidade: Nova finalidade após desassociação
     ****************************************************************************************/
    public class DesassociarViagemRequest
    {
        public Guid ViagemId { get; set; }
        public string NovaFinalidade { get; set; }
    }
}
