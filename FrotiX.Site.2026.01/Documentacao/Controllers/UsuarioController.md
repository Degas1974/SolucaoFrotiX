# Documentação: UsuarioController.cs

> **Última Atualização**: 12/01/2026
> **Versão Atual**: 3.4

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O `UsuarioController` gerencia operações CRUD de usuários do sistema (AspNetUsers), incluindo gestão de status e carga patrimonial.

**Principais características:**

✅ **CRUD Completo**: Listagem, exclusão e atualização de status
✅ **Carga Patrimonial**: Gestão de flag `DetentorCargaPatrimonial`
✅ **Validação de Dependências**: Verifica controles de acesso antes de excluir

**Nota**: Controller implementado como partial class dividido em múltiplos arquivos:
- `UsuarioController.cs` - Métodos principais (Get, Delete, Update)
- `UsuarioController.Usuarios.cs` - Métodos específicos de usuários (GetAll, GetFoto)

**⚠️ IMPORTANTE**: Esta documentação abrange **ambos** os arquivos da classe parcial.

---

## Endpoints API

### GET `/api/Usuario`

**Descrição**: Retorna lista de usuários com informações básicas e flag indicando se podem ser excluídos

**MELHORIA (v3.2)**: Endpoint agora verifica antecipadamente se cada usuário pode ser excluído, aplicando a mesma lógica de validação do endpoint Delete. Isso permite que a UI desabilite botões de exclusão preventivamente, melhorando a UX.

