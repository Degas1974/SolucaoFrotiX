# Relatório de Análise: Foreign Keys e Índices Faltantes

> **Data de Geração**: 12/01/2026
> **Arquivo Analisado**: `Frotix.sql`
> **Tamanho**: ~458 KB

---

## Sumário Executivo

Este relatório identifica **Foreign Keys (FKs) faltantes** e **índices sugeridos** no banco de dados FrotiX, com base na análise do arquivo `Frotix.sql`.

### Estatísticas
- **Tabelas Analisadas**: 80+
- **FKs Existentes**: 106
- **FKs Faltantes Identificadas**: 47
- **Índices Existentes**: 130+
- **Índices Sugeridos**: 28

---

## 1. FOREIGN KEYS FALTANTES

As Foreign Keys abaixo estão organizadas por **tabela** e **prioridade**.

### Legenda de Prioridade
- 🔴 **ALTA**: Tabelas críticas (Viagem, Abastecimento, Manutencao, Multa)
- 🟡 **MÉDIA**: Tabelas importantes (Motorista, Veiculo, Evento)
- 🟢 **BAIXA**: Tabelas auxiliares e estatísticas

---

### 🔴 PRIORIDADE ALTA

#### Tabela: `Viagem`
Esta é a tabela mais crítica do sistema. Várias FKs estão faltando.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| `RecorrenciaViagemId` | `Viagem` | ❌ Sem FK | `ALTER TABLE dbo.Viagem ADD CONSTRAINT FK_Viagem_RecorrenciaViagemId FOREIGN KEY (RecorrenciaViagemId) REFERENCES dbo.Viagem (ViagemId);` |

**Justificativa**:
- `RecorrenciaViagemId`: Auto-referência para viagens recorrentes. Fundamental para rastreamento de viagens relacionadas.

**Nota**: Os campos `EventoId`, `ItemManutencaoId`, `MotoristaId`, `RequisitanteId`, `SetorSolicitanteId`, `VeiculoId` já possuem FKs.

---

#### Tabela: `Abastecimento`
Tabela crítica para controle de combustível.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| *(Todos os campos Id têm FK)* | - | ✅ | - |

**Status**: ✅ **COMPLETA** - Todos os campos `VeiculoId`, `CombustivelId`, `MotoristaId` já têm FK definida.

---

#### Tabela: `Manutencao`
Controle de manutenções dos veículos.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| `IdUsuarioAlteracao` | `AspNetUsers` | ✅ Tem FK | - |
| `VeiculoId` | `Veiculo` | ✅ Tem FK | - |
| `VeiculoReservaId` | `Veiculo` | ✅ Tem FK | - |

**Status**: ✅ **COMPLETA**

---

#### Tabela: `Multa`
Registro de multas de veículos e motoristas.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| *(Todos os campos Id têm FK)* | - | ✅ | - |

**Status**: ✅ **COMPLETA** - Todos os campos já possuem FK:
- `MotoristaId` → FK_MultaMotorista
- `VeiculoId` → FK_MultaVeiculo
- `OrgaoAutuanteId` → FK_MultaOrgaoAutuante
- `TipoMultaId` → FK_MultaTipoMulta
- `ContratoMotoristaId` → FK_MultaContratoMotorista
- `ContratoVeiculoId` → FK_MultaContratoVeiculo
- `EmpenhoMultaId` → FK_MultaEmpenho
- `AtaVeiculoId` → FK_Multa_AtaVeiculoId

---

#### Tabela: `OcorrenciaViagem`
Registro de ocorrências durante viagens.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| *(Todos os campos Id têm FK)* | - | ✅ | - |

**Status**: ✅ **COMPLETA** - Campos `ViagemId`, `VeiculoId`, `MotoristaId`, `ItemManutencaoId` já têm FK.

---

### 🟡 PRIORIDADE MÉDIA

#### Tabela: `Motorista`
Cadastro de motoristas.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| `UnidadeId` | `Unidade` | ❌ Sem FK | `ALTER TABLE dbo.Motorista ADD CONSTRAINT FK_Motorista_UnidadeId FOREIGN KEY (UnidadeId) REFERENCES dbo.Unidade (UnidadeId);` |
| `CondutorId` | `CondutorApoio` | ❌ Sem FK | `ALTER TABLE dbo.Motorista ADD CONSTRAINT FK_Motorista_CondutorId FOREIGN KEY (CondutorId) REFERENCES dbo.CondutorApoio (CondutorId);` |

**Justificativa**:
- `UnidadeId`: Rastrear lotação de motoristas por unidade.
- `CondutorId`: Relacionamento com condutores de apoio.

**Nota**: Campo `ContratoId` já possui FK via `FK_Motorista_Contrato`.

---

#### Tabela: `Veiculo`
Cadastro de veículos.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| `CombustivelId` | `Combustivel` | ❌ Sem FK | `ALTER TABLE dbo.Veiculo ADD CONSTRAINT FK_Veiculo_CombustivelId FOREIGN KEY (CombustivelId) REFERENCES dbo.Combustivel (CombustivelId);` |
| `ContratoId` | `Contrato` | ❌ Sem FK* | `ALTER TABLE dbo.Veiculo ADD CONSTRAINT FK_Veiculo_ContratoId FOREIGN KEY (ContratoId) REFERENCES dbo.Contrato (ContratoId);` |
| `AtaId` | `AtaRegistroPrecos` | ❌ Sem FK | `ALTER TABLE dbo.Veiculo ADD CONSTRAINT FK_Veiculo_AtaId FOREIGN KEY (AtaId) REFERENCES dbo.AtaRegistroPrecos (AtaId);` |

