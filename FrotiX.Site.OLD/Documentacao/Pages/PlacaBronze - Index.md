# Gestão de Identificação: Placas de Bronze

A **Placa de Bronze** é um identificador patrimonial histórico e físico utilizado para etiquetar bens permanentes no ecossistema público gerido pelo FrotiX. O PlacaBronzeController gerencia estes códigos, garantindo que cada identificador seja único e esteja corretamente associado a um veículo ou equipamento.

## 🏷 Vínculos e Desvinculações

Diferente de uma placa de rodagem comum (Mercosul), a placa de bronze é uma etiqueta de inventário. O sistema trata este vínculo como uma propriedade opcional (Nullable), mas altamente monitorada:

### Pontos de Atenção na Implementação:

1.  **Regra de Unicidade Virtual:** 
    O sistema proíbe a exclusão de uma Placa de Bronze que já esteja associada a um veículo ativo. O método Delete verifica preventivamente a tabela Veiculo antes de qualquer alteração física no banco.
    
2.  **Operação de Desvinculo:**
    Diferente de outros módulos, este controlador expõe o método Desvincula, que permite "soltar" uma placa de bronze de um veículo sem deletar nenhuma das duas entidades. Isso é útil em casos de substituição de etiquetas ou renomeação de inventário.

3.  **Toggle de Status Amigável:**
    As placas podem ser inativadas (Ex: extraviadas ou danificadas), impedindo que sejam escolhidas em novos cadastros de veículos, mas mantendo a trilha histórica.

## 🛠 Snippets de Lógica Principal

### Desassociação de Identificador
Este trecho mostra como o FrotiX limpa um identificador de forma segura, mantendo as duas entidades no banco:

`csharp
[HttpPost("Desvincula")]
public IActionResult Desvincula(PlacaBronzeViewModel model)
{
    // Localiza o veículo que está utilizando esta placa de bronze
    var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(u => u.PlacaBronzeId == model.PlacaBronzeId);
    
    if (veiculo != null) {
        veiculo.PlacaBronzeId = Guid.Empty; // Remove o vínculo, mas preserva os dados do veículo
        _unitOfWork.Veiculo.Update(veiculo);
    }
    
    return Json(new { success = true, message = "Placa de Bronze desassociada com sucesso!" });
}
`

## 📝 Notas de Implementação (Padrão FrotiX)

- **Tratamento de Erros:** Segue o padrão de 	ry-catch com registro via Alerta.TratamentoErroComLinha, assegurando que erros de deleção de chaves estrangeiras sejam reportados com clareza.
- **Join de Verificação na Listagem:** Ao listar placas de bronze (Get), o sistema faz um *Left Join* com a tabela de veículos para mostrar em tempo real qual placa do Mercosul está usando cada identificador de bronze.
- **Feedback Visual:** As mensagens de retorno são padronizadas para acionar componentes de Alerta no frontend frotista, diferenciando tipos de sucesso e erro via variável 	ype.
