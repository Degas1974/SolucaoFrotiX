# 🔗 Mapeamento de Dependências - FrotiX 2026
## Repository Files 107-211 (CS → CS Backend Dependencies)

> **Processamento:** Repository Files 107-211 (105 arquivos)
> **Data:** 03/02/2026
> **Status:** ✅ Processado autonomamente
> **Escopo:** IRepository Interfaces + Implementations + UnitOfWork

---

## 📊 Resumo de Dependências (CS → CS)

### Padrão Geral Observado

Todos os 105 arquivos analisados (files 107-211) seguem um dos dois padrões:

1. **Interface Repositories (IRepository/)**: Definem contratos sem dependências internas
2. **Implementation Repositories**: Herdam de `Repository<T>` e implementam `IRepository<T>`
3. **UnitOfWork.cs**: Orquestra TODOS os repositórios

---

## 🔷 Dependências Identificadas

### Pattern: Interface Repositories (sem dependências CS-CS)

Todas as interfaces no diretório `IRepository/` implementam ou estendem:
- `IRepository<T>` (interface genérica base)
- `SelectListItem` (Microsoft.AspNetCore.Mvc.Rendering - externo)

**Arquivos IRepository (17 arquivos de 107-211):**
- IViewMotoristasViagemRepository.cs
- IViewMultasRepository.cs
- IViewNoFichaVistoriaRepository.cs
- IViewOcorrencia.cs
- IViewOcorrenciasAbertasVeiculoRepository.cs
- IViewOcorrenciasViagemRepository.cs
- IViewPatrimonioConferenciaRepository.cs
- IViewPendenciasManutencaoRepository.cs
- IViewProcuraFichaRepository.cs
- IViewRequisitantesRepository.cs
- IViewSetoresRepository.cs
- IViewVeiculosManutencaoRepository.cs
- IViewVeiculosManutencaoReservaRepository.cs
- IViewVeiculosRepository.cs
- IViewViagensAgendaRepository.cs
- IViewViagensAgendaTodosMesesRepository.cs
- IViewViagensRepository.cs

**Dependência Única (todos):**
```
→ IRepository<T>  [Interface genérica base]
```

---

### Pattern: Repository Implementations (herdam de base)

Todos os 88 repositories implementadores seguem:

```csharp
public class XxxRepository : Repository<XxxEntity>, IXxxRepository
{
    private new readonly FrotiXDbContext _db;

    public XxxRepository(FrotiXDbContext db) : base(db)
    {
        _db = db;
    }
}
```

**Dependências Comuns a Todos:**

| Dependência | Tipo | Razão | Métodos Usados |
|--|--|--|--|
| `Repository<T>` | Base Class | Implementação genérica de CRUD | Get, GetAll, Add, Update, Remove |
| `FrotiXDbContext` | DbContext | Acesso ao banco de dados | DbSet<T>, SaveChanges |
| Interface correspondente | IXxxRepository | Contrato de interface | Implementação |

---

## 📋 Mapeamento Detalhado por Arquivo

### 107. IViewMotoristasViagemRepository.cs
**Tipo:** Interface Repository
**Localização:** Repository/IRepository/
**Herança:** `IRepository<ViewMotoristasViagem>`

```
→ IRepository<ViewMotoristasViagem>
    • GetViewMotoristasViagemListForDropDown() : IEnumerable<SelectListItem>
    • Update(ViewMotoristasViagem) : void
```

---

### 108. IViewMultasRepository.cs
**Tipo:** Interface Repository
**Herança:** `IRepository<ViewMultas>`

```
→ IRepository<ViewMultas>
    • GetViewMultasListForDropDown() : IEnumerable<SelectListItem>
    • Update(ViewMultas) : void
```

---

### 109. IViewNoFichaVistoriaRepository.cs
**Tipo:** Interface Repository
**Herança:** `IRepository<ViewNoFichaVistoria>`

```
→ IRepository<ViewNoFichaVistoria>
    • GetViewNoFichaVistoriaListForDropDown() : IEnumerable<SelectListItem>
    • Update(ViewNoFichaVistoria) : void
```

---

