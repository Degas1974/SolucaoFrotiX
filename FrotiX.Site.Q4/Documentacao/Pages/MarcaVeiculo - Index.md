# Padronização de Marcas de Veículos

A tabela de **Marcas** é o primeiro nível de classificação da frota no FrotiX. O MarcaVeiculoController garante que todos os veículos cadastrados sigam uma nomenclatura padronizada, fundamental para gerar estatísticas de manutenção por fabricante e relatórios de desempenho de combustível.

## 🏷 Estrutura de Dependência

No FrotiX, uma Marca é a "âncora" para múltiplos Modelos. Esta hierarquia é protegida por regras rígidas de banco de dados para evitar inconsistências nos ativos do Estado.

### Pontos de Atenção na Implementação:

1.  **Cascata de Deleção Bloqueada:** 
    O sistema nunca permite excluir uma Marca que possua Modelos vinculados. O método Delete realiza esta conferência de forma proativa, retornando uma mensagem de aviso em vez de estourar um erro de chave estrangeira do SQL.
    
2.  **Status e Ativação:**
    A gestão de marcas permite desativar fabricantes antigos ou não mais utilizados em novas licitações, mantendo o histórico de veículos antigos intacto através da coluna Status.

3.  **Endpoint Simplificado:**
    O Get desta API é otimizado para preencher rapidamente Dropdowns e Select2 em toda a plataforma, fornecendo apenas os dados essencias (MarcaId e DescricaoMarca).

## 🛠 Snippets de Lógica Principal

### Validação de Dependência antes da Exclusão
Este trecho mostra como o FrotiX protege a árvore de dados antes de executar um comando de deleção:

`csharp
[HttpPost]
public IActionResult Delete(MarcaVeiculoViewModel model)
{
    // Verifica se existem modelos dependentes desta marca
    var modeloDependente = _unitOfWork.ModeloVeiculo.GetFirstOrDefault(u => u.MarcaId == model.MarcaId);
    
    if (modeloDependente != null) {
        return Json(new { success = false, message = "Existem modelos associados a essa marca" });
    }

    _unitOfWork.MarcaVeiculo.Remove(objFromDb);
    _unitOfWork.Save();
    return Json(new { success = true, message = "Marca removida com sucesso" });
}
`

## 📝 Notas de Implementação (Padrão FrotiX)

- **Auditória de Log:** Todas as mudanças de status geram descrições estruturadas (ex: "Atualizado Status da Marca [Nome: X] (Inativo)"), facilitando a leitura de logs administrativos.
- **Tratamento de Erros:** Utiliza a injeção de logs global Alerta.TratamentoErroComLinha mencionando explicitamente o arquivo MarcaVeiculoController.cs.
- **Roteamento API:** Segue o padrão RESTful pi/[controller], permitindo integração fluida com o frontend moderno do FrotiX.
