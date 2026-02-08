# 🚀 Portal de Documentação - Solução FrotiX 2026

Bem-vindo à base de conhecimento técnica e operacional do **FrotiX**. Este portal consolida toda a inteligência por trás da gestão de frotas, manutenção, patrimônio e logística integrada.

## 🧭 Navegação por Módulos Core

Abaixo estão os pilares fundamentais do sistema, explicados sob a ótica de "Prosa Leve" (foco no porquê e como, além do código):

### 1. 🏎️ Operação e Logística
- **[Agenda de Viagens](Agenda - Index.md):** O coração do agendamento, focado em ocupação inteligente e conflitos zero.
- **[Motor de Viagens](Viagens - Index.md):** Onde os custos são calculados e a rastreabilidade acontece.
- **[TaxiLeg & Integrações](TaxiLeg - Importacao.md):** Sincronização com serviços de mobilidade externa.

### 2. 💰 Gestão Financeira e Ativos
- **[Patrimônio e Ativos](Patrimonio - Index.md):** Rastreabilidade total de bens móveis e eletrônicos.
- **[Notas Fiscais e Empenhos](NotaFiscal - Index.md):** O fechamento financeiro e a liquidação de contas.
- **[Contratos e Faturamento](Contrato - Index.md):** Gestão de fornecedores e itens contratados.

### 3. ⛽ Sustentabilidade e Manutenção
- **[Abastecimento](Abastecimento - Index.md):** Controle de consumo e importação massiva de cupons.
- **[Manutenção e Oficina](Manutencao - Gestao.md):** Ciclo de vida de Ordens de Serviço e higienização.

### 4. 🔒 Infraestrutura e Segurança
- **[Identidade e Acesso](Usuarios - Index.md):** Gestão de permissões, perfis e auditoria.
- **[Comunicação via WhatsApp](WhatsApp - Index.md):** Notificações e alertas em tempo real.

## 🛠 Padrões de Desenvolvimento

Para desenvolvedores que desejam estender o FrotiX, consultem os guias de estilo e regras mandatórias:
- **Tratamento de Erros:** Obrigatório o uso de Alerta.TratamentoErroComLinha.
- **Interface:** Uso exclusivo de ícones **Duotone** e componentes **Syncfusion**.
- **Performance:** Preferência por **Views SQL** e projeções em vez de carregar entidades completas.

---
*FrotiX 2026 - Tecnologia a serviço da mobilidade eficiente.*


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
