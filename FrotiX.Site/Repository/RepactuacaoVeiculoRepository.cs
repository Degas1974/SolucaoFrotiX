/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: RepactuacaoVeiculoRepository.cs                                                        ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para repactuações de valores de locação de veículos.                                ║
   ║    Gerencia reajustes individuais por veículo dentro do contrato de locação.                      ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • RepactuacaoVeiculoRepository(FrotiXDbContext db)                                               ║
   ║    • GetRepactuacaoVeiculoListForDropDown()                                                        ║
   ║    • Update(RepactuacaoVeiculo repactuacaoVeiculo)                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    A atualização atribui Valor, Observacao e vínculos do contrato/veículo.                         ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/
using System;
using System.Collections.Generic;
using System.Linq;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository
{
    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    /// │ 🎯 CLASSE: RepactuacaoVeiculoRepository                                                        │
    /// │ 📦 HERDA DE: Repository<RepactuacaoVeiculo>                                                    │
    /// │ 🔌 IMPLEMENTA: IRepactuacaoVeiculoRepository                                                   │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório responsável pelas repactuações de veículos em contratos de locação.
    /// Disponibiliza listagens e atualização de valores e observações.
    /// </summary>
    public class RepactuacaoVeiculoRepository : Repository<RepactuacaoVeiculo>, IRepactuacaoVeiculoRepository
    {
        private new readonly FrotiXDbContext _db;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: RepactuacaoVeiculoRepository                                                   │
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
        public RepactuacaoVeiculoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetRepactuacaoVeiculoListForDropDown                                           │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        /// │    ➡️ CHAMA       : DbContext.RepactuacaoVeiculo, OrderBy, Select                         │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Obter lista de repactuações de veículos para composição de dropdowns.
        ///    Ordena pela chave da repactuação e apresenta o valor.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        /// </para>
        /// </summary>
        /// <returns>Lista de itens de seleção para repactuações de veículos.</returns>
        public IEnumerable<SelectListItem> GetRepactuacaoVeiculoListForDropDown()
        {
            return _db.RepactuacaoVeiculo
                .OrderBy(o => o.RepactuacaoVeiculoId)
                .Select(i => new SelectListItem()
                {
                    Text = i.Valor.ToString(),
                    Value = i.RepactuacaoVeiculoId.ToString()
                });
        }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Update                                                                        │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        /// │    ➡️ CHAMA       : DbContext.RepactuacaoVeiculo.FirstOrDefault, _db.SaveChanges          │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Atualizar os dados de uma repactuação de veículo no banco de dados.
        ///    Sincroniza valor, observação e vínculos com contrato e veículo.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    repactuacaoVeiculo - Entidade contendo os dados atualizados.
        /// </para>
        /// </summary>
        /// <param name="repactuacaoVeiculo">Entidade <see cref="RepactuacaoVeiculo"/> com dados atualizados.</param>
        public new void Update(RepactuacaoVeiculo repactuacaoVeiculo)
        {
            var objFromDb = _db.RepactuacaoVeiculo.FirstOrDefault(s =>
                s.RepactuacaoVeiculoId == repactuacaoVeiculo.RepactuacaoVeiculoId
            );

            if (objFromDb != null)
            {
                objFromDb.Valor = repactuacaoVeiculo.Valor;
                objFromDb.Observacao = repactuacaoVeiculo.Observacao;
                objFromDb.VeiculoId = repactuacaoVeiculo.VeiculoId;
                objFromDb.RepactuacaoContratoId = repactuacaoVeiculo.RepactuacaoContratoId;
            }

            _db.SaveChanges();
        }
    }
}
