# RELATÓRIO DE AUDITORIA COMPLETA DO BANCO DE DADOS FROTIX

**Data da análise:** 06/02/2026
**Banco de dados:** Frotix (SQL Server 2022 v16.00.1165)
**Arquivo analisado:** `Frotix.sql` (13.502 linhas)
**Código-fonte analisado:** 94 Controllers (Site) + 12 Controllers (Api) + 168 Pages + 235 Models

---

## SUMÁRIO EXECUTIVO

| Categoria | Quantidade |
|---|---|
| Tabelas | ~70 |
| Views | ~40 |
| Triggers | 15 |
| Stored Procedures | 25+ |
| Functions | 6 |
| Foreign Keys existentes | ~80 |
| **FKs faltantes (críticas)** | **14** |
| **FKs duplicadas (remover)** | **4** |
| **Índices faltantes (recomendados)** | **12** |
| **Índices redundantes/sobrepostos** | **6** |
| **Problemas estruturais** | **3** |

---

## PARTE 1 — PROBLEMAS ESTRUTURAIS CRÍTICOS

### 1.1 Tabela Fornecedor: FornecedorId NULL (SEM PRIMARY KEY)

**Problema:** A tabela `Fornecedor` define `FornecedorId` como `NULL` em vez de `NOT NULL`. Usa um `UNIQUE INDEX` em vez de `PRIMARY KEY`. Isso é semanticamente incorreto — a coluna funciona como PK mas permite NULLs.

**Impacto:** Possibilidade teórica de inserir um Fornecedor sem ID. FKs referenciando essa coluna (Contrato, AtaRegistroPrecos) funcionam apenas porque o índice único existe, mas a integridade não é completa.

```sql
-- ============================================================
-- SCRIPT 1.1: Corrigir Fornecedor - Adicionar PRIMARY KEY
-- Propósito: Converter FornecedorId de NULL + UNIQUE INDEX
-- para NOT NULL + PRIMARY KEY, garantindo integridade referencial
-- ============================================================
-- PASSO 1: Verificar se há registros com FornecedorId NULL
SELECT COUNT(*) AS RegistrosComIdNull
FROM dbo.Fornecedor
WHERE FornecedorId IS NULL;
-- Se retornar 0, prosseguir. Se > 0, tratar antes.

-- PASSO 2: Remover o índice único existente
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'KEY_Fornecedor_FornecedorId' AND object_id = OBJECT_ID('dbo.Fornecedor'))
    DROP INDEX KEY_Fornecedor_FornecedorId ON dbo.Fornecedor;
GO

-- PASSO 3: Alterar coluna para NOT NULL
ALTER TABLE dbo.Fornecedor ALTER COLUMN FornecedorId uniqueidentifier NOT NULL;
GO

-- PASSO 4: Adicionar PRIMARY KEY
ALTER TABLE dbo.Fornecedor ADD CONSTRAINT PK_Fornecedor_FornecedorId PRIMARY KEY CLUSTERED (FornecedorId);
GO

PRINT 'Fornecedor: PRIMARY KEY criada com sucesso.';
```

---

### 1.2 FKs Duplicadas — WhatsAppMensagens e WhatsAppFilaMensagens

**Problema:** A tabela `WhatsAppMensagens` possui DUAS FKs para `InstanciaId`:
1. FK anônima (linha 167): `FOREIGN KEY (InstanciaId) REFERENCES WhatsAppInstancias(Id)`
2. FK nomeada `FK_WhatsAppMensagens_InstanciaId` (linha 185): mesma referência

O mesmo ocorre com `WhatsAppFilaMensagens.MensagemId`:
1. FK anônima (linha 244): `FOREIGN KEY (MensagemId) REFERENCES WhatsAppMensagens(Id)`
2. FK nomeada `FK_WhatsAppFilaMensagens_MensagemId` (linha 253): mesma referência

**Impacto:** Duplicidade desnecessária. O SQL Server valida ambas as FKs em cada INSERT/UPDATE, causando overhead duplo desnecessário.

```sql
-- ============================================================
-- SCRIPT 1.2: Remover FKs duplicadas
-- Propósito: Eliminar FKs anônimas duplicadas que causam
-- overhead de validação dupla em INSERT/UPDATE
-- ============================================================

-- Primeiro, identificar as FKs anônimas
-- (Nomes gerados pelo SQL Server, começam com 'FK__')
SELECT
    fk.name AS NomeFK,
    OBJECT_NAME(fk.parent_object_id) AS Tabela,
    COL_NAME(fkc.parent_object_id, fkc.parent_column_id) AS Coluna,
    OBJECT_NAME(fk.referenced_object_id) AS TabelaReferenciada
FROM sys.foreign_keys fk
JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
WHERE OBJECT_NAME(fk.parent_object_id) IN ('WhatsAppMensagens', 'WhatsAppFilaMensagens')
ORDER BY Tabela, Coluna;
GO

-- REMOVER as FKs ANÔNIMAS (que começam com FK__), mantendo as nomeadas
-- Exemplo (ajustar o nome real conforme resultado da query acima):
-- ALTER TABLE dbo.WhatsAppMensagens DROP CONSTRAINT [FK__WhatsAppM__Insta__XXXXXXXX];
-- ALTER TABLE dbo.WhatsAppFilaMensagens DROP CONSTRAINT [FK__WhatsAppF__Mensa__XXXXXXXX];

-- Nota: Execute a query acima primeiro para identificar os nomes exatos.
```