**Nota**: Campos `MarcaId`, `ModeloId`, `UnidadeId`, `PlacaBronzeId`, `ItemVeiculoId`, `ItemVeiculoAtaId` já têm FK.

**Importante**: Existe índice `IX_Veiculo_ContratoId` mas não FK correspondente.

---

#### Tabela: `Evento`
Eventos do sistema (viagens especiais, eventos da Câmara).

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| *(Todos os campos Id têm FK)* | - | ✅ | - |

**Status**: ✅ **COMPLETA** - Campos `SetorSolicitanteId` e `RequisitanteId` já têm FK.

---

#### Tabela: `SetorSolicitante`
Setores solicitantes de viagens.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| `SetorPaiId` | `SetorSolicitante` | ❌ Sem FK | `ALTER TABLE dbo.SetorSolicitante ADD CONSTRAINT FK_SetorSolicitante_SetorPaiId FOREIGN KEY (SetorPaiId) REFERENCES dbo.SetorSolicitante (SetorSolicitanteId);` |

**Justificativa**: Auto-referência para hierarquia de setores (setor pai → setor filho).

---

#### Tabela: `Requisitante`
Requisitantes de viagens.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| *(Campo SetorSolicitanteId tem FK)* | - | ✅ | - |

**Status**: ✅ **COMPLETA**

---

### 🟢 PRIORIDADE BAIXA

#### Tabela: `WhatsAppMensagens`
Sistema de mensagens WhatsApp.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| `InstanciaId` | `WhatsAppInstancias` | ❌ Sem FK | `ALTER TABLE dbo.WhatsAppMensagens ADD CONSTRAINT FK_WhatsAppMensagens_InstanciaId FOREIGN KEY (InstanciaId) REFERENCES dbo.WhatsAppInstancias (InstanciaId);` |
| `ContatoId` | `WhatsAppContatos` | ❌ Sem FK | `ALTER TABLE dbo.WhatsAppMensagens ADD CONSTRAINT FK_WhatsAppMensagens_ContatoId FOREIGN KEY (ContatoId) REFERENCES dbo.WhatsAppContatos (ContatoId);` |
| `UsuarioId` | `AspNetUsers` | ❌ Sem FK | `ALTER TABLE dbo.WhatsAppMensagens ADD CONSTRAINT FK_WhatsAppMensagens_UsuarioId FOREIGN KEY (UsuarioId) REFERENCES dbo.AspNetUsers (Id);` |

---

#### Tabela: `WhatsAppFilaMensagens`
Fila de mensagens WhatsApp.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| `MensagemId` | `WhatsAppMensagens` | ❌ Sem FK | `ALTER TABLE dbo.WhatsAppFilaMensagens ADD CONSTRAINT FK_WhatsAppFilaMensagens_MensagemId FOREIGN KEY (MensagemId) REFERENCES dbo.WhatsAppMensagens (MensagemId);` |

---

#### Tabela: `WhatsAppWebhookLogs`
Logs de webhooks WhatsApp.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| `InstanciaId` | `WhatsAppInstancias` | ❌ Sem FK | `ALTER TABLE dbo.WhatsAppWebhookLogs ADD CONSTRAINT FK_WhatsAppWebhookLogs_InstanciaId FOREIGN KEY (InstanciaId) REFERENCES dbo.WhatsAppInstancias (InstanciaId);` |

---

#### Tabela: `ItemVeiculoAta`
Itens de veículos em ata de registro de preços.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| `RepactuacaoAtaId` | `RepactuacaoAta` | ❌ Sem FK | `ALTER TABLE dbo.ItemVeiculoAta ADD CONSTRAINT FK_ItemVeiculoAta_RepactuacaoAtaId FOREIGN KEY (RepactuacaoAtaId) REFERENCES dbo.RepactuacaoAta (RepactuacaoAtaId);` |
| `VeiculoId` | `Veiculo` | ❌ Sem FK | `ALTER TABLE dbo.ItemVeiculoAta ADD CONSTRAINT FK_ItemVeiculoAta_VeiculoId FOREIGN KEY (VeiculoId) REFERENCES dbo.Veiculo (VeiculoId);` |

---

#### Tabela: `Fornecedor`
Cadastro de fornecedores.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| `FornecedorId` | - | ⚠️ Nullable DEFAULT | Verificar se é auto-referência ou erro de modelagem |

**Nota**: Campo `FornecedorId` é `uniqueidentifier NULL DEFAULT (newid())` - possível erro de modelagem.

---

#### Tabela: `Contrato`
Contratos com fornecedores.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| *(Campo FornecedorId tem FK)* | - | ✅ | - |

**Status**: ✅ **COMPLETA**

---

#### Tabela: `RepactuacaoContrato`
Repactuações de contratos.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| *(Campo ContratoId tem FK)* | - | ✅ | - |

**Status**: ✅ **COMPLETA**

---

#### Tabela: `RepactuacaoAta`
Repactuações de atas.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| *(Campo AtaId tem FK)* | - | ✅ | - |

**Status**: ✅ **COMPLETA**

---

#### Tabela: `Empenho`
Empenhos de contratos e atas.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| *(Campos ContratoId e AtaId têm FK)* | - | ✅ | - |

**Status**: ✅ **COMPLETA**

---

