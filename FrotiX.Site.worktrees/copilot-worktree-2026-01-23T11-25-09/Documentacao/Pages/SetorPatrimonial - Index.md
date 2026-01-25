# Gestão de Setores Patrimoniais

Os **Setores Patrimoniais** representam as unidades administrativas de alto nível no FrotiX. Eles funcionam como contêineres para as **Seções** e são fundamentais para o controle de detentores de carga, garantindo que cada bem permanente tenha um responsável legal (usuário) e uma localização definida.

## 🏢 Estrutura Organizacional

No FrotiX, o setor é a unidade mínima para a qual se pode designar um **Detentor**. A integridade desta estrutura é vital para o inventário anual.

### Pontos de Atenção na Implementação:

1.  **Vínculo com Detentores (Usuários):** 
    A listagem de setores (ListaSetores) realiza um Join com a tabela AspNetUsers para identificar o detentor de carga atual. Isso permite auditorias rápidas sobre quem responde legalmente por cada área.
    
2.  **Proteção de Deleção em Cascata:**
    O sistema impede a remoção de um Setor se ele possuir qualquer **Seção Patrimonial** cadastrada. Esta barreira de negócio evita que sub-localizações e bens fiquem "órfãos" no banco de dados.

3.  **Filtragem para Combos:**
    O método ListaSetoresCombo fornece uma versão enxuta da lista, filtrando apenas setores ativos para alimentar dropdowns de movimentação patrimonial, otimizando o carregamento da interface.

## 🛠 Snippets de Lógica Principal

### Consulta de Listagem com Identificação de Responsável
Este trecho mostra como o FrotiX cruza os dados do setor com o sistema de identidade (Identity) para exibir o nome do detentor:

`csharp
[HttpGet("ListaSetores")]
public IActionResult ListaSetores()
{
    var setores = _unitOfWork.SetorPatrimonial.GetAll()
        .Join(_unitOfWork.AspNetUsers.GetAll(), setor => setor.DetentorId, usuario => usuario.Id,
            (setor, usuario) => new {
                setor.SetorId,
                setor.NomeSetor,
                usuario.NomeCompleto, // Nome do Detentor
                setor.Status
            }
        ).OrderBy(x => x.NomeSetor).ToList();
    return Json(new { success = true, data = setores });
}
`

## 📝 Notas de Implementação (Padrão FrotiX)

- **Auditória de Status:** Mudanças de estado (Ativo/Inativo) são registradas com descrições detalhadas ("Atualizado Status do Setor [Nome: X] (Ativo)"), fundamentais para trilhas de auditoria administrativa.
- **Tratamento de Erros:** Utiliza o helper global Alerta.TratamentoErroComLinha, garantindo que falhas em Joins complexos sejam capturadas com precisão técnica.
- **Integração de Cadastro:** Este controlador serve tanto à grid administrativa quanto aos fluxos de movimentação de bens permanentes.


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
