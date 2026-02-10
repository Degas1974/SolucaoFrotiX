/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: LavadorContratoRepository.cs                                                           ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para vínculo N:N entre lavadores e contratos terceirizados.                        ║
   ║    Gerencia associações de lavadores habilitados a trabalhar em contratos de lavagem.             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • LavadorContratoRepository(FrotiXDbContext db)                                                 ║
   ║    • IEnumerable<SelectListItem> GetLavadorContratoListForDropDown()                              ║
   ║    • void Update(LavadorContrato lavadorContrato)                                                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    Tabela associativa com chave composta (LavadorId, ContratoId).                                 ║
   ║    Update utiliza busca por chave composta para identificar registro existente.                   ║
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
    // │ 🎯 CLASSE: LavadorContratoRepository                                                          │
    // │ 📦 HERDA DE: Repository&lt;LavadorContrato&gt;                                                        │
    // │ 🔌 IMPLEMENTA: ILavadorContratoRepository                                                     │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório para relacionamento muitos-para-muitos entre Lavador e Contrato.
    // Controla quais lavadores estão autorizados a trabalhar em cada contrato terceirizado.
    
    public class LavadorContratoRepository : Repository<LavadorContrato>, ILavadorContratoRepository
        {
        private new readonly FrotiXDbContext _db;

        public LavadorContratoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetLavadorContratoListForDropDown                                           │
        // │ 🔗 RASTREABILIDADE:                                                                    │
        // │    ⬅️ CHAMADO POR : Controllers de gerenciamento de vínculos lavador-contrato          │
        // │    ➡️ CHAMA       : DbContext.LavadorContrato, Linq Select                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retorna lista de vínculos lavador-contrato para uso em DropDown.
        // Implementação pendente - retorna estrutura vazia.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Lista de vínculos (implementação incompleta)
        
        
        // Returns: Lista de SelectListItem com vínculos lavador-contrato
        public IEnumerable<SelectListItem> GetLavadorContratoListForDropDown()
            {
            return _db.LavadorContrato.Select(i => new SelectListItem()
                {
                //Text = i.Placa,
                //Value = i.VeiculoId.ToString()
                }); ;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                      │
        // │ 🔗 RASTREABILIDADE:                                                                    │
        // │    ⬅️ CHAMADO POR : Controllers de LavadorContrato, UnitOfWork                          │
        // │    ➡️ CHAMA       : DbContext.Update(), DbContext.SaveChanges()                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualiza registro de vínculo entre lavador e contrato.
        // Utiliza chave composta (LavadorId + ContratoId) para localizar registro.
        
        
        
        // 📥 PARÂMETROS:
        // lavadorContrato - Entidade com chave composta e dados atualizados
        
        
        // Param lavadorContrato: Entidade LavadorContrato com dados a serem persistidos
        public new void Update(LavadorContrato lavadorContrato)
            {
            var objFromDb = _db.LavadorContrato.FirstOrDefault(s => (s.LavadorId == lavadorContrato.LavadorId) && (s.ContratoId == lavadorContrato.ContratoId));

            _db.Update(lavadorContrato);
            _db.SaveChanges();

            }


        }
    }


