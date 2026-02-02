/* ****************************************************************************************
 * ⚡ ARQUIVO: NavigationViewComponent.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : ViewComponent de navegação (menu lateral) com fallback inteligente
 *                   (tenta BD primeiro, fallback para nav.json se falhar).
 *
 * 📥 ENTRADAS     : HttpContext.User (Claims do usuário autenticado).
 *
 * 📤 SAÍDAS       : IViewComponentResult com View("TreeView") ou View(items).
 *
 * 🔗 CHAMADA POR  : Layout principal (_Layout.cshtml) via @await Component.InvokeAsync().
 *
 * 🔄 CHAMA        : IUnitOfWork.Recurso.GetAll(), IUnitOfWork.ControleAcesso.GetAll(),
 *                   INavigationModel.Full, MontarArvoreRecursiva().
 *
 * 📦 DEPENDÊNCIAS : INavigationModel, IUnitOfWork, RecursoTreeDTO, ASP.NET Core MVC.
 *
 * 📝 OBSERVAÇÕES  : 1) Prioridade: Banco de dados (hierarquia Recurso)
 *                   2) Fallback: nav.json (INavigationModel.Full)
 *                   3) Filtra por ControleAcesso.Acesso = true para usuário logado
 *                   4) Inclui pais necessários para manter hierarquia visual
 *                   5) Se usuário sem acessos configurados, concede acesso total (admin temp)
 **************************************************************************************** */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace FrotiX.ViewComponents
{
    /****************************************************************************************
     * ⚡ CLASSE: NavigationViewComponent
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Renderizar menu de navegação lateral com base em permissões do usuário
     *
     * 📥 ENTRADAS     : HttpContext.User.Claims (NameIdentifier)
     *
     * 📤 SAÍDAS       : View("TreeView", arvore) ou View(items)
     *
     * 🔗 CHAMADA POR  : _Layout.cshtml
     *
     * 🔄 CHAMA        : GetTreeFromDatabase(), MontarArvoreRecursiva()
     *
     * 📦 DEPENDÊNCIAS : INavigationModel, IUnitOfWork
     *
     * 📝 OBSERVAÇÕES  : Usa arquitetura de fallback: BD → JSON → Vazio
     ****************************************************************************************/
    public class NavigationViewComponent : ViewComponent
    {
        private readonly INavigationModel _navigationModel;
        private readonly IUnitOfWork _unitOfWork;

        /****************************************************************************************
         * ⚡ CONSTRUTOR: NavigationViewComponent
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Inicializar dependências via injeção (NavigationModel e UnitOfWork)
         *
         * 📥 ENTRADAS     : [INavigationModel] navigationModel - Acesso ao nav.json
         *                   [IUnitOfWork] unitOfWork - Acesso ao banco (Recurso/ControleAcesso)
         *
         * 📤 SAÍDAS       : Instância configurada do ViewComponent
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI Container
         *
         * 🔄 CHAMA        : Nenhum
         *
         * 📦 DEPENDÊNCIAS : INavigationModel, IUnitOfWork
         ****************************************************************************************/
        public NavigationViewComponent(INavigationModel navigationModel, IUnitOfWork unitOfWork)
        {
            _navigationModel = navigationModel;
            _unitOfWork = unitOfWork;
        }

        /****************************************************************************************
         * ⚡ MÉTODO: Invoke
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Renderizar menu de navegação com fallback inteligente (BD → JSON)
         *
         * 📥 ENTRADAS     : Nenhuma (usa HttpContext.User interno)
         *
         * 📤 SAÍDAS       : [IViewComponentResult] View("TreeView", arvore) ou View(items)
         *
         * 🔗 CHAMADA POR  : ASP.NET Core quando ViewComponent é invocado no layout
         *
         * 🔄 CHAMA        : GetTreeFromDatabase(), _navigationModel.Full
         *
         * 📦 DEPENDÊNCIAS : HttpContext.User, INavigationModel.Full
         *
         * 📝 OBSERVAÇÕES  : Try-catch garante que erros no BD não quebram a navegação
         *                   (sempre tem fallback para JSON). Logs via Console.WriteLine.
         ****************************************************************************************/
        public IViewComponentResult Invoke()
        {
            try
            {
                // [DOC] Tenta ler do banco de dados primeiro (prioridade)
                var arvoreDb = GetTreeFromDatabase();

                if (arvoreDb != null && arvoreDb.Any())
                {
                    Console.WriteLine($"NavigationViewComponent: Usando TreeView com {arvoreDb.Count} itens raiz do banco de dados");
                    // [DOC] Usa Syncfusion TreeView com dados do BD
                    return View("TreeView", arvoreDb);
                }
                else
                {
                    Console.WriteLine("NavigationViewComponent: Nenhum dado retornado do banco, usando fallback JSON");
                }
            }
            catch (Exception ex)
            {
                // [DOC] Log do erro, mas continua com fallback (não quebra a navegação)
                Console.WriteLine($"NavigationViewComponent: Erro ao ler navegação do BD: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"NavigationViewComponent: Inner: {ex.InnerException.Message}");
                }
            }

            // [DOC] Fallback: usa nav.json quando BD falha ou está vazio
            Console.WriteLine("NavigationViewComponent: Usando navegação do JSON");
            var items = _navigationModel.Full;
            return View(items);
        }

        /****************************************************************************************
         * ⚡ MÉTODO: GetTreeFromDatabase
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Carregar árvore de navegação do banco com filtro de permissões
         *
         * 📥 ENTRADAS     : HttpContext.User (NameIdentifier Claim)
         *
         * 📤 SAÍDAS       : [List<RecursoTreeDTO>] Árvore hierárquica ou null se falhar
         *
         * 🔗 CHAMADA POR  : Invoke()
         *
         * 🔄 CHAMA        : _unitOfWork.Recurso.GetAll(), _unitOfWork.ControleAcesso.GetAll(),
         *                   MontarArvoreRecursiva()
         *
         * 📦 DEPENDÊNCIAS : IUnitOfWork, RecursoTreeDTO, HttpContext.User
         *
         * 📝 OBSERVAÇÕES  : 1) Retorna null se usuário não autenticado
         *                   2) Se usuário sem acessos configurados, concede acesso total (admin temp)
         *                   3) Inclui pais necessários para manter hierarquia visual
         *                   4) Filtra por Ativo = true e ordena por Ordem
         ****************************************************************************************/
        /// <summary>
        /// Lê a árvore de navegação do banco de dados
        /// </summary>
        private List<RecursoTreeDTO> GetTreeFromDatabase()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Console.WriteLine($"NavigationViewComponent: UserId = {userId ?? "NULL"}");

            // [DOC] Retorna null se usuário não autenticado (fallback para JSON)
            if (string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("NavigationViewComponent: UserId não encontrado, usando JSON");
                return null;
            }

            // [DOC] Busca todos os recursos do banco
            var todoRecursos = _unitOfWork.Recurso.GetAll().ToList();
            Console.WriteLine($"NavigationViewComponent: Total de recursos no banco: {todoRecursos.Count}");

            // [DOC] Verifica se a migração hierárquica foi feita (recursos de nível 0 com filhos)
            var temHierarquia = todoRecursos.Any(r => r.Nivel == 0) && todoRecursos.Any(r => r.ParentId != null);

            if (!temHierarquia && todoRecursos.Count == 0)
            {
                Console.WriteLine("NavigationViewComponent: Sem recursos no banco, usando JSON");
                return null;
            }

            // [DOC] Busca recursos ativos ordenados
            var recursosAtivos = todoRecursos
                .Where(r => r.Ativo)
                .OrderBy(r => r.Ordem)
                .ToList();

            Console.WriteLine($"NavigationViewComponent: Recursos ativos: {recursosAtivos.Count}");

            // [DOC] Busca controle de acesso do usuário (Acesso = true)
            var controlesAcesso = _unitOfWork.ControleAcesso
                .GetAll(ca => ca.UsuarioId == userId && ca.Acesso == true)
                .Select(ca => ca.RecursoId)
                .ToHashSet();

            Console.WriteLine($"NavigationViewComponent: Controles de acesso do usuário: {controlesAcesso.Count}");

            // [DOC] Se usuário não tem nenhum acesso configurado, dá acesso a tudo (admin temporário)
            HashSet<Guid> idsComAcessoDireto;
            if (controlesAcesso.Count == 0)
            {
                Console.WriteLine("NavigationViewComponent: Usuário sem acessos configurados - concedendo acesso total temporário");
                idsComAcessoDireto = recursosAtivos.Select(r => r.RecursoId).ToHashSet();
            }
            else
            {
                idsComAcessoDireto = recursosAtivos
                    .Where(r => controlesAcesso.Contains(r.RecursoId))
                    .Select(r => r.RecursoId)
                    .ToHashSet();
            }

            // [DOC] Inclui pais de recursos com acesso (para manter hierarquia visual)
            var idsComAcessoEPais = new HashSet<Guid>(idsComAcessoDireto);
            foreach (var recurso in recursosAtivos)
            {
                if (idsComAcessoDireto.Contains(recurso.RecursoId) && recurso.ParentId.HasValue)
                {
                    // [DOC] Adiciona todos os ancestrais (para evitar itens órfãos na árvore)
                    var parentId = recurso.ParentId;
                    while (parentId.HasValue)
                    {
                        idsComAcessoEPais.Add(parentId.Value);
                        var parent = recursosAtivos.FirstOrDefault(r => r.RecursoId == parentId);
                        parentId = parent?.ParentId;
                    }
                }
            }

            // [DOC] Filtra recursos com acesso ou que são pais necessários
            var recursosComAcesso = recursosAtivos
                .Where(r => idsComAcessoEPais.Contains(r.RecursoId))
                .ToList();

            Console.WriteLine($"NavigationViewComponent: {recursosComAcesso.Count} recursos finais para exibir");

            if (recursosComAcesso.Count == 0)
            {
                Console.WriteLine("NavigationViewComponent: Nenhum recurso para exibir, usando JSON");
                return null;
            }

            // [DOC] Monta árvore hierárquica recursiva
            return MontarArvoreRecursiva(recursosComAcesso, null);
        }

        /****************************************************************************************
         * ⚡ MÉTODO: MontarArvoreRecursiva
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Construir árvore hierárquica recursiva de recursos (Parent-Child)
         *
         * 📥 ENTRADAS     : [List<Recurso>] recursos - Lista flat de recursos
         *                   [Guid?] parentId - ID do pai (null para raiz)
         *
         * 📤 SAÍDAS       : [List<RecursoTreeDTO>] Árvore hierárquica de DTOs
         *
         * 🔗 CHAMADA POR  : GetTreeFromDatabase(), MontarArvoreRecursiva() (recursão)
         *
         * 🔄 CHAMA        : MontarArvoreRecursiva() (recursão para filhos)
         *
         * 📦 DEPENDÊNCIAS : RecursoTreeDTO, LINQ
         *
         * 📝 OBSERVAÇÕES  : Recursão termina quando não há mais filhos.
         *                   Ordena por Ordem antes de retornar.
         *                   Expanded = true para expandir árvore por padrão.
         ****************************************************************************************/
        /// <summary>
        /// Monta a árvore hierárquica de recursos
        /// </summary>
        private List<RecursoTreeDTO> MontarArvoreRecursiva(List<Recurso> recursos, Guid? parentId)
        {
            return recursos
                .Where(r => r.ParentId == parentId)
                .OrderBy(r => r.Ordem)
                .Select(r =>
                {
                    var filhos = MontarArvoreRecursiva(recursos, r.RecursoId);
                    return new RecursoTreeDTO
                    {
                        Id = r.RecursoId.ToString(),
                        Text = r.Nome,
                        NomeMenu = r.NomeMenu,
                        Icon = r.Icon,
                        IconCss = r.Icon,
                        Href = r.Href,
                        ParentId = r.ParentId?.ToString(),
                        Ordem = r.Ordem,
                        Nivel = r.Nivel,
                        Ativo = r.Ativo,
                        HasChild = filhos.Any(),
                        Expanded = true,
                        Items = filhos
                    };
                })
                .ToList();
        }
    }
}


