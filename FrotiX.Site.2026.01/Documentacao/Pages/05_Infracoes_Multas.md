# Guia de Infrações: Gestão de Multas e Penalidades

Este módulo é dedicado ao acompanhamento de infrações de trânsito, desde a autuação inicial até o pagamento e identificação do condutor.

## 📑 Ciclo da Infração (Pages/Multa)
1.  **Autuação:** Registro inicial da infração. O sistema permite o upload do PDF da notificação.
2.  **Identificação:** Vinculação automática do motorista que estava em posse do veículo no dia e hora exata da infração (cruzamento com o módulo de Viagens).
3.  **Penalidade:** Transformação da autuação em multa real com código de barras e valor.

## 🔍 Visualização e Eficiência
- **PDF Viewer Integrado:** O FrotiX possui um componente de visualização de PDF que permite ao gestor ler a notificação e o comprovante de pagamento sem baixar o arquivo.
- **Órgãos Autuantes:** Cadastro centralizado de prefeituras, Detran e órgãos federais para padronização de destinos de pagamento.

## 🛠 Detalhes Técnicos
- **Cross-Reference:** A lógica de VincularViagemId busca na tabela de Viagens quem era o motorista logado no momento da infração, reduzindo o trabalho manual do setor jurídico.
- **Gestão de Prazos:** Alertas SignalR avisam os gestores sobre multas próximas ao vencimento do desconto de 20%/40%.


## 📂 Arquivos do Módulo (Listagem Completa)

### 📑 Gestão de Multas (Core)
- Pages/Multa/ListaAutuacao.cshtml & .cs: Central de gestão de notificações de infrações.
- Pages/Multa/UpsertAutuacao.cshtml & .cs: Registro detalhado e identificação automático do condutor.
- Pages/Multa/ListaPenalidade.cshtml & .cs: Controle de multas impostas e faturadas.
- Pages/Multa/UpsertPenalidade.cshtml & .cs: Detalhamento de valores, descontos e vencimentos.
- Pages/Multa/PreencheListas.cshtml & .cs: Utilitário para carga de dados rápidos e correções em massa.

### 📄 Documentos e PDFs
- Pages/Multa/UploadPDF.cshtml & .cs: Lógica de processamento e armazenamento de anexos fiscais.
- Pages/Multa/ExibePDFAutuacao.cshtml & .cs: Visualizador de notificação.
- Pages/Multa/ExibePDFPenalidade.cshtml & .cs: Visualizador de multa.
- Pages/Multa/ExibePDFComprovante.cshtml & .cs: Visualizador de pagamento.

### ⚙️ Parametrização e Suporte
- Pages/Multa/ListaTiposMulta.cshtml & .cs / UpsertTipoMulta.cshtml & .cs: Cadastro de códigos de infração (CTB).
- Pages/Multa/ListaOrgaosAutuantes.cshtml & .cs / UpsertOrgaoAutuante.cshtml & .cs: Cadastro de órgãos emissores (Detran, PRF).
- Pages/Multa/ListaEmpenhosMulta.cshtml & .cs / UpsertEmpenhosMulta.cshtml & .cs: Vínculo financeiro para quitação de multas de frota própria.


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
