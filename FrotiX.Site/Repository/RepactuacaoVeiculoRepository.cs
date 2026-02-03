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
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: RepactuacaoVeiculoRepository                                                        │
    // │ 📦 HERDA DE: Repository                                                    │
    // │ 🔌 IMPLEMENTA: IRepactuacaoVeiculoRepository                                                   │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pelas repactuações de veículos em contratos de locação.
    // Disponibiliza listagens e atualização de valores e observações.
    
    public class RepactuacaoVeiculoRepository : Repository<RepactuacaoVeiculo>, IRepactuacaoVeiculoRepository
    {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: RepactuacaoVeiculoRepository                                                   │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork [UnitOfWork.cs:165]                                        │
        // │    ➡️ CHAMA       : base(db) [linha ~62]                                                 │
        // │ 📦 DEPENDÊNCIAS  : FrotiXDbContext                                                      │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        

        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.



        // 📥 PARÂMETROS:
        // db [FrotiXDbContext] - Contexto do banco de dados da aplicação.


        // 📤 SAÍDAS: Instância inicializada do repositório

        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public RepactuacaoVeiculoRepository(FrotiXDbContext db) : base(db)
        {
            try
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
            }
            catch (Exception erro)
            {
                throw;
            }
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetRepactuacaoVeiculoListForDropDown                                           │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        // │    ➡️ CHAMA       : DbContext.RepactuacaoVeiculo, OrderBy, Select                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista de repactuações de veículos para composição de dropdowns.
        // Ordena pela chave da repactuação e apresenta o valor.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        
        
        // Returns: Lista de itens de seleção para repactuações de veículos.
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        // │    ➡️ CHAMA       : DbContext.RepactuacaoVeiculo.FirstOrDefault, _db.SaveChanges          │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualizar os dados de uma repactuação de veículo no banco de dados.
        // Sincroniza valor, observação e vínculos com contrato e veículo.
        
        
        
        // 📥 PARÂMETROS:
        // repactuacaoVeiculo - Entidade contendo os dados atualizados.
        
        
        // Param repactuacaoVeiculo: Entidade <see cref="RepactuacaoVeiculo"/> com dados atualizados.
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