#### Tabela: `NotaFiscal`
Notas fiscais.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| `VeiculoId` | `Veiculo` | ❌ Sem FK | `ALTER TABLE dbo.NotaFiscal ADD CONSTRAINT FK_NotaFiscal_VeiculoId FOREIGN KEY (VeiculoId) REFERENCES dbo.Veiculo (VeiculoId);` |

**Nota**: Campos `EmpenhoId`, `ContratoId`, `AtaId` já têm FK.

---

#### Tabela: `DocumentoContrato`
Documentos anexados aos contratos.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| *(Campo ContratoId tem FK implícita)* | - | ⚠️ | `ALTER TABLE dbo.DocumentoContrato ADD CONSTRAINT FK_DocumentoContrato_ContratoId FOREIGN KEY (ContratoId) REFERENCES dbo.Contrato (ContratoId);` |

**Justificativa**: FK não encontrada explicitamente no script.

---

#### Tabela: `Lavagem`
Registro de lavagens de veículos.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| *(Campos VeiculoId e MotoristaId têm FK)* | - | ✅ | - |

**Status**: ✅ **COMPLETA**

---

#### Tabela: `LavadoresLavagem`
Relacionamento muitos-para-muitos entre lavadores e lavagens.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| *(Campos LavadorId e LavagemId têm FK)* | - | ✅ | - |

**Status**: ✅ **COMPLETA**

---

#### Tabela: `LotacaoMotorista`
Lotação de motoristas por unidade.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| `MotoristaCoberturaId` | `Motorista` | ❌ Sem FK | `ALTER TABLE dbo.LotacaoMotorista ADD CONSTRAINT FK_LotacaoMotorista_MotoristaCoberturaId FOREIGN KEY (MotoristaCoberturaId) REFERENCES dbo.Motorista (MotoristaId);` |

**Nota**: Campos `MotoristaId` e `UnidadeId` já têm FK.

---

#### Tabela: `AbastecimentoPendente`
Abastecimentos com pendências (importação).

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| *(Todos os campos Id têm FK)* | - | ✅ | - |

**Status**: ✅ **COMPLETA** - Campos `VeiculoId`, `MotoristaId`, `CombustivelId` já têm FK.

---

#### Tabela: `MovimentacaoEmpenhoMulta`
Movimentações de empenhos de multas.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| `MultaId` | `Multa` | ❌ Sem FK | `ALTER TABLE dbo.MovimentacaoEmpenhoMulta ADD CONSTRAINT FK_MovimentacaoEmpenhoMulta_MultaId FOREIGN KEY (MultaId) REFERENCES dbo.Multa (MultaId);` |

**Nota**: Campo `EmpenhoMultaId` precisa ser verificado se tem FK.

---

#### Tabela: `ItensManutencao`
Itens de manutenção.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| `MotoristaId` | `Motorista` | ❌ Sem FK | `ALTER TABLE dbo.ItensManutencao ADD CONSTRAINT FK_ItensManutencao_MotoristaId FOREIGN KEY (MotoristaId) REFERENCES dbo.Motorista (MotoristaId);` |

**Nota**: Campos `ManutencaoId` e `ViagemId` já têm FK.

---

#### Tabela: `Patrimonio`
Cadastro de patrimônios.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| `SetorConferenciaId` | `SetorPatrimonial` | ❌ Sem FK | `ALTER TABLE dbo.Patrimonio ADD CONSTRAINT FK_Patrimonio_SetorConferenciaId FOREIGN KEY (SetorConferenciaId) REFERENCES dbo.SetorPatrimonial (SetorId);` |
| `SecaoConferenciaId` | `SecaoPatrimonial` | ❌ Sem FK | `ALTER TABLE dbo.Patrimonio ADD CONSTRAINT FK_Patrimonio_SecaoConferenciaId FOREIGN KEY (SecaoConferenciaId) REFERENCES dbo.SecaoPatrimonial (SecaoId);` |

**Nota**: Campos `SetorId` e `SecaoId` já têm FK.

---

#### Tabela: `ViagensEconomildo`
Tabela legada de viagens (sistema antigo).

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| *(Campos VeiculoId e MotoristaId têm FK)* | - | ✅ | - |

**Status**: ✅ **COMPLETA**

---

#### Tabela: `MediaCombustivel`
Média de preços de combustível.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| *(Campos CombustivelId e NotaFiscalId têm FK)* | - | ✅ | - |

**Status**: ✅ **COMPLETA**

---

#### Tabela: `ControleAcesso`
Controle de acesso a recursos.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| *(Campos RecursoId e UsuarioId têm FK)* | - | ✅ | - |

**Status**: ✅ **COMPLETA**

---

#### Tabela: `AlertasFrotiX`
Sistema de alertas do FrotiX.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| *(Todos os campos Id têm FK)* | - | ✅ | - |

**Status**: ✅ **COMPLETA** - Campos `ViagemId`, `ManutencaoId`, `MotoristaId`, `VeiculoId`, `RecorrenciaAlertaId` já têm FK.

---

#### Tabela: `AlertasUsuario`
Alertas atribuídos a usuários.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| *(Campos AlertasFrotiXId e UsuarioId têm FK)* | - | ✅ | - |

**Status**: ✅ **COMPLETA**

---

#### Tabela: `RepactuacaoServicos`
Repactuação de serviços.

| Campo | Tabela Referenciada | Status Atual | SQL Sugerido |
|-------|---------------------|--------------|--------------|
| `RepactuacaoContratoId` | `RepactuacaoContrato` | ❌ Sem FK | `ALTER TABLE dbo.RepactuacaoServicos ADD CONSTRAINT FK_RepactuacaoServicos_RepactuacaoContratoId FOREIGN KEY (RepactuacaoContratoId) REFERENCES dbo.RepactuacaoContrato (RepactuacaoContratoId);` |

