# CONTEXTO DE BANCO DE DADOS - FROTIX

> **Instrução:** Este arquivo descreve o esquema do banco de dados SQL Server do projeto FrotiX. Use-o para entender as relações, tabelas principais e regras de negócio implementadas no banco.

---

## 1. Visão Geral
- **SGBD**: SQL Server 2016+
- **Schema**: `dbo` (padrão)
- **Convenções**:
  - Chaves Primárias: `[NomeTabela]Id` (GUID `uniqueidentifier` na maioria, alguns `int identity`)
  - Chaves Estrangeiras: `[NomeTabelaEstrangeira]Id`
  - Soft Delete: Campo `Status` (bit) onde 1 = Ativo, 0 = Inativo (geralmente).
  - Datas: `datetime` ou `datetime2`.

## 2. Tabelas Principais (Core)

### 2.1. Frota e Pessoas
- **`Veiculo`**: Cadastro central de veículos.
  - *PK*: `VeiculoId`
  - *Principais Campos*: `Placa`, `ModeloId`, `MarcaId`, `UnidadeId`, `Status`, `KmAtual`, `CombustivelId`.
  - *Relações*: `ModeloVeiculo`, `MarcaVeiculo`, `Unidade`, `Contrato`.
- **`Motorista`**: Condutores.
  - *PK*: `MotoristaId`
  - *Principais Campos*: `Nome`, `CNH`, `Celular`, `UnidadeId`, `ContratoId`, `Status`.
- **`Unidade`**: Estrutura organizacional (Setores/Departamentos).
  - *PK*: `UnidadeId`
  - *Principais Campos*: `Sigla`, `Descricao`, `Status`.

### 2.2. Operacional
- **`Viagem`**: Registro de deslocamentos.
  - *PK*: `ViagemId`
  - *Principais Campos*: `DataInicial`, `HoraInicio`, `VeiculoId`, `MotoristaId`, `KmInicial`, `KmFinal`, `Status` (Aberta, Realizada, Cancelada, Agendada).
  - *Trigger Importante*: `TR_Viagem_NormalizarMinutos` e `trg_Viagem_AtualizarEstatisticasMotoristas`.
- **`Abastecimento`**: Lançamento de combustível.
  - *PK*: `AbastecimentoId`
  - *Principais Campos*: `VeiculoId`, `MotoristaId`, `Litros`, `ValorUnitario`, `KmRodado`.
  - *Trigger*: `trg_Abastecimento_NormalizacaoAutomatica` (detecta outliers de consumo).
- **`Manutencao`**: Ordens de serviço.
  - *PK*: `ManutencaoId`
  - *Principais Campos*: `VeiculoId`, `DataSolicitacao`, `StatusOS`, `ResumoOS`.
- **`Multa`**: Infrações de trânsito.
  - *PK*: `MultaId`
  - *Principais Campos*: `VeiculoId`, `MotoristaId`, `Valor`, `Data`, `Status`.

### 2.3. Contratos e Financeiro
- **`Contrato`**: Contratos de locação ou serviços.
  - *PK*: `ContratoId`
  - *Relações*: `Fornecedor`, `VeiculoContrato` (N-N), `MotoristaContrato` (N-N).
- **`Fornecedor`**: Empresas prestadoras.
- **`Empenho`**: Controle orçamentário.

---

## 3. Automação e Inteligência (Banco de Dados)

### 3.1. Tabelas de Estatísticas (Data Warehouse Interno)
O sistema possui tabelas desnormalizadas para performance de dashboards, atualizadas via Triggers/Procedures:
- `EstatisticaGeralMensal`
- `EstatisticaMotoristasMensal`
- `EstatisticaVeiculoUsoMensal`
- `ViagemEstatistica`
- `HeatmapViagensMensal`

### 3.2. Procedures Críticas
- **`sp_JobAtualizacaoViagens`**: "Job" mestre que roda periodicamente para:
  1. Normalizar abastecimentos (`sp_NormalizarAbastecimentos`).
  2. Calcular consumo (`sp_CalcularConsumoVeiculos`).
  3. Atualizar padrões (`sp_AtualizarPadroesVeiculos`).
  4. Normalizar viagens (`sp_NormalizarViagens`).
  5. Recalcular custos (`sp_RecalcularCustosTodasViagens`).
  6. Atualizar estatísticas (`sp_AtualizarTodasEstatisticasViagem`).

### 3.3. Views Estratégicas
- **`ViewViagens`**: Visão completa formatada para grids.
- **`ViewVeiculos`**: Dados do veículo + consumo médio + contrato atual.
- **`ViewManutencao`**: Detalhes da OS + dias parado + custos.

---

## 4. Dicas de Consultas (Snippets)

### 4.1. Filtrar Veículos Ativos
```sql
SELECT * FROM Veiculo WHERE Status = 1
```

### 4.2. Buscar Viagens de um Período
```sql
SELECT * FROM ViewViagens 
WHERE DataInicial >= '2025-01-01' AND DataInicial <= '2025-01-31'
```

### 4.3. Verificar Dependências antes de Delete
(Exemplo da lógica usada no sistema)
```sql
-- Checar se unidade tem veículos
SELECT COUNT(*) FROM Veiculo WHERE UnidadeId = @Id
```

---

## 5. Observações Importantes
- **Integridade Referencial**: Uso extensivo de FKs. Sempre verifique dependências antes de deletar.
- **Normalização de Dados**: O sistema possui rotinas (`sp_Normalizar...`) que corrigem dados sujos (datas invertidas, KM negativo) automaticamente. Não confie cegamente nos dados brutos (`Raw`), prefira os campos normalizados (`...Normalizado`) se existirem.
- **Triggers**: Cuidado com Updates em massa em `Viagem` e `Abastecimento`, pois disparam recálculos pesados de estatísticas. Use `DISABLE TRIGGER` se necessário em migrações grandes.
