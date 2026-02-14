# RELATÓRIO EXECUTIVO - SINCRONIZAÇÃO BANCO vs MODELOS
**Data:** 13/02/2026 14:30
**Projeto:** FrotiX - Sistema de Gestão de Frota
**Banco de Dados:** SQL Server `localhost\Frotix`

---

## SUMÁRIO EXECUTIVO

Este relatório documenta a análise comparativa **completa** entre o **banco de dados SQL Server REAL** e os **modelos C# Entity Framework** do projeto FrotiX.

### Principais Descobertas

| Métrica | Valor | Status |
|---------|:-----:|:------:|
| Tabelas no Banco | 84 | ✅ |
| Modelos C# Detectados | 125 | ✅ |
| **Discrepâncias Totais** | **243** | ❌ |
| Problemas CRÍTICOS | 6 | 🔴 |
| Problemas ALTOS | 54 | 🟡 |
| Problemas MÉDIOS | 163 | 🟢 |
| Tabelas Sem Modelo | 20 | ⚠️ |

---

## IMPACTO NO SISTEMA

### 🔴 CRÍTICO - FUNCIONALIDADES QUEBRADAS

| Funcionalidade | Tabela | Problema | Impacto |
|----------------|--------|----------|---------|
| **Alertas do Sistema** | AlertasFrotiX | Tipos incompatíveis (enum vs int) | Erros ao salvar/carregar alertas |
| **Corridas TaxiLeg** | CorridasTaxiLeg | QRU string vs int | Erro ao buscar corridas |
| **Finalização de Viagens** | Viagem | DataFinalizacao string vs DateTime | Erro ao finalizar viagens |
| **Datas Selecionadas** | Viagem | List vs string JSON | Recorrência não funciona |

**RISCO:** Sistema pode apresentar erros em produção a qualquer momento.

---

### 🟡 ALTO - DADOS NÃO PERSISTIDOS

| Funcionalidade | Tabela | Colunas Perdidas | Impacto |
|----------------|--------|------------------|---------|
| **Estatísticas de Veículos** | VeiculoPadraoViagem | **22 colunas** | Sistema de análise **COMPLETAMENTE NÃO FUNCIONA** |
| **Normalização de Abastecimentos** | Abastecimento | 5 colunas | Outliers não detectados |
| **Ocorrências de Viagem** | Viagem | 11 colunas | Ocorrências/fotos/vídeos **NÃO são salvos** |
| **Estatísticas de Viagens** | ViagemEstatistica | 4 colunas | KM e tempo não computados |
| **Alertas Recorrentes** | AlertasFrotiX | 3 colunas | Recorrência não funciona |
| **Corridas TaxiLeg** | CorridasTaxiLeg | Coluna `Valor` | Valor da corrida **NUNCA é salvo** |

**RISCO:** Perda de dados críticos para análises e relatórios.

---

### 🟢 MÉDIO - PROPRIEDADES NÃO MAPEADAS

**163 propriedades** no código C# que não existem no banco:
- 56 propriedades `IEnumerable<SelectListItem>` (dropdowns)
- 28 propriedades auxiliares (NomeUsuario, ArquivoFoto)
- 79 propriedades calculadas/UI

**RISCO:** Possíveis erros ao tentar salvar dados. Necessário adicionar `[NotMapped]`.

---

## TOP 5 TABELAS CRÍTICAS

### 1️⃣ VeiculoPadraoViagem
**Severidade:** 🔴 CRÍTICO
**Problema:** Modelo está **COMPLETAMENTE DESALINHADO** com o banco
**Colunas Faltantes:** 22 de 24 colunas (91% do modelo)
**Impacto:** Sistema de estatísticas de veículos **NÃO FUNCIONA**

**Ação:** Recriar modelo do zero (código disponível em `CORRECOES_MODELOS_CSHARP_BANCO_REAL.md`)

---

### 2️⃣ Viagem
**Severidade:** 🔴 CRÍTICO + 🟡 ALTO
**Problemas:**
- 2 tipos incompatíveis (DataFinalizacao, DatasSelecionadas)
- 11 colunas não mapeadas (ocorrências, fotos, vídeos, agendamento)

**Impacto:**
- Erros ao finalizar viagens
- Ocorrências não são registradas
- Fotos/vídeos não são salvos
- Agendamento temporário não funciona

**Ação:** Corrigir tipos + adicionar 11 colunas

---

### 3️⃣ AlertasFrotiX
**Severidade:** 🔴 CRÍTICO + 🟡 ALTO
**Problemas:**
- 3 tipos incompatíveis (enum vs int)
- 3 colunas não mapeadas (recorrência)

**Impacto:**
- Erros ao criar/editar alertas
- Alertas recorrentes não funcionam

