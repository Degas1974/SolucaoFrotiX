# GitHub Copilot - Instrucoes do Projeto FrotiX

> **Sistema:** GitHub Copilot (Chat, Agent, Edits & Inline Suggestions)
> **Versao:** 3.0
> **Ultima Atualizacao:** 09/02/2026

---

## LEITURA OBRIGATORIA (ANTES DE QUALQUER RESPOSTA)

**No inicio de CADA sessao/chat/agente/editor**, voce DEVE ler OBRIGATORIAMENTE estes 2 arquivos na **RAIZ do workspace** (`Solucao FrotiX 2026/`):

1. **`RegrasDesenvolvimentoFrotiX.md`** - Regras OFICIAIS de desenvolvimento (TODAS as convencoes, padroes UI/UX, banco de dados, etc.)
2. **`ControlesKendo.md`** - Documentacao OFICIAL dos controles Telerik Kendo UI (DatePicker, TimePicker, Grid, etc.)

**IMPORTANTE:**
- Estes sao os UNICOS arquivos oficiais. NAO existem copias nos subprojetos.
- NUNCA gere codigo sem antes ter lido ambos os arquivos.
- Se a tarefa envolver banco de dados, leia tambem `FrotiX.Site.OLD/FrotiX.sql`.

---

## DIRETORIO PADRAO DE TRABALHO

**SEMPRE trabalhe no diretorio:** `FrotiX.Site.OLD/`

- Este e o projeto ativo principal (Janeiro 2026)
- Todos os caminhos relativos devem partir deste diretorio
- Ao buscar arquivos, priorize esta pasta
- `FrotiX.Site.Fevereiro` e o projeto em migracao - trabalhar somente se solicitado
- Outros projetos sao legados

---

## REGRA CRITICA PARA ALTERACOES DE BANCO

Se voce for fazer algum acrescimo, decrescimo ou alteracao de recursos no Banco, voce tem que:

1. **Confrontar** seu codigo contra o `FrotiX.sql` para ver se nao ha nenhum tipo de incompatibilidade
2. **Nao havendo incompatibilidades**, atualizar o `FrotiX.sql` com suas alteracoes
3. **Gerar** um `script.sql` separado para rodar tanto no banco de producao como no de desenvolvimento

---

## CONFIRMACAO VISUAL OBRIGATORIA

**AO INICIAR CADA NOVA SESSAO/CHAT**, voce DEVE exibir a seguinte mensagem de confirmacao ANTES da primeira resposta ao usuario:

```
FROTIX - COPILOT CONFIGURADO

Arquivos Carregados:
  RegrasDesenvolvimentoFrotiX.md (regras de desenvolvimento)
  ControlesKendo.md (controles Telerik Kendo UI)
  FrotiX.sql (estrutura do banco - quando necessario)

Regras Criticas Ativas:
  - Try-catch obrigatorio em todas funcoes
  - Usar Alerta.* (NUNCA alert())
  - Usar fa-duotone (NUNCA fa-solid)
  - DatePicker/TimePicker: SEMPRE Telerik Kendo UI
  - Consultar FrotiX.sql antes de alterar banco
  - Gerar script.sql para alteracoes de banco

Pronto para comecar!
```

---

## REGRAS CRITICAS (RESUMO)

1. **Try-Catch OBRIGATORIO** em todas as funcoes (C# e JS)
2. **Alertas:** Usar `Alerta.*` (SweetAlert), NUNCA `alert()`
3. **Icones:** SEMPRE `fa-duotone`, NUNCA `fa-solid/regular/light`
4. **Loading:** SEMPRE `FtxSpin.show()`, NUNCA spinner Bootstrap
5. **Banco de dados:** SEMPRE consultar `FrotiX.sql` antes de codificar
6. **DatePicker/TimePicker:** SEMPRE Telerik Kendo UI (ver ControlesKendo.md)
7. **Tooltips:** SEMPRE Syncfusion `data-ejtip`
8. **Log de erros:** SEMPRE usar `ILogService` no backend

---

## MEMORIA PERMANENTE

Quando o usuario pedir para "memorizar", "guardar", "lembrar", "adicionar as regras", "nunca esquecer", ou "de agora em diante":

1. **Abrir e ler** `RegrasDesenvolvimentoFrotiX.md` na raiz do workspace
2. **VERIFICAR DUPLICATAS:** Procurar se a informacao ja existe no arquivo
   - **Se ja existe e esta COMPLETA:** Informar que a regra ja esta registrada
   - **Se ja existe mas esta INCOMPLETA/DESATUALIZADA:** Atualizar a entrada existente sem duplicar
   - **Se NAO existe:** Criar nova entrada na secao tematica apropriada
3. **Respeitar a organizacao existente** do arquivo - NUNCA baguncar, reordenar ou reformatar o que ja esta la
4. Seguir o formato de numeracao e estilo ja usado no arquivo (secao.subsecao)
5. Confirmar ao usuario o que foi feito (criado novo, atualizado existente, ou ja existia)
