# FrotiX 2026 – Guia Técnico do Projeto

Este repositório contém o código-fonte e os artefatos do sistema **FrotiX 2026**, uma aplicação Web ASP.NET Core MVC para **Gestão Corporativa de Frotas**.

⚠️ **ATENÇÃO – LEIA ANTES DE QUALQUER ALTERAÇÃO**

Este projeto possui **regras técnicas mandatórias**, fluxo de trabalho rígido e padrões obrigatórios.  
Qualquer desenvolvimento **fora dessas regras é considerado incorreto**.

---

## ✅ Documento ÚNICO de Regras (OBRIGATÓRIO)

Toda a filosofia, padrões, regras técnicas, UX, fluxo de trabalho, banco de dados e comportamento esperado de desenvolvedores e agentes de IA estão consolidados no arquivo abaixo:

➡️ **`RegrasDesenvolvimentoFrotiXPOE.md`**

📌 **Este é o ÚNICO arquivo vivo de regras do projeto.**

- Nenhuma regra técnica deve ser inferida “de cabeça”
- Nenhum código deve ser escrito sem respeitar este documento
- Em caso de conflito de interpretação, **este arquivo sempre vence**

---

## 🧱 Banco de Dados – Fonte da Verdade

A estrutura oficial do banco de dados SQL Server do FrotiX está documentada em:

➡️ **`FrotiX.txt`**

Regras fundamentais:
- O banco de dados **manda**
- Models C# **devem refletir exatamente** o banco
- Qualquer divergência deve ser apontada
- Alterações estruturais exigem:
  - Script SQL
  - Explicação de impacto
  - Diff mental (antes/depois)
  - Atualização do `FrotiX.txt`

---

## 🧠 Arquivos Históricos / Ponte (NÃO EDITAR)

Os arquivos abaixo **existem apenas como ponte para agentes e ferramentas** que procuram por eles automaticamente:

- `GEMINI.md`
- `CLAUDE.md`

⚠️ **NÃO devem ser atualizados**  
✅ Todas as regras estão em `RegrasDesenvolvimentoFrotiXPOE.md`

---

## 🔄 Fluxo de Trabalho Obrigatório (Resumo)

- Try-catch obrigatório em **todas** as funções (C# e JS)
- SweetAlert FrotiX obrigatório (proibido alert nativo)
- Ícones **sempre** FontAwesome Duotone
- Loading Overlay FrotiX obrigatório
- Documentação dupla: `.md` + `.html`
- Logs de conversa obrigatórios (`Conversas/`)
- Commits frequentes, descritivos e rastreáveis

---

## ✅ Conclusão

Antes de qualquer ação neste projeto:

1. Leia **`RegrasDesenvolvimentoFrotiXPOE.md`**
2. Consulte **`FrotiX.txt`** se houver banco de dados
3. Siga os padrões sem exceção

Este README existe para **evitar ambiguidades** e **proteger a consistência técnica do FrotiX**.