/* ****************************************************************************************
 * ⚡ ARQUIVO: ViewLotacoes.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Vista SQL somente leitura para consolida de lotações de motoristas.
 *                   Agrupa dados de alocação por unidade/setor com status consolidado.
 *                   Utilizada em relatórios de distribuição de RH, grid de lotações,
 *                   análises de capacidade de unidades e planejamento de recursos.
 *
 * 📥 ENTRADAS     : Dados da view SQL vLotacoes:
 *                   - LotacaoMotoristaId, MotoristaId, UnidadeId
 *                   - NomeCategoria, Unidade, Motorista
 *                   - DataInicio, Lotado (flag)
 *
 * 📤 SAÍDAS       : Registros de leitura (somente get; set) para relatórios e analytics
 *
 * 🔗 CHAMADA POR  : RelatorioSetorSolicitanteController
 *                   Telas de análise de distribuição de pessoal
 *
 * 🔄 CHAMA        : Não se aplica (modelo puro)
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations
 *                   FrotiX.Validations (para validações customizadas)
 *
 * 📝 OBSERVAÇÕES  : View SQL mapeada via DbSet<ViewLotacoes>
 *                   Otimizada para relatórios gerenciais e BI
 *                   Suporta agregações e análises por unidade/categoria
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
     * ⚡ MODEL: ViewLotacoes
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar view SQL de lotações consolidadas por unidade/categoria
     *
     * 📥 ENTRADAS     : Motorista, unidade, categoria, data e status de lotação
     *
     * 📤 SAÍDAS       : Registros somente leitura para listagens e relatórios
     *
     * 🔗 CHAMADA POR  : Relatórios de distribuição, dashboards gerenciais
     *
     * 🔄 CHAMA        : Não se aplica (modelo puro para leitura)
     ****************************************************************************************/
    public class ViewLotacoes
    {
        // [DADOS] LotacaoMotoristaId - GUID único da lotação.
        // Chave primária do resultado da view; referencia LotacaoMotorista.LotacaoMotoristaId
        public Guid LotacaoMotoristaId { get; set; }

        // [DADOS] MotoristaId - GUID do motorista (FK para Motorista).
        // Identifica qual motorista está alocado nesta lotação.
        public Guid MotoristaId { get; set; }

        // [DADOS] UnidadeId - GUID da unidade (FK para Unidade).
        // Em qual unidade/setor o motorista está lotado.
        public Guid UnidadeId { get; set; }

        // [DADOS] NomeCategoria - Categoria funcional do motorista (string 1..100).
        // Exemplo: "Motorista Categoria D", "Motorista Categoria E"
        // Preenchido na view SQL (JOIN com Motorista.Categoria).
        // Opcional; pode ser nulo se categoria não definida.
        public string? NomeCategoria { get; set; }

        // [DADOS] Unidade - Nome da unidade (string 1..100).
        // Exemplo: "Unidade Central", "Unidade Norte"
        // Preenchido na view SQL (JOIN com Unidade.Nome).
        // Usado em relatórios e analytics por setor.
        public string? Unidade { get; set; }

        // [DADOS] Motorista - Nome completo do motorista (string 1..200).
        // Exemplo: "João da Silva Santos"
        // Preenchido na view SQL (JOIN com Motorista.Nome).
        // Opcional; pode ser nulo em históricos deletados.
        public string? Motorista { get; set; }

        // [DADOS] DataInicio - Data de início da lotação (string formatada).
        // Exemplo: "2026-01-15" ou "15/01/2026" (conforme localização)
        // Quando motorista foi alocado à unidade.
        // Opcional; pode ser nulo em registros antigos.
        public string? DataInicio { get; set; }

        // [DADOS] Lotado - Flag indicando status atual da lotação (bool).
        // true = motorista está efetivamente lotado (vigente)
        // false = lotação encerrada ou suspensa
        // [VALIDACAO] Obrigatório (nunca null); utilizado para filtros de ativos.
        public bool Lotado { get; set; }
    }
}

