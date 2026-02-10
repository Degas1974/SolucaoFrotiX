# Documentação: usuario-index.js - Gestão de Usuários (Atual)

> **Última Atualização**: 12/01/2026
> **Versão Atual**: 2.3
> **Status**: Atual (versão moderna do sistema)

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
Arquivo JavaScript moderno para gerenciamento completo de usuários no sistema FrotiX. Versão atualizada que substitui gradualmente o `usuario_001.js`, com código refatorado, melhor organização e uso dos padrões FrotiX.

### Características Principais
- ✅ **DataTables Avançadas**: Duas tabelas (Usuários e Recursos) com renderização customizada
- ✅ **Toggle de Status**: Ativa/inativa usuários com feedback visual
- ✅ **Toggle Carga Patrimonial**: Define detentores de carga patrimonial
- ✅ **Exclusão Segura**: Confirmação com **Alerta.Confirmar() padrão FrotiX**
- ✅ **Gestão de Recursos**: Modal para atribuir/remover acesso a recursos do sistema
- ✅ **Visualização de Fotos**: Modal para exibir fotos dos usuários
- ✅ **Avatares Dinâmicos**: Exibe foto ou ícone de usuário na lista

### Objetivo
Fornecer interface moderna e responsiva para gerenciamento completo de usuários, integrando funcionalidades de CRUD, controle de acesso, e gestão patrimonial em uma única tela.

---

## Arquitetura

### Tecnologias Utilizadas
| Tecnologia | Versão | Uso |
|------------|--------|-----|
| jQuery | 3.x | Manipulação DOM e AJAX |
| DataTables | 1.10.25+ | Tabelas interativas com paginação |
| Bootstrap | 5.x | Modais e componentes UI |
| FontAwesome | Duotone | Ícones customizados |

### Padrões de Design
- **Module Pattern**: Encapsulamento em IIFE (Immediately Invoked Function Expression)
- **Event Delegation**: Delegação de eventos para elementos dinâmicos
- **AJAX Assíncrono**: Comunicação com API via jQuery AJAX
- **Modal Management**: Controle de instâncias Bootstrap para modais

---

## Estrutura de Arquivos

### Arquivo Principal
```
wwwroot/js/cadastros/usuario-index.js
```

### Arquivos Relacionados
- `Areas/Authorization/Pages/Usuarios.cshtml` - Página que usa este script
- `Controllers/UsuarioController.cs` - API backend
- `wwwroot/js/alerta.js` - Sistema de alertas customizado
- `wwwroot/js/global-toast.js` - Sistema de toasts

---

## Lógica de Negócio

### Função: `inicializarDataTableUsuarios()`
**Localização**: Linha 140

**Propósito**: Inicializa DataTable principal de usuários com renderização customizada de colunas

**Código**:
```javascript
function inicializarDataTableUsuarios() {
    try {
        dataTableUsuarios = $('#tblUsuario').DataTable({
            order: [[0, 'asc']],
            autoWidth: false,
            columnDefs: [
                { targets: 0, className: "text-left", width: "40%" },
                { targets: 1, className: "text-center", width: "15%" },
                { targets: 2, className: "text-center", width: "15%" },
                { targets: 3, className: "text-center", width: "15%" },
                { targets: 4, className: "text-center ftx-actions", width: "15%", orderable: false }
            ],
            ajax: {
                url: "/api/Usuario/GetAll",
                type: "GET"
            },
            columns: [ /* ver código completo abaixo */ ]
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha("usuario-index.js", "inicializarDataTableUsuarios", error);
    }
}
```

**Colunas Renderizadas**:
1. **Nome com Avatar**: Foto do usuário (se houver) ou ícone padrão
2. **Ponto**: Código do ponto funcional do usuário
3. **Detentor Carga Patrimonial**: Badge clicável (Sim/Não)
4. **Status**: Badge clicável (Ativo/Inativo)
5. **Ações**: Botões de Editar, Recursos, Foto, Excluir

---

