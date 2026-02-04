/* ****************************************************************************************
 * ⚡ ARQUIVO: ViewLavagem.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Vista SQL somente leitura para registros de lavagem de veículos.
 *                   Consolida dados de higienização: motorista responsável, lavadores,
 *                   veículo, datas/horários e status. Utilizada em telas de higiene
 *                   da frota, relatórios de limpeza e conformidade.
 *
 * 📥 ENTRADAS     : Dados da view SQL vLavagem:
 *                   - LavagemId, VeiculoId, MotoristaId
 *                   - Data, Horario, LavadoresId, Lavadores
 *                   - DescricaoVeiculo
 *
 * 📤 SAÍDAS       : Registros de leitura (somente get; set) para grids e relatórios
 *
 * 🔗 CHAMADA POR  : DashboardLavagemController
 *                   Telas de higiene, relatórios de conformidade
 *
 * 🔄 CHAMA        : Não se aplica (modelo puro)
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations
 *                   FrotiX.Services (para utilitários)
 *
 * 📝 OBSERVAÇÕES  : View SQL mapeada via DbSet<ViewLavagem>
 *                   Suporta filtros por data/motorista/veículo
 *                   Otimizada para compliance/auditoria de limpeza
 **************************************************************************************** */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using FrotiX.Services;
using FrotiX.Validations;
using Microsoft.AspNetCore.Http;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ MODEL: ViewLavagem
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar view SQL de lavagens de veículos
     *
     * 📥 ENTRADAS     : Veículo, motorista, lavadores, data e horário
     *
     * 📤 SAÍDAS       : Registro somente leitura para controle de lavagens
     *
     * 🔗 CHAMADA POR  : Telas de manutenção e limpeza de frota
     *
     * 🔄 CHAMA        : Não se aplica
     ****************************************************************************************/
    public class ViewLavagem
    {
        // [DADOS] LavagemId - GUID único do registro de lavagem.
        // Chave primária da view; referencia Lavagem.LavagemId
        public Guid LavagemId { get; set; }

        // [DADOS] MotoristaId - GUID do motorista responsável (FK).
        // Quem reivindicó/acompanhou a higienização.
        public Guid MotoristaId { get; set; }

        // [DADOS] VeiculoId - GUID do veículo lavado (FK).
        // Qual veículo recebeu manutenção de higiene.
        public Guid VeiculoId { get; set; }

        // [DADOS] LavadoresId - IDs dos lavadores (string concatenada).
        // Formato esperado: "ID1,ID2,ID3" ou lista separada por delimitador.
        // Opcional; pode ser nulo se lavagem terceirizada.
        public string? LavadoresId { get; set; }

        // [DADOS] Data - Data formatada da lavagem (string ISO 8601).
        // Exemplo: "2026-02-04" ou "04/02/2026" (conforme localização).
        // Opcional; pode vir nulo se não tiver data exata.
        public string? Data { get; set; }

        // [DADOS] Horario - Horário da lavagem (string HH:mm).
        // Exemplo: "14:30" - hora em que higienização foi realizada.
        // Opcional; pode ser nulo se data aproximada.
        public string? Horario { get; set; }

        // [DADOS] Lavadores - Nomes dos lavadores (string concatenada).
        // Exemplo: "João Silva, Maria Santos" (nomes separados por vírgula).
        // Opcional; pode ser nulo se equipe dinâmica não registrada.
        public string? Lavadores { get; set; }

        // [DADOS] DescricaoVeiculo - Descrição completa do veículo.
        // Exemplo: "Fiat Ducato Branco 2020 - Placa GHI-5678"
        // Concatena Marca + Modelo + Cor + Ano na view SQL.
        public string? DescricaoVeiculo { get; set; }

        // [DADOS] Nome - Nome do motorista responsável.
        // Preenchido na view SQL (JOIN com Motorista.Nome).
        // Opcional; pode ser nulo em registros históricos.
        public string? Nome { get; set; }
    }
}