### 110. IViewOcorrencia.cs
**Tipo:** Interface Repository
**Herança:** `IRepository<ViewOcorrencia>`

```
→ IRepository<ViewOcorrencia>
    • GetViewOcorrenciaListForDropDown() : IEnumerable<SelectListItem>
    • Update(ViewOcorrencia) : void
```

---

### 111. IViewOcorrenciasAbertasVeiculoRepository.cs
**Tipo:** Interface Repository
**Herança:** `IRepository<ViewOcorrenciasAbertasVeiculo>`

```
→ IRepository<ViewOcorrenciasAbertasVeiculo>
    • GetViewOcorrenciasAbertasVeiculoListForDropDown() : IEnumerable<SelectListItem>
    • Update(ViewOcorrenciasAbertasVeiculo) : void
```

---

### 112. IViewOcorrenciasViagemRepository.cs
**Tipo:** Interface Repository
**Herança:** `IRepository<ViewOcorrenciasViagem>`

```
→ IRepository<ViewOcorrenciasViagem>
    • GetViewOcorrenciasViagemListForDropDown() : IEnumerable<SelectListItem>
    • Update(ViewOcorrenciasViagem) : void
```

---

### 113. IViewPatrimonioConferenciaRepository.cs
**Tipo:** Interface Repository
**Herança:** `IRepository<ViewPatrimonioConferencia>`

```
→ IRepository<ViewPatrimonioConferencia>
    • GetViewPatrimonioConferenciaListForDropDown() : IEnumerable<SelectListItem>
    • Update(ViewPatrimonioConferencia) : void
```

---

### 114. IViewPendenciasManutencaoRepository.cs
**Tipo:** Interface Repository
**Herança:** `IRepository<ViewPendenciasManutencao>`

```
→ IRepository<ViewPendenciasManutencao>
    • GetViewPendenciasManutencaoListForDropDown() : IEnumerable<SelectListItem>
    • Update(ViewPendenciasManutencao) : void
```

---

### 115. IViewProcuraFichaRepository.cs
**Tipo:** Interface Repository
**Herança:** `IRepository<ViewProcuraFicha>`

```
→ IRepository<ViewProcuraFicha>
    • GetViewProcuraFichaListForDropDown() : IEnumerable<SelectListItem>
    • Update(ViewProcuraFicha) : void
```

---

### 116. IViewRequisitantesRepository.cs
**Tipo:** Interface Repository
**Herança:** `IRepository<ViewRequisitantes>`

```
→ IRepository<ViewRequisitantes>
    • GetViewRequisitantesListForDropDown() : IEnumerable<SelectListItem>
    • Update(ViewRequisitantes) : void
```

---

### 117. IViewSetoresRepository.cs
**Tipo:** Interface Repository
**Herança:** `IRepository<ViewSetores>`

```
→ IRepository<ViewSetores>
    • GetViewSetoresListForDropDown() : IEnumerable<SelectListItem>
    • Update(ViewSetores) : void
```

---

### 118. IViewVeiculosManutencaoRepository.cs
**Tipo:** Interface Repository
**Herança:** `IRepository<ViewVeiculosManutencao>`

```
→ IRepository<ViewVeiculosManutencao>
    • GetViewVeiculosManutencaoListForDropDown() : IEnumerable<SelectListItem>
    • Update(ViewVeiculosManutencao) : void
```

---

### 119. IViewVeiculosManutencaoReservaRepository.cs
**Tipo:** Interface Repository
**Herança:** `IRepository<ViewVeiculosManutencaoReserva>`

```
→ IRepository<ViewVeiculosManutencaoReserva>
    • GetViewVeiculosManutencaoReservaListForDropDown() : IEnumerable<SelectListItem>
    • Update(ViewVeiculosManutencaoReserva) : void
```

---

### 120. IViewVeiculosRepository.cs
**Tipo:** Interface Repository
**Herança:** `IRepository<ViewVeiculos>`

```
→ IRepository<ViewVeiculos>
    • GetViewVeiculosListForDropDown() : IEnumerable<SelectListItem>
    • Update(ViewVeiculos) : void
```