---

### 1.3 FKs Duplicadas — MotoristaItensPendentes

**Problema:** A tabela `MotoristaItensPendentes` possui FKs duplicadas:
- `FK_MotoristaItensPendentes_Motorista` e `FK_MotoristaItensPendentes_MotoristaId` (ambas em MotoristaId)
- `FK_MotoristaItensPendentes_Viagem` e `FK_MotoristaItensPendentes_ViagemId` (ambas em ViagemId)

```sql
-- ============================================================
-- SCRIPT 1.3: Remover FKs duplicadas em MotoristaItensPendentes
-- Propósito: Eliminar duplicidades nas constraints de FK
-- ============================================================

-- Manter as versões com sufixo _Id (mais descritivas), remover as genéricas
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_MotoristaItensPendentes_Motorista')
    ALTER TABLE dbo.MotoristaItensPendentes DROP CONSTRAINT FK_MotoristaItensPendentes_Motorista;
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_MotoristaItensPendentes_Viagem')
    ALTER TABLE dbo.MotoristaItensPendentes DROP CONSTRAINT FK_MotoristaItensPendentes_Viagem;
GO

PRINT 'MotoristaItensPendentes: FKs duplicadas removidas.';
```

---

## PARTE 2 — FOREIGN KEYS FALTANTES

### 2.1 Viagem — Colunas de Usuário sem FK

**Tabela:** `Viagem`
**Colunas afetadas:** `UsuarioIdCriacao`, `UsuarioIdFinalizacao`, `UsuarioIdAgendamento`, `UsuarioIdCancelamento`, `VistoriadorInicialId`, `VistoriadorFinalId`

**Motivo:** Estas colunas armazenam IDs de AspNetUsers mas não possuem FK. Isso permite inserir IDs de usuários inexistentes.

**Análise de risco:** MÉDIO. A tabela Viagem é a mais movimentada do sistema (~100 colunas, ~40 índices). Adicionar 6 FKs impactará performance de INSERT/UPDATE. **Recomendação: Adicionar apenas para UsuarioIdCriacao** (campo obrigatório) e usar validação no código para os demais.

```sql
-- ============================================================
-- SCRIPT 2.1: FK para Viagem.UsuarioIdCriacao → AspNetUsers
-- Propósito: Garantir que o usuário criador de uma viagem
-- exista no sistema. Apenas UsuarioIdCriacao é recomendado
-- como FK para não impactar performance de INSERT/UPDATE
-- na tabela mais movimentada do sistema.
-- ============================================================

-- Verificar se há registros órfãos ANTES de criar a FK
SELECT COUNT(*) AS ViagensComUsuarioInvalido
FROM dbo.Viagem v
WHERE v.UsuarioIdCriacao IS NOT NULL
  AND v.UsuarioIdCriacao <> ''
  AND NOT EXISTS (SELECT 1 FROM dbo.AspNetUsers u WHERE u.Id = v.UsuarioIdCriacao);
GO

-- Se retornar 0, prosseguir:
ALTER TABLE dbo.Viagem WITH NOCHECK
  ADD CONSTRAINT FK_Viagem_UsuarioIdCriacao
  FOREIGN KEY (UsuarioIdCriacao) REFERENCES dbo.AspNetUsers (Id);
GO

PRINT 'Viagem: FK UsuarioIdCriacao criada com sucesso.';
```

---

### 2.2 Lavador — ContratoId sem FK

**Tabela:** `Lavador`
**Coluna:** `ContratoId`
**Referência faltante:** `Contrato(ContratoId)`

**Análise:** As tabelas `Operador` e `Encarregado` possuem FK para ContratoId, mas `Lavador` não. Inconsistência.

```sql
-- ============================================================
-- SCRIPT 2.2: FK para Lavador.ContratoId → Contrato
-- Propósito: Manter consistência com Operador e Encarregado
-- que já possuem FK para ContratoId. Garante que um lavador
-- só possa ser vinculado a um contrato existente.
-- ============================================================

-- Verificar órfãos
SELECT COUNT(*) AS LavadoresComContratoInvalido
FROM dbo.Lavador l
WHERE l.ContratoId IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.Contrato c WHERE c.ContratoId = l.ContratoId);
GO

-- Se retornar 0:
ALTER TABLE dbo.Lavador WITH NOCHECK
  ADD CONSTRAINT FK_Lavador_ContratoId
  FOREIGN KEY (ContratoId) REFERENCES dbo.Contrato (ContratoId);
GO

PRINT 'Lavador: FK ContratoId criada com sucesso.';
```

---

### 2.3 MovimentacaoEmpenhoMulta — EmpenhoMultaId sem FK

**Tabela:** `MovimentacaoEmpenhoMulta`
**Coluna:** `EmpenhoMultaId`
**Referência faltante:** `EmpenhoMulta(EmpenhoMultaId)`

