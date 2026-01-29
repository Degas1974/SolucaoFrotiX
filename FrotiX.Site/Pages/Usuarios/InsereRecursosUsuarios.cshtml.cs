// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ ARQUIVO    : InsereRecursosUsuarios.cshtml.cs                                ║
// ║ LOCALIZAÇÃO: Pages/Usuarios/                                                 ║
// ║ FINALIDADE : PageModel para inserção em lote de recursos para usuários.      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO: Insere ControleAcesso para todos Recursos/Usuários ausentes.      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ LOTE: 23 — Pages/Usuarios | DATA: 29/01/2026                                 ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace FrotiX.Pages.Usuarios
{
    public class InsereRecursosUsuariosModel :PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public InsereRecursosUsuariosModel(IUnitOfWork unitOfWork , IWebHostEnvironment hostingEnvironment)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _hostingEnvironment = hostingEnvironment;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("InsereRecursosUsuarios.cshtml.cs" , "InsereRecursosUsuariosModel" , error);
            }
        }
    }
}
