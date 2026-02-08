# Documentação: UsuarioController.Usuarios.cs (Classe Parcial)

> **Última Atualização**: 12/01/2026
> **Versão Atual**: 3.4

---

## ⚠️ CLASSE PARCIAL

Este arquivo faz parte da classe `UsuarioController` implementada como **partial class**.

**A documentação completa está em**:
📄 [UsuarioController.md](./UsuarioController.md)

---

## Arquivos da Classe Parcial

A classe `UsuarioController` é dividida em:

1. **`UsuarioController.cs`** - Métodos principais
   - `Get()` - Listagem básica de usuários
   - `Delete()` - Exclusão com validação de vínculos
   - `UpdateStatusUsuario()` - Toggle ativo/inativo
   - `UpdateCargaPatrimonial()` - Toggle detentor de carga
   - `UpdateStatusAcesso()` - Toggle acesso a recursos
   - Outros métodos de gestão

2. **`UsuarioController.Usuarios.cs`** (ESTE ARQUIVO) - Métodos específicos de usuários
   - `GetAll()` - Lista completa com fotos e validação de exclusão
   - `GetFoto()` - Retorna foto individual de usuário

---

## Métodos Implementados Neste Arquivo

### GET `/api/Usuario/GetAll`

**Descrição**: Retorna lista completa de usuários com foto em Base64 e validação de exclusão

**Validações de PodeExcluir**:
- ✅ Verifica vínculos em `ControleAcesso`
- ✅ Verifica vínculos em `Viagem` (UsuarioIdCriacao, UsuarioIdFinalizacao)
- ✅ Verifica vínculos em `Manutencao` (IdUsuarioCriacao, IdUsuarioAlteracao, IdUsuarioFinalizacao, IdUsuarioCancelamento)
- ✅ Verifica vínculos em `MovimentacaoPatrimonio` (ResponsavelMovimentacao)
- ✅ Verifica vínculos em `SetorPatrimonial` (DetentorId)

**Response**:
```json
{
  "data": [
    {
      "usuarioId": "guid",
      "nomeCompleto": "João Silva",
      "ponto": "PONTO_01",
      "detentorCargaPatrimonial": true,
      "status": true,
      "fotoBase64": "base64string...",
      "podeExcluir": false
    }
  ]
}
```

**Localização**: Linhas 18-101

---

### GET `/api/Usuario/GetFoto`

**Descrição**: Retorna foto de um usuário específico em Base64

**Parâmetros**:
- `usuarioId` (string) - ID do usuário

**Response**:
```json
{
  "success": true,
  "data": {
    "nomeCompleto": "João Silva",
    "fotoBase64": "base64string..."
  }
}
```

**Localização**: Linhas 106-117

---

## Integração com Frontend

### Usado por:
- `Pages/Usuarios/Index.cshtml` → `wwwroot/js/cadastros/usuario-index.js`
  - DataTable chama `GetAll()` para popular grid
  - Modal de foto usa endpoint (indiretamente, foto vem do GetAll)

---

## Histórico de Modificações

Ver [UsuarioController.md - PARTE 2](./UsuarioController.md#parte-2-log-de-modificaçõescorreções) para histórico completo.

**Última modificação neste arquivo**:
- **12/01/2026 10:15** - Adicionada validação de `PodeExcluir` no método `GetAll()`

---

**Última atualização**: 12/01/2026
**Autor**: Sistema FrotiX
**Versão**: 3.4


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