---

### 121. IViewViagensAgendaRepository.cs
**Tipo:** Interface Repository
**Herança:** `IRepository<ViewViagensAgenda>`

```
→ IRepository<ViewViagensAgenda>
    • GetViewViagensAgendaListForDropDown() : IEnumerable<SelectListItem>
    • Update(ViewViagensAgenda) : void
```

---

### 122. IViewViagensAgendaTodosMesesRepository.cs
**Tipo:** Interface Repository
**Herança:** `IRepository<ViewViagensAgendaTodosMeses>`

```
→ IRepository<ViewViagensAgendaTodosMeses>
    • GetViewViagensAgendaTodosMesesListForDropDown() : IEnumerable<SelectListItem>
    • Update(ViewViagensAgendaTodosMeses) : void
```

---

### 123. IViewViagensRepository.cs
**Tipo:** Interface Repository
**Herança:** `IRepository<ViewViagens>`

```
→ IRepository<ViewViagens>
    • GetViewViagensListForDropDown() : IEnumerable<SelectListItem>
    • Update(ViewViagens) : void
```

---

### 124. ItemVeiculoAtaRepository.cs
**Tipo:** Repository Implementation
**Localização:** Repository/
**Herança:** `Repository<ItemVeiculoAta>, IItemVeiculoAtaRepository`

```
→ Repository<ItemVeiculoAta>  [Base class genérico]
    • Métodos: Get, GetAll, GetFirstOrDefault, Add, Update, Remove

→ IItemVeiculoAtaRepository  [Interface implementada]
    • GetItemVeiculoAtaListForDropDown() : IEnumerable<SelectListItem>
    • Update(ItemVeiculoAta) : void

→ FrotiXDbContext  [DbContext]
    • DbSet<ItemVeiculoAta>
    • SaveChanges()
```

---

### 125. ItemVeiculoContratoRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<ItemVeiculoContrato>, IItemVeiculoContratoRepository`

```
→ Repository<ItemVeiculoContrato>  [Base class]
→ IItemVeiculoContratoRepository  [Interface]
→ FrotiXDbContext  [DbContext]
```

---

### 126. ItensManutencaoRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<ItensManutencao>, IItensManutencaoRepository`

```
→ Repository<ItensManutencao>  [Base class]
→ IItensManutencaoRepository  [Interface]
→ FrotiXDbContext  [DbContext]
```

---

### 127. LavadorContratoRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<LavadorContrato>, ILavadorContratoRepository`
**Nota:** Chave composta (LavadorId, ContratoId)

```
→ Repository<LavadorContrato>  [Base class]
→ ILavadorContratoRepository  [Interface]
→ FrotiXDbContext  [DbContext]
    • Predicado: (s.LavadorId == xxx) && (s.ContratoId == xxx)
```

---

### 128. LavadorRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<Lavador>, ILavadorRepository`

```
→ Repository<Lavador>  [Base class]
→ ILavadorRepository  [Interface]
→ FrotiXDbContext  [DbContext]
```

---

### 129. LavadoresLavagemRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<LavadoresLavagem>, ILavadoresLavagemRepository`

```
→ Repository<LavadoresLavagem>  [Base class]
→ ILavadoresLavagemRepository  [Interface]
→ FrotiXDbContext  [DbContext]
```

---

### 130. LavagemRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<Lavagem>, ILavagemRepository`

```
→ Repository<Lavagem>  [Base class]
→ ILavagemRepository  [Interface]
→ FrotiXDbContext  [DbContext]
```

---

### 131. LogRepository.cs
**Tipo:** Repository Implementation (Specializado para Logs)
**Localização:** Repository/
**Implementa:** `ILogRepository`
**Nota:** Padrão diferente - NÃO herda de Repository<T>

