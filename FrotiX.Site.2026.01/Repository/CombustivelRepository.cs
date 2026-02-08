/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: CombustivelRepository.cs                                                                ║
    ║ 📂 CAMINHO: /Repository                                                                             ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: Repositório de Combustivel (listas e CRUD de tipos de combustível).                   ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 MÉTODOS: GetCombustivelListForDropDown(), Update()                                               ║
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
     * ⚡ CLASSE: CombustivelRepository
     * ─────────────────────────────────────────────────────────────────────────────────────
     * 🎯 OBJETIVO     : Repositório para tipos de combustível (gasolina, diesel, etanol, etc)
     *
     * 📥 ENTRADAS     : DbContext injetado no construtor
     *
     * 📤 SAÍDAS       : Dropdowns de combustíveis ativos ordenados alfabeticamente
     *
     * 🔗 CHAMADA POR  : Controllers de Abastecimento, Pages de Manutenção
     *
     * 🔄 CHAMA        : DbContext, LINQ queries, Repository<T> (classe base)
     *
     * 📦 DEPENDÊNCIAS : FrotiXDbContext, Repository<Combustivel>, ICombustivelRepository
     *********************************************************************************************/
    public class CombustivelRepository : Repository<Combustivel>, ICombustivelRepository
        {
        private new readonly FrotiXDbContext _db;

        /********************************************************************************************
         * ⚡ MÉTODO: CombustivelRepository (Construtor)
         * ─────────────────────────────────────────────────────────────────────────────────────
         * 🎯 OBJETIVO     : Inicializar repositório com injeção do DbContext
         *
         * 📥 ENTRADAS     : db [FrotiXDbContext] - Contexto do banco de dados
         *
         * ⬅️ CHAMADO POR  : UnitOfWork, DI container
         *
         * ➡️ CHAMA        : base(db)
         *********************************************************************************************/
        public CombustivelRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        /********************************************************************************************
         * ⚡ MÉTODO: GetCombustivelListForDropDown
         * ─────────────────────────────────────────────────────────────────────────────────────
         * 🎯 OBJETIVO     : Retornar lista de combustíveis ativos para dropdown/select UI
         *
         * 📥 ENTRADAS     : Nenhum parâmetro
         *
         * 📤 SAÍDAS       : IEnumerable<SelectListItem> - Combustíveis ordenados alfabeticamente
         *
         * ⬅️ CHAMADO POR  : Controllers de Abastecimento, Formulários de entrada de combustível
         *
         * ➡️ CHAMA        : DbContext.Combustivel, LINQ Where/OrderBy/Select
         *
         * 📝 OBSERVAÇÕES  : [LOGICA] Filtra apenas combustíveis com Status=true (ativos)
         *                   [LOGICA] Ordenação alfabética por Descricao facilita busca do usuário
         *********************************************************************************************/
        public IEnumerable<SelectListItem> GetCombustivelListForDropDown()
            {
            // [LOGICA] Query com 3 operações: Where (status ativo) + OrderBy (alfabético) + Select (formato UI)
            return _db.Combustivel
                .Where(e => e.Status)
                .OrderBy(o => o.Descricao)
                .Select(i => new SelectListItem()
                    {
                    Text = i.Descricao,
                    Value = i.CombustivelId.ToString()
                    });
            }

        /********************************************************************************************
         * ⚡ MÉTODO: Update
         * ─────────────────────────────────────────────────────────────────────────────────────
         * 🎯 OBJETIVO     : Atualizar registro de combustível no banco de dados
         *
         * 📥 ENTRADAS     : combustivel [Combustivel] - Entidade com dados atualizados
         *
         * 📤 SAÍDAS       : void - Alterações persistidas no DbContext
         *
         * ⬅️ CHAMADO POR  : UnitOfWork.SaveAsync(), Controllers de edição de Combustível
         *
         * ➡️ CHAMA        : DbContext.Update(), DbContext.SaveChanges()
         *********************************************************************************************/
        public new void Update(Combustivel combustivel)
            {
            // [DB] Localizar entidade no contexto (atualmente não utilizado, mantido para compatibilidade)
            var objFromDb = _db.Combustivel.FirstOrDefault(s => s.CombustivelId == combustivel.CombustivelId);

            // [DB] Marcar entidade como modificada e persistir
            _db.Update(combustivel);
            _db.SaveChanges();

            }


        }
    }


