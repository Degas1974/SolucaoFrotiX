# AUDITORIA BANCO REAL VS MODELOS C#
**Data:** 13/02/2026
**Banco de Dados:** SQL Server - Frotix
**Total de Tabelas no Banco:** 84 (85 incluindo sysdiagrams)
**Total de Modelos C# Encontrados:** 125

---

## RESUMO EXECUTIVO

| Métrica | Valor | Status |
|---------|-------|--------|
| **Total de Discrepâncias** | 243 | ❌ CRÍTICO |
| **Problemas CRÍTICOS** | 6 | 🔴 URGENTE |
| **Problemas ALTOS** | 54 | 🟡 IMPORTANTE |
| **Problemas MÉDIOS** | 163 | 🟢 REVISÃO |
| **Problemas BAIXOS** | 20 | ⚪ INFORMATIVO |
| **Tabelas Órfãs (sem modelo)** | 20 | ⚠️ |
| **Taxa de Conformidade** | 0% | ❌ |

---

## ANÁLISE DE SEVERIDADE

### 🔴 CRÍTICO (6 problemas)
Tipos incompatíveis entre banco e modelo - **PODEM CAUSAR ERROS EM RUNTIME**

| Tabela | Coluna | Tipo Banco | Tipo Modelo | Impacto |
|--------|--------|------------|-------------|---------|
| **AlertasFrotiX** | TipoAlerta | `int` | `TipoAlerta` (enum) | Conversão pode falhar |
| **AlertasFrotiX** | Prioridade | `int` | `PrioridadeAlerta` (enum) | Conversão pode falhar |
| **AlertasFrotiX** | TipoExibicao | `int` | `TipoExibicaoAlerta` (enum) | Conversão pode falhar |
| **CorridasTaxiLeg** | QRU | `int?` | `string?` | Conversão falha |
| **Viagem** | DataFinalizacao | `DateTime?` | `string?` | Conversão falha |
| **Viagem** | DatasSelecionadas | `string` | `List<DateTime>?` | Deserialização manual |

**AÇÃO IMEDIATA REQUERIDA:** Corrigir ANTES de produção!

---

### 🟡 ALTO (54 problemas)
Colunas existem no banco mas **NÃO estão mapeadas** no modelo C#

#### Top 10 Tabelas com Mais Colunas Não Mapeadas:

| Tabela | Colunas Não Mapeadas | Mais Crítica |
|--------|----------------------|--------------|
| **VeiculoPadraoViagem** | 22 colunas | `VeiculoPadraoViagemId` (PK) |
| **Viagem** | 11 colunas | `ResumoOcorrencia`, `Id` |
| **Abastecimento** | 5 colunas | `KmRodadoNormalizado` |
| **ViagemEstatistica** | 4 colunas | `KmTotal`, `MinutosTotal` |
| **AlertasFrotiX** | 3 colunas | `Recorrente`, `Intervalo` |
| **AtaRegistroPrecos** | 2 colunas | `UsuarioIdAlteracao` |
| **Contrato** | 2 colunas | `UsuarioIdAlteracao` |
| **Lavagem** | 1 coluna | `Horario` |
| **Motorista** | 1 coluna | `CondutorId` |
| **CorridasTaxiLeg** | 1 coluna | `Valor` |

**IMPACTO:** Dados dessas colunas **NUNCA serão carregados** pelo EF Core.

---

### 🟢 MÉDIO (163 problemas)
Propriedades existem no modelo mas **NÃO existem no banco**

**Tipos Comuns:**
1. **Propriedades de Navegação EF Core** (automapróprias): `IEnumerable<SelectListItem>` (56 ocorrências)
2. **Propriedades Auxiliares/UI**: `NomeUsuarioAlteracao`, `ArquivoFoto` (28 ocorrências)
3. **Propriedades Calculadas**: Sem `[NotMapped]` (79 ocorrências)

**AÇÃO:** Adicionar `[NotMapped]` em todas as propriedades que não correspondem a colunas.

---

### ⚪ BAIXO (20 tabelas)
Tabelas do banco **SEM modelo C# correspondente**