```sql
-- ============================================================
-- SCRIPT 2.3: FK para MovimentacaoEmpenhoMulta.EmpenhoMultaId
-- Propósito: Garantir integridade entre movimentações de
-- empenho de multa e o empenho de origem. Sem esta FK,
-- é possível criar movimentações para empenhos inexistentes.
-- ============================================================

SELECT COUNT(*) AS MovimentacoesOrfas
FROM dbo.MovimentacaoEmpenhoMulta m
WHERE m.EmpenhoMultaId IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.EmpenhoMulta e WHERE e.EmpenhoMultaId = m.EmpenhoMultaId);
GO

ALTER TABLE dbo.MovimentacaoEmpenhoMulta WITH NOCHECK
  ADD CONSTRAINT FK_MovimentacaoEmpenhoMulta_EmpenhoMultaId
  FOREIGN KEY (EmpenhoMultaId) REFERENCES dbo.EmpenhoMulta (EmpenhoMultaId);
GO

PRINT 'MovimentacaoEmpenhoMulta: FK EmpenhoMultaId criada.';
```

---

### 2.4 AlertasFrotiX — UsuarioCriadorId sem FK

**Tabela:** `AlertasFrotiX`
**Coluna:** `UsuarioCriadorId` (nvarchar(450), NOT NULL)
**Referência faltante:** `AspNetUsers(Id)`

```sql
-- ============================================================
-- SCRIPT 2.4: FK para AlertasFrotiX.UsuarioCriadorId
-- Propósito: Garantir que o criador de um alerta é um
-- usuário válido. Campo NOT NULL então todo alerta tem
-- um criador — deve apontar para um usuário existente.
-- ============================================================

SELECT COUNT(*) AS AlertasComCriadorInvalido
FROM dbo.AlertasFrotiX a
WHERE NOT EXISTS (SELECT 1 FROM dbo.AspNetUsers u WHERE u.Id = a.UsuarioCriadorId);
GO

ALTER TABLE dbo.AlertasFrotiX WITH NOCHECK
  ADD CONSTRAINT FK_AlertasFrotiX_UsuarioCriadorId
  FOREIGN KEY (UsuarioCriadorId) REFERENCES dbo.AspNetUsers (Id);
GO

PRINT 'AlertasFrotiX: FK UsuarioCriadorId criada.';
```

---

### 2.5 Motorista — FKs de referência cruzada faltantes

**Tabela:** `Motorista`
**Colunas sem FK:** `UnidadeId`, `ContratoId`, `CondutorId`

**Observação:** Estas colunas são referenciadas em Views (ViewMotoristas faz JOIN com Unidade, Contrato, CondutorApoio), mas as FKs não existem no esquema.

```sql
-- ============================================================
-- SCRIPT 2.5: FKs para Motorista → Unidade, Contrato, CondutorApoio
-- Propósito: Garantir integridade referencial nas 3 colunas
-- de FK que as Views já assumem existir via JOINs.
-- ============================================================

-- 2.5a: Motorista.UnidadeId → Unidade
SELECT COUNT(*) AS MotoristasComUnidadeInvalida
FROM dbo.Motorista m
WHERE m.UnidadeId IS NOT NULL
  AND m.UnidadeId <> '00000000-0000-0000-0000-000000000000'
  AND NOT EXISTS (SELECT 1 FROM dbo.Unidade u WHERE u.UnidadeId = m.UnidadeId);
GO

ALTER TABLE dbo.Motorista WITH NOCHECK
  ADD CONSTRAINT FK_Motorista_UnidadeId
  FOREIGN KEY (UnidadeId) REFERENCES dbo.Unidade (UnidadeId);
GO

-- 2.5b: Motorista.ContratoId → Contrato
SELECT COUNT(*) AS MotoristasComContratoInvalido
FROM dbo.Motorista m
WHERE m.ContratoId IS NOT NULL
  AND m.ContratoId <> '00000000-0000-0000-0000-000000000000'
  AND NOT EXISTS (SELECT 1 FROM dbo.Contrato c WHERE c.ContratoId = m.ContratoId);
GO

ALTER TABLE dbo.Motorista WITH NOCHECK
  ADD CONSTRAINT FK_Motorista_ContratoId
  FOREIGN KEY (ContratoId) REFERENCES dbo.Contrato (ContratoId);
GO

-- 2.5c: Motorista.CondutorId → CondutorApoio
SELECT COUNT(*) AS MotoristasComCondutorInvalido
FROM dbo.Motorista m
WHERE m.CondutorId IS NOT NULL
  AND m.CondutorId <> '00000000-0000-0000-0000-000000000000'
  AND NOT EXISTS (SELECT 1 FROM dbo.CondutorApoio c WHERE c.CondutorId = m.CondutorId);
GO

ALTER TABLE dbo.Motorista WITH NOCHECK
  ADD CONSTRAINT FK_Motorista_CondutorId
  FOREIGN KEY (CondutorId) REFERENCES dbo.CondutorApoio (CondutorId);
GO

PRINT 'Motorista: 3 FKs criadas (UnidadeId, ContratoId, CondutorId).';
```

---

### 2.6 Manutencao — UsuarioId's sem FK

**Tabela:** `Manutencao`
**Colunas sem FK:** `IdUsuarioCriacao`, `IdUsuarioCancelamento`, `IdUsuarioFinalizacao`

**Nota:** Apenas `IdUsuarioAlteracao` já possui FK. As outras 3 colunas de usuário não possuem.

