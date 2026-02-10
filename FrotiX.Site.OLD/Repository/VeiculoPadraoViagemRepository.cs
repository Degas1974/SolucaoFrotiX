/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: VeiculoPadraoViagemRepository.cs                                                       ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para padrões de viagem por veículo.                                                 ║
   ║    Atualiza métricas agregadas e data de atualização do veículo.                                   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • VeiculoPadraoViagemRepository(FrotiXDbContext db)                                              ║
   ║    • Update(VeiculoPadraoViagem veiculoPadraoViagem)                                                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    A atualização define DataAtualizacao com DateTime.Now.                                          ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;

namespace FrotiX.Repository
{
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: VeiculoPadraoViagemRepository                                                      │
    // │ 📦 HERDA DE: Repository                                                  │
    // │ 🔌 IMPLEMENTA: IVeiculoPadraoViagemRepository                                                 │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pelos padrões de viagem por veículo.
    // Atualiza métricas de uso e abastecimento registradas em VeiculoPadraoViagem.
    
    public class VeiculoPadraoViagemRepository : Repository<VeiculoPadraoViagem>, IVeiculoPadraoViagemRepository
    {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: VeiculoPadraoViagemRepository                                                │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public VeiculoPadraoViagemRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext.VeiculoPadraoViagem.FirstOrDefault, _db.SaveChanges         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualizar métricas consolidadas de viagem para um veículo.
        // Define DataAtualizacao com o horário corrente.
        
        
        
        // 📥 PARÂMETROS:
        // veiculoPadraoViagem - Entidade contendo os dados atualizados.
        
        
        // Param veiculoPadraoViagem: Entidade <see cref="VeiculoPadraoViagem"/> com dados atualizados.
        public new void Update(VeiculoPadraoViagem veiculoPadraoViagem)
        {
            var objFromDb = _db.VeiculoPadraoViagem.FirstOrDefault(s => s.VeiculoId == veiculoPadraoViagem.VeiculoId);

            if (objFromDb != null)
            {
                objFromDb.TipoUso = veiculoPadraoViagem.TipoUso;
                objFromDb.TotalViagens = veiculoPadraoViagem.TotalViagens;
                objFromDb.MediaDuracaoMinutos = veiculoPadraoViagem.MediaDuracaoMinutos;
                objFromDb.MediaKmPorViagem = veiculoPadraoViagem.MediaKmPorViagem;
                objFromDb.MediaKmPorDia = veiculoPadraoViagem.MediaKmPorDia;
                objFromDb.MediaKmEntreAbastecimentos = veiculoPadraoViagem.MediaKmEntreAbastecimentos;
                objFromDb.MediaDiasEntreAbastecimentos = veiculoPadraoViagem.MediaDiasEntreAbastecimentos;
                objFromDb.TotalAbastecimentosAnalisados = veiculoPadraoViagem.TotalAbastecimentosAnalisados;
                objFromDb.DataAtualizacao = DateTime.Now;

                _db.SaveChanges();
            }
        }
    }
}
