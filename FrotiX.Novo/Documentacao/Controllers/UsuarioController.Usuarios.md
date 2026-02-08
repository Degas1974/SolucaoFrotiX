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
