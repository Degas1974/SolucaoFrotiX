# CLAUDE.md - Configuracao Claude Code (Workspace Root)

> **ATENCAO CLAUDE:** Este arquivo e carregado automaticamente no inicio de cada sessao.
> **Versao:** 5.0
> **Ultima Atualizacao:** 10/02/2026

---

## PROTOCOLO DE INICIALIZACAO (LEIA PRIMEIRO)

### ACOES OBRIGATORIAS AO INICIAR SESSAO

Antes de qualquer resposta ao usuario, voce DEVE:

1. **Ler completamente** o arquivo `RegrasDesenvolvimentoFrotiX.md` (NESTE MESMO DIRETORIO - raiz do workspace)
2. **Ler completamente** o arquivo `ControlesKendo.md` (NESTE MESMO DIRETORIO - raiz do workspace)
3. **Se a tarefa envolver banco de dados:** Ler `FrotiX.Site.Fevereiro/FrotiX.sql`
4. **Confirmar mentalmente** que todos os arquivos foram lidos

**IMPORTANTE:** Nao prossiga sem ler `RegrasDesenvolvimentoFrotiX.md` e `ControlesKendo.md`. Eles contem TODAS as regras do projeto e dos controles Kendo UI.

---

## DIRETORIO PADRAO DE TRABALHO

**SEMPRE trabalhe no diretorio:** `FrotiX.Site.Fevereiro/`

- Este e o projeto ativo principal (Fevereiro 2026)
- Todos os caminhos relativos devem partir deste diretorio
- Ao buscar arquivos, priorize esta pasta
- `FrotiX.Site.OLD` e o projeto anterior (Janeiro 2026) - usar apenas para referencia
- Outros projetos (FrotiX.Site.Novo, FrotiX.Site.Q4, FrotiX.Site.Backup) sao legados

---

## HIERARQUIA DE ARQUIVOS DE REGRAS

| Arquivo | Funcao | Quando Ler |
|---------|--------|------------|
| **`RegrasDesenvolvimentoFrotiX.md`** | FONTE OFICIAL DE REGRAS (raiz do workspace) | **SEMPRE** (obrigatorio) |
| **`ControlesKendo.md`** | DOCUMENTACAO OFICIAL Telerik Kendo UI (raiz do workspace) | **SEMPRE** (obrigatorio) |
| **`CLAUDE.md`** | Indice e instrucoes de inicializacao (voce esta aqui) | Automatico |
| **`FrotiX.Site.Fevereiro/FrotiX.sql`** | Estrutura do banco de dados | Quando trabalhar com dados |

---

## SISTEMA DE MEMORIA PERMANENTE

### REGRA: Como Memorizar Regras Permanentemente

**GATILHOS** - Quando o usuario disser:
- "memorize", "guarde na memoria", "lembre-se disso"
- "adicione as regras", "isso e uma regra nova"
- "nunca esqueca", "sempre faca X", "de agora em diante"

**ACAO OBRIGATORIA:**

1. Abrir e ler: `RegrasDesenvolvimentoFrotiX.md` (raiz do workspace)
2. **VERIFICAR DUPLICATAS:** Procurar se a informacao ja existe no arquivo
   - **Se ja existe e esta COMPLETA:** Informar o usuario que a regra ja esta registrada
   - **Se ja existe mas esta INCOMPLETA/DESATUALIZADA:** Atualizar a entrada existente sem duplicar
   - **Se NAO existe:** Criar nova entrada seguindo os passos abaixo
3. Identificar a secao tematica mais apropriada do arquivo (respeitando a organizacao ja existente)
4. Adicionar a nova regra/orientacao seguindo o formato padrao abaixo
5. **NUNCA baguncar, reordenar ou reformatar** o conteudo ja existente no arquivo
6. Salvar arquivo
7. Confirmar ao usuario o que foi feito (criado novo, atualizado existente, ou ja existia)

### FORMATO PADRAO PARA NOVAS REGRAS

```markdown
### [NUMERO_SECAO].[NUMERO_SUBSECAO] [Nome da Regra]

**Contexto:** [Por que esta regra existe]

**Regra:** [O que deve ser feito/evitado - IMPERATIVO]

**Exemplo:**
\`\`\`[linguagem]
// Codigo de exemplo mostrando aplicacao da regra
\`\`\`

**Data de Adicao:** DD/MM/AAAA
```

---

## REGRAS CRITICAS (RESUMO RAPIDO)

### Banco de Dados
- **SEMPRE** consultar `FrotiX.sql` ANTES de codificar operacoes com banco
- Nunca assumir nome de coluna "de cabeca"
- Verificar tipos de dados, nullable, FKs

### Try-Catch
- **OBRIGATORIO** em TODAS as funcoes (C# e JS)
- Usar `Alerta.TratamentoErroComLinha(arquivo, metodo, erro)`
- Registrar no `ILogService` (backend)

### UI/UX
- **Alertas:** SEMPRE usar `Alerta.*` (SweetAlert), NUNCA `alert()`
- **Icones:** SEMPRE `fa-duotone`, NUNCA `fa-solid/regular/light`
- **Loading:** SEMPRE `FtxSpin.show()`, NUNCA spinner Bootstrap
- **Tooltips:** SEMPRE Syncfusion `data-ejtip`, NUNCA Bootstrap
- **DatePicker/TimePicker:** SEMPRE Telerik Kendo UI, NUNCA HTML5 nativo

### Git
- Branch preferencial: `main`
- **Push SEMPRE para `main`**
- Tipos de commit: `feat:`, `fix:`, `refactor:`, `docs:`, `style:`, `chore:`

---

## VERSIONAMENTO

| Versao | Data | Mudancas |
|--------|------|----------|
| 5.0 | 10/02/2026 | Projeto principal alterado para FrotiX.Site.Fevereiro |
| 4.0 | 09/02/2026 | Adicionado ControlesKendo.md como leitura obrigatoria, centralizado na raiz |
| 3.0 | 09/02/2026 | Movido para raiz do workspace, projeto padrao FrotiX.Site.OLD |
| 2.0 | 18/01/2026 | Reformulacao completa: protocolo de inicializacao, sistema de memoria permanente |
| 1.0 | 14/01/2026 | Versao inicial |
