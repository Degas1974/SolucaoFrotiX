# Documentação: Motorista - Upsert (Criação e Edição)

> **Última Atualização**: 13/01/2026
> **Versão Atual**: 1.1

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice

1. [Visão Geral](#visão-geral)
2. [Arquitetura](#arquitetura)
3. [Estrutura da Interface](#estrutura-da-interface)
4. [Lógica de Frontend (JavaScript)](#lógica-de-frontend-javascript)
5. [Endpoints API](#endpoints-api)
6. [Troubleshooting](#troubleshooting)

---

## Visão Geral

A página **Upsert de Motorista** (`Pages/Motorista/Upsert.cshtml`) permite cadastrar novos motoristas ou editar os existentes. O formulário é dividido em seções lógicas (Dados Pessoais, Habilitação, Vínculo, Foto) para facilitar o preenchimento.

### Características Principais

- ✅ **Formulário Segmentado**: Organização clara em cards.
- ✅ **Upload de Foto**: Pré-visualização imediata da imagem selecionada.
- ✅ **Máscaras de Input**: Formatação automática para CPF e Celular.
- ✅ **Validação**: Campos obrigatórios destacados.

---

## Arquitetura

### Estrutura de Arquivos

```
FrotiX.Site/
├── Pages/
│   └── Motorista/
│       └── Upsert.cshtml            # View do Formulário
│
├── Controllers/
│   └── MotoristaController.cs       # Controller (Submit)
│
├── wwwroot/
│   ├── js/
│   │   └── cadastros/
│   │       └── motorista_upsert.js  # Lógica de validação e máscaras
```

### Tecnologias Utilizadas

| Tecnologia                   | Uso                      |
| ---------------------------- | ------------------------ |
| **ASP.NET Core Razor Pages** | Renderização             |
| **jQuery Mask Plugin**       | Máscaras de CPF/Telefone |
| **Bootstrap 5**              | Layout                   |

---

## Estrutura da Interface

### Seção Dados Pessoais

Campos para identificação básica.

```html
<div class="ftx-section">
  <div class="row">
    <div class="col-md-5">
      <label>Nome</label>
      <input asp-for="MotoristaObj.Motorista.Nome" />
    </div>
    <div class="col-md-3">
      <label>CPF</label>
      <input
        id="txtCPF"
        asp-for="MotoristaObj.Motorista.CPF"
        placeholder="000.000.000-00"
      />
    </div>
  </div>
</div>
```

### Seção Foto

Permite selecionar um arquivo de imagem. Se for edição e já existir foto, ela é exibida.

```html
<div class="col-md-8">
  <input type="file" id="fotoUpload" name="FotoUpload" accept="image/*" />
</div>
<div class="col-md-4">
  <img id="imgPreview" src="..." />
</div>
```

---

## Lógica de Frontend (JavaScript)

O arquivo `motorista_upsert.js` configura as máscaras e o preview.

### Máscaras

```javascript
$(document).ready(function () {
  $("#txtCPF").mask("000.000.000-00", { reverse: true });
  $("#txtCelular01").mask("(00) 00000-0000");
  $("#txtCelular02").mask("(00) 00000-0000");
});
```

### Preview de Imagem

Ao selecionar um arquivo, o navegador lê o conteúdo e exibe na tag `img`.

```javascript
$("#fotoUpload").change(function () {
  if (this.files && this.files[0]) {
    var reader = new FileReader();
    reader.onload = function (e) {
      $("#imgPreview").attr("src", e.target.result).show();
      $("#txtSemFoto").hide();
    };
    reader.readAsDataURL(this.files[0]);
  }
});
```

### Validação no Submit

A função `validarFormulario` verifica se campos críticos (Nome, CPF, CNH) estão preenchidos.

```javascript
function validarFormulario() {
  if ($("#txtNome").val() == "") {
    Alerta.Erro("Campo Obrigatório", "Informe o Nome.");
    return false;
  }
  // ... outras validações
  return true;
}
```

---

## Endpoints API

### POST `/Motorista/Upsert` (Handler)

Processa o formulário. O `IFormFile FotoUpload` é convertido em array de bytes e salvo no banco.

---

## Troubleshooting

### Máscara não funciona

**Causa**: jQuery Mask Plugin não carregado ou conflito de scripts.
**Solução**: Verifique se o script `jquery.mask.min.js` está incluído na seção `ScriptsBlock`.

### Imagem não salva

**Causa**: O formulário não tem `enctype="multipart/form-data"`.
**Solução**: Verifique a tag form: `<form method="post" enctype="multipart/form-data">`.

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [16/01/2026 17:15] - Auditoria Global: Campos Obrigatórios (.label-required)

**Descrição**: Adicionado asterisco vermelho em labels de campos mandatórios identificados via lógica de validação (Back/Front).

---

## [13/01/2026 15:30] - Padronização: Substituição de btn-ftx-fechar por btn-vinho

**Descrição**: Substituída classe `btn-ftx-fechar` por `btn-vinho` em botões de cancelar/fechar operação.

**Problema Identificado**:

- Classe `btn-ftx-fechar` não tinha `background-color` definido no estado `:active`
- Botões ficavam BRANCOS ao serem pressionados (em vez de manter cor rosada/vinho)
- Comportamento visual inconsistente com padrão FrotiX

**Solução Implementada**:

- Todos os botões cancelar/fechar padronizados para usar classe `.btn-vinho`
- Classe `.btn-vinho` já possui `background-color: #4a1f24` no estado `:active`
- Garantia de cor rosada/vinho ao pressionar botão

**Arquivos Afetados**:

- Pages/Motorista/Upsert.cshtml - Substituição de `btn-ftx-fechar` por `btn-vinho` em botão de modal

**Impacto**:

- ✅ Botão mantém cor rosada/vinho ao ser pressionado
- ✅ Alinhamento com padrão visual FrotiX
- ✅ Consistência em todo o sistema

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 1.1

---

## [06/01/2026] - Criação da Documentação

**Descrição**:
Documentação inicial do formulário de Upsert de Motoristas.

**Status**: ✅ **Documentado**

**Responsável**: Claude (AI Assistant)
**Versão**: 1.0


---

# PARTE 2: LOG DE MODIFICAÃ‡Ã•ES/CORREÃ‡Ã•ES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [19/01/2026] - AtualizaÃ§Ã£o: ImplementaÃ§Ã£o de MÃ©todos com Tracking Seletivo

**DescriÃ§Ã£o**: MigraÃ§Ã£o de chamadas .AsTracking() para novos mÃ©todos GetWithTracking() e GetFirstOrDefaultWithTracking() como parte da otimizaÃ§Ã£o de performance do Entity Framework Core.

**Arquivos Afetados**:
- Este arquivo (uso dos novos mÃ©todos do repositÃ³rio)
- Repository/IRepository/IRepository.cs (definiÃ§Ã£o dos novos mÃ©todos)
- Repository/Repository.cs (implementaÃ§Ã£o)
- RegrasDesenvolvimentoFrotiX.md (seÃ§Ã£o 4.2 - nova regra permanente)

**MudanÃ§as**:
- âŒ **ANTES**: _unitOfWork.Entity.AsTracking().Get(id) ou _unitOfWork.Entity.AsTracking().GetFirstOrDefault(filter)
- âœ… **AGORA**: _unitOfWork.Entity.GetWithTracking(id) ou _unitOfWork.Entity.GetFirstOrDefaultWithTracking(filter)

**Motivo**: 
- OtimizaÃ§Ã£o de memÃ³ria e performance
- Tracking seletivo (apenas quando necessÃ¡rio para Update/Delete)
- PadrÃ£o mais limpo e explÃ­cito
- Conformidade com nova regra permanente (RegrasDesenvolvimentoFrotiX.md seÃ§Ã£o 4.2)

**Impacto**: 
- Melhoria de performance em operaÃ§Ãµes de leitura (usa AsNoTracking por padrÃ£o)
- Tracking correto em operaÃ§Ãµes de escrita (Update/Delete)
- Zero impacto funcional (comportamento mantido)

**Status**: âœ… **ConcluÃ­do**

**ResponsÃ¡vel**: Sistema (AtualizaÃ§Ã£o AutomÃ¡tica)

**VersÃ£o**: Incremento de patch
