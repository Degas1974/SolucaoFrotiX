# GitHub Copilot - Instrucoes do Projeto FrotiX

> **Sistema:** GitHub Copilot (Chat & Inline Suggestions)
> **Versao:** 2.0
> **Ultima Atualizacao:** 09/02/2026

---

## LEITURA OBRIGATORIA

**ANTES de gerar qualquer codigo**, leia o arquivo de regras consolidadas:

**`RegrasDesenvolvimentoFrotiX.md`** (raiz do workspace)

Este arquivo contem TODAS as regras de desenvolvimento, padroes de codigo, convencoes de UI/UX e fluxo de trabalho do projeto FrotiX.

---

## DIRETORIO PADRAO DE TRABALHO

**SEMPRE trabalhe no diretorio:** `FrotiX.Site.OLD/`

- Este e o projeto ativo principal (Janeiro 2026)
- Todos os caminhos relativos devem partir deste diretorio
- Ao buscar arquivos, priorize esta pasta
- `FrotiX.Site.Fevereiro` e o projeto em migracao - trabalhar somente se solicitado
- Outros projetos sao legados

---

## REGRAS CRITICAS (RESUMO)

1. **Try-Catch OBRIGATORIO** em todas as funcoes (C# e JS)
2. **Alertas:** Usar `Alerta.*` (SweetAlert), NUNCA `alert()`
3. **Icones:** SEMPRE `fa-duotone`, NUNCA `fa-solid/regular/light`
4. **Loading:** SEMPRE `FtxSpin.show()`, NUNCA spinner Bootstrap
5. **Banco de dados:** SEMPRE consultar `FrotiX.sql` antes de codificar
6. **DatePicker/TimePicker:** SEMPRE Telerik Kendo UI
7. **Tooltips:** SEMPRE Syncfusion `data-ejtip`
8. **Log de erros:** SEMPRE usar `ILogService` no backend

---

## MEMORIA PERMANENTE

Se o usuario pedir para "memorizar" algo, adicione ao arquivo `RegrasDesenvolvimentoFrotiX.md` na raiz do workspace.
