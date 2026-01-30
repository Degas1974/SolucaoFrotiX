/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewOcorrenciasViagemRepository.cs                                                     ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para a SQL View ViewOcorrenciasViagem.                                              ║
   ║    Fornece consultas filtradas de ocorrências registradas em viagens.                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewOcorrenciasViagemRepository(FrotiXDbContext db)                                           ║
   ║    • GetAll(Expression<Func<ViewOcorrenciasViagem, bool>> filter = null, ...)                     ║
   ║    • GetFirstOrDefault(Expression<Func<ViewOcorrenciasViagem, bool>> filter, ...)                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    O parâmetro includeProperties é aceito por compatibilidade, mas não é utilizado.               ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;

namespace FrotiX.Repository
{
    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    /// │ 🎯 CLASSE: ViewOcorrenciasViagemRepository                                                    │
    /// │ 📦 HERDA DE: (não aplicável)                                                                  │
    /// │ 🔌 IMPLEMENTA: IViewOcorrenciasViagemRepository                                               │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório responsável pela view de ocorrências em viagens.
    /// Disponibiliza consultas filtradas para uso em serviços e controllers.
    /// </summary>
    public class ViewOcorrenciasViagemRepository : IViewOcorrenciasViagemRepository
    {
        private new readonly FrotiXDbContext _db;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: ViewOcorrenciasViagemRepository                                              │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        /// │    ➡️ CHAMA       : (atribuição de contexto)                                              │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Inicializar o repositório com o contexto do banco de dados.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    db - Contexto do banco de dados da aplicação.
        /// </para>
        /// </summary>
        /// <param name="db">Instância de <see cref="FrotiXDbContext"/>.</param>
        public ViewOcorrenciasViagemRepository(FrotiXDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetAll                                                                        │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        /// │    ➡️ CHAMA       : DbContext.ViewOcorrenciasViagem, Where, ToList                        │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Obter lista de ocorrências de viagem, com filtro opcional.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    filter - Filtro opcional para a consulta<br/>
        ///    includeProperties - Propriedades de navegação (não utilizado)
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;ViewOcorrenciasViagem&gt; - Lista de ocorrências.
        /// </para>
        /// </summary>
        /// <param name="filter">Filtro opcional para a consulta.</param>
        /// <param name="includeProperties">Includes (não utilizado).</param>
        /// <returns>Lista de ocorrências de viagem.</returns>
        public IEnumerable<ViewOcorrenciasViagem> GetAll(Expression<Func<ViewOcorrenciasViagem , bool>>? filter = null , string? includeProperties = null)
        {
            IQueryable<ViewOcorrenciasViagem> query = _db.ViewOcorrenciasViagem;

            if (filter != null)
                query = query.Where(filter);

            return query.ToList();
        }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetFirstOrDefault                                                             │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        /// │    ➡️ CHAMA       : DbContext.ViewOcorrenciasViagem, Where, FirstOrDefault               │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Retornar a primeira ocorrência que atende ao filtro informado.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    filter - Filtro obrigatório para a consulta<br/>
        ///    includeProperties - Propriedades de navegação (não utilizado)
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    ViewOcorrenciasViagem? - Registro encontrado ou null.
        /// </para>
        /// </summary>
        /// <param name="filter">Filtro obrigatório.</param>
        /// <param name="includeProperties">Includes (não utilizado).</param>
        /// <returns>Registro encontrado ou null.</returns>
        public ViewOcorrenciasViagem? GetFirstOrDefault(Expression<Func<ViewOcorrenciasViagem , bool>> filter , string? includeProperties = null)
        {
            return _db.ViewOcorrenciasViagem.Where(filter).FirstOrDefault();
        }
    }
}
