# Correção Erro 500 - Agenda (RecuperaViagem)

**Data:** 14/02/2026
**Status:** ✅ Código Corrigido | ⏳ SQL Pendente de Execução

---

## 📋 Resumo do Problema

Ao clicar em eventos do calendário da Agenda, ocorria erro 500 no endpoint `/api/Agenda/RecuperaViagem`.

### Causa Raiz Identificada

**SqlException:** 4 colunas faltantes na tabela `Viagem` do banco de dados:
- `CartaoAbastecimentoDevolvido`
- `CartaoAbastecimentoEntregue`
- `DocumentoDevolvido`
- `DocumentoEntregue`

Essas colunas existem no modelo C# (`Viagem.cs`) mas não foram criadas no banco de dados.

---

## ✅ Correções Aplicadas (Já Commitadas)

### 1. **AgendaController.cs** - Melhorias no Endpoint
- ✅ Adicionado endpoint de diagnóstico `TesteRecuperaViagem`
- ✅ Melhorada validação e tratamento de erros em `RecuperaViagem`
- ✅ Adicionado logging detalhado para troubleshooting

**Commits:**
- `244018e4` - fix(agenda): corrigir erro 500 em RecuperaViagem
- `4e649299` - debug(agenda): adicionar diagnóstico detalhado

### 2. **Index.cshtml** - Correção JavaScript
- ✅ Adicionado referência ao `kendo-datetime.js` (linha 1885)
- ✅ Corrigido erro: "window.getKendoDateValue is not a function"

**Commit:** `a9325021` - fix(agenda): adicionar kendo-datetime.js helper

### 3. **SQL Migration Script** - Adicionar Colunas Faltantes
- ✅ Criado script idempotente: `Scripts/SQL/FIX_Add_Missing_Columns_Viagem.sql`
- ⏳ **PENDENTE DE EXECUÇÃO NO BANCO DE DADOS**

**Commit:** `7e43423d` - fix(viagem): adicionar colunas faltantes no banco de dados

---

## 🚀 Passos para Concluir a Correção

### Passo 1: Executar Script SQL

Abra o SQL Server Management Studio (SSMS) e execute:

```sql
-- Arquivo: FrotiX.Site.OLD\Scripts\SQL\FIX_Add_Missing_Columns_Viagem.sql
USE [Frotix];
GO

-- Adiciona DocumentoEntregue
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Viagem') AND name = 'DocumentoEntregue')
BEGIN
    ALTER TABLE dbo.Viagem ADD DocumentoEntregue bit NULL DEFAULT (0);
    PRINT '✅ Coluna DocumentoEntregue adicionada com sucesso.';
END
ELSE
    PRINT '⚠️ Coluna DocumentoEntregue já existe.';

-- Adiciona DocumentoDevolvido
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Viagem') AND name = 'DocumentoDevolvido')
BEGIN
    ALTER TABLE dbo.Viagem ADD DocumentoDevolvido bit NULL DEFAULT (0);
    PRINT '✅ Coluna DocumentoDevolvido adicionada com sucesso.';
END
ELSE
    PRINT '⚠️ Coluna DocumentoDevolvido já existe.';

-- Adiciona CartaoAbastecimentoEntregue
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Viagem') AND name = 'CartaoAbastecimentoEntregue')
BEGIN
    ALTER TABLE dbo.Viagem ADD CartaoAbastecimentoEntregue bit NULL DEFAULT (0);
    PRINT '✅ Coluna CartaoAbastecimentoEntregue adicionada com sucesso.';
END
ELSE
    PRINT '⚠️ Coluna CartaoAbastecimentoEntregue já existe.';

-- Adiciona CartaoAbastecimentoDevolvido
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Viagem') AND name = 'CartaoAbastecimentoDevolvido')
BEGIN
    ALTER TABLE dbo.Viagem ADD CartaoAbastecimentoDevolvido bit NULL DEFAULT (0);
    PRINT '✅ Coluna CartaoAbastecimentoDevolvido adicionada com sucesso.';
END
ELSE
    PRINT '⚠️ Coluna CartaoAbastecimentoDevolvido já existe.';

GO
```

### Passo 2: Verificar Colunas Criadas

