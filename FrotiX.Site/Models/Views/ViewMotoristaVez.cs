/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: ViewMotoristaVez.cs                                                                    ║
    ║ 📂 CAMINHO: /Models/Views                                                                           ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: View SQL de motorista da vez (fila de atendimento).                                   ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 PROPS: MotoristaId, NomeMotorista, DataEscala, NumeroSaidas, StatusMotorista, VeiculoDescricao   ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🔗 DEPS: FrotiX.Validations                                                                         ║
    ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
    ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Validations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ MODEL: ViewMotoristasVez
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar motorista atual da vez (fila de atendimento)
     *
     * 📥 ENTRADAS     : Motorista, veículo, status, horários
     *
     * 📤 SAÍDAS       : Registro somente leitura para UI de fila
     *
     * 🔗 CHAMADA POR  : Tela de motorista da vez/fila
     *
     * 🔄 CHAMA        : GetStatusClass() [método helper]
     ****************************************************************************************/
    public class ViewMotoristasVez
    {
        // [DADOS] Identificador único do motorista
        public Guid MotoristaId { get; set; }
        // [DADOS] Nome completo do motorista
        public string NomeMotorista { get; set; }
        // [DADOS] Ponto/matrícula do motorista
        public string? Ponto { get; set; }
        // [DADOS] Foto do motorista (blob)
        public byte[]? Foto { get; set; }
        // [DADOS] Data da escala atual
        public DateTime DataEscala { get; set; }
        // [DADOS] Número de saídas/expedições
        public int NumeroSaidas { get; set; }
        // [DADOS] Status atual (disponível/em serviço/etc)
        public string StatusMotorista { get; set; }
        // [DADOS] Lotação/local de trabalho
        public string? Lotacao { get; set; }
        // [DADOS] Descrição do veículo designado
        public string? VeiculoDescricao { get; set; }
        // [DADOS] Placa do veículo
        public string? Placa { get; set; }
        // [DADOS] Horário de início do turno
        public string HoraInicio { get; set; }
        // [DADOS] Horário de fim do turno
        public string HoraFim { get; set; }

        /****************************************************************************************
         * ⚡ MÉTODO: GetStatusClass
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar classe CSS baseada no status do motorista
         *
         * 📥 ENTRADAS     : StatusMotorista (propriedade interna)
         *
         * 📤 SAÍDAS       : String com classe CSS (text-success, text-warning, text-secondary)
         *
         * ⬅️ CHAMADO POR  : Views de escala e fila
         *
         * 🔄 CHAMA        : Não se aplica
         *
         * 📝 OBSERVAÇÕES  : Usa switch expression para mapear status em cores
         ****************************************************************************************/
        public string GetStatusClass()
        {
            // [LOGICA] Map status para classe CSS com acento normalizado
            return StatusMotorista?.ToLower() switch
            {
                "disponível" or "disponivel" => "text-success",
                "em serviço" or "em servico" => "text-warning",
                _ => "text-secondary"
            };
        }
    }
}
