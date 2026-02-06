/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: GlosaService.cs                                                                         ║
   ║ 📂 CAMINHO: /Services                                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Implementação IGlosaService. Cálculo de glosas de contratos (Qtd * VlrUnit - Glosa).   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: ListarResumo (GroupBy NumItem), ListarDetalhes (linhas individuais)                      ║
   ║ 🔗 DEPS: IUnitOfWork | 📅 29/01/2026 | 👤 Copilot | 📝 v2.0                                         ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.Collections.Generic;
using System.Linq;
using FrotiX.Repository.IRepository;

namespace FrotiX.Services
    {
    /// <summary>
    /// Serviço de Glosa (v2):
    /// - Resumo consolidado por NumItem/Descricao, COM cálculo correto do contrato (Qtd * VlrUnit) independente de O.S.
    /// - Glosa somada por item (somatório de todas as O.S.)
    /// - Ordenação por NumItem
    /// - Detalhes exibem DataDevolucao como "Retorno"
    /// </summary>
    public class GlosaService : IGlosaService
        {
        private readonly IUnitOfWork _uow;

        // [HELPER] Classe de trabalho para agregação intermediária
        // Usada apenas em ListarResumo para fazer GroupBy eficientemente
        private class ResumoWork
            {
            public int? NumItem { get; set; }
            public string Descricao { get; set; }
            public int Quantidade { get; set; }           // Qtd do item do contrato
            public decimal ValorUnitario { get; set; }    // Vlr unit do item
            public decimal ValorGlosa { get; set; }       // Vlr glosa por O.S.
            }

        /****************************
         * ⚡ CONSTRUTOR: GlosaService
         * ✅ Injeta IUnitOfWork
         ****************************/
        public GlosaService(IUnitOfWork uow)
            {
            _uow = uow;
            }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ListarResumo
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retorna glosas CONSOLIDADAS por Item do Contrato (não por O.S.)
         *                   Agrupa múltiplas O.S. do mesmo item, somando glosas
         *
         * 📥 ENTRADAS     : contratoId [Guid] - ID do contrato
         *                   mes [int] - Mês (1-12)
         *                   ano [int] - Ano (2024+)
         *
         * 📤 SAÍDAS       : IEnumerable<GlosaResumoItemDto> - Lista consolidada por item
         *
         * ⬅️ CHAMADO POR  : GlosaController.ObterResumo() [Dashboard]
         *                   ReportController.GerarRelatorioGlosa() [Relatório]
         *
         * ➡️ CHAMA        : _uow.ViewGlosa.GetAllReducedIQueryable() [Query otimizada]
         *
         * 📝 OBSERVAÇÕES  : [LOGICA] GroupBy(NumItem, Descricao) - 1 item = 1 linha
         *                   [REGRA] PrecoTotalMensal = Qtd × ValorUnitário (FIXO do contrato)
         *                   [REGRA] Glosa = SUM de TODAS as O.S. do item
         *                   [REGRA] ValorParaAteste = PrecoTotal - Glosa (valor cobrado)
         *                   [PERFORMANCE] AsNoTracking + GetAllReducedIQueryable (SQL puro)
         ****************************************************************************************/
        public IEnumerable<GlosaResumoItemDto> ListarResumo(Guid contratoId, int mes, int ano)
            {
            // [LOGICA] Etapa 1: Busca base = uma linha por O.S., com dados do contrato
            // GetAllReducedIQueryable = executa SELECT projetado no SQL (não traz tudo)
            var baseQuery = _uow.ViewGlosa.GetAllReducedIQueryable(
                selector: x => new ResumoWork
                    {
                    NumItem = x.NumItem,
                    Descricao = x.Descricao,
                    Quantidade = x.Quantidade ?? 0, // [DB] do item do contrato
                    ValorUnitario = (decimal)(x.ValorUnitario ?? 0d), // [DB] do item do contrato
                    ValorGlosa = x.ValorGlosa, // [DB] por O.S. (pode haver múltiplas)
                    },
                filter: x =>
                    x.ContratoId == contratoId
                    && x.DataSolicitacaoRaw.Month == mes
                    && x.DataSolicitacaoRaw.Year == ano,
                asNoTracking: true
            );

            // [LOGICA] Etapa 2: Consolidação = GroupBy NumItem, SUM glosas, MAX qtd/valor
            var query = baseQuery
                .GroupBy(g => new { g.NumItem, g.Descricao })
                .Select(s => new GlosaResumoItemDto
                    {
                    NumItem = s.Key.NumItem,
                    Descricao = s.Key.Descricao,
                    Quantidade = s.Max(i => (int?)i.Quantidade),
                    ValorUnitario = s.Max(i => i.ValorUnitario),
                    // [REGRA] PrecoTotalMensal = Qtd × VlrUnit (FIXO, não multiplica O.S.)
                    PrecoTotalMensal = (s.Max(i => i.Quantidade) * s.Max(i => i.ValorUnitario)),
                    // [REGRA] Preço diário = Valor mensal / 30 dias (para análise)
                    PrecoDiario = (s.Max(i => i.ValorUnitario) / 30m),
                    // [LOGICA] Glosa = SUM de TODAS as O.S. do item (pode ser > 0)
                    Glosa = s.Sum(i => i.ValorGlosa),
                    // [REGRA] ValorParaAteste = PreçoTotal - Glosa (o que será cobrado)
                    ValorParaAteste =
                        (s.Max(i => i.Quantidade) * s.Max(i => i.ValorUnitario))
                        - s.Sum(i => i.ValorGlosa),
                    })
                .OrderBy(x => x.NumItem);

            return query.ToList();
            }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ListarDetalhes
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retorna glosas DETALHADAS por Ordem de Serviço (O.S.)
         *                   Uma linha por O.S., mostrando datas, placa e dias de glosa
         *
         * 📥 ENTRADAS     : contratoId [Guid] - ID do contrato
         *                   mes [int] - Mês (1-12)
         *                   ano [int] - Ano (2024+)
         *
         * 📤 SAÍDAS       : IEnumerable<GlosaDetalheItemDto> - Lista detalha por O.S.
         *
         * ⬅️ CHAMADO POR  : GlosaController.ObterDetalhes() [Drill-down de resumo]
         *                   ReportController.GerarDetalhamentoGlosa() [Relatório detalh.]
         *
         * ➡️ CHAMA        : _uow.ViewGlosa.GetAllReducedIQueryable() [Query otimizada]
         *
         * 📝 OBSERVAÇÕES  : [LOGICA] Sem GroupBy - cada linha é 1 O.S.
         *                   [LOGICA] Mostra timeline completo: Solicitação → Disponib → Devol
         *                   [REGRA] DiasGlosa = dias entre DataDisponibilidade e DataDevolução
         *                   [PERFORMANCE] AsNoTracking + GetAllReducedIQueryable
         ****************************************************************************************/
        public IEnumerable<GlosaDetalheItemDto> ListarDetalhes(Guid contratoId, int mes, int ano)
            {
            // [LOGICA] Query SEM GroupBy = detalhe por O.S. (linhas individuais)
            var query = _uow.ViewGlosa.GetAllReducedIQueryable(
                selector: x => new GlosaDetalheItemDto
                    {
                    NumItem = x.NumItem,
                    Descricao = x.Descricao,
                    Placa = x.Placa,
                    // [DB] Timeline da O.S.
                    DataSolicitacao = x.DataSolicitacao,       // Solicitado em
                    DataDisponibilidade = x.DataDisponibilidade, // Veículo disponível em
                    DataRecolhimento = x.DataRecolhimento,     // Recolhido em
                    DataDevolucao = x.DataDevolucao,           // Devolvido em ("Retorno")
                    // [LOGICA] Dias de glosa = período sem uso/cobrado indevidamente
                    DiasGlosa = x.DiasGlosa,
                    },
                filter: x =>
                    x.ContratoId == contratoId
                    && x.DataSolicitacaoRaw.Month == mes
                    && x.DataSolicitacaoRaw.Year == ano,
                asNoTracking: true
            );

            return query.ToList();
            }

        // Implementação explícita (protege contra ambiguidades de namespace)
        IEnumerable<GlosaDetalheItemDto> IGlosaService.ListarDetalhes(
            Guid contratoId,
            int mes,
            int ano
        ) => ListarDetalhes(contratoId, mes, ano);
        }
    }