**Ação:** Converter enums para int + adicionar colunas de recorrência

---

### 4️⃣ Abastecimento
**Severidade:** 🟡 ALTO
**Problema:** 5 colunas de normalização não mapeadas
**Impacto:** Detecção de outliers e normalização de dados não funcionam

**Ação:** Adicionar 5 colunas ao modelo

---

### 5️⃣ ViagemEstatistica
**Severidade:** 🟡 ALTO
**Problema:** 4 colunas de KM/tempo não mapeadas
**Impacto:** Estatísticas de quilometragem e tempo não são calculadas

**Ação:** Adicionar 4 colunas ao modelo

---

## DISTRIBUIÇÃO DE PROBLEMAS

```
CRÍTICO (6)     ████████
ALTO (54)       ████████████████████████████████████████████████████
MÉDIO (163)     ███████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
BAIXO (20)      ████████████████████
```

---

## TABELAS SEM MODELO C#

**20 tabelas no banco** não possuem modelo C# correspondente:

### Críticas (Criar Modelo):
- ✅ **AlertasUsuario** - Relação muitos-para-muitos alertas/usuários
- ✅ **WhatsAppContatos** - Sistema WhatsApp
- ✅ **WhatsAppInstancias** - Sistema WhatsApp
- ✅ **WhatsAppMensagens** - Sistema WhatsApp
- ✅ **WhatsAppFilaMensagens** - Sistema WhatsApp
- ✅ **WhatsAppWebhookLogs** - Sistema WhatsApp (total: 5 tabelas)
- ✅ **RepactuacaoAta** - Gestão de contratos
- ✅ **RepactuacaoContrato** - Gestão de contratos
- ✅ **RepactuacaoServicos** - Gestão de contratos
- ✅ **RepactuacaoTerceirizacao** - Gestão de contratos (total: 4 tabelas)
- ✅ **ItemVeiculoAta** - Relacionamento
- ✅ **ItemVeiculoContrato** - Relacionamento
- ✅ **DocumentoContrato** - Documentos
- ✅ **CondutorApoio** - Cadastro
- ✅ **Contatos** - Cadastro

### Opcionais (Avaliar Necessidade):
- ⚠️ **CorridasCanceladasTaxiLeg** - Histórico
- ⚠️ **CustoMensalItensContrato** - Agregação
- ⚠️ **MediaCombustivel** - Agregação

### Ignorar:
- ❌ **Viagem_Backup_OrigemDestino** - Backup temporário
- ❌ **sysdiagrams** - Sistema

---

## PLANO DE AÇÃO

### FASE 1 - CRÍTICO (1-2 DIAS) 🔴

**Objetivo:** Eliminar risco de erro em produção

✅ **Tarefa 1.1:** Corrigir tipos incompatíveis
- [ ] AlertasFrotiX: 3 enums → int
- [ ] CorridasTaxiLeg: QRU string → int
- [ ] Viagem: DataFinalizacao string → DateTime
- [ ] Viagem: DatasSelecionadas List → string

**Estimativa:** 2-4 horas
**Risco:** Baixo (mudanças pontuais)

---

### FASE 2 - ALTO (3-5 DIAS) 🟡

**Objetivo:** Restaurar funcionalidades críticas

✅ **Tarefa 2.1:** Reconstruir VeiculoPadraoViagem
- [ ] Recriar modelo com 22 colunas
- [ ] Testar cálculos de estatísticas

**Estimativa:** 4-6 horas
**Risco:** Médio (modelo novo)

✅ **Tarefa 2.2:** Atualizar Viagem
- [ ] Adicionar 11 colunas
- [ ] Testar salvamento de ocorrências
- [ ] Testar upload de fotos/vídeos

**Estimativa:** 6-8 horas
**Risco:** Médio (tabela core)

✅ **Tarefa 2.3:** Atualizar Abastecimento, AlertasFrotiX, ViagemEstatistica
- [ ] Adicionar 5 + 3 + 4 = 12 colunas
- [ ] Testar normalização
- [ ] Testar recorrência de alertas

**Estimativa:** 4-6 horas
**Risco:** Baixo

✅ **Tarefa 2.4:** Correções menores
- [ ] CorridasTaxiLeg.Valor
- [ ] Lavagem.Horario
- [ ] Motorista.CondutorId
- [ ] AtaRegistroPrecos auditoria
- [ ] Contrato auditoria

**Estimativa:** 2-3 horas
**Risco:** Baixo

---

### FASE 3 - MÉDIO (1-2 SEMANAS) 🟢

**Objetivo:** Organizar código e prevenir erros futuros

✅ **Tarefa 3.1:** Adicionar [NotMapped] em 163 propriedades
- [ ] 56 propriedades `*List`
- [ ] 28 propriedades auxiliares
- [ ] 79 propriedades calculadas/UI

