// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ ARQUIVO    : IUnitOfWork.cs                                                  ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ FINALIDADE : Interface central do padrão Unit of Work para FrotiX.           ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO FUNCIONAL                                                          ║
// ║ Interface IUnitOfWork que expõe propriedades para todos os repositórios:     ║
// ║ • Entidades principais: Veiculo, Motorista, Viagem, Contrato, Empenho, etc.  ║
// ║ • Views SQL: ViewMotoristas, ViewViagens, ViewAbastecimentos, etc.           ║
// ║ • Sistema de Escalas: TipoServico, Turno, EscalaDiaria, etc.                 ║
// ║ • Sistema de Alertas: AlertasFrotiX, AlertasUsuario                          ║
// ║ • GetDbContext(): Acesso direto ao DbContext se necessário                   ║
// ║ • Save/SaveAsync: Persiste todas as alterações no banco via DbContext        ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ IMPLEMENTAÇÃO: UnitOfWork.cs + partials                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ LOTE        : 24 — Repository/IRepository                                    ║
// ║ DATA        : 29/01/2026                                                     ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using FrotiX.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FrotiX.Repository.IRepository
{
    public partial interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Retorna o DbContext para operações avançadas (ChangeTracker, etc.)
        /// </summary>
        DbContext GetDbContext();

        IAbastecimentoRepository Abastecimento
        {
            get;
        }

        IAspNetUsersRepository AspNetUsers
        {
            get;
        }

        IAtaRegistroPrecosRepository AtaRegistroPrecos
        {
            get;
        }

        ICombustivelRepository Combustivel
        {
            get;
        }

        IContratoRepository Contrato
        {
            get;
        }

        IControleAcessoRepository ControleAcesso
        {
            get;
        }

        ICorridasCanceladasTaxiLegRepository CorridasCanceladasTaxiLeg
        {
            get;
        }

        ICorridasTaxiLegRepository CorridasTaxiLeg
        {
            get;
        }

        IRepository<AbastecimentoPendente> AbastecimentoPendente { get; }

        ICustoMensalItensContratoRepository CustoMensalItensContrato
        {
            get;
        }

        IEmpenhoRepository Empenho
        {
            get;
        }

        IEmpenhoMultaRepository EmpenhoMulta
        {
            get;
        }

        // ============================================================
        // ENCARREGADO - NOVO
        // ============================================================
        IEncarregadoRepository Encarregado
        {
            get;
        }

        IEncarregadoContratoRepository EncarregadoContrato
        {
            get;
        }

        IEventoRepository Evento
        {
            get;
        }

        IFornecedorRepository Fornecedor
        {
            get;
        }

        IItemVeiculoAtaRepository ItemVeiculoAta
        {
            get;
        }

        IItemVeiculoContratoRepository ItemVeiculoContrato
        {
            get;
        }

        IItensManutencaoRepository ItensManutencao
        {
            get;
        }

        ILavadorRepository Lavador
        {
            get;
        }

        ILavadorContratoRepository LavadorContrato
        {
            get;
        }

        ILavadoresLavagemRepository LavadoresLavagem
        {
            get;
        }

        ILavagemRepository Lavagem
        {
            get;
        }

        ILotacaoMotoristaRepository LotacaoMotorista
        {
            get;
        }

        IManutencaoRepository Manutencao
        {
            get;
        }

        IMarcaVeiculoRepository MarcaVeiculo
        {
            get;
        }

        IMediaCombustivelRepository MediaCombustivel
        {
            get;
        }

        IModeloVeiculoRepository ModeloVeiculo
        {
            get;
        }

        IMotoristaRepository Motorista
        {
            get;
        }

        IMotoristaContratoRepository MotoristaContrato
        {
            get;
        }

        IMovimentacaoEmpenhoRepository MovimentacaoEmpenho
        {
            get;
        }

        IMovimentacaoEmpenhoMultaRepository MovimentacaoEmpenhoMulta
        {
            get;
        }

        IMovimentacaoPatrimonioRepository MovimentacaoPatrimonio
        {
            get;
        }

        IMultaRepository Multa
        {
            get;
        }

        INotaFiscalRepository NotaFiscal
        {
            get;
        }

        IOperadorRepository Operador
        {
            get;
        }

        IOperadorContratoRepository OperadorContrato
        {
            get;
        }

        IOrgaoAutuanteRepository OrgaoAutuante
        {
            get;
        }

        IPatrimonioRepository Patrimonio
        {
            get;
        }

        IPlacaBronzeRepository PlacaBronze
        {
            get;
        }

        IRecursoRepository Recurso
        {
            get;
        }

        IRegistroCupomAbastecimentoRepository RegistroCupomAbastecimento
        {
            get;
        }

        IRepactuacaoAtaRepository RepactuacaoAta
        {
            get;
        }

        IRepactuacaoContratoRepository RepactuacaoContrato
        {
            get;
        }

        IRepactuacaoServicosRepository RepactuacaoServicos
        {
            get;
        }

        IRepactuacaoTerceirizacaoRepository RepactuacaoTerceirizacao
        {
            get;
        }

        IRequisitanteRepository Requisitante
        {
            get;
        }

        ISecaoPatrimonialRepository SecaoPatrimonial
        {
            get;
        }

        ISetorPatrimonialRepository SetorPatrimonial
        {
            get;
        }

        ISetorSolicitanteRepository SetorSolicitante
        {
            get;
        }

        ITipoMultaRepository TipoMulta
        {
            get;
        }

        IUnidadeRepository Unidade
        {
            get;
        }

        IVeiculoRepository Veiculo
        {
            get;
        }

        IVeiculoAtaRepository VeiculoAta
        {
            get;
        }

        IVeiculoContratoRepository VeiculoContrato
        {
            get;
        }

        IVeiculoPadraoViagemRepository VeiculoPadraoViagem
        {
            get;
        }

        IViagemRepository Viagem
        {
            get;
        }

        IViagensEconomildoRepository ViagensEconomildo
        {
            get;
        }

        IViagemEstatisticaRepository ViagemEstatistica
        {
            get;
        }

        IViewAbastecimentosRepository ViewAbastecimentos
        {
            get;
        }

        IViewAtaFornecedorRepository ViewAtaFornecedor
        {
            get;
        }

        IViewContratoFornecedorRepository ViewContratoFornecedor
        {
            get;
        }

        IViewControleAcessoRepository ViewControleAcesso
        {
            get;
        }

        IViewCustosViagemRepository ViewCustosViagem
        {
            get;
        }

        IViewEmpenhoMultaRepository ViewEmpenhoMulta
        {
            get;
        }

        IViewEmpenhosRepository ViewEmpenhos
        {
            get;
        }

        IViewEventosRepository ViewEventos
        {
            get;
        }

        IViewExisteItemContratoRepository ViewExisteItemContrato
        {
            get;
        }

        IViewFluxoEconomildoRepository ViewFluxoEconomildo
        {
            get;
        }

        IViewFluxoEconomildoDataRepository ViewFluxoEconomildoData
        {
            get;
        }

        IViewItensManutencaoRepository ViewItensManutencao
        {
            get;
        }

        IViewLavagemRepository ViewLavagem
        {
            get;
        }

        IViewLotacaoMotoristaRepository ViewLotacaoMotorista
        {
            get;
        }

        IViewLotacoesRepository ViewLotacoes
        {
            get;
        }

        IViewManutencaoRepository ViewManutencao
        {
            get;
        }

        IViewMediaConsumoRepository ViewMediaConsumo
        {
            get;
        }

        IViewMotoristaFluxoRepository ViewMotoristaFluxo
        {
            get;
        }

        IViewMotoristasRepository ViewMotoristas
        {
            get;
        }

        IViewMotoristasViagemRepository ViewMotoristasViagem
        {
            get;
        }

        IviewMultasRepository viewMultas
        {
            get;
        }

        IViewNoFichaVistoriaRepository ViewNoFichaVistoria
        {
            get;
        }

        IViewOcorrenciaRepository ViewOcorrencia
        {
            get;
        }

        IViewPatrimonioConferenciaRepository ViewPatrimonioConferencia
        {
            get;
        }

        IViewPendenciasManutencaoRepository ViewPendenciasManutencao
        {
            get;
        }

        IViewProcuraFichaRepository ViewProcuraFicha
        {
            get;
        }

        IViewRequisitantesRepository ViewRequisitantes
        {
            get;
        }

        IViewSetoresRepository ViewSetores
        {
            get;
        }

        IViewVeiculosRepository ViewVeiculos
        {
            get;
        }

        IViewVeiculosManutencaoRepository ViewVeiculosManutencao
        {
            get;
        }

        IViewVeiculosManutencaoReservaRepository ViewVeiculosManutencaoReserva
        {
            get;
        }

        IViewViagensRepository ViewViagens
        {
            get;
        }

        IViewViagensAgendaRepository ViewViagensAgenda
        {
            get;
        }

        IViewViagensAgendaTodosMesesRepository ViewViagensAgendaTodosMeses
        {
            get;
        }

        IViewGlosaRepository ViewGlosa
        {
            get;
        }

        // Repositórios para o sistema de Alertas
        IAlertasFrotiXRepository AlertasFrotiX
        {
            get;
        }

        IAlertasUsuarioRepository AlertasUsuario
        {
            get;
        }

        //Repositório para o sistema de Escalas

        ITipoServicoRepository TipoServico
        {
            get;
        }

        ITurnoRepository Turno
        {
            get;
        }

        IVAssociadoRepository VAssociado
        {
            get;
        }

        IEscalaDiariaRepository EscalaDiaria
        {
            get;
        }

        IFolgaRecessoRepository FolgaRecesso
        {
            get;
        }

        IFeriasRepository Ferias
        {
            get;
        }

        ICoberturaFolgaRepository CoberturaFolga
        {
            get;
        }

        IObservacoesEscalaRepository ObservacoesEscala
        {
            get;
        }

        IViewEscalasCompletasRepository ViewEscalasCompletas
        {
            get;
        }

        IViewMotoristasVezRepository ViewMotoristasVez
        {
            get;
        }

        IViewStatusMotoristasRepository ViewStatusMotoristas
        {
            get;
        }

        void Save();

        Task SaveAsync();
    }
}
