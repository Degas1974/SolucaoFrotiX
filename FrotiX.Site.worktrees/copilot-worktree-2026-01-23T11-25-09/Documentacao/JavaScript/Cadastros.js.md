# Cadastros (JavaScript) - Inteligência de Formulários

Os scripts em wwwroot/js/cadastros/ adicionam a "vida" aos formulários do sistema, gerenciando validações complexas e comportamentos dinâmicos.

## O Que É?
Um conjunto de scripts específicos para cada entidade (Veículo, Motorista, Viagem, etc.) que gerenciam a interação do usuário na interface de inserção e edição.

## Por Que Existe?
Para melhorar a UX, prevenindo erros antes mesmo do formulário ser enviado ao servidor e automatizando o preenchimento de campos dependentes.

## Como Funciona?

### 1. Padrão de Inicialização
Quase todos seguem o padrão (function() { ... })() para evitar poluição do escopo global, iniciando no $(document).ready.

### 2. Comportamentos Comuns
- **Dependent Dropdowns:** Ao selecionar uma Marca, o script dispara GetModeloList(id) para filtrar os modelos compatíveis via AJAX.
- **Uploads de Documentos:** Gerenciados via inputs ocultos. O JS valida o tamanho do arquivo (limite de 10MB) e a extensão (PDF/JPG/PNG) antes do upload.
- **Visibilidade Condicional:** Exemplo em eiculo_upsert.js, onde campos de Contrato ou Ata são exibidos/escondidos dependendo da natureza do veículo (Próprio vs. Terceiro).

## Scripts Principais

### veiculo_upsert.js
Gerencia a complexidade de vincular um veículo a um Contrato ou Ata de Registro de Preço. Se o veículo for "Próprio", o script desativa automaticamente as seleções de fornecedor.

### ViagemUpsert.js
Controla a lógica de quilometragem. Valida se o KM de retorno é maior que o de saída e gerencia os modais de seleção de passageiros e escalas.

## Detalhes Técnicos (Desenvolvedor)
- **Tratamento de Erros:** Todos os métodos são envolvidos em 	ry-catch chamando Alerta.TratamentoErroComLinha.
- **Integração Backend:** Utilizam predominantemente $.getJSON ou $.ajax apontando para Actions que retornam JsonResult.


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
