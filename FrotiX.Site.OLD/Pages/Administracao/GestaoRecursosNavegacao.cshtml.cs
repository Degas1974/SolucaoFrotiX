// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: GestaoRecursosNavegacao.cshtml.cs                                  ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ PageModel para gestão de recursos de navegação (menu do sistema).           ║
// ║ Permite CRUD dos itens de menu e estrutura hierárquica.                     ║
// ║                                                                              ║
// ║ CARACTERÍSTICAS:                                                              ║
// ║ • Página simples - dados carregados via AJAX                                ║
// ║ • Gerencia entidade Recurso com estrutura pai-filho                         ║
// ║ • Define ícones, URLs e ordenação dos menus                                 ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 19                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrotiX.Pages.Administracao
{
    public class GestaoRecursosNavegacaoModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
