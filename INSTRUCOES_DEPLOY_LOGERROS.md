# 📋 INSTRUÇÕES: Deploy da Tabela LogErros em Produção

## 🎯 O QUE FOI CRIADO

Foram criados **2 scripts SQL**:

### 1. `002_Add_LogErros_To_FrotiX.sql`
- **Objetivo**: Adicionar ao arquivo master `FrotiX.sql` (controle de versão)
- **Ação necessária**: Você precisa copiar o conteúdo deste arquivo e colar NO FINAL do arquivo `FrotiX.Site.2026.01\FrotiX.sql`
- **Quando fazer**: Antes de comitar as alterações no Git

### 2. `PRODUCAO_Add_Table_LogErros.sql` ⭐
- **Objetivo**: Executar diretamente no banco de dados de PRODUÇÃO
- **Ação necessária**: Executar este script no SQL Server Management Studio conectado ao servidor CTRAN01
- **Quando fazer**: Imediatamente, em horário de baixo uso

---

## 🚀 PASSO A PASSO: Deploy em Produção

### ✅ ETAPA 1: Backup (OBRIGATÓRIO)
1. Conectar no SQL Server Management Studio
2. Servidor: `CTRAN01`
3. Banco: `Frotix`
4. Clicar com botão direito no banco → Tasks → Back Up...
5. Salvar backup em local seguro

### ✅ ETAPA 2: Executar o Script
1. Abrir o arquivo: `PRODUCAO_Add_Table_LogErros.sql`
2. No SSMS, abrir uma nova query (Ctrl+N)
3. Copiar TODO o conteúdo do arquivo
4. Colar na janela de query
5. **IMPORTANTE**: Verificar que está conectado no banco `Frotix` (ver dropdown superior)
6. Executar o script (F5 ou botão Execute)

### ✅ ETAPA 3: Verificar Resultado
O próprio script mostrará um RESUMO no final:

```
╔════════════════════════════════════════════════════════════════════════════════════╗
║                           RESUMO DA EXECUÇÃO                                         ║
╠════════════════════════════════════════════════════════════════════════════════════╣
║  Tabela LogErros: ✅ EXISTE
║  Total de Índices: 9 (esperado: 9)
║  Total de Estatísticas: 2 (esperado: 2)
║  Total de Registros: 0
╠════════════════════════════════════════════════════════════════════════════════════╣
║  STATUS: ✅ SUCESSO - Tabela configurada corretamente!
╚════════════════════════════════════════════════════════════════════════════════════╝
```

Se ver ✅ SUCESSO, está tudo OK!

### ✅ ETAPA 4: Atualizar FrotiX.sql (Controle de Versão)
1. Abrir o arquivo: `FrotiX.Site.2026.01\FrotiX.sql`
2. Ir até o final do arquivo (Ctrl+End)
3. Abrir: `002_Add_LogErros_To_FrotiX.sql`
4. Copiar TODO o conteúdo
5. Colar no final de `FrotiX.sql`
6. Salvar `FrotiX.sql`
7. Comitar no Git

### ✅ ETAPA 5: Reiniciar Aplicação
1. Reiniciar o IIS ou pool da aplicação FrotiX
2. Aguardar 30 segundos
3. Acessar: `/Administracao/LogErros` (se existir o menu)

---

## ⚠️ TROUBLESHOOTING

### ❌ Erro: "Este script deve ser executado no banco Frotix!"
**Solução**: No SSMS, selecionar o banco `Frotix` no dropdown superior antes de executar

### ❌ Erro: "Objeto 'LogErros' já existe"
**Solução**: Ignorar - o script já detecta isso e pula a criação. Veja a mensagem:
```
⚠️ Tabela LogErros JÁ EXISTE. Pulando criação.
```

### ❌ Erro: "Cannot insert the value NULL into column..."
**Problema**: Isso pode acontecer se a aplicação tentar gravar logs DURANTE a execução
**Solução**: Execute o script em horário de baixo uso (madrugada) OU pause temporariamente o IIS

---

## 📊 ESTRUTURA CRIADA

### Tabela: `dbo.LogErros`
- **27 colunas**: ID, DataHora, Tipo, Origem, Mensagem, StackTrace, etc.
- **2 colunas computadas**: MensagemCurta, HashErro (agrupamento automático)
- **9 índices otimizados**: Para consultas rápidas por data, tipo, usuário, URL, etc.
- **2 estatísticas**: Para otimização do plano de execução

### Tempo estimado de criação:
- Banco vazio: ~10 segundos
- Banco com dados: ~30 segundos a 2 minutos (depende do tamanho)

---

## 🔒 SEGURANÇA

✅ O script é **IDEMPOTENTE**: Pode ser executado múltiplas vezes sem causar erro  
✅ Usa `IF NOT EXISTS`: Só cria objetos que não existem  
✅ Usa `SET XACT_ABORT ON`: Rollback automático se der erro  
✅ Valida banco correto: Só executa se estiver no banco `Frotix`

---

## 📞 SUPORTE

Se encontrar algum erro não listado acima:
1. Tire print da mensagem de erro
2. Copie o texto completo do erro
3. Informe qual ETAPA estava executando
4. Verifique se o backup foi feito antes de tentar novamente

---

**Data de criação**: 09/02/2026  
**Versão**: 1.0  
**Autor**: Claude Code