### Função: `confirmarExclusao()` (ATUALIZADA v2.0)
**Localização**: Linha 498

**Propósito**: Exclui usuário após confirmação usando padrão FrotiX

**Código (v2.0 - PADRÃO FROTIX)**:
```javascript
function confirmarExclusao(usuarioId, nome) {
    try {
        Alerta.Confirmar(
            "Confirmar Exclusão",
            `Deseja realmente excluir o usuário <strong>${nome}</strong>?<br><br>` +
            `<small style="color: #dc3545; font-size: 0.875rem;">⚠️ Esta ação não pode ser desfeita!</small><br><br>` +
            `<small style="color: #6c757d; font-size: 0.875rem;">O sistema verificará automaticamente se o usuário possui registros vinculados (Viagens, Manutenções, etc.).</small>`,
            "Sim, Excluir",
            "Cancelar"
        ).then((willDelete) => {
            if (willDelete) {
                excluirUsuario(usuarioId);
            }
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha("usuario-index.js", "confirmarExclusao", error);
    }
}
```

**Mudança Importante (v2.0)**:
- ✅ **ANTES**: Usava `Swal.fire()` direto (não seguia padrão FrotiX)
- ✅ **AGORA**: Usa `Alerta.Confirmar()` (padrão FrotiX customizado)
- ✅ **Texto Informativo**: Avisa que o sistema valida vínculos automaticamente

---

### Função: `excluirUsuario()`
**Localização**: Linha 525

**Propósito**: Executa requisição AJAX para excluir usuário

**Código**:
```javascript
function excluirUsuario(usuarioId) {
    try {
        $.ajax({
            url: "/api/Usuario/Delete",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({ Id: usuarioId }),
            success: function (response) {
                if (response.success) {
                    AppToast.show("Verde", response.message || "Usuário excluído!", 3000);
                    dataTableUsuarios.ajax.reload(null, false);
                } else {
                    // Mensagem de erro formatada em HTML (backend retorna com vínculos listados)
                    AppToast.show("Vermelho", response.message || "Erro ao excluir", 5000);
                }
            },
            error: function (xhr, status, error) {
                console.error("Erro ao excluir:", error);
                AppToast.show("Vermelho", "Erro ao excluir usuário", 5000);
            }
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha("usuario-index.js", "excluirUsuario", error);
    }
}
```

**Fluxo**:
1. Envia POST com `{ Id: usuarioId }`
2. Backend valida vínculos (Viagens, Manutenções, etc.)
3. Se houver vínculos, backend retorna `success: false` com mensagem HTML detalhada
4. Toast exibe mensagem formatada com lista de vínculos

---

### Função: `carregarRecursosUsuario()`
**Localização**: Linha 74

**Propósito**: Carrega tabela de recursos do usuário no modal

**Código**:
```javascript
function carregarRecursosUsuario(usuarioId) {
    if ($.fn.DataTable.isDataTable('#tblRecursos')) {
        $('#tblRecursos').DataTable().clear().destroy();
    }

    dataTableRecursos = $('#tblRecursos').DataTable({
        order: [[0, 'asc']],
        ajax: {
            url: "/api/Usuario/PegaRecursosUsuario",
            type: "GET",
            data: { usuarioId: usuarioId }
        },
        columns: [
            { data: "nome" },
            { data: "acesso", render: function(data, type, row) {
                const url = `/api/Usuario/UpdateStatusAcesso?IDS=${row.ids}`;
                if (data === true) {
                    return `<a href="javascript:void(0)"
                               class="btn btn-xs ftx-badge-status btn-verde updateStatusAcesso"
                               data-url="${url}">
                                <i class="fa-solid fa-unlock me-1"></i>Com Acesso
                            </a>`;
                } else {
                    return `<a href="javascript:void(0)"
                               class="btn btn-xs ftx-badge-status fundo-cinza updateStatusAcesso"
                               data-url="${url}">
                                <i class="fa-solid fa-lock me-1"></i>Sem Acesso
                            </a>`;
                }
            }}
        ]
    });
}
```