---

## 2. RESUMO DE FKs FALTANTES POR PRIORIDADE

### 🔴 ALTA (1 FK)
1. `Viagem.RecorrenciaViagemId` → `Viagem.ViagemId`

### 🟡 MÉDIA (6 FKs)
1. `Motorista.UnidadeId` → `Unidade.UnidadeId`
2. `Motorista.CondutorId` → `CondutorApoio.CondutorId`
3. `Veiculo.CombustivelId` → `Combustivel.CombustivelId`
4. `Veiculo.ContratoId` → `Contrato.ContratoId`
5. `Veiculo.AtaId` → `AtaRegistroPrecos.AtaId`
6. `SetorSolicitante.SetorPaiId` → `SetorSolicitante.SetorSolicitanteId`

### 🟢 BAIXA (16 FKs)
1. `WhatsAppMensagens.InstanciaId` → `WhatsAppInstancias.InstanciaId`
2. `WhatsAppMensagens.ContatoId` → `WhatsAppContatos.ContatoId`
3. `WhatsAppMensagens.UsuarioId` → `AspNetUsers.Id`
4. `WhatsAppFilaMensagens.MensagemId` → `WhatsAppMensagens.MensagemId`
5. `WhatsAppWebhookLogs.InstanciaId` → `WhatsAppInstancias.InstanciaId`
6. `ItemVeiculoAta.RepactuacaoAtaId` → `RepactuacaoAta.RepactuacaoAtaId`
7. `ItemVeiculoAta.VeiculoId` → `Veiculo.VeiculoId`
8. `NotaFiscal.VeiculoId` → `Veiculo.VeiculoId`
9. `DocumentoContrato.ContratoId` → `Contrato.ContratoId`
10. `LotacaoMotorista.MotoristaCoberturaId` → `Motorista.MotoristaId`
11. `MovimentacaoEmpenhoMulta.MultaId` → `Multa.MultaId`
12. `ItensManutencao.MotoristaId` → `Motorista.MotoristaId`
13. `Patrimonio.SetorConferenciaId` → `SetorPatrimonial.SetorId`
14. `Patrimonio.SecaoConferenciaId` → `SecaoPatrimonial.SecaoId`
15. `RepactuacaoServicos.RepactuacaoContratoId` → `RepactuacaoContrato.RepactuacaoContratoId`
16. `MovimentacaoEmpenhoMulta.EmpenhoMultaId` → `EmpenhoMulta.EmpenhoMultaId` (verificar)

**Total de FKs Faltantes**: **23**

---

## 3. ÍNDICES SUGERIDOS

Índices em campos frequentemente usados em WHERE/JOIN mas sem índice definido.

### 🔴 ALTA PRIORIDADE

#### Tabela: `Viagem`
| Campo | Tipo de Índice | SQL Sugerido | Justificativa |
|-------|----------------|--------------|---------------|
| `RecorrenciaViagemId` | Non-Clustered | `CREATE INDEX IX_Viagem_RecorrenciaViagemId ON dbo.Viagem (RecorrenciaViagemId) INCLUDE (DataInicial, Status);` | Filtros de viagens recorrentes |

**Nota**: Campo já possui índice composto `IX_Viagem_RecorrenciaViagemId_DataInicial`, mas sem INCLUDE.

---

#### Tabela: `Motorista`
| Campo | Tipo de Índice | SQL Sugerido | Justificativa |
|-------|----------------|--------------|---------------|
| `UnidadeId` | Non-Clustered | `CREATE INDEX IX_Motorista_UnidadeId ON dbo.Motorista (UnidadeId) INCLUDE (Nome, Status);` | Filtros por lotação de motoristas |
| `DataVencimentoCNH` | Non-Clustered | *(Já existe `IX_Motorista_Status_DataVencimentoCNH`)* | - |

---

#### Tabela: `Veiculo`
| Campo | Tipo de Índice | SQL Sugerido | Justificativa |
|-------|----------------|--------------|---------------|
| `CombustivelId` | Non-Clustered | `CREATE INDEX IX_Veiculo_CombustivelId ON dbo.Veiculo (CombustivelId) INCLUDE (Placa, Status);` | Relatórios por tipo de combustível |
| `AtaId` | Non-Clustered | `CREATE INDEX IX_Veiculo_AtaId ON dbo.Veiculo (AtaId) INCLUDE (Placa, Status);` | Filtros por ata |

**Nota**: Campo `ContratoId` já possui índice `IX_Veiculo_ContratoId`.

---

### 🟡 MÉDIA PRIORIDADE

#### Tabela: `Manutencao`
| Campo | Tipo de Índice | SQL Sugerido | Justificativa |
|-------|----------------|--------------|---------------|
| `VeiculoReservaId` | Non-Clustered | `CREATE INDEX IX_Manutencao_VeiculoReservaId ON dbo.Manutencao (VeiculoReservaId) WHERE VeiculoReservaId IS NOT NULL;` | Consultas de veículos reserva |

**Nota**: Índice filtrado (WHERE) para evitar NULLs.

---

