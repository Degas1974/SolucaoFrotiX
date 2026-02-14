# GUIA RÁPIDO: Sincronização Banco ↔ Modelos C#

**Data:** 13/02/2026
**Versão:** 1.0
**Tempo estimado:** 30-60 minutos

---

## OVERVIEW

Este guia descreve o processo completo para sincronizar o banco de dados FrotiX com os modelos C#, corrigindo **761 discrepâncias** identificadas na auditoria.

---

## PASSO 1: PREPARAÇÃO (5 min)

### 1.1 Backup do Banco de Dados

```sql
-- SQL Server Management Studio (SSMS)
BACKUP DATABASE Frotix
TO DISK = 'C:\Backups\Frotix_PreSincronizacao_20260213.bak'
WITH FORMAT, INIT, COMPRESSION;
GO
```

### 1.2 Criar Branch Git

```bash
cd "C:\FrotiX\Solucao FrotiX 2026"
git checkout -b feature/sincronizacao-modelos-banco
git status
```

### 1.3 Verificar Arquivos

Certifique-se de que os seguintes arquivos existem:

- ✅ `FrotiX.Site.OLD\Scripts\AUDITORIA_COMPLETA_MODELOS_VS_BANCO.md`
- ✅ `FrotiX.Site.OLD\Scripts\SINCRONIZAR_BANCO_COM_MODELOS.sql`
- ✅ `FrotiX.Site.OLD\Scripts\ACOES_MODELOS_CSHARP_POS_SINCRONIZACAO.md`
- ✅ `FrotiX.Site.OLD\Frotix.sql`

---

## PASSO 2: EXECUTAR SCRIPT SQL (10-15 min)

### 2.1 Abrir SSMS

1. Abrir SQL Server Management Studio
2. Conectar ao servidor do FrotiX
3. Selecionar banco `Frotix`

### 2.2 Executar Script

```sql
-- Abrir arquivo: SINCRONIZAR_BANCO_COM_MODELOS.sql
-- Executar (F5)

-- O script irá:
-- ✅ Criar backups de 9 tabelas
-- ✅ Alterar 7 colunas (AlertasFrotiX - dias da semana)
-- ✅ Validar alterações
-- ✅ Fazer COMMIT ou ROLLBACK automático
```

### 2.3 Validar Resultado

Verificar no painel de mensagens:

```
✅ SINCRONIZAÇÃO CONCLUÍDA COM SUCESSO!
📊 Discrepâncias nullable processadas: 190/190
📊 Discrepâncias MaxLength processadas: 11/11
📊 Total de erros: 0
⏱️ Tempo total de execução: XXs
```

### 2.4 Verificar Backups Criados

```sql
-- Listar tabelas de backup
SELECT name
FROM sys.tables
WHERE name LIKE '%_BACKUP_20260213'
ORDER BY name;

-- Resultado esperado:
-- Abastecimento_BACKUP_20260213
-- AbastecimentoPendente_BACKUP_20260213
-- AlertasFrotiX_BACKUP_20260213
-- AlertasUsuario_BACKUP_20260213
-- AnosDisponiveisAbastecimento_BACKUP_20260213
-- AspNetUsers_BACKUP_20260213
-- AtaRegistroPrecos_BACKUP_20260213
-- Combustivel_BACKUP_20260213
-- Contrato_BACKUP_20260213
```

---

## PASSO 3: CORRIGIR MODELOS C# (20-30 min)

### 3.1 Abrir Visual Studio

```bash
# Abrir solução
start "FrotiX.Site.OLD\FrotiX.Site.OLD.csproj"
```

### 3.2 Corrigir Modelos de Alta Prioridade

Siga as instruções em `ACOES_MODELOS_CSHARP_POS_SINCRONIZACAO.md`:

#### Alta Prioridade (FAZER AGORA):

1. **Abastecimento.cs** (5 correções nullable)
   ```csharp
   // Remover ? de: Litros, ValorUnitario, DataHora, KmRodado, Hodometro
   public double Litros { get; set; } // era: double?
   ```

2. **AlertasFrotiX.cs** (12 correções nullable)
   ```csharp
   // Remover ? de: Titulo, Descricao, DataInsercao, UsuarioCriadorId
   public string Titulo { get; set; } = string.Empty; // era: string?

   // Adicionar ? em: Monday-Sunday
   public bool? Monday { get; set; } // era: bool
   ```

