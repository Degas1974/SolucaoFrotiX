/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
║ 🚀 ARQUIVO: IUnitOfWork.RepactuacaoVeiculo.cs                                                     ║
║ 📂 CAMINHO: /Repository/IRepository                                                                ║
╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
║    Parcial do UnitOfWork que expõe o repositório de repactuação de veículos.                       ║
╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
║ 📋 ÍNDICE DE MÉTODOS (Entradas -> Saídas):                                                         ║
║ 1. [RepactuacaoVeiculo] : Repositório de repactuação.. () -> IRepactuacaoVeiculoRepository         ║
╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
║ ⚠️ MANUTENÇÃO:                                                                                     ║
║    Qualquer alteração neste código exige atualização imediata deste Card e do Header do Método.   ║
╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/
namespace FrotiX.Repository.IRepository
{
    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
    /// │ ⚡ INTERFACE: IUnitOfWork (RepactuacaoVeiculo)                                          │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
    /// │    Parcial do UnitOfWork responsável por expor o repositório de repactuação de         │
    /// │    veículos, centralizando o acesso às operações desse domínio.                        │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🔗 RASTREABILIDADE:                                                                   │
    /// │    ⬅️ CHAMADO POR : Controllers e Services de RepactuacaoVeiculo                       │
    /// │    ➡️ CHAMA       : Repositório de RepactuacaoVeiculo                                  │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    public partial interface IUnitOfWork
    {
        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ FUNCIONALIDADE: RepactuacaoVeiculo                                                 │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
        /// │    Fornece acesso ao repositório de repactuação de veículos, utilizado para           │
        /// │    consultas e atualizações de reajustes contratuais por veículo.                     │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 📥 INPUTS (Entradas):                                                                 │
        /// │    • Nenhum parâmetro                                                                 │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 📤 OUTPUTS (Saídas):                                                                  │
        /// │    • [IRepactuacaoVeiculoRepository]: Instância do repositório                         │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🔗 RASTREABILIDADE:                                                                   │
        /// │    ⬅️ CHAMADO POR : Controllers e Services de RepactuacaoVeiculo                      │
        /// │    ➡️ CHAMA       : DbContext, LINQ queries                                           │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        /// </summary>
        IRepactuacaoVeiculoRepository RepactuacaoVeiculo { get; }
    }
}