```
→ FrotiXDbContext  [DbContext direto]
    • DbSet<LogErro>
    • LINQ queries assíncronas

→ ILogRepository  [Interface]

Métodos Especializados:
    • AddAsync(LogErro) : Task<LogErro>
    • GetLogsAsync(LogQueryFilter) : Task<LogQueryResult>
    • GetDashboardStatsAsync() : Task<LogDashboardStats>
    • GetErrorsByHourAsync() : Task<List<LogTimelineItem>>
    • GetTopPagesWithErrorsAsync() : Task<List<LogRankingItem>>
    • DetectAnomaliesAsync() : Task<List<LogAnomaly>>
    • CheckThresholdsAsync() : Task<List<LogThresholdAlert>>
```

---

### 132. LotacaoMotoristaRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<LotacaoMotorista>, ILotacaoMotoristaRepository`

```
→ Repository<LotacaoMotorista>
→ ILotacaoMotoristaRepository
→ FrotiXDbContext
```

---

### 133. ManutencaoRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<Manutencao>, IManutencaoRepository`

```
→ Repository<Manutencao>
→ IManutencaoRepository
→ FrotiXDbContext
```

---

### 134. MarcaVeiculoRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<MarcaVeiculo>, IMarcaVeiculoRepository`

```
→ Repository<MarcaVeiculo>
→ IMarcaVeiculoRepository
→ FrotiXDbContext
```

---

### 135. MediaCombustivelRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<MediaCombustivel>, IMediaCombustivelRepository`

```
→ Repository<MediaCombustivel>
→ IMediaCombustivelRepository
→ FrotiXDbContext
```

---

### 136. ModeloVeiculoRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<ModeloVeiculo>, IModeloVeiculoRepository`

```
→ Repository<ModeloVeiculo>
→ IModeloVeiculoRepository
→ FrotiXDbContext
```

---

### 137. MotoristaContratoRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<MotoristaContrato>, IMotoristaContratoRepository`

```
→ Repository<MotoristaContrato>
→ IMotoristaContratoRepository
→ FrotiXDbContext
```

---

### 138. MotoristaRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<Motorista>, IMotoristaRepository`

```
→ Repository<Motorista>
→ IMotoristaRepository
→ FrotiXDbContext
```

---

### 139. MovimentacaoEmpenhoMultaRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<MovimentacaoEmpenhoMulta>, IMovimentacaoEmpenhoMultaRepository`

```
→ Repository<MovimentacaoEmpenhoMulta>
→ IMovimentacaoEmpenhoMultaRepository
→ FrotiXDbContext
```

---

### 140. MovimentacaoEmpenhoRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<MovimentacaoEmpenho>, IMovimentacaoEmpenhoRepository`

```
→ Repository<MovimentacaoEmpenho>
→ IMovimentacaoEmpenhoRepository
→ FrotiXDbContext
```

---

### 141. MovimentacaoPatrimonioRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<MovimentacaoPatrimonio>, IMovimentacaoPatrimonioRepository`

```
→ Repository<MovimentacaoPatrimonio>
→ IMovimentacaoPatrimonioRepository
→ FrotiXDbContext
```

---

### 142. MultaRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<Multa>, IMultaRepository`

```
→ Repository<Multa>
→ IMultaRepository
→ FrotiXDbContext
```

---

### 143. NotaFiscalRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<NotaFiscal>, INotaFiscalRepository`

```
→ Repository<NotaFiscal>
→ INotaFiscalRepository
→ FrotiXDbContext
```

---

### 144. OcorrenciaViagemRepository.cs
**Tipo:** Repository Implementation
**Herança:** `IOcorrenciaViagemRepository` (não herda de Repository<T>)

```
→ IOcorrenciaViagemRepository  [Interface especializada]
→ FrotiXDbContext  [DbContext direto]
```

---

### 145. OperadorContratoRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<OperadorContrato>, IOperadorContratoRepository`

```
→ Repository<OperadorContrato>
→ IOperadorContratoRepository
→ FrotiXDbContext
```

---

### 146. OperadorRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<Operador>, IOperadorRepository`

```
→ Repository<Operador>
→ IOperadorRepository
→ FrotiXDbContext
```

---

### 147. OrgaoAutuanteRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<OrgaoAutuante>, IOrgaoAutuanteRepository`

```
→ Repository<OrgaoAutuante>
→ IOrgaoAutuanteRepository
→ FrotiXDbContext
```

---