---

## Interconexões

### Quem Chama Este Arquivo
- `Areas/Authorization/Pages/Usuarios.cshtml` → Referência no `<script src="~/js/cadastros/usuario-index.js"></script>`

### O Que Este Arquivo Chama
- `/api/Usuario/GetAll` (GET) → Lista todos os usuários
- `/api/Usuario/UpdateStatusUsuario?Id=...` (GET) → Alterna status
- `/api/Usuario/UpdateCargaPatrimonial?Id=...` (GET) → Alterna carga patrimonial
- `/api/Usuario/Delete` (POST) → Exclui usuário (com validação completa backend)
- `/api/Usuario/PegaRecursosUsuario?usuarioId=...` (GET) → Lista recursos do usuário
- `/api/Usuario/UpdateStatusAcesso?IDS=...` (GET) → Alterna acesso a recurso
- `Alerta.Confirmar()` → Sistema de confirmação customizado FrotiX
- `Alerta.TratamentoErroComLinha()` → Tratamento de erros
- `AppToast.show()` → Notificações toast

---

## Endpoints API

### GET `/api/Usuario/GetAll`
**Descrição**: Lista todos os usuários do sistema

**Response**:
```json
{
  "data": [
    {
      "usuarioId": "guid",
      "nomeCompleto": "João Silva",
      "ponto": "12345",
      "fotoBase64": "base64_string_ou_null",
      "detentorCargaPatrimonial": true,
      "status": true
    }
  ]
}
```

---

### POST `/api/Usuario/Delete`
**Descrição**: Exclui usuário com validação COMPLETA de vínculos

**Request**:
```json
{
  "Id": "guid-do-usuario"
}
```

**Response (Sucesso)**:
```json
{
  "success": true,
  "message": "✅ Usuário <strong>João Silva</strong> removido com sucesso!"
}
```

**Response (Erro - Vínculos)**:
```json
{
  "success": false,
  "message": "❌ Não é possível excluir o usuário <strong>João Silva</strong>.<br><br>...(lista de vínculos em HTML)..."
}
```

---

## Frontend

### Estrutura HTML Esperada
```html
<table id="tblUsuario" class="table">
  <thead><!-- Cabeçalho gerado pela DataTable --></thead>
</table>

<!-- Modal Controle de Acesso -->
<div id="modalControleAcesso" class="modal">
  <div class="modal-body">
    <input type="hidden" id="txtUsuarioIdRecurso" />
    <p id="txtNomeUsuarioRecurso"></p>
    <table id="tblRecursos" class="table"></table>
  </div>
</div>

<!-- Modal Foto -->
<div id="modalFoto" class="modal">
  <div class="modal-body">
    <p id="txtNomeUsuarioFoto"></p>
    <div id="divFotoContainer"></div>
  </div>
</div>
```

### JavaScript Principal
- **IIFE Pattern**: Código encapsulado em `(function() { ... })()`
- **Event Delegation**: Todos os eventos delegados ao `$(document)`
- **Bootstrap Modals**: Controle manual de instâncias (`new bootstrap.Modal()`)
- **Try-Catch Universal**: Todos os blocos têm tratamento de erro

### Eventos
- `DOMContentLoaded` → Inicializa tudo
- `click` em `.btnAbrirFoto` → Abre modal de foto (avatar na lista)
- `click` em `.btnFoto` → Abre modal de foto (botão de ação)
- `click` em `.btnRecursos` → Abre modal de recursos
- `click` em `.updateStatusUsuario` → Alterna status
- `click` em `.updateCargaPatrimonial` → Alterna carga patrimonial
- `click` em `.updateStatusAcesso` → Alterna acesso a recurso (modal)
- `click` em `.btnExcluir` → Confirma e exclui usuário

---

## Troubleshooting

### Problema: Alerta de exclusão não segue padrão FrotiX
**Sintoma**: Modal de confirmação aparece com estilo genérico (ícone laranja, bordas diferentes)

