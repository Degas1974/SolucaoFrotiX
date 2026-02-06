# Documentação: AppToast.cs

> **Última Atualização**: 23/01/2026 11:52  
> **Versão**: 1.0  
> **Documentação Intra-Código**: ✅ Completa (Cards adicionados)

---

# PARTE 1: VISÃO GERAL

## Sistema de Notificações Toast do FrotiX

O **AppToast** é um serviço estático que gerencia notificações toast (mensagens temporárias não-intrusivas) no sistema FrotiX. Diferente de alertas modais que bloqueiam a interface, os toasts aparecem no canto da tela e desaparecem automaticamente.

### Características Principais

- **Persistência entre Requisições**: Utiliza TempData para funcionar com RedirectToAction
- **Cores Semânticas**: Verde (sucesso), Vermelho (erro), Amarelo (aviso), Azul (info)
- **Escapamento Automático**: Previne XSS escapando caracteres especiais
- **Duração Configurável**: Controle sobre quanto tempo o toast permanece visível

### Métodos Disponíveis

```csharp
// Método principal
AppToast.show("Verde", "Operação realizada!", 2000);

// Atalhos por tipo
AppToast.ShowSuccess("Salvo com sucesso!");
AppToast.ShowError("Erro ao processar");
AppToast.ShowWarning("Atenção necessária");
AppToast.ShowInfo("Informação importante");
```

### Integração com Frontend

O serviço gera scripts JavaScript armazenados em TempData que são executados automaticamente no layout:

```javascript
AppToast.show("Verde", "Mensagem aqui", 2000);
```

---

# PARTE 2: LOG DE MODIFICAÇÕES

## [23/01/2026 11:52] - Documentação Intra-Código Completa

**Descrição**: Criação da documentação completa com Cards padrão RegrasDesenvolvimentoFrotiX.md  
**Arquivos Afetados**:

- AppToast.cs (adição de Cards e cabeçalho)
- AppToast.md (este arquivo - criação inicial)
  **Status**: ✅ Concluído

---

# PARTE 3: EXEMPLOS DE USO

## Em Controllers

```csharp
public IActionResult SalvarDados(Modelo model)
{
    try
    {
        _repository.Save(model);
        AppToast.ShowSuccess("Dados salvos com sucesso!");
        return RedirectToAction("Index"); // Toast será exibido após redirect
    }
    catch (Exception ex)
    {
        AppToast.ShowError($"Erro: {ex.Message}");
        return View(model);
    }
}
```

## Em Pages (Razor Pages)

```csharp
public IActionResult OnPost()
{
    if (!ModelState.IsValid)
    {
        AppToast.ShowWarning("Verifique os campos obrigatórios");
        return Page();
    }

    // Processar...
    AppToast.ShowSuccess("Processado!");
    return RedirectToPage("./Index");
}
```

---