```sql
-- ============================================================
-- SCRIPT 2.6: FKs para Manutencao → AspNetUsers (colunas de usuário)
-- Propósito: Completar a integridade referencial — apenas
-- IdUsuarioAlteracao possui FK, as outras 3 colunas não.
-- ============================================================

-- 2.6a: Manutencao.IdUsuarioCriacao
SELECT COUNT(*) AS ManutencoesCriadorInvalido
FROM dbo.Manutencao m
WHERE m.IdUsuarioCriacao IS NOT NULL AND m.IdUsuarioCriacao <> ''
  AND NOT EXISTS (SELECT 1 FROM dbo.AspNetUsers u WHERE u.Id = m.IdUsuarioCriacao);
GO

ALTER TABLE dbo.Manutencao WITH NOCHECK
  ADD CONSTRAINT FK_Manutencao_IdUsuarioCriacao
  FOREIGN KEY (IdUsuarioCriacao) REFERENCES dbo.AspNetUsers (Id);
GO

-- 2.6b: Manutencao.IdUsuarioFinalizacao
ALTER TABLE dbo.Manutencao WITH NOCHECK
  ADD CONSTRAINT FK_Manutencao_IdUsuarioFinalizacao
  FOREIGN KEY (IdUsuarioFinalizacao) REFERENCES dbo.AspNetUsers (Id);
GO

-- 2.6c: Manutencao.IdUsuarioCancelamento
ALTER TABLE dbo.Manutencao WITH NOCHECK
  ADD CONSTRAINT FK_Manutencao_IdUsuarioCancelamento
  FOREIGN KEY (IdUsuarioCancelamento) REFERENCES dbo.AspNetUsers (Id);
GO

PRINT 'Manutencao: 3 FKs de usuario criadas.';
```

---

### 2.7 ItensManutencao.ViagemId sem FK

**Tabela:** `ItensManutencao`
**Coluna:** `ViagemId`
**Referência faltante:** `Viagem(ViagemId)`

```sql
-- ============================================================
-- SCRIPT 2.7: FK para ItensManutencao.ViagemId → Viagem
-- Propósito: Garantir que um item de manutenção vinculado
-- a uma viagem aponte para uma viagem existente. A View
-- ViewItensManutencao já faz JOIN implícito com Viagem.
-- ============================================================

SELECT COUNT(*) AS ItensComViagemInvalida
FROM dbo.ItensManutencao i
WHERE i.ViagemId IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.Viagem v WHERE v.ViagemId = i.ViagemId);
GO

ALTER TABLE dbo.ItensManutencao WITH NOCHECK
  ADD CONSTRAINT FK_ItensManutencao_ViagemId
  FOREIGN KEY (ViagemId) REFERENCES dbo.Viagem (ViagemId);
GO

PRINT 'ItensManutencao: FK ViagemId criada.';
```

---

### 2.8 Tabelas de Estatísticas — VeiculoId sem FK

**Tabelas:** `HeatmapAbastecimentoMensal`, `HeatmapViagensMensal`, `EvolucaoViagensDiaria`, `EstatisticaAbastecimentoVeiculoMensal`
**Coluna:** `VeiculoId` e/ou `MotoristaId`

**Análise:** Estas são tabelas de **estatísticas pré-calculadas** populadas por SPs (DELETE/INSERT em batch). Adicionar FKs aqui **NÃO É RECOMENDADO** porque:
1. As SPs fazem DELETE FROM + INSERT em transação — FKs em VeiculoId/MotoristaId bloqueariam desnecessariamente
2. Os dados são derivados de tabelas que já têm FKs
3. O overhead de validação em batch operations seria significativo

**Decisão: NÃO adicionar FKs em tabelas de estatísticas.** Isso é intencional e correto.

---

## PARTE 3 — ÍNDICES FALTANTES (Baseado em Padrões de Acesso)

### 3.1 Abastecimento — Índice composto para Dashboard

**Problema:** O `AbastecimentoController.DashboardAPI` filtra extensivamente por `DataHora` + `VeiculoId` + `MotoristaId`, mas o índice existente `IX_Abastecimento_VeiculoId_DataHora` não cobre queries por `MotoristaId` + `DataHora`.

```sql
-- ============================================================
-- SCRIPT 3.1: Índice cobrindo para Abastecimento Dashboard
-- Propósito: Cobrir o padrão de query mais comum no dashboard
-- de abastecimentos: filtro por DataHora + agrupamento por
-- VeiculoId, MotoristaId, TipoCombustivel com SUM de valores.
-- Atual: IX_Abastecimento_VeiculoId_DataHora não cobre queries
-- que filtram por MotoristaId.
-- ============================================================

CREATE NONCLUSTERED INDEX IX_Abastecimento_DataHora_Cobertura
ON dbo.Abastecimento (DataHora DESC)
INCLUDE (VeiculoId, MotoristaId, CombustivelId, ValorTotal, LitrosAbastecidos, ValorUnitario, TipoCombustivel, Categoria)
WITH (FILLFACTOR = 90, ONLINE = ON)
ON [PRIMARY];
GO

PRINT 'Abastecimento: Índice de cobertura para Dashboard criado.';
```

---

### 3.2 Abastecimento — Índice para MotoristaId + DataHora