**Causa**: Uso de `Swal.fire()` direto em vez de `Alerta.Confirmar()`

**Solução**: ✅ Corrigido na v2.0 (linha 500)
```javascript
// ❌ ANTES (v1.x)
Swal.fire({ title: '...', icon: 'warning', ... })

// ✅ AGORA (v2.0)
Alerta.Confirmar("Confirmar Exclusão", "Deseja...", "Sim, Excluir", "Cancelar")
```

**Status**: ✅ Corrigido em 12/01/2026

---

### Problema: Exclusão de usuário com viagens/manutenções
**Sintoma**: Usuário é excluído mesmo tendo registros vinculados

**Causa**: Backend não validava todas as tabelas

**Diagnóstico**: Verificar método `Delete` no `UsuarioController.cs`

**Solução**: ✅ Corrigido no backend (v3.0 do UsuarioController)
- Backend agora valida 5 tabelas antes de excluir
- Mensagem de erro detalha todos os vínculos
- Impede exclusão se houver QUALQUER vínculo

**Status**: ✅ Corrigido em 12/01/2026

---

### Problema: Modal não abre
**Sintoma**: Clique nos botões de Recursos/Foto não abre modal

**Causa**: Modal não encontrado no DOM ou Bootstrap não inicializado

**Solução**: Verificar se IDs `modalControleAcesso` e `modalFoto` existem no HTML

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [12/01/2026 10:45] - Melhoria: Estabilidade de Tooltips em Botões Desabilitados

**Descrição**: Melhorada estabilidade das tooltips em botões desabilitados para evitar desaparecimento rápido (flickering)

**Problema**:
- Tooltip desaparecia muito rápido ao passar mouse sobre botão Excluir desabilitado
- Movimento do mouse entre span wrapper e elementos internos causava perda de foco
- Tooltip "piscava" ou sumia antes do usuário conseguir ler a mensagem

**Causa**:
- Elementos internos (botão e ícone) ainda podiam receber eventos de mouse momentaneamente
- Falta de `tabindex` no span impedia foco adequado
- Transição entre elementos causava re-renderização da tooltip

**Solução Implementada**:
- Adicionado `tabindex="0"` ao span wrapper (permite foco via teclado e estabiliza tooltip)
- Classe Bootstrap `d-inline-block` em vez de style inline
- `pointer-events: none` aplicado TAMBÉM ao ícone `<i>` (não apenas ao botão)
- Garante que APENAS o span captura eventos de mouse

**Código Atualizado**:
```javascript
// Botão Excluir DESABILITADO (versão estável)
btnExcluir = `<span class="d-inline-block" tabindex="0"
                    data-ejtip="Usuário não pode ser excluído pois está em uso">
                  <button type="button" class="btn btn-vinho btn-icon-28"
                          disabled style="pointer-events: none;">
                      <i class="fa-duotone fa-trash-can" style="pointer-events: none;"></i>
                  </button>
              </span>`;

// Botão Foto DESABILITADO (versão estável)
btnFoto = `<span class="d-inline-block" tabindex="0"
                 data-ejtip="Usuário sem foto">
               <button type="button" class="btn btn-foto btn-icon-28"
                       disabled style="pointer-events: none;">
                   <i class="fa-duotone fa-id-badge" style="pointer-events: none;"></i>
               </button>
           </span>`;
```

**Mudanças**:
1. ✅ `tabindex="0"` → Permite foco e estabiliza tooltip
2. ✅ `class="d-inline-block"` → Classe Bootstrap (mais limpo)
3. ✅ `style="pointer-events: none;"` no `<i>` → Garante que ícone não captura eventos

**Arquivos Afetados**:
- `wwwroot/js/cadastros/usuario-index.js` (linhas 293-336)

**Impacto**:
- ✅ Tooltip permanece visível tempo suficiente para leitura
- ✅ Sem flickering ou desaparecimento rápido
- ✅ Experiência consistente em todos os navegadores
- ✅ Acessibilidade melhorada (foco via teclado)

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 2.3

