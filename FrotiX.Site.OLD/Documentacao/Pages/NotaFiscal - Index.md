# Gestão de Notas Fiscais e Liquidação Financeira

O processamento de **Notas Fiscais** (NF) no FrotiX é a etapa final do ciclo financeiro, onde o serviço prestado é validado e o pagamento é autorizado. O NotaFiscalController gerencia a entrada destes documentos, vinculando-os a empenhos específicos e tratando as glosas (deduções) de forma automatizada.

## 💳 Fluxo de Caixa e Ajuste de Empenho

Diferente de um simples registro de recibo, a integração da NF no FrotiX é bidirecional com o saldo do empenho:

### Pontos de Atenção na Implementação:

1.  **Sincronização de Saldo Líquido:** 
    Ao registrar uma NF, o valor é debitado do empenho. Contudo, ao **Excluir** uma NF, o sistema devolve o valor líquido (ValorNF - ValorGlosa) ao saldo final do empenho automaticamente, evitando discrepâncias contábeis.
    
2.  **Lógica de Glosa Inteligente:**
    O método Glosa permite dois modos de operação: **Somar** (adicionar uma nova penalidade à existente) ou **Substituir**. O sistema valida se o total glosado não excede o valor bruto da nota, garantindo a integridade dos cálculos.

3.  **Tratamento de Centavos:**
    Devido a variações de inputs em diferentes navegadores, o controlador implementa uma heurística de detecção automática para valores que chegam multiplicados por 100 (formato de centavos), corrigindo-os antes da persistência no banco.

## 🛠 Snippets de Lógica Principal

### Estorno de Saldo na Exclusão de NF
Este trecho exemplifica a responsabilidade financeira do controlador ao remover um documento fiscal:

`csharp
[HttpPost("Delete")]
public IActionResult Delete(NotaFiscalViewModel model)
{
    var objFromDb = _unitOfWork.NotaFiscal.GetFirstOrDefault(u => u.NotaFiscalId == model.NotaFiscalId);
    var empenho = _unitOfWork.Empenho.GetFirstOrDefault(u => u.EmpenhoId == objFromDb.EmpenhoId);

    if (empenho != null) {
        // Devolve ao empenho apenas o que de fato foi "consumido" (Valor NF - Glosa aplicada)
        empenho.SaldoFinal += ((objFromDb.ValorNF ?? 0) - (objFromDb.ValorGlosa ?? 0));
        _unitOfWork.Empenho.Update(empenho);
    }

    _unitOfWork.NotaFiscal.Remove(objFromDb);
    _unitOfWork.Save();
    return Json(new { success = true, message = "Nota Fiscal removida com sucesso" });
}
`

## 📝 Notas de Implementação (Padrão FrotiX)

- **Auditória de Motivos:** Toda glosa exige um MotivoGlosa, que é persistido para justificar tecnicamente o desconto ao fornecedor durante o fechamento de contas.
- **IgnoreAntiforgeryToken:** Aplicado nesta API para permitir que sistemas externos de faturamento (via integração manual/AJAX) possam postar dados sem a barreira de tokens de página, mantendo a autenticação via claims.
- **Formatadores de Interface:** O método GetGlosa já retorna o valor formatado em duas casas decimais (N2), pronto para ser exibido em labels de interface sem necessidade de tratamento JS extra.