| Tabela | Tipo | Necessita Modelo? |
|--------|------|-------------------|
| AlertasUsuario | Relacional (muitos-para-muitos) | ✅ SIM |
| CondutorApoio | Lookup | ✅ SIM |
| Contatos | Entidade | ✅ SIM |
| CorridasCanceladasTaxiLeg | Histórico | ⚠️ TALVEZ |
| CustoMensalItensContrato | Agregação | ⚠️ TALVEZ |
| DocumentoContrato | Entidade | ✅ SIM |
| ItemVeiculoAta | Relacional | ✅ SIM |
| ItemVeiculoContrato | Relacional | ✅ SIM |
| MediaCombustivel | Agregação | ⚠️ TALVEZ |
| RepactuacaoAta | Entidade | ✅ SIM |
| RepactuacaoContrato | Entidade | ✅ SIM |
| RepactuacaoServicos | Entidade | ✅ SIM |
| RepactuacaoTerceirizacao | Entidade | ✅ SIM |
| Viagem_Backup_OrigemDestino | Backup | ❌ NÃO |
| WhatsAppContatos | WhatsApp | ✅ SIM |
| WhatsAppFilaMensagens | WhatsApp | ✅ SIM |
| WhatsAppInstancias | WhatsApp | ✅ SIM |
| WhatsAppMensagens | WhatsApp | ✅ SIM |
| WhatsAppWebhookLogs | WhatsApp | ✅ SIM |
| sysdiagrams | Sistema | ❌ NÃO |

---

## DETALHAMENTO POR TABELA CRÍTICA

### 1. Abastecimento (5 colunas não mapeadas)

**Colunas faltantes:**
```csharp
public int? KmRodadoNormalizado { get; set; }
public double? LitrosNormalizado { get; set; }
public decimal? ConsumoCalculado { get; set; }
public decimal? ConsumoNormalizado { get; set; }
public bool? EhOutlier { get; set; }
```

**Impacto:** Sistema de normalização de abastecimentos **NÃO funciona**.

---

### 2. AlertasFrotiX (3 tipos incompatíveis + 3 colunas não mapeadas)

**CRÍTICO - Tipos Incompatíveis:**
```csharp
// ERRADO (atual)
public TipoAlerta TipoAlerta { get; set; }          // Banco: int
public PrioridadeAlerta Prioridade { get; set; }    // Banco: int
public TipoExibicaoAlerta TipoExibicao { get; set; } // Banco: int

// CORRETO (deve ser)
public int TipoAlerta { get; set; }
public int Prioridade { get; set; }
public int TipoExibicao { get; set; }
```

**Colunas faltantes:**
```csharp
public string Recorrente { get; set; }           // char(1) - 'S'/'N'
public string Intervalo { get; set; }            // char(1) - 'D'/'S'/'M'
public DateTime? DataFinalRecorrencia { get; set; }
```

**Impacto:** Alertas recorrentes **NÃO funcionam**.

---

### 3. VeiculoPadraoViagem (22 colunas não mapeadas)

**TABELA COMPLETAMENTE NÃO MAPEADA!**

**Todas as colunas estatísticas estão faltando:**
```csharp
public int VeiculoPadraoViagemId { get; set; }  // PK!!!
public decimal? AvgDuracaoMinutos { get; set; }
public decimal? DesvioPadraoDuracaoMinutos { get; set; }
public int? MinDuracaoMinutos { get; set; }
public int? MaxDuracaoNormalMinutos { get; set; }
public decimal? MedianaDuracaoMinutos { get; set; }
public decimal? AvgKmPorViagem { get; set; }
public decimal? DesvioPadraoKm { get; set; }
public decimal? MaxKmNormalPorViagem { get; set; }
public decimal? MedianaKm { get; set; }
public decimal? Q1Km { get; set; }
public decimal? Q3Km { get; set; }
public decimal? IQRKm { get; set; }
public decimal? LimiteInferiorKm { get; set; }
public decimal? LimiteSuperiorKm { get; set; }
public decimal? MedianaMinutos { get; set; }
public decimal? AvgKmPorDia { get; set; }
public int? TotalViagensAnalisadas { get; set; }
public int? TotalViagensRealizadas { get; set; }
public int? Percentil95Duracao { get; set; }
public int? Percentil99Duracao { get; set; }
public DateTime? DataCriacao { get; set; }
```

**Impacto:** Sistema de estatísticas de veículos **NÃO funciona**.

---

### 4. Viagem (2 tipos incompatíveis + 11 colunas não mapeadas)

