/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewOcorrenciasAbertasVeiculoRepository.cs                                             ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para a SQL View ViewOcorrenciasAbertasVeiculo.                                      ║
   ║    Fornece consultas filtradas de ocorrências abertas por veículo.                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewOcorrenciasAbertasVeiculoRepository(FrotiXDbContext db)                                   ║
   ║    • GetAll(Expression<Func<ViewOcorrenciasAbertasVeiculo, bool>> filter = null, ...)             ║
   ║    • GetFirstOrDefault(Expression<Func<ViewOcorrenciasAbertasVeiculo, bool>> filter, ...)         ║
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
    /// │ 🎯 CLASSE: ViewOcorrenciasAbertasVeiculoRepository                                            │
    /// │ 📦 HERDA DE: (não aplicável)                                                                  │
    /// │ 🔌 IMPLEMENTA: IViewOcorrenciasAbertasVeiculoRepository                                       │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório responsável pela view de ocorrências abertas por veículo.
    /// Disponibiliza consultas filtradas para uso em serviços e controllers.
    /// </summary>
    public class ViewOcorrenciasAbertasVeiculoRepository : IViewOcorrenciasAbertasVeiculoRepository
    {
        private new readonly FrotiXDbContext _db;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: ViewOcorrenciasAbertasVeiculoRepository                                      │
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
        public ViewOcorrenciasAbertasVeiculoRepository(FrotiXDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetAll                                                                        │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        /// │    ➡️ CHAMA       : DbContext.ViewOcorrenciasAbertasVeiculo, Where, ToList               │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Obter lista de ocorrências abertas por veículo, com filtro opcional.
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
        ///    IEnumerable&lt;ViewOcorrenciasAbertasVeiculo&gt; - Lista de ocorrências abertas.
        /// </para>
        /// </summary>
        /// <param name="filter">Filtro opcional para a consulta.</param>
        /// <param name="includeProperties">Includes (não utilizado).</param>
        /// <returns>Lista de ocorrências abertas.</returns>
        public IEnumerable<ViewOcorrenciasAbertasVeiculo> GetAll(Expression<Func<ViewOcorrenciasAbertasVeiculo , bool>>? filter = null , string? includeProperties = null)
        {
            IQueryable<ViewOcorrenciasAbertasVeiculo> query = _db.ViewOcorrenciasAbertasVeiculo;

            if (filter != null)
                query = query.Where(filter);

            return query.ToList();
        }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetFirstOrDefault                                                             │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        /// │    ➡️ CHAMA       : DbContext.ViewOcorrenciasAbertasVeiculo, Where, FirstOrDefault       │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Retornar a primeira ocorrência aberta que atende ao filtro informado.
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
        ///    ViewOcorrenciasAbertasVeiculo? - Registro encontrado ou null.
        /// </para>
        /// </summary>
        /// <param name="filter">Filtro obrigatório.</param>
        /// <param name="includeProperties">Includes (não utilizado).</param>
        /// <returns>Registro encontrado ou null.</returns>
        public ViewOcorrenciasAbertasVeiculo? GetFirstOrDefault(Expression<Func<ViewOcorrenciasAbertasVeiculo , bool>> filter , string? includeProperties = null)
        {
            return _db.ViewOcorrenciasAbertasVeiculo.Where(filter).FirstOrDefault();
        }
    }
}
