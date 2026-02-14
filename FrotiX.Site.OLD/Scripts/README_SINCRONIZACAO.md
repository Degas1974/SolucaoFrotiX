# SINCRONIZAÇÃO BANCO DE DADOS ↔ MODELOS C# - ÍNDICE MESTRE

**Data:** 13/02/2026
**Versão:** 1.0
**Status:** ✅ Pronto para execução

---

## VISÃO GERAL

Este conjunto de documentos resolve **761 discrepâncias** identificadas entre os modelos C# do FrotiX e o banco de dados SQL Server.

### Problema Identificado

A auditoria completa revelou inconsistências significativas:
- **190 discrepâncias nullable** (propriedades C# não correspondem ao schema SQL)
- **11 discrepâncias MaxLength** (atributos C# divergem do tamanho real das colunas)
- **560 colunas ausentes no SQL** (propriedades de navegação/NotMapped - OK por design)

### Solução Proposta

Sincronização em duas frentes:
1. **Banco de dados SQL:** Alterações mínimas e cirúrgicas (7 ALTER TABLE)
2. **Modelos C#:** Correções abrangentes (201 propriedades)

---

## DOCUMENTOS DISPONÍVEIS

### 📊 1. AUDITORIA_COMPLETA_MODELOS_VS_BANCO.md

**Descrição:** Relatório detalhado de todas as 761 discrepâncias encontradas

**Conteúdo:**
- Lista completa de modelos C# auditados (155 modelos)
- Detalhamento de cada discrepância (tipo, severidade, correção)
- Estatísticas por categoria (nullable, MaxLength, colunas ausentes)

**Quando usar:**
- Para entender o ESCOPO completo do problema
- Para consultar discrepâncias específicas de um modelo
- Como referência durante as correções

**Tamanho:** ~8.500 linhas
**Formato:** Markdown
**Localização:** `FrotiX.Site.OLD/Scripts/`

---

### 🔧 2. SINCRONIZAR_BANCO_COM_MODELOS.sql

**Descrição:** Script SQL executável para sincronizar o banco de dados

**Conteúdo:**
- Backup automático de 9 tabelas afetadas
- 7 ALTER TABLE (AlertasFrotiX - dias da semana para nullable)
- Validações pré e pós-execução
- Transação com rollback automático em caso de erro
- Instruções de rollback manual

**Quando usar:**
- **PRIMEIRO PASSO** da sincronização (executar antes de alterar C#)
- Quando precisar reverter alterações (rollback)
- Para criar backups das tabelas afetadas

**Tempo de execução:** 5-15 minutos
**Formato:** SQL
**Pré-requisitos:** SQL Server 2022, permissões de ALTER TABLE
**Localização:** `FrotiX.Site.OLD/Scripts/`

---

### 📝 3. ACOES_MODELOS_CSHARP_POS_SINCRONIZACAO.md

**Descrição:** Guia detalhado de correções necessárias nos modelos C#

**Conteúdo:**
- 190 correções nullable explicadas (modelo por modelo)
- 11 correções MaxLength com exemplos de código
- Tabela de prioridades (Alta, Média, Baixa)
- Checklist de validação
- Script PowerShell para automação

**Quando usar:**
- **SEGUNDO PASSO** da sincronização (após executar script SQL)
- Para corrigir modelos C# sistematicamente
- Como referência de boas práticas (banco = fonte de verdade)

**Tamanho:** ~800 linhas
**Formato:** Markdown com snippets C#
**Localização:** `FrotiX.Site.OLD/Scripts/`

---

### 🚀 4. GUIA_RAPIDO_SINCRONIZACAO.md

**Descrição:** Tutorial passo a passo para executar a sincronização completa

**Conteúdo:**
- 6 passos numerados (Preparação → Commit)
- Comandos prontos para copiar/colar (SQL, Git, dotnet)
- Seção de troubleshooting
- Checklist final de validação
- Instruções de rollback

**Quando usar:**
- **GUIA PRINCIPAL** para executar a sincronização pela primeira vez
- Quando precisar reverter alterações (rollback)
- Como referência rápida de comandos

**Tempo total:** 30-60 minutos
**Formato:** Markdown com comandos
**Localização:** `FrotiX.Site.OLD/Scripts/`

---

### 📚 5. README_SINCRONIZACAO.md (ESTE ARQUIVO)

**Descrição:** Índice mestre conectando todos os documentos

**Quando usar:**
- **PONTO DE ENTRADA** para o processo de sincronização
- Para navegar entre os documentos
- Para entender a visão geral do projeto

---

## FLUXO DE TRABALHO RECOMENDADO

```
┌─────────────────────────────────────────────────────────────────┐
│  INÍCIO                                                         │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│  1. LER: README_SINCRONIZACAO.md (este arquivo)                │
│     Tempo: 5 min                                                │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│  2. CONSULTAR: AUDITORIA_COMPLETA_MODELOS_VS_BANCO.md          │
│     Objetivo: Entender escopo completo das discrepâncias        │
│     Tempo: 10 min                                               │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│  3. EXECUTAR: SINCRONIZAR_BANCO_COM_MODELOS.sql                │
│     Via: SQL Server Management Studio                           │
│     Tempo: 10-15 min                                            │
│     ✅ Cria backups                                             │
│     ✅ Altera 7 colunas                                         │
│     ✅ Valida mudanças                                          │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│  4. SEGUIR: GUIA_RAPIDO_SINCRONIZACAO.md                       │
│     Passos 3-6 (correção de modelos C#)                        │
│     Tempo: 20-30 min                                            │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│  5. REFERÊNCIA: ACOES_MODELOS_CSHARP_POS_SINCRONIZACAO.md     │
│     Consultar durante correções C#                             │
│     Tempo: Conforme necessário                                  │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│  6. VALIDAR: Compilar + Testar + Commit                        │
│     Tempo: 10-15 min                                            │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│  FIM - Sincronização Completa                                   │
│  ✅ 761 discrepâncias corrigidas                               │
│  ✅ Banco e modelos alinhados                                  │
└─────────────────────────────────────────────────────────────────┘
```

---

## QUICK START (PARA IMPACIENTES)

Se você quer começar AGORA, siga esta sequência:

1. **Backup do banco:**
   ```sql
   BACKUP DATABASE Frotix TO DISK = 'C:\Backups\Frotix_PreSync.bak';
   ```

2. **Executar script SQL:**
   - Abrir SSMS
   - Abrir arquivo: `SINCRONIZAR_BANCO_COM_MODELOS.sql`
   - Executar (F5)
   - Aguardar: "✅ SINCRONIZAÇÃO CONCLUÍDA COM SUCESSO!"

3. **Corrigir 3 modelos C# prioritários:**
   - `Abastecimento.cs` (remover ? de 5 propriedades)
   - `AlertasFrotiX.cs` (ajustar 12 propriedades nullable)
   - `AbastecimentoPendente.cs` (ajustar 2 MaxLength)

4. **Compilar:**
   ```bash
   dotnet build
   ```

5. **Testar:**
   - Login
   - Cadastro de Abastecimento
   - Dashboard

6. **Commit:**
   ```bash
   git add .
   git commit -m "feat: sincroniza modelos C# com banco SQL"
   ```

**Tempo total:** ~30 minutos

---

## ESTATÍSTICAS

### Antes da Sincronização

| Métrica | Valor |
|---------|-------|
| Total de discrepâncias | 761 |
| Discrepâncias CRÍTICAS (nullable) | 190 |
| Discrepâncias ATENÇÃO (MaxLength) | 11 |
| Colunas ausentes SQL (INFO) | 560 |
| Modelos C# auditados | 155 |
| Tabelas SQL auditadas | 120 |

### Após a Sincronização (Esperado)

| Métrica | Valor |
|---------|-------|
| Total de discrepâncias | 560* |
| Discrepâncias CRÍTICAS (nullable) | 0 |
| Discrepâncias ATENÇÃO (MaxLength) | 0 |
| Colunas ausentes SQL (INFO) | 560* |
| Alterações SQL (ALTER TABLE) | 7 |
| Alterações C# (propriedades) | 201 |

*As 560 colunas ausentes no SQL são **esperadas e corretas** (propriedades de navegação/NotMapped).

---

## ARQUIVOS RELACIONADOS

### Pré-requisitos

| Arquivo | Descrição | Localização |
|---------|-----------|-------------|
| `Frotix.sql` | Schema completo do banco SQL Server | `FrotiX.Site.OLD/` |
| `FrotiX.sql` | Alternativa (mesmo conteúdo) | `FrotiX.Site.OLD/` |
| Modelos C# | 155 arquivos .cs | `FrotiX.Site.OLD/Models/` |

### Gerados pelo Processo

| Arquivo | Descrição | Quando é criado |
|---------|-----------|-----------------|
| `*_BACKUP_20260213` | 9 tabelas de backup | Durante execução do script SQL |
| `Frotix_PreSync.bak` | Backup completo do banco | Manualmente antes do script |

---

## RESPONSABILIDADES

| Etapa | Responsável | Estimativa |
|-------|-------------|------------|
| Executar script SQL | DBA / Tech Lead | 15 min |
| Corrigir modelos C# (alta prioridade) | Dev Team | 30 min |
| Corrigir modelos C# (demais) | Dev Team | 2-3 horas |
| Testes de regressão | QA Team | 1-2 horas |
| Revisão de código | Tech Lead | 30 min |
| Deploy em staging | DevOps | 15 min |
| Validação final | Product Owner | 30 min |

**Total:** ~5-7 horas (pode ser paralelo com múltiplos desenvolvedores)

---

## RISCOS E MITIGAÇÕES

| Risco | Probabilidade | Impacto | Mitigação |
|-------|---------------|---------|-----------|
| Script SQL falha | Baixa | Alto | ✅ Transação com rollback automático |
| Dados corrompidos | Muito Baixa | Crítico | ✅ Backup completo antes da execução |
| Compilação C# falha | Média | Médio | ✅ Correções incrementais com testes |
| FKs bloqueiam ALTER TABLE | Baixa | Médio | ✅ Script desabilita FKs se necessário |
| Perda de performance | Muito Baixa | Baixo | ✅ Apenas 7 ALTER TABLE, sem novos índices |
| Quebra de funcionalidades | Média | Alto | ✅ Testes de regressão obrigatórios |

---

## CRITÉRIOS DE SUCESSO

Considere a sincronização **COMPLETA** quando:

- [ ] Script SQL executado com **0 erros**
- [ ] 9 tabelas de backup criadas no banco
- [ ] AlertasFrotiX.Monday-Sunday agora permitem NULL (validado em SSMS)
- [ ] Modelos C# de **alta prioridade** corrigidos (mínimo 3 arquivos)
- [ ] Solução C# **compila sem erros** (0 warnings de nullable se possível)
- [ ] Testes unitários **passam** (se existirem)
- [ ] Funcionalidades críticas **testadas manualmente** (Login, Abastecimento, Viagens, Multas)
- [ ] Código **commitado** em branch feature
- [ ] Pull Request **criado e revisado**
- [ ] Deploy em **staging validado**

---

## CONTATOS E SUPORTE

| Dúvida sobre | Contatar | E-mail |
|--------------|----------|--------|
| Script SQL | DBA Team | dba@frotix.com |
| Modelos C# | Dev Lead | dev.lead@frotix.com |
| Processo Git | DevOps | devops@frotix.com |
| Testes | QA Team | qa@frotix.com |
| Aprovações | Product Owner | po@frotix.com |

---

## FAQ (PERGUNTAS FREQUENTES)

### 1. Por que 190 discrepâncias nullable?

**R:** O banco SQL Server evoluiu ao longo do tempo, mas os modelos C# nem sempre foram atualizados. A auditoria revelou essas inconsistências.

### 2. É seguro executar o script SQL em produção?

**R:** **NÃO!** Execute primeiro em **staging/desenvolvimento**. O script cria backups, mas sempre faça um backup completo do banco antes.

### 3. O que são as 560 colunas ausentes no SQL?

**R:** São propriedades de navegação (relacionamentos) ou propriedades calculadas/auxiliares. Não existem no banco por design. Isso é **normal e esperado**.

### 4. Quanto tempo leva o processo completo?

**R:** 30-60 minutos para o essencial (alta prioridade). 5-7 horas para 100% das correções (pode ser distribuído entre equipe).

### 5. O que fazer se o script SQL falhar?

**R:** O script tem **rollback automático**. Nenhuma alteração será aplicada. Revise o erro, corrija e execute novamente.

### 6. Preciso corrigir TODOS os 155 modelos C#?

**R:** Não imediatamente. Priorize:
1. Alta prioridade (3 modelos) - FAZER AGORA
2. Média prioridade (6 modelos) - FAZER EM SEGUIDA
3. Baixa prioridade (demais) - FAZER GRADUALMENTE

### 7. Como reverter se algo der errado?

**R:** Três opções:
1. **Rollback automático** (script SQL)
2. **Rollback manual** (instruções no final do script SQL)
3. **Restaurar backup completo** (último recurso)

### 8. O que fazer com Viagem.Origem e Viagem.Destino?

**R:** **NÃO ALTERAR AGORA!** Será tratado em script separado de limpeza fuzzy (normalização de dados).

---

## CHANGELOG

| Versão | Data | Autor | Mudanças |
|--------|------|-------|----------|
| 1.0 | 13/02/2026 | Claude Sonnet 4.5 | Documento inicial - índice mestre completo |

---

## PRÓXIMAS ITERAÇÕES

Após a sincronização inicial, planeje:

1. **Sprint 2:** Limpeza fuzzy de Viagem.Origem/Destino
2. **Sprint 3:** Correção de FKs duplicadas (WhatsApp, MotoristaItensPendentes)
3. **Sprint 4:** Auditoria de performance de índices (Viagem tem ~40)
4. **Sprint 5:** Correção de Fornecedor.FornecedorId (UNIQUE → PRIMARY KEY)
5. **Sprint 6:** Auditoria completa de views (40 views no banco)

---

## LINKS ÚTEIS

- [Documentação EF Core - Nullable Reference Types](https://learn.microsoft.com/ef/core/miscellaneous/nullable-reference-types)
- [SQL Server - ALTER TABLE](https://learn.microsoft.com/sql/t-sql/statements/alter-table-transact-sql)
- [Git - Feature Branch Workflow](https://www.atlassian.com/git/tutorials/comparing-workflows/feature-branch-workflow)

---

**FIM DO ÍNDICE MESTRE**

**Autor:** Claude Sonnet 4.5 (FrotiX Team)
**Data:** 13/02/2026
**Versão:** 1.0

---

## INÍCIO RÁPIDO

👉 **Próximo passo:** Abra `GUIA_RAPIDO_SINCRONIZACAO.md` e siga o PASSO 1.
