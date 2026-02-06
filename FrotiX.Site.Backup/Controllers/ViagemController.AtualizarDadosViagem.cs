using FrotiX.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace FrotiX.Controllers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: ViagemController (Partial: AtualizarDadosViagem)                   ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Atualizações de dados e cálculo de jornada.                               ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rotas: /api/Viagem/*                                                   ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public partial class ViagemController
    {
        // Constantes para cálculo de jornada
        private const int MINUTOS_JORNADA_DIA = 480; // 8 horas
        private static readonly TimeSpan INICIO_EXPEDIENTE = new TimeSpan(8, 0, 0);  // 08:00
        private static readonly TimeSpan FIM_EXPEDIENTE = new TimeSpan(18, 0, 0);    // 18:00

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: AtualizarViagemDashboardDTO                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    DTO do modal de ajuste no Dashboard de Viagens.                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📦 PROPRIEDADES:                                                             ║
        /// ║    • ViagemId, NoFichaVistoria, Finalidade, EventoId                          ║
        /// ║    • DataInicial, HoraInicio, DataFinal, HoraFim                              ║
        /// ║    • KmInicial, KmFinal, MotoristaId, VeiculoId                               ║
        /// ║    • SetorSolicitanteId, RequisitanteId, RamalRequisitante                     ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public class AtualizarViagemDashboardDTO
        {
            public Guid ViagemId { get; set; }
            public int? NoFichaVistoria { get; set; }
            public string Finalidade { get; set; }
            public Guid? EventoId { get; set; }
            public string DataInicial { get; set; }
            public string HoraInicio { get; set; }
            public string DataFinal { get; set; }
            public string HoraFim { get; set; }
            public int? KmInicial { get; set; }
            public int? KmFinal { get; set; }
            public Guid? MotoristaId { get; set; }
            public Guid? VeiculoId { get; set; }
            public Guid? SetorSolicitanteId { get; set; }
            public Guid? RequisitanteId { get; set; }
            public string RamalRequisitante { get; set; }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: CalcularMinutosNormalizadoComJornada (Privado)                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Normaliza minutos de viagem por jornada (08:00–18:00).                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dataInicial (DateTime), dataFinal (DateTime)                            ║
        /// ║    • horaInicio (TimeSpan), horaFim (TimeSpan)                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • int: minutos normalizados.                                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        private int CalcularMinutosNormalizadoComJornada(DateTime dataInicial, DateTime dataFinal, TimeSpan horaInicio, TimeSpan horaFim)
        {
            try
            {
                // [LOGICA] Calcula o número total de dias abrangidos pela viagem
                int totalDias = (dataFinal.Date - dataInicial.Date).Days + 1;

                // ═══════════════════════════════════════════════════════════════
                // 🔹 BLOCO: Cálculo para Viagens de Um Único Dia
                // Se a viagem ocorre no mesmo dia, calcula o tempo real e limita
                // à jornada máxima diária (MINUTOS_JORNADA_DIA).
                // ═══════════════════════════════════════════════════════════════
                if (totalDias == 1)
                {
                    int minutosDia = (int)(horaFim - horaInicio).TotalMinutes;
                    return Math.Min(Math.Max(minutosDia, 0), MINUTOS_JORNADA_DIA);
                }

                int totalMinutos = 0;

                // ═══════════════════════════════════════════════════════════════
                // 🔹 BLOCO: Cálculo para Viagens de Múltiplos Dias
                // Calcula os minutos do primeiro, dias intermediários e último dia
                // separadamente, aplicando as regras de jornada.
                // ═══════════════════════════════════════════════════════════════

                // [LOGICA] Primeiro dia: Calcula minutos desde a hora de início até o fim do expediente
                int minutosPrimeiroDia = (int)(FIM_EXPEDIENTE - horaInicio).TotalMinutes;
                minutosPrimeiroDia = Math.Min(Math.Max(minutosPrimeiroDia, 0), MINUTOS_JORNADA_DIA);
                totalMinutos += minutosPrimeiroDia;

                // [LOGICA] Dias intermediários: Cada dia intermediário conta como uma jornada completa
                int diasIntermediarios = totalDias - 2;
                if (diasIntermediarios > 0)
                {
                    totalMinutos += diasIntermediarios * MINUTOS_JORNADA_DIA;
                }

                // [LOGICA] Último dia: Calcula minutos desde o início do expediente até a hora final
                int minutosUltimoDia = (int)(horaFim - INICIO_EXPEDIENTE).TotalMinutes;
                minutosUltimoDia = Math.Min(Math.Max(minutosUltimoDia, 0), MINUTOS_JORNADA_DIA);
                totalMinutos += minutosUltimoDia;

                return totalMinutos;
            }
            catch
            {
                // 🛡️ BLOCO: Tratamento de Erro no Cálculo de Minutos
                // Em caso de erro no cálculo, retorna 0 para evitar quebras.
                return 0;
            }
        }
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: AtualizarDadosViagemDashboard (POST)                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Atualiza dados da viagem via modal do dashboard.                          ║
        /// ║    Trigger tr_Viagem_CalculaCustos recalcula custos.                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dados (AtualizarViagemDashboardDTO): Dados editados.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da atualização.                          ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("AtualizarDadosViagemDashboard")]
        [HttpPost]
        public IActionResult AtualizarDadosViagemDashboard([FromBody] AtualizarViagemDashboardDTO dados)
        {
            try
            {
                // [VALIDACAO] Dados obrigatórios.
                if (dados == null || dados.ViagemId == Guid.Empty)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Dados inválidos ou ID da viagem não informado"
                    });
                }

                // [DADOS] Busca viagem.
                var viagem = _unitOfWork.Viagem.GetFirstOrDefault(v => v.ViagemId == dados.ViagemId);
                // [VALIDACAO] Viagem encontrada.
                if (viagem == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Viagem não encontrada"
                    });
                }

                // [ACAO] Atualiza campos básicos.
                viagem.NoFichaVistoria = dados.NoFichaVistoria;
                viagem.Finalidade = dados.Finalidade;
                viagem.EventoId = dados.EventoId;

                // [CALCULO] Variáveis para normalização.
                DateTime? dataInicialDt = null;
                DateTime? dataFinalDt = null;
                TimeSpan? horaInicioTs = null;
                TimeSpan? horaFimTs = null;

                // [DADOS] Atualiza datas.
                if (!string.IsNullOrEmpty(dados.DataInicial))
                {
                    if (DateTime.TryParse(dados.DataInicial, out DateTime dtInicial))
                    {
                        viagem.DataInicial = dtInicial;
                        viagem.DataInicialNormalizada = dtInicial;
                        dataInicialDt = dtInicial;
                    }
                }

                if (!string.IsNullOrEmpty(dados.DataFinal))
                {
                    if (DateTime.TryParse(dados.DataFinal, out DateTime dtFinal))
                    {
                        viagem.DataFinal = dtFinal;
                        viagem.DataFinalNormalizada = dtFinal;
                        dataFinalDt = dtFinal;
                    }
                }

                // Atualiza horas
                if (!string.IsNullOrEmpty(dados.HoraInicio))
                {
                    if (TimeSpan.TryParse(dados.HoraInicio, out TimeSpan horaInicio))
                    {
                        viagem.HoraInicio = DateTime.Today.Add(horaInicio);
                        viagem.HoraInicioNormalizada = horaInicio;
                        horaInicioTs = horaInicio;
                    }
                }

                if (!string.IsNullOrEmpty(dados.HoraFim))
                {
                    if (TimeSpan.TryParse(dados.HoraFim, out TimeSpan horaFim))
                    {
                        viagem.HoraFim = DateTime.Today.Add(horaFim);
                        viagem.HoraFimNormalizada = horaFim;
                        horaFimTs = horaFim;
                    }
                }

                // Atualiza quilometragem
                viagem.KmInicial = dados.KmInicial;
                viagem.KmFinal = dados.KmFinal;

                // Atualiza campos normalizados de KM
                viagem.KmInicialNormalizado = dados.KmInicial;
                viagem.KmFinalNormalizado = dados.KmFinal;

                // Calcula KmRodadoNormalizado
                if (dados.KmFinal.HasValue && dados.KmInicial.HasValue)
                {
                    int kmRodado = dados.KmFinal.Value - dados.KmInicial.Value;
                    viagem.KmRodadoNormalizado = kmRodado > 0 ? kmRodado : 0;
                }
                else
                {
                    viagem.KmRodadoNormalizado = 0;
                }

                // ================================================================
                // CALCULA MinutosNormalizado COM JORNADA DE 8H/DIA (Opção B)
                // ================================================================
                if (dataInicialDt.HasValue && dataFinalDt.HasValue && 
                    horaInicioTs.HasValue && horaFimTs.HasValue)
                {
                    viagem.MinutosNormalizado = CalcularMinutosNormalizadoComJornada(
                        dataInicialDt.Value,
                        dataFinalDt.Value,
                        horaInicioTs.Value,
                        horaFimTs.Value
                    );
                }
                else if (viagem.DataInicial.HasValue && viagem.DataFinal.HasValue &&
                         viagem.HoraInicioNormalizada.HasValue && viagem.HoraFimNormalizada.HasValue)
                {
                    // Fallback usando campos já existentes
                    viagem.MinutosNormalizado = CalcularMinutosNormalizadoComJornada(
                        viagem.DataInicial.Value,
                        viagem.DataFinal.Value,
                        viagem.HoraInicioNormalizada.Value,
                        viagem.HoraFimNormalizada.Value
                    );
                }
                else
                {
                    viagem.MinutosNormalizado = 0;
                }

                // Atualiza relacionamentos
                viagem.MotoristaId = dados.MotoristaId;
                viagem.VeiculoId = dados.VeiculoId;
                viagem.SetorSolicitanteId = dados.SetorSolicitanteId;
                viagem.RequisitanteId = dados.RequisitanteId;
                viagem.RamalRequisitante = dados.RamalRequisitante;

                // Salva - O TRIGGER tr_Viagem_CalculaCustos vai recalcular automaticamente
                _unitOfWork.Viagem.Update(viagem);
                _unitOfWork.Save();

                return Json(new
                {
                    success = true,
                    message = "Viagem atualizada com sucesso"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs", "AtualizarDadosViagemDashboard", error);
                return Json(new
                {
                    success = false,
                    message = $"Erro ao atualizar viagem: {error.Message}"
                });
            }
        }
    }
}