**Problema:** Dashboard de motoristas consulta abastecimentos por MotoristaId dentro de período.

```sql
-- ============================================================
-- SCRIPT 3.2: Índice para queries de Motorista no Dashboard
-- Propósito: Acelerar consultas WHERE MotoristaId = @id
-- AND DataHora BETWEEN @inicio AND @fim, que ocorrem no
-- DashboardMotoristasController para calcular totais de
-- abastecimento por motorista.
-- ============================================================

CREATE NONCLUSTERED INDEX IX_Abastecimento_MotoristaId_DataHora
ON dbo.Abastecimento (MotoristaId, DataHora DESC)
INCLUDE (ValorTotal, LitrosAbastecidos, CombustivelId)
WHERE (MotoristaId IS NOT NULL)
WITH (FILLFACTOR = 90, ONLINE = ON)
ON [PRIMARY];
GO

PRINT 'Abastecimento: Índice MotoristaId + DataHora criado.';
```

---

### 3.3 CorridasTaxiLeg — Sem nenhum índice

**Problema:** Tabela `CorridasTaxiLeg` possui apenas a PK. Sem índices para consultas de período ou agrupamento.

```sql
-- ============================================================
-- SCRIPT 3.3: Índices para CorridasTaxiLeg
-- Propósito: A tabela não possui NENHUM índice além da PK.
-- Queries típicas filtram por DataAgenda (período) e agrupam
-- por Setor/Unidade/MotivoUso.
-- ============================================================

CREATE NONCLUSTERED INDEX IX_CorridasTaxiLeg_DataAgenda
ON dbo.CorridasTaxiLeg (DataAgenda DESC)
INCLUDE (CorridaId, Setor, Unidade, Valor, KmReal, QtdPassageiros)
WITH (FILLFACTOR = 90)
ON [PRIMARY];
GO

PRINT 'CorridasTaxiLeg: Índice DataAgenda criado.';
```

---

### 3.4 CorridasCanceladasTaxiLeg — Sem nenhum índice

```sql
-- ============================================================
-- SCRIPT 3.4: Índice para CorridasCanceladasTaxiLeg
-- Propósito: Idem — zero índices além da PK. Filtros comuns
-- são por DataAgenda e TipoCancelamento.
-- ============================================================

CREATE NONCLUSTERED INDEX IX_CorridasCanceladasTaxiLeg_DataAgenda
ON dbo.CorridasCanceladasTaxiLeg (DataAgenda DESC)
INCLUDE (TipoCancelamento, MotivoCancelamento, TempoEspera)
WITH (FILLFACTOR = 90)
ON [PRIMARY];
GO

PRINT 'CorridasCanceladasTaxiLeg: Índice DataAgenda criado.';
```

---

### 3.5 MovimentacaoEmpenhoMulta — Sem nenhum índice

**Problema:** Tabela sem índices. Queries de relatório de multas fazem JOIN por MultaId e EmpenhoMultaId.

```sql
-- ============================================================
-- SCRIPT 3.5: Índices para MovimentacaoEmpenhoMulta
-- Propósito: Tabela sem índices — precisa de cobertura para
-- JOINs com Multa e EmpenhoMulta feitos nas Views de empenho.
-- ============================================================

CREATE NONCLUSTERED INDEX IX_MovimentacaoEmpenhoMulta_MultaId
ON dbo.MovimentacaoEmpenhoMulta (MultaId)
INCLUDE (EmpenhoMultaId, DataMovimentacao, Valor)
ON [PRIMARY];
GO

CREATE NONCLUSTERED INDEX IX_MovimentacaoEmpenhoMulta_EmpenhoMultaId
ON dbo.MovimentacaoEmpenhoMulta (EmpenhoMultaId)
INCLUDE (MultaId, DataMovimentacao, Valor)
ON [PRIMARY];
GO

PRINT 'MovimentacaoEmpenhoMulta: 2 índices criados.';
```

---

### 3.6 NotaFiscal — Índices para EmpenhoId e ContratoId

**Problema:** ViewEmpenhos faz LEFT JOIN com NotaFiscal por EmpenhoId, mas não há índice dedicado.

```sql
-- ============================================================
-- SCRIPT 3.6: Índices para NotaFiscal
-- Propósito: ViewEmpenhos faz LEFT JOIN NotaFiscal ON
-- Empenho.EmpenhoId = NotaFiscal.EmpenhoId. Sem índice,
-- o SQL Server faz TABLE SCAN na NotaFiscal para cada empenho.
-- ============================================================

CREATE NONCLUSTERED INDEX IX_NotaFiscal_EmpenhoId
ON dbo.NotaFiscal (EmpenhoId)
INCLUDE (NotaFiscalId, ValorNF, ValorGlosa, DataEmissao, ContratoId)
WHERE (EmpenhoId IS NOT NULL)
WITH (FILLFACTOR = 90)
ON [PRIMARY];
GO

CREATE NONCLUSTERED INDEX IX_NotaFiscal_ContratoId_AnoMes
ON dbo.NotaFiscal (ContratoId, AnoReferencia DESC, MesReferencia DESC)
INCLUDE (NotaFiscalId, NumeroNF, ValorNF, EmpenhoId)
WHERE (ContratoId IS NOT NULL)
WITH (FILLFACTOR = 90)
ON [PRIMARY];
GO

PRINT 'NotaFiscal: 2 índices criados.';
```

