/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: RegistroCupomAbastecimentoRepository.cs                                                ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para registros de cupons de abastecimento digitalizados.                            ║
   ║    Armazena referências a arquivos PDF para auditoria e consulta.                                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • RegistroCupomAbastecimentoRepository(FrotiXDbContext db)                                       ║
   ║    • GetRegistroCupomAbastecimentoListForDropDown()                                                 ║
   ║    • Update(RegistroCupomAbastecimento registroCupomAbastecimento)                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    A listagem é ordenada por DataRegistro e exibe o campo RegistroPDF.                              ║
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
    // │ 🎯 CLASSE: RegistroCupomAbastecimentoRepository                                                │
    // │ 📦 HERDA DE: Repository                                            │
    // │ 🔌 IMPLEMENTA: IRegistroCupomAbastecimentoRepository                                           │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pelos registros de cupons de abastecimento digitalizados.
    // Mantém consultas para dropdowns e atualização de arquivos associados.
    
    public class RegistroCupomAbastecimentoRepository : Repository<RegistroCupomAbastecimento>, IRegistroCupomAbastecimentoRepository
        {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: RegistroCupomAbastecimentoRepository                                           │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public RegistroCupomAbastecimentoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetRegistroCupomAbastecimentoListForDropDown                                   │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        // │    ➡️ CHAMA       : DbContext.RegistroCupomAbastecimento, OrderBy, Select                │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista de registros de cupons para composição de dropdowns.
        // Ordena por data do registro e exibe o identificador do arquivo PDF.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        
        
        // Returns: Lista de itens de seleção para registros de cupons.
        public IEnumerable<SelectListItem> GetRegistroCupomAbastecimentoListForDropDown()
            {
            return _db.RegistroCupomAbastecimento
                .OrderBy(o => o.DataRegistro)
                .Select(i => new SelectListItem()
                    {
                    Text = i.RegistroPDF,
                    Value = i.RegistroCupomId.ToString()
                    });
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        // │    ➡️ CHAMA       : DbContext.RegistroCupomAbastecimento.FirstOrDefault, _db.Update,     │
        // │                     _db.SaveChanges                                                     │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualizar os dados de um registro de cupom digitalizado no banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // registroCupomAbastecimento - Entidade contendo os dados atualizados.
        
        
        // Param registroCupomAbastecimento: Entidade <see cref="RegistroCupomAbastecimento"/> com dados atualizados.
        public new void Update(RegistroCupomAbastecimento registroCupomAbastecimento)
            {
            var objFromDb = _db.RegistroCupomAbastecimento.FirstOrDefault(s => s.RegistroCupomId == registroCupomAbastecimento.RegistroCupomId);

            _db.Update(registroCupomAbastecimento);
            _db.SaveChanges();

            }


        }
    }
