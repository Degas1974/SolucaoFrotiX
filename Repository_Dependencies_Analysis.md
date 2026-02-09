# FrotiX Repository Layer - Dependencies Analysis
**Project:** FrotiX.Site
**Module:** Repository Layer (C# Repositories)
**Total Files:** 106 files
**Analysis Date:** 2026-02-03

---

## Executive Summary

The Repository layer in FrotiX consists of 106 C# files organized as follows:
- **1 Generic Base Class:** `Repository.cs` (generic CRUD operations)
- **1 Interface Base:** `IRepository.cs` (interface definition)
- **1 Unit of Work Pattern:** `UnitOfWork.cs` + extensions
- **~50 Concrete Repositories:** Domain-specific repositories
- **~50 Repository Interfaces:** `IRepository/*` interfaces
- **2 Special Repositories:** TextNormalization and View repositories

All repositories follow a **consistent dependency pattern** with minimal cross-repository dependencies, maximizing modularity.

---

## CS → CS Dependencies (Concrete Repository Files)

### Base Infrastructure

#### Repository.cs (Generic Base Class)
**Localização:** FrotiX.Site/Repository/Repository.cs
**Tipo:** Generic Base Class

**Base Class:** `IRepository<T>` interface implementation

**Specialized Methods:**
- `GetFirstOrDefault(filter, includeProperties)` - Single entity with tracking control
- `GetFirstOrDefaultAsync(filter, includeProperties)` - Async variant with concurrency handling
- `GetAll(filter, orderBy, includes, asNoTracking)` - List with filtering and ordering
- `GetAllAsync(filter, orderBy, includes, asNoTracking, take)` - Async with pagination support
- `GetAllReduced<TResult>(selector, filter, orderBy, includes)` - Projection materialized
- `GetAllReducedIQueryable<TResult>()` - Lazy projection via IQueryable
- `Add(entity)` / `AddAsync(entity)` - Entity insertion
- `Update(entity)` - Entity modification
- `Remove(id)` / `Remove(entity)` - Entity deletion

**DbContext Usage:**
- `DbContext _db` - Stores context reference
- `DbSet<T> dbSet` - Works with DbSet operations
- `DbSet.Find()`, `DbSet.Add()`, `DbSet.Update()`, `DbSet.Remove()`
- `AsTracking()` / `AsNoTracking()` - Tracks DB context globally as NoTracking

**Depende de:**
1. **Microsoft.EntityFrameworkCore** - EF Core framework
   - DbContext, DbSet, IQueryable operations
2. **FrotiX.Repository.IRepository** - Generic interface definition

**Key Pattern:**
```csharp
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DbContext _db;
    protected readonly DbSet<T> dbSet;

    public Repository(DbContext db) { ... }
    protected IQueryable<T> PrepareQuery(filter, includes, asNoTracking) { ... }
}
```

---

### Unit of Work Pattern

#### UnitOfWork.cs (Main Orchestrator)
**Localização:** FrotiX.Site/Repository/UnitOfWork.cs
**Tipo:** Unit of Work Pattern Implementation

**Base Class:** `IUnitOfWork` interface

**Properties (Repositories Exposed):**
- `Unidade` → UnidadeRepository
- `Combustivel` → CombustivelRepository
- `MarcaVeiculo` → MarcaVeiculoRepository
- `ModeloVeiculo` → ModeloVeiculoRepository
- `Veiculo` → VeiculoRepository
- `Fornecedor` → FornecedorRepository
- `Contrato` → ContratoRepository
- `AtaRegistroPrecos` → AtaRegistroPrecosRepository
- `VeiculoContrato` → VeiculoContratoRepository
- `VeiculoAta` → VeiculoAtaRepository
- `AspNetUsers` → AspNetUsersRepository
- `PlacaBronze` → PlacaBronzeRepository
- `Motorista` → MotoristaRepository
- `Operador` → OperadorRepository
- `Lavador` → LavadorRepository
- `Viagem` → ViagemRepository
- `Manutencao` → ManutencaoRepository
- `Escalas` → EscalasRepository
- `Evento` → EventoRepository
- `Abastecimento` → AbastecimentoRepository
- `Empenho` → EmpenhoRepository
- `NotaFiscal` → NotaFiscalRepository
- `Combustivel` → CombustivelRepository
- `Multa` → MultaRepository
- `(and many View repositories...)`

**Specialized Methods:**
- `GetDbContext()` - Returns DbContext for advanced operations
- `Save()` - Synchronous persistence
- `SaveAsync()` - Asynchronous persistence
- `Dispose()` - Resource cleanup

**DbContext Usage:**
- `FrotiXDbContext _db` - Central database context

**Depende de:**
1. **FrotiX.Data** - FrotiXDbContext
2. **FrotiX.Repository.IRepository** - Interface contract
3. **60+ Concrete Repository Classes** - Lazy initialization

**Partial Class Extensions:**
- `UnitOfWork.OcorrenciaViagem.cs` - OcorrenciaViagem-specific operations
- `UnitOfWork.RepactuacaoVeiculo.cs` - RepactuacaoVeiculo-specific operations

---

## Concrete Repository Files (Detailed Analysis)

### Domain Model Repositories

#### AbastecimentoRepository.cs
**Localização:** FrotiX.Site/Repository/AbastecimentoRepository.cs
**Tipo:** Repository

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: Abastecimento
2. **Repository<Abastecimento>** - Base CRUD operations
3. **IAbastecimentoRepository** - Interface contract
4. **Microsoft.AspNetCore.Mvc.Rendering.SelectListItem** - UI dropdowns

**Specialized Methods:**
- `GetAbastecimentoListForDropDown()` → IEnumerable<SelectListItem>
- `Update(abastecimento)` - Override with SaveChanges()

**LINQ Operations:**
- SELECT for dropdown composition (Litros field)

---

#### AlertasFrotiXRepository.cs
**Localização:** FrotiX.Site/Repository/AlertasFrotiXRepository.cs
**Tipo:** Repository with Complex Queries

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: AlertasFrotiX, AlertasUsuario, AspNetUsers, Viagem, Manutencao
2. **Repository<AlertasFrotiX>** - Base CRUD operations
3. **IAlertasFrotiXRepository** - Interface contract
4. **Microsoft.EntityFrameworkCore** - Async operations, Include/ThenInclude

**Specialized Methods:**
- `GetTodosAlertasAtivosAsync()` - Fetch active alerts with users
- `GetTodosAlertasComLeituraAsync()` - Alerts with read status
- `GetQuantidadeAlertasNaoLidosAsync(usuarioId)` - Count unread alerts
- `MarcarComoLidoAsync(alertaId, usuarioId)` - Mark alert as read
- `CriarAlertaAsync(alerta, usuariosIds)` - Create alert and link users
- `GetAlertaComDetalhesAsync(alertaId)` - Full alert details with includes
- `MarcarComoApagadoAsync(alertaId, usuarioId)` - Soft delete
- `DesativarAlertaAsync(alertaId)` - Deactivate alert
- `GetUsuariosNotificadosAsync(alertaId)` - Get users linked to alert
- `GetUsuarioAsync(usuarioId)` - Fetch user details
- `GetAlertasParaNotificarAsync()` - Smart alert filtering by type and date

**LINQ Operations:**
- Include/ThenInclude chains for navigation properties
- Where with complex predicates
- Join operations between AlertasUsuario and AlertasFrotiX
- TimeOfDay comparisons for scheduled alerts

**Cross-Repository Dependencies:**
- AlertasUsuario (many-to-many junction)
- AspNetUsers (user references)
- Viagem (nullable reference)
- Manutencao (nullable reference)

---

#### AlertasUsuarioRepository.cs
**Localização:** FrotiX.Site/Repository/AlertasUsuarioRepository.cs
**Tipo:** Junction Repository (N:N Relationship)

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: AlertasUsuario, AlertasFrotiX
2. **Repository<AlertasUsuario>** - Base CRUD operations
3. **IAlertasUsuarioRepository** - Interface contract

**Specialized Methods:**
- `ObterAlertasPorUsuarioAsync(usuarioId)` - User's alerts
- `ObterUsuariosPorAlertaAsync(alertaId)` - Alert's users
- `UsuarioTemAlertaAsync(alertaId, usuarioId)` - Existence check
- `RemoverAlertasDoUsuarioAsync(usuarioId)` - Bulk delete by user
- `RemoverUsuariosDoAlertaAsync(alertaId)` - Bulk delete by alert
- `Update(alertaUsuario)` - Override update

**LINQ Operations:**
- AsNoTracking for read-only queries
- Where with compound conditions
- RemoveRange for bulk operations

---

#### AspNetUsersRepository.cs
**Localização:** FrotiX.Site/Repository/AspNetUsersRepository.cs
**Tipo:** Repository

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: AspNetUsers
2. **Repository<AspNetUsers>** - Base CRUD operations
3. **IAspNetUsersRepository** - Interface contract
4. **Microsoft.AspNetCore.Mvc.Rendering.SelectListItem** - UI dropdowns

**Specialized Methods:**
- `GetAspNetUsersListForDropDown()` - Active users ordered by name
- `Update(aspNetUsers)` - Override with SaveChanges()

**LINQ Operations:**
- Where to filter active users (Status == true)
- OrderBy for display ordering

---

#### AtaRegistroPrecosRepository.cs
**Localização:** FrotiX.Site/Repository/AtaRegistroPrecosRepository.cs
**Tipo:** Repository

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: AtaRegistroPrecos
2. **Repository<AtaRegistroPrecos>** - Base CRUD operations
3. **IAtaRegistroPrecosRepository** - Interface contract

---

#### CombustivelRepository.cs
**Localização:** FrotiX.Site/Repository/CombustivelRepository.cs
**Tipo:** Repository

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: Combustivel
2. **Repository<Combustivel>** - Base CRUD operations
3. **ICombustivelRepository** - Interface contract

---

#### ContratoRepository.cs
**Localização:** FrotiX.Site/Repository/ContratoRepository.cs
**Tipo:** Repository with Query Composition

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: Contrato, Fornecedor (navigation)
2. **Repository<Contrato>** - Base CRUD operations
3. **IContratoRepository** - Interface contract
4. **Microsoft.AspNetCore.Mvc.Rendering.SelectListItem** - UI dropdowns
5. **Microsoft.EntityFrameworkCore** - AsNoTracking, Select

**Specialized Methods:**
- `GetDropDown(tipoContrato?)` → IQueryable<SelectListItem>
  - Filters by optional type
  - Orders by: AnoContrato desc, NumeroContrato desc, Fornecedor desc
  - Includes fornecedor details in text

**LINQ Operations:**
- AsNoTracking for read-only projection
- Where with conditional type filtering
- OrderBy chaining (3-field sort)
- Select with conditional text formatting

---

#### ControleAcessoRepository.cs
**Localização:** FrotiX.Site/Repository/ControleAcessoRepository.cs
**Tipo:** Repository

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: ControleAcesso
2. **Repository<ControleAcesso>** - Base CRUD operations
3. **IControleAcessoRepository** - Interface contract

---

#### CorridasTaxiLegRepository.cs & CorridasTaxiLegCanceladasRepository.cs
**Localização:** FrotiX.Site/Repository/CorridasTaxiLeg*.cs
**Tipo:** Repositories for TaxiLeg Integration

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: CorridasTaxiLeg, CorridasTaxiLegCanceladas
2. **Repository<T>** - Base CRUD operations
3. **ICorridasTaxiLeg** / **ICorridasTaxiLegCanceladas** - Interface contracts

---

#### CustoMensalItensContratoRepository.cs
**Localização:** FrotiX.Site/Repository/CustoMensalItensContratoRepository.cs
**Tipo:** Repository

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: CustoMensalItensContrato
2. **Repository<CustoMensalItensContrato>** - Base CRUD operations
3. **ICustoMensalItensContratoRepository** - Interface contract

---

#### EmpenhoRepository.cs & EmpenhoMultaRepository.cs
**Localização:** FrotiX.Site/Repository/Empenho*.cs
**Tipo:** Repositories for Financial Tracking

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: Empenho, EmpenhoMulta
2. **Repository<T>** - Base CRUD operations
3. **IEmpenhoRepository** / **IEmpenhoMultaRepository** - Interface contracts

**Key Entities:**
- `Empenho` - Budget allocation
- `EmpenhoMulta` - Penalty budget link

---

#### EncarregadoRepository.cs & EncarregadoContratoRepository.cs
**Localização:** FrotiX.Site/Repository/Encarregado*.cs
**Tipo:** Repositories for Site Managers

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: Encarregado, EncarregadoContrato
2. **Repository<T>** - Base CRUD operations
3. **IEncarregadoRepository** / **IEncarregadoContratoRepository** - Interface contracts

---

#### EscalasRepository.cs
**Localização:** FrotiX.Site/Repository/EscalasRepository.cs
**Tipo:** Repository for Schedule Management

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: Escalas
2. **Repository<Escalas>** - Base CRUD operations
3. **IEscalasRepository** - Interface contract

---

#### EventoRepository.cs
**Localização:** FrotiX.Site/Repository/EventoRepository.cs
**Tipo:** Repository for Recurring Events

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: Evento
2. **Repository<Evento>** - Base CRUD operations
3. **IEventoRepository** - Interface contract

---

#### FornecedorRepository.cs
**Localização:** FrotiX.Site/Repository/FornecedorRepository.cs
**Tipo:** Repository

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: Fornecedor
2. **Repository<Fornecedor>** - Base CRUD operations
3. **IFornecedorRepository** - Interface contract

---

#### ItemVeiculoAtaRepository.cs & ItemVeiculoContratoRepository.cs
**Localização:** FrotiX.Site/Repository/ItemVeiculo*.cs
**Tipo:** Repositories for Junction Entities

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: ItemVeiculoAta, ItemVeiculoContrato
2. **Repository<T>** - Base CRUD operations
3. **IItemVeiculoAtaRepository** / **IItemVeiculoContratoRepository** - Interface contracts

---

#### ItensManutencaoRepository.cs
**Localização:** FrotiX.Site/Repository/ItensManutencaoRepository.cs
**Tipo:** Repository

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: ItensManutencao
2. **Repository<ItensManutencao>** - Base CRUD operations
3. **IItensManutencaoRepository** - Interface contract

---

#### Lavador* Repositories (4 files)
**Localização:** FrotiX.Site/Repository/Lavador*.cs
**Tipo:** Repositories for Cleaning Service

**Files:**
- `LavadorRepository.cs`
- `LavadorContratoRepository.cs`
- `LavadoresLavagemRepository.cs`
- `LavagemRepository.cs`

**Depende de:**
1. **FrotiXDbContext** - Database access
2. **Repository<T>** - Base CRUD operations
3. **ILavador* Interfaces** - Interface contracts

---

#### LogRepository.cs
**Localização:** FrotiX.Site/Repository/LogRepository.cs
**Tipo:** Repository for Error Logging

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: Log
2. **Repository<Log>** - Base CRUD operations
3. **ILogRepository** - Interface contract

---

#### LotacaoMotoristaRepository.cs
**Localização:** FrotiX.Site/Repository/LotacaoMotoristaRepository.cs
**Tipo:** Repository for Driver Assignment

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: LotacaoMotorista
2. **Repository<LotacaoMotorista>** - Base CRUD operations
3. **ILotacaoMotoristaRepository** - Interface contract

---

#### ManutencaoRepository.cs
**Localização:** FrotiX.Site/Repository/ManutencaoRepository.cs
**Tipo:** Repository for Vehicle Maintenance

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: Manutencao
2. **Repository<Manutencao>** - Base CRUD operations
3. **IManutencaoRepository** - Interface contract

---

#### MarcaVeiculoRepository.cs & ModeloVeiculoRepository.cs
**Localização:** FrotiX.Site/Repository/Marca*.cs & Modelo*.cs
**Tipo:** Repositories for Vehicle Master Data

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: MarcaVeiculo, ModeloVeiculo
2. **Repository<T>** - Base CRUD operations
3. **IMarcaVeiculoRepository** / **IModeloVeiculoRepository** - Interface contracts

---

#### MediaCombustivelRepository.cs
**Localização:** FrotiX.Site/Repository/MediaCombustivelRepository.cs
**Tipo:** Repository

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: MediaCombustivel
2. **Repository<MediaCombustivel>** - Base CRUD operations
3. **IMediaCombustivelRepository** - Interface contract

---

#### Motorista* Repositories (2 files)
**Localização:** FrotiX.Site/Repository/Motorista*.cs
**Tipo:** Repositories for Driver Management

**Files:**
- `MotoristaRepository.cs`
- `MotoristaContratoRepository.cs`

**Depende de:**
1. **FrotiXDbContext** - Database access
2. **Repository<T>** - Base CRUD operations
3. **IMotoristaRepository** / **IMotoristaContratoRepository** - Interface contracts

---

#### Movimentacao* Repositories (2 files)
**Localização:** FrotiX.Site/Repository/Movimentacao*.cs
**Tipo:** Repositories for Material Movement

**Files:**
- `MovimentacaoPatrimonioRepository.cs`
- `MovimentacaoEmpenhoRepository.cs`
- `MovimentacaoEmpenhoMultaRepository.cs`

**Depende de:**
1. **FrotiXDbContext** - Database access
2. **Repository<T>** - Base CRUD operations
3. **IMovimentacao* Interfaces** - Interface contracts

---

#### MultaRepository.cs
**Localização:** FrotiX.Site/Repository/MultaRepository.cs
**Tipo:** Repository for Penalty Management

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: Multa
2. **Repository<Multa>** - Base CRUD operations
3. **IMultaRepository** - Interface contract

---

#### NotaFiscalRepository.cs
**Localização:** FrotiX.Site/Repository/NotaFiscalRepository.cs
**Tipo:** Repository for Invoice Management

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: NotaFiscal
2. **Repository<NotaFiscal>** - Base CRUD operations
3. **INotaFiscalRepository** - Interface contract

---

#### OcorrenciaViagemRepository.cs
**Localização:** FrotiX.Site/Repository/OcorrenciaViagemRepository.cs
**Tipo:** Repository for Trip Incidents

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: OcorrenciaViagem
2. **Repository<OcorrenciaViagem>** - Base CRUD operations
3. **IOcorrenciaViagemRepository** - Interface contract

---

#### Operador* Repositories (2 files)
**Localização:** FrotiX.Site/Repository/Operador*.cs
**Tipo:** Repositories for Fleet Operator Management

**Files:**
- `OperadorRepository.cs`
- `OperadorContratoRepository.cs`

**Depende de:**
1. **FrotiXDbContext** - Database access
2. **Repository<T>** - Base CRUD operations
3. **IOperadorRepository** / **IOperadorContratoRepository** - Interface contracts

---

#### OrgaoAutuanteRepository.cs
**Localização:** FrotiX.Site/Repository/OrgaoAutuanteRepository.cs
**Tipo:** Repository for Traffic Authorities

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: OrgaoAutuante
2. **Repository<OrgaoAutuante>** - Base CRUD operations
3. **IOrgaoAutuanteRepository** - Interface contract

---

#### PatrimonioRepository.cs
**Localização:** FrotiX.Site/Repository/PatrimonioRepository.cs
**Tipo:** Repository for Asset Management

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: Patrimonio
2. **Repository<Patrimonio>** - Base CRUD operations
3. **IPatrimonioRepository** - Interface contract

---

#### PlacaBronzeRepository.cs
**Localização:** FrotiX.Site/Repository/PlacaBronzeRepository.cs
**Tipo:** Repository

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: PlacaBronze
2. **Repository<PlacaBronze>** - Base CRUD operations
3. **IPlacaBronzeRepository** - Interface contract

---

#### RecursoRepository.cs
**Localização:** FrotiX.Site/Repository/RecursoRepository.cs
**Tipo:** Repository for Permission Management

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: Recurso
2. **Repository<Recurso>** - Base CRUD operations
3. **IRecursoRepository** - Interface contract

---

#### RegistroCupomAbastecimentoRepository.cs
**Localização:** FrotiX.Site/Repository/RegistroCupomAbastecimentoRepository.cs
**Tipo:** Repository for Fuel Receipts

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: RegistroCupomAbastecimento
2. **Repository<RegistroCupomAbastecimento>** - Base CRUD operations
3. **IRegistroCupomAbastecimentoRepository** - Interface contract

---

#### Repactuacao* Repositories (4 files)
**Localização:** FrotiX.Site/Repository/Repactuacao*.cs
**Tipo:** Repositories for Contract Renegotiation

**Files:**
- `RepactuacaoContratoRepository.cs`
- `RepactuacaoAtaRepository.cs`
- `RepactuacaoServicosRepository.cs`
- `RepactuacaoTerceirizacaoRepository.cs`
- `RepactuacaoVeiculoRepository.cs`

**Depende de:**
1. **FrotiXDbContext** - Database access
2. **Repository<T>** - Base CRUD operations
3. **IRepactuacao* Interfaces** - Interface contracts

---

#### RequisitanteRepository.cs
**Localização:** FrotiX.Site/Repository/RequisitanteRepository.cs
**Tipo:** Repository for Request Originator Management

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: Requisitante
2. **Repository<Requisitante>** - Base CRUD operations
3. **IRequisitanteRepository** - Interface contract

---

#### SecaoPatrimonialRepository.cs & SetorPatrimonialRepository.cs
**Localização:** FrotiX.Site/Repository/SecaoPatrimonial*.cs & SetorPatrimonial*.cs
**Tipo:** Repositories for Asset Organization

**Depende de:**
1. **FrotiXDbContext** - Database access
2. **Repository<T>** - Base CRUD operations
3. **ISecaoPatrimonialRepository** / **ISetorPatrimonialRepository** - Interface contracts

---

#### SetorSolicitanteRepository.cs
**Localização:** FrotiX.Site/Repository/SetorSolicitanteRepository.cs
**Tipo:** Repository for Request Organization

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: SetorSolicitante
2. **Repository<SetorSolicitante>** - Base CRUD operations
3. **ISetorSolicitanteRepository** - Interface contract

---

#### TipoMultaRepository.cs
**Localização:** FrotiX.Site/Repository/TipoMultaRepository.cs
**Tipo:** Repository for Penalty Types

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: TipoMulta
2. **Repository<TipoMulta>** - Base CRUD operations
3. **ITipoMultaRepository** - Interface contract

---

#### UnidadeRepository.cs
**Localização:** FrotiX.Site/Repository/UnidadeRepository.cs
**Tipo:** Repository for Organizational Units

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: Unidade
2. **Repository<Unidade>** - Base CRUD operations
3. **IUnidadeRepository** - Interface contract

---

#### VeiculoRepository.cs
**Localização:** FrotiX.Site/Repository/VeiculoRepository.cs
**Tipo:** Repository for Vehicle Management

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: Veiculo
2. **Repository<Veiculo>** - Base CRUD operations
3. **IVeiculoRepository** - Interface contract

---

#### VeiculoAtaRepository.cs & VeiculoContratoRepository.cs
**Localização:** FrotiX.Site/Repository/Veiculo*.cs
**Tipo:** Repositories for Vehicle-Document Links

**Depende de:**
1. **FrotiXDbContext** - Database access
2. **Repository<T>** - Base CRUD operations
3. **IVeiculoAtaRepository** / **IVeiculoContratoRepository** - Interface contracts

---

#### VeiculoPadraoViagemRepository.cs
**Localização:** FrotiX.Site/Repository/VeiculoPadraoViagemRepository.cs
**Tipo:** Repository

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: VeiculoPadraoViagem
2. **Repository<VeiculoPadraoViagem>** - Base CRUD operations
3. **IVeiculoPadraoViagemRepository** - Interface contract

---

#### ViagemRepository.cs
**Localização:** FrotiX.Site/Repository/ViagemRepository.cs
**Tipo:** Repository with Complex Queries and Pagination

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: Viagem, ViewViagens (for optimized queries)
2. **Repository<Viagem>** - Base CRUD operations
3. **IViagemRepository** - Interface contract
4. **Microsoft.AspNetCore.Mvc.Rendering.SelectListItem** - UI dropdowns
5. **Microsoft.EntityFrameworkCore** - Async operations, Stopwatch
6. **NPOI.SS.Formula.Functions** - Excel import support

**Specialized Methods:**
- `GetViagemListForDropDown()` - Ordered by initial date
- `Update(viagem)` - Override with SaveChanges()
- `GetDistinctOrigensAsync()` - Unique origin cities
- `GetDistinctDestinosAsync()` - Unique destination cities
- `CorrigirOrigemAsync(origensAntigas, novaOrigem)` - Batch origin correction
- `CorrigirDestinoAsync(destinosAntigos, novoDestino)` - Batch destination correction
- `BuscarViagensRecorrenciaAsync(id)` - Recurring event trips
- `GetViagensEventoPaginadoAsync(eventoId, page, pageSize)` - Optimized pagination
- `GetQuery(filter)` - IQueryable for composition

**LINQ Operations:**
- Complex filtering with EventoId and Status
- Two-phase query: COUNT + SELECT
- ViewViagens projection to reduce JOIN complexity
- Pagination with Skip/Take
- Distinct operations for dropdown values
- Batch updates with loops

**Special Features:**
- Performance logging with Stopwatch
- ViewViagens used for optimized data retrieval
- Async-first design
- Error handling with Alerta.TratamentoErroComLinha

---

#### ViagemEstatisticaRepository.cs
**Localização:** FrotiX.Site/Repository/ViagemEstatisticaRepository.cs
**Tipo:** Repository

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: ViagemEstatistica
2. **Repository<ViagemEstatistica>** - Base CRUD operations
3. **IViagemEstatisticaRepository** - Interface contract

---

#### ViagensEconomildoRepository.cs
**Localização:** FrotiX.Site/Repository/ViagensEconomildoRepository.cs
**Tipo:** Repository for Dashboard Analytics

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: ViagensEconomildo
2. **Repository<ViagensEconomildo>** - Base CRUD operations
3. **IViagensEconomildoRepository** - Interface contract

---

### View/Read-Only Repositories (35+ files)

These repositories provide optimized read-only access to database views and denormalized data.

#### ViewAbastecimentosRepository.cs
**Localização:** FrotiX.Site/Repository/ViewAbastecimentosRepository.cs
**Tipo:** Read-only View Repository

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: ViewAbastecimentos
2. **Repository<ViewAbastecimentos>** - Base CRUD operations
3. **IViewAbastecimentosRepository** - Interface contract

---

#### ViewAtaFornecedorRepository.cs
**Localização:** FrotiX.Site/Repository/ViewAtaFornecedorRepository.cs
**Tipo:** Read-only View Repository

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: ViewAtaFornecedor
2. **Repository<ViewAtaFornecedor>** - Base CRUD operations
3. **IViewAtaFornecedor** - Interface contract

---

#### ViewContratoFornecedorRepository.cs
**Localização:** FrotiX.Site/Repository/ViewContratoFornecedorRepository.cs
**Tipo:** Read-only View Repository

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: ViewContratoFornecedor
2. **Repository<ViewContratoFornecedor>** - Base CRUD operations
3. **IViewContratoFornecedor** - Interface contract

---

#### ViewControleAcessoRepository.cs
**Localização:** FrotiX.Site/Repository/ViewControleAcessoRepository.cs
**Tipo:** Read-only View Repository

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: ViewControleAcesso
2. **Repository<ViewControleAcesso>** - Base CRUD operations
3. **IViewControleAcessoRepository** - Interface contract

---

#### ViewCustosViagemRepository.cs
**Localização:** FrotiX.Site/Repository/ViewCustosViagemRepository.cs
**Tipo:** Read-only View Repository

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: ViewCustosViagem
2. **Repository<ViewCustosViagem>** - Base CRUD operations
3. **IViewCustosViagemRepository** - Interface contract

---

#### ViewEmpenho* Repositories (2 files)
**Localização:** FrotiX.Site/Repository/ViewEmpenho*.cs
**Tipo:** Read-only View Repositories

**Files:**
- `ViewEmpenhoMultaRepository.cs`
- `ViewEmpenhosRepository.cs`

**Depende de:**
1. **FrotiXDbContext** - Database access
2. **Repository<T>** - Base CRUD operations
3. **IViewEmpenho* Interfaces** - Interface contracts

---

#### ViewEventosRepository.cs
**Localização:** FrotiX.Site/Repository/ViewEventosRepository.cs
**Tipo:** Read-only View Repository

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: ViewEventos
2. **Repository<ViewEventos>** - Base CRUD operations
3. **IViewEventos** - Interface contract

---

#### ViewExisteItemContratoRepository.cs
**Localização:** FrotiX.Site/Repository/ViewExisteItemContratoRepository.cs
**Tipo:** Read-only View Repository

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: ViewExisteItemContrato
2. **Repository<ViewExisteItemContrato>** - Base CRUD operations
3. **IViewExisteItemContratoRepository** - Interface contract

---

#### ViewFluxo* Repositories (3 files)
**Localização:** FrotiX.Site/Repository/ViewFluxo*.cs
**Tipo:** Read-only View Repositories for Financial Dashboard

**Files:**
- `ViewFluxoEconomildo.cs`
- `ViewFluxoEconomildoData.cs`

**Depende de:**
1. **FrotiXDbContext** - Database access
2. **Repository<T>** - Base CRUD operations
3. **IViewFluxoEconomildo** / **IViewFluxoEconomildoDataRepository** - Interface contracts

---

#### ViewGlosaRepository.cs
**Localização:** FrotiX.Site/Repository/ViewGlosaRepository.cs
**Tipo:** Read-only View Repository for Deduction Tracking

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: ViewGlosa
2. **Repository<ViewGlosa>** - Base CRUD operations
3. **IViewGlosaRepository** - Interface contract

---

#### ViewItensManutencaoRepository.cs
**Localização:** FrotiX.Site/Repository/ViewItensManutencaoRepository.cs
**Tipo:** Read-only View Repository

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: ViewItensManutencao
2. **Repository<ViewItensManutencao>** - Base CRUD operations
3. **IViewItensManutencaoRepository** - Interface contract

---

#### ViewLavagem* Repositories (2 files)
**Localização:** FrotiX.Site/Repository/ViewLavagem*.cs
**Tipo:** Read-only View Repositories for Cleaning Operations

**Files:**
- `ViewLavagemRepository.cs`
- `ViewLotacaoMotoristaRepository.cs`

**Depende de:**
1. **FrotiXDbContext** - Database access
2. **Repository<T>** - Base CRUD operations
3. **IViewLavagem* Interfaces** - Interface contracts

---

#### ViewLotacoes* Repositories (2 files)
**Localização:** FrotiX.Site/Repository/ViewLotacao*.cs
**Tipo:** Read-only View Repositories for Assignment Management

**Files:**
- `ViewLotacoesRepository.cs`
- `ViewLotacaoMotoristaRepository.cs`

**Depende de:**
1. **FrotiXDbContext** - Database access
2. **Repository<T>** - Base CRUD operations
3. **IViewLotacao* Interfaces** - Interface contracts

---

#### ViewManutencaoRepository.cs
**Localização:** FrotiX.Site/Repository/ViewManutencaoRepository.cs
**Tipo:** Read-only View Repository

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: ViewManutencao
2. **Repository<ViewManutencao>** - Base CRUD operations
3. **IViewManutencaoRepository** - Interface contract

---

#### ViewMediaConsumoRepository.cs
**Localização:** FrotiX.Site/Repository/ViewMediaConsumoRepository.cs
**Tipo:** Read-only View Repository for Fuel Efficiency Metrics

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: ViewMediaConsumo
2. **Repository<ViewMediaConsumo>** - Base CRUD operations
3. **IViewMediaConsumoRepository** - Interface contract

---

#### ViewMotorista* Repositories (3 files)
**Localização:** FrotiX.Site/Repository/ViewMotorista*.cs
**Tipo:** Read-only View Repositories for Driver Analytics

**Files:**
- `ViewMotoristasRepository.cs`
- `ViewMotoristasViagemRepository.cs`
- `ViewMotoristaFluxo.cs`

**Depende de:**
1. **FrotiXDbContext** - Database access
2. **Repository<T>** - Base CRUD operations
3. **IViewMotorista* Interfaces** - Interface contracts

---

#### ViewMultasRepository.cs
**Localização:** FrotiX.Site/Repository/ViewMultasRepository.cs
**Tipo:** Read-only View Repository for Penalty Analysis

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: ViewMultas
2. **Repository<ViewMultas>** - Base CRUD operations
3. **IViewMultasRepository** - Interface contract

---

#### ViewNoFichaVistoriaRepository.cs
**Localização:** FrotiX.Site/Repository/ViewNoFichaVistoriaRepository.cs
**Tipo:** Read-only View Repository

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: ViewNoFichaVistoria
2. **Repository<ViewNoFichaVistoria>** - Base CRUD operations
3. **IViewNoFichaVistoriaRepository** - Interface contract

---

#### ViewOcorrencia* Repositories (3 files)
**Localização:** FrotiX.Site/Repository/ViewOcorrencia*.cs
**Tipo:** Read-only View Repositories for Incident Tracking

**Files:**
- `ViewOcorrencia.cs`
- `ViewOcorrenciasAbertasVeiculoRepository.cs`
- `ViewOcorrenciasViagemRepository.cs`

**Depende de:**
1. **FrotiXDbContext** - Database access
2. **Repository<T>** - Base CRUD operations
3. **IViewOcorrencia* Interfaces** - Interface contracts

---

#### ViewPatrimonioConferenciaRepository.cs
**Localização:** FrotiX.Site/Repository/ViewPatrimonioConferenciaRepository.cs
**Tipo:** Read-only View Repository for Asset Verification

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: ViewPatrimonioConferencia
2. **Repository<ViewPatrimonioConferencia>** - Base CRUD operations
3. **IViewPatrimonioConferenciaRepository** - Interface contract

---

#### ViewPendencias* Repositories
**Localização:** FrotiX.Site/Repository/ViewPendencias*.cs
**Tipo:** Read-only View Repository for Pending Items

**Depende de:**
1. **FrotiXDbContext** - Database access
2. **Repository<T>** - Base CRUD operations
3. **IViewPendencias* Interface** - Interface contract

---

#### ViewProcuraFichaRepository.cs
**Localização:** FrotiX.Site/Repository/ViewProcuraFichaRepository.cs
**Tipo:** Read-only View Repository

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: ViewProcuraFicha
2. **Repository<ViewProcuraFicha>** - Base CRUD operations
3. **IViewProcuraFichaRepository** - Interface contract

---

#### ViewRequisitantesRepository.cs
**Localização:** FrotiX.Site/Repository/ViewRequisitantesRepository.cs
**Tipo:** Read-only View Repository

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: ViewRequisitantes
2. **Repository<ViewRequisitantes>** - Base CRUD operations
3. **IViewRequisitantesRepository** - Interface contract

---

#### ViewSetoresRepository.cs
**Localização:** FrotiX.Site/Repository/ViewSetoresRepository.cs
**Tipo:** Read-only View Repository

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: ViewSetores
2. **Repository<ViewSetores>** - Base CRUD operations
3. **IViewSetoresRepository** - Interface contract

---

#### ViewVeiculos* Repositories (3 files)
**Localização:** FrotiX.Site/Repository/ViewVeiculos*.cs
**Tipo:** Read-only View Repositories for Vehicle Management

**Files:**
- `ViewVeiculosRepository.cs`
- `ViewVeiculosManutencaoRepository.cs`
- `ViewVeiculosManutencaoReservaRepository.cs`

**Depende de:**
1. **FrotiXDbContext** - Database access
2. **Repository<T>** - Base CRUD operations
3. **IViewVeiculos* Interfaces** - Interface contracts

---

#### ViewViagens* Repositories (3 files)
**Localização:** FrotiX.Site/Repository/ViewViagens*.cs
**Tipo:** Read-only View Repositories for Trip Analytics

**Files:**
- `ViewViagensRepository.cs`
- `ViewViagensAgendaRepository.cs`
- `ViewViagensAgendaTodosMesesRepository.cs`

**Depende de:**
1. **FrotiXDbContext** - Database access
   - DbSet: ViewViagens, ViewViagensAgenda, ViewViagensAgendaTodosMeses
2. **Repository<T>** - Base CRUD operations
3. **IViewViagens* Interfaces** - Interface contracts

---

### Special Repositories

#### ProperDataRepository.cs (TextNormalization)
**Localização:** FrotiX.Site/TextNormalization/Repository/ProperDataRepository.cs
**Tipo:** Text Normalization Repository

**Depende de:**
1. **FrotiXDbContext** - Database access
2. **Repository<T>** - Base CRUD operations (if applicable)

**Purpose:** Handles proper casing and text standardization for data fields.

---

## Dependency Summary Matrix

### All Dependencies (Aggregated)

**Primary Dependencies (All Repositories):**
1. **FrotiX.Data.FrotiXDbContext** - UNIVERSAL
   - Every single concrete repository depends on this
   - Provides DbSet access to all entities

2. **FrotiX.Repository.Repository<T>** - UNIVERSAL
   - Base class for all 50+ concrete repositories
   - Provides CRUD operations and query composition

3. **FrotiX.Repository.IRepository.IRepository<T>** - UNIVERSAL
   - Interface contract for all repositories
   - Defined in generic base

4. **Microsoft.EntityFrameworkCore** - UNIVERSAL
   - DbSet, DbContext, LINQ-to-SQL
   - Include/ThenInclude, AsTracking/AsNoTracking
   - Async operations: ToListAsync, FirstOrDefaultAsync, etc.

**Secondary Dependencies (Domain-Specific):**

5. **Microsoft.AspNetCore.Mvc.Rendering** (15+ repositories)
   - SelectListItem for UI dropdowns
   - Used by: Abastecimento, AspNetUsers, Contrato, Veiculo, Motorista, etc.

6. **Repository-to-Repository References** (MINIMAL)
   - AlertasFrotiXRepository → AlertasUsuarioRepository (optional reference)
   - Mostly avoided through abstraction layers

7. **FrotiX.Models** (All repositories)
   - Entity models (Viagem, Motorista, Contrato, etc.)
   - Required for generic parameter T

---

## Cross-Cutting Concerns

### Error Handling Pattern
All repositories use `Alerta.TratamentoErroComLinha()` for consistent error logging:
```csharp
catch (Exception ex)
{
    Alerta.TratamentoErroComLinha("ClassName.cs", "MethodName", ex);
    // Handle gracefully or rethrow
}
```

### DbContext Configuration
- Global `NoTracking` by default (performance optimization)
- `AsTracking()` forced when writes are needed
- Separate count queries from data queries (ViagemRepository example)

### Lazy Initialization Pattern
UnitOfWork uses property-based lazy loading for repositories:
```csharp
public ViagemRepository Viagem { get; set; }
// Initialized in constructor: new ViagemRepository(_db)
```

---

## Performance Optimizations

1. **ViewViagens Used:** ViagemRepository avoids complex JOINs by using denormalized views
2. **Stopwatch Logging:** GetViagensEventoPaginadoAsync includes performance metrics
3. **Two-Phase Queries:** COUNT separate from SELECT for large datasets
4. **Skip/Take Pagination:** Implemented in critical repository methods
5. **AsNoTracking:** Default for read-only operations
6. **AsTracking:** Explicit for write operations

---

## Architecture Patterns Applied

1. **Generic Repository Pattern** - Base Repository<T> for common CRUD
2. **Unit of Work Pattern** - UnitOfWork coordinates multiple repositories
3. **Dependency Injection** - All repositories accept DbContext in constructor
4. **Interface-Based Design** - Every repository implements IRepository<T> variant
5. **Lazy Evaluation** - IQueryable returned where appropriate
6. **Async-First** - Async methods preferred over synchronous

---

## File Organization

```
Repository/
├── Repository.cs                          (Generic base)
├── IRepository/
│   ├── IRepository.cs                     (Generic interface)
│   └── I*.cs                              (50+ specific interfaces)
├── *.Repository.cs                        (50+ concrete repositories)
├── ViewViagens*.cs, View*.cs              (35+ view repositories)
├── UnitOfWork.cs                          (Main orchestrator)
├── UnitOfWork.*.cs                        (Partial extensions)
└── TextNormalization/Repository/
    └── ProperDataRepository.cs            (Text normalization)
```

---

## Key Statistics

- **Total Files:** 106
- **Concrete Repositories:** 51
- **View Repositories:** 35
- **Interface Files:** 19
- **Base/Utility Files:** 1 (Repository.cs)
- **UnitOfWork Files:** 2 (main + extensions)
- **Special Repositories:** 1 (TextNormalization)

- **Total DbSet Dependencies:** 80+
- **SelectListItem Dependencies:** 15+
- **Cross-Repository References:** < 5 (minimal coupling)

---

## Conclusion

The FrotiX Repository layer demonstrates **excellent separation of concerns** with:
- Minimal cross-repository dependencies
- Consistent generic base for standardization
- Clear interface contracts for testability
- Efficient data access through views and optimized queries
- Centralized coordination through Unit of Work pattern

This architecture supports both read-heavy operations (analytics dashboards) and write-heavy operations (transaction processing) effectively.
