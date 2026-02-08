# 🎯 FrotiX Development Rules - Continue AI

## 👋 Welcome Message

Bem-vindo à sessão de desenvolvimento FrotiX com Continue!

**BEFORE ANY CODE GENERATION:**

✅ Read `../../RegrasDesenvolvimentoFrotiX.md` completely
✅ If working with database: Read `../../FrotiX.sql` (complete SQL Server structure)

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

## 🗄️ CRITICAL RULE: Database Changes

If you make any addition, modification or deletion in the database:

1. **Compare** your code against `FrotiX.sql` to check for incompatibilities
2. **If compatible**, update `FrotiX.sql` with your changes
3. **Generate** a separate `script.sql` file to run on both production and development databases

---

## ⚠️ Mandatory Patterns

### Try-Catch (REQUIRED)
```csharp
public IActionResult MyAction()
{
    try
    {
        // code
    }
    catch (Exception error)
    {
        Alerta.TratamentoErroComLinha("MyController.cs", "MyAction", error);
        return Json(new { success = false, message = error.Message });
    }
}
```

### Alerts (NEVER use alert())
```javascript
Alerta.Sucesso(titulo, msg)
Alerta.Erro(titulo, msg)
Alerta.Warning(titulo, msg)
Alerta.Info(titulo, msg)
Alerta.Confirmar(titulo, msg, btnSim, btnNao).then(ok => { ... })
```

### Icons (ALWAYS fa-duotone)
```html
<i class="fa-duotone fa-car" style="--fa-primary-color:#ff6b35; --fa-secondary-color:#6c757d;"></i>
```

### Loading (ALWAYS FtxSpin)
```javascript
FtxSpin.show("Loading...");
FtxSpin.hide();
```

### Tooltips (ALWAYS Syncfusion)
```html
<button data-ejtip="Tooltip text"></button>
```

---

## 📝 Documentation

- ALWAYS update documentation in `Documentacao/` before committing
- Format: Technical prose + code snippets + line-by-line explanation

---

## 🔄 Git

- Preferred branch: `main`
- ALWAYS push to `main`
- Immediate automatic commit and push after creating/modifying important code

---

✅ **Continue configured for FrotiX. Read `RegrasDesenvolvimentoFrotiX.md` to continue.**
