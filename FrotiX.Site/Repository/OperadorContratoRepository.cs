/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: OperadorContratoRepository.cs                                                          ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para relacionamento N:N entre Operador e Contrato.                                 ║
   ║    Gerencia vínculo de operadores de frota aos contratos de abastecimento.                        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • OperadorContratoRepository(FrotiXDbContext db)                                                ║
   ║    • IEnumerable<SelectListItem> GetOperadorContratoListForDropDown()                             ║
   ║    • void Update(OperadorContrato operadorContrato)                                                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    Entidade com chave composta (OperadorId, ContratoId).                                          ║
   ║    GetOperadorContratoListForDropDown possui implementação incompleta.                            ║
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
    /// │ 🎯 CLASSE: OperadorContratoRepository                                                         │
    /// │ 📦 HERDA DE: Repository&lt;OperadorContrato&gt;                                                       │
    /// │ 🔌 IMPLEMENTA: IOperadorContratoRepository                                                    │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório de relacionamento N:N entre Operador e Contrato.
    /// Controla quais operadores estão autorizados em quais contratos de abastecimento.
    /// </summary>
    public class OperadorContratoRepository : Repository<OperadorContrato>, IOperadorContratoRepository
        {
        private new readonly FrotiXDbContext _db;

        public OperadorContratoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetOperadorContratoListForDropDown                                          │
        /// │ 🔗 RASTREABILIDADE:                                                                    │
        /// │    ⬅️ CHAMADO POR : Controllers de gerenciamento de contratos                          │
        /// │    ➡️ CHAMA       : DbContext.OperadorContrato, Linq Select                             │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Retorna lista de vínculos operador-contrato para uso em DropDown.
        /// </para>
        ///
        /// <para>
        /// ⚠️ <b>ATENÇÃO:</b><br/>
        ///    IMPLEMENTAÇÃO INCOMPLETA - Propriedades Text e Value comentadas.
        ///    Necessita definição de como exibir o vínculo na lista.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Lista de vínculos operador-contrato
        /// </para>
        /// </summary>
        /// <returns>Lista de SelectListItem com vínculos operador-contrato</returns>
        public IEnumerable<SelectListItem> GetOperadorContratoListForDropDown()
            {
            return _db.OperadorContrato.Select(i => new SelectListItem()
                {
                //Text = i.Placa,
                //Value = i.VeiculoId.ToString()
                }); ;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Update                                                                      │
        /// │ 🔗 RASTREABILIDADE:                                                                    │
        /// │    ⬅️ CHAMADO POR : Controllers de OperadorContrato, UnitOfWork                         │
        /// │    ➡️ CHAMA       : DbContext.Update(), DbContext.SaveChanges()                         │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Atualiza vínculo entre operador e contrato usando chave composta.
        ///    Busca registro existente por OperadorId e ContratoId.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    operadorContrato - Entidade OperadorContrato com dados atualizados
        /// </para>
        ///
        /// <para>
        /// ⚠️ <b>CHAVE COMPOSTA:</b><br/>
        ///    Busca utiliza (OperadorId + ContratoId) para identificação única do registro.
        /// </para>
        /// </summary>
        /// <param name="operadorContrato">Entidade OperadorContrato com dados a serem persistidos</param>
        public new void Update(OperadorContrato operadorContrato)
            {
            var objFromDb = _db.OperadorContrato.FirstOrDefault(s => (s.OperadorId == operadorContrato.OperadorId) && (s.ContratoId == operadorContrato.ContratoId));

            _db.Update(operadorContrato);
            _db.SaveChanges();

            }


        }
    }


