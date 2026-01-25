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
    /// ║ REPOSITORY: ControleAcessoRepository
    /// ║══════════════════════════════════════════════════════════════════════════════
    /// ║ DESCRIÇÃO:
    /// ║    Gerencia controle de acesso ao sistema (permissões usuário-recurso).
    /// ║    Tabela intermediária N:N entre AspNetUsers e Recurso.
    /// ║    Define quais recursos (telas/funcionalidades) cada usuário pode acessar.
    /// ║
    /// ║ ENTIDADE PRINCIPAL:
    /// ║    - ControleAcesso (permissões de acesso).
    /// ║
    /// ║ RELACIONAMENTOS:
    /// ║    - AspNetUsers (N:1) - Usuário que recebe permissão.
    /// ║    - Recurso (N:1) - Recurso (tela/funcionalidade) do sistema.
    /// ║
    /// ║ MÉTODOS PRINCIPAIS:
    /// ║    1. GetControleAcessoListForDropDown() - Lista controles para dropdown.
    /// ║    2. Update() - Atualiza controle de acesso com SaveChanges imediato.
    /// ║
    /// ║ OBSERVAÇÕES:
    /// ║    - GetControleAcessoListForDropDown() retorna RecursoId como Text (ERRO?).
    /// ║      Deveria retornar Recurso.Nome ou descrição legível.
    /// ║    - Não há filtros de Status ou validações.
    /// ║    - Update() quebra padrão Unit of Work (SaveChanges direto).
    /// ║
    /// ║ INTEGRAÇÃO:
    /// ║    - Authorization: Filtros de autorização por recurso.
    /// ║    - Menu: Exibição de itens de menu baseado em permissões.
    /// ║    - Controllers: Verificação de acesso antes de executar ações.
    /// ║══════════════════════════════════════════════════════════════════════════════
    /// </summary>
    public class ControleAcessoRepository : Repository<ControleAcesso>, IControleAcessoRepository
    {
        private new readonly FrotiXDbContext _db;

        public ControleAcessoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: GetControleAcessoListForDropDown (Lista para DropDown)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Retorna lista de controles de acesso formatada para dropdown.
        /// │    SelectListItem: Text = RecursoId (GUID), Value = UsuarioId (string).
        /// │
        /// │ PROBLEMA IDENTIFICADO:
        /// │    ❌ Text = RecursoId.ToString() (GUID não é legível para usuário!).
        /// │    ✅ DEVERIA SER: Text = Recurso.Nome ou Recurso.Descricao.
        /// │
        /// │ PROBLEMA 2:
        /// │    ❌ Não há JOIN com Recurso para obter nome legível.
        /// │    ❌ Não há JOIN com AspNetUsers para exibir nome do usuário.
        /// │
        /// │ SUGESTÃO DE REFATORAÇÃO:
        /// │    return _db.ControleAcesso
        /// │        .Include(ca => ca.Recurso)
        /// │        .Include(ca => ca.Usuario)
        /// │        .Select(ca => new SelectListItem {
        /// │            Text = $"{ca.Usuario.NomeCompleto} - {ca.Recurso.Nome}",
        /// │            Value = ca.RecursoId.ToString()
        /// │        });
        /// │
        /// │ USO ATUAL:
        /// │    - Admin: Gerenciamento de permissões (uso limitado devido ao problema).
        /// │
        /// │ RETORNO:
        /// │    IEnumerable<SelectListItem> (com Text = GUID - não user-friendly).
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public IEnumerable<SelectListItem> GetControleAcessoListForDropDown()
        {
            try
            {
                // [QUERY] - Lista controles de acesso (SEM JOINS - problema de usabilidade)
                return _db.ControleAcesso
                    .Select(i => new SelectListItem()
                    {
                        Text = i.RecursoId.ToString(),  // ❌ GUID não é legível!
                        Value = i.UsuarioId.ToString()
                    });
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("ControleAcessoRepository.cs", "GetControleAcessoListForDropDown", ex);
                return new List<SelectListItem>();
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: Update (Atualizar Controle de Acesso)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Atualiza registro ControleAcesso com SaveChanges IMEDIATO.
        /// │    QUEBRA PADRÃO UNIT OF WORK (não aguarda UnitOfWork.SaveAsync()).
        /// │
        /// │ FLUXO:
        /// │    1. Busca objFromDb com AsTracking() (não usado depois).
        /// │    2. Update(controleAcesso) marca entidade como Modified.
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
        /// │    - Admin: Atualizar permissões de usuário (conceder/revogar acesso).
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public new void Update(ControleAcesso controleAcesso)
        {
            try
            {
                // [QUERY TRACKING] - Busca controle (NÃO USADO - código morto)
                var objFromDb = _db.ControleAcesso.AsTracking().FirstOrDefault(s => s.RecursoId == controleAcesso.RecursoId);

                // [UPDATE] - Marca entidade como Modified
                _db.Update(controleAcesso);

                // [SAVE] - QUEBRA UNIT OF WORK - Persiste imediatamente
                _db.SaveChanges(); // PROBLEMA: Transação independente
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("ControleAcessoRepository.cs", "Update", ex);
                throw; // Propaga exceção (update deve falhar)
            }
        }
    }
}