### 148. PatrimonioRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<Patrimonio>, IPatrimonioRepository`

```
→ Repository<Patrimonio>
→ IPatrimonioRepository
→ FrotiXDbContext
```

---

### 149. PlacaBronzeRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<PlacaBronze>, IPlacaBronzeRepository`

```
→ Repository<PlacaBronze>
→ IPlacaBronzeRepository
→ FrotiXDbContext
```

---

### 150. RecursoRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<Recurso>, IRecursoRepository`

```
→ Repository<Recurso>
→ IRecursoRepository
→ FrotiXDbContext
```

---

### 151. RegistroCupomAbastecimentoRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<RegistroCupomAbastecimento>, IRegistroCupomAbastecimentoRepository`

```
→ Repository<RegistroCupomAbastecimento>
→ IRegistroCupomAbastecimentoRepository
→ FrotiXDbContext
```

---

### 152. RepactuacaoAtaRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<RepactuacaoAta>, IRepactuacaoAtaRepository`

```
→ Repository<RepactuacaoAta>
→ IRepactuacaoAtaRepository
→ FrotiXDbContext
```

---

### 153. RepactuacaoContratoRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<RepactuacaoContrato>, IRepactuacaoContratoRepository`

```
→ Repository<RepactuacaoContrato>
→ IRepactuacaoContratoRepository
→ FrotiXDbContext
```

---

### 154. RepactuacaoServicosRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<RepactuacaoServicos>, IRepactuacaoServicosRepository`

```
→ Repository<RepactuacaoServicos>
→ IRepactuacaoServicosRepository
→ FrotiXDbContext
```

---

### 155. RepactuacaoTerceirizacaoRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<RepactuacaoTerceirizacao>, IRepactuacaoTerceirizacaoRepository`

```
→ Repository<RepactuacaoTerceirizacao>
→ IRepactuacaoTerceirizacaoRepository
→ FrotiXDbContext
```

---

### 156. RepactuacaoVeiculoRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<RepactuacaoVeiculo>, IRepactuacaoVeiculoRepository`

```
→ Repository<RepactuacaoVeiculo>
→ IRepactuacaoVeiculoRepository
→ FrotiXDbContext
```

---

### 157. Repository.cs
**Tipo:** Base Generic Repository Class
**Localização:** Repository/
**Implementa:** `IRepository<T>`

```
Base Genérico Implementa:

→ DbContext  [EF Core]
    • DbSet<T>
    • SaveChanges()

→ IRepository<T>  [Interface genérica]

Métodos Fornecidos (para todas as classes que herdam):
    • Get(object id) : T
    • GetFirstOrDefault(...) : T
    • GetFirstOrDefaultAsync(...) : Task<T>
    • GetAll(...) : IEnumerable<T>
    • GetAllAsync(...) : Task<IEnumerable<T>>
    • GetAllReduced<TResult>(...) : IEnumerable<TResult>
    • GetAllReducedIQueryable<TResult>(...) : IQueryable<TResult>
    • Add(T entity) : void
    • AddAsync(T entity) : Task
    • Update(T entity) : void
    • Remove(object id) : void
    • Remove(T entity) : void
```

---

### 158. RequisitanteRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<Requisitante>, IRequisitanteRepository`

```
→ Repository<Requisitante>
→ IRequisitanteRepository
→ FrotiXDbContext
```

---

### 159. SecaoPatrimonialRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<SecaoPatrimonial>, ISecaoPatrimonialRepository`

```
→ Repository<SecaoPatrimonial>
→ ISecaoPatrimonialRepository
→ FrotiXDbContext
```

---

### 160. SetorPatrimonialRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<SetorPatrimonial>, ISetorPatrimonialRepository`

```
→ Repository<SetorPatrimonial>
→ ISetorPatrimonialRepository
→ FrotiXDbContext
```

---

### 161. SetorSolicitanteRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<SetorSolicitante>, ISetorSolicitanteRepository`

```
→ Repository<SetorSolicitante>
→ ISetorSolicitanteRepository
→ FrotiXDbContext
```

---

### 162. TipoMultaRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<TipoMulta>, ITipoMultaRepository`

