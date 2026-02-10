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
