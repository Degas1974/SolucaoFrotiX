/* ****************************************************************************************
 * ⚡ ARQUIVO: ViewLotacaoMotorista.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Vista SQL somente leitura para lotações de motoristas por unidade.
 *                   Consolida dados de atribuição de motorista a unidade/setor por
 *                   período (férias, licença, transferência). Utilizada em telas de
 *                   gestão de alocação, escalas e cobertura de postos.
 *
 * 📥 ENTRADAS     : Dados da view SQL vLotacaoMotorista:
 *                   - LotacaoMotoristaId, MotoristaId, UnidadeId
 *                   - Lotado (bool), Motivo, DataInicial, DataFim
 *                   - MotoristaCobertura
 *
 * 📤 SAÍDAS       : Registros de leitura (somente get; set) para grids de escala
 *
 * 🔗 CHAMADA POR  : EscalaController, GestaoPessoalController
 *                   Telas de alocação, cobertura de turnos
 *
 * 🔄 CHAMA        : Não se aplica (modelo puro)
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations
 *                   FrotiX.Validations (para validações customizadas)
 *
 * 📝 OBSERVAÇÕES  : View SQL mapeada via DbSet<ViewLotacaoMotorista>
 *                   Suporta filtros por período/unidade/motivo
 *                   Utilizada para planejamento de recurso humano (RH)
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
     * ⚡ MODEL: ViewLotacaoMotorista
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar view SQL de lotações de motoristas por unidade
     *
     * 📥 ENTRADAS     : Motorista, unidade, datas, motivo de lotação
     *
     * 📤 SAÍDAS       : Registro somente leitura para controle de lotação
     *
     * 🔗 CHAMADA POR  : Telas de escala e gestão de motoristas
     *
     * 🔄 CHAMA        : Não se aplica
     ****************************************************************************************/
    public class ViewLotacaoMotorista
    {
        // [DADOS] UnidadeId - GUID da unidade (FK para Unidade).
        // Qual unidade/setor o motorista está lotado.
        public Guid UnidadeId { get; set; }

        // [DADOS] LotacaoMotoristaId - GUID único da lotação.
        // Chave primária da view; referencia LotacaoMotorista.LotacaoMotoristaId
        public Guid LotacaoMotoristaId { get; set; }

        // [DADOS] MotoristaId - GUID do motorista (FK para Motorista).
        // Qual motorista está alocado.
        public Guid MotoristaId { get; set; }

        // [DADOS] Lotado - Flag indicando status de lotação (bool).
        // true = motorista está efetivamente lotado na unidade
        // false = lotação encerrada ou suspensa
        // [VALIDACAO] Obrigatório (nunca null).
        public bool Lotado { get; set; }

        // [DADOS] Motivo - Razão da lotação (string: "Férias", "Licença", "Transferência").
        // Classifica tipo de afastamento/movimentação.
        // [VALIDACAO] Valores esperados: "férias", "licença", "transferência", "afastamento"
        // Opcional; pode ser nulo para lotação padrão.
        public string? Motivo { get; set; }

        // [DADOS] Unidade - Nome da unidade (string 1..100).
        // Preenchido na view SQL (JOIN com Unidade.Nome).
        public string? Unidade { get; set; }

        // [DADOS] DataInicial - Data de início da lotação (string formatada).
        // Exemplo: "2026-02-01" ou "01/02/2026".
        // Opcional; pode ser nulo em históricos antigos.
        public string? DataInicial { get; set; }

        // [DADOS] DataFim - Data de término da lotação (string formatada).
        // Exemplo: "2026-02-28" - quando motorista deixa a unidade.
        // Opcional; pode ser nulo se lotação ativa (sem previsão de fim).
        public string? DataFim { get; set; }

        // [DADOS] MotoristaCobertura - Nome do motorista substituto (string).
        // Quem cobre a posição durante lotação do titular.
        // Exemplo: "Carlos Souza" - preenchido no SQL via JOIN com cobertura.
        // Opcional; pode ser nulo se sem substituição definida.
        public string? MotoristaCobertura { get; set; }
    }
}