---

### 3.7 DocumentoContrato — Sem índice em ContratoId

```sql
-- ============================================================
-- SCRIPT 3.7: Índice para DocumentoContrato.ContratoId
-- Propósito: Queries de documentos de contrato filtram por
-- ContratoId. Sem índice, qualquer listagem de documentos
-- de um contrato faz TABLE SCAN.
-- ============================================================

CREATE NONCLUSTERED INDEX IX_DocumentoContrato_ContratoId
ON dbo.DocumentoContrato (ContratoId)
INCLUDE (DocumentoContratoId, TipoDocumento, Descricao)
ON [PRIMARY];
GO

PRINT 'DocumentoContrato: Índice ContratoId criado.';
```

---

### 3.8 Contatos — Índice para Nome (busca textual)

```sql
-- ============================================================
-- SCRIPT 3.8: Índice para Contatos.Nome
-- Propósito: WhatsApp contacts são pesquisados por nome.
-- O índice existente em Celular e Email não cobre buscas
-- por nome.
-- ============================================================

CREATE NONCLUSTERED INDEX IX_Contatos_Nome
ON dbo.Contatos (Nome)
INCLUDE (Celular, Email, Ativo)
WHERE (Ativo = 1)
ON [PRIMARY];
GO

PRINT 'Contatos: Índice Nome criado.';
```

---

## PARTE 4 — ÍNDICES REDUNDANTES/SOBREPOSTOS (Candidatos a Remoção)

### 4.1 Viagem — Índices sobrepostos no EventoId

**Problema:** Existem 3 índices similares no EventoId:
1. `IX_Viagem_EventoId` — INCLUDE (custos)
2. `IX_Viagem_EventoId_Custos` — INCLUDE (mesmos custos) WHERE EventoId IS NOT NULL
3. `IX_Viagem_EventoId_Include_Custos` — INCLUDE (mesmos custos) WHERE EventoId IS NOT NULL

Os índices #2 e #3 são **idênticos** — mesma coluna chave, mesmas colunas INCLUDE, mesmo filtro.

```sql
-- ============================================================
-- SCRIPT 4.1: Remover índice duplicado em Viagem.EventoId
-- Propósito: IX_Viagem_EventoId_Custos e
-- IX_Viagem_EventoId_Include_Custos são idênticos.
-- Manter apenas um deles.
-- ============================================================

-- Verificar se são realmente idênticos
SELECT
    i.name AS IndexName,
    ic.key_ordinal, ic.is_included_column,
    COL_NAME(ic.object_id, ic.column_id) AS ColumnName
FROM sys.indexes i
JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
WHERE i.object_id = OBJECT_ID('dbo.Viagem')
  AND i.name IN ('IX_Viagem_EventoId_Custos', 'IX_Viagem_EventoId_Include_Custos')
ORDER BY i.name, ic.key_ordinal, ic.is_included_column;
GO

-- Se confirmado idênticos, remover um:
DROP INDEX IX_Viagem_EventoId_Include_Custos ON dbo.Viagem;
GO

PRINT 'Viagem: Índice duplicado EventoId removido.';
```

---

### 4.2 Viagem — IDX_Ficha e IDX_NoFichaVistoria

**Problema:** Ambos indexam a mesma coluna `NoFichaVistoria`:
1. `IDX_Ficha` — ON (NoFichaVistoria)
2. `IDX_NoFichaVistoria` — ON (NoFichaVistoria)

São o mesmo índice com nomes diferentes.

```sql
-- ============================================================
-- SCRIPT 4.2: Remover índice duplicado em NoFichaVistoria
-- Propósito: IDX_Ficha e IDX_NoFichaVistoria indexam a mesma
-- coluna sem nenhuma diferença. Manter apenas um.
-- ============================================================

DROP INDEX IDX_Ficha ON dbo.Viagem;
GO

PRINT 'Viagem: Índice duplicado IDX_Ficha removido (mantido IDX_NoFichaVistoria).';
```

---

### 4.3 Viagem — Sobreposição em MotoristaId

**Problema:** Existem 3 índices com MotoristaId como chave:
1. `IDX_MotoristaId` — ON (MotoristaId) simples
2. `IX_Viagem_MotoristaId` — ON (MotoristaId, DataInicial) INCLUDE (custos, KM)
3. `IX_Viagem_MotoristaId_DataInicial` — ON (MotoristaId, DataInicial) sem INCLUDE

O índice #1 é totalmente coberto pelo #2.
O índice #3 é totalmente coberto pelo #2 (mesmas chaves, #2 tem INCLUDE extra).

```sql
-- ============================================================
-- SCRIPT 4.3: Remover índices redundantes em MotoristaId
-- Propósito: IDX_MotoristaId e IX_Viagem_MotoristaId_DataInicial
-- são totalmente cobertos por IX_Viagem_MotoristaId.
-- ============================================================

DROP INDEX IDX_MotoristaId ON dbo.Viagem;
GO

DROP INDEX IX_Viagem_MotoristaId_DataInicial ON dbo.Viagem;
GO

PRINT 'Viagem: 2 índices redundantes de MotoristaId removidos.';
```

