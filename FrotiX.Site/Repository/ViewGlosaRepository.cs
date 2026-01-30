/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewGlosaRepository.cs                                                                 ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para a SQL View ViewGlosa.                                                          ║
   ║    Centraliza o acesso a dados consolidados de glosas.                                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewGlosaRepository(FrotiXDbContext db)                                                        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    Views são somente leitura; este repositório atua como ponto de consulta.                        ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/
using FrotiX.Data;
using FrotiX.Models; // ViewGlosaModel
using FrotiX.Repository.IRepository;

namespace FrotiX.Repository
    {
    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    /// │ 🎯 CLASSE: ViewGlosaRepository                                                                │
    /// │ 📦 HERDA DE: Repository<ViewGlosa>                                                           │
    /// │ 🔌 IMPLEMENTA: IViewGlosaRepository                                                          │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório responsável pela view de glosas.
    /// Disponibiliza acesso consolidado para consultas.
    /// </summary>
    public class ViewGlosaRepository : Repository<ViewGlosa>, IViewGlosaRepository
        {
        private new readonly FrotiXDbContext _db;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: ViewGlosaRepository                                                          │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        /// │    ➡️ CHAMA       : base(db)                                                             │
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
        public ViewGlosaRepository(FrotiXDbContext db)
            : base(db)
            {
            _db = db;
            }
        }
    }
