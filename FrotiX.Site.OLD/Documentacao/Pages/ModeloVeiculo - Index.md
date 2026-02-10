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
