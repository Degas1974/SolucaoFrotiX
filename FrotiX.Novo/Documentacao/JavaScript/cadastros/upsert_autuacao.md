# Documentação: JavaScript - upsert_autuacao.js

> **Última Atualização**: 16/01/2026
> **Versão Atual**: 1.0

---

# PARTE 1: DOCUMENTAÇÃO TÉCNICA

## Visão Geral

Script responsável pela interatividade, formatação e validação do formulário de Multas (Autuações).

## Funcionalidades Principais

### 1. Formatação de Texto (Title Case)

- Aplica Title Case automaticamente nos campos de Número da Infração e Localização.
- Garante que a primeira letra de cada palavra seja maiúscula.

### 2. Validação Chronológica de Datas

- Função `validarOrdemDatas(campoId)` garante que:
  - Data da Infração < Data da Notificação
  - Data da Notificação < Data Limite de Defesa
  - Data Limite de Defesa < Data do Pagamento (se houver)

### 3. Integração com Alertas FrotiX

- Utiliza a biblioteca `Alerta` para exibir mensagens de erro padronizadas.

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

---

## [16/01/2026 17:20] - Criação e Refatoração de Validações

**Descrição**: Refatoração completa para suportar as novas regras de negócio do módulo de Multas.

**Mudanças**:

- Implementação de Title Case para campos de texto.
- Implementação de lógica de datas encadeadas.
- Migração para o padrão de alertas FrotiX.
