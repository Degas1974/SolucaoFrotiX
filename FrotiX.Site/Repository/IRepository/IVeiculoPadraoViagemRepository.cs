// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IVeiculoPadraoViagemRepository.cs                               ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de VeiculoPadraoViagem, gerenciando associações     ║
// ║ padrão entre requisitantes/setores e veículos preferenciais.                 ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • Update() → Atualização de veículo padrão                                   ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Models;

namespace FrotiX.Repository.IRepository
{
    /// <summary>
    /// Interface do repositório de VeiculoPadraoViagem. Estende IRepository&lt;VeiculoPadraoViagem&gt;.
    /// </summary>
    public interface IVeiculoPadraoViagemRepository : IRepository<VeiculoPadraoViagem>
    {
        void Update(VeiculoPadraoViagem veiculoPadraoViagem);
    }
}
