// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IFornecedorRepository.cs                                        ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de Fornecedor, definindo contrato para fornecedores ║
// ║ de serviços de frota (combustível, manutenção, lavagem, etc.).               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetFornecedorListForDropDown() → DropDown de fornecedores ativos           ║
// ║ • Update() → Atualização de fornecedor                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository.IRepository
    {
    
    // Interface do repositório de Fornecedor. Estende IRepository&lt;Fornecedor&gt;.
    
    public interface IFornecedorRepository : IRepository<Fornecedor>
        {

        IEnumerable<SelectListItem> GetFornecedorListForDropDown();

        void Update(Fornecedor fornecedor);

        }
    }


