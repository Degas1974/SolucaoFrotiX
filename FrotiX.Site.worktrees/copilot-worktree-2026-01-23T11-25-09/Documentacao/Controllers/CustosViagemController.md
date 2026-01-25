# Documentação: Análise e Auditoria de Custos de Viagem (CustosViagemController)

O \CustosViagemController\ é a ferramenta definitiva para a transparência financeira da frota. Ele não apenas exibe quanto cada viagem custou, mas oferece o motor de cálculo necessário para realizar conciliações bancárias e auditorias contratuais. Este controller funciona como um "consolidado" que une dados de motoristas, veículos e combustíveis em uma única visão monetária.

## 1. Otimização de Leitura (Reduce Pattern)

Dada a alta volumetria de viagens no sistema, o endpoint principal \Get\ utiliza a técnica de **Seleção Reduzida**. Em vez de carregar todos os campos da \ViewCustosViagem\, o controller projeta apenas o essencial (KMs, custos específicos, IDs de referência), o que reduz drasticamente o consumo de memória do servidor e o tempo de transferência para o browser.

\\\csharp
// Projeção otimizada para garantir que grids de milhares de linhas carreguem instantaneamente
var objCustos = _unitOfWork.ViewCustosViagem.GetAllReduced(selector: v => new
{
    v.ViagemId,
    v.DataInicial,
    v.Quilometragem,
    v.CustoMotorista,
    v.CustoCombustivel,
    v.CustoVeiculo,
    v.NomeMotorista,
    v.DescricaoVeiculo
});
\\\

## 2. Processamento em Lote (Batch Calculation)

Um recurso vital deste controller é o \CalculaCustoViagens\. Ele permite que o administrador recalcule retroativamente os custos de todas as viagens "Realizadas". Isso é fundamental quando uma Ata de Registro de Preços sofre uma repactuação retroativa ou quando um erro de lançamento de contrato é corrigido; com um único comando, o sistema realinha os custos de centenas de viagens com os novos valores de contrato.

## 3. Filtragem por Dimensões

Para facilitar o trabalho das setoriais, o controller oferece múltiplos endpoints de filtragem (\ViagemVeiculos\, \ViagemMotoristas\, \ViagemSetores\, etc.). Isso permite que um gestor de determinado setor veja apenas os custos gerados pela sua unidade, ou que um encarregado de manutenção analise se um veículo específico está custando mais do que o esperado por KM rodado.

## 4. Evidência Documental (Ficha de Vistoria)

Além dos números, o controller gerencia a recuperação das **Fichas de Vistoria** através do endpoint \PegaFicha\. O sistema recupera a imagem da ficha (comprovante físico da viagem) armazenada em formato binário no banco de dados e a converte para exibição imediata no frontend, servindo como prova documental em auditorias.

---

### Notas de Implementação (Padrão FrotiX)

*   **Helper de Serviços:** A lógica pesada de cálculo não reside no controller, mas sim na classe \Servicos\, garantindo que as regras de cálculo (Custo Motorista, Custo Veículo) sejam centralizadas e reutilizáveis.
*   **IgnoreAntiforgeryToken:** Utilizado para permitir que processos de cálculo pesados disparados via AJAX não sejam interrompidos por expiração de tokens CSRF.
*   **Tratamento de Erros:** Todas as chamadas ao repositório são protegidas, retornando listas vazias e logs detalhados em caso de falha de conexão ou integridade.

---
*Documentação atualizada em 2026.01.14 conforme novo padrão de Prosa Leve.*


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