**Validações aplicadas para cada usuário**:
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
      "podeExcluir": false
    }
  ]
}
```

**Propriedades**:
- `usuarioId` (string): ID único do usuário
- `nomeCompleto` (string): Nome completo
- `ponto` (string): Ponto de lotação
- `detentorCargaPatrimonial` (bool): Se é detentor de carga patrimonial
- `status` (bool): Se está ativo (true) ou inativo (false)
- `podeExcluir` (bool): **NOVO** - Se o usuário pode ser excluído (false = tem vínculos em outras tabelas)

---

### POST `/api/Usuario/Delete`

**Descrição**: Exclui usuário com validação COMPLETA de integridade referencial

**Validações IMPLEMENTADAS (v3.0)**:
✅ `ControleAcesso` - Recursos do sistema vinculados
✅ `Viagem` - Usuário como responsável por cadastro ou finalização
✅ `Manutencao` - Usuário como responsável por cadastro, alteração, finalização ou cancelamento
✅ `MovimentacaoPatrimonio` - Usuário como responsável pela movimentação
✅ `SetorPatrimonial` - Usuário como detentor do setor

**Response (Sucesso)**:
```json
{
  "success": true,
  "message": "✅ Usuário <strong>João Silva</strong> removido com sucesso!"
}
```

**Response (Erro - Vínculos Encontrados)**:
```json
{
  "success": false,
  "message": "❌ Não é possível excluir o usuário <strong>João Silva</strong>.<br><br><strong>Motivo:</strong> Existem registros vinculados a este usuário nas seguintes áreas:<br><br><ul style='text-align: left; margin: 0.5rem 0;'><li>Viagens (como responsável pelo cadastro ou finalização)</li><li>Manutenções (como responsável pelo cadastro, alteração, finalização ou cancelamento)</li></ul><br><small style='color: #6c757d;'>Para excluir este usuário, primeiro remova ou transfira os registros vinculados.</small>"
}
```

**Segurança**: A validação impede perda de dados de auditoria (rastreabilidade de quem criou/modificou registros)

---

### GET `/api/Usuario/UpdateStatusUsuario`

**Descrição**: Alterna status ativo/inativo

**Parâmetros**: `Id` (string) - ID do usuário

---

### GET `/api/Usuario/UpdateCargaPatrimonial`

**Descrição**: Alterna flag `DetentorCargaPatrimonial`

**Parâmetros**: `Id` (string) - ID do usuário

**Uso**: Define se usuário é detentor de carga patrimonial

---

## Interconexões

### Quem Chama Este Controller

- **Pages**: `Pages/Usuario/Index.cshtml`
- **Pages**: Páginas administrativas

### O Que Este Controller Chama

- **`_unitOfWork.AspNetUsers`**: CRUD
- **`_unitOfWork.ControleAcesso`**: Validação de dependências

---

## Notas Importantes

1. **Partial Class**: Controller dividido em múltiplos arquivos
2. **Carga Patrimonial**: Flag específica para gestão patrimonial
3. **Controle de Acesso**: Usuários têm relacionamento com `ControleAcesso` para permissões

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [12/01/2026 10:15] - Correção de Duplicação de Método GetAll

**Descrição**: Corrigida duplicação de método `GetAll` que causava erro de compilação CS0111

**Problema**:
- Endpoint `GetAll` foi adicionado em `UsuarioController.cs` mas já existia em `UsuarioController.Usuarios.cs` (classe parcial)
- Causava erro: "Tipo 'UsuarioController' já define um membro chamado 'GetAll' com os mesmos tipos de parâmetro"

**Solução Implementada**:
- Removido `GetAll` duplicado de `UsuarioController.cs`
- Modificado `GetAll` existente em `UsuarioController.Usuarios.cs` para incluir validação de `PodeExcluir`
- Adicionado `using System.Collections.Generic;` necessário para `List<object>`
- Mantida ordenação por `NomeCompleto`

**Arquivos Afetados**:
- `Controllers/UsuarioController.cs` - Removido GetAll duplicado
- `Controllers/UsuarioController.Usuarios.cs` (método GetAll, linhas 11-101) - Adicionada validação PodeExcluir

**Validações aplicadas no GetAll**:
1. `ControleAcesso` - Verifica se usuário tem recursos vinculados
2. `Viagem` - Verifica se é responsável por criação ou finalização
3. `Manutencao` - Verifica se é responsável por qualquer operação
4. `MovimentacaoPatrimonio` - Verifica se é responsável por movimentações
5. `SetorPatrimonial` - Verifica se é detentor de setor

**Impacto**:
- ✅ Corrige erro de compilação
- ✅ Mantém funcionalidade de desabilitar botões preventivamente
- ✅ Código organizado em arquivo parcial correto

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 3.4

---

## [12/01/2026 09:30] - Melhoria UX: Desabilitar Botão de Exclusão Preventivamente

**Descrição**: Implementada validação antecipada de exclusão no endpoint GET, permitindo desabilitar botões de exclusão antes do usuário tentar excluir

**Problema Anterior**:
- Usuário só descobria que não podia excluir APÓS clicar no botão e tentar excluir
- Isso causava frustração e perda de tempo
- A mensagem de erro aparecia depois da ação, não antes

**Solução Implementada**:
- Endpoint GET agora retorna propriedade `podeExcluir` para cada usuário
- JavaScript verifica `podeExcluir` e renderiza botão desabilitado quando `false`
- Tooltip informativo no botão desabilitado: "Usuário não pode ser excluído pois está em uso"
- Aplica mesma lógica de validação do Delete (5 tabelas verificadas)
- Ícone do botão Senha corrigido de `fa-camera-retro` para `fa-key`

**Vantagens**:
- ✅ **Melhor UX**: Usuário sabe imediatamente quais usuários podem/não podem ser excluídos
- ✅ **Menos cliques**: Evita tentativa de exclusão que falharia
- ✅ **Feedback proativo**: Tooltip explica o motivo antes da ação
- ✅ **Performance**: Validação feita de uma vez no carregamento (batch) em vez de uma requisição por tentativa

**Arquivos Afetados**:
- `Controllers/UsuarioController.cs` (método Get, linhas 28-111)
- `wwwroot/js/cadastros/usuario_001.js` (coluna Ações, linhas 293-342, drawCallback 350-356)

**Impacto**:
- ✅ Melhora significativa na experiência do usuário
- ✅ Reduz chamadas AJAX desnecessárias ao backend
- ✅ Consistência visual e funcional

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 3.2

---

## [12/01/2026 09:14] - Correção de Erro de Compilação (CS0246)

**Descrição**: Adicionada diretiva `using System.Collections.Generic` faltante

**Problema**:
- Erro CS0246: O nome do tipo ou do namespace "List<>" não pode ser encontrado
- Faltava a diretiva `using System.Collections.Generic;` no topo do arquivo
- Causava erro de compilação na linha 77 ao usar `var vinculos = new List<string>();`

**Solução Implementada**:
- Adicionada a diretiva `using System.Collections.Generic;` junto com os outros usings

**Arquivos Afetados**:
- `Controllers/UsuarioController.cs` (linha 5)

**Impacto**:
- ✅ Corrige erro de compilação
- ✅ Permite uso de `List<T>` no método Delete

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 3.1

---

## [12/01/2026 09:05] - Validação Completa de Exclusão de Usuário

**Descrição**: Implementada validação COMPLETA de integridade referencial antes de excluir usuário

**Problema Anterior**:
- Validava apenas tabela `ControleAcesso`
- Usuários com viagens/manutenções podiam ser excluídos
- Perda de rastreabilidade de auditoria (quem criou/finalizou registros)

**Solução Implementada**:
- Verificação de vínculos em 5 tabelas:
  1. `ControleAcesso` (recursos)
  2. `Viagem` (UsuarioIdCriacao, UsuarioIdFinalizacao)
  3. `Manutencao` (IdUsuarioCriacao, IdUsuarioAlteracao, IdUsuarioFinalizacao, IdUsuarioCancelamento)
  4. `MovimentacaoPatrimonio` (ResponsavelMovimentacao)
  5. `SetorPatrimonial` (DetentorId)
- Mensagem de erro detalhada com lista de todos os vínculos
- Impede exclusão se houver QUALQUER registro vinculado

**Arquivos Afetados**:
- `Controllers/UsuarioController.cs` (método Delete, linhas 60-164)

**Impacto**:
- ✅ Protege integridade referencial
- ✅ Preserva histórico de auditoria
- ✅ Previne exclusões acidentais de usuários com dados

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 3.0

---

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação da documentação completa do UsuarioController

**Arquivos Afetados**:
- `Controllers/UsuarioController.cs`
- `Controllers/UsuarioController.Usuarios.cs`

**Impacto**: Documentação de referência para operações de usuários

**Status**: ✅ **Concluído**

**Versão**: 2.0

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
