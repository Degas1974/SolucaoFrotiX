/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: ControleAcessoRepository.cs                                                             ║
    ║ 📂 CAMINHO: /Repository                                                                             ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: Repositório de ControleAcesso (permissões de usuário x recurso).                      ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 MÉTODOS: GetControleAcessoListForDropDown(), Update()                                            ║
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
     * ⚡ CLASSE: ControleAcessoRepository
     * ─────────────────────────────────────────────────────────────────────────────────────
     * 🎯 OBJETIVO     : Repositório para mapeamento de controle de acesso (usuário x recurso)
     *
     * 📥 ENTRADAS     : DbContext injetado no construtor
     *
     * 📤 SAÍDAS       : Listas de permissões, validações de acesso
     *
     * 🔗 CHAMADA POR  : Controllers de Segurança, Services de Autorização
     *
     * 🔄 CHAMA        : DbContext, LINQ queries, Repository<T> (classe base)
     *
     * 📦 DEPENDÊNCIAS : FrotiXDbContext, Repository<ControleAcesso>, IControleAcessoRepository
     *
     * 📝 OBSERVAÇÕES  : [SEGURANCA] Crítico para sistema de permissões
     *********************************************************************************************/
    public class ControleAcessoRepository : Repository<ControleAcesso>, IControleAcessoRepository
        {
        private new readonly FrotiXDbContext _db;

        /********************************************************************************************
         * ⚡ MÉTODO: ControleAcessoRepository (Construtor)
         * ─────────────────────────────────────────────────────────────────────────────────────
         * 🎯 OBJETIVO     : Inicializar repositório com injeção do DbContext
         *
         * 📥 ENTRADAS     : db [FrotiXDbContext] - Contexto do banco de dados
         *
         * ⬅️ CHAMADO POR  : UnitOfWork, DI container
         *
         * ➡️ CHAMA        : base(db)
         *********************************************************************************************/
        public ControleAcessoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        /********************************************************************************************
         * ⚡ MÉTODO: GetControleAcessoListForDropDown
         * ─────────────────────────────────────────────────────────────────────────────────────
         * 🎯 OBJETIVO     : Retornar lista de mapeamentos de controle de acesso
         *
         * 📥 ENTRADAS     : Nenhum parâmetro
         *
         * 📤 SAÍDAS       : IEnumerable<SelectListItem> - Pares usuário-recurso
         *
         * ⬅️ CHAMADO POR  : Controllers de administração, formulários de permissão
         *
         * ➡️ CHAMA        : DbContext.ControleAcesso, LINQ Select
         *
         * 📝 OBSERVAÇÕES  : [VALIDACAO] Valores: RecursoId (Text), UsuarioId (Value)
         *                   Mapeamento inverso: usuário é value, recurso é text para referência
         *********************************************************************************************/
        public IEnumerable<SelectListItem> GetControleAcessoListForDropDown()
            {
            // [LOGICA] Simples Select sem filtros - retorna todos os mapeamentos de acesso
            return _db.ControleAcesso
            .Select(i => new SelectListItem()
                {
                Text = i.RecursoId.ToString(),
                Value = i.UsuarioId.ToString()
                });
            }

        /********************************************************************************************
         * ⚡ MÉTODO: Update
         * ─────────────────────────────────────────────────────────────────────────────────────
         * 🎯 OBJETIVO     : Atualizar mapeamento de controle de acesso
         *
         * 📥 ENTRADAS     : controleAcesso [ControleAcesso] - Mapeamento com dados atualizados
         *
         * 📤 SAÍDAS       : void - Alterações persistidas no DbContext
         *
         * ⬅️ CHAMADO POR  : UnitOfWork.SaveAsync(), Controllers de edição de permissão
         *
         * ➡️ CHAMA        : DbContext.Update(), DbContext.SaveChanges()
         *
         * 📝 OBSERVAÇÕES  : [SEGURANCA] Mudança de permissões é operação crítica
         *********************************************************************************************/
        public new void Update(ControleAcesso controleAcesso)
            {
            // [DB] Localizar entidade no contexto
            var objFromDb = _db.ControleAcesso.FirstOrDefault(s => s.RecursoId == controleAcesso.RecursoId);

            // [DB] Marcar entidade como modificada e persistir
            _db.Update(controleAcesso);
            _db.SaveChanges();

            }


        }
    }


