/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: EscalasRepository.cs                                                                  ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Arquivo composto com múltiplos repositórios do módulo de Escalas de Motoristas.                ║
   ║    Gerencia escalas diárias, turnos, folgas, férias, coberturas e views consolidadas.             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 REPOSITÓRIOS INCLUÍDOS:                                                                         ║
   ║    • TipoServicoRepository, TurnoRepository, VAssociadoRepository                                 ║
   ║    • EscalaDiariaRepository, FolgaRecessoRepository, FeriasRepository                              ║
   ║    • CoberturaFolgaRepository, ObservacoesEscalaRepository                                         ║
   ║    • ViewEscalasCompletasRepository, ViewMotoristasVezRepository, ViewStatusMotoristasRepository  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    Existem métodos mantidos por compatibilidade e trechos com logs/diagnóstico.                   ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FrotiX.Repository
{
    // Implementação TipoServico Repository
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: TipoServicoRepository                                                          │
    // │ 📦 HERDA DE: Repository                                                      │
    // │ 🔌 IMPLEMENTA: ITipoServicoRepository                                                    │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório do módulo de escalas: TipoServicoRepository.
    
    public class TipoServicoRepository : Repository<TipoServico>, ITipoServicoRepository
    {
        private readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: TipoServicoRepository                                             │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Executar a operação definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // db - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // void - Sem retorno
        
        
        // Param db: Descrição do parâmetro.
        public TipoServicoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetTipoServicoListForDropDown                                     │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // Sem parâmetros.
        
        
        
        // 📤 RETORNO:
        // IEnumerable - Resultado da operação
        
        
        // Returns: Descrição do retorno.
        public IEnumerable<SelectListItem> GetTipoServicoListForDropDown()
        {
            return _db.Set<TipoServico>()
                .Where(x => x.Ativo)
                .Select(i => new SelectListItem()
                {
                    Text = i.NomeServico,
                    Value = i.TipoServicoId.ToString()
                });
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                            │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualizar os dados da entidade correspondente.
        
        
        // 📥 PARÂMETROS:
        // tipoServico - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // void - Sem retorno
        
        
        // Param tipoServico: Descrição do parâmetro.
        public void Update(TipoServico tipoServico)
        {
            var objFromDb = _db.Set<TipoServico>().FirstOrDefault(s => s.TipoServicoId == tipoServico.TipoServicoId);
            if (objFromDb != null)
            {
                objFromDb.NomeServico = tipoServico.NomeServico;
                objFromDb.Descricao = tipoServico.Descricao;
                objFromDb.Ativo = tipoServico.Ativo;
                objFromDb.DataAlteracao = DateTime.Now;
                objFromDb.UsuarioIdAlteracao = tipoServico.UsuarioIdAlteracao;
            }
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ExisteNomeServicoAsync                                            │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Verificar condição/regra de negócio e retornar o resultado.
        
        
        // 📥 PARÂMETROS:
        // nomeServico - Descrição do parâmetro
        // excludeId - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task - Resultado da operação
        
        
        // Param nomeServico: Descrição do parâmetro.
        // Param excludeId: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public async Task<bool> ExisteNomeServicoAsync(string nomeServico, Guid? excludeId = null)
        {
            var query = _db.Set<TipoServico>().Where(x => x.NomeServico == nomeServico && x.Ativo);
            
            if (excludeId.HasValue)
            {
                query = query.Where(x => x.TipoServicoId != excludeId.Value);
            }

            return await query.AnyAsync();
        }
    }

    // Implementação Turno Repository
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: TurnoRepository                                                                │
    // │ 📦 HERDA DE: Repository                                                            │
    // │ 🔌 IMPLEMENTA: ITurnoRepository                                                          │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório do módulo de escalas: TurnoRepository.
    
    public class TurnoRepository : Repository<Turno>, ITurnoRepository
    {
        private readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: TurnoRepository                                                   │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Executar a operação definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // db - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // void - Sem retorno
        
        
        // Param db: Descrição do parâmetro.
        public TurnoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetTurnoListForDropDown                                           │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // Sem parâmetros.
        
        
        
        // 📤 RETORNO:
        // IEnumerable - Resultado da operação
        
        
        // Returns: Descrição do retorno.
        public IEnumerable<SelectListItem> GetTurnoListForDropDown()
        {
            return _db.Set<Turno>()
                .Where(x => x.Ativo)
                .Select(i => new SelectListItem()
                {
                    Text = i.NomeTurno,
                    Value = i.TurnoId.ToString()
                });
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                            │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualizar os dados da entidade correspondente.
        
        
        // 📥 PARÂMETROS:
        // turno - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // void - Sem retorno
        
        
        // Param turno: Descrição do parâmetro.
        public void Update(Turno turno)
        {
            var objFromDb = _db.Set<Turno>().FirstOrDefault(s => s.TurnoId == turno.TurnoId);
            if (objFromDb != null)
            {
                objFromDb.NomeTurno = turno.NomeTurno;
                objFromDb.HoraInicio = turno.HoraInicio;
                objFromDb.HoraFim = turno.HoraFim;
                objFromDb.Ativo = turno.Ativo;
                objFromDb.DataAlteracao = DateTime.Now;
                objFromDb.UsuarioIdAlteracao = turno.UsuarioIdAlteracao;
            }
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetTurnoByNomeAsync                                               │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // nomeTurno - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task - Resultado da operação
        
        
        // Param nomeTurno: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public async Task<Turno> GetTurnoByNomeAsync(string nomeTurno)
        {
            return await _db.Set<Turno>()
                .FirstOrDefaultAsync(x => x.NomeTurno == nomeTurno && x.Ativo);
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: VerificarConflitoHorarioAsync                                     │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Verificar condição/regra de negócio e retornar o resultado.
        
        
        // 📥 PARÂMETROS:
        // horaInicio - Descrição do parâmetro
        // horaFim - Descrição do parâmetro
        // excludeId - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task - Resultado da operação
        
        
        // Param horaInicio: Descrição do parâmetro.
        // Param horaFim: Descrição do parâmetro.
        // Param excludeId: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public async Task<bool> VerificarConflitoHorarioAsync(TimeSpan horaInicio, TimeSpan horaFim, Guid? excludeId = null)
        {
            var query = _db.Set<Turno>().Where(x => x.Ativo);
            
            if (excludeId.HasValue)
            {
                query = query.Where(x => x.TurnoId != excludeId.Value);
            }

            return await query.AnyAsync(x => 
                (horaInicio >= x.HoraInicio && horaInicio < x.HoraFim) ||
                (horaFim > x.HoraInicio && horaFim <= x.HoraFim) ||
                (horaInicio <= x.HoraInicio && horaFim >= x.HoraFim)
            );
        }
    }

    // Implementação VAssociado Repository
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: VAssociadoRepository                                                           │
    // │ 📦 HERDA DE: Repository                                                       │
    // │ 🔌 IMPLEMENTA: IVAssociadoRepository                                                     │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório do módulo de escalas: VAssociadoRepository.
    
    public class VAssociadoRepository : Repository<VAssociado>, IVAssociadoRepository
    {
        private readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: VAssociadoRepository                                              │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Executar a operação definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // db - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // void - Sem retorno
        
        
        // Param db: Descrição do parâmetro.
        public VAssociadoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                            │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualizar os dados da entidade correspondente.
        
        
        // 📥 PARÂMETROS:
        // vAssociado - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // void - Sem retorno
        
        
        // Param vAssociado: Descrição do parâmetro.
        public void Update(VAssociado vAssociado)
        {
            var objFromDb = _db.Set<VAssociado>().FirstOrDefault(s => s.AssociacaoId == vAssociado.AssociacaoId);
            if (objFromDb != null)
            {
                objFromDb.MotoristaId = vAssociado.MotoristaId;
                objFromDb.VeiculoId = vAssociado.VeiculoId;
                objFromDb.DataInicio = vAssociado.DataInicio;
                objFromDb.DataFim = vAssociado.DataFim;
                objFromDb.Observacoes = vAssociado.Observacoes;
                objFromDb.Ativo = vAssociado.Ativo;
                objFromDb.DataAlteracao = DateTime.Now;
                objFromDb.UsuarioIdAlteracao = vAssociado.UsuarioIdAlteracao;
            }
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetAssociacaoAtivaAsync                                           │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // motoristaId - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task - Resultado da operação
        
        
        // Param motoristaId: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public async Task<VAssociado> GetAssociacaoAtivaAsync(Guid motoristaId)
        {
            return await _db.Set<VAssociado>()
                .Include(x => x.Motorista)
                .Include(x => x.Veiculo)
                .FirstOrDefaultAsync(x => x.MotoristaId == motoristaId && 
                                         x.Ativo && 
                                         (x.DataFim == null || x.DataFim > DateTime.Now));
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetHistoricoAssociacoesAsync                                      │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // motoristaId - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task> - Resultado da operação
        
        
        // Param motoristaId: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public async Task<List<VAssociado>> GetHistoricoAssociacoesAsync(Guid motoristaId)
        {
            return await _db.Set<VAssociado>()
                .Include(x => x.Motorista)
                .Include(x => x.Veiculo)
                .Where(x => x.MotoristaId == motoristaId)
                .OrderByDescending(x => x.DataInicio)
                .ToListAsync();
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: MotoristaTemVeiculoAsync                                          │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Executar a operação definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // motoristaId - Descrição do parâmetro
        // data - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task - Resultado da operação
        
        
        // Param motoristaId: Descrição do parâmetro.
        // Param data: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public async Task<bool> MotoristaTemVeiculoAsync(Guid motoristaId, DateTime data)
        {
            return await _db.Set<VAssociado>()
                .AnyAsync(x => x.MotoristaId == motoristaId && 
                              x.Ativo && 
                              x.DataInicio <= data &&
                              (x.DataFim == null || x.DataFim > data));
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetAssociacaoPorDataAsync                                         │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // motoristaId - Descrição do parâmetro
        // data - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task - Resultado da operação
        
        
        // Param motoristaId: Descrição do parâmetro.
        // Param data: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public async Task<VAssociado> GetAssociacaoPorDataAsync(Guid motoristaId, DateTime data)
        {
            return await _db.Set<VAssociado>()
                .Include(x => x.Motorista)
                .Include(x => x.Veiculo)
                .FirstOrDefaultAsync(x => x.MotoristaId == motoristaId && 
                                         x.Ativo && 
                                         x.DataInicio <= data &&
                                         (x.DataFim == null || x.DataFim > data));
        }
    }

    // Implementação EscalaDiaria Repository
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: EscalaDiariaRepository                                                         │
    // │ 📦 HERDA DE: Repository                                                     │
    // │ 🔌 IMPLEMENTA: IEscalaDiariaRepository                                                   │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório do módulo de escalas: EscalaDiariaRepository.
    
    public class EscalaDiariaRepository : Repository<EscalaDiaria>, IEscalaDiariaRepository
    {
        private readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: EscalaDiariaRepository                                            │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Executar a operação definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // db - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // void - Sem retorno
        
        
        // Param db: Descrição do parâmetro.
        public EscalaDiariaRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                            │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualizar os dados da entidade correspondente.
        
        
        // 📥 PARÂMETROS:
        // escalaDiaria - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // void - Sem retorno
        
        
        // Param escalaDiaria: Descrição do parâmetro.
        public void Update(EscalaDiaria escalaDiaria)
        {
            var objFromDb = _db.Set<EscalaDiaria>().FirstOrDefault(s => s.EscalaDiaId == escalaDiaria.EscalaDiaId);
            if (objFromDb != null)
            {
                objFromDb.AssociacaoId = escalaDiaria.AssociacaoId;
                objFromDb.TipoServicoId = escalaDiaria.TipoServicoId;
                objFromDb.TurnoId = escalaDiaria.TurnoId;
                objFromDb.DataEscala = escalaDiaria.DataEscala;
                objFromDb.HoraInicio = escalaDiaria.HoraInicio;
                objFromDb.HoraFim = escalaDiaria.HoraFim;
                objFromDb.HoraIntervaloInicio = escalaDiaria.HoraIntervaloInicio;
                objFromDb.HoraIntervaloFim = escalaDiaria.HoraIntervaloFim;
                objFromDb.Lotacao = escalaDiaria.Lotacao;
                objFromDb.NumeroSaidas = escalaDiaria.NumeroSaidas;
                objFromDb.StatusMotorista = escalaDiaria.StatusMotorista;
                objFromDb.RequisitanteId = escalaDiaria.RequisitanteId;
                objFromDb.Observacoes = escalaDiaria.Observacoes;
                objFromDb.DataAlteracao = DateTime.Now;
                objFromDb.UsuarioIdAlteracao = escalaDiaria.UsuarioIdAlteracao;
            }
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetEscalasCompletasAsync                                          │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // data - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task> - Resultado da operação
        
        
        // Param data: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public async Task<List<ViewEscalasCompletas>> GetEscalasCompletasAsync(DateTime? data = null)
        {
            var dataEscala = data ?? DateTime.Today;

            var query = from ed in _db.Set<EscalaDiaria>()
                       join va in _db.Set<VAssociado>() on ed.AssociacaoId equals va.AssociacaoId into vaLeft
                       from va in vaLeft.DefaultIfEmpty()
                       join m in _db.Set<Motorista>() on va.MotoristaId equals m.MotoristaId into mLeft
                       from m in mLeft.DefaultIfEmpty()
                       join v in _db.Set<ViewVeiculos>() on va.VeiculoId equals v.VeiculoId into vLeft
                       from v in vLeft.DefaultIfEmpty()
                       join ts in _db.Set<TipoServico>() on ed.TipoServicoId equals ts.TipoServicoId
                       join t in _db.Set<Turno>() on ed.TurnoId equals t.TurnoId
                       join r in _db.Set<Requisitante>() on ed.RequisitanteId equals r.RequisitanteId into rLeft
                       from r in rLeft.DefaultIfEmpty()
                       where ed.DataEscala == dataEscala && ed.Ativo
                       orderby ed.HoraInicio
                       select new ViewEscalasCompletas
                       {
                           EscalaDiaId = ed.EscalaDiaId,
                           DataEscala = ed.DataEscala,
                           HoraInicio = ed.HoraInicio.ToString(@"hh\:mm"),
                           HoraFim = ed.HoraFim.ToString(@"hh\:mm"),
                           HoraIntervaloInicio = ed.HoraIntervaloInicio.HasValue ? 
                               ed.HoraIntervaloInicio.Value.ToString(@"hh\:mm") : null,
                           HoraIntervaloFim = ed.HoraIntervaloFim.HasValue ? 
                               ed.HoraIntervaloFim.Value.ToString(@"hh\:mm") : null,
                           NumeroSaidas = ed.NumeroSaidas,
                           StatusMotorista = ed.StatusMotorista,
                           Lotacao = ed.Lotacao,
                           Observacoes = ed.Observacoes,
                           MotoristaId = m.MotoristaId,
                           NomeMotorista = m.Nome,
                           Ponto = m.Ponto,
                           CPF = m.CPF,
                           CNH = m.CNH,
                           Celular01 = m.Celular01,
                           Foto = m.Foto,
                           VeiculoId = v.VeiculoId,
                           VeiculoDescricao = v.Descricao,
                           Placa = v.Placa,
                           Modelo = v.MarcaModelo,
                           TipoServicoId = ts.TipoServicoId,
                           NomeServico = ts.NomeServico,
                           TurnoId = t.TurnoId,
                           NomeTurno = t.NomeTurno,
                           RequisitanteId = r.RequisitanteId,
                           NomeRequisitante = r.Nome
                       };

            return await query.ToListAsync();
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetEscalaCompletaByIdAsync                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // id - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task - Resultado da operação
        
        
        // Param id: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public async Task<ViewEscalasCompletas> GetEscalaCompletaByIdAsync(Guid id)
        {
            var query = from ed in _db.Set<EscalaDiaria>()
                       join va in _db.Set<VAssociado>() on ed.AssociacaoId equals va.AssociacaoId into vaLeft
                       from va in vaLeft.DefaultIfEmpty()
                       join m in _db.Set<Motorista>() on va.MotoristaId equals m.MotoristaId into mLeft
                       from m in mLeft.DefaultIfEmpty()
                       join v in _db.Set<ViewVeiculos>() on va.VeiculoId equals v.VeiculoId into vLeft
                       from v in vLeft.DefaultIfEmpty()
                       join ts in _db.Set<TipoServico>() on ed.TipoServicoId equals ts.TipoServicoId
                       join t in _db.Set<Turno>() on ed.TurnoId equals t.TurnoId
                       join r in _db.Set<Requisitante>() on ed.RequisitanteId equals r.RequisitanteId into rLeft
                       from r in rLeft.DefaultIfEmpty()
                       where ed.EscalaDiaId == id && ed.Ativo
                       select new ViewEscalasCompletas
                       {
                           EscalaDiaId = ed.EscalaDiaId,
                           DataEscala = ed.DataEscala,
                           HoraInicio = ed.HoraInicio.ToString(@"hh\:mm"),
                           HoraFim = ed.HoraFim.ToString(@"hh\:mm"),
                           HoraIntervaloInicio = ed.HoraIntervaloInicio.HasValue ? 
                               ed.HoraIntervaloInicio.Value.ToString(@"hh\:mm") : null,
                           HoraIntervaloFim = ed.HoraIntervaloFim.HasValue ? 
                               ed.HoraIntervaloFim.Value.ToString(@"hh\:mm") : null,
                           NumeroSaidas = ed.NumeroSaidas,
                           StatusMotorista = ed.StatusMotorista,
                           Lotacao = ed.Lotacao,
                           Observacoes = ed.Observacoes,
                           MotoristaId = m.MotoristaId,
                           NomeMotorista = m.Nome,
                           Ponto = m.Ponto,
                           CPF = m.CPF,
                           CNH = m.CNH,
                           Celular01 = m.Celular01,
                           Foto = m.Foto,
                           VeiculoId = v.VeiculoId,
                           VeiculoDescricao = v.Descricao,
                           Placa = v.Placa,
                           Modelo = v.MarcaModelo,
                           TipoServicoId = ts.TipoServicoId,
                           NomeServico = ts.NomeServico,
                           TurnoId = t.TurnoId,
                           NomeTurno = t.NomeTurno,
                           RequisitanteId = r.RequisitanteId,
                           NomeRequisitante = r.Nome
                       };

            return await query.FirstOrDefaultAsync();
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetEscalasPorFiltroAsync                                          │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // filtro - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task> - Resultado da operação
        
        
        // Param filtro: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public async Task<List<ViewEscalasCompletas>> GetEscalasPorFiltroAsync(FiltroEscalaViewModel filtro)
        {
            var query = from ed in _db.Set<EscalaDiaria>()
                       join va in _db.Set<VAssociado>() on ed.AssociacaoId equals va.AssociacaoId into vaLeft
                       from va in vaLeft.DefaultIfEmpty()
                       join m in _db.Set<Motorista>() on va.MotoristaId equals m.MotoristaId into mLeft
                       from m in mLeft.DefaultIfEmpty()
                       join v in _db.Set<ViewVeiculos>() on va.VeiculoId equals v.VeiculoId into vLeft
                       from v in vLeft.DefaultIfEmpty()
                       join ts in _db.Set<TipoServico>() on ed.TipoServicoId equals ts.TipoServicoId
                       join t in _db.Set<Turno>() on ed.TurnoId equals t.TurnoId
                       join r in _db.Set<Requisitante>() on ed.RequisitanteId equals r.RequisitanteId into rLeft
                       from r in rLeft.DefaultIfEmpty()
                       where ed.Ativo
                       select new { ed, va, m, v, ts, t, r };

            // Aplicar filtros
            if (filtro.DataFiltro.HasValue)
                query = query.Where(x => x.ed.DataEscala == filtro.DataFiltro.Value);

            if (filtro.TipoServicoId.HasValue)
                query = query.Where(x => x.ed.TipoServicoId == filtro.TipoServicoId.Value);

            if (!string.IsNullOrWhiteSpace(filtro.Lotacao))
                query = query.Where(x => x.ed.Lotacao == filtro.Lotacao);

            if (filtro.VeiculoId.HasValue)
                query = query.Where(x => x.v != null && x.v.VeiculoId == filtro.VeiculoId.Value);

            if (filtro.MotoristaId.HasValue)
                query = query.Where(x => x.m != null && x.m.MotoristaId == filtro.MotoristaId.Value);

            if (!string.IsNullOrWhiteSpace(filtro.StatusMotorista))
                query = query.Where(x => x.ed.StatusMotorista == filtro.StatusMotorista);

            if (filtro.TurnoId.HasValue)
                query = query.Where(x => x.ed.TurnoId == filtro.TurnoId.Value);

            if (!string.IsNullOrWhiteSpace(filtro.TextoPesquisa))
            {
                query = query.Where(x => 
                    (x.m != null && x.m.Nome.Contains(filtro.TextoPesquisa)) ||
                    (x.v != null && x.v.Placa.Contains(filtro.TextoPesquisa)) ||
                    (x.ed.Observacoes != null && x.ed.Observacoes.Contains(filtro.TextoPesquisa))
                );
            }

            var result = await query.OrderBy(x => x.ed.DataEscala)
                                   .ThenBy(x => x.ed.HoraInicio)
                                   .Select(x => new ViewEscalasCompletas
                                   {
                                       EscalaDiaId = x.ed.EscalaDiaId,
                                       DataEscala = x.ed.DataEscala,
                                       HoraInicio = x.ed.HoraInicio.ToString(@"hh\:mm"),
                                       HoraFim = x.ed.HoraFim.ToString(@"hh\:mm"),
                                       NumeroSaidas = x.ed.NumeroSaidas,
                                       StatusMotorista = x.ed.StatusMotorista,
                                       Lotacao = x.ed.Lotacao,
                                       Observacoes = x.ed.Observacoes,
                                       MotoristaId = x.m.MotoristaId,
                                       NomeMotorista = x.m.Nome,
                                       Ponto = x.m.Ponto,
                                       VeiculoId = x.v.VeiculoId,
                                       VeiculoDescricao = x.v.Descricao,
                                       Placa = x.v.Placa,
                                       NomeServico = x.ts.NomeServico,
                                       NomeTurno = x.t.NomeTurno,
                                       NomeRequisitante = x.r.Nome
                                   }).ToListAsync();

            return result;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetMotoristasVezAsync                                             │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // quantidade - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task> - Resultado da operação
        
        
        // Param quantidade: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public async Task<List<ViewMotoristasVez>> GetMotoristasVezAsync(int quantidade = 5)
        {
            var hoje = DateTime.Today;
            var agora = DateTime.Now.TimeOfDay;

            // Primeiro, buscar contagem de viagens realizadas hoje
            var viagensHoje = await _db.Set<Viagem>()
                .Where(v => v.DataFinalizacao == hoje && v.Status == "Realizada")
                .GroupBy(v => v.MotoristaId)
                .Select(g => new 
                { 
                    MotoristaId = g.Key, 
                    NumeroViagens = g.Count() 
                })
                .ToListAsync();

            var query = from ed in _db.Set<EscalaDiaria>()
                       join va in _db.Set<VAssociado>() on ed.AssociacaoId equals va.AssociacaoId
                       join m in _db.Set<Motorista>() on va.MotoristaId equals m.MotoristaId
                       join v in _db.Set<ViewVeiculos>() on va.VeiculoId equals v.VeiculoId into vLeft
                       from v in vLeft.DefaultIfEmpty()
                       where ed.DataEscala == hoje &&
                             ed.Ativo &&
                             ed.StatusMotorista == "Disponível" &&
                             ed.HoraInicio <= agora &&
                             ed.HoraFim >= agora
                       select new 
                       {
                           ed,
                           m,
                           v
                       };

            var escalas = await query.ToListAsync();

            var resultado = escalas
                .Select(x => new ViewMotoristasVez
                {
                    MotoristaId = x.m.MotoristaId,
                    NomeMotorista = x.m.Nome,
                    Ponto = x.m.Ponto,
                    Foto = x.m.Foto,
                    DataEscala = x.ed.DataEscala,
                    NumeroSaidas = viagensHoje.FirstOrDefault(vh => vh.MotoristaId == x.m.MotoristaId)?.NumeroViagens ?? 0,
                    StatusMotorista = x.ed.StatusMotorista,
                    Lotacao = x.ed.Lotacao,
                    VeiculoDescricao = x.v?.Descricao,
                    Placa = x.v?.Placa,
                    HoraInicio = x.ed.HoraInicio.ToString(@"hh\:mm"),
                    HoraFim = x.ed.HoraFim.ToString(@"hh\:mm")
                })
                .OrderBy(x => x.NumeroSaidas)
                .ThenBy(x => x.HoraInicio)
                .Take(quantidade)
                .ToList();

            return resultado;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetStatusMotoristasAsync                                          │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // Sem parâmetros.
        
        
        
        // 📤 RETORNO:
        // Task> - Resultado da operação
        
        
        // Returns: Descrição do retorno.
        public async Task<List<ViewStatusMotoristas>> GetStatusMotoristasAsync()
        {
            var hoje = DateTime.Today;

            var query = from m in _db.Set<Motorista>()
                       join va in _db.Set<VAssociado>() on m.MotoristaId equals va.MotoristaId into vaLeft
                       from va in vaLeft.Where(x => x.Ativo).DefaultIfEmpty()
                       join v in _db.Set<ViewVeiculos>() on va.VeiculoId equals v.VeiculoId into vLeft
                       from v in vLeft.DefaultIfEmpty()
                       join ed in _db.Set<EscalaDiaria>() on va.AssociacaoId equals ed.AssociacaoId into edLeft
                       from ed in edLeft.Where(x => x.DataEscala == hoje && x.Ativo).DefaultIfEmpty()
                       join fr in _db.Set<FolgaRecesso>() on m.MotoristaId equals fr.MotoristaId into frLeft
                       from fr in frLeft.Where(x => hoje >= x.DataInicio && hoje <= x.DataFim && x.Ativo).DefaultIfEmpty()
                       join f in _db.Set<Ferias>() on m.MotoristaId equals f.MotoristaId into fLeft
                       from f in fLeft.Where(x => hoje >= x.DataInicio && hoje <= x.DataFim && x.Ativo).DefaultIfEmpty()
                       where m.Status == true
                       select new ViewStatusMotoristas
                       {
                           MotoristaId = m.MotoristaId,
                           Nome = m.Nome,
                           Ponto = m.Ponto,
                           StatusAtual = f != null ? "Férias" :
                                       fr != null ? fr.Tipo :
                                       ed != null ? ed.StatusMotorista :
                                       "Sem Escala",
                           DataEscala = ed.DataEscala,
                           NumeroSaidas = ed != null ? ed.NumeroSaidas : 0,
                           Placa = v.Placa,
                           Veiculo = v.Descricao
                       };

            return await query.ToListAsync();
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: AtualizarStatusMotoristaAsync                                     │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualizar os dados da entidade correspondente.
        
        
        // 📥 PARÂMETROS:
        // motoristaId - Descrição do parâmetro
        // novoStatus - Descrição do parâmetro
        // data - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task - Resultado da operação
        
        
        // Param motoristaId: Descrição do parâmetro.
        // Param novoStatus: Descrição do parâmetro.
        // Param data: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public async Task<bool> AtualizarStatusMotoristaAsync(Guid motoristaId, string novoStatus, DateTime? data = null)
        {
            var dataEscala = data ?? DateTime.Today;

            var escala = await _db.Set<EscalaDiaria>()
                .FirstOrDefaultAsync(ed => 
                    ed.Associacao.MotoristaId == motoristaId &&
                    ed.DataEscala == dataEscala &&
                    ed.Ativo);

            if (escala != null)
            {
                escala.StatusMotorista = novoStatus;
                escala.DataAlteracao = DateTime.Now;
                _db.Update(escala);
                return true;
            }

            return false;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: IncrementarContadorViagemAsync                                    │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Executar operação de incremento/compatibilidade.
        
        
        // 📥 PARÂMETROS:
        // motoristaId - Descrição do parâmetro
        // data - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task - Resultado da operação
        
        
        // Param motoristaId: Descrição do parâmetro.
        // Param data: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public async Task<bool> IncrementarContadorViagemAsync(Guid motoristaId, DateTime data)
        {
            // Este método não é mais necessário pois o contador agora vem da tabela Viagem
            // Mantido por compatibilidade
            return await Task.FromResult(true);
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: MotoristaDisponivelAsync                                          │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Verificar disponibilidade do motorista para o período informado.
        
        
        // 📥 PARÂMETROS:
        // motoristaId - Descrição do parâmetro
        // data - Descrição do parâmetro
        // horaInicio - Descrição do parâmetro
        // horaFim - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task - Resultado da operação
        
        
        // Param motoristaId: Descrição do parâmetro.
        // Param data: Descrição do parâmetro.
        // Param horaInicio: Descrição do parâmetro.
        // Param horaFim: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public async Task<bool> MotoristaDisponivelAsync(Guid motoristaId, DateTime data, TimeSpan horaInicio, TimeSpan horaFim)
        {
            // Verificar se não há conflito com outras escalas
            var temConflito = await _db.Set<EscalaDiaria>()
                .AnyAsync(ed => 
                    ed.Associacao.MotoristaId == motoristaId &&
                    ed.DataEscala == data &&
                    ed.Ativo &&
                    ((horaInicio >= ed.HoraInicio && horaInicio < ed.HoraFim) ||
                     (horaFim > ed.HoraInicio && horaFim <= ed.HoraFim) ||
                     (horaInicio <= ed.HoraInicio && horaFim >= ed.HoraFim))
                );

            // Verificar se não está em folga/férias
            var estaEmFolga = await _db.Set<FolgaRecesso>()
                .AnyAsync(f => f.MotoristaId == motoristaId &&
                              f.DataInicio <= data &&
                              f.DataFim >= data &&
                              f.Ativo);

            var estaEmFerias = await _db.Set<Ferias>()
                .AnyAsync(f => f.MotoristaId == motoristaId &&
                               f.DataInicio <= data &&
                               f.DataFim >= data &&
                               f.Ativo);

            return !temConflito && !estaEmFolga && !estaEmFerias;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: VeiculoDisponivelAsync                                            │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Verificar disponibilidade do veículo para o período informado.
        
        
        // 📥 PARÂMETROS:
        // veiculoId - Descrição do parâmetro
        // data - Descrição do parâmetro
        // horaInicio - Descrição do parâmetro
        // horaFim - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task - Resultado da operação
        
        
        // Param veiculoId: Descrição do parâmetro.
        // Param data: Descrição do parâmetro.
        // Param horaInicio: Descrição do parâmetro.
        // Param horaFim: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public async Task<bool> VeiculoDisponivelAsync(Guid veiculoId, DateTime data, TimeSpan horaInicio, TimeSpan horaFim)
        {
            var temConflito = await _db.Set<EscalaDiaria>()
                .AnyAsync(ed => 
                    ed.Associacao.VeiculoId == veiculoId &&
                    ed.DataEscala == data &&
                    ed.Ativo &&
                    ((horaInicio >= ed.HoraInicio && horaInicio < ed.HoraFim) ||
                     (horaFim > ed.HoraInicio && horaFim <= ed.HoraFim) ||
                     (horaInicio <= ed.HoraInicio && horaFim >= ed.HoraFim))
                );

            return !temConflito;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ExisteEscalaConflitanteAsync                                      │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Verificar condição/regra de negócio e retornar o resultado.
        
        
        // 📥 PARÂMETROS:
        // motoristaId - Descrição do parâmetro
        // data - Descrição do parâmetro
        // horaInicio - Descrição do parâmetro
        // horaFim - Descrição do parâmetro
        // excludeId - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task - Resultado da operação
        
        
        // Param motoristaId: Descrição do parâmetro.
        // Param data: Descrição do parâmetro.
        // Param horaInicio: Descrição do parâmetro.
        // Param horaFim: Descrição do parâmetro.
        // Param excludeId: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public async Task<bool> ExisteEscalaConflitanteAsync(Guid motoristaId, DateTime data, TimeSpan horaInicio, TimeSpan horaFim, Guid? excludeId = null)
        {
            var query = _db.Set<EscalaDiaria>()
                .Where(ed => 
                    ed.Associacao.MotoristaId == motoristaId &&
                    ed.DataEscala == data &&
                    ed.Ativo);

            if (excludeId.HasValue)
            {
                query = query.Where(ed => ed.EscalaDiaId != excludeId.Value);
            }

            return await query.AnyAsync(ed =>
                (horaInicio >= ed.HoraInicio && horaInicio < ed.HoraFim) ||
                (horaFim > ed.HoraInicio && horaFim <= ed.HoraFim) ||
                (horaInicio <= ed.HoraInicio && horaFim >= ed.HoraFim)
            );
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetEscalasPorPeriodoAsync                                         │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // dataInicio - Descrição do parâmetro
        // dataFim - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task> - Resultado da operação
        
        
        // Param dataInicio: Descrição do parâmetro.
        // Param dataFim: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public async Task<List<EscalaDiaria>> GetEscalasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await _db.Set<EscalaDiaria>()
                .Include(x => x.Associacao)
                    .ThenInclude(a => a.Motorista)
                .Include(x => x.Associacao)
                    .ThenInclude(a => a.Veiculo)
                .Include(x => x.TipoServico)
                .Include(x => x.Turno)
                .Include(x => x.Requisitante)
                .Where(x => x.DataEscala >= dataInicio && 
                           x.DataEscala <= dataFim && 
                           x.Ativo)
                .OrderBy(x => x.DataEscala)
                .ThenBy(x => x.HoraInicio)
                .ToListAsync();
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetEscalasMotoristaAsync                                          │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // motoristaId - Descrição do parâmetro
        // data - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task> - Resultado da operação
        
        
        // Param motoristaId: Descrição do parâmetro.
        // Param data: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public async Task<List<EscalaDiaria>> GetEscalasMotoristaAsync(Guid motoristaId, DateTime? data = null)
        {
            var query = _db.Set<EscalaDiaria>()
                .Include(x => x.Associacao)
                    .ThenInclude(a => a.Motorista)
                .Include(x => x.Associacao)
                    .ThenInclude(a => a.Veiculo)
                .Include(x => x.TipoServico)
                .Include(x => x.Turno)
                .Include(x => x.Requisitante)
                .Where(x => x.Associacao.MotoristaId == motoristaId && x.Ativo);

            if (data.HasValue)
            {
                query = query.Where(x => x.DataEscala == data.Value);
            }

            return await query.OrderBy(x => x.DataEscala)
                             .ThenBy(x => x.HoraInicio)
                             .ToListAsync();
        }
    }

    // ========================= FolgaRecesso =========================
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: FolgaRecessoRepository                                                         │
    // │ 📦 HERDA DE: Repository                                       │
    // │ 🔌 IMPLEMENTA: IFolgaRecessoRepository                                                   │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório do módulo de escalas: FolgaRecessoRepository.
    
    public class FolgaRecessoRepository : Repository<FrotiX.Models.FolgaRecesso>, IFolgaRecessoRepository
    {
        private readonly FrotiXDbContext _db;
        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: FolgaRecessoRepository                                            │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Executar a operação definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // db - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // void - Sem retorno
        
        
        // Param db: Descrição do parâmetro.
        public FolgaRecessoRepository(FrotiXDbContext db) : base(db) => _db = db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                            │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualizar os dados da entidade correspondente.
        
        
        // 📥 PARÂMETROS:
        // folgaRecesso - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // void - Sem retorno
        
        
        // Param folgaRecesso: Descrição do parâmetro.
        public void Update(FrotiX.Models.FolgaRecesso folgaRecesso)
        {
            var set = _db.Set<FrotiX.Models.FolgaRecesso>();
            var obj = set.FirstOrDefault(x => x.FolgaId == folgaRecesso.FolgaId);
            if (obj != null)
            {
                obj.MotoristaId = folgaRecesso.MotoristaId;
                obj.Tipo = folgaRecesso.Tipo;
                obj.DataInicio = folgaRecesso.DataInicio;
                obj.DataFim = folgaRecesso.DataFim;
                obj.Ativo = folgaRecesso.Ativo;
                obj.Observacoes = folgaRecesso.Observacoes;
                obj.DataAlteracao = DateTime.Now;
                obj.UsuarioIdAlteracao = folgaRecesso.UsuarioIdAlteracao;
            }
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetFolgasPorMotoristaAsync                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // motoristaId - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task> - Resultado da operação
        
        
        // Param motoristaId: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public Task<List<FrotiX.Models.FolgaRecesso>> GetFolgasPorMotoristaAsync(Guid motoristaId)
            => _db.Set<FrotiX.Models.FolgaRecesso>()
                  .Where(x => x.MotoristaId == motoristaId)
                  .OrderByDescending(x => x.DataInicio)
                  .ToListAsync();

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetFolgasAtivasAsync                                              │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // data - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task> - Resultado da operação
        
        
        // Param data: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public Task<List<FrotiX.Models.FolgaRecesso>> GetFolgasAtivasAsync(DateTime data)
            => _db.Set<FrotiX.Models.FolgaRecesso>()
                  .Where(x => x.Ativo && x.DataInicio <= data && x.DataFim >= data)
                  .ToListAsync();

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: MotoristaEstaEmFolgaAsync                                         │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Executar a operação definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // motoristaId - Descrição do parâmetro
        // data - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task - Resultado da operação
        
        
        // Param motoristaId: Descrição do parâmetro.
        // Param data: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public Task<bool> MotoristaEstaEmFolgaAsync(Guid motoristaId, DateTime data)
            => _db.Set<FrotiX.Models.FolgaRecesso>()
                  .AnyAsync(x => x.MotoristaId == motoristaId && x.Ativo && x.DataInicio <= data && x.DataFim >= data);

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetFolgaAtivaMotoristaAsync                                       │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // motoristaId - Descrição do parâmetro
        // data - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task - Resultado da operação
        
        
        // Param motoristaId: Descrição do parâmetro.
        // Param data: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public Task<FrotiX.Models.FolgaRecesso> GetFolgaAtivaMotoristaAsync(Guid motoristaId, DateTime data)
            => _db.Set<FrotiX.Models.FolgaRecesso>()
                  .FirstOrDefaultAsync(x => x.MotoristaId == motoristaId && x.Ativo && x.DataInicio <= data && x.DataFim >= data);
    }

    // ============================= Ferias =============================
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: FeriasRepository                                                               │
    // │ 📦 HERDA DE: Repository                                             │
    // │ 🔌 IMPLEMENTA: IFeriasRepository                                                         │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório do módulo de escalas: FeriasRepository.
    
    public class FeriasRepository : Repository<FrotiX.Models.Ferias>, IFeriasRepository
    {
        private readonly FrotiXDbContext _db;
        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: FeriasRepository                                                  │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Executar a operação definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // db - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // void - Sem retorno
        
        
        // Param db: Descrição do parâmetro.
        public FeriasRepository(FrotiXDbContext db) : base(db) => _db = db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                            │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualizar os dados da entidade correspondente.
        
        
        // 📥 PARÂMETROS:
        // ferias - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // void - Sem retorno
        
        
        // Param ferias: Descrição do parâmetro.
        public void Update(FrotiX.Models.Ferias ferias)
        {
            var set = _db.Set<FrotiX.Models.Ferias>();
            var obj = set.FirstOrDefault(x => x.FeriasId == ferias.FeriasId);
            if (obj != null)
            {
                obj.MotoristaId = ferias.MotoristaId;
                obj.DataInicio = ferias.DataInicio;
                obj.DataFim = ferias.DataFim;
                obj.Ativo = ferias.Ativo;
                obj.Observacoes = ferias.Observacoes;
                obj.DataAlteracao = DateTime.Now;
                obj.UsuarioIdAlteracao = ferias.UsuarioIdAlteracao;
            }
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetFeriasPorMotoristaAsync                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // motoristaId - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task> - Resultado da operação
        
        
        // Param motoristaId: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public Task<List<FrotiX.Models.Ferias>> GetFeriasPorMotoristaAsync(Guid motoristaId)
            => _db.Set<FrotiX.Models.Ferias>()
                  .Where(x => x.MotoristaId == motoristaId)
                  .OrderByDescending(x => x.DataInicio)
                  .ToListAsync();

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetFeriasAtivasAsync                                              │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // data - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task> - Resultado da operação
        
        
        // Param data: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public Task<List<FrotiX.Models.Ferias>> GetFeriasAtivasAsync(DateTime data)
            => _db.Set<FrotiX.Models.Ferias>()
                  .Where(x => x.Ativo && x.DataInicio <= data && x.DataFim >= data)
                  .ToListAsync();

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: MotoristaEstaEmFeriasAsync                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Executar a operação definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // motoristaId - Descrição do parâmetro
        // data - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task - Resultado da operação
        
        
        // Param motoristaId: Descrição do parâmetro.
        // Param data: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public Task<bool> MotoristaEstaEmFeriasAsync(Guid motoristaId, DateTime data)
            => _db.Set<FrotiX.Models.Ferias>()
                  .AnyAsync(x => x.MotoristaId == motoristaId && x.Ativo && x.DataInicio <= data && x.DataFim >= data);

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetFeriasAtivaMotoristaAsync                                      │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // motoristaId - Descrição do parâmetro
        // data - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task - Resultado da operação
        
        
        // Param motoristaId: Descrição do parâmetro.
        // Param data: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public Task<FrotiX.Models.Ferias> GetFeriasAtivaMotoristaAsync(Guid motoristaId, DateTime data)
            => _db.Set<FrotiX.Models.Ferias>()
                  .FirstOrDefaultAsync(x => x.MotoristaId == motoristaId && x.Ativo && x.DataInicio <= data && x.DataFim >= data);

        // Sem a coluna de substituto no modelo, retornamos as férias ativas na data.
        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetFeriasSemSubstitutoAsync                                       │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // data - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task> - Resultado da operação
        
        
        // Param data: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public Task<List<FrotiX.Models.Ferias>> GetFeriasSemSubstitutoAsync(DateTime data)
            => _db.Set<FrotiX.Models.Ferias>()
                  .Where(x => x.Ativo && x.DataInicio <= data && x.DataFim >= data)
                  .ToListAsync();
    }

    // ========================== CoberturaFolga ==========================
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: CoberturaFolgaRepository                                                       │
    // │ 📦 HERDA DE: Repository                                     │
    // │ 🔌 IMPLEMENTA: ICoberturaFolgaRepository                                                 │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório do módulo de escalas: CoberturaFolgaRepository.
    
    public class CoberturaFolgaRepository : Repository<FrotiX.Models.CoberturaFolga>, ICoberturaFolgaRepository
    {
        private readonly FrotiXDbContext _db;
        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: CoberturaFolgaRepository                                          │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Executar a operação definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // db - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // void - Sem retorno
        
        
        // Param db: Descrição do parâmetro.
        public CoberturaFolgaRepository(FrotiXDbContext db) : base(db) => _db = db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                            │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualizar os dados da entidade correspondente.
        
        
        // 📥 PARÂMETROS:
        // coberturaFolga - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // void - Sem retorno
        
        
        // Param coberturaFolga: Descrição do parâmetro.
        public void Update(FrotiX.Models.CoberturaFolga coberturaFolga)
        {
            var set = _db.Set<FrotiX.Models.CoberturaFolga>();
            var obj = set.FirstOrDefault(x => x.CoberturaId == coberturaFolga.CoberturaId);
            if (obj != null)
            {
                obj.MotoristaFolgaId = coberturaFolga.MotoristaFolgaId;
                obj.MotoristaCoberturaId = coberturaFolga.MotoristaCoberturaId;
                obj.DataInicio = coberturaFolga.DataInicio;
                obj.DataFim = coberturaFolga.DataFim;
                obj.Ativo = coberturaFolga.Ativo;
                obj.Observacoes = coberturaFolga.Observacoes;
                obj.DataAlteracao = DateTime.Now;
                obj.UsuarioIdAlteracao = coberturaFolga.UsuarioIdAlteracao;
            }
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetCoberturasAtivasAsync                                          │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // data - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task> - Resultado da operação
        
        
        // Param data: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public Task<List<FrotiX.Models.CoberturaFolga>> GetCoberturasAtivasAsync(DateTime data)
            => _db.Set<FrotiX.Models.CoberturaFolga>()
                  .Where(x => x.Ativo && x.DataInicio <= data && x.DataFim >= data)
                  .ToListAsync();

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetCoberturaPorMotoristaAsync                                     │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // motoristaCoberturaId - Descrição do parâmetro
        // data - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task - Resultado da operação
        
        
        // Param motoristaCoberturaId: Descrição do parâmetro.
        // Param data: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public Task<FrotiX.Models.CoberturaFolga> GetCoberturaPorMotoristaAsync(Guid motoristaCoberturaId, DateTime data)
            => _db.Set<FrotiX.Models.CoberturaFolga>()
                  .FirstOrDefaultAsync(x => x.MotoristaCoberturaId == motoristaCoberturaId && x.Ativo && x.DataInicio <= data && x.DataFim >= data);

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: MotoristaEstaCobridoAsync                                         │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Executar a operação definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // motoristaFolgaId - Descrição do parâmetro
        // data - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task - Resultado da operação
        
        
        // Param motoristaFolgaId: Descrição do parâmetro.
        // Param data: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public Task<bool> MotoristaEstaCobridoAsync(Guid motoristaFolgaId, DateTime data)
            => _db.Set<FrotiX.Models.CoberturaFolga>()
                  .AnyAsync(x => x.MotoristaFolgaId == motoristaFolgaId && x.Ativo && x.DataInicio <= data && x.DataFim >= data);

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetCoberturaMotoristaAsync                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // motoristaFolgaId - Descrição do parâmetro
        // data - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task - Resultado da operação
        
        
        // Param motoristaFolgaId: Descrição do parâmetro.
        // Param data: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public Task<FrotiX.Models.CoberturaFolga> GetCoberturaMotoristaAsync(Guid motoristaFolgaId, DateTime data)
            => _db.Set<FrotiX.Models.CoberturaFolga>()
                  .FirstOrDefaultAsync(x => x.MotoristaFolgaId == motoristaFolgaId && x.Ativo && x.DataInicio <= data && x.DataFim >= data);

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetHistoricoCoberturas                                            │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // motoristaId - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task> - Resultado da operação
        
        
        // Param motoristaId: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public Task<List<FrotiX.Models.CoberturaFolga>> GetHistoricoCoberturas(Guid motoristaId)
            => _db.Set<FrotiX.Models.CoberturaFolga>()
                  .Where(x => x.MotoristaFolgaId == motoristaId || x.MotoristaCoberturaId == motoristaId)
                  .OrderByDescending(x => x.DataInicio)
                  .ToListAsync();
    }

    // ======================== ObservacoesEscala ========================
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ObservacoesEscalaRepository                                                    │
    // │ 📦 HERDA DE: Repository                                  │
    // │ 🔌 IMPLEMENTA: IObservacoesEscalaRepository                                              │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório do módulo de escalas: ObservacoesEscalaRepository.
    
    public class ObservacoesEscalaRepository : Repository<FrotiX.Models.ObservacoesEscala>, IObservacoesEscalaRepository
    {
        private readonly FrotiXDbContext _db;
        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ObservacoesEscalaRepository                                       │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Executar a operação definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // db - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // void - Sem retorno
        
        
        // Param db: Descrição do parâmetro.
        public ObservacoesEscalaRepository(FrotiXDbContext db) : base(db) => _db = db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                            │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualizar os dados da entidade correspondente.
        
        
        // 📥 PARÂMETROS:
        // observacaoEscala - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // void - Sem retorno
        
        
        // Param observacaoEscala: Descrição do parâmetro.
        public void Update(FrotiX.Models.ObservacoesEscala observacaoEscala)
        {
            var set = _db.Set<FrotiX.Models.ObservacoesEscala>();
            var obj = set.FirstOrDefault(x => x.ObservacaoId == observacaoEscala.ObservacaoId);
            if (obj != null)
            {
                obj.DataEscala = observacaoEscala.DataEscala;
                obj.Titulo = observacaoEscala.Titulo;
                obj.Descricao = observacaoEscala.Descricao;
                obj.Ativo = observacaoEscala.Ativo;
                obj.DataAlteracao = DateTime.Now;
                obj.UsuarioIdAlteracao = observacaoEscala.UsuarioIdAlteracao;
            }
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetObservacoesAtivasAsync                                         │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // data - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task> - Resultado da operação
        
        
        // Param data: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public Task<List<FrotiX.Models.ObservacoesEscala>> GetObservacoesAtivasAsync(DateTime data)
            => _db.Set<FrotiX.Models.ObservacoesEscala>()
                  .Where(x => x.Ativo && x.DataEscala.Date == data.Date)
                  .OrderBy(x => x.DataEscala)
                  .ToListAsync();

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetObservacoesPorPeriodoAsync                                     │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // dataInicio - Descrição do parâmetro
        // dataFim - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task> - Resultado da operação
        
        
        // Param dataInicio: Descrição do parâmetro.
        // Param dataFim: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public Task<List<FrotiX.Models.ObservacoesEscala>> GetObservacoesPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
            => _db.Set<FrotiX.Models.ObservacoesEscala>()
                  .Where(x => x.DataEscala.Date >= dataInicio.Date && x.DataEscala.Date <= dataFim.Date)
                  .OrderBy(x => x.DataEscala)
                  .ToListAsync();

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ExisteObservacaoAsync                                             │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Verificar condição/regra de negócio e retornar o resultado.
        
        
        // 📥 PARÂMETROS:
        // data - Descrição do parâmetro
        // titulo - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task - Resultado da operação
        
        
        // Param data: Descrição do parâmetro.
        // Param titulo: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public Task<bool> ExisteObservacaoAsync(DateTime data, string titulo)
            => _db.Set<FrotiX.Models.ObservacoesEscala>()
                  .AnyAsync(x => x.DataEscala.Date == data.Date && x.Titulo == titulo);
    }

    // ===================== Views (consultas de leitura) =====================
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ViewEscalasCompletasRepository                                                 │
    // │ 📦 HERDA DE: Repository                               │
    // │ 🔌 IMPLEMENTA: IViewEscalasCompletasRepository                                           │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório do módulo de escalas: ViewEscalasCompletasRepository.
    
    public class ViewEscalasCompletasRepository : Repository<FrotiX.Models.ViewEscalasCompletas>, IViewEscalasCompletasRepository
    {
        private readonly FrotiXDbContext _db;
        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ViewEscalasCompletasRepository                                    │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Executar a operação definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // db - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // void - Sem retorno
        
        
        // Param db: Descrição do parâmetro.
        public ViewEscalasCompletasRepository(FrotiXDbContext db) : base(db) => _db = db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetAllAsync                                                       │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // Sem parâmetros.
        
        
        
        // 📤 RETORNO:
        // Task> - Resultado da operação
        
        
        // Returns: Descrição do retorno.
        public Task<List<FrotiX.Models.ViewEscalasCompletas>> GetAllAsync()
            => _db.Set<FrotiX.Models.ViewEscalasCompletas>()
                  .OrderBy(x => x.DataEscala)
                  .ThenBy(x => x.HoraInicio)
                  .ToListAsync();

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetPaginatedAsync                                                 │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // Items - Descrição do parâmetro
        // TotalCount - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // void - Sem retorno
        
        
        // Param Items: Descrição do parâmetro.
        // Param TotalCount: Descrição do parâmetro.
        public async Task<(List<FrotiX.Models.ViewEscalasCompletas> Items, int TotalCount)> GetPaginatedAsync(
            Expression<Func<FrotiX.Models.ViewEscalasCompletas, bool>> filter,
            int page,
            int pageSize)
        {
            var query = _db.Set<FrotiX.Models.ViewEscalasCompletas>().Where(filter);
            var total = await query.CountAsync();
            var items = await query
                .OrderBy(x => x.DataEscala)
                .ThenBy(x => x.HoraInicio)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (items, total);
        }
    }

    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ViewMotoristasVezRepository                                                    │
    // │ 📦 HERDA DE: Repository                                  │
    // │ 🔌 IMPLEMENTA: IViewMotoristasVezRepository                                              │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório do módulo de escalas: ViewMotoristasVezRepository.
    
    public class ViewMotoristasVezRepository : Repository<FrotiX.Models.ViewMotoristasVez>, IViewMotoristasVezRepository
    {
        private readonly FrotiXDbContext _db;
        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ViewMotoristasVezRepository                                       │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Executar a operação definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // db - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // void - Sem retorno
        
        
        // Param db: Descrição do parâmetro.
        public ViewMotoristasVezRepository(FrotiXDbContext db) : base(db) => _db = db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetTopMotoristasAsync                                             │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // quantidade - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task> - Resultado da operação
        
        
        // Param quantidade: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public Task<List<FrotiX.Models.ViewMotoristasVez>> GetTopMotoristasAsync(int quantidade = 5)
            => _db.Set<FrotiX.Models.ViewMotoristasVez>()
                  .OrderBy(x => x.NumeroSaidas)
                  .ThenBy(x => x.HoraInicio)
                  .Take(quantidade)
                  .ToListAsync();
    }

    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ViewStatusMotoristasRepository                                                 │
    // │ 📦 HERDA DE: Repository                               │
    // │ 🔌 IMPLEMENTA: IViewStatusMotoristasRepository                                           │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório do módulo de escalas: ViewStatusMotoristasRepository.
    
    public class ViewStatusMotoristasRepository : Repository<FrotiX.Models.ViewStatusMotoristas>, IViewStatusMotoristasRepository
    {
        private readonly FrotiXDbContext _db;
        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ViewStatusMotoristasRepository                                    │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Executar a operação definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // db - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // void - Sem retorno
        
        
        // Param db: Descrição do parâmetro.
        public ViewStatusMotoristasRepository(FrotiXDbContext db) : base(db) => _db = db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetStatusAtualizadoAsync                                          │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // Sem parâmetros.
        
        
        
        // 📤 RETORNO:
        // Task> - Resultado da operação
        
        
        // Returns: Descrição do retorno.
        public Task<List<FrotiX.Models.ViewStatusMotoristas>> GetStatusAtualizadoAsync()
            => _db.Set<FrotiX.Models.ViewStatusMotoristas>()
                  .OrderBy(x => x.Nome)
                  .ToListAsync();

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetStatusMotoristaAsync                                           │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext, LINQ                                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter dados conforme a consulta definida pelo método.
        
        
        // 📥 PARÂMETROS:
        // motoristaId - Descrição do parâmetro
        
        
        
        // 📤 RETORNO:
        // Task - Resultado da operação
        
        
        // Param motoristaId: Descrição do parâmetro.
        // Returns: Descrição do retorno.
        public Task<FrotiX.Models.ViewStatusMotoristas> GetStatusMotoristaAsync(Guid motoristaId)
            => _db.Set<FrotiX.Models.ViewStatusMotoristas>()
                  .FirstOrDefaultAsync(x => x.MotoristaId == motoristaId);
    }
}
