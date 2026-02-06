/* ****************************************************************************************
 * ⚡ ARQUIVO: ViewStatusMotoristas.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Vista SQL somente leitura para mostrar status em tempo real de todos
 *                   os motoristas ativos (escalados). Utilizada em dashboards operacionais,
 *                   telas de acompanhamento de frota e monitoramento de turnos.
 *
 * 📥 ENTRADAS     : Dados da view SQL vStatusMotoristas:
 *                   - MotoristaId, Nome, Ponto (matrícula)
 *                   - StatusAtual, DataEscala, NumeroSaidas
 *                   - Placa, Veiculo (descrição)
 *
 * 📤 SAÍDAS       : Registros de leitura (somente get; set) para grids e cards
 *
 * 🔗 CHAMADA POR  : DashboardMotoristasController.GetStatusMotoristasAtivos()
 *                   Views de monitoramento, telas de operação
 *
 * 🔄 CHAMA        : Não se aplica (modelo puro)
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations
 *                   FrotiX.Validations (validações customizadas)
 *
 * 📝 OBSERVAÇÕES  : View SQL mapeada via DbSet<ViewStatusMotoristas>
 *                   Otimizado para queries com múltiplos JOINs (Motorista+Escala+Veiculo)
 *                   Utiliza índices do banco para performance em grids grandes
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
     * ⚡ MODEL: ViewStatusMotoristas
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar status atual de motoristas ativos
     *
     * 📥 ENTRADAS     : Motorista, escala, veículo, status em tempo real
     *
     * 📤 SAÍDAS       : Registro somente leitura para dashboards de status
     *
     * 🔗 CHAMADA POR  : Tela de status de motoristas
     *
     * 🔄 CHAMA        : Não se aplica
     ****************************************************************************************/
    public class ViewStatusMotoristas
    {
        // [DADOS] MotoristaId - GUID único do motorista (FK para Motorista).
        // Chave primária do resultado da view SQL.
        public Guid MotoristaId { get; set; }

        // [DADOS] Nome - Nome completo do motorista (string 1..200).
        // Obrigatório, preenchido na view SQL.
        public string Nome { get; set; }

        // [DADOS] Ponto - Código de matrícula/ponto funcional (string 1..20).
        // Opcional; identificador de RH do motorista.
        public string? Ponto { get; set; }

        // [DADOS] StatusAtual - Status operacional (string: "Disponível", "Em Viagem", "Folga").
        // Renderizado em dashboards com cores (CSS) baseadas em GetStatusColor().
        // [VALIDACAO] Valores válidos: "disponível", "em viagem", "almoço", "folga", "pausa"
        public string StatusAtual { get; set; }

        // [DADOS] DataEscala - Data do turno (DateTime?).
        // Nullable; pode ser nulo se motorista sem escala ativa.
        // Formato esperado: YYYY-MM-DD
        public DateTime? DataEscala { get; set; }

        // [DADOS] NumeroSaidas - Quantidade de saídas/expedições hoje (int >= 0).
        // Quantas vezes o motorista saiu para atendimento neste turno.
        // [VALIDACAO] Nunca deve ser negativo (restritivo em INSERT/UPDATE).
        public int NumeroSaidas { get; set; }

        // [DADOS] Placa - Placa do veículo (string ABC-1234 ou format local).
        // Opcional; pode ser nulo se motorista sem veículo designado.
        // Utilizado em dashboards para rastreabilidade.
        public string? Placa { get; set; }

        // [DADOS] Veiculo - Descrição/modelo do veículo (string 1..150).
        // Ex: "Fiat Ducato Branco 2020" - opcional.
        // Concatena Marca + Modelo + Cor + Ano na view SQL.
        public string? Veiculo { get; set; }
    }
}
