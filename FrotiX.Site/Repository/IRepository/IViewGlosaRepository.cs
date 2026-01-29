// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IViewGlosaRepository.cs                                         ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de ViewGlosa, consultando SQL View de glosas        ║
// ║ (descontos) aplicadas em contratos de terceirização.                         ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ OBSERVAÇÃO: Interface vazia, herda apenas métodos do IRepository genérico.   ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using FrotiX.Models; // ViewGlosaModel
using FrotiX.Repository.IRepository;

namespace FrotiX.Repository.IRepository
    {
    /// <summary>
    /// Repositório para a projeção ViewGlosa (view/keyless).
    /// Mantém apenas as operações genéricas herdadas.
    /// </summary>
    public interface IViewGlosaRepository : IRepository<ViewGlosa>
        {
        // Se precisar, adicione métodos específicos de leitura/consulta aqui.
        }
    }


