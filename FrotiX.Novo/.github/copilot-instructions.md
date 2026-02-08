# 🤖 GITHUB COPILOT - Regras de Desenvolvimento FrotiX

> **ATENÇÃO COPILOT:** Leia este arquivo antes de gerar sugestões de código.
> **Versão:** 2.0
> **Última Atualização:** 29/01/2026

---

## 👋 MENSAGEM DE BOAS-VINDAS

Bem-vindo à sessão de desenvolvimento FrotiX com GitHub Copilot!

**ANTES DE QUALQUER SUGESTÃO:**

✅ Leia o arquivo `./RegrasDesenvolvimentoFrotiX.md` antes de qualquer coisa.

✅ Se for mexer em banco de dados, leia também o `./FrotiX.sql`, que é nossa estrutura completa do SQL Server.

**REGRA CRÍTICA PARA ALTERAÇÕES DE BANCO:**

Se você for fazer algum acréscimo, decréscimo ou alteração de recursos no Banco, você tem que:

1. **Confrontar** seu código contra o `FrotiX.sql` para ver se não há nenhum tipo de incompatibilidade
2. **Não havendo incompatibilidades**, atualizar o `FrotiX.sql` com suas alterações
3. **Gerar** um `script.sql` separado para rodar tanto no banco de produção como no de desenvolvimento

---

## ✅ CONFIRMAÇÃO VISUAL OBRIGATÓRIA

**AO INICIAR CADA NOVA SESSÃO/CHAT**, você DEVE exibir a seguinte mensagem de confirmação ANTES da primeira resposta ao usuário:

```
✅ FROTIX - GITHUB COPILOT CONFIGURADO

📚 Arquivos Carregados:
  ✅ RegrasDesenvolvimentoFrotiX.md
  ✅ FrotiX.sql (estrutura do banco - quando necessário)

⚠️ Regras Críticas Ativas:
  • Try-catch obrigatório em todas funções
  • Usar Alerta.* (NUNCA alert())
  • Usar fa-duotone (NUNCA fa-solid)
  • Consultar FrotiX.sql antes de alterar banco
  • Gerar script.sql para alterações de banco

🚀 Pronto para começar! Como posso ajudar?
```

---

## 📋 ARQUIVO PRINCIPAL DE REGRAS

Este arquivo redireciona para o arquivo ÚNICO e OFICIAL de regras do projeto:

→ **`./RegrasDesenvolvimentoFrotiX.md`**

**IMPORTANTE:**
- TODAS as regras detalhadas estão em `RegrasDesenvolvimentoFrotiX.md`
- NUNCA adicione regras aqui. Adicione APENAS em `RegrasDesenvolvimentoFrotiX.md`

---

## ⚠️ REGRAS CRÍTICAS (RESUMO RÁPIDO)

### 🗄️ Banco de Dados
- SEMPRE consultar `FrotiX.sql` ANTES de codificar operações com banco
- Nunca assumir nome de coluna "de cabeça"
- Verificar tipos de dados, nullable, FKs

### 🔒 Try-Catch
- OBRIGATÓRIO em TODAS as funções (C# e JS)
- Usar `Alerta.TratamentoErroComLinha(arquivo, metodo, erro)`

### 🎨 UI/UX
- Alertas: SEMPRE usar `Alerta.*` (SweetAlert), NUNCA `alert()`
- Ícones: SEMPRE `fa-duotone`, NUNCA `fa-solid/regular/light`
- Loading: SEMPRE `FtxSpin.show()`, NUNCA spinner Bootstrap
- Tooltips: SEMPRE Syncfusion `data-ejtip`, NUNCA Bootstrap

### 📝 Documentação
- SEMPRE atualizar documentação em `Documentacao/` antes de commitar
- Formato: Prosa técnica + snippets + explicação linha-a-linha

### 🔄 Git
- Branch preferencial: `main`
- Push SEMPRE para `main`
- Commit e push automáticos IMEDIATOS após criar/alterar código importante

---

## 🎯 SUGESTÕES DE CÓDIGO

Ao gerar sugestões de código:

1. Consultar `RegrasDesenvolvimentoFrotiX.md` para padrões
2. Se envolver banco: consultar `FrotiX.sql` para estrutura
3. Seguir TODAS as regras visuais e de código
4. Incluir try-catch em TODAS as funções
5. Usar componentes e padrões FrotiX (Alerta, FtxSpin, etc.)

---

✅ **GitHub Copilot configurado para FrotiX. Leia `RegrasDesenvolvimentoFrotiX.md` antes de sugerir código.**
