# Documentação: usuario_001.js - Gestão de Usuários (Legacy)

> **Última Atualização**: 12/01/2026
> **Versão Atual**: 1.2
> **Status**: Legacy (coexiste com usuario-index.js)

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice
1. [Visão Geral](#visão-geral)
2. [Arquitetura](#arquitetura)
3. [Estrutura de Arquivos](#estrutura-de-arquivos)
4. [Lógica de Negócio](#lógica-de-negócio)
5. [Interconexões](#interconexões)
6. [Endpoints API](#endpoints-api)
7. [Frontend](#frontend)
8. [Troubleshooting](#troubleshooting)

---

## Visão Geral

**Descrição:**
Arquivo JavaScript legacy para gerenciamento de usuários no sistema FrotiX. Contém funcionalidades básicas de listagem, exclusão e toggle de status (Ativo/Inativo e Detentor de Carga Patrimonial).

### Características Principais
- ✅ **DataTable de Usuários**: Exibe lista de usuários com paginação
- ✅ **Toggle de Status**: Permite ativar/inativar usuários
- ✅ **Toggle Carga Patrimonial**: Define se usuário é detentor de carga
- ✅ **Exclusão de Usuários**: Com confirmação via SweetAlert
- ✅ **Modais**: Gerenciamento de senha e controle de acesso

### Objetivo
Fornecer interface interativa para gerenciar usuários do sistema, permitindo ao administrador listar, editar, excluir e alterar status dos usuários de forma rápida e visual.

---

## Arquitetura

### Tecnologias Utilizadas
| Tecnologia | Versão | Uso |
|------------|--------|-----|
| jQuery | 3.x | Manipulação DOM e AJAX |
| DataTables | 1.10.25 | Tabela interativa com paginação |
| SweetAlert2 | - | Confirmações e alertas |
| Bootstrap | 5.x | Modais e componentes UI |

### Padrões de Design
- Event Delegation (delegação de eventos para elementos dinâmicos)
- AJAX assíncrono para comunicação com API
- Toast notifications para feedback ao usuário

---

## Estrutura de Arquivos

### Arquivo Principal
```
wwwroot/js/cadastros/usuario_001.js
```

### Arquivos Relacionados
- `Areas/Authorization/Pages/Usuarios.cshtml` - Página que usa este script
- `usuario-index.js` - Versão mais recente (ambos podem coexistir)
- `Controllers/UsuarioController.cs` - API backend
- `wwwroot/js/alerta.js` - Sistema de alertas
- `wwwroot/js/global-toast.js` - Sistema de toasts

---

## Lógica de Negócio

### Função Principal: `loadList()`
**Localização**: Linha 198

**Propósito**: Inicializa a DataTable de usuários com dados da API

**Código**:
```javascript
function loadList() {
    try {
        dataTable = $("#tblUsuario").DataTable({
            columnDefs: [ ... ],
            responsive: true,
            ajax: {
                url: "/api/usuario",
                type: "GET",
                datatype: "json",
            },
            columns: [
                { data: "nomeCompleto" },
                { data: "ponto" },
                { data: "detentorCargaPatrimonial", render: ... },
                { data: "status", render: ... },
                { data: "usuarioId", render: ... }
            ],
            language: {
                url: "https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json"
            }
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha("usuario_<num>.js", "loadList", error);
    }
}
```

**Fluxo de Execução**:
1. Chama API `/api/usuario` para buscar lista de usuários
2. Renderiza cada coluna com formatação específica
3. Coluna Status e Carga Patrimonial rendem badges clicáveis
4. Coluna Ação renderiza botões de editar, excluir, senha e acesso

---

### Função: Toggle Status Usuário
**Localização**: Linha 84-123

**Propósito**: Alterna status Ativo/Inativo do usuário

**Código**:
```javascript
$(document).on("click", ".updateStatusUsuario", function () {
    try {
        var url = $(this).data("url");
        var currentElement = $(this);

        $.get(url, function (data) {
            if (data.success) {
                AppToast.show('Verde', "Status alterado com sucesso!");
                var text = "Ativo";
                var iconHtml = '<i class="fa-solid fa-circle-check me-1"></i>';

                if (data.type == 1) {
                    text = "Inativo";
                    iconHtml = '<i class="fa-solid fa-circle-xmark me-1"></i>';
                    currentElement.removeClass("btn-verde").addClass("fundo-cinza");
                } else {
                    currentElement.removeClass("fundo-cinza").addClass("btn-verde");
                }

                currentElement.html(iconHtml + text); // Mantém ícone + texto
            }
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha("usuario_<num>.js", "callback@$.on#2", error);
    }
});
```

**Fluxo de Execução**:
1. Clique no badge de status
2. Chama API com URL do data-attribute
3. Recebe resposta com novo status (type: 1 = inativo, 0 = ativo)
4. Atualiza classes CSS (btn-verde ↔ fundo-cinza)
5. **Atualiza HTML do badge** mantendo ícone e texto
6. Exibe toast de sucesso

**Casos Especiais**:
- **Correção aplicada em 12/01/2026**: Anteriormente usava `.text()` que removia o ícone. Agora usa `.html()` para manter ícone + texto.

---

### Função: Toggle Carga Patrimonial
**Localização**: Linha 125-164

**Propósito**: Alterna se usuário é Detentor de Carga Patrimonial

**Código**:
```javascript
$(document).on("click", ".updateCargaPatrimonial", function () {
    try {
        var url = $(this).data("url");
        var currentElement = $(this);

        $.get(url, function (data) {
            if (data.success) {
                AppToast.show('Verde', "Carga Patrimonial alterada com sucesso!");
                var text = "Sim";
                var iconHtml = '<i class="fa-duotone fa-badge-check me-1"></i>';

                if (data.type == 1) {
                    text = "Não";
                    iconHtml = '<i class="fa-duotone fa-circle-xmark me-1"></i>';
                    currentElement.removeClass("btn-verde").addClass("fundo-cinza");
                } else {
                    currentElement.removeClass("fundo-cinza").addClass("btn-verde");
                }

                currentElement.html(iconHtml + text);
            }
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha("usuario_<num>.js", "callback@$.on#2", error);
    }
});
```

**Fluxo**: Idêntico ao toggle de status, mas para Carga Patrimonial

---

### Função: Exclusão de Usuário
**Localização**: Linha 8-82

**Propósito**: Exclui usuário após confirmação

**Código**:
```javascript
$(document).on("click", ".btn-delete", function () {
    var Id = $(this).data("id");

    Alerta.Confirmar(
        "Você tem certeza que deseja apagar este Usuário?",
        "Não será possível recuperar os dados eliminados!",
        "Excluir",
        "Cancelar"
    ).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: "/api/Usuario/Delete",
                type: "POST",
                data: JSON.stringify({ Id: Id }),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.success) {
                        AppToast.show('Verde', data.message);
                        dataTable.ajax.reload();
                    } else {
                        AppToast.show('Vermelho', data.message);
                    }
                }
            });
        }
    });
});
```

**Fluxo de Execução**:
1. Clique no botão de exclusão
2. Exibe confirmação SweetAlert
3. Se confirmado, chama API `/api/Usuario/Delete`
4. Recarrega DataTable após sucesso
5. Exibe toast verde (sucesso) ou vermelho (erro)

---

## Interconexões

### Quem Chama Este Arquivo
- `Areas/Authorization/Pages/Usuarios.cshtml` → Referência via `<script src="~/js/usuarios.js"></script>` (provavelmente aponta para este arquivo)

### O Que Este Arquivo Chama
- `/api/usuario` (GET) → Lista todos os usuários
- `/api/Usuario/updateStatusUsuario?Id=...` (GET) → Alterna status
- `/api/Usuario/updateCargaPatrimonial?Id=...` (GET) → Alterna carga patrimonial
- `/api/Usuario/Delete` (POST) → Exclui usuário
- `Alerta.Confirmar()` → Sistema de confirmação
- `Alerta.TratamentoErroComLinha()` → Tratamento de erros
- `AppToast.show()` → Notificações toast

### Fluxo de Dados
```
Usuário → Clique no Badge Status → AJAX GET /api/Usuario/updateStatusUsuario
                                    ↓
                            Resposta { success, type }
                                    ↓
                            Atualiza Badge HTML (ícone + texto)
                                    ↓
                            Exibe Toast de Sucesso
```

---

## Endpoints API

### GET `/api/usuario`
**Descrição**: Lista todos os usuários do sistema

**Response**:
```json
[
  {
    "usuarioId": "guid",
    "nomeCompleto": "João Silva",
    "ponto": "12345",
    "detentorCargaPatrimonial": true,
    "status": true
  }
]
```

**Uso no Código**: Linha 232

---

### GET `/api/Usuario/updateStatusUsuario?Id={guid}`
**Descrição**: Alterna status Ativo/Inativo do usuário

**Parâmetros**:
- `Id` (query): GUID do usuário

**Response**:
```json
{
  "success": true,
  "type": 0 // 0 = ativo, 1 = inativo
}
```

**Uso no Código**: Linha 90-117

---

### GET `/api/Usuario/updateCargaPatrimonial?Id={guid}`
**Descrição**: Alterna se usuário é detentor de carga patrimonial

**Parâmetros**:
- `Id` (query): GUID do usuário

**Response**:
```json
{
  "success": true,
  "type": 0 // 0 = Sim, 1 = Não
}
```

**Uso no Código**: Linha 131-158

---

### POST `/api/Usuario/Delete`
**Descrição**: Exclui um usuário do sistema

**Request Body**:
```json
{
  "Id": "guid-do-usuario"
}
```

**Response**:
```json
{
  "success": true,
  "message": "Usuário excluído com sucesso"
}
```

**Uso no Código**: Linha 24-49

---

## Frontend

### Estrutura HTML Esperada
```html
<table id="tblUsuario" class="table">
  <thead>
    <tr>
      <th>Nome</th>
      <th>Ponto</th>
      <th>Carga Patrimonial</th>
      <th>Status</th>
      <th>Ação</th>
    </tr>
  </thead>
</table>

<!-- Modal Senha -->
<div id="modalSenha" class="modal">...</div>

<!-- Modal Controle de Acesso -->
<div id="modalControleAcesso" class="modal">...</div>
```

### JavaScript Principal
- **Event Delegation**: Eventos delegados ao `$(document)` para capturar elementos dinâmicos da DataTable
- **Bootstrap Modals**: Uso de `bootstrap.Modal` para abrir modais
- **Try-Catch**: Todos os blocos têm tratamento de erro via `Alerta.TratamentoErroComLinha`

### Eventos
- `click` em `.btn-delete` → Exclui usuário
- `click` em `.updateStatusUsuario` → Alterna status
- `click` em `.updateCargaPatrimonial` → Alterna carga patrimonial
- `click` em `.btn-modal-senha` → Abre modal de alteração de senha
- `click` em `.btn-modal-acesso` → Abre modal de controle de acesso

---

## Troubleshooting

### Problema: Badge perde ícone ao clicar
**Sintoma**: Ao clicar no badge Ativo/Inativo, o ícone desaparece

**Causa**: Uso de `.text()` que remove HTML interno

**Solução**: Usar `.html()` com ícone + texto completo
```javascript
// ❌ Errado (versão antiga)
currentElement.text("Ativo");

// ✅ Correto (versão atual)
currentElement.html('<i class="fa-solid fa-circle-check me-1"></i>Ativo');
```

**Status**: ✅ Corrigido em 12/01/2026

---

### Problema: DataTable não carrega
**Sintoma**: Tabela vazia ou erro no console

**Diagnóstico**:
1. Verificar se endpoint `/api/usuario` está respondendo
2. Verificar console do navegador para erros AJAX
3. Verificar se jQuery e DataTables estão carregados

**Solução**: Verificar ordem de carregamento dos scripts e conectividade com API

---

### Problema: Modal não abre
**Sintoma**: Clique nos botões de Senha/Acesso não abre modal

**Causa**: Modal não encontrado no DOM ou Bootstrap não inicializado

**Solução**: Verificar se `id="modalSenha"` e `id="modalControleAcesso"` existem no HTML

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [12/01/2026 09:30] - Melhoria UX: Botão de Exclusão Desabilitado Preventivamente

**Descrição**: Implementada renderização condicional de botões de exclusão, desabilitando-os preventivamente quando o usuário não pode ser excluído

**Problema Anterior**:
- Todos os botões de exclusão eram renderizados como habilitados
- Usuário só descobria que não podia excluir APÓS clicar e enviar requisição ao backend
- Experiência frustrante: clique → tentativa → erro

**Solução Implementada**:
- Backend agora retorna propriedade `podeExcluir` para cada usuário no endpoint GET
- Coluna de Ações verifica `row.podeExcluir` antes de renderizar botão
- Se `podeExcluir === false`: renderiza `<button disabled>` com tooltip explicativo
- Tooltip Bootstrap: "Usuário não pode ser excluído pois está em uso" (classe `tooltip-ftx-azul`)
- `drawCallback` adicionado ao DataTable para inicializar tooltips após renderização

**Código Adicionado**:
```javascript
// Linhas 303-318: Renderização condicional
if (row.podeExcluir) {
    btnExcluir = `<a class="btn-delete btn btn-vinho ...">...</a>`;
} else {
    btnExcluir = `<button disabled data-bs-toggle="tooltip"
        title="Usuário não pode ser excluído pois está em uso">...</button>`;
}

// Linhas 350-356: drawCallback para inicializar tooltips
drawCallback: function() {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
}
```

**Melhorias Adicionais**:
- Ícone do botão Senha corrigido: `fa-camera-retro` → `fa-key` (mais apropriado)
- Função render agora recebe parâmetro `row` completo para acessar `podeExcluir`

**Arquivos Afetados**:
- `wwwroot/js/cadastros/usuario_001.js` (linhas 293-342, 350-356)
- Integração com `Controllers/UsuarioController.cs` (endpoint GET retorna `podeExcluir`)

**Impacto**:
- ✅ **UX melhorada**: Feedback visual imediato sobre possibilidade de exclusão
- ✅ **Menos requisições**: Evita chamadas AJAX desnecessárias para exclusões que falhariam
- ✅ **Tooltip informativo**: Usuário entende o motivo da desabilitação antes de tentar

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 1.2

---

## [12/01/2026 15:30] - Correção do Toggle Ativo/Inativo - Manutenção de Ícones

**Descrição**: Corrigido bug onde o ícone do badge desaparecia ao clicar para alternar status

**Problema**:
- Ao clicar no badge Ativo/Inativo ou Sim/Não (Carga Patrimonial), o ícone era removido
- Apenas o texto "Ativo"/"Inativo" ou "Sim"/"Não" permanecia

**Causa**:
- Uso de `.text()` que substitui TODO o conteúdo HTML por texto puro
- O HTML original tinha `<i class="..."></i>` + texto, mas `.text()` removia o `<i>`

**Solução**:
- Substituído `.text()` por `.html()`
- Construir string HTML completa: `iconHtml + text`
- Para Status: `fa-solid fa-circle-check` (ativo) e `fa-solid fa-circle-xmark` (inativo)
- Para Carga: `fa-duotone fa-badge-check` (sim) e `fa-duotone fa-circle-xmark` (não)

**Código Alterado**:
```javascript
// Linha 106 (Status)
currentElement.html(iconHtml + text);

// Linha 147 (Carga Patrimonial)
currentElement.html(iconHtml + text);
```

**Arquivos Afetados**:
- `wwwroot/js/cadastros/usuario_001.js` (linhas 84-164)

**Impacto**: Agora os badges mantêm ícones + texto após toggle

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 1.1

---

## Histórico de Versões

| Versão | Data | Descrição |
|--------|------|-----------|
| 1.0 | Anterior a 2026 | Versão inicial (legacy) |
| 1.1 | 12/01/2026 | Correção toggle status mantendo ícones |
| 1.2 | 12/01/2026 | Botão exclusão desabilitado preventivamente + tooltips |

---

## Referências

- [usuario-index.js](./usuario-index.md) - Versão mais recente (coexiste com este)
- [Alerta.js](../alerta.js.md) - Sistema de alertas
- [Global Toast](../global-toast.js.md) - Sistema de toasts

---

**Última atualização**: 12/01/2026
**Autor**: Sistema FrotiX
**Versão**: 1.2