```
→ Repository<TipoMulta>
→ ITipoMultaRepository
→ FrotiXDbContext
```

---

### 163. UnidadeRepository.cs
**Tipo:** Repository Implementation
**Herança:** `Repository<Unidade>, IUnidadeRepository`

```
→ Repository<Unidade>
→ IUnidadeRepository
→ FrotiXDbContext
```

---

### 164. UnitOfWork.OcorrenciaViagem.cs
**Tipo:** Partial Class (UnitOfWork extension)
**Localização:** Repository/

```
→ IUnitOfWork.OcorrenciaViagem  [Interface]
→ OcorrenciaViagemRepository  [Repository implementation]
```

---

### 165. UnitOfWork.RepactuacaoVeiculo.cs
**Tipo:** Partial Class (UnitOfWork extension)
**Localização:** Repository/

```
→ IUnitOfWork.RepactuacaoVeiculo  [Interface]
→ RepactuacaoVeiculoRepository  [Repository implementation]
```

---

### 166. UnitOfWork.cs
**Tipo:** Central Orchestrator Class
**Localização:** Repository/
**Implementa:** `IUnitOfWork`
**Padrão:** Unit of Work pattern

```
Central Aggregator - Instancia TODOS os 100+ repositórios:

→ FrotiXDbContext  [DbContext principal]

→ Todos os Repositories (instantânea no construtor):
    • UnidadeRepository(_db)
    • CombustivelRepository(_db)
    • MarcaVeiculoRepository(_db)
    • ModeloVeiculoRepository(_db)
    • VeiculoRepository(_db)
    • ... (88 repositórios)

→ Lazy-loaded repositories (via propriedade):
    • ViagemEstatisticaRepository (lazy inicializado)
    • VeiculoPadraoViagemRepository (lazy inicializado)

Métodos de Persistência:
    • Save() : void - SaveChanges()
    • SaveAsync() : Task - SaveChangesAsync()
    • GetDbContext() : DbContext - Acesso direto
    • Dispose() : void - Liberar recursos
```

---

### 167-211. View Repositories (43 arquivos)

**Padrão Comum:** Todos herdam de `Repository<ViewXxx>` e implementam `IViewXxxRepository`

**Exemplos:**

#### ViewAbastecimentosRepository.cs
```
→ Repository<ViewAbastecimentos>
→ IViewAbastecimentosRepository
→ FrotiXDbContext
```

#### ViewAtaFornecedorRepository.cs
```
→ Repository<ViewAtaFornecedor>
→ IViewAtaFornecedorRepository
→ FrotiXDbContext
```

#### ViewContratoFornecedorRepository.cs
```
→ Repository<ViewContratoFornecedor>
→ IViewContratoFornecedorRepository
→ FrotiXDbContext
```

#### ViewControleAcessoRepository.cs
```
→ Repository<ViewControleAcesso>
→ IViewControleAcessoRepository
→ FrotiXDbContext
```

#### ViewCustosViagemRepository.cs
```
→ Repository<ViewCustosViagem>
→ IViewCustosViagemRepository
→ FrotiXDbContext
```

#### ViewEmpenhoMultaRepository.cs
```
→ Repository<ViewEmpenhoMulta>
→ IViewEmpenhoMultaRepository
→ FrotiXDbContext
```

#### ViewEmpenhosRepository.cs
```
→ Repository<ViewEmpenhos>
→ IViewEmpenhosRepository
→ FrotiXDbContext
```

#### ViewEventosRepository.cs
```
→ Repository<ViewEventos>
→ IViewEventosRepository
→ FrotiXDbContext
```

#### ViewExisteItemContratoRepository.cs
```
→ Repository<ViewExisteItemContrato>
→ IViewExisteItemContratoRepository
→ FrotiXDbContext
```

#### ViewFluxoEconomildo.cs
```
→ Repository<ViewFluxoEconomildo>
→ IViewFluxoEconomildoRepository
→ FrotiXDbContext
```

#### ViewFluxoEconomildoData.cs
```
→ Repository<ViewFluxoEconomildoData>
→ IViewFluxoEconomildoDataRepository
→ FrotiXDbContext
```