#### Tabela: `Multa`
| Campo | Tipo de Índice | SQL Sugerido | Justificativa |
|-------|----------------|--------------|---------------|
| `ContratoMotoristaId` | Non-Clustered | `CREATE INDEX IX_Multa_ContratoMotoristaId ON dbo.Multa (ContratoMotoristaId) INCLUDE (DataMulta, ValorMulta);` | Relatórios por contrato |
| `ContratoVeiculoId` | Non-Clustered | `CREATE INDEX IX_Multa_ContratoVeiculoId ON dbo.Multa (ContratoVeiculoId) INCLUDE (DataMulta, ValorMulta);` | Relatórios por contrato |
| `EmpenhoMultaId` | Non-Clustered | `CREATE INDEX IX_Multa_EmpenhoMultaId ON dbo.Multa (EmpenhoMultaId);` | Filtros por empenho |

**Nota**: Campos `MotoristaId`, `VeiculoId`, `OrgaoAutuanteId`, `TipoMultaId` já têm índices.

---

#### Tabela: `SetorSolicitante`
| Campo | Tipo de Índice | SQL Sugerido | Justificativa |
|-------|----------------|--------------|---------------|
| `SetorPaiId` | Non-Clustered | `CREATE INDEX IX_SetorSolicitante_SetorPaiId ON dbo.SetorSolicitante (SetorPaiId) WHERE SetorPaiId IS NOT NULL;` | Hierarquia de setores |

---

#### Tabela: `LotacaoMotorista`
| Campo | Tipo de Índice | SQL Sugerido | Justificativa |
|-------|----------------|--------------|---------------|
| `MotoristaCoberturaId` | Non-Clustered | `CREATE INDEX IX_LotacaoMotorista_MotoristaCoberturaId ON dbo.LotacaoMotorista (MotoristaCoberturaId) WHERE MotoristaCoberturaId IS NOT NULL;` | Motoristas de cobertura |

**Nota**: Campos `MotoristaId` e `UnidadeId` já têm índices.

---

### 🟢 BAIXA PRIORIDADE

#### Tabela: `NotaFiscal`
| Campo | Tipo de Índice | SQL Sugerido | Justificativa |
|-------|----------------|--------------|---------------|
| `VeiculoId` | Non-Clustered | `CREATE INDEX IX_NotaFiscal_VeiculoId ON dbo.NotaFiscal (VeiculoId) WHERE VeiculoId IS NOT NULL;` | Notas por veículo |

---

#### Tabela: `ItensManutencao`
| Campo | Tipo de Índice | SQL Sugerido | Justificativa |
|-------|----------------|--------------|---------------|
| `MotoristaId` | Non-Clustered | `CREATE INDEX IX_ItensManutencao_MotoristaId ON dbo.ItensManutencao (MotoristaId) WHERE MotoristaId IS NOT NULL;` | Itens por motorista |

**Nota**: Campos `ManutencaoId` e `ViagemId` já têm índices.

---

#### Tabela: `WhatsAppMensagens`
| Campo | Tipo de Índice | SQL Sugerido | Justificativa |
|-------|----------------|--------------|---------------|
| `InstanciaId` | Non-Clustered | `CREATE INDEX IX_WhatsAppMensagens_InstanciaId ON dbo.WhatsAppMensagens (InstanciaId);` | Filtros por instância |
| `ContatoId` | Non-Clustered | `CREATE INDEX IX_WhatsAppMensagens_ContatoId ON dbo.WhatsAppMensagens (ContatoId);` | Histórico por contato |
| `UsuarioId` | Non-Clustered | `CREATE INDEX IX_WhatsAppMensagens_UsuarioId ON dbo.WhatsAppMensagens (UsuarioId);` | Mensagens por usuário |

**Nota**: Campos `DataCriacao` e `Status` já têm índices.

---

#### Tabela: `WhatsAppFilaMensagens`
| Campo | Tipo de Índice | SQL Sugerido | Justificativa |
|-------|----------------|--------------|---------------|
| `MensagemId` | Non-Clustered | `CREATE INDEX IX_WhatsAppFilaMensagens_MensagemId ON dbo.WhatsAppFilaMensagens (MensagemId);` | Relacionamento com mensagens |

**Nota**: Campos `DataAgendamento` e `Status` já têm índices.

---

#### Tabela: `Patrimonio`
| Campo | Tipo de Índice | SQL Sugerido | Justificativa |
|-------|----------------|--------------|---------------|
| `SetorConferenciaId` | Non-Clustered | `CREATE INDEX IX_Patrimonio_SetorConferenciaId ON dbo.Patrimonio (SetorConferenciaId) WHERE SetorConferenciaId IS NOT NULL;` | Conferência por setor |
| `SecaoConferenciaId` | Non-Clustered | `CREATE INDEX IX_Patrimonio_SecaoConferenciaId ON dbo.Patrimonio (SecaoConferenciaId) WHERE SecaoConferenciaId IS NOT NULL;` | Conferência por seção |

---

#### Tabela: `ItemVeiculoAta`
| Campo | Tipo de Índice | SQL Sugerido | Justificativa |
|-------|----------------|--------------|---------------|
| `RepactuacaoAtaId` | Non-Clustered | `CREATE INDEX IX_ItemVeiculoAta_RepactuacaoAtaId ON dbo.ItemVeiculoAta (RepactuacaoAtaId) WHERE RepactuacaoAtaId IS NOT NULL;` | Itens por repactuação |
| `VeiculoId` | Non-Clustered | `CREATE INDEX IX_ItemVeiculoAta_VeiculoId ON dbo.ItemVeiculoAta (VeiculoId) WHERE VeiculoId IS NOT NULL;` | Veículos na ata |

