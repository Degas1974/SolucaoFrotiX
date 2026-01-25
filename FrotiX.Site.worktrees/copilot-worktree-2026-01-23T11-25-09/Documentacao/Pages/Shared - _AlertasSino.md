# Documentação: Shared - \_AlertasSino

> **Última Atualização**: 23/01/2026
> **Versão Atual**: 1.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

Partial responsável por renderizar o sino de alertas no layout global, incluindo badge de contagem, menu de alertas e link para a listagem completa.

## Estrutura Principal

- **Botão do sino** com badge oculto por padrão.
- **Menu de alertas** com header, corpo (lista) e footer.
- **Estado inicial** com spinner enquanto carrega os alertas.

## Integrações

- Atualização de alertas via SignalR (manipulada por scripts globais).
- Redirecionamento para `/AlertasFrotiX/AlertasFrotiX`.

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [23/01/2026 09:20] - Registro de documentação inicial

**Descrição**:
Documentação inicial do partial do sino de alertas.

**Arquivos Afetados**:

- `Pages/Shared/_AlertasSino.cshtml`

**Impacto**: Rastreabilidade e padronização da documentação interna.

**Status**: ✅ Concluído

**Versão**: 1.0
