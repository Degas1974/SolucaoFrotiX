using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using FrotiX.Helpers;

namespace FrotiX.Repository
{
    /// <summary>
    /// ╔══════════════════════════════════════════════════════════════════════════════
    /// ║ REPOSITORY: AspNetUsersRepository
    /// ║══════════════════════════════════════════════════════════════════════════════
    /// ║ DESCRIÇÃO:
    /// ║    Gerencia entidades AspNetUsers (usuários do sistema ASP.NET Core Identity).
    /// ║    Responsável por operações customizadas sobre usuários além das fornecidas
    /// ║    pelo Identity (UserManager<AspNetUsers>).
    /// ║
    /// ║ ENTIDADE PRINCIPAL:
    /// ║    - AspNetUsers (tabela do ASP.NET Core Identity estendida).
    /// ║
    /// ║ RELACIONAMENTOS:
    /// ║    - AspNetRoles (N:M via AspNetUserRoles) - Perfis/Roles.
    /// ║    - AlertasUsuario (1:N) - Alertas recebidos pelo usuário.
    /// ║    - Motorista (1:1 opcional) - Motorista vinculado ao usuário.
    /// ║    - Múltiplas entidades via campos AuditUserInsert/Update (rastreamento).
    /// ║
    /// ║ MÉTODOS PRINCIPAIS:
    /// ║    1. GetAspNetUsersListForDropDown() - Lista para dropdowns (usuários ativos).
    /// ║    2. Update() - Atualiza usuário com SaveChanges imediato.
    /// ║
    /// ║ OBSERVAÇÕES:
    /// ║    - NÃO substituir UserManager<AspNetUsers> para operações de autenticação.
    /// ║    - Update() quebra padrão Unit of Work (chama SaveChanges diretamente).
    /// ║    - Filtro Status=true (usuários ativos) é aplicado em GetList.
    /// ║
    /// ║ INTEGRAÇÃO:
    /// ║    - Identity: Usuários criados via UserManager são gerenciados aqui.
    /// ║    - AlertasHub: Notificações vinculadas a usuários.
    /// ║    - Controllers: Seleção de usuários em formulários.
    /// ║══════════════════════════════════════════════════════════════════════════════
    /// </summary>
    public class AspNetUsersRepository : Repository<AspNetUsers>, IAspNetUsersRepository
    {
        private new readonly FrotiXDbContext _db;

        public AspNetUsersRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: GetAspNetUsersListForDropDown (Lista para DropDown)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Retorna lista de usuários ATIVOS formatada para dropdown/select.
        /// │    SelectListItem: Text = NomeCompleto, Value = Id.
        /// │
        /// │ FILTRO:
        /// │    Status == true (usuários ativos/habilitados).
        /// │
        /// │ ORDENAÇÃO:
        /// │    NomeCompleto ASC (ordem alfabética).
        /// │
        /// │ USO:
        /// │    - Formulários: Seleção de usuário em Viagem, Manutenção, Alertas.
        /// │    - ViewBag.Usuarios: Popular dropdown no Razor.
        /// │    - Ex: <select asp-items="ViewBag.Usuarios"></select>
        /// │
        /// │ RETORNO:
        /// │    IEnumerable<SelectListItem> (MVC SelectListItem).
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public IEnumerable<SelectListItem> GetAspNetUsersListForDropDown()
        {
            try
            {
                // [QUERY] - Lista usuários ativos ordenados por NomeCompleto
                return _db.AspNetUsers
                    .Where(e => (bool)e.Status) // Cast para bool (Status é bool?)
                    .OrderBy(o => o.NomeCompleto)
                    .Select(i => new SelectListItem()
                    {
                        Text = i.NomeCompleto,
                        Value = i.Id.ToString()
                    });
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AspNetUsersRepository.cs", "GetAspNetUsersListForDropDown", ex);
                return new List<SelectListItem>();
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: Update (Atualizar Usuário)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Atualiza registro AspNetUsers com SaveChanges IMEDIATO.
        /// │    QUEBRA PADRÃO UNIT OF WORK (não aguarda UnitOfWork.SaveAsync()).
        /// │
        /// │ FLUXO:
        /// │    1. Busca objFromDb com AsTracking() (não usado depois).
        /// │    2. Update(aspNetUsers) marca entidade como Modified.
        /// │    3. SaveChanges() persiste imediatamente.
        /// │
        /// │ PROBLEMA:
        /// │    - Transação independente (não participa da transação do UnitOfWork).
        /// │    - Se houver erro depois, esta alteração NÃO será revertida.
        /// │    - objFromDb é buscado mas não utilizado (código morto).
        /// │
        /// │ SUGESTÃO DE REFATORAÇÃO:
        /// │    - Remover SaveChanges() daqui.
        /// │    - Deixar UnitOfWork.SaveAsync() fazer o commit transacional.
        /// │    - Remover linha 'objFromDb' (não é usada).
        /// │
        /// │ USO:
        /// │    - Controllers: Atualizar perfil de usuário.
        /// │    - Admin: Alterar Status, NomeCompleto, Email, etc.
        /// │
        /// │ ATENÇÃO:
        /// │    NÃO usar para alterar senha/email autenticação - usar UserManager.
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public new void Update(AspNetUsers aspNetUsers)
        {
            try
            {
                // [QUERY TRACKING] - Busca usuário (NÃO USADO - código morto)
                var objFromDb = _db.AspNetUsers.AsTracking().FirstOrDefault(s => s.Id == aspNetUsers.Id);

                // [UPDATE] - Marca entidade como Modified
                _db.Update(aspNetUsers);

                // [SAVE] - QUEBRA UNIT OF WORK - Persiste imediatamente
                _db.SaveChanges(); // PROBLEMA: Transação independente
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AspNetUsersRepository.cs", "Update", ex);
                throw; // Propaga exceção (update deve falhar)
            }
        }
    }
}