---

## [12/01/2026 10:30] - Correção: Tooltips em Botões Desabilitados com Wrapper

**Descrição**: Corrigido problema onde tooltips não apareciam em botões desabilitados

**Problema**:
- Tooltips não funcionam em elementos HTML com atributo `disabled`
- Elementos desabilitados não disparam eventos de mouse (hover, mouseenter, etc.)
- Usuário não via as mensagens explicativas nos botões desabilitados

**Causa**:
- Navegadores bloqueiam eventos de ponteiro em elementos `disabled`
- Tentativa anterior colocava `data-ejtip` diretamente no botão desabilitado

**Solução Implementada**:
- Envolvido botões desabilitados em um `<span>` wrapper
- `<span>` recebe o atributo `data-ejtip` (tooltip)
- Botão dentro do span tem `pointer-events: none` para não capturar eventos
- Span captura eventos de mouse e exibe tooltip normalmente

**Código Modificado** (linhas 293-336):
```javascript
// Botão Foto DESABILITADO (sem foto)
btnFoto = `<span data-ejtip="Usuário sem foto" style="display: inline-block;">
               <button type="button" class="btn btn-foto btn-icon-28"
                       disabled style="pointer-events: none;">
                   <i class="fa-duotone fa-id-badge"></i>
               </button>
           </span>`;

// Botão Excluir DESABILITADO (com vínculos)
btnExcluir = `<span data-ejtip="Usuário não pode ser excluído pois está em uso"
                    style="display: inline-block;">
                  <button type="button" class="btn btn-vinho btn-icon-28"
                          disabled style="pointer-events: none;">
                      <i class="fa-duotone fa-trash-can"></i>
                  </button>
              </span>`;
```

**Arquivos Afetados**:
- `wwwroot/js/cadastros/usuario-index.js` (linhas 269-344)

**Impacto**:
- ✅ Tooltips agora aparecem corretamente em botões desabilitados
- ✅ Mensagens explicativas visíveis: "Usuário sem foto" e "Usuário não pode ser excluído pois está em uso"
- ✅ Visual mantido (botão continua desabilitado/acinzentado)
- ✅ Técnica padrão para tooltips em elementos desabilitados

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 2.2

---

## [12/01/2026 10:00] - Botões de Exclusão e Foto Desabilitados Preventivamente com Tooltips

**Descrição**: Implementada renderização condicional de botões Excluir e Foto, desabilitando-os preventivamente com tooltips informativos

**Problema Anterior**:
- Botão de exclusão sempre habilitado, mesmo para usuários que não podem ser excluídos
- Usuário descobria que não podia excluir apenas APÓS clicar e tentar
- Botão de foto não tinha tooltip informativo quando usuário não tinha foto

**Solução Implementada**:
- **Botão Excluir**:
  - Verifica `row.podeExcluir` retornado pelo endpoint `GetAll`
  - Se `false`: renderiza botão com `disabled` e classe `disabled`
  - Tooltip diferenciado: "Usuário não pode ser excluído pois está em uso"
- **Botão Foto**:
  - Verifica se `row.fotoBase64` está presente
  - Se vazio: renderiza botão com `disabled`
  - Tooltip ajustado: "Usuário sem foto" (antes era "Sem foto")

**Código Modificado** (linhas 269-314):
```javascript
// Botão Foto
const temFoto = row.fotoBase64 ? '' : ' disabled';
const tooltipFoto = row.fotoBase64 ? 'Visualizar foto' : 'Usuário sem foto';

// Botão Excluir
const podeExcluir = row.podeExcluir !== false;
const disabledExcluir = podeExcluir ? '' : ' disabled';
const tooltipExcluir = podeExcluir
    ? 'Excluir usuário'
    : 'Usuário não pode ser excluído pois está em uso';

// Renderização
<button ... class="btnFoto${temFoto}" data-ejtip="${tooltipFoto}"${temFoto ? ' disabled' : ''}>
<button ... class="btnExcluir${disabledExcluir}" data-ejtip="${tooltipExcluir}"${disabledExcluir ? ' disabled' : ''}>
```