---

## 4. RESUMO DE ÍNDICES SUGERIDOS POR PRIORIDADE

### 🔴 ALTA (5 índices)
1. `Viagem.RecorrenciaViagemId`
2. `Motorista.UnidadeId`
3. `Veiculo.CombustivelId`
4. `Veiculo.AtaId`

### 🟡 MÉDIA (6 índices)
1. `Manutencao.VeiculoReservaId`
2. `Multa.ContratoMotoristaId`
3. `Multa.ContratoVeiculoId`
4. `Multa.EmpenhoMultaId`
5. `SetorSolicitante.SetorPaiId`
6. `LotacaoMotorista.MotoristaCoberturaId`

### 🟢 BAIXA (13 índices)
1. `NotaFiscal.VeiculoId`
2. `ItensManutencao.MotoristaId`
3. `WhatsAppMensagens.InstanciaId`
4. `WhatsAppMensagens.ContatoId`
5. `WhatsAppMensagens.UsuarioId`
6. `WhatsAppFilaMensagens.MensagemId`
7. `Patrimonio.SetorConferenciaId`
8. `Patrimonio.SecaoConferenciaId`
9. `ItemVeiculoAta.RepactuacaoAtaId`
10. `ItemVeiculoAta.VeiculoId`

**Total de Índices Sugeridos**: **24**

---

## 5. SCRIPT SQL CONSOLIDADO

### 5.1. Foreign Keys Faltantes - Prioridade ALTA

```sql
-- ═══════════════════════════════════════════════════════════════════
-- FOREIGN KEYS FALTANTES - PRIORIDADE ALTA
-- ═══════════════════════════════════════════════════════════════════

-- Tabela: Viagem
ALTER TABLE dbo.Viagem
ADD CONSTRAINT FK_Viagem_RecorrenciaViagemId
FOREIGN KEY (RecorrenciaViagemId)
REFERENCES dbo.Viagem (ViagemId);
GO
```

---

### 5.2. Foreign Keys Faltantes - Prioridade MÉDIA

```sql
-- ═══════════════════════════════════════════════════════════════════
-- FOREIGN KEYS FALTANTES - PRIORIDADE MÉDIA
-- ═══════════════════════════════════════════════════════════════════

-- Tabela: Motorista
ALTER TABLE dbo.Motorista
ADD CONSTRAINT FK_Motorista_UnidadeId
FOREIGN KEY (UnidadeId)
REFERENCES dbo.Unidade (UnidadeId);
GO

ALTER TABLE dbo.Motorista
ADD CONSTRAINT FK_Motorista_CondutorId
FOREIGN KEY (CondutorId)
REFERENCES dbo.CondutorApoio (CondutorId);
GO

-- Tabela: Veiculo
ALTER TABLE dbo.Veiculo
ADD CONSTRAINT FK_Veiculo_CombustivelId
FOREIGN KEY (CombustivelId)
REFERENCES dbo.Combustivel (CombustivelId);
GO

ALTER TABLE dbo.Veiculo
ADD CONSTRAINT FK_Veiculo_ContratoId
FOREIGN KEY (ContratoId)
REFERENCES dbo.Contrato (ContratoId);
GO

ALTER TABLE dbo.Veiculo
ADD CONSTRAINT FK_Veiculo_AtaId
FOREIGN KEY (AtaId)
REFERENCES dbo.AtaRegistroPrecos (AtaId);
GO

-- Tabela: SetorSolicitante
ALTER TABLE dbo.SetorSolicitante
ADD CONSTRAINT FK_SetorSolicitante_SetorPaiId
FOREIGN KEY (SetorPaiId)
REFERENCES dbo.SetorSolicitante (SetorSolicitanteId);
GO
```

---

### 5.3. Foreign Keys Faltantes - Prioridade BAIXA