3. **AbastecimentoPendente.cs** (2 correções MaxLength)
   ```csharp
   [MaxLength(50)] // era: 2000
   public string? TipoPendencia { get; set; }

   [MaxLength(20)] // era: 50
   public string? CampoCorrecao { get; set; }
   ```

#### Média Prioridade (fazer em seguida):

4. AlertasUsuario.cs (1 correção)
5. AnosDisponiveisAbastecimento.cs (2 correções)
6. AspNetUsers.cs (1 correção)
7. AtaRegistroPrecos.cs (4 correções)
8. Combustivel.cs (1 correção)
9. Contrato.cs (6 correções)

#### Baixa Prioridade (fazer depois):

10-50. Demais modelos (ver lista completa no documento de ações)

---

## PASSO 4: COMPILAR E TESTAR (5-10 min)

### 4.1 Compilar Solução

```bash
# Visual Studio: Build > Rebuild Solution (Ctrl+Shift+B)

# OU via linha de comando:
dotnet build "FrotiX.Site.OLD\FrotiX.Site.OLD.csproj" --configuration Release
```

### 4.2 Verificar Erros de Compilação

Se houver erros relacionados a nullable:

```csharp
// ERRO: Cannot convert null to 'double' because it is a non-nullable value type

// SOLUÇÃO: Verificar se a propriedade deve ser nullable ou não
// Consultar AUDITORIA_COMPLETA_MODELOS_VS_BANCO.md
```

### 4.3 Executar Testes Unitários (se existirem)

```bash
dotnet test "FrotiX.Site.OLD.Tests\FrotiX.Site.OLD.Tests.csproj"
```

---

## PASSO 5: VALIDAÇÃO FINAL (5 min)

### 5.1 Executar Nova Auditoria

```bash
# Executar script de auditoria novamente
# Verificar se o número de discrepâncias diminuiu
```

### 5.2 Testar Funcionalidades Críticas

- [ ] Login
- [ ] Cadastro de Abastecimento
- [ ] Cadastro de Viagem
- [ ] Listagem de Multas
- [ ] Dashboard principal

---

## PASSO 6: COMMIT E PUSH (5 min)

### 6.1 Revisar Alterações

```bash
git status
git diff
```

### 6.2 Commit

```bash
git add .
git commit -m "feat: sincroniza modelos C# com banco de dados SQL

- Corrige 190 discrepâncias nullable (Abastecimento, AlertasFrotiX, etc.)
- Corrige 11 discrepâncias MaxLength (AbastecimentoPendente, etc.)
- Executa script SINCRONIZAR_BANCO_COM_MODELOS.sql
- Cria backups de 9 tabelas afetadas
- Altera AlertasFrotiX para permitir NULL em dias da semana

Ref: AUDITORIA_COMPLETA_MODELOS_VS_BANCO.md

Co-Authored-By: Claude Sonnet 4.5 <noreply@anthropic.com>"
```

### 6.3 Push

```bash
git push origin feature/sincronizacao-modelos-banco
```

### 6.4 Criar Pull Request

```bash
# Usar GitHub/GitLab/Azure DevOps para criar PR
# Título: "[FEAT] Sincronização Banco ↔ Modelos C#"
# Descrição: Corrige 761 discrepâncias identificadas na auditoria
```

---

## ROLLBACK (EM CASO DE ERRO)

### Opção 1: Rollback do Banco de Dados

```sql
-- Se o script SQL deu erro, ele faz rollback automático
-- Se precisar reverter manualmente:

USE Frotix;
GO

-- Restaurar dados dos backups (ver instruções no final do script SQL)
-- ATENÇÃO: Isso irá sobrescrever dados atuais!
```

### Opção 2: Rollback do Código C#

```bash
# Descartar alterações no Git
git checkout main
git branch -D feature/sincronizacao-modelos-banco
```

### Opção 3: Restaurar Backup Completo

```sql
-- Restaurar backup do banco inteiro
USE master;
GO

ALTER DATABASE Frotix SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO

RESTORE DATABASE Frotix
FROM DISK = 'C:\Backups\Frotix_PreSincronizacao_20260213.bak'
WITH REPLACE;
GO

ALTER DATABASE Frotix SET MULTI_USER;
GO
```

