using System;
using System.Collections.Generic;
using System.Linq;
using FrotiX.Repository.IRepository;

namespace FrotiX.Services
{
    /// <summary>
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║                                                                              ║
    /// ║  📋 CLASSE: GlosaService (v2)                                                ║
    /// ║                                                                              ║
    /// ║  DESCRIÇÃO:                                                                  ║
    /// ║  Serviço especializado em cálculos de Glosa contratual.                     ║
    /// ║                                                                              ║
    /// ║  CONCEITO DE GLOSA:                                                          ║
    /// ║  - Glosa = Desconto/Penalidade aplicada por descumprimento contratual.      ║
    /// ║  - Exemplos: Atraso na entrega, serviço não realizado, recurso indisponível.║
    /// ║                                                                              ║
    /// ║  FUNCIONALIDADES PRINCIPAIS:                                                 ║
    /// ║  1. ListarResumo(): Consolida múltiplas O.S. em uma linha por item.         ║
    /// ║  2. ListarDetalhes(): Lista individual de cada O.S. glosada.                ║
    /// ║                                                                              ║
    /// ║  REGRAS DE NEGÓCIO:                                                          ║
    /// ║  - Quantidade e ValorUnitario vêm do CONTRATO (fixos, não da O.S.).         ║
    /// ║  - ValorGlosa é SOMADO de todas as O.S. de um mesmo item.                   ║
    /// ║  - ValorParaAteste = PrecoTotalMensal - GlosaTotal.                          ║
    /// ║                                                                              ║
    /// ║  FONTE DE DADOS:                                                             ║
    /// ║  - ViewGlosa (banco de dados): View agregada que retorna uma linha por O.S. ║
    /// ║                                                                              ║
    /// ║  ÚLTIMA ATUALIZAÇÃO: 14/01/2026                                              ║
    /// ║                                                                              ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    /// </summary>
    public class GlosaService : IGlosaService
    {
        private readonly IUnitOfWork _uow;

        // classe de trabalho para agregação
        private class ResumoWork
        {
            public int? NumItem { get; set; }
            public string Descricao { get; set; }
            public int Quantidade { get; set; }
            public decimal ValorUnitario { get; set; }
            public decimal ValorGlosa { get; set; }
        }