---

### 4.4 Viagem — Sobreposição em VeiculoId

**Mesmo padrão:** `IDX_VeiculoId` (simples) é coberto por `IX_Viagem_VeiculoId` (composto com DataInicial + INCLUDE).

```sql
-- ============================================================
-- SCRIPT 4.4: Remover índice redundante em VeiculoId
-- Propósito: IDX_VeiculoId é coberto por IX_Viagem_VeiculoId
-- que inclui DataInicial como segunda chave + colunas INCLUDE.
-- ============================================================

DROP INDEX IDX_VeiculoId ON dbo.Viagem;
GO

PRINT 'Viagem: Índice redundante IDX_VeiculoId removido.';
```

---

### 4.5 Viagem — Sobreposição em SetorSolicitanteId e RequisitanteId

```sql
-- ============================================================
-- SCRIPT 4.5: Remover índices simples cobertos por compostos
-- Propósito: IDX_SetorId e IDX_Requistante são cobertos
-- pelos índices compostos IX_Viagem_SetorSolicitanteId e
-- IX_Viagem_RequisitanteId respectivamente.
-- ============================================================

DROP INDEX IDX_SetorId ON dbo.Viagem;
GO

DROP INDEX IDX_Requistante ON dbo.Viagem;
GO

PRINT 'Viagem: Índices simples SetorId e RequisitanteId removidos.';
```

---

## PARTE 5 — TABELAS/OBJETOS PARA LIMPEZA

### 5.1 Tabelas BACKUP desnecessárias

```sql
-- ============================================================
-- SCRIPT 5.1: Remover tabelas de backup no schema de produção
-- Propósito: As tabelas _BACKUP existem no schema de produção
-- e não são referenciadas por nenhum código. São lixo de
-- operações manuais anteriores.
-- ============================================================

-- VERIFICAR ANTES se estão sendo usadas:
-- SELECT * FROM dbo.Recurso_BACKUP;  -- ver se tem dados úteis
-- SELECT * FROM dbo.ControleAcesso_BACKUP;

-- Se confirmado desnecessárias:
-- DROP TABLE dbo.Recurso_BACKUP;
-- DROP TABLE dbo.ControleAcesso_BACKUP;

-- NOTA: Comentado por precaução. Descomentar após verificação manual.
```

---

### 5.2 Views duplicadas/obsoletas

**ViewMotoristas_original** — Versão antiga substituída por `ViewMotoristas`. Usa `CondutorApoio` em vez de `TipoCondutor`.

**ViewCalculaMediana (backup)** — Nome indica ser backup.

```sql
-- ============================================================
-- SCRIPT 5.2: Remover views obsoletas
-- Propósito: Views com sufixo "original" ou "backup" no nome
-- indicam versões substituídas que não devem estar em produção.
-- ============================================================

-- VERIFICAR ANTES se há código referenciando estas views:
-- Buscar no código: "ViewMotoristas_original", "ViewCalculaMediana"

-- Se confirmado sem uso:
-- DROP VIEW dbo.ViewMotoristas_original;
-- DROP VIEW dbo.[ViewCalculaMediana (backup)];

-- NOTA: Comentado por precaução.
```

---

## PARTE 6 — STORED PROCEDURES RECOMENDADAS

### 6.1 SP para Recálculo de Estatísticas de Multas por Motorista

**Situação atual:** Não existe uma SP consolidada para estatísticas de multas como existe para viagens e abastecimentos.

```sql
-- ============================================================
-- SCRIPT 6.1: SP para estatísticas de multa por motorista
-- Propósito: Complementar o sistema de estatísticas existente.
-- As tabelas EstatisticaMotoristasMensal já têm campos
-- TotalMultas e ValorTotalMultas, mas dependem das triggers
-- trg_Multa_AtualizarEstatisticasMotoristas. Esta SP permite
-- recálculo forçado quando necessário.
-- ============================================================

CREATE OR ALTER PROCEDURE dbo.sp_RecalcularEstatisticasMultasMotoristas
    @Ano INT = NULL,
    @Mes INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Se não informado, recalcula mês atual e anterior
    IF @Ano IS NULL SET @Ano = YEAR(GETDATE());
    IF @Mes IS NULL SET @Mes = MONTH(GETDATE());

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Atualizar contadores de multas na EstatisticaMotoristasMensal
        UPDATE em
        SET
            em.TotalMultas = ISNULL(m.TotalMultas, 0),
            em.ValorTotalMultas = ISNULL(m.ValorTotal, 0),
            em.DataAtualizacao = GETDATE()
        FROM dbo.EstatisticaMotoristasMensal em
        LEFT JOIN (
            SELECT
                MotoristaId,
                YEAR(Data) AS Ano,
                MONTH(Data) AS Mes,
                COUNT(*) AS TotalMultas,
                SUM(ISNULL(ValorAteVencimento, 0)) AS ValorTotal
            FROM dbo.Multa
            WHERE MotoristaId IS NOT NULL
              AND YEAR(Data) = @Ano AND MONTH(Data) = @Mes
            GROUP BY MotoristaId, YEAR(Data), MONTH(Data)
        ) m ON em.MotoristaId = m.MotoristaId
           AND em.Ano = m.Ano AND em.Mes = m.Mes
        WHERE em.Ano = @Ano AND em.Mes = @Mes;

        COMMIT TRANSACTION;
        PRINT 'Estatísticas de multas recalculadas para ' + CAST(@Ano AS VARCHAR) + '/' + CAST(@Mes AS VARCHAR);
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

PRINT 'SP sp_RecalcularEstatisticasMultasMotoristas criada.';
```

