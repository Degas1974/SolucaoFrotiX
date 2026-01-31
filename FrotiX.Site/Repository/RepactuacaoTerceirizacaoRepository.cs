/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: RepactuacaoTerceirizacaoRepository.cs                                                  ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para repactuações de valores de terceirização.                                      ║
   ║    Gerencia reajustes de motoristas, operadores e encarregados em contratos.                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • RepactuacaoTerceirizacaoRepository(FrotiXDbContext db)                                         ║
   ║    • GetRepactuacaoTerceirizacaoListForDropDown()                                                  ║
   ║    • Update(RepactuacaoTerceirizacao repactuacaoTerceirizacao)                                     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    A listagem usa ValorEncarregado como texto e RepactuacaoContratoId como identificador.          ║
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
    // │ 🎯 CLASSE: RepactuacaoTerceirizacaoRepository                                                  │
    // │ 📦 HERDA DE: Repository                                              │
    // │ 🔌 IMPLEMENTA: IRepactuacaoTerceirizacaoRepository                                             │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pelas repactuações de terceirização.
    // Centraliza listagens para UI e atualização de registros.
    
    public class RepactuacaoTerceirizacaoRepository : Repository<RepactuacaoTerceirizacao>, IRepactuacaoTerceirizacaoRepository
        {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: RepactuacaoTerceirizacaoRepository                                             │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public RepactuacaoTerceirizacaoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetRepactuacaoTerceirizacaoListForDropDown                                     │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        // │    ➡️ CHAMA       : DbContext.RepactuacaoTerceirizacao, Select                            │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista de repactuações de terceirização para composição de dropdowns.
        // Exibe o valor do encarregado e usa o vínculo do contrato como chave.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        
        
        // Returns: Lista de itens de seleção para repactuações de terceirização.
        public IEnumerable<SelectListItem> GetRepactuacaoTerceirizacaoListForDropDown()
            {
            return _db.RepactuacaoTerceirizacao
                .Select(i => new SelectListItem()
                    {
                    Text = i.ValorEncarregado.ToString(),
                    Value = i.RepactuacaoContratoId.ToString()
                    });
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        // │    ➡️ CHAMA       : DbContext.RepactuacaoTerceirizacao.FirstOrDefault, _db.Update,        │
        // │                     _db.SaveChanges                                                     │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualizar os dados de uma repactuação de terceirização no banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // repactuacaoTerceirizacao - Entidade contendo os dados atualizados.
        
        
        // Param repactuacaoTerceirizacao: Entidade <see cref="RepactuacaoTerceirizacao"/> com dados atualizados.
        public new void Update(RepactuacaoTerceirizacao repactuacaoTerceirizacao)
            {
            var objFromDb = _db.RepactuacaoTerceirizacao.FirstOrDefault(s => s.RepactuacaoTerceirizacaoId == repactuacaoTerceirizacao.RepactuacaoTerceirizacaoId);

            _db.Update(repactuacaoTerceirizacao);
            _db.SaveChanges();

            }


        }
    }
