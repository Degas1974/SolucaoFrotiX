# Índice: Documentação de Repository

> **Última Atualização**: 08/01/2026  
> **Versão**: 1.0

---

## 📋 Status da Documentação

**Total de Arquivos**: ~207 arquivos  
**Documentados (Principais)**: 4/207  
**Padrão Documentado**: ✅ Sim

---

## ✅ Arquivos Base Documentados

- [x] [`Repository.md`](./Repository.md) - Classe base genérica de repositório
- [x] [`UnitOfWork.md`](./UnitOfWork.md) - Padrão Unit of Work (principal + extensões)
- [x] [`PADRAO-REPOSITORIES-ESPECIFICOS.md`](./PADRAO-REPOSITORIES-ESPECIFICOS.md) - Padrão dos repositories específicos
- [x] [`IRepository/IRepository.md`](./IRepository/IRepository.md) - Interface base genérica
- [x] [`IRepository/IUnitOfWork.md`](./IRepository/IUnitOfWork.md) - Interface Unit of Work (principal + extensões)

---

## 📝 Repositories Específicos

**Total**: ~200 repositories específicos seguindo o padrão documentado em `PADRAO-REPOSITORIES-ESPECIFICOS.md`

### Categorias

#### Cadastros (~40 repositories)
- CombustivelRepository, MarcaVeiculoRepository, ModeloVeiculoRepository
- VeiculoRepository, MotoristaRepository, EncarregadoRepository
- OperadorRepository, LavadorRepository
- ContratoRepository, AtaRegistroPrecosRepository
- FornecedorRepository, RequisitanteRepository
- SetorSolicitanteRepository, SetorPatrimonialRepository
- SecaoPatrimonialRepository, PatrimonioRepository
- PlacaBronzeRepository, AspNetUsersRepository, RecursoRepository
- E outros...

#### Operações (~20 repositories)
- ViagemRepository, ViagensEconomildoRepository
- AbastecimentoRepository, LavagemRepository
- ManutencaoRepository, MultaRepository
- EmpenhoRepository, NotaFiscalRepository
- EventoRepository, OcorrenciaViagemRepository
- ViagemEstatisticaRepository
- E outros...

#### Relacionamentos (~15 repositories)
- VeiculoContratoRepository, VeiculoAtaRepository
- MotoristaContratoRepository, OperadorContratoRepository
- EncarregadoContratoRepository, LavadorContratoRepository
- ItemVeiculoContratoRepository, ItemVeiculoAtaRepository
- LavadoresLavagemRepository, LotacaoMotoristaRepository
- E outros...

#### Views (~35 repositories)
- ViewAbastecimentosRepository, ViewVeiculosRepository
- ViewMotoristasRepository, ViewViagensRepository
- ViewCustosViagemRepository, ViewManutencaoRepository
- ViewMultasRepository, ViewEmpenhosRepository
- ViewFluxoEconomildoRepository, ViewLavagemRepository
- ViewEventosRepository, ViewOcorrenciaRepository
- E muitos outros...

#### Especiais (~10 repositories)
- AlertasFrotiXRepository, AlertasUsuarioRepository
- RepactuacaoContratoRepository, RepactuacaoAtaRepository
- RepactuacaoServicosRepository, RepactuacaoTerceirizacaoRepository
- RepactuacaoVeiculoRepository
- CorridasTaxiLegRepository, CorridasCanceladasTaxiLegRepository
- E outros...

---

## 📚 Documentação de Referência

Para entender como os repositories específicos funcionam, consulte:
- [`PADRAO-REPOSITORIES-ESPECIFICOS.md`](./PADRAO-REPOSITORIES-ESPECIFICOS.md) - Padrão completo
- [`Repository.md`](./Repository.md) - Métodos disponíveis na classe base
- [`IRepository/IRepository.md`](./IRepository/IRepository.md) - Contrato base

---

**Última atualização**: 08/01/2026  
**Versão**: 1.0


---

# PARTE 2: LOG DE MODIFICAÃ‡Ã•ES/CORREÃ‡Ã•ES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [19/01/2026] - AtualizaÃ§Ã£o: ImplementaÃ§Ã£o de MÃ©todos com Tracking Seletivo

**DescriÃ§Ã£o**: MigraÃ§Ã£o de chamadas .AsTracking() para novos mÃ©todos GetWithTracking() e GetFirstOrDefaultWithTracking() como parte da otimizaÃ§Ã£o de performance do Entity Framework Core.

**Arquivos Afetados**:
- Este arquivo (uso dos novos mÃ©todos do repositÃ³rio)
- Repository/IRepository/IRepository.cs (definiÃ§Ã£o dos novos mÃ©todos)
- Repository/Repository.cs (implementaÃ§Ã£o)
- RegrasDesenvolvimentoFrotiX.md (seÃ§Ã£o 4.2 - nova regra permanente)

**MudanÃ§as**:
- âŒ **ANTES**: _unitOfWork.Entity.AsTracking().Get(id) ou _unitOfWork.Entity.AsTracking().GetFirstOrDefault(filter)
- âœ… **AGORA**: _unitOfWork.Entity.GetWithTracking(id) ou _unitOfWork.Entity.GetFirstOrDefaultWithTracking(filter)

**Motivo**: 
- OtimizaÃ§Ã£o de memÃ³ria e performance
- Tracking seletivo (apenas quando necessÃ¡rio para Update/Delete)
- PadrÃ£o mais limpo e explÃ­cito
- Conformidade com nova regra permanente (RegrasDesenvolvimentoFrotiX.md seÃ§Ã£o 4.2)

**Impacto**: 
- Melhoria de performance em operaÃ§Ãµes de leitura (usa AsNoTracking por padrÃ£o)
- Tracking correto em operaÃ§Ãµes de escrita (Update/Delete)
- Zero impacto funcional (comportamento mantido)

**Status**: âœ… **ConcluÃ­do**

**ResponsÃ¡vel**: Sistema (AtualizaÃ§Ã£o AutomÃ¡tica)

**VersÃ£o**: Incremento de patch
