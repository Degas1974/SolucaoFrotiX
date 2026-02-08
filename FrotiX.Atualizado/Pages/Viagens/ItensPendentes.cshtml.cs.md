# Pages/Viagens/ItensPendentes.cshtml.cs

**ARQUIVO NOVO** | 33 linhas de codigo

> Copiar integralmente para o Janeiro.

---

```csharp
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Pages.Viagens
{
    public class ItensPendentesModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public ItensPendentesModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<Viagem> ViagensComItensPendentes { get;set; }

        public void OnGet()
        {
            ViagensComItensPendentes = _unitOfWork.Viagem.GetAll(
                v => v.Status == "Realizada" &&
                ((v.DocumentoEntregue == true && v.DocumentoDevolvido == false) ||
                (v.CartaoAbastecimentoEntregue == true && v.CartaoAbastecimentoDevolvido == false) ||
                (v.CintaEntregue == true && v.CintaDevolvida == false) ||
                (v.TabletEntregue == true && v.TabletDevolvido == false) ||
                (v.ArlaEntregue == true && v.ArlaDevolvido == false) ||
                (v.CaboEntregue == true && v.CaboDevolvido == false)),
                includeProperties: "Motorista,Veiculo"
            ).ToList();
        }
    }
}
```
