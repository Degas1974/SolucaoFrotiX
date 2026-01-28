/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║  📄 DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md                  ║
 * ║  Seção: ViagemController.AtualizarDadosViagem.cs                         ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using FrotiX.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: Viagem API (Partial - AtualizarDadosViagemDashboard)
     * 🎯 OBJETIVO: Atualizar viagem via Dashboard com cálculo normalizado de minutos (jornada 8h)
     * 📋 ROTAS: /api/Viagem/AtualizarDadosViagemDashboard [POST]
     * 🔗 ENTIDADES: Viagem
     * 📦 DEPENDÊNCIAS: IUnitOfWork
     * ⚙️ REGRA DE NEGÓCIO: Jornada limitada a 8h/dia (480 minutos/dia)
     * 🗄️ TRIGGER: tr_Viagem_CalculaCustos recalcula custos automaticamente após Update
     * 📝 NOTA: Classe parcial - ver ViagemController.cs principal
     ****************************************************************************************/
    public partial class ViagemController
    {
        // [DOC] Constantes para cálculo de jornada normalizada (8h/dia, expediente 08h-18h)
        private const int MINUTOS_JORNADA_DIA = 480; // 8 horas
        private static readonly TimeSpan INICIO_EXPEDIENTE = new TimeSpan(8, 0, 0);  // 08:00
        private static readonly TimeSpan FIM_EXPEDIENTE = new TimeSpan(18, 0, 0);    // 18:00

        /****************************************************************************************
         * 📦 DTO: AtualizarViagemDashboardDTO
         * 🎯 OBJETIVO: Request para atualização de viagem via modal do Dashboard
         * 📋 PROPRIEDADES: Todos os campos de viagem (datas/horas como string para parsing)
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: CalcularMinutosNormalizadoComJornada
         * 🎯 OBJETIVO: Calcular minutos trabalhados considerando jornada de 8h/dia (480 min/dia)
         * 📥 ENTRADAS: dataInicial, dataFinal, horaInicio, horaFim
         * 📤 SAÍDAS: int (total de minutos normalizados)
         * 📊 ALGORITMO:
         *    - Mesmo dia: (horaFim - horaInicio), limitado a 480 min
         *    - Múltiplos dias:
         *      1. Primeiro dia: (18:00 - horaInicio), max 480
         *      2. Dias intermediários: 480 min cada
         *      3. Último dia: (horaFim - 08:00), max 480
         * 📝 EXEMPLO: 10/01 14:00 → 12/01 10:00
         *    - Dia 10: (18:00-14:00) = 240 min
         *    - Dia 11: 480 min (dia inteiro)
         *    - Dia 12: (10:00-08:00) = 120 min
         *    - Total: 840 min
         ****************************************************************************************/
        private int CalcularMinutosNormalizadoComJornada(DateTime dataInicial, DateTime dataFinal, TimeSpan horaInicio, TimeSpan horaFim)
        {
            try
            {
                // [DOC] Calcula total de dias (inclusive)
                int totalDias = (dataFinal.Date - dataInicial.Date).Days + 1;

                // [DOC] CASO 1: Mesmo dia - tempo real limitado a 480 min (jornada máxima)
                if (totalDias == 1)
                {
                    int minutosDia = (int)(horaFim - horaInicio).TotalMinutes;
                    return Math.Min(Math.Max(minutosDia, 0), MINUTOS_JORNADA_DIA);
                }

                // [DOC] CASO 2: Múltiplos dias - soma primeiro + intermediários + último
                int totalMinutos = 0;

                // [DOC] Primeiro dia: de HoraInicio até FIM_EXPEDIENTE (18:00), limitado a 480
                int minutosPrimeiroDia = (int)(FIM_EXPEDIENTE - horaInicio).TotalMinutes;
                minutosPrimeiroDia = Math.Min(Math.Max(minutosPrimeiroDia, 0), MINUTOS_JORNADA_DIA);
                totalMinutos += minutosPrimeiroDia;

                // [DOC] Dias intermediários: 480 minutos (jornada completa) para cada dia
                int diasIntermediarios = totalDias - 2;
                if (diasIntermediarios > 0)
                {
                    totalMinutos += diasIntermediarios * MINUTOS_JORNADA_DIA;
                }

                // [DOC] Último dia: de INICIO_EXPEDIENTE (08:00) até HoraFim, limitado a 480
                int minutosUltimoDia = (int)(horaFim - INICIO_EXPEDIENTE).TotalMinutes;
                minutosUltimoDia = Math.Min(Math.Max(minutosUltimoDia, 0), MINUTOS_JORNADA_DIA);
                totalMinutos += minutosUltimoDia;

                return totalMinutos;
            }
            catch
            {
                return 0;
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: AtualizarDadosViagemDashboard
         * 🎯 OBJETIVO: Atualizar viagem com cálculo normalizado (campos *Normalizado + MinutosNormalizado)
         * 📥 ENTRADAS: AtualizarViagemDashboardDTO (todos os campos da viagem)
         * 📤 SAÍDAS: JSON { success, message }
         * 🔗 CHAMADA POR: Modal de ajuste de viagem no Dashboard
         * 🔄 CHAMA: Viagem.Update(), CalcularMinutosNormalizadoComJornada()
         * 📊 CÁLCULOS:
         *    1. Normaliza datas/horas (DataInicialNormalizada, HoraInicioNormalizada, etc.)
         *    2. Calcula KmRodadoNormalizado (KmFinal - KmInicial)
         *    3. Calcula MinutosNormalizado com jornada 8h/dia
         * 🗄️ TRIGGER: tr_Viagem_CalculaCustos recalcula automaticamente após Update
         ****************************************************************************************/
        [Route("AtualizarDadosViagemDashboard")]
        [HttpPost]
        public IActionResult AtualizarDadosViagemDashboard([FromBody] AtualizarViagemDashboardDTO dados)
        {
            try
            {
                if (dados == null || dados.ViagemId == Guid.Empty)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Dados inválidos ou ID da viagem não informado"
                    });
                }

                // Busca a viagem
                var viagem = _unitOfWork.Viagem.GetFirstOrDefault(v => v.ViagemId == dados.ViagemId);
                if (viagem == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Viagem não encontrada"
                    });
                }

                // [DOC] Atualiza campos básicos
                viagem.NoFichaVistoria = dados.NoFichaVistoria;
                viagem.Finalidade = dados.Finalidade;
                viagem.EventoId = dados.EventoId;

                // [DOC] Variáveis temporárias para cálculo de MinutosNormalizado
                DateTime? dataInicialDt = null;
                DateTime? dataFinalDt = null;
                TimeSpan? horaInicioTs = null;
                TimeSpan? horaFimTs = null;

                // [DOC] Atualiza datas (campos normais + normalizados)
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

                // [DOC] Atualiza quilometragem (campos normais + normalizados)
                viagem.KmInicial = dados.KmInicial;
                viagem.KmFinal = dados.KmFinal;
                viagem.KmInicialNormalizado = dados.KmInicial;
                viagem.KmFinalNormalizado = dados.KmFinal;

                // [DOC] Calcula KmRodadoNormalizado (KmFinal - KmInicial, mínimo 0)
                if (dados.KmFinal.HasValue && dados.KmInicial.HasValue)
                {
                    int kmRodado = dados.KmFinal.Value - dados.KmInicial.Value;
                    viagem.KmRodadoNormalizado = kmRodado > 0 ? kmRodado : 0;
                }
                else
                {
                    viagem.KmRodadoNormalizado = 0;
                }

                // [DOC] ================================================================
                // [DOC] CALCULA MinutosNormalizado COM JORNADA DE 8H/DIA (Opção B)
                // [DOC] ================================================================
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