        public GlosaService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Lista resumo de glosas por item de contrato.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Consolida todas as Ordens de Serviço (O.S.) do período por NumItem,
        /// │    mantendo Qtd/Valor do contrato e SOMANDO a glosa total.
        /// │
        /// │ LÓGICA DE NEGÓCIO:
        /// │    1. Busca dados da ViewGlosa filtrados por Contrato/Mês/Ano.
        /// │    2. Cada linha da view representa uma O.S. individual.
        /// │    3. Agrupa por NumItem (identificador do item no contrato).
        /// │    4. Quantidade e ValorUnitario vêm do CONTRATO (não da O.S.).
        /// │    5. ValorGlosa é SOMADO de todas as O.S. do item.
        /// │    6. ValorParaAteste = PrecoTotalMensal - GlosaTotal.
        /// │
        /// │ PARÂMETROS:
        /// │    -> contratoId: GUID do contrato pai.
        /// │    -> mes: Mês de referência (1-12).
        /// │    -> ano: Ano de referência (ex: 2026).
        /// │
        /// │ RETORNO:
        /// │    -> IEnumerable<GlosaResumoItemDto>: Lista consolidada por item.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
        public IEnumerable<GlosaResumoItemDto> ListarResumo(Guid contratoId, int mes, int ano)
        {
            // ─────────────────────────────────────────────────────────────────────────────
            // [ETAPA 1] BUSCA BASE: Recupera dados da View filtrados por período/contrato
            // ─────────────────────────────────────────────────────────────────────────────
            // IMPORTANTE: ViewGlosa retorna UMA LINHA POR O.S. (Ordem de Serviço).
            // Precisamos consolidar múltiplas O.S. do mesmo item em uma única linha.
            var baseQuery = _uow.ViewGlosa.GetAllReducedIQueryable(
                selector: x => new ResumoWork
                {
                    NumItem = x.NumItem,                                  // Identificador do item no contrato
                    Descricao = x.Descricao,                             // Descrição do serviço/item
                    Quantidade = x.Quantidade ?? 0,                      // ⚠️ Vem do CONTRATO, não da O.S.
                    ValorUnitario = (decimal)(x.ValorUnitario ?? 0d),   // ⚠️ Vem do CONTRATO, não da O.S.
                    ValorGlosa = x.ValorGlosa,                           // ✅ Este vem da O.S. específica
                },
                filter: x =>
                    x.ContratoId == contratoId
                    && x.DataSolicitacaoRaw.Month == mes
                    && x.DataSolicitacaoRaw.Year == ano,
                asNoTracking: true                                        // Performance: não rastrear mudanças
            );

            // ─────────────────────────────────────────────────────────────────────────────
            // [ETAPA 2] CONSOLIDAÇÃO: Agrupa múltiplas O.S. do mesmo item em uma linha
            // ─────────────────────────────────────────────────────────────────────────────
            // REGRA DE NEGÓCIO:
            // - NumItem + Descricao identificam UNICAMENTE um item do contrato.
            // - Quantidade e ValorUnitario são FIXOS (vêm do contrato, não variam).
            // - ValorGlosa é ACUMULATIVO (soma de todas as O.S. do item).
            // EXEMPLO: Se há 3 O.S. para "Item 5 - Motorista", todas terão Qtd=10, VlrUnit=100,
            //          mas cada O.S. pode ter glosa diferente (R$50, R$30, R$20). Total: R$100.
            var query = baseQuery
                .GroupBy(g => new { g.NumItem, g.Descricao })            // Agrupa por identificador único do item
                .Select(s => new GlosaResumoItemDto
                {
                    NumItem = s.Key.NumItem,
                    Descricao = s.Key.Descricao,
                    // QUANTIDADE: Como é do contrato (não varia entre O.S.), pegamos o Max() como proxy
                    Quantidade = s.Max(i => (int?)i.Quantidade),
                    // VALOR UNITÁRIO: Também é do contrato, não varia
                    ValorUnitario = s.Max(i => i.ValorUnitario),
                    // ─────────────────────────────────────────────────────────────────────
                    // [CÁLCULO 1] Preço Total Mensal do Contrato (SEM glosa)
                    // ─────────────────────────────────────────────────────────────────────
                    // FÓRMULA: Quantidade × ValorUnitario
                    // IMPORTANTE: Independente do número de O.S., é sempre Qtd * VlrUnit.
                    PrecoTotalMensal = (s.Max(i => i.Quantidade) * s.Max(i => i.ValorUnitario)),
                    // ─────────────────────────────────────────────────────────────────────
                    // [CÁLCULO 2] Preço Diário (para cálculos de glosa proporcional)
                    // ─────────────────────────────────────────────────────────────────────
                    // FÓRMULA: ValorUnitario ÷ 30 dias
                    // USO: Glosa por dia de atraso/falha no serviço.
                    PrecoDiario = (s.Max(i => i.ValorUnitario) / 30m),
                    // ─────────────────────────────────────────────────────────────────────
                    // [CÁLCULO 3] Glosa Total do Item (SOMA de todas as O.S.)
                    // ─────────────────────────────────────────────────────────────────────
                    // FÓRMULA: Σ(ValorGlosa de cada O.S.)
                    // CONCEITO: Glosa = Desconto/Penalidade por descumprimento contratual.
                    // EXEMPLO: Item com 3 O.S. → Glosa1=R$50 + Glosa2=R$30 + Glosa3=R$20 = R$100
                    Glosa = s.Sum(i => i.ValorGlosa),
                    // ─────────────────────────────────────────────────────────────────────
                    // [CÁLCULO 4] Valor Para Ateste (Valor a pagar após descontos)
                    // ─────────────────────────────────────────────────────────────────────
                    // FÓRMULA: PrecoTotalMensal - GlosaTotal
                    // CONCEITO: Valor que o contratante efetivamente pagará após aplicar penalidades.
                    // EXEMPLO: PrecoTotal=R$1.000, Glosa=R$100 → ValorAteste=R$900
                    ValorParaAteste =
                        (s.Max(i => i.Quantidade) * s.Max(i => i.ValorUnitario))
                        - s.Sum(i => i.ValorGlosa),
                })
                .OrderBy(x => x.NumItem);                                 // Ordenação: por número do item

            return query.ToList();
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Lista detalhes individuais de cada O.S. (Ordem de Serviço) glosada.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Retorna UMA LINHA POR O.S., mostrando datas e dias de glosa individual.
        /// │    Útil para auditoria e rastreamento granular de penalidades.
        /// │
        /// │ DIFERENÇA DO RESUMO:
        /// │    - ListarResumo(): Consolida N O.S. em UMA linha por item.
        /// │    - ListarDetalhes(): Retorna N linhas (uma por O.S.), sem agregação.
        /// │
        /// │ PARÂMETROS:
        /// │    -> contratoId: GUID do contrato pai.
        /// │    -> mes: Mês de referência (1-12).
        /// │    -> ano: Ano de referência (ex: 2026).
        /// │
        /// │ RETORNO:
        /// │    -> IEnumerable<GlosaDetalheItemDto>: Lista detalhada (uma linha por O.S.).
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
        public IEnumerable<GlosaDetalheItemDto> ListarDetalhes(Guid contratoId, int mes, int ano)
        {
            // ─────────────────────────────────────────────────────────────────────────────
            // [LÓGICA] Busca direta da ViewGlosa SEM agregação
            // ─────────────────────────────────────────────────────────────────────────────
            // CONCEITO: Cada linha representa uma Ordem de Serviço (O.S.) individual.
            // CAMPOS PRINCIPAIS:
            // - Placa: Identifica o veículo/recurso da O.S.
            // - DataSolicitacao: Quando o serviço foi requisitado.
            // - DataDisponibilidade: Quando deveria estar disponível.
            // - DataRecolhimento: Quando foi efetivamente iniciado.
            // - DataDevolucao: Quando foi finalizado/devolvido (aparece como "Retorno" no front).
            // - DiasGlosa: Quantidade de dias de atraso/falha que geraram penalidade.
            var query = _uow.ViewGlosa.GetAllReducedIQueryable(
                selector: x => new GlosaDetalheItemDto
                {
                    NumItem = x.NumItem,
                    Descricao = x.Descricao,
                    Placa = x.Placa,
                    DataSolicitacao = x.DataSolicitacao,
                    DataDisponibilidade = x.DataDisponibilidade,
                    DataRecolhimento = x.DataRecolhimento,
                    DataDevolucao = x.DataDevolucao,         // Frontend exibe como "Retorno"
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


