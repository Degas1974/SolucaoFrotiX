/* ****************************************************************************************
 * ⚡ ARQUIVO: ViewMotoristaVez.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Vista SQL somente leitura para representar motorista atual da fila
 *                   de atendimento (escala do dia). Utilizada em telas de despacho
 *                   e gestão de motoristas para identificar quem está de turno.
 *
 * 📥 ENTRADAS     : Dados da view SQL vMotoristaVez:
 *                   - MotoristaId, NomeMotorista, Ponto, Foto
 *                   - DataEscala, NumeroSaidas, StatusMotorista, Lotacao
 *                   - VeiculoDescricao, Placa, HoraInicio, HoraFim
 *
 * 📤 SAÍDAS       : Registro de leitura (somente get; set) para exibição em grid/modal
 *
 * 🔗 CHAMADA POR  : EscalaController.GetMotoristaDaVez()
 *                   Views de escala, telas de despacho, dashboards operacionais
 *
 * 🔄 CHAMA        : GetStatusClass() - método helper para mapear status em CSS
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations
 *                   FrotiX.Validations (validações customizadas)
 *
 * 📝 OBSERVAÇÕES  : View SQL mapeada via DbSet<ViewMotoristasVez>
 *                   Somente leitura (sem [Key], sem navigation properties)
 *                   Utilizada para otimizar queries de escala/turno
 **************************************************************************************** */

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
        // [DADOS] MotoristaId - GUID único do motorista (FK para Motorista).
        // Nunca é nulo em registros válidos da view.
        public Guid MotoristaId { get; set; }

        // [DADOS] NomeMotorista - Nome completo do motorista (string 1..200).
        // Obrigatório, vem preenchido da view SQL.
        public string NomeMotorista { get; set; }

        // [DADOS] Ponto/Matrícula - Código de matrícula funcional (string 1..20).
        // Opcional; pode ser null se não configurado no cadastro.
        public string? Ponto { get; set; }

        // [DADOS] Foto - Foto do motorista armazenada como blob (byte[]).
        // Opcional; pode ser null se matricula sem foto registrada.
        public byte[]? Foto { get; set; }

        // [DADOS] DataEscala - Ano/mês/dia da escala atual (DateTime).
        // Indica qual turno o motorista está escalado.
        public DateTime DataEscala { get; set; }

        // [DADOS] NumeroSaidas - Quantidade de expedições/saídas (int >= 0).
        // Quantas vezes motorista saiu para atendimento neste turno.
        // [VALIDACAO] Nunca deve ser negativo (restritivo).
        public int NumeroSaidas { get; set; }

        // [DADOS] StatusMotorista - Status operacional (string: "Disponível", "Em Serviço", "Off-duty").
        // Utilizado pela UI para renderizar CSS e ícones de status.
        // [VALIDACAO] Valores esperados: "disponável", "em serviço", "folga", "férias", "licença".
        public string StatusMotorista { get; set; }

        // [DADOS] Lotacao - Unidade/setor de trabalho (string 1..100).
        // Opcional; identifica origem ou destino de lotação.
        public string? Lotacao { get; set; }

        // [DADOS] VeiculoDescricao - Descrição/modelo do veículo (string 1..150).
        // Ex: "Fiat Ducato Branco 2020" - opcional se motorista sem veículo designado.
        public string? VeiculoDescricao { get; set; }

        // [DADOS] Placa - Placa do veículo (string ABC-1234).
        // Opcional; pode ser null se sem veículo designado.
        public string? Placa { get; set; }

        // [DADOS] HoraInicio - Hora de início do turno (string HH:mm).
        // Ex: "08:00" - Obrigatório em turnos definidos.
        public string HoraInicio { get; set; }

        // [DADOS] HoraFim - Hora de término do turno (string HH:mm).
        // Ex: "18:00" - Obrigatório em turnos definidos.
        public string HoraFim { get; set; }

        /****************************************************************************************
         * ⚡ MÉTODO: GetStatusClass
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Mapear status do motorista em classe CSS para renderização visual
         *                   (color, icon, background). Utilizado em grids e cards da UI.
         *
         * 📥 ENTRADAS     : Utiliza propriedade StatusMotorista (interna)
         *
         * 📤 SAÍDAS       : String com classe CSS Bootstrap/customizada
         *                   - "text-success" para Disponível
         *                   - "text-warning" para Em Serviço
         *                   - "text-secondary" para outros
         *
         * ⬅️ CHAMADO POR  : Views de escala, renders de grid, componentes de status
         *
         * 🔄 CHAMA        : string.ToLower() (normalizador de case)
         *
         * 📝 OBSERVAÇÕES  : Trata variações de acentuação (é/e)
         *                   Switch expression eficiente para 3+ possibilidades
         ****************************************************************************************/
        public string GetStatusClass()
        {
            // [LOGICA] Normalizar status removendo acentos e mapear para classe CSS
            // Considera "disponível" = "disponivel" via expressão switch
            return StatusMotorista?.ToLower() switch
            {
                "disponível" or "disponivel" => "text-success",
                "em serviço" or "em servico" => "text-warning",
                _ => "text-secondary"
            };
        }
    }
}
