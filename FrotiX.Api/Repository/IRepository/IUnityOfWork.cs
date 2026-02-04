/* ****************************************************************************************
 * ⚡ ARQUIVO: IUnitOfWork.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Interface principal (Unit of Work pattern) que coordena e gerencia
 *                   todos os repositories de domínio, abstração transacional e persistência
 *                   de dados
 *
 * 📥 ENTRADAS     : Inicialização de transações, scope da sessão de banco de dados
 *
 * 📤 SAÍDAS       : Gerenciador central de acesso aos repositories, métodos Save() e
 *                   SaveAsync() para persistência
 *
 * 🔗 CHAMADA POR  : Controllers, Services (padrão Dependency Injection)
 *
 * 🔄 CHAMA        : Todas as interfaces IRepository especializadas (Abastecimento,
 *                   Contrato, Veiculo, Viagem, View repositories, etc.)
 *
 * 📦 DEPENDÊNCIAS : IRepository[T], IDisposable, System.Threading.Tasks, Models
 *
 * 📝 OBSERVAÇÕES  : Interface PARCIAL - tem extensões em arquivos de padrão:
 *                   - IUnitOfWork.OcorrenciaViagem.cs (propriedades OcorrenciaViagem)
 *                   - IUnitOfWork.RepactuacaoVeiculo.cs (propriedades Repactuação)
 *                   Coordena transações ACID e garante consistência de dados
 **************************************************************************************** */

using System;
using System.Threading.Tasks;

namespace FrotiXApi.Repository.IRepository
{
    public partial interface IUnitOfWork : IDisposable
    {
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

        IViagemRepository Viagem
        {
            get;
        }

        IViagensEconomildoRepository ViagensEconomildo
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

        void Save();

        Task SaveAsync();
    }
}
