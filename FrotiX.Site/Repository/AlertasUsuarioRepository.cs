// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : AlertasUsuarioRepository.cs                                     ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade AlertasUsuario. Gerencia a tabela    ║
// ║ de relação N:N entre alertas e usuários (marcação de leitura, etc).           ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • ObterAlertasPorUsuarioAsync() → Lista alertas de um usuário               ║
// ║ • ObterUsuariosPorAlertaAsync() → Lista usuários de um alerta               ║
// ║ • UsuarioTemAlertaAsync() → Verifica vínculo existente                       ║
// ║ • RemoverAlertasDoUsuarioAsync() → Remove todos alertas do usuário          ║
// ║ • RemoverUsuariosDoAlertaAsync() → Remove todos usuários do alerta          ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrotiX.Repository
{
    public class AlertasUsuarioRepository :Repository<AlertasUsuario>, IAlertasUsuarioRepository
    {
        private new readonly FrotiXDbContext _db;

        public AlertasUsuarioRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        /// <summary>
        /// Obtém todos os alertas de um usuário específico
        /// </summary>
        public async Task<IEnumerable<AlertasUsuario>> ObterAlertasPorUsuarioAsync(string usuarioId)
        {
            return await _db.Set<AlertasUsuario>()
                .Where(au => au.UsuarioId == usuarioId)
                .Include(au => au.AlertasFrotiX)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Obtém todos os usuários vinculados a um alerta
        /// </summary>
        public async Task<IEnumerable<AlertasUsuario>> ObterUsuariosPorAlertaAsync(Guid alertaId)
        {
            return await _db.Set<AlertasUsuario>()
                .Where(au => au.AlertasFrotiXId == alertaId)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Verifica se um usuário já tem um alerta específico vinculado
        /// </summary>
        public async Task<bool> UsuarioTemAlertaAsync(Guid alertaId , string usuarioId)
        {
            return await _db.Set<AlertasUsuario>()
                .AnyAsync(au => au.AlertasFrotiXId == alertaId && au.UsuarioId == usuarioId);
        }

        /// <summary>
        /// Remove todos os alertas de um usuário
        /// </summary>
        public async Task RemoverAlertasDoUsuarioAsync(string usuarioId)
        {
            var alertasUsuario = await _db.Set<AlertasUsuario>()
                .Where(au => au.UsuarioId == usuarioId)
                .ToListAsync();

            if (alertasUsuario.Any())
            {
                _db.Set<AlertasUsuario>().RemoveRange(alertasUsuario);
            }
        }

        /// <summary>
        /// Remove todos os usuários de um alerta
        /// </summary>
        public async Task RemoverUsuariosDoAlertaAsync(Guid alertaId)
        {
            var alertasUsuario = await _db.Set<AlertasUsuario>()
                .Where(au => au.AlertasFrotiXId == alertaId)
                .ToListAsync();

            if (alertasUsuario.Any())
            {
                _db.Set<AlertasUsuario>().RemoveRange(alertasUsuario);
            }
        }

        /// <summary>
        /// Atualiza a entidade AlertasUsuario
        /// </summary>
        public new void Update(AlertasUsuario alertaUsuario)
        {
            if (alertaUsuario == null)
                throw new ArgumentNullException(nameof(alertaUsuario));

            _db.Set<AlertasUsuario>().Update(alertaUsuario);
        }
    }
}