**Dependências**:
- Endpoint `GetAll` no UsuarioController retornando `podeExcluir` e `fotoBase64`

**Arquivos Afetados**:
- `wwwroot/js/cadastros/usuario-index.js` (linhas 273-308)

**Impacto**:
- ✅ **Melhor UX**: Feedback visual imediato sobre ações possíveis/impossíveis
- ✅ **Menos frustrações**: Usuário vê antecipadamente o que pode/não pode fazer
- ✅ **Tooltips informativos**: Explicam o motivo da desabilitação
- ✅ **Menos requisições**: Evita chamadas AJAX para ações que falhariam

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 2.1

---

## [12/01/2026 09:10] - Correção do Alerta de Exclusão - Padrão FrotiX

**Descrição**: Corrigido alerta de confirmação de exclusão para usar o padrão customizado FrotiX

**Problema**:
- Alerta de confirmação usava `Swal.fire()` diretamente
- Não seguia padrão visual FrotiX (cores, ícones, formatação)
- Imagem fornecida mostrava estilo genérico (ícone laranja, bordas simples)

**Causa**:
- Código legado que não foi migrado para o padrão FrotiX
- Uso de `Swal.fire()` em vez de `Alerta.Confirmar()`

**Solução**:
- Substituído `Swal.fire()` por `Alerta.Confirmar()` (sistema customizado)
- Adicionado texto informativo sobre validação automática de vínculos
- Mantém formatação HTML com cores e tamanhos de fonte consistentes
- Usa botões "Sim, Excluir" e "Cancelar" conforme padrão

**Código Alterado** (linhas 498-519):
```javascript
// ANTES
Swal.fire({
    title: 'Confirmar Exclusão',
    html: `...`,
    icon: 'warning',
    confirmButtonColor: '#8B0000',
    cancelButtonColor: '#6c757d',
    confirmButtonText: '<i class="fa-solid fa-trash-can me-1"></i> Sim, Excluir',
    reverseButtons: true
})

// AGORA
Alerta.Confirmar(
    "Confirmar Exclusão",
    `Deseja realmente excluir o usuário <strong>${nome}</strong>?<br><br>` +
    `<small style="color: #dc3545; font-size: 0.875rem;">⚠️ Esta ação não pode ser desfeita!</small><br><br>` +
    `<small style="color: #6c757d; font-size: 0.875rem;">O sistema verificará automaticamente se o usuário possui registros vinculados (Viagens, Manutenções, etc.).</small>`,
    "Sim, Excluir",
    "Cancelar"
)
```

**Arquivos Afetados**:
- `wwwroot/js/cadastros/usuario-index.js` (função confirmarExclusao, linha 498)

**Impacto**:
- ✅ Visual consistente em todo o sistema
- ✅ Seguir padrão FrotiX de alertas
- ✅ Texto informativo sobre validação automática

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 2.0

---

## Histórico de Versões

| Versão | Data | Descrição |
|--------|------|-----------|
| 1.0 | Anterior a 2026 | Versão inicial com Swal.fire() |
| 2.0 | 12/01/2026 | Correção para padrão FrotiX (Alerta.Confirmar) |
| 2.1 | 12/01/2026 | Botões desabilitados preventivamente + tooltips informativos |
| 2.2 | 12/01/2026 | Correção tooltips em botões desabilitados (wrapper span) |
| 2.3 | 12/01/2026 | Melhoria estabilidade tooltips (tabindex + pointer-events no ícone) |

---

## Referências

- [usuario_001.js](./usuario_001.md) - Versão legacy (coexiste com este)
- [UsuarioController](../../Controllers/UsuarioController.md) - API backend
- [Alerta.js](../alerta.js.md) - Sistema de alertas FrotiX

---

**Última atualização**: 12/01/2026
**Autor**: Sistema FrotiX
**Versão**: 2.3
