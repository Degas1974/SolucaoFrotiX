# CONTEXTO DO SISTEMA FROTIX - BASE DE CONHECIMENTO VITAL

> **Instrução Crítica para IA:** Leia este arquivo no início de cada sessão. Ele contém as leis imutáveis, padrões arquiteturais e mapas de conhecimento do Projeto FrotiX.

---

## 1. LEIS IMUTÁVEIS (Regras de Ouro)

### 1.1. Regra Suprema da Documentação (CRÍTICA 🚨)
- **Sincronia Total**: Alterou código? **Atualize a documentação no mesmo commit.**
- **Git Hook**: Existe um hook `pre-commit` que bloqueia commits se a documentação estiver desatualizada.
- **Fluxo**:
  1. Alterar código (`.cs`, `.js`, `.cshtml`).
  2. Atualizar respectivo `.md` em `Documentacao/`.
  3. Adicionar entrada no Log do `.md`.
  4. `git add` código + doc.
  5. `git commit`.

### 1.2. Segurança e Robustez
- **Try-Catch Universal**: 
  - **JS**: `try { ... } catch (e) { Alerta.TratamentoErroComLinha("arquivo.js", "metodo", e); }`
  - **C#**: `try { ... } catch (Ex) { Alerta.TratamentoErroComLinha("Controller.cs", "Metodo", Ex); return Json(...); }`
- **Sem Alertas Nativos**: Proibido `alert()`, `confirm()`. Use `Alerta.Sucesso()`, `Alerta.Confirmar()`, `AppToast.show()`.

### 1.3. Identidade Visual (UI/UX)
- **Ícones**: **SEMPRE** `fa-duotone`. Primária: Laranja (`#ff6b35`), Secundária: Cinza (`#6c757d`).
- **Botões**: 
  - Ação: `btn-azul` (Salvar/Editar).
  - Voltar: `btn-header-orange` (Header) ou `btn-voltar` (Footer).
  - Excluir: `btn-vinho` ou `btn-delete`.
- **Feedback**:
  - **Spinner**: `FtxSpin.show()` (transição de página) ou `FtxLoading.apply(btn)` (botões).
  - **Ripple**: Automático em botões (via `frotix.js`).

---

## 2. MAPA ARQUITETURAL

### 2.1. Estrutura de Pastas
| Diretório | Propósito | Padrão de Arquivo |
|-----------|-----------|-------------------|
| `Pages/` | Frontend + Backend Leve (Razor) | `Módulo/Index.cshtml` |
| `Controllers/` | API e Lógica Pesada | `[Nome]Controller.cs` |
| `wwwroot/js/cadastros/` | Scripts Específicos de Página | `[modulo].js` ou `[modulo]_upsert.js` |
| `Documentacao/` | Base de Conhecimento | `Funcionalidade - [Módulo] - [Página].md` |

### 2.2. Tecnologias Core
- **Backend**: ASP.NET Core (.NET 8/9), Entity Framework Core.
- **Frontend**: Razor Pages, jQuery, Bootstrap 5 (custom), Syncfusion (Grids/Combos).
- **Utils**: `frotix.js` (Lib proprietária), `Alerta.js` (Wrapper SweetAlert).

---

## 3. PADRÕES DE IMPLEMENTAÇÃO (Cheat Sheet)

### 3.1. Frontend: DataTable com AJAX e Renderers
*Padrão para listagens (Index).*
```javascript
// Exemplo: wwwroot/js/cadastros/veiculo.js
var dataTable = $('#tbl').DataTable({
    ajax: { url: "/api/veiculo", type: "GET" },
    columns: [
        { data: "placa" },
        { 
            data: "status",
            render: function(data, type, row) {
                // Badge clicável para alternar status
                const classe = data ? 'btn-verde' : 'fundo-cinza';
                const texto = data ? 'Ativo' : 'Inativo';
                return `<a href="javascript:void(0)" class="updateStatus ${classe}" data-url="...">...</a>`;
            } 
        },
        {
            data: "id",
            render: function(data) {
                // Botões de ação com ícones Duotone
                return `<a href="/Veiculo/Upsert?id=${data}" class="btn btn-azul"><i class="fa-duotone fa-pen-to-square"></i></a>`;
            }
        }
    ]
});
```

### 3.2. Frontend: Filtros Inteligentes
*Padrão para recarregar tabelas.*
```javascript
// Syncfusion ComboBox change event
change: function(args) {
    if (args.value) {
        dtDestroySafe(); // Helper para destruir tabela
        var opts = dtCommonOptions(); // Opções padrão
        opts.ajax = {
            url: "/api/veiculo/filtrar",
            data: { id: args.value } // Parâmetro para API
        };
        $('#tbl').DataTable(opts); // Recria tabela
    }
}
```

### 3.3. Backend: API Controller Híbrido
*Padrão para Controllers.*
```csharp
// Exemplo: Controllers/VeiculoController.cs
[HttpGet] // API para DataTable
public IActionResult Get() {
    try {
        var dados = _uow.Veiculo.GetAll().Select(v => new { ... });
        return Json(new { data = dados });
    } catch (Exception ex) { ... }
}

[Route("Delete")] // API de Exclusão com Validação
[HttpPost]
public IActionResult Delete(ViewModel model) {
    // 1. Verificar dependências
    if (_uow.Viagem.Existe(v => v.VeiculoId == model.Id))
        return Json(new { success = false, message = "Possui viagens vinculadas!" });
    
    // 2. Excluir
    _uow.Veiculo.Remove(id);
    _uow.Save();
    return Json(new { success = true });
}
```

### 3.4. Infraestrutura: frotix.js
*Funcionalidades globais disponíveis.*
- `FtxSpin.show(msg)`: Tela de loading full-screen.
- `FtxLoading.apply(elem)`: Coloca spinner dentro de um botão.
- `tiraAcento(str)`: Normaliza strings para nomes de arquivo.
- `formatarDataBR(str)`: Formata ISO/Ticks para DD/MM/YYYY.

---

## 4. ÍNDICE DE CONHECIMENTO (Onde ler mais?)

Se você vai mexer em... **LEIA ISTO PRIMEIRO:**

| Módulo/Área | Arquivo de Documentação Principal |
|-------------|-----------------------------------|
| **Viagens (Complexo)** | `Documentacao/Pages/Viagens - Index.md` |
| **Abastecimento** | `Documentacao/Pages/Abastecimento - Index.md` |
| **Contratos** | `Documentacao/Pages/Contrato - Index.md` |
| **Motoristas** | `Documentacao/Pages/Motorista - Index.md` |
| **Unidades** | `Documentacao/Pages/Unidade - Index.md` |
| **Frontend Core** | `Documentacao/JavaScript/frotix.js.md` |
| **Banco de Dados** | `Documentacao/Data/FrotiXDbContext.md` |

---

## 5. CHECKLIST DE ENTREGA (Antes de dizer "Terminei")

1. [ ] O código segue o estilo do projeto (var, try-catch, indentação)?
2. [ ] Usei ícones `fa-duotone`?
3. [ ] Os alertas são `Alerta.*` e não `alert()`?
4. [ ] **CRÍTICO:** Atualizei a documentação `.md` correspondente e o Log de Alterações?
5. [ ] **CRÍTICO:** Commitei o código E a documentação juntos?

---
*Versão: 2026.1 - Gerado por Agente IA após análise profunda do codebase.*