**Estimativa:** 8-12 horas (tedioso mas simples)
**Risco:** Baixo

---

### FASE 4 - BAIXO (2-3 SEMANAS) ⚪

**Objetivo:** Completar cobertura do banco

✅ **Tarefa 4.1:** Criar modelos para tabelas órfãs
- [ ] AlertasUsuario
- [ ] WhatsApp* (5 modelos)
- [ ] Repactuacao* (4 modelos)
- [ ] ItemVeiculo* (2 modelos)
- [ ] Outros (4 modelos)

**Estimativa:** 12-16 horas
**Risco:** Baixo (tabelas novas)

---

## CRONOGRAMA RESUMIDO

| Fase | Duração | Quando | Prioridade |
|------|---------|--------|------------|
| **FASE 1** | 1-2 dias | **IMEDIATO** | 🔴 CRÍTICO |
| **FASE 2** | 3-5 dias | Esta semana | 🟡 ALTO |
| **FASE 3** | 1-2 semanas | Próximas 2 semanas | 🟢 MÉDIO |
| **FASE 4** | 2-3 semanas | Próximo mês | ⚪ BAIXO |

**Total:** 4-6 semanas para sincronização completa

---

## RECOMENDAÇÕES TÉCNICAS

### ✅ Fazer
1. Aplicar correções CRÍTICAS **IMEDIATAMENTE**
2. Criar branch específica para sincronização: `sync-banco-real-2026`
3. Executar testes após cada fase
4. Gerar migrations mas **NÃO aplicar** (apenas validar)
5. Documentar todas as alterações
6. Re-executar análise após cada fase

### ❌ NÃO Fazer
1. **NÃO alterar o banco de dados** - ele é a fonte da verdade
2. **NÃO aplicar todas correções de uma vez** - risco muito alto
3. **NÃO pular testes** - cada fase deve ser testada
4. **NÃO remover propriedades sem antes marcar [NotMapped]**

---

## VALIDAÇÃO E TESTES

Após cada fase, executar:

```bash
# 1. Compilação
dotnet build
# ✅ Deve compilar sem erros

# 2. Re-executar análise
powershell -ExecutionPolicy Bypass -File "Scripts\Analisa-Schema.ps1"
# ✅ Verificar redução de discrepâncias

# 3. Gerar migration de validação
dotnet ef migrations add ValidacaoFaseX
# ✅ Revisar SQL gerado (NÃO aplicar)

# 4. Testes unitários
dotnet test
# ✅ Todos os testes devem passar

# 5. Teste manual das funcionalidades alteradas
# ✅ Verificar CRUD das tabelas modificadas
```

---

## MÉTRICAS DE SUCESSO

Ao final da sincronização, esperamos:

| Métrica | Atual | Meta |
|---------|:-----:|:----:|
| Problemas CRÍTICOS | 6 | **0** ✅ |
| Problemas ALTOS | 54 | **< 5** 🎯 |
| Problemas MÉDIOS | 163 | **< 10** 🎯 |
| Cobertura de Tabelas | 76% | **> 95%** 🎯 |
| Modelos com [NotMapped] | 0% | **100%** 🎯 |

---

## ARQUIVOS DE REFERÊNCIA

| Arquivo | Descrição |
|---------|-----------|
| **AUDITORIA_BANCO_REAL_VS_MODELOS.md** | Análise detalhada completa (este documento) |
| **CORRECOES_MODELOS_CSHARP_BANCO_REAL.md** | Código C# pronto para aplicar |
| **RELATORIO_SINCRONIZACAO_BANCO_REAL.md** | Resumo executivo (você está aqui) |
| **analise_discrepancias.csv** | Dados brutos (243 linhas) |
| **Analisa-Schema.ps1** | Script PowerShell de análise |
| **schema_banco_real.csv** | Schema completo do banco SQL Server |

---

## CONCLUSÃO

A análise revelou **243 discrepâncias** entre o banco de dados REAL e os modelos C#, sendo:
- **6 CRÍTICAS** que podem causar erros imediatos em produção
- **54 ALTAS** representando perda de dados importantes
- **163 MÉDIAS** que precisam de organização/documentação
- **20 tabelas órfãs** que podem precisar de modelos

**AÇÃO URGENTE NECESSÁRIA:** Aplicar correções CRÍTICAS nas próximas 24-48 horas para prevenir erros em produção.

**Previsão de Conclusão:** 4-6 semanas para sincronização 100% completa.

---

**Relatório Gerado em:** 13/02/2026 14:30
**Método:** Conexão direta ao SQL Server `localhost\Frotix`
**Ferramenta:** `Analisa-Schema.ps1` v2.0
**Analista:** Claude (Anthropic AI) via sqlcmd
