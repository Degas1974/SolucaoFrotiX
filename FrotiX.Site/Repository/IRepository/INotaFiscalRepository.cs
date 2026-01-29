// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : INotaFiscalRepository.cs                                        ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de NotaFiscal, gerenciando notas fiscais de         ║
// ║ serviços prestados à frota (manutenção, abastecimento, lavagem).             ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetNotaFiscalListForDropDown() → DropDown de notas fiscais                 ║
// ║ • Update() → Atualização de nota fiscal                                      ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository.IRepository
    {
    /// <summary>
    /// Interface do repositório de NotaFiscal. Estende IRepository&lt;NotaFiscal&gt;.
    /// </summary>
    public interface INotaFiscalRepository : IRepository<NotaFiscal>
        {

        IEnumerable<SelectListItem> GetNotaFiscalListForDropDown();

        void Update(NotaFiscal notaFiscal);

        }
    }


