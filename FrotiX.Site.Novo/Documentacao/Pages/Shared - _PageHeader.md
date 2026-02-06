# Documentação: Shared - \_PageHeader.cshtml

> **Última Atualização**: 21/01/2026  
> **Versão Atual**: 1.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

Partial responsável pelo header principal do FrotiX, exibindo atalhos de navegação, alertas e o menu do usuário. A implementação atual exibe o usuário logado no formato `(ponto.) Nome` com ícone duotone padrão e atualização via API.

## Estrutura de Arquivo

```
Pages/Shared/_PageHeader.cshtml
```

## Trechos Relevantes

```html
<div class="ftx-header-user d-flex align-items-center ms-3 me-2">
  <i class="fa-duotone fa-user"></i>
  <span
    id="ftxUserLabel"
    class="ftx-header-user-label"
    data-default="@(Settings.Theme.User)"
    >@(Settings.Theme.User)</span
  >
</div>
```

```javascript
function formatarLabelUsuario(ponto, nome) {
  try {
    const pontoLimpo = (ponto || "").trim();
    const nomeLimpo = (nome || "").trim();
    if (!pontoLimpo && !nomeLimpo) return "";
    if (!pontoLimpo) return nomeLimpo;
    return `(${pontoLimpo}.) ${nomeLimpo}`.trim();
  } catch (error) {
    Alerta.TratamentoErroComLinha(
      "_PageHeader.cshtml",
      "formatarLabelUsuario",
      error,
    );
    return "";
  }
}
```

## Dependências

- **CSS**: `wwwroot/css/frotix.css`
- **API**: `GET /api/Login/RecuperaUsuarioAtual`

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [21/01/2026 14:20] - Exibição do usuário logado no header

**Descrição**: Adicionada exibição do usuário com formato `(ponto.) Nome` e ícone duotone padrão.

**Arquivos Afetados**:

- Pages/Shared/\_PageHeader.cshtml
- wwwroot/css/frotix.css

**Status**: ✅ **Concluído**
