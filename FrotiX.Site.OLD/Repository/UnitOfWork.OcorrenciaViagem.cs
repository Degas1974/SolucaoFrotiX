/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: UnitOfWork.OcorrenciaViagem.cs                                                         ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Extensão parcial do UnitOfWork para repositórios de ocorrências de viagem.                      ║
   ║    Centraliza acesso às views de ocorrências e ocorrências abertas por veículo.                    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 PROPRIEDADES DISPONÍVEIS:                                                                       ║
   ║    • OcorrenciaViagem                                                                             ║
   ║    • ViewOcorrenciasViagem                                                                        ║
   ║    • ViewOcorrenciasAbertasVeiculo                                                                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    Instanciação lazy dos repositórios com base no contexto do UnitOfWork.                          ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/
using FrotiX.Data;
using FrotiX.Repository.IRepository;

namespace FrotiX.Repository
{
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: UnitOfWork (parcial)                                                              │
    // │ 📦 HERDA DE: IUnitOfWork                                                                     │
    // │ 🔌 IMPLEMENTA: Repositórios de ocorrências                                                   │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Parte parcial do UnitOfWork dedicada às ocorrências de viagem.
    
    public partial class UnitOfWork
    {
        private IOcorrenciaViagemRepository? _ocorrenciaViagem;
        private IViewOcorrenciasViagemRepository? _viewOcorrenciasViagem;
        private IViewOcorrenciasAbertasVeiculoRepository? _viewOcorrenciasAbertasVeiculo;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ PROPRIEDADE: OcorrenciaViagem                                                         │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : OcorrenciaViagemRepository                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Fornecer acesso ao repositório de ocorrências de viagem com inicialização lazy.
        
        
        public IOcorrenciaViagemRepository OcorrenciaViagem
        {
            get
            {
                if (_ocorrenciaViagem == null)
                    _ocorrenciaViagem = new OcorrenciaViagemRepository(_db);
                return _ocorrenciaViagem;
            }
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ PROPRIEDADE: ViewOcorrenciasViagem                                                    │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : ViewOcorrenciasViagemRepository                                        │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Fornecer acesso ao repositório da view de ocorrências de viagem.
        
        
        public IViewOcorrenciasViagemRepository ViewOcorrenciasViagem
        {
            get
            {
                if (_viewOcorrenciasViagem == null)
                    _viewOcorrenciasViagem = new ViewOcorrenciasViagemRepository(_db);
                return _viewOcorrenciasViagem;
            }
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ PROPRIEDADE: ViewOcorrenciasAbertasVeiculo                                            │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : ViewOcorrenciasAbertasVeiculoRepository                               │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Fornecer acesso ao repositório da view de ocorrências abertas por veículo.
        
        
        public IViewOcorrenciasAbertasVeiculoRepository ViewOcorrenciasAbertasVeiculo
        {
            get
            {
                if (_viewOcorrenciasAbertasVeiculo == null)
                    _viewOcorrenciasAbertasVeiculo = new ViewOcorrenciasAbertasVeiculoRepository(_db);
                return _viewOcorrenciasAbertasVeiculo;
            }
        }
    }
}
