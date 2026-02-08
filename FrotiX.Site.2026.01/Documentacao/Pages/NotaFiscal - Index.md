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
var objFromDb = \_unitOfWork.NotaFiscal.GetFirstOrDefault(u => u.NotaFiscalId == model.NotaFiscalId);
var empenho = \_unitOfWork.Empenho.GetFirstOrDefault(u => u.EmpenhoId == objFromDb.EmpenhoId);

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

---

# PARTE 2: LOG DE MODIFICAÃ‡Ã•ES/CORREÃ‡Ã•ES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [21/01/2026] - Documentação: JSDoc Inline nos Scripts JavaScript

**Descrição**: Adição de documentação JSDoc completa nos scripts JavaScript embarcados no arquivo CSHTML.

**Arquivos Afetados**:

- Pages/NotaFiscal/Index.cshtml

**Mudanças**:

- ✅ Adicionado bloco de cabeçalho JSDoc com descrição geral, dependências e metadados
- ✅ Documentadas 11 funções: moeda, renderActionButtons, getDataTableConfig, loadListaContratos, loadListaAtas, resetNFTable, GetEmpenhoList, GetEmpenhoListAta, atualizarPreviewGlosa, parseCurrencyBR
- ✅ Cada função agora possui @@description, @@param e @@returns conforme padrão FrotiX

**Motivo**:

- Padronização da documentação inline de JavaScript
- Facilitar manutenção e compreensão do código
- Conformidade com RegrasDesenvolvimentoFrotiX.md

**Impacto**:

- Nenhum impacto funcional (apenas comentários)
- Melhoria significativa na legibilidade do código

**Status**: ✅ **Concluído**

**Responsável**: Equipe de Desenvolvimento

**Versão**: Documentação

---

## [19/01/2026] - AtualizaÃ§Ã£o: ImplementaÃ§Ã£o de MÃ©todos com Tracking Seletivo

**DescriÃ§Ã£o**: MigraÃ§Ã£o de chamadas .AsTracking() para novos mÃ©todos GetWithTracking() e GetFirstOrDefaultWithTracking() como parte da otimizaÃ§Ã£o de performance do Entity Framework Core.

**Arquivos Afetados**:

- Este arquivo (uso dos novos mÃ©todos do repositÃ³rio)
- Repository/IRepository/IRepository.cs (definiÃ§Ã£o dos novos mÃ©todos)
- Repository/Repository.cs (implementaÃ§Ã£o)
- RegrasDesenvolvimentoFrotiX.md (seÃ§Ã£o 4.2 - nova regra permanente)

**MudanÃ§as**:

- âŒ **ANTES**: \_unitOfWork.Entity.AsTracking().Get(id) ou \_unitOfWork.Entity.AsTracking().GetFirstOrDefault(filter)
- âœ… **AGORA**: \_unitOfWork.Entity.GetWithTracking(id) ou \_unitOfWork.Entity.GetFirstOrDefaultWithTracking(filter)

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
