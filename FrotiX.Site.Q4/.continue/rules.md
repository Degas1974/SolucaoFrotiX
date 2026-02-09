# ⚠️ REDIRECIONAMENTO → ../RegrasDesenvolvimentoFrotiX.md

> **ATENÇÃO CONTINUE:** Este arquivo redireciona para as regras completas do projeto.
> **Versão:** 2.0
> **Última Atualização:** 29/01/2026

---

## 👋 MENSAGEM DE BOAS-VINDAS

Bem-vindo à sessão de desenvolvimento FrotiX com Continue!

**ANTES DE QUALQUER AÇÃO:**

✅ Leia o arquivo `../RegrasDesenvolvimentoFrotiX.md` antes de qualquer coisa.

✅ Se for mexer em banco de dados, leia também o `../FrotiX.sql`, que é nossa estrutura completa do SQL Server.

**REGRA CRÍTICA PARA ALTERAÇÕES DE BANCO:**

Se você for fazer algum acréscimo, decréscimo ou alteração de recursos no Banco, você tem que:

1. **Confrontar** seu código contra o `FrotiX.sql` para ver se não há nenhum tipo de incompatibilidade
2. **Não havendo incompatibilidades**, atualizar o `FrotiX.sql` com suas alterações
3. **Gerar** um `script.sql` separado para rodar tanto no banco de produção como no de desenvolvimento

---

## ✅ CONFIRMAÇÃO VISUAL OBRIGATÓRIA

**AO INICIAR CADA NOVA SESSÃO/CHAT**, você DEVE exibir a seguinte mensagem de confirmação ANTES da primeira resposta ao usuário:

```
✅ FROTIX - CONTINUE CONFIGURADO

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

## 📋 ARQUIVOS DE REGRAS

| Arquivo | Função |
|---------|--------|
| **`../RegrasDesenvolvimentoFrotiX.md`** | ⭐ Regras completas (LEIA PRIMEIRO) |
| **`.continue/rules/frotix-rules.md`** | Regras no formato Continue (auto-load) |
| **`.continue/config.json`** | Configuração + systemMessage |

**IMPORTANTE:**
- TODAS as regras detalhadas estão em `RegrasDesenvolvimentoFrotiX.md`
- NUNCA adicione regras aqui. Adicione APENAS em `RegrasDesenvolvimentoFrotiX.md`

---

## 🎯 SLASH COMMANDS DISPONÍVEIS

- `/regras` - Ver resumo das regras
- `/memorizar` - Adicionar nova regra permanente
- `/banco` - Consultar estrutura do banco
- `/test` - Gerar testes unitários
- `/check` - Verificar conformidade com regras

---

✅ **Continue configurado. Aguardando suas instruções.**
