# Guia Financeiro: Contratos, Atas e Glosas

O FrotiX não é apenas logístico, é um ERP financeiro de frotas. Este módulo garante a integridade dos pagamentos e a auditoria dos contratos públicos ou privados.

## 📄 Gestão de Contratos e Itens (Pages/Contrato)
Os contratos são a base legal para a existência dos ativos.
- **Itens Contratuais:** Cada veículo locado ou motorista terceirizado está vinculado a um item específico do contrato, com valores de diária e quilometragem pré-definidos.
- **Repactuação de Preços:** Ferramenta dedicada para aplicar reajustes anuais ou emergenciais em massa nos itens do contrato, mantendo o histórico de valores.

## 🛠 Auditoria e Glosas (Pages/Manutencao/Glosas)
O "pulo do gato" do sistema. O módulo de Glosas analisa automaticamente se um veículo locado ficou parado por manutenção além do tempo permitido em contrato.
- **Cálculo Automático de Desconto:** Se o contrato prevê substituição em 24h e o veículo ficou 48h parado, o sistema gera uma glosa financeira para abater da fatura do fornecedor.

## 📝 Atas de Registro de Preço (Pages/AtaRegistroPrecos)
Gerencia o saldo de itens registrados. O sistema abate o "empenho" de cada item conforme novos veículos ou serviços são ativados, evitando extrapolamento de teto orçamentário.

## 🛠 Detalhes Técnicos
- **Transaction Safety:** As repactuações de preços utilizam transações SQL via UnitOfWork para garantir que, se um veículo falhar no reajuste, nenhum seja alterado, mantendo a consistência.
- **Geração de PDF:** Relatórios de glosa e itens por unidade utilizam a biblioteca Stimulsoft para gerar documentos prontos para instrução de processos de pagamento.


## 📂 Arquivos do Módulo (Listagem Completa)

### 📄 Contratos e Itens
- Pages/Contrato/Index.cshtml & .cs: Quadro geral de contratos ativos e prazos de vigência.
- Pages/Contrato/Upsert.cshtml & .cs: Gestão de cláusulas contratuais e dados da contratada.
- Pages/Contrato/ItensContrato.cshtml & .cs: Detalhamento de valores de diária, KM excedente e especificações técnicas.
- Pages/Contrato/RepactuacaoContrato.cshtml & .cs: Interface para reajuste monetário e atualização de valores contratuais.

### 📝 Atas e Empenhos
- Pages/AtaRegistroPrecos/Index.cshtml & .cs / Upsert.cshtml & .cs: Cadastro e controle de Atas (ARPs).
- Pages/Empenho/Index.cshtml & .cs / Upsert.cshtml & .cs: Lançamento e controle de reserva orçamentária vinculada a contratos.

### 🧾 Documentos Fiscais
- Pages/NotaFiscal/Index.cshtml & .cs / Upsert.cshtml & .cs: Gestão de faturamento, liquidação e vínculos com empenhos.

### 🛠️ Auditoria e Manutenção (Conexão Financeira)
- Pages/Manutencao/Glosas.cshtml & .cs: Motor de cálculo de descontos por indisponibilidade de veículo.


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
