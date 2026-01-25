using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FrotiX.Controllers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: ViagemController (Partial: AtualizarDados)                         ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Recuperação e atualização de dados de viagens.                            ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rotas: /api/Viagem/*                                                   ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public partial class ViagemController
    {
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetViagem (GET)                                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Obtém dados detalhados da viagem pelo ID.                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • id (Guid): ID da viagem.                                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com dados formatados.                              ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("GetViagem/{id}")]
        [HttpGet]
        public IActionResult GetViagem(Guid id)
        {
            try
            {
                // [DADOS] Busca a viagem pelo ID fornecido
                var viagem = _unitOfWork.Viagem.GetFirstOrDefault(v => v.ViagemId == id);

                // [VALIDACAO] Viagem encontrada.
                if (viagem == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Viagem não encontrada"
                    });
                }

                // [RETORNO] Dados formatados.
                return Json(new
                {
                    success = true,
                    data = new // [DADOS] Cria um objeto anônimo com os dados da viagem formatados
                    {
                        viagemId = viagem.ViagemId,
                        noFichaVistoria = viagem.NoFichaVistoria,
                        finalidade = viagem.Finalidade,
                        eventoId = viagem.EventoId,
                        dataInicial = viagem.DataInicial?.ToString("yyyy-MM-dd"), // [HELPER] Formata a data
                        horaInicio = viagem.HoraInicio?.ToString("HH:mm"),       // [HELPER] Formata a hora
                        dataFinal = viagem.DataFinal?.ToString("yyyy-MM-dd"),     // [HELPER] Formata a data
                        horaFim = viagem.HoraFim?.ToString("HH:mm"),             // [HELPER] Formata a hora
                        kmInicial = viagem.KmInicial,
                        kmFinal = viagem.KmFinal,
                        motoristaId = viagem.MotoristaId,
                        veiculoId = viagem.VeiculoId,
                        setorSolicitanteId = viagem.SetorSolicitanteId,
                        requisitanteId = viagem.RequisitanteId,
                        ramalRequisitante = viagem.RamalRequisitante
                    }
                });
            }
            catch (Exception error)
            {
                // 🛡️ BLOCO: Tratamento de Erro ao Buscar Viagem
                // Registra o erro no log e retorna uma resposta JSON de falha.
                Alerta.TratamentoErroComLinha("ViagemController.AtualizarDados.cs", "GetViagem", error); // Correção do nome do arquivo
                return Json(new
                {
                    success = false,
                    message = "Erro ao buscar viagem: " + error.Message
                });
            }
        }
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: AtualizarDadosViagem (POST)                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Atualiza dados de viagem de forma condicional.                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • request (AtualizarDadosViagemRequest): Dados de atualização.            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da atualização.                          ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("AtualizarDadosViagem")]
        [HttpPost]
        public IActionResult AtualizarDadosViagem([FromBody] AtualizarDadosViagemRequest request)
        {
            try
            {
                // [VALIDACAO] Requisição.
                if (request == null || request.ViagemId == Guid.Empty)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Dados inválidos"
                    });
                }

                // [DADOS] Busca a viagem no banco de dados
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

                // ═══════════════════════════════════════════════════════════════
                // 🔹 BLOCO: Atualização Condicional de Campos
                // Cada bloco verifica se um novo valor foi fornecido na requisição
                // antes de atualizar o campo correspondente na entidade 'viagem'.
                // Isso permite atualizações parciais dos dados.
                // ═══════════════════════════════════════════════════════════════

                // [DADOS] Atualizar campos gerais
                if (request.NoFichaVistoria.HasValue)
                {
                    viagem.NoFichaVistoria = request.NoFichaVistoria.Value;
                }

                if (!string.IsNullOrEmpty(request.Finalidade))
                {
                    viagem.Finalidade = request.Finalidade;
                }

                // [REGRA] EventoId condicionado à finalidade.
                if (request.EventoId.HasValue)
                {
                    viagem.EventoId = request.EventoId.Value;
                }
                else if (request.Finalidade != "Evento")
                {
                    viagem.EventoId = null;
                }

                // [DADOS] Datas e Horas
                if (request.DataInicial.HasValue)
                {
                    viagem.DataInicial = request.DataInicial.Value;
                }

                if (request.HoraInicio.HasValue)
                {
                    viagem.HoraInicio = request.HoraInicio.Value;
                }

                if (request.DataFinal.HasValue)
                {
                    viagem.DataFinal = request.DataFinal.Value;
                }

                if (request.HoraFim.HasValue)
                {
                    viagem.HoraFim = request.HoraFim.Value;
                }

                // [DADOS] Quilometragem
                if (request.KmInicial.HasValue)
                {
                    viagem.KmInicial = request.KmInicial.Value;
                }

                if (request.KmFinal.HasValue)
                {
                    viagem.KmFinal = request.KmFinal.Value;
                }

                // [DADOS] Motorista, Veículo e Setor
                if (request.MotoristaId.HasValue)
                {
                    viagem.MotoristaId = request.MotoristaId.Value;
                }

                if (request.VeiculoId.HasValue)
                {
                    viagem.VeiculoId = request.VeiculoId.Value;
                }

                if (request.SetorSolicitanteId.HasValue)
                {
                    viagem.SetorSolicitanteId = request.SetorSolicitanteId.Value;
                }

                // [DADOS] Requisitante e Ramal
                if (request.RequisitanteId.HasValue)
                {
                    viagem.RequisitanteId = request.RequisitanteId.Value;
                }

                if (!string.IsNullOrEmpty(request.RamalRequisitante))
                {
                    viagem.RamalRequisitante = request.RamalRequisitante;
                }

                // [DADOS] Atualiza o registro da viagem no banco de dados
                _unitOfWork.Viagem.Update(viagem);
                _unitOfWork.Save(); // Salva as mudanças

                // [RETORNO] Sucesso.
                return Json(new
                {
                    success = true,
                    message = "Viagem atualizada com sucesso"
                });
            }
            catch (Exception error)
            {
                // 🛡️ BLOCO: Tratamento de Erro na Atualização de Dados da Viagem
                // Registra o erro no log e retorna uma resposta JSON de falha.
                Alerta.TratamentoErroComLinha("ViagemController.AtualizarDados.cs", "AtualizarDadosViagem", error); // Correção do nome do arquivo
                return Json(new
                {
                    success = false,
                    message = "Erro ao atualizar viagem: " + error.Message
                });
            }
        }    }

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: AtualizarDadosViagemRequest                                       ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Contrato de dados para atualização de viagem.                             ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📦 PROPRIEDADES:                                                             ║
    /// ║    • ViagemId, NoFichaVistoria, Finalidade, EventoId                          ║
    /// ║    • DataInicial, HoraInicio, DataFinal, HoraFim                              ║
    /// ║    • KmInicial, KmFinal, MotoristaId, VeiculoId                               ║
    /// ║    • SetorSolicitanteId, RequisitanteId, RamalRequisitante                     ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public class AtualizarDadosViagemRequest
    {
        public Guid ViagemId { get; set; }
        public int? NoFichaVistoria { get; set; }
        public string Finalidade { get; set; }
        public Guid? EventoId { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? HoraInicio { get; set; }
        public DateTime? DataFinal { get; set; }
        public DateTime? HoraFim { get; set; }
        public int? KmInicial { get; set; }
        public int? KmFinal { get; set; }
        public Guid? MotoristaId { get; set; }
        public Guid? VeiculoId { get; set; }
        public Guid? SetorSolicitanteId { get; set; }
        public Guid? RequisitanteId { get; set; }
        public string RamalRequisitante { get; set; }
    }
}
