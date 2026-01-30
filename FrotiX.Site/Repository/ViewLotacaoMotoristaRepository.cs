/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewLotacaoMotoristaRepository.cs                                                      ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para a SQL View ViewLotacaoMotorista.                                               ║
   ║    Centraliza acesso a dados consolidados de lotação por motorista.                                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewLotacaoMotoristaRepository(FrotiXDbContext db)                                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    Views são somente leitura; este repositório atua como ponto de consulta.                        ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/
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
    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    /// │ 🎯 CLASSE: ViewLotacaoMotoristaRepository                                                     │
    /// │ 📦 HERDA DE: Repository<ViewLotacaoMotorista>                                                 │
    /// │ 🔌 IMPLEMENTA: IViewLotacaoMotoristaRepository                                                │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório responsável pela view de lotação por motorista.
    /// Disponibiliza acesso consolidado para consultas.
    /// </summary>
    public class ViewLotacaoMotoristaRepository : Repository<ViewLotacaoMotorista>, IViewLotacaoMotoristaRepository
        {
        private new readonly FrotiXDbContext _db;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: ViewLotacaoMotoristaRepository                                               │
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
        public ViewLotacaoMotoristaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        }
    }
