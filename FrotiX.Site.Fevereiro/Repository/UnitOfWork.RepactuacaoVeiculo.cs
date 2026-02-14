/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: UnitOfWork.RepactuacaoVeiculo.cs                                                       ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Extensão parcial do UnitOfWork para o repositório de repactuação de veículos.                   ║
   ║    Centraliza o acesso ao repositório no contexto da unidade de trabalho.                          ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 PROPRIEDADES DISPONÍVEIS:                                                                       ║
   ║    • RepactuacaoVeiculo                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    Instanciação lazy do repositório com base no contexto do UnitOfWork.                             ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/
using FrotiX.Data;
using FrotiX.Repository.IRepository;

namespace FrotiX.Repository
{
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: UnitOfWork (parcial)                                                              │
    // │ 📦 HERDA DE: IUnitOfWork                                                                     │
    // │ 🔌 IMPLEMENTA: Repositórios de repactuação de veículos                                       │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Parte parcial do UnitOfWork dedicada às repactuações de veículos.
    
    public partial class UnitOfWork
    {
        private IRepactuacaoVeiculoRepository _repactuacaoVeiculo;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ PROPRIEDADE: RepactuacaoVeiculo                                                       │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : RepactuacaoVeiculoRepository                                           │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Fornecer acesso ao repositório de repactuação de veículos com inicialização lazy.
        
        
        public IRepactuacaoVeiculoRepository RepactuacaoVeiculo
        {
            get
            {
                if (_repactuacaoVeiculo == null)
                {
                    _repactuacaoVeiculo = new RepactuacaoVeiculoRepository(_db);
                }
                return _repactuacaoVeiculo;
            }
        }
    }
}