---

## TROUBLESHOOTING

### Problema 1: Script SQL falha com erro de FK

**Sintoma:**
```
The ALTER TABLE statement conflicted with the FOREIGN KEY constraint "FK_..."
```

**Solução:**
```sql
-- Desabilitar FK temporariamente
ALTER TABLE dbo.TabelaProblematica NOCHECK CONSTRAINT ALL;

-- Executar ALTER TABLE

-- Reabilitar FK
ALTER TABLE dbo.TabelaProblematica CHECK CONSTRAINT ALL;
```

### Problema 2: Compilação C# falha após correções

**Sintoma:**
```
CS0266: Cannot implicitly convert type 'bool?' to 'bool'
```

**Solução:**
```csharp
// Usar null-coalescing operator
bool valor = propriedadeNullable ?? false;

// OU verificar null explicitamente
if (propriedadeNullable.HasValue && propriedadeNullable.Value)
{
    // ...
}
```

### Problema 3: EF Core não reconhece alterações

**Sintoma:**
Migrations geram código para recriar colunas já existentes

**Solução:**
```bash
# Remover migration pendente
dotnet ef migrations remove

# Atualizar modelo do banco
dotnet ef dbcontext scaffold "ConnectionString" Microsoft.EntityFrameworkCore.SqlServer --force
```

---

## CHECKLIST FINAL

Antes de considerar a sincronização completa:

- [ ] Script SQL executado com sucesso (0 erros)
- [ ] 9 tabelas de backup criadas
- [ ] AlertasFrotiX.Monday-Sunday agora permitem NULL
- [ ] Modelos C# de alta prioridade corrigidos (3 arquivos mínimo)
- [ ] Solução C# compila sem erros
- [ ] Testes unitários passam (se existirem)
- [ ] Funcionalidades críticas testadas manualmente
- [ ] Commit criado com mensagem descritiva
- [ ] Push realizado para branch feature
- [ ] Pull Request criado (opcional, dependendo do workflow)

---

## MÉTRICAS DE SUCESSO

| Métrica | Antes | Depois | Status |
|---------|-------|--------|--------|
| Discrepâncias nullable | 190 | 0 | ⏳ Em andamento |
| Discrepâncias MaxLength | 11 | 0 | ⏳ Em andamento |
| Colunas ausentes no SQL | 560 | 560 | ✅ OK (NotMapped) |
| Erros de compilação | ? | 0 | ⏳ Em andamento |
| Testes falhando | ? | 0 | ⏳ Em andamento |

---

## PRÓXIMOS PASSOS (PÓS-SINCRONIZAÇÃO)

1. **Limpeza fuzzy de Viagem.Origem/Destino**
   - Script separado (não incluído aqui)
   - Normalização de dados

2. **Correção de FKs duplicadas**
   - WhatsAppMensagens
   - WhatsAppFilaMensagens
   - MotoristaItensPendentes

3. **Correção de Fornecedor.FornecedorId**
   - Converter UNIQUE INDEX em PRIMARY KEY
   - Requer aprovação de DBA

4. **Otimização de índices em Viagem**
   - Tabela já tem ~40 índices
   - Analisar performance antes de adicionar mais

---

## CONTATOS

| Responsável | E-mail | Função |
|-------------|--------|--------|
| DBA Team | dba@frotix.com | Aprovações de schema |
| Dev Lead | dev.lead@frotix.com | Revisão de código |
| QA Team | qa@frotix.com | Testes de regressão |

---

## REFERÊNCIAS

1. `AUDITORIA_COMPLETA_MODELOS_VS_BANCO.md` - Fonte de verdade para discrepâncias
2. `SINCRONIZAR_BANCO_COM_MODELOS.sql` - Script de sincronização SQL
3. `ACOES_MODELOS_CSHARP_POS_SINCRONIZACAO.md` - Guia detalhado de correções C#
4. `Frotix.sql` - Schema completo do banco

---

**Autor:** Claude Sonnet 4.5 (FrotiX Team)
**Data:** 13/02/2026
**Versão:** 1.0
