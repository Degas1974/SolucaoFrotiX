# Documentação: \_PageHeader.cshtml — Header Principal do App

> **Última Atualização**: 21/01/2026  
> **Versão Atual**: 1.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

Partial responsável pelo header principal do FrotiX, exibindo atalhos de navegação, sino de alertas e o menu do usuário. Nesta atualização, passou a exibir o usuário logado no formato `(ponto.) Nome` com ícone duotone padrão e atualização via API.

## Estrutura de Arquivo

```
Pages/Shared/_PageHeader.cshtml
```

## Elementos Principais

- **Atalhos rápidos**: Home, Agenda, Requisições, Viagens, Manutenção, Abastecimento, Contratos, Multas, Cadastros e TaxiLeg.
- **Alertas**: partial `_AlertasSino`.
- **Usuário logado**: rótulo com ícone duotone e texto formatado.
- **Menu do usuário**: dropdown com `_DropdownMenu`.

## Atualização do usuário logado

A label é preenchida via chamada a `/api/Login/RecuperaUsuarioAtual` e formatada para:

```
(ponto.) Nome Completo
```

Trecho aplicado:

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

- **CSS**: `wwwroot/css/frotix.css` (classe `.ftx-header-user`)
- **API**: `GET /api/Login/RecuperaUsuarioAtual`
- **Alerts**: `Alerta.TratamentoErroComLinha`

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [21/01/2026 14:20] - Exibição do usuário logado no header

**Descrição**: Adicionado rótulo do usuário logado com ícone duotone e chamada ao endpoint para montar o formato `(ponto.) Nome`.

**Arquivos Afetados**:

- Pages/Shared/\_PageHeader.cshtml
- wwwroot/css/frotix.css

**Status**: ✅ **Concluído**
