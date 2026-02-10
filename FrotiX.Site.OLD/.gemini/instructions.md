# 🤖 GENIE AI / GEMINI - Regras de Desenvolvimento FrotiX

> **ATENÇÃO GEMINI:** Leia este arquivo antes de gerar código.
> **Versão:** 2.0
> **Última Atualização:** 29/01/2026
> **Status Auto-Load:** ❓ Não confirmado (necessita teste)

---

## 👋 MENSAGEM DE BOAS-VINDAS

Bem-vindo à sessão de desenvolvimento FrotiX com Gemini!

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
✅ FROTIX - GEMINI CONFIGURADO

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

→ **`../RegrasDesenvolvimentoFrotiX.md`**

**IMPORTANTE:**
- Este arquivo (`instructions.md`) contém apenas a mensagem inicial e redirecionamento
- TODAS as regras detalhadas estão em `RegrasDesenvolvimentoFrotiX.md`
- NUNCA adicione regras aqui. Adicione APENAS em `RegrasDesenvolvimentoFrotiX.md`

---

## ⚠️ REGRAS CRÍTICAS (RESUMO RÁPIDO)

### 🗄️ Banco de Dados
- **SEMPRE** consultar `FrotiX.sql` ANTES de codificar operações com banco
- Nunca assumir nome de coluna "de cabeça"
- Verificar tipos de dados, nullable, FKs
- Ao alterar: confrontar contra FrotiX.sql → atualizar FrotiX.sql → gerar script.sql

### 🔒 Try-Catch (OBRIGATÓRIO)

**C#:**
```csharp
public IActionResult MinhaAction()
{
    try
    {
        // código
    }
    catch (Exception error)
    {
        Alerta.TratamentoErroComLinha("MeuController.cs", "MinhaAction", error);
        return Json(new { success = false, message = error.Message });
    }
}
```

**JavaScript:**
```javascript
function minhaFuncao() {
  try {
    // código
  } catch (erro) {
    Alerta.TratamentoErroComLinha("arquivo.js", "minhaFuncao", erro);
  }
}
```

### 🎨 UI/UX (NUNCA VIOLAR)

| ❌ PROIBIDO | ✅ OBRIGATÓRIO |
|------------|---------------|
| `alert()` | `Alerta.Sucesso()`, `Alerta.Erro()`, etc. |
| `fa-solid`, `fa-regular` | `fa-duotone` (cores: #ff6b35, #6c757d) |
| Spinner Bootstrap | `FtxSpin.show()`, `FtxSpin.hide()` |
| `data-bs-toggle="tooltip"` | `data-ejtip="..."` (Syncfusion) |

### 📝 Documentação
- **SEMPRE** atualizar documentação em `Documentacao/` antes de commitar
- Formato: Prosa técnica + snippets + explicação linha-a-linha

### 🔄 Git
- Branch preferencial: `main`
- **Push SEMPRE para `main`** (nunca outras branches sem autorização)
- **Commit e push automáticos IMEDIATOS** após criar/alterar código importante
- Tipos de commit: `feat:`, `fix:`, `refactor:`, `docs:`, `style:`, `chore:`

---

## 🎯 CHECKLIST DE INÍCIO DE SESSÃO

Antes de responder ao usuário, confirme mentalmente:

- [ ] Li `RegrasDesenvolvimentoFrotiX.md` completamente?
- [ ] Se envolver banco: li `FrotiX.sql`?
- [ ] Entendi as regras críticas (try-catch, alertas, ícones)?
- [ ] Sei como alterar banco (confrontar → atualizar → script)?
- [ ] Sei que devo atualizar documentação antes de commitar?
- [ ] Exibi a mensagem de confirmação visual?

---

## 📚 REFERÊNCIA RÁPIDA DE ARQUIVOS

| Arquivo | Descrição |
|---------|-----------|
| `RegrasDesenvolvimentoFrotiX.md` | ⭐ REGRAS CONSOLIDADAS (fonte oficial) |
| `FrotiX.sql` | Estrutura do banco de dados SQL Server |
| `wwwroot/js/alerta.js` | Sistema de alertas SweetAlert FrotiX |
| `wwwroot/js/frotix.js` | JS global (FtxSpin, utilitários) |
| `wwwroot/css/frotix.css` | CSS global do sistema |

---

## 🧪 INSTRUÇÕES PARA TESTE DE AUTO-LOAD

Para verificar se o auto-load está funcionando:

1. Inicie uma nova conversa/chat com Gemini
2. Pergunte: "Você leu o arquivo instructions.md?"
3. Pergunte: "Quais são as regras críticas do projeto FrotiX?"
4. Se responder corretamente, auto-load está funcionando ✅
5. Se não souber, auto-load NÃO está funcionando ❌

---

## 📋 FALLBACK: MENSAGEM INICIAL MANUAL

**Se o teste acima indicar que auto-load NÃO funciona**, copie e cole esta mensagem no início de cada chat:

```
👋 Gemini, leia o seguinte antes de começar:

ARQUIVOS OBRIGATÓRIOS:
1. Leia RegrasDesenvolvimentoFrotiX.md antes de qualquer coisa
2. Se mexer em banco: leia FrotiX.sql

REGRA CRÍTICA - BANCO:
Se alterar banco → confrontar contra FrotiX.sql → atualizar FrotiX.sql → gerar script.sql

REGRAS OBRIGATÓRIAS:
• Try-catch em TODAS funções
• Alerta.* (NUNCA alert())
• fa-duotone (NUNCA fa-solid)
• FtxSpin.show() (NUNCA spinner Bootstrap)
• data-ejtip (NUNCA Bootstrap tooltips)
• Atualizar Documentacao/ antes de commitar

Confirme que entendeu exibindo a mensagem de confirmação visual.
```

---

✅ **Genie AI/Gemini configurado para FrotiX. Aguardando confirmação visual.**
