/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: MotoristaContratoRepository.cs                                                         ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para vínculo N:N entre motoristas e contratos terceirizados.                       ║
   ║    Gerencia associações de motoristas habilitados a trabalhar em contratos de transporte.         ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • MotoristaContratoRepository(FrotiXDbContext db)                                               ║
   ║    • IEnumerable<SelectListItem> GetMotoristaContratoListForDropDown()                            ║
   ║    • void Update(MotoristaContrato motoristaContrato)                                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    Tabela associativa com chave composta (MotoristaId, ContratoId).                               ║
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
    // │ 🎯 CLASSE: MotoristaContratoRepository                                                        │
    // │ 📦 HERDA DE: Repository&lt;MotoristaContrato&gt;                                                      │
    // │ 🔌 IMPLEMENTA: IMotoristaContratoRepository                                                   │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório para relacionamento muitos-para-muitos entre Motorista e Contrato.
    // Controla quais motoristas estão autorizados a trabalhar em cada contrato terceirizado.
    
    public class MotoristaContratoRepository : Repository<MotoristaContrato>, IMotoristaContratoRepository
        {
        private new readonly FrotiXDbContext _db;

        public MotoristaContratoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetMotoristaContratoListForDropDown                                         │
        // │ 🔗 RASTREABILIDADE:                                                                    │
        // │    ⬅️ CHAMADO POR : Controllers de gerenciamento de vínculos motorista-contrato        │
        // │    ➡️ CHAMA       : DbContext.MotoristaContrato, Linq Select                            │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retorna lista de vínculos motorista-contrato para uso em DropDown.
        // Implementação pendente - retorna estrutura vazia.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Lista de vínculos (implementação incompleta)
        
        
        // Returns: Lista de SelectListItem com vínculos motorista-contrato
        public IEnumerable<SelectListItem> GetMotoristaContratoListForDropDown()
            {
            return _db.MotoristaContrato.Select(i => new SelectListItem()
                {
                //Text = i.Placa,
                //Value = i.VeiculoId.ToString()
                }); ;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                      │
        // │ 🔗 RASTREABILIDADE:                                                                    │
        // │    ⬅️ CHAMADO POR : Controllers de MotoristaContrato, UnitOfWork                        │
        // │    ➡️ CHAMA       : DbContext.Update(), DbContext.SaveChanges()                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualiza registro de vínculo entre motorista e contrato.
        // Utiliza chave composta (MotoristaId + ContratoId) para localizar registro.
        
        
        
        // 📥 PARÂMETROS:
        // motoristaContrato - Entidade com chave composta e dados atualizados
        
        
        // Param motoristaContrato: Entidade MotoristaContrato com dados a serem persistidos
        public new void Update(MotoristaContrato motoristaContrato)
            {
            var objFromDb = _db.MotoristaContrato.FirstOrDefault(s => (s.MotoristaId == motoristaContrato.MotoristaId) && (s.ContratoId == motoristaContrato.ContratoId));

            _db.Update(motoristaContrato);
            _db.SaveChanges();

            }


        }
    }