```sql
-- Verificar se as 4 colunas foram criadas
SELECT
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Viagem'
  AND COLUMN_NAME IN (
      'DocumentoEntregue',
      'DocumentoDevolvido',
      'CartaoAbastecimentoEntregue',
      'CartaoAbastecimentoDevolvido'
  )
ORDER BY COLUMN_NAME;
```

**Resultado Esperado:** 4 linhas retornadas (uma para cada coluna)

### Passo 3: Recompilar Aplicação

```bash
cd "c:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD"
dotnet clean
dotnet build
dotnet run
```

### Passo 4: Testar Endpoint de Diagnóstico

Abra o navegador e acesse (substitua o ID por um ID válido de viagem):

```
http://localhost:5000/api/Agenda/TesteRecuperaViagem?id=95d94e8d-23ae-4c77-231a-08de68505d0f
```

**Resultado Esperado:**
```json
{
  "sucesso": true,
  "mensagem": "Viagem carregada e serializada com sucesso!",
  "dadosBasicos": {
    "viagemId": "95d94e8d-23ae-4c77-231a-08de68505d0f",
    "dataInicial": "2025-01-15",
    "descricao": "..."
  },
  "tamanhoJson": 12345
}
```

### Passo 5: Testar Funcionalidade na Agenda

1. Acesse a página da Agenda: `http://localhost:5000/Agenda`
2. Clique em qualquer evento do calendário
3. Verifique se o modal de viagem abre corretamente
4. Verifique se os DatePickers/TimePickers funcionam sem erros no console

---

## 🔍 Troubleshooting

### Se ainda ocorrer erro 500:

1. **Verificar logs do servidor:**
   ```bash
   # Procurar por "[RecuperaViagem]" ou "[TesteRecuperaViagem]" nos logs
   ```

2. **Verificar console do navegador (F12):**
   - Procurar por erros JavaScript
   - Verificar se `kendo-datetime.js` foi carregado (aba Network)

3. **Testar endpoint diretamente:**
   - Usar o endpoint de diagnóstico `TesteRecuperaViagem` primeiro
   - Verificar qual etapa falha: existência, carregamento ou serialização

### Se erros JavaScript persistirem:

1. **Limpar cache do navegador:** Ctrl + Shift + Delete
2. **Verificar se todos os scripts foram carregados:**
   - F12 → Network → Filter by JS
   - Procurar por `kendo-datetime.js` (deve retornar 200 OK)

---

## 📝 Notas Técnicas

### Arquivos Modificados

| Arquivo | Modificação | Commit |
|---------|-------------|--------|
| `Controllers/AgendaController.cs` | Endpoint diagnóstico + validação | `244018e4`, `4e649299` |
| `Pages/Agenda/Index.cshtml` | Script `kendo-datetime.js` | `a9325021` |
| `Scripts/SQL/FIX_Add_Missing_Columns_Viagem.sql` | Script migração | `7e43423d` |

### Colunas Adicionadas

| Coluna | Tipo | Nullable | Default |
|--------|------|----------|---------|
| `DocumentoEntregue` | bit | NULL | (0) |
| `DocumentoDevolvido` | bit | NULL | (0) |
| `CartaoAbastecimentoEntregue` | bit | NULL | (0) |
| `CartaoAbastecimentoDevolvido` | bit | NULL | (0) |

### Por Que Esse Erro Aconteceu?

O modelo C# (`Viagem.cs`) foi atualizado com 4 novas propriedades bool?, mas o script de criação do banco (`FrotiX.sql`) não foi executado novamente ou as colunas não foram adicionadas manualmente.

Quando o Entity Framework Core tenta fazer `SELECT *` da tabela `Viagem`, ele busca todas as propriedades do modelo C#, incluindo as 4 colunas que não existem no banco, causando `SqlException`.

---

## ✅ Checklist de Conclusão

- [ ] Script SQL executado sem erros
- [ ] Verificação de colunas retornou 4 linhas
- [ ] Aplicação recompilada e rodando
- [ ] Endpoint de diagnóstico retorna `"sucesso": true`
- [ ] Modal de viagem abre ao clicar em evento do calendário
- [ ] Sem erros no console do navegador (F12)
- [ ] DatePickers/TimePickers funcionando corretamente

---

**Próximo Passo:** Execute o script SQL e teste a aplicação! 🚀