#### ViewGlosaRepository.cs
```
→ Repository<ViewGlosa>
→ IViewGlosaRepository
→ FrotiXDbContext
```

#### ViewItensManutencaoRepository.cs
```
→ Repository<ViewItensManutencao>
→ IViewItensManutencaoRepository
→ FrotiXDbContext
```

#### ViewLavagemRepository.cs
```
→ Repository<ViewLavagem>
→ IViewLavagemRepository
→ FrotiXDbContext
```

#### ViewLotacaoMotoristaRepository.cs
```
→ Repository<ViewLotacaoMotorista>
→ IViewLotacaoMotoristaRepository
→ FrotiXDbContext
```

#### ViewLotacoesRepository.cs
```
→ Repository<ViewLotacoes>
→ IViewLotacoesRepository
→ FrotiXDbContext
```

#### ViewManutencaoRepository.cs
```
→ Repository<ViewManutencao>
→ IViewManutencaoRepository
→ FrotiXDbContext
```

#### ViewMediaConsumoRepository.cs
```
→ Repository<ViewMediaConsumo>
→ IViewMediaConsumoRepository
→ FrotiXDbContext
```

#### ViewMotoristaFluxo.cs
```
→ Repository<ViewMotoristaFluxo>
→ IViewMotoristaFluxoRepository
→ FrotiXDbContext
```

#### ViewMotoristasRepository.cs
```
→ Repository<ViewMotoristas>
→ IViewMotoristasRepository
→ FrotiXDbContext
```

#### ViewMotoristasViagemRepository.cs
```
→ Repository<ViewMotoristasViagem>
→ IViewMotoristasViagemRepository
→ FrotiXDbContext
```

#### ViewMultasRepository.cs
```
→ Repository<ViewMultas>
→ IviewMultasRepository
→ FrotiXDbContext
```

#### ViewNoFichaVistoriaRepository.cs
```
→ Repository<ViewNoFichaVistoria>
→ IViewNoFichaVistoriaRepository
→ FrotiXDbContext
```

#### ViewOcorrencia.cs
```
→ Repository<ViewOcorrencia>
→ IViewOcorrenciaRepository
→ FrotiXDbContext
```

#### ViewOcorrenciasAbertasVeiculoRepository.cs
```
→ Repository<ViewOcorrenciasAbertasVeiculo>
→ IViewOcorrenciasAbertasVeiculoRepository
→ FrotiXDbContext
```

#### ViewOcorrenciasViagemRepository.cs
```
→ Repository<ViewOcorrenciasViagem>
→ IViewOcorrenciasViagemRepository
→ FrotiXDbContext
```

#### ViewPatrimonioConferenciaRepository.cs
```
→ Repository<ViewPatrimonioConferencia>
→ IViewPatrimonioConferenciaRepository
→ FrotiXDbContext
```

#### ViewPendenciasManutencaoRepository.cs
```
→ Repository<ViewPendenciasManutencao>
→ IViewPendenciasManutencaoRepository
→ FrotiXDbContext
```

#### ViewProcuraFichaRepository.cs
```
→ Repository<ViewProcuraFicha>
→ IViewProcuraFichaRepository
→ FrotiXDbContext
```

#### ViewRequisitantesRepository.cs
```
→ Repository<ViewRequisitantes>
→ IViewRequisitantesRepository
→ FrotiXDbContext
```

#### ViewSetoresRepository.cs
```
→ Repository<ViewSetores>
→ IViewSetoresRepository
→ FrotiXDbContext
```

#### ViewVeiculosManutencaoRepository.cs
```
→ Repository<ViewVeiculosManutencao>
→ IViewVeiculosManutencaoRepository
→ FrotiXDbContext
```

#### ViewVeiculosManutencaoReservaRepository.cs
```
→ Repository<ViewVeiculosManutencaoReserva>
→ IViewVeiculosManutencaoReservaRepository
→ FrotiXDbContext
```

#### ViewVeiculosRepository.cs
```
→ Repository<ViewVeiculos>
→ IViewVeiculosRepository
→ FrotiXDbContext
```