---

## PARTE 7 — SCRIPT DE EXECUÇÃO CONSOLIDADO

### 7.1 Ordem de Execução Recomendada

**FASE 1 — Correções Críticas (executar primeiro, em ambiente de teste):**
1. Script 1.1 — Corrigir Fornecedor PK
2. Script 1.2 — Remover FKs duplicadas (WhatsApp)
3. Script 1.3 — Remover FKs duplicadas (MotoristaItensPendentes)

**FASE 2 — Foreign Keys Faltantes (testar integração com EF Core):**
4. Script 2.1 — FK Viagem.UsuarioIdCriacao
5. Script 2.2 — FK Lavador.ContratoId
6. Script 2.3 — FK MovimentacaoEmpenhoMulta.EmpenhoMultaId
7. Script 2.4 — FK AlertasFrotiX.UsuarioCriadorId
8. Script 2.5 — FKs Motorista (UnidadeId, ContratoId, CondutorId)
9. Script 2.6 — FKs Manutencao (3 colunas de usuário)
10. Script 2.7 — FK ItensManutencao.ViagemId

**FASE 3 — Novos Índices (podem ser aplicados online):**
11. Script 3.1 — Índice Abastecimento Dashboard
12. Script 3.2 — Índice Abastecimento MotoristaId
13. Script 3.3 — Índice CorridasTaxiLeg
14. Script 3.4 — Índice CorridasCanceladasTaxiLeg
15. Script 3.5 — Índices MovimentacaoEmpenhoMulta
16. Script 3.6 — Índices NotaFiscal
17. Script 3.7 — Índice DocumentoContrato
18. Script 3.8 — Índice Contatos

**FASE 4 — Remoção de Redundâncias (após validar que queries não degradam):**
19. Script 4.1 — Remover índice duplicado EventoId
20. Script 4.2 — Remover índice duplicado NoFichaVistoria
21. Script 4.3 — Remover índices redundantes MotoristaId
22. Script 4.4 — Remover índice redundante VeiculoId
23. Script 4.5 — Remover índices simples SetorId e RequisitanteId

**FASE 5 — SP (após validar que não conflita com lógica existente):**
24. Script 6.1 — SP estatísticas de multas

**FASE 6 — Limpeza (após backup completo):**
25. Script 5.1 — Remover tabelas backup
26. Script 5.2 — Remover views obsoletas

---

## PARTE 8 — RESUMO DE IMPACTO ESPERADO

| Ação | Impacto Performance | Impacto Integridade |
|---|---|---|
| Corrigir Fornecedor PK | Neutro | **ALTO** — previne dados inválidos |
| Remover FKs duplicadas | **POSITIVO** — remove validação dupla | Neutro |
| Adicionar FKs faltantes | Leve overhead em INSERT/UPDATE | **ALTO** — previne dados órfãos |
| Novos índices Abastecimento | **ALTO** — acelera dashboards | Neutro |
| Novos índices CorridasTaxi | **ALTO** — acelera queries sem índice | Neutro |
| Novos índices NotaFiscal | **MÉDIO** — acelera ViewEmpenhos | Neutro |
| Remover ~8 índices redundantes | **POSITIVO** — menos overhead em INSERT/UPDATE | Neutro |

---

## NOTAS IMPORTANTES

1. **WITH NOCHECK**: Todas as FKs novas usam `WITH NOCHECK` para não falhar caso existam dados históricos inválidos. Após limpeza de dados, execute `ALTER TABLE ... WITH CHECK CHECK CONSTRAINT` para ativar validação completa.

2. **ONLINE = ON**: Os novos índices usam `ONLINE = ON` quando possível (requer Enterprise Edition). Se usando Standard Edition, remover esta cláusula.

3. **Viagem tem ~40 índices**: Ao remover ~6 redundantes (Fase 4), o ganho em performance de INSERT/UPDATE será significativo para a tabela mais movimentada do sistema.

4. **Entity Framework Core**: Após criar FKs, verificar se o DbContext precisa de atualização nos modelos (navigation properties). As FKs adicionadas com `WITH NOCHECK` não afetam o EF Core imediatamente.

5. **Backup**: SEMPRE fazer backup completo antes de executar qualquer script deste relatório.

---

## ATUALIZAÇÕES PÓS-AUDITORIA (07/02/2026)

1. **Campos de usuário com string vazia**: Foi adicionada a normalização de strings vazias para `NULL` antes da ativação das FKs (`UsuarioIdCriacao`, `IdUsuarioCriacao`, `IdUsuarioFinalizacao`, `IdUsuarioCancelamento`, `UsuarioCriadorId`). Isso evita conflito de FK quando o campo possui `''` em vez de `NULL`.
2. **Triggers de validação**: Triggers de prevenção foram ajustadas para converter vazio → `NULL` e aplicar usuário padrão apenas quando o valor estiver preenchido e inexistente em `AspNetUsers`.
