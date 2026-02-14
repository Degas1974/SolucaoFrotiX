# Gestão de Identidade, Perfis e Acesso

O ecossistema **FrotiX** utiliza um modelo de segurança baseado em **Claims (ASP.NET Core Identity)** para garantir que cada usuário tenha acesso apenas ao que lhe é de direito. O UsuarioController orquestra não apenas o ciclo de vida da conta, mas também a integridade histórica de quem opera o sistema.

## 🔐 Segurança e Auditoria

A gestão de usuários é o pilar da rastreabilidade. Cada ação crítica no sistema (como autorizar uma viagem ou liquidar uma nota fiscal) grava o UsuarioId, permitindo auditorias precisas.

### O Fluxo de Segurança:
1.  **Controle de Acesso por Recurso:** Através do ControleAcesso, vinculamos usuários a permissões específicas. O controlador garante que um usuário não possa ser removido se houver permissões ativas vinculadas a ele.
2.  **Identidade Digital:** O sistema suporta o armazenamento de fotos de perfil em formato binário (yte[]), convertidas em Base64 para exibição fluida na interface sem sobrecarregar o tráfego de imagens estáticas.
3.  **Status e Ativação:** Bloqueio instantâneo de acesso através da flag de Status, permitindo desativar colaboradores sem perder seu histórico de ações.

## 🛠 Snippets de Lógica Principal

### Validação de Vínculos (Integridade Referencial)
O método Delete e o GetAll (na parcial UsuarioController.Usuarios.cs) implementam uma barreira de segurança robusta. Antes de permitir a exclusão (ou até de habilitar o botão na interface), o sistema varre 5 dimensões críticas:

`csharp
bool podeExcluir = true;

// 1. Possui permissões configuradas?
var temControleAcesso = _unitOfWork.ControleAcesso.GetFirstOrDefault(ca => ca.UsuarioId == u.Id);
if (temControleAcesso != null) podeExcluir = false;

// 2. Criou ou finalizou viagens? (Rastreabilidade operacional)
if (podeExcluir) {
    var temViagens = _unitOfWork.Viagem.GetFirstOrDefault(v => v.UsuarioIdCriacao == u.Id || v.UsuarioIdFinalizacao == u.Id);
    if (temViagens != null) podeExcluir = false;
}

// 3. Gerenciou manutenções?
if (podeExcluir) {
    var temManutencoes = _unitOfWork.Manutencao.GetFirstOrDefault(m => m.IdUsuarioCriacao == u.Id);
    if (temManutencoes != null) podeExcluir = false;
}

// 4. É responsável por movimentar bens? (Módulo de Patrimônio)
if (podeExcluir) {
    var temMovimentacao = _unitOfWork.MovimentacaoPatrimonio.GetFirstOrDefault(mp => mp.ResponsavelMovimentacao == u.Id);
    if (temMovimentacao != null) podeExcluir = false;
}
`

## 📝 Notas de Implementação

- **Performance na Grid:** O método GetAll retorna as fotos em Base64 junto com os metadados. Para listas muito grandes, recomenda-se o uso do endpoint específico GetFoto via demanda.
- **Detentores de Frota:** A flag DetentorCargaPatrimonial sinaliza que o usuário responde legalmente por bens do inventário, travando sua conta para conferência em caso de desligamento.
- **Auditoria de Alteração:** O sistema captura automaticamente o UsuarioId de qualquer modificação em registros do banco de dados através da classe base de auditoria.

---
*Documentação gerada para a Solução FrotiX 2026.*
