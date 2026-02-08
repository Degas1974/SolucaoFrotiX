/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: EmpenhoRepository.cs                                                                   ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para notas de empenho orçamentário vinculadas a contratos.                          ║
   ║    Fornece listagem com JOIN de contrato e atualização de registros.                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • EmpenhoRepository(FrotiXDbContext db)                                                         ║
   ║    • GetEmpenhoListForDropDown()                                                                   ║
   ║    • Update(Empenho empenho)                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    A listagem utiliza JOIN com Contrato e usa ContratoId como Value.                               ║
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
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: EmpenhoRepository                                                                  │
    // │ 📦 HERDA DE: Repository                                                              │
    // │ 🔌 IMPLEMENTA: IEmpenhoRepository                                                             │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável por notas de empenho orçamentário.
    // Centraliza listagens para UI e atualização de registros.
    
    public class EmpenhoRepository : Repository<Empenho>, IEmpenhoRepository
        {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: EmpenhoRepository                                                             │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public EmpenhoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }


        /********************************************************************************************
         * ⚡ MÉTODO: GetEmpenhoListForDropDown
         * ─────────────────────────────────────────────────────────────────────────────────────
         * 🎯 OBJETIVO     : Retornar lista de notas de empenho com contrato para dropdown UI
         *
         * 📥 ENTRADAS     : Nenhum parâmetro
         *
         * 📤 SAÍDAS       : IEnumerable<SelectListItem> - Notas formatadas com ano/número contrato
         *
         * ⬅️ CHAMADO POR  : Controllers de Empenho, Formulários de seleção
         *
         * ➡️ CHAMA        : DbContext.Empenho, DbContext.Contrato, LINQ Join/OrderBy/Select
         *
         * 📝 OBSERVAÇÕES  : [LOGICA] Join manual: Empenho + Contrato para enriquecimento de dados
         *                   Value é ContratoId, não EmpenhoId - para filtrar por contrato
         *********************************************************************************************/
        public IEnumerable<SelectListItem> GetEmpenhoListForDropDown()
            {
            // [LOGICA] Query com 3 operações encadeadas:
            // 1. Join: Empenho com Contrato (chave: ContratoId)
            // 2. OrderBy: Ordenação por nota de empenho
            // 3. Select: Transformação em SelectListItem com formatação
            return _db.Empenho
            .Join(_db.Contrato,
                empenho => empenho.ContratoId,
                contrato => contrato.ContratoId,
                (empenho, contrato) => new { empenho, contrato })
            .OrderBy(o => o.empenho.NotaEmpenho)
            .Select(i => new SelectListItem()
                {
                // [DADOS] Formatação: NotaEmpenho (AnoContrato/NumeroContrato)
                Text = i.empenho.NotaEmpenho + "(" + i.contrato.AnoContrato + "/" + i.contrato.NumeroContrato + ")",
                // [LOGICA] Value é ContratoId para filtrar por contrato associado
                Value = i.contrato.ContratoId.ToString()
                });
            }


        /********************************************************************************************
         * ⚡ MÉTODO: Update
         * ─────────────────────────────────────────────────────────────────────────────────────
         * 🎯 OBJETIVO     : Atualizar registro de empenho no banco de dados
         *
         * 📥 ENTRADAS     : empenho [Empenho] - Entidade com dados atualizados
         *
         * 📤 SAÍDAS       : void - Alterações persistidas no DbContext
         *
         * ⬅️ CHAMADO POR  : UnitOfWork.SaveAsync(), Controllers de edição de Empenho
         *
         * ➡️ CHAMA        : DbContext.Update(), DbContext.SaveChanges()
         *********************************************************************************************/
        public new void Update(Empenho empenho)
            {
            // [DB] Localizar entidade no contexto
            var objFromDb = _db.Empenho.FirstOrDefault(s => s.EmpenhoId == empenho.EmpenhoId);

            // [DB] Marcar como modificada e persistir
            _db.Update(empenho);
            _db.SaveChanges();

            }
        }
    }
