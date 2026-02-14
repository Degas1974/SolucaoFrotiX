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
