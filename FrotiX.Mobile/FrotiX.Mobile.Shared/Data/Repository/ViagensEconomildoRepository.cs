using FrotiX.Mobile.Shared.Data.Repository.IRepository;
using FrotiX.Mobile.Shared.Models;
using FrotiX.Mobile.Shared.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrotiX.Mobile.Shared.Data.Repository
{
    public class ViagensEconomildoRepository : Repository<ViagensEconomildo>, IViagensEconomildoRepository
    {
        private readonly FrotiXDbContext _db;

        // CORREÇÃO: Adicionar IAlertaService no construtor
        public ViagensEconomildoRepository(FrotiXDbContext db , IAlertaService alerta) : base(db , alerta)
        {
            _db = db;
        }

        public async Task<List<ViagensEconomildo>> GetViagensByDataAsync(DateTime data)
        {
            try
            {
                return await _db.ViagensEconomildo
                    .Include(v => v.Veiculo)
                    .Include(v => v.Motorista)
                    .Where(v => v.Data.HasValue && v.Data.Value.Date == data.Date)
                    .OrderBy(v => v.HoraInicio)
                    .ToListAsync();
            }
            catch
            {
                return new List<ViagensEconomildo>();
            }
        }

        public async Task<List<ViagensEconomildo>> GetViagensByVeiculoAsync(Guid veiculoId)
        {
            try
            {
                return await _db.ViagensEconomildo
                    .Include(v => v.Veiculo)
                    .Include(v => v.Motorista)
                    .Where(v => v.VeiculoId == veiculoId)
                    .OrderByDescending(v => v.Data)
                    .ThenBy(v => v.HoraInicio)
                    .ToListAsync();
            }
            catch
            {
                return new List<ViagensEconomildo>();
            }
        }

        public async Task<List<ViagensEconomildo>> GetViagensByMotoristaAsync(Guid motoristaId)
        {
            try
            {
                return await _db.ViagensEconomildo
                    .Include(v => v.Veiculo)
                    .Include(v => v.Motorista)
                    .Where(v => v.MotoristaId == motoristaId)
                    .OrderByDescending(v => v.Data)
                    .ThenBy(v => v.HoraInicio)
                    .ToListAsync();
            }
            catch
            {
                return new List<ViagensEconomildo>();
            }
        }

        public async Task<List<ViagensEconomildo>> GetViagensNaoTransmitidasAsync()
        {
            try
            {
                // Esta funcionalidade será implementada no serviço usando SecureStorage
                // pois as viagens não transmitidas ficam em armazenamento local
                return await Task.FromResult(new List<ViagensEconomildo>());
            }
            catch
            {
                return new List<ViagensEconomildo>();
            }
        }

        public async Task<ViagensEconomildo?> GetLastViagemByVeiculoAsync(Guid veiculoId)
        {
            try
            {
                return await _db.ViagensEconomildo
                    .Include(v => v.Veiculo)
                    .Include(v => v.Motorista)
                    .Where(v => v.VeiculoId == veiculoId)
                    .OrderByDescending(v => v.Data)
                    .ThenByDescending(v => v.HoraFim)
                    .FirstOrDefaultAsync();
            }
            catch
            {
                return null;
            }
        }
    }
}