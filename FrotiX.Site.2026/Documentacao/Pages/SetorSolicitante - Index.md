# Gestão de Setores Solicitantes

Os **Setores Solicitantes** no FrotiX são as unidades organizacionais que têm autonomia para pedir e consumir serviços de frota. Eles formam a base da hierarquia de custos do sistema, permitindo que a gestão identifique exatamente qual órgão ou departamento está gerando maior demanda de transporte.

## 🌳 Estrutura Hierárquica

Diferente de setores patrimoniais simples, o setor solicitante no FrotiX suporta uma estrutura de auto-relacionamento (SetorPai-SetorFilho). Isso permite representar a organograma completo de uma instituição, de secretarias a sub-secretarias e gerências.

### Pontos de Atenção na Implementação:

1.  **Proteção de Árvore Logística:** 
    O sistema proíbe a exclusão de um "Setor Pai" que ainda possua "Setores Filhos" cadastrados. Esta validação proativa no método Delete impede que sub-setores fiquem isolados, mantendo a integridade da hierarquia de relatórios.
    
2.  **Módulo Parcial (Scalability):**
    O controlador é definido como partial class, fatiando responsabilidades entre Delete, GetAll e UpdateStatus. Esta arquitetura facilita a manutenção em um sistema com centenas de ações possíveis sobre a mesma entidade.

3.  **Filtragem de Ativos:**
    A maioria das consultas para alimentar dropdowns de requisição utiliza filtros de Status == true, garantindo que um setor desativado administrativamente não possa mais gerar novos gastos para o Estado.

## 🛠 Snippets de Lógica Principal

### Validação de Recursividade na Exclusão
Este código demonstra como o FrotiX impede a fragmentação da árvore organizacional:

`csharp
[HttpPost("Delete")]
public IActionResult Delete(SetorSolicitanteViewModel model)
{
    // Busca se existe algum outro setor que aponta para este como "Pai"
    var filhos = _unitOfWork.SetorSolicitante.GetFirstOrDefault(u => u.SetorPaiId == model.SetorSolicitanteId);
    
    if (filhos != null) {
        return Json(new { success = false, message = "Existem setores filho associados a esse Setor pai" });
    }

    _unitOfWork.SetorSolicitante.Remove(objFromDb);
    _unitOfWork.Save();
    return Json(new { success = true, message = "Setor Solicitante removido com sucesso" });
}
`

## 📝 Notas de Implementação (Padrão FrotiX)

- **Auditória de Operações:** Todas as falhas de exclusão ou alteração são registradas via Alerta.TratamentoErroComLinha, injetando logs contextualizados no arquivo SetorSolicitanteController.cs.
- **Performance IQueryable:** Utiliza carregamento sob demanda para os filhos, evitando o carregamento recursivo infinito na memória do servidor e processando os filtros diretamente no SQL Server.
- **Relacionamento com Requisitantes:** Esta entidade é o ponto de ancoragem para os Requisitantes, criando a ponte necessária entre a estrutura administrativa e as pessoas autorizadas a usar a frota.
