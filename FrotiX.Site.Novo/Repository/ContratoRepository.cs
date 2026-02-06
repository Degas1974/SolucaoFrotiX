/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: ContratoRepository.cs                                                                   ║
    ║ 📂 CAMINHO: /Repository                                                                             ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: Repositório de Contrato (dropdown filtrado por tipo e contratos ativos).              ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 MÉTODOS: GetDropDown(tipoContrato?) → IQueryable<SelectListItem>                                 ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🔗 DEPS: FrotiX.Data, Repository<T>, SelectListItem, Microsoft.EntityFrameworkCore                  ║
    ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
    ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */
using System.Linq;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FrotiX.Repository
    {
    /********************************************************************************************
     * ⚡ CLASSE: ContratoRepository
     * ─────────────────────────────────────────────────────────────────────────────────────
     * 🎯 OBJETIVO     : Repositório especializado para entidade Contrato
     *
     * 📥 ENTRADAS     : Contexto FrotiXDbContext injetado no construtor
     *
     * 📤 SAÍDAS       : Métodos que retornam queries lazy (IQueryable) e dropdowns formatados
     *
     * 🔗 CHAMADA POR  : UnitOfWork, Services, Controllers de Contrato
     *
     * 🔄 CHAMA        : DbContext, LINQ queries, Repository<T> (classe base)
     *
     * 📦 DEPENDÊNCIAS : FrotiXDbContext, Repository<Contrato>, IContratoRepository
     *
     * 📝 OBSERVAÇÕES  : Padrão GenericRepository com especializações para Contrato.
     *                   Implementa interface IContratoRepository para contrato com DI.
     *********************************************************************************************/
    public class ContratoRepository : Repository<Contrato>, IContratoRepository
        {
        private new readonly FrotiXDbContext _db;

        /********************************************************************************************
         * ⚡ MÉTODO: ContratoRepository (Construtor)
         * ─────────────────────────────────────────────────────────────────────────────────────
         * 🎯 OBJETIVO     : Inicializar repositório com injeção de dependência do DbContext
         *
         * 📥 ENTRADAS     : db [FrotiXDbContext] - Contexto do banco de dados
         *
         * 📤 SAÍDAS       : void (construtor)
         *
         * ⬅️ CHAMADO POR  : UnitOfWork, injeção de dependência (DI container)
         *
         * ➡️ CHAMA        : base(db) - classe Repository<Contrato>
         *********************************************************************************************/
        public ContratoRepository(FrotiXDbContext db)
            : base(db)
            {
            _db = db;
            }

        /********************************************************************************************
         * ⚡ MÉTODO: GetDropDown
         * ─────────────────────────────────────────────────────────────────────────────────────
         * 🎯 OBJETIVO     : Retornar lista de contratos ativos formatados para dropdown/select UI
         *
         * 📥 ENTRADAS     : tipoContrato [string?] - Tipo de contrato para filtro opcional
         *                   (ex: "LOCAÇÃO", "COMPRA"). Se null, retorna todos os tipos.
         *
         * 📤 SAÍDAS       : IQueryable<SelectListItem> - Query lazy (não executada até ToList())
         *                   com contratos formatados em (Ano/Numero - Fornecedor [Tipo])
         *
         * ⬅️ CHAMADO POR  : Controllers/Pages que populam selects de contrato
         *                   Ex: ContratoController, VeiculoPage
         *
         * ➡️ CHAMA        : DbContext.Set<Contrato>, LINQ Where/OrderBy/Select
         *
         * 📝 OBSERVAÇÕES  : [PERFORMANCE] Query sem .Include() - navegação Fornecedor
         *                   vira JOIN automático na SQL. AsNoTracking() otimiza para leitura.
         *                   Retorna IQueryable (lazy) não IEnumerable (eager).
         *********************************************************************************************/
        public IQueryable<SelectListItem> GetDropDown(string? tipoContrato = null)
            {
            // [VALIDACAO] Verificar se tipoContrato foi preenchido para filtro condicional
            var temTipo = !string.IsNullOrWhiteSpace(tipoContrato);

            // [LOGICA] LINQ complexo com 5 operações encadeadas:
            // 1. AsNoTracking: Otimização para queries de leitura (sem rastreamento de mudanças)
            // 2. Where: Filtro duplo - Status=true AND (sem tipo OU tipo correspondente)
            // 3. OrderByDescending 3x: Ordenação: Ano desc → Numero desc → Fornecedor desc
            // 4. Select: Transformação em SelectListItem com formato condicional (com/sem tipo)
            return _db.Set<Contrato>()
                .AsNoTracking()
                .Where(c => c.Status && (!temTipo || c.TipoContrato == tipoContrato))
                // [PERFORMANCE] Navegação para Fornecedor.DescricaoFornecedor será JOIN automático
                .OrderByDescending(c => c.AnoContrato)
                .ThenByDescending(c => c.NumeroContrato)
                .ThenByDescending(c => c.Fornecedor.DescricaoFornecedor)
                .Select(c => new SelectListItem
                    {
                    Value = c.ContratoId.ToString(),
                    // [LOGICA] Formatação condicional:
                    // Se temTipo=true: mostrar "2026/001 - Fornecedor" (sem tipo redundante)
                    // Se temTipo=false: mostrar "2026/001 - Fornecedor (LOCAÇÃO)" (com tipo para clareza)
                    Text = temTipo
                        ? $"{c.AnoContrato}/{c.NumeroContrato} - {c.Fornecedor.DescricaoFornecedor}"
                        : $"{c.AnoContrato}/{c.NumeroContrato} - {c.Fornecedor.DescricaoFornecedor} ({c.TipoContrato})",
                    });
            }
        }
    }


