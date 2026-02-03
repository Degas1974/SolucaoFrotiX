/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: AtaRegistroPrecosRepository.cs                                                          ║
    ║ 📂 CAMINHO: /Repository                                                                             ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: Repositório de AtaRegistroPrecos (atas de registro e seleção em dropdown).           ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 MÉTODOS: GetAtaListForDropDown(status), Update()                                                 ║
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
     * ⚡ CLASSE: AtaRegistroPrecosRepository
     * ─────────────────────────────────────────────────────────────────────────────────────
     * 🎯 OBJETIVO     : Repositório para Atas de Registro de Preços (licitações com fornecedores)
     *
     * 📥 ENTRADAS     : DbContext injetado no construtor
     *
     * 📤 SAÍDAS       : Dropdowns formatados com ano/numero - fornecedor
     *
     * 🔗 CHAMADA POR  : Controllers de Ata, Services de licitação
     *
     * 🔄 CHAMA        : DbContext, LINQ queries, Repository<T> (classe base)
     *
     * 📦 DEPENDÊNCIAS : FrotiXDbContext, Repository<AtaRegistroPrecos>, IAtaRegistroPrecosRepository
     *********************************************************************************************/
    public class AtaRegistroPrecosRepository : Repository<AtaRegistroPrecos>, IAtaRegistroPrecosRepository
        {
        private new readonly FrotiXDbContext _db;

        /********************************************************************************************
         * ⚡ MÉTODO: AtaRegistroPrecosRepository (Construtor)
         * ─────────────────────────────────────────────────────────────────────────────────────
         * 🎯 OBJETIVO     : Inicializar repositório com injeção do DbContext
         *
         * 📥 ENTRADAS     : db [FrotiXDbContext] - Contexto do banco de dados
         *
         * ⬅️ CHAMADO POR  : UnitOfWork, DI container
         *
         * ➡️ CHAMA        : base(db)
         *********************************************************************************************/
        public AtaRegistroPrecosRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        /********************************************************************************************
         * ⚡ MÉTODO: GetAtaListForDropDown
         * ─────────────────────────────────────────────────────────────────────────────────────
         * 🎯 OBJETIVO     : Retornar lista de atas de registro formatadas para dropdown/select UI
         *
         * 📥 ENTRADAS     : status [int] - 1 (true) ou 0 (false) para filtrar atas ativas/inativas
         *
         * 📤 SAÍDAS       : IEnumerable<SelectListItem> - Atas formatadas em (Ano/Numero - Fornecedor)
         *
         * ⬅️ CHAMADO POR  : Controllers de Licitação, Pages de Ata, Dropdowns de seleção
         *
         * ➡️ CHAMA        : DbContext.AtaRegistroPrecos, DbContext.Fornecedor, LINQ Join/Where/Select
         *
         * 📝 OBSERVAÇÕES  : [LOGICA] Usa .Join() manual ao invés de navegação de propriedade.
         *                   [PERFORMANCE] Eager load de Fornecedor via Join explícito.
         *                   Converte status int→bool para comparação com flag booleana.
         *********************************************************************************************/
        public IEnumerable<SelectListItem> GetAtaListForDropDown(int status)
            {
            // [VALIDACAO] Converter status int (0/1) para booleano para comparação
            return _db.AtaRegistroPrecos
            .Where(s => s.Status == Convert.ToBoolean(status))
            // [LOGICA] Join manual: AtaRegistroPrecos com Fornecedor
            // Chave esquerda: FornecedorId de Ata
            // Chave direita: FornecedorId de Fornecedor
            // Resultado: Anônimo com ataregistroprecos e fornecedor para uso em Select
            .Join(_db.Fornecedor,
                ataregistroprecos => ataregistroprecos.FornecedorId,
                fornecedor => fornecedor.FornecedorId,
                (ataregistroprecos, fornecedor) => new { ataregistroprecos, fornecedor })
            // [LOGICA] Ordenação por string concatenada: usa campo calculado descendente
            .OrderByDescending(o => o.ataregistroprecos.AnoAta + "/" + o.ataregistroprecos.NumeroAta + " - " + o.fornecedor.DescricaoFornecedor)
            .Select(i => new SelectListItem()
                {
                // [DADOS] Formatação: Ano/Numero - NomeFornecedor
                Text = i.ataregistroprecos.AnoAta + "/" + i.ataregistroprecos.NumeroAta + " - " + i.fornecedor.DescricaoFornecedor,
                Value = i.ataregistroprecos.AtaId.ToString()
                });
            }

        /********************************************************************************************
         * ⚡ MÉTODO: Update
         * ─────────────────────────────────────────────────────────────────────────────────────
         * 🎯 OBJETIVO     : Atualizar registro de ata no banco de dados
         *
         * 📥 ENTRADAS     : ataRegistroPrecos [AtaRegistroPrecos] - Entidade com dados atualizados
         *
         * 📤 SAÍDAS       : void - Alterações persistidas no DbContext
         *
         * ⬅️ CHAMADO POR  : UnitOfWork.SaveAsync(), Controllers de edição de Ata
         *
         * ➡️ CHAMA        : DbContext.Update(), DbContext.SaveChanges()
         *
         * 📝 OBSERVAÇÕES  : [DB] FirstOrDefault localiza entidade atual (não usado no Update)
         *                   [PERFORMANCE] SaveChanges() executa UPDATE SQL imediato
         *********************************************************************************************/
        public new void Update(AtaRegistroPrecos ataRegistroPrecos)
            {
            // [DB] Localizar entidade no contexto (atualmente não utilizado, mantido para compatibilidade)
            var objFromDb = _db.AtaRegistroPrecos.FirstOrDefault(s => s.AtaId == ataRegistroPrecos.AtaId);

            // [DB] Marcar entidade como modificada no contexto EF Core
            _db.Update(ataRegistroPrecos);
            // [DB] Persistir alterações no banco de dados
            _db.SaveChanges();

            }


        }
    }


