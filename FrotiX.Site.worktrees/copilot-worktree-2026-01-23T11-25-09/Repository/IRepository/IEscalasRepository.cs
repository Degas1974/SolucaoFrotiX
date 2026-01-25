using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FrotiX.Models;    
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository.IRepository
{
    // Interface para TipoServico
    public interface ITipoServicoRepository : IRepository<TipoServico>
    {
        IEnumerable<SelectListItem> GetTipoServicoListForDropDown();
        void Update(TipoServico tipoServico);
        Task<bool> ExisteNomeServicoAsync(string nomeServico, Guid? excludeId = null);
    }

    // Interface para Turno
    public interface ITurnoRepository : IRepository<Turno>
    {
        IEnumerable<SelectListItem> GetTurnoListForDropDown();
        void Update(Turno turno);
        Task<Turno> GetTurnoByNomeAsync(string nomeTurno);
        Task<bool> VerificarConflitoHorarioAsync(TimeSpan horaInicio, TimeSpan horaFim, Guid? excludeId = null);
    }

    // Interface para VAssociado
    public interface IVAssociadoRepository : IRepository<VAssociado>
    {
        void Update(VAssociado vAssociado);
        Task<VAssociado> GetAssociacaoAtivaAsync(Guid motoristaId);
        Task<List<VAssociado>> GetHistoricoAssociacoesAsync(Guid motoristaId);
        Task<bool> MotoristaTemVeiculoAsync(Guid motoristaId, DateTime data);
        Task<VAssociado> GetAssociacaoPorDataAsync(Guid motoristaId, DateTime data);
    }

    // Interface para EscalaDiaria
    public interface IEscalaDiariaRepository : IRepository<EscalaDiaria>
    {
        void Update(EscalaDiaria escalaDiaria);
        
        // MÃ©todos de consulta
        Task<List<ViewEscalasCompletas>> GetEscalasCompletasAsync(DateTime? data = null);
        Task<ViewEscalasCompletas> GetEscalaCompletaByIdAsync(Guid id);
        Task<List<ViewEscalasCompletas>> GetEscalasPorFiltroAsync(FiltroEscalaViewModel filtro);
        
        // Motoristas da vez
        Task<List<ViewMotoristasVez>> GetMotoristasVezAsync(int quantidade = 5);
        
        // Status dos motoristas
        Task<List<ViewStatusMotoristas>> GetStatusMotoristasAsync();
        Task<bool> AtualizarStatusMotoristaAsync(Guid motoristaId, string novoStatus, DateTime? data = null);
        
        // Contador de viagens
        Task<bool> IncrementarContadorViagemAsync(Guid motoristaId, DateTime data);
        
        // VerificaÃ§Ãµes
        Task<bool> MotoristaDisponivelAsync(Guid motoristaId, DateTime data, TimeSpan horaInicio, TimeSpan horaFim);
        Task<bool> VeiculoDisponivelAsync(Guid veiculoId, DateTime data, TimeSpan horaInicio, TimeSpan horaFim);
        Task<bool> ExisteEscalaConflitanteAsync(Guid motoristaId, DateTime data, TimeSpan horaInicio, TimeSpan horaFim, Guid? excludeId = null);
        
        // Escalas por perÃ­odo
        Task<List<EscalaDiaria>> GetEscalasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
        Task<List<EscalaDiaria>> GetEscalasMotoristaAsync(Guid motoristaId, DateTime? data = null);
    }

    // Interface para FolgaRecesso
    public interface IFolgaRecessoRepository : IRepository<FolgaRecesso>
    {
        void Update(FolgaRecesso folgaRecesso);
        Task<List<FolgaRecesso>> GetFolgasPorMotoristaAsync(Guid motoristaId);
        Task<List<FolgaRecesso>> GetFolgasAtivasAsync(DateTime data);
        Task<bool> MotoristaEstaEmFolgaAsync(Guid motoristaId, DateTime data);
        Task<FolgaRecesso> GetFolgaAtivaMotoristaAsync(Guid motoristaId, DateTime data);
    }

    // Interface para Ferias
    public interface IFeriasRepository : IRepository<Ferias>
    {
        void Update(Ferias ferias);
        Task<List<Ferias>> GetFeriasPorMotoristaAsync(Guid motoristaId);
        Task<List<Ferias>> GetFeriasAtivasAsync(DateTime data);
        Task<bool> MotoristaEstaEmFeriasAsync(Guid motoristaId, DateTime data);
        Task<Ferias> GetFeriasAtivaMotoristaAsync(Guid motoristaId, DateTime data);
        Task<List<Ferias>> GetFeriasSemSubstitutoAsync(DateTime data);
    }

    // Interface para CoberturaFolga
    public interface ICoberturaFolgaRepository : IRepository<CoberturaFolga>
    {
        void Update(CoberturaFolga coberturaFolga);
        Task<List<CoberturaFolga>> GetCoberturasAtivasAsync(DateTime data);
        Task<CoberturaFolga> GetCoberturaPorMotoristaAsync(Guid motoristaCobertorId, DateTime data);
        Task<bool> MotoristaEstaCobridoAsync(Guid motoristaFolgaId, DateTime data);
        Task<CoberturaFolga> GetCoberturaMotoristaAsync(Guid motoristaFolgaId, DateTime data);
        Task<List<CoberturaFolga>> GetHistoricoCoberturas(Guid motoristaId);
    }

    // Interface para ObservacoesEscala
    public interface IObservacoesEscalaRepository : IRepository<ObservacoesEscala>
    {
        void Update(ObservacoesEscala observacaoEscala);
        Task<List<ObservacoesEscala>> GetObservacoesAtivasAsync(DateTime data);
        Task<List<ObservacoesEscala>> GetObservacoesPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
        Task<bool> ExisteObservacaoAsync(DateTime data, string titulo);
    }

    // Interface para Views
    public interface IViewEscalasCompletasRepository : IRepository<ViewEscalasCompletas>
    {
        Task<List<ViewEscalasCompletas>> GetAllAsync();
        Task<(List<ViewEscalasCompletas> Items, int TotalCount)> GetPaginatedAsync(
            Expression<Func<ViewEscalasCompletas, bool>> filter,
            int page,
            int pageSize
        );
    }

    public interface IViewMotoristasVezRepository : IRepository<ViewMotoristasVez>
    {
        Task<List<ViewMotoristasVez>> GetTopMotoristasAsync(int quantidade = 5);
    }

    public interface IViewStatusMotoristasRepository : IRepository<ViewStatusMotoristas>
    {
        Task<List<ViewStatusMotoristas>> GetStatusAtualizadoAsync();
        Task<ViewStatusMotoristas> GetStatusMotoristaAsync(Guid motoristaId);
    }
}