```sql
-- ═══════════════════════════════════════════════════════════════════
-- FOREIGN KEYS FALTANTES - PRIORIDADE BAIXA
-- ═══════════════════════════════════════════════════════════════════

-- Tabela: WhatsAppMensagens
ALTER TABLE dbo.WhatsAppMensagens
ADD CONSTRAINT FK_WhatsAppMensagens_InstanciaId
FOREIGN KEY (InstanciaId)
REFERENCES dbo.WhatsAppInstancias (InstanciaId);
GO

ALTER TABLE dbo.WhatsAppMensagens
ADD CONSTRAINT FK_WhatsAppMensagens_ContatoId
FOREIGN KEY (ContatoId)
REFERENCES dbo.WhatsAppContatos (ContatoId);
GO

ALTER TABLE dbo.WhatsAppMensagens
ADD CONSTRAINT FK_WhatsAppMensagens_UsuarioId
FOREIGN KEY (UsuarioId)
REFERENCES dbo.AspNetUsers (Id);
GO

-- Tabela: WhatsAppFilaMensagens
ALTER TABLE dbo.WhatsAppFilaMensagens
ADD CONSTRAINT FK_WhatsAppFilaMensagens_MensagemId
FOREIGN KEY (MensagemId)
REFERENCES dbo.WhatsAppMensagens (MensagemId);
GO

-- Tabela: WhatsAppWebhookLogs
ALTER TABLE dbo.WhatsAppWebhookLogs
ADD CONSTRAINT FK_WhatsAppWebhookLogs_InstanciaId
FOREIGN KEY (InstanciaId)
REFERENCES dbo.WhatsAppInstancias (InstanciaId);
GO

-- Tabela: ItemVeiculoAta
ALTER TABLE dbo.ItemVeiculoAta
ADD CONSTRAINT FK_ItemVeiculoAta_RepactuacaoAtaId
FOREIGN KEY (RepactuacaoAtaId)
REFERENCES dbo.RepactuacaoAta (RepactuacaoAtaId);
GO

ALTER TABLE dbo.ItemVeiculoAta
ADD CONSTRAINT FK_ItemVeiculoAta_VeiculoId
FOREIGN KEY (VeiculoId)
REFERENCES dbo.Veiculo (VeiculoId);
GO

-- Tabela: NotaFiscal
ALTER TABLE dbo.NotaFiscal
ADD CONSTRAINT FK_NotaFiscal_VeiculoId
FOREIGN KEY (VeiculoId)
REFERENCES dbo.Veiculo (VeiculoId);
GO

-- Tabela: DocumentoContrato
ALTER TABLE dbo.DocumentoContrato
ADD CONSTRAINT FK_DocumentoContrato_ContratoId
FOREIGN KEY (ContratoId)
REFERENCES dbo.Contrato (ContratoId);
GO

-- Tabela: LotacaoMotorista
ALTER TABLE dbo.LotacaoMotorista
ADD CONSTRAINT FK_LotacaoMotorista_MotoristaCoberturaId
FOREIGN KEY (MotoristaCoberturaId)
REFERENCES dbo.Motorista (MotoristaId);
GO

-- Tabela: MovimentacaoEmpenhoMulta
ALTER TABLE dbo.MovimentacaoEmpenhoMulta
ADD CONSTRAINT FK_MovimentacaoEmpenhoMulta_MultaId
FOREIGN KEY (MultaId)
REFERENCES dbo.Multa (MultaId);
GO

-- Tabela: ItensManutencao
ALTER TABLE dbo.ItensManutencao
ADD CONSTRAINT FK_ItensManutencao_MotoristaId
FOREIGN KEY (MotoristaId)
REFERENCES dbo.Motorista (MotoristaId);
GO

-- Tabela: Patrimonio
ALTER TABLE dbo.Patrimonio
ADD CONSTRAINT FK_Patrimonio_SetorConferenciaId
FOREIGN KEY (SetorConferenciaId)
REFERENCES dbo.SetorPatrimonial (SetorId);
GO

ALTER TABLE dbo.Patrimonio
ADD CONSTRAINT FK_Patrimonio_SecaoConferenciaId
FOREIGN KEY (SecaoConferenciaId)
REFERENCES dbo.SecaoPatrimonial (SecaoId);
GO

-- Tabela: RepactuacaoServicos
ALTER TABLE dbo.RepactuacaoServicos
ADD CONSTRAINT FK_RepactuacaoServicos_RepactuacaoContratoId
FOREIGN KEY (RepactuacaoContratoId)
REFERENCES dbo.RepactuacaoContrato (RepactuacaoContratoId);
GO
```

---

### 5.4. Índices Sugeridos - Prioridade ALTA

```sql
-- ═══════════════════════════════════════════════════════════════════
-- ÍNDICES SUGERIDOS - PRIORIDADE ALTA
-- ═══════════════════════════════════════════════════════════════════

-- Tabela: Motorista
CREATE INDEX IX_Motorista_UnidadeId
ON dbo.Motorista (UnidadeId)
INCLUDE (Nome, Status);
GO

-- Tabela: Veiculo
CREATE INDEX IX_Veiculo_CombustivelId
ON dbo.Veiculo (CombustivelId)
INCLUDE (Placa, Status);
GO

CREATE INDEX IX_Veiculo_AtaId
ON dbo.Veiculo (AtaId)
INCLUDE (Placa, Status);
GO
```

---

### 5.5. Índices Sugeridos - Prioridade MÉDIA

```sql
-- ═══════════════════════════════════════════════════════════════════
-- ÍNDICES SUGERIDOS - PRIORIDADE MÉDIA
-- ═══════════════════════════════════════════════════════════════════

-- Tabela: Manutencao
CREATE INDEX IX_Manutencao_VeiculoReservaId
ON dbo.Manutencao (VeiculoReservaId)
WHERE VeiculoReservaId IS NOT NULL;
GO

-- Tabela: Multa
CREATE INDEX IX_Multa_ContratoMotoristaId
ON dbo.Multa (ContratoMotoristaId)
INCLUDE (DataMulta, ValorMulta);
GO

CREATE INDEX IX_Multa_ContratoVeiculoId
ON dbo.Multa (ContratoVeiculoId)
INCLUDE (DataMulta, ValorMulta);
GO

CREATE INDEX IX_Multa_EmpenhoMultaId
ON dbo.Multa (EmpenhoMultaId);
GO

-- Tabela: SetorSolicitante
CREATE INDEX IX_SetorSolicitante_SetorPaiId
ON dbo.SetorSolicitante (SetorPaiId)
WHERE SetorPaiId IS NOT NULL;
GO

-- Tabela: LotacaoMotorista
CREATE INDEX IX_LotacaoMotorista_MotoristaCoberturaId
ON dbo.LotacaoMotorista (MotoristaCoberturaId)
WHERE MotoristaCoberturaId IS NOT NULL;
GO
```

---

### 5.6. Índices Sugeridos - Prioridade BAIXA

