/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: CorridasTaxiLegRepository.cs                                                            ║
    ║ 📂 CAMINHO: /Repository                                                                             ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: Repositório de corridas TaxiLeg (agendadas, realizadas e validações).                 ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 MÉTODOS: GetCorridasTaxiLegListForDropDown(), Update(), ExisteCorridaNoMesAno()                  ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🔗 DEPS: FrotiX.Data, Repository<T>, SelectListItem                                                 ║
    ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
    ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository
    {
    /********************************************************************************************
     * ⚡ CLASSE: CorridasTaxiLegRepository
     * ─────────────────────────────────────────────────────────────────────────────────────
     * 🎯 OBJETIVO     : Repositório para corridas TaxiLeg (táxi legislativo/parlamentar)
     *
     * 📥 ENTRADAS     : DbContext injetado no construtor
     *
     * 📤 SAÍDAS       : Dropdowns de corridas, validações de disponibilidade
     *
     * 🔗 CHAMADA POR  : Controllers de TaxiLeg, Services de Agenda
     *
     * 🔄 CHAMA        : DbContext, LINQ queries, Repository<T> (classe base)
     *
     * 📦 DEPENDÊNCIAS : FrotiXDbContext, Repository<CorridasTaxiLeg>, ICorridasTaxiLegRepository
     *********************************************************************************************/
    public class CorridasTaxiLegRepository : Repository<CorridasTaxiLeg>, ICorridasTaxiLegRepository
        {
        private new readonly FrotiXDbContext _db;

        /********************************************************************************************
         * ⚡ MÉTODO: CorridasTaxiLegRepository (Construtor)
         * ─────────────────────────────────────────────────────────────────────────────────────
         * 🎯 OBJETIVO     : Inicializar repositório com injeção do DbContext
         *
         * 📥 ENTRADAS     : db [FrotiXDbContext] - Contexto do banco de dados
         *
         * ⬅️ CHAMADO POR  : UnitOfWork, DI container
         *
         * ➡️ CHAMA        : base(db)
         *********************************************************************************************/
        public CorridasTaxiLegRepository(FrotiXDbContext db)
            : base(db)
            {
            _db = db;
            }

        /********************************************************************************************
         * ⚡ MÉTODO: GetCorridasTaxiLegListForDropDown
         * ─────────────────────────────────────────────────────────────────────────────────────
         * 🎯 OBJETIVO     : Retornar lista de corridas TaxiLeg para dropdown/select UI
         *
         * 📥 ENTRADAS     : Nenhum parâmetro
         *
         * 📤 SAÍDAS       : IEnumerable<SelectListItem> - Corridas com descrição de unidade
         *
         * ⬅️ CHAMADO POR  : Formulários que listam corridas disponíveis
         *
         * ➡️ CHAMA        : DbContext.CorridasTaxiLeg, LINQ Select
         *********************************************************************************************/
        public IEnumerable<SelectListItem> GetCorridasTaxiLegListForDropDown()
            {
            // [LOGICA] Simples Select: retorna todas as corridas com descrição de unidade
            return _db.CorridasTaxiLeg.Select(i => new SelectListItem()
                {
                Text = i.DescUnidade,
                Value = i.CorridaId.ToString(),
                });
            }

        /********************************************************************************************
         * ⚡ MÉTODO: Update
         * ─────────────────────────────────────────────────────────────────────────────────────
         * 🎯 OBJETIVO     : Atualizar registro de corrida TaxiLeg no banco
         *
         * 📥 ENTRADAS     : corridasTaxiLeg [CorridasTaxiLeg] - Entidade a atualizar
         *
         * 📤 SAÍDAS       : void - Alterações persistidas no DbContext
         *
         * ⬅️ CHAMADO POR  : UnitOfWork.SaveAsync(), Controllers de edição de TaxiLeg
         *
         * ➡️ CHAMA        : DbContext.Update(), DbContext.SaveChanges()
         *********************************************************************************************/
        public new void Update(CorridasTaxiLeg corridasTaxiLeg)
            {
            // [DB] Localizar entidade no contexto
            var objFromDb = _db.CorridasTaxiLeg.FirstOrDefault(s =>
                s.CorridaId == corridasTaxiLeg.CorridaId
            );

            // [DB] Marcar como modificada e persistir
            _db.Update(corridasTaxiLeg);
            _db.SaveChanges();
            }

        /********************************************************************************************
         * ⚡ MÉTODO: ExisteCorridaNoMesAno
         * ─────────────────────────────────────────────────────────────────────────────────────
         * 🎯 OBJETIVO     : Verificar se existe corrida agendada em mês/ano específico
         *
         * 📥 ENTRADAS     : ano [int] - Ano de busca
         *                   mes [int] - Mês de busca (1-12)
         *
         * 📤 SAÍDAS       : bool - true se existe corrida no período, false caso contrário
         *
         * ⬅️ CHAMADO POR  : Services/Controllers de validação de agenda
         *
         * ➡️ CHAMA        : DbContext.CorridasTaxiLeg, LINQ Any
         *
         * 📝 OBSERVAÇÕES  : [LOGICA] Filtra por DataAgenda nullable com validação Year/Month
         *                   [PERFORMANCE] Any() otimizado para existência (sem carregar dados)
         *********************************************************************************************/
        public bool ExisteCorridaNoMesAno(int ano, int mes)
            {
            // [LOGICA] Verificação: DataAgenda preenchida AND Year/Month correspondem
            return _db.CorridasTaxiLeg.Any(x =>
                x.DataAgenda.HasValue
                && x.DataAgenda.Value.Year == ano
                && x.DataAgenda.Value.Month == mes
            );
            }
        }
    }


