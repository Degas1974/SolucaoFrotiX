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
