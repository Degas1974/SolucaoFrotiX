/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: EmpenhoMultaRepository.cs                                                               ║
    ║ 📂 CAMINHO: /Repository                                                                             ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: Repositório de empenhos de multas (controle financeiro por órgão).                    ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 MÉTODOS: GetEmpenhoMultaListForDropDown(), Update()                                              ║
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
     * ⚡ CLASSE: EmpenhoMultaRepository
     * ─────────────────────────────────────────────────────────────────────────────────────
     * 🎯 OBJETIVO     : Repositório para empenho de multas (alocação de recursos por órgão)
     *
     * 📥 ENTRADAS     : DbContext injetado no construtor
     *
     * 📤 SAÍDAS       : Dropdowns de notas de empenho com órgão responsável
     *
     * 🔗 CHAMADA POR  : Controllers de Multa, Services de Empenho
     *
     * 🔄 CHAMA        : DbContext, LINQ queries, Repository<T> (classe base)
     *
     * 📦 DEPENDÊNCIAS : FrotiXDbContext, Repository<EmpenhoMulta>, IEmpenhoMultaRepository
     *********************************************************************************************/
    public class EmpenhoMultaRepository : Repository<EmpenhoMulta>, IEmpenhoMultaRepository
        {
        private new readonly FrotiXDbContext _db;

        /********************************************************************************************
         * ⚡ MÉTODO: EmpenhoMultaRepository (Construtor)
         * ─────────────────────────────────────────────────────────────────────────────────────
         * 🎯 OBJETIVO     : Inicializar repositório com injeção do DbContext
         *
         * 📥 ENTRADAS     : db [FrotiXDbContext] - Contexto do banco de dados
         *
         * ⬅️ CHAMADO POR  : UnitOfWork, DI container
         *
         * ➡️ CHAMA        : base(db)
         *********************************************************************************************/
        public EmpenhoMultaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        /********************************************************************************************
         * ⚡ MÉTODO: GetEmpenhoMultaListForDropDown
         * ─────────────────────────────────────────────────────────────────────────────────────
         * 🎯 OBJETIVO     : Retornar lista de empenhos de multa para dropdown/select UI
         *
         * 📥 ENTRADAS     : Nenhum parâmetro
         *
         * 📤 SAÍDAS       : IEnumerable<SelectListItem> - Notas com órgão autuan formatt-u
         *
         * ⬅️ CHAMADO POR  : Formulários de multa, seleção de empenho
         *
         * ➡️ CHAMA        : DbContext.EmpenhoMulta, DbContext.OrgaoAutuante, LINQ Join/Select
         *
         * 📝 OBSERVAÇÕES  : [LOGICA] Join manual: EmpenhoMulta com OrgaoAutuante
         *                   Formata resultado: NotaEmpenho (Sigla/Nome do órgão)
         *********************************************************************************************/
        public IEnumerable<SelectListItem> GetEmpenhoMultaListForDropDown()
            {
            // [LOGICA] Join manual com 3 operações encadeadas:
            // 1. Join: EmpenhoMulta + OrgaoAutuante (liga por OrgaoAutuanteId)
            // 2. OrderBy: Ordenação por nota de empenho
            // 3. Select: Transformação em SelectListItem com formatação
            return _db.EmpenhoMulta
            .Join(_db.OrgaoAutuante,
                empenhomulta => empenhomulta.OrgaoAutuanteId,
                orgaoautuante => orgaoautuante.OrgaoAutuanteId,
                (empenhomulta, orgaoautuante) => new { empenhomulta, orgaoautuante })
            .OrderBy(o => o.empenhomulta.NotaEmpenho)
            .Select(i => new SelectListItem()
                {
                // [DADOS] Formatação: NotaEmpenho (SiglaOrgao/NomeOrgao)
                Text = i.empenhomulta.NotaEmpenho + "(" + i.orgaoautuante.Sigla + "/" + i.orgaoautuante.Nome + ")",
                Value = i.empenhomulta.EmpenhoMultaId.ToString()
                });
            }

        /********************************************************************************************
         * ⚡ MÉTODO: Update
         * ─────────────────────────────────────────────────────────────────────────────────────
         * 🎯 OBJETIVO     : Atualizar registro de empenho de multa no banco
         *
         * 📥 ENTRADAS     : empenhomulta [EmpenhoMulta] - Entidade a atualizar
         *
         * 📤 SAÍDAS       : void - Alterações persistidas no DbContext
         *
         * ⬅️ CHAMADO POR  : UnitOfWork.SaveAsync(), Controllers de edição de Empenho
         *
         * ➡️ CHAMA        : DbContext.Update(), DbContext.SaveChanges()
         *********************************************************************************************/
        public new void Update(EmpenhoMulta empenhomulta)
            {
            // [DB] Localizar entidade no contexto
            var objFromDb = _db.EmpenhoMulta.FirstOrDefault(s => s.EmpenhoMultaId == empenhomulta.EmpenhoMultaId);

            // [DB] Marcar como modificada e persistir
            _db.Update(empenhomulta);
            _db.SaveChanges();

            }
        }
    }


