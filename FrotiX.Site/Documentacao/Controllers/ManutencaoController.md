# Documentação: ManutencaoController.cs

> **Última Atualização**: 03/02/2026  
> **Versão Atual**: 2.1

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O `ManutencaoController` gerencia operações CRUD de manutenções de veículos, incluindo filtros avançados por veículo, status, mês/ano e período.

**Principais características:**

✅ **CRUD Completo**: Listagem com filtros avançados  
✅ **Filtros Múltiplos**: Por veículo, status, mês/ano, período  
✅ **Cache**: Usa `IMemoryCache` para otimização  
✅ **Upload**: Upload de arquivos relacionados

---

## Endpoints API Principais

### GET `/api/Manutencao`

**Descrição**: Lista manutenções com filtros múltiplos

**Parâmetros**:
- `veiculoId` (string GUID opcional)
- `statusId` (string opcional)
- `mes` (string opcional)
- `ano` (string opcional)
- `dataInicial` (string opcional) - Formatos: dd/MM/yyyy, yyyy-MM-dd, etc.
- `dataFinal` (string opcional)

**Filtros**: Usa `ViewManutencao` com filtro por `DataSolicitacaoRaw`

---

### Endpoints de Lavagem (ControleLavagem)

- `GET /api/Manutencao/ListaLavagens`
- `GET /api/Manutencao/ListaLavagemVeiculos`
- `GET /api/Manutencao/ListaLavagemMotoristas`
- `GET /api/Manutencao/ListaLavagemLavadores`
- `POST /api/Manutencao/InsereLavagem` (payload inclui `HorarioLavagem`)
- `POST /api/Manutencao/InsereLavadoresLavagem`

## Interconexões

### Quem Chama Este Controller

- **Pages**: `Pages/Manutencao/*.cshtml`
- **Pages**: `Pages/Veiculo/*.cshtml` - Para histórico de manutenções

### O Que Este Controller Chama

- **`_unitOfWork.ViewManutencao`**: View otimizada
- **`_unitOfWork.Manutencao`**: CRUD
- **`_cache`**: Cache de memória

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [03/02/2026] - Ajustes no Controle de Lavagem

**Descrição**: Atualização da documentação para refletir o horário único (`HorarioLavagem`) e o campo `Horario` nas listagens de lavagem.

**Arquivos Afetados**:
- `Controllers/ManutencaoController.cs`

**Status**: ✅ **Concluído**

**Versão**: 2.1

---

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação da documentação completa do ManutencaoController

**Arquivos Afetados**:
- `Controllers/ManutencaoController.cs`

**Impacto**: Documentação de referência para operações de manutenções

**Status**: ✅ **Concluído**

**Versão**: 2.0

---

**Última atualização**: 03/02/2026  
**Autor**: Sistema FrotiX  
**Versão**: 2.1