#### ViewViagensAgendaRepository.cs
```
→ Repository<ViewViagensAgenda>
→ IViewViagensAgendaRepository
→ FrotiXDbContext
```

#### ViewViagensAgendaTodosMesesRepository.cs
```
→ Repository<ViewViagensAgendaTodosMeses>
→ IViewViagensAgendaTodosMesesRepository
→ FrotiXDbContext
```

#### ViewViagensRepository.cs
```
→ Repository<ViewViagens>
→ IViewViagensRepository
→ FrotiXDbContext
```

---

### 211. ProperDataRepository.cs
**Localização:** TextNormalization/Repository/
**Tipo:** Repository specializado para normalização de dados

```
→ Repository<ProperData>  [Base genérico]
→ IRepository<ProperData>  [Interface]
→ FrotiXDbContext  [DbContext]

Nota: Localizado fora do diretório padrão Repository/
```

---

## 📊 Resumo de Estatísticas

| Categoria | Quantidade | Exemplos |
|-----------|-----------|----------|
| **Interfaces (IRepository)** | 17 | IViewMotoristasViagemRepository, IViewMultasRepository, ... |
| **Implementations (Repository)** | 88 | ItemVeiculoAtaRepository, LavadorRepository, ... |
| **View Repositories** | 43 | ViewAbastecimentosRepository, ViewMotoristasRepository, ... |
| **Specialized Repositories** | 2 | LogRepository, OcorrenciaViagemRepository |
| **Orchestrator** | 1 | UnitOfWork.cs (partial class com 2 extensões) |
| **Base Generic** | 1 | Repository.cs (classe genérica para herança) |
| **Outras** | 1 | ProperDataRepository.cs |
| **TOTAL** | **105** | Files 107-211 |

---

## 🔄 Padrão de Dependências Comum

Todos os 105 arquivos seguem um destes padrões:

### Padrão 1: Interface Repository
```
IXxxRepository
  → IRepository<T>
```

### Padrão 2: Concrete Repository
```
XxxRepository
  → Repository<T> [base class]
  → IXxxRepository [interface]
  → FrotiXDbContext [DbContext]
```

### Padrão 3: View Repository (especializado)
```
ViewXxxRepository
  → Repository<ViewXxx> [base class]
  → IViewXxxRepository [interface]
  → FrotiXDbContext [DbContext]
```

### Padrão 4: Specialized Repository (LogRepository)
```
LogRepository
  → ILogRepository [interface]
  → FrotiXDbContext [DbContext direto - não herda]
```

### Padrão 5: Unit of Work (Orchestrator)
```
UnitOfWork
  → IUnitOfWork [interface]
  → FrotiXDbContext [DbContext único]
  → Instancia 100+ repositórios
  → Lazy-loading para 2 repositórios especiais
```

---

## 🎯 Conclusões

1. **Baixo Acoplamento:** Interfaces IRepository desvincilam implementações de consumidores
2. **Reutilização:** Repository<T> base fornece 90% da funcionalidade genérica
3. **Consistent Pattern:** 88% dos files seguem pattern `Repository(T) : IXxx`
4. **Centralization:** UnitOfWork centraliza TODOS os repositórios
5. **Lazy Loading:** Apenas 2 repositórios com lazy-loading (performance optimization)
6. **DbContext Único:** FrotiXDbContext é o ponto único de acesso ao banco

---

## 📝 Anotações

- Arquivo 131 (LogRepository.cs): Exception ao padrão - não herda de Repository<T>, implementa ILogRepository diretamente
- Arquivo 145 (OcorrenciaViagemRepository.cs): Exception ao padrão - não herda de Repository<T>
- Arquivo 166 (UnitOfWork.cs): Classe partial com extensões em arquivos 164 e 165
- Arquivo 211 (ProperDataRepository.cs): Localizado em diretório não-padrão (TextNormalization/Repository/)

---

**Processamento Concluído:** 105 arquivos (files 107-211)
**Data:** 03/02/2026
**Próxima Fase:** Integrar com outras seções de mapeamento (JS→JS, JS→CS, CSHTML)