**CRÍTICO - Tipos Incompatíveis:**
```csharp
// ERRADO
public string? DataFinalizacao { get; set; }     // Banco: DateTime?
public List<DateTime>? DatasSelecionadas { get; set; } // Banco: string (JSON)

// CORRETO
public DateTime? DataFinalizacao { get; set; }
public string DatasSelecionadas { get; set; }    // JSON serializado
```

**Colunas faltantes:**
```csharp
public string ResumoOcorrencia { get; set; }
public string DescricaoOcorrencia { get; set; }
public string StatusOcorrencia { get; set; }
public string DescricaoSolucaoOcorrencia { get; set; }
public Guid? ItemManutencaoId { get; set; }
public string AgendamentoTMP { get; set; }
public string DanoAvaria { get; set; }
public string DanoAvariaFinal { get; set; }
public byte[] FotosBase64 { get; set; }
public byte[] VideosBase64 { get; set; }
public byte[] FotosFinaisBase64 { get; set; }
public byte[] VideosFinaisBase64 { get; set; }
public int Id { get; set; }  // Coluna ID adicional!
```

**Impacto:**
- Ocorrências de viagem **NÃO funcionam**
- Fotos/vídeos **NÃO são salvos**
- Agendamento temporário **NÃO funciona**

---

### 5. ViagemEstatistica (4 colunas não mapeadas)

```csharp
public int? KmTotal { get; set; }
public decimal? KmMedio { get; set; }
public int? MinutosTotal { get; set; }
public int? MinutosMedio { get; set; }
```

**Impacto:** Estatísticas de Km e tempo **NÃO funcionam**.

---

### 6. CorridasTaxiLeg (1 tipo incompatível + 1 coluna não mapeada)

**CRÍTICO:**
```csharp
// ERRADO
public string? QRU { get; set; }  // Banco: int?

// CORRETO
public int? QRU { get; set; }
```

**Coluna faltante:**
```csharp
public double? Valor { get; set; }
```

**Impacto:** Valor da corrida **NUNCA é salvo/carregado**.

---

## RECOMENDAÇÕES URGENTES

### ⚡ PRIORIDADE 1 - CRÍTICO (HOJE)
1. Corrigir tipos incompatíveis em:
   - `AlertasFrotiX` (3 colunas enum → int)
   - `CorridasTaxiLeg.QRU` (string → int)
   - `Viagem.DataFinalizacao` (string → DateTime?)
   - `Viagem.DatasSelecionadas` (List<DateTime> → string)

### ⚡ PRIORIDADE 2 - ALTO (ESTA SEMANA)
2. Mapear **VeiculoPadraoViagem** completamente (22 colunas)
3. Mapear colunas de **Viagem** (11 colunas faltantes)
4. Mapear colunas normalizadas de **Abastecimento** (5 colunas)
5. Mapear colunas recorrentes de **AlertasFrotiX** (3 colunas)
6. Mapear colunas de **ViagemEstatistica** (4 colunas)

### ⚡ PRIORIDADE 3 - MÉDIO (PRÓXIMAS 2 SEMANAS)
7. Adicionar `[NotMapped]` em todas propriedades auxiliares (163 ocorrências)
8. Criar modelos para tabelas órfãs importantes:
   - AlertasUsuario
   - WhatsApp* (5 tabelas)
   - Repactuacao* (4 tabelas)
   - ItemVeiculo* (2 tabelas)

---

## CHECKLIST DE VALIDAÇÃO

Após correções, executar:

```bash
# 1. Re-executar análise
powershell -ExecutionPolicy Bypass -File "Scripts\Analisa-Schema.ps1"

# 2. Verificar se CRÍTICO = 0
# 3. Verificar se ALTO < 10
# 4. Compilar projeto
dotnet build

# 5. Executar testes
dotnet test

# 6. Verificar migrations
dotnet ef migrations add VerificacaoSincronizacao
```

---

## ARQUIVOS RELACIONADOS

- **CORRECOES_MODELOS_CSHARP_BANCO_REAL.md** - Código C# pronto para copiar/colar
- **RELATORIO_SINCRONIZACAO_BANCO_REAL.md** - Resumo executivo
- **analise_discrepancias.csv** - Dados brutos (243 linhas)

---

**Gerado automaticamente em:** 13/02/2026 via conexão direta ao SQL Server
**Fonte da Verdade:** Banco SQL Server `localhost\Frotix`
**Script:** `Analisa-Schema.ps1` v2.0
