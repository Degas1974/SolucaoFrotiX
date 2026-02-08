# Cadastro de Fornecedores e Parceiros

A base de **Fornecedores** é o alicerce para todos os serviços externos do FrotiX, desde a locação de frotas pesadas até a aquisição de peças. O FornecedorController gerencia essas entidades, garantindo que apenas parceiros ativos e com contratos válidos participem da operação diária do sistema.

## 🤝 Relacionamento e Conformidade

No ecossistema FrotiX, o fornecedor não é apenas um registro de nome; ele é o "Pai" de múltiplos contratos. A integridade referencial aqui é levada ao extremo:

### Pontos de Atenção na Implementação:

1.  **Vínculo com Contratos:** 
    O sistema impede terminantemente a remoção de um fornecedor que possua qualquer contrato (ativo ou histórico) cadastrado. Isso preserva a rastreabilidade financeira e operacional de anos anteriores.
    
2.  **Status Operacional:**
    O método UpdateStatusFornecedor permite desativar um fornecedor sem removê-lo. Um fornecedor inativo é automaticamente filtrado em novas seleções de contratos e empenhos, sem quebrar o histórico de dados já existentes.

3.  **Simplicidade e Performance:**
    Diferente de outros controladores complexos, o fornecedor foca em uma listagem direta (GetAll), fornecendo dados rápidos para alimentar seletores (Select2) e Grids de consulta básica.

## 🛠 Snippets de Lógica Principal

### Toggle de Status com Feedback Amigável
Este método exemplifica como o FrotiX trata a mudança de estado de uma entidade, fornecendo mensagens de log prontas para o frontend:

`csharp
[Route("UpdateStatusFornecedor")]
public JsonResult UpdateStatusFornecedor(Guid Id)
{
    var objFromDb = _unitOfWork.Fornecedor.GetFirstOrDefault(u => u.FornecedorId == Id);
    string Description = "";

    if (objFromDb.Status == true) {
        objFromDb.Status = false;
        Description = $"Atualizado Status do Fornecedor [Nome: {objFromDb.DescricaoFornecedor}] (Inativo)";
    } else {
        objFromDb.Status = true;
        Description = $"Atualizado Status do Fornecedor [Nome: {objFromDb.DescricaoFornecedor}] (Ativo)";
    }
    
    _unitOfWork.Fornecedor.Update(objFromDb);
    _unitOfWork.Save();
    return Json(new { success = true, message = Description });
}
`

## 📝 Notas de Implementação (Padrão FrotiX)

- **Tratamento de Erros:** Segue o padrão rigoroso de 	ry-catch em todas as Actions, com registro via Alerta.TratamentoErroComLinha mencionando o arquivo FornecedorController.cs.
- **Integridade Referencial:** A verificação de contratos no método Delete é feita diretamente via Repository, garantindo que a regra de negócio seja aplicada antes de qualquer comando de SQL ser enviado ao banco.
- **API REST:** O controlador utiliza atributos de rota explícitos ([Route("api/[controller]")]), facilitando a integração com ferramentas de terceiros ou o frontend modular em jQuery.
