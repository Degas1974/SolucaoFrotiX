# Usuarios.cshtml — Gestão de Usuários (Português)

> **Arquivo:** `Areas/Authorization/Pages/Usuarios.cshtml`  
> **Papel:** página de administração de usuários com grid clássico e botão de criação.

---

## ✅ Visão Geral

Página em português destinada à gestão de usuários com tabela detalhada e ação de cadastro rápido. Usa DataTables clássico e script externo `~/js/usuarios.js`.

---

## 🔧 Estrutura Principal

- **Tabela**: `#tblUser` com colunas de usuário, email, papel e status.
- **Botão**: “Adicionar Usuário” apontando para `/admin/user/create`.
- **Script**: `~/js/usuarios.js` controla o comportamento.

---

## 🧩 Snippets Comentados

```cshtml
<a href="/admin/user/create" class="btn btn-info">
  <i class="fa fa-user-plus"></i> Adicionar Usuário
</a>
```

```cshtml
<table id="tblUser" class="table table-bordered table-striped">
  <thead>
    <tr>
      <th>Usuário</th>
      <th>Nome</th>
      <th>Email</th>
      <th>Papel</th>
      <th>Data de Registro</th>
      <th>Status</th>
      <th>Ação</th>
    </tr>
  </thead>
</table>
```

---

## ✅ Observações Técnicas

- Possui estilos locais (`.fundo-cinza`, `.label`).
- O script externo concentra a lógica de carregamento.
- Usa CDN do DataTables para layout rápido.


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
