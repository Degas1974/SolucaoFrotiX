# Gestão de Modelos de Veículos

Enquanto a Marca define o fabricante, o **Modelo** define a capacidade, o tipo de combustível padrão e a categoria do veículo. O ModeloVeiculoController é o responsável por gerenciar estas definições, servindo de base para o cadastro detalhado de cada placa da frota.

## 🚗 O Elo com os Veículos (Ativos)

O modelo é a peça central que une as especificações de engenharia (Marca) aos ativos reais (Veículos). 

### Pontos de Atenção na Implementação:

1.  **Carregamento de Relacionamentos (Eager Loading):** 
    Diferente da Marca, a listagem de modelos utiliza o parâmetro includeProperties: "MarcaVeiculo" no GetAll. Isso garante que a Grid exiba o nome do fabricante sem precisar de múltiplas consultas ao banco, otimizando o tempo de resposta.
    
2.  **Proteção de Ativos Reais:**
    O sistema proíbe a exclusão de um modelo se houver pelo menos um **Veículo** cadastrado com ele. Esta é uma regra de negócio crítica para garantir que os cálculos de depreciação e manutenção nunca percam sua referência técnica.

3.  **Flexibilidade de Status:**
    Modelos de veículos que saem de linha podem ser inativados, impedindo sua seleção em novos cadastros, mas permanecendo ativos para consulta em veículos que ainda compõem a frota.

## 🛠 Snippets de Lógica Principal

### Consulta com Injeção de Propriedades (Eager Loading)
Exemplo de como o repositório traz a marca vinculada de forma otimizada:

`csharp
[HttpGet]
public IActionResult Get()
{
    // O parâmetro "MarcaVeiculo" garante que o JOIN seja feito no SQL
    var data = _unitOfWork.ModeloVeiculo.GetAll(includeProperties: "MarcaVeiculo");
    return Json(new { data = data });
}
`

## 📝 Notas de Implementação (Padrão FrotiX)

- **Tratamento de Erros:** Padronizado com 	ry-catch e Alerta.TratamentoErroComLinha, garantindo que falhas em cascata sejam rastreadas até a linha exata no controlador.
- **UI Feedback:** Todas as operações de deleção ou alteração de status retornam mensagens de sucesso/erro que são interpretadas pelo componente SweetAlert do frontend FrotiX.
- **Integridade de Dados:** A verificação de ativos (veículos) antes da deleção é feita via GetFirstOrDefault, evitando o processamento desnecessário de listas completas.


---

# PARTE 2: LOG DE MODIFICAÃ‡Ã•ES/CORREÃ‡Ã•ES

## [21/01/2026] - PadronizaÃ§Ã£o de Nomenclatura

**DescriÃ§Ã£o**: Renomeada coluna "AÃ§Ã£o" para "AÃ§Ãµes" no cabeÃ§alho do DataTable para padronizaÃ§Ã£o do sistema

**Arquivos Afetados**:
- Arquivo .cshtml correspondente

**Impacto**: AlteraÃ§Ã£o cosmÃ©tica, sem impacto funcional

**Status**: âœ… **ConcluÃ­do**

**ResponsÃ¡vel**: Sistema

**VersÃ£o**: Atual

---

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