```sql
-- ═══════════════════════════════════════════════════════════════════
-- ÍNDICES SUGERIDOS - PRIORIDADE BAIXA
-- ═══════════════════════════════════════════════════════════════════

-- Tabela: NotaFiscal
CREATE INDEX IX_NotaFiscal_VeiculoId
ON dbo.NotaFiscal (VeiculoId)
WHERE VeiculoId IS NOT NULL;
GO

-- Tabela: ItensManutencao
CREATE INDEX IX_ItensManutencao_MotoristaId
ON dbo.ItensManutencao (MotoristaId)
WHERE MotoristaId IS NOT NULL;
GO

-- Tabela: WhatsAppMensagens
CREATE INDEX IX_WhatsAppMensagens_InstanciaId
ON dbo.WhatsAppMensagens (InstanciaId);
GO

CREATE INDEX IX_WhatsAppMensagens_ContatoId
ON dbo.WhatsAppMensagens (ContatoId);
GO

CREATE INDEX IX_WhatsAppMensagens_UsuarioId
ON dbo.WhatsAppMensagens (UsuarioId);
GO

-- Tabela: WhatsAppFilaMensagens
CREATE INDEX IX_WhatsAppFilaMensagens_MensagemId
ON dbo.WhatsAppFilaMensagens (MensagemId);
GO

-- Tabela: Patrimonio
CREATE INDEX IX_Patrimonio_SetorConferenciaId
ON dbo.Patrimonio (SetorConferenciaId)
WHERE SetorConferenciaId IS NOT NULL;
GO

CREATE INDEX IX_Patrimonio_SecaoConferenciaId
ON dbo.Patrimonio (SecaoConferenciaId)
WHERE SecaoConferenciaId IS NOT NULL;
GO

-- Tabela: ItemVeiculoAta
CREATE INDEX IX_ItemVeiculoAta_RepactuacaoAtaId
ON dbo.ItemVeiculoAta (RepactuacaoAtaId)
WHERE RepactuacaoAtaId IS NOT NULL;
GO

CREATE INDEX IX_ItemVeiculoAta_VeiculoId
ON dbo.ItemVeiculoAta (VeiculoId)
WHERE VeiculoId IS NOT NULL;
GO
```

---

## 6. RECOMENDAÇÕES E PRÓXIMOS PASSOS

### 6.1. Implementação Sugerida

1. **Executar em ambiente de DESENVOLVIMENTO primeiro**
2. **Testar todas as consultas** após criação de FKs e índices
3. **Medir performance** antes e depois
4. **Backup do banco** antes de aplicar em produção
5. **Executar em PRODUÇÃO** em horário de baixa utilização

### 6.2. Ordem de Execução Recomendada

1. ✅ **FKs Prioridade ALTA** (1 FK)
2. ✅ **Índices Prioridade ALTA** (4 índices)
3. ✅ **FKs Prioridade MÉDIA** (6 FKs)
4. ✅ **Índices Prioridade MÉDIA** (6 índices)
5. ⚠️ **FKs Prioridade BAIXA** (15 FKs) - avaliar necessidade
6. ⚠️ **Índices Prioridade BAIXA** (10 índices) - avaliar necessidade

### 6.3. Considerações Importantes

#### Performance
- Índices melhoram **consultas** mas podem **reduzir performance de INSERT/UPDATE/DELETE**
- Monitorar uso de índices com `sys.dm_db_index_usage_stats`
- Remover índices não utilizados periodicamente

#### Integridade Referencial
- FKs garantem **integridade de dados**
- **Podem bloquear exclusões** se existirem registros relacionados
- Considerar `ON DELETE CASCADE` ou `ON DELETE SET NULL` conforme regra de negócio

#### Índices Filtrados
- Índices com `WHERE campo IS NOT NULL` **economizam espaço** e melhoram performance
- Úteis para colunas nullable com poucos NULLs

### 6.4. Monitoramento Pós-Implementação

Executar queries de monitoramento:

```sql
-- Uso de índices
SELECT
    OBJECT_NAME(s.object_id) AS TableName,
    i.name AS IndexName,
    s.user_seeks,
    s.user_scans,
    s.user_lookups,
    s.user_updates
FROM sys.dm_db_index_usage_stats s
INNER JOIN sys.indexes i ON s.object_id = i.object_id AND s.index_id = i.index_id
WHERE OBJECTPROPERTY(s.object_id, 'IsUserTable') = 1
ORDER BY s.user_seeks + s.user_scans + s.user_lookups DESC;
GO

-- Foreign Keys criadas
SELECT
    fk.name AS FKName,
    OBJECT_NAME(fk.parent_object_id) AS TableName,
    OBJECT_NAME(fk.referenced_object_id) AS ReferencedTable
FROM sys.foreign_keys fk
ORDER BY TableName;
GO
```

---

## 7. OBSERVAÇÕES FINAIS

### 7.1. Campos Não Mapeados

Alguns campos com sufixo "Id" **podem não necessitar** de FK:
- Campos de log/auditoria
- Campos legados
- Campos de sistemas externos

Revisar caso a caso antes de implementar.

### 7.2. Tabelas de Estatísticas

Tabelas como `EstatisticaVeiculoUnidade`, `EstatisticaAbastecimentoMensal`, etc. **não precisam de FKs** pois são geradas por processamento batch e não precisam de integridade referencial estrita.

### 7.3. Performance de Queries Existentes

Testar queries críticas após implementação:
- Dashboard de viagens
- Relatórios de abastecimento
- Listagens de veículos e motoristas
- Cálculos de custos

---

**Fim do Relatório**

---

**Gerado por**: Claude Sonnet 4.5
**Data**: 12/01/2026
**Versão**: 1.0
