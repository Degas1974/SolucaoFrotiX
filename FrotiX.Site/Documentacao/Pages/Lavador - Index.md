# Gestão de Lavadores e Controle de Higienização

A gestão de **Lavadores** no FrotiX é um componente essencial para a longevidade da frota e a conformidade com as normas de higiene do Estado. O LavadorController gerencia esses profissionais, vinculando-os a fornecedores e contratos específicos, o que permite o rastreio rigoroso de quem realizou cada serviço de limpeza.

## 🧼 Responsabilidades e Fluxo Operacional

Diferente de outros colaboradores, o lavador tem um vínculo direto com a **Garantia de Qualidade** (Glosas). A listagem principal do FrotiX consolida o histórico de quem está ativo em cada pátio:

### Pontos de Atenção na Implementação:

1.  **Bloqueio de Exclusão (Integridade de Contrato):** 
    O sistema proíbe a remoção de um lavador que esteja nominalmente citado em qualquer contrato de prestação de serviço. No método Delete, a tabela LavadorContrato é consultada para garantir que nenhum histórico de auditoria seja perdido.
    
2.  **Identificação Visual (Foto de Perfil):**
    O controlador fornece métodos dedicados (PegaFoto e PegaFotoModal) que convertem os dados binários do banco em Base64 para exibição instantânea na interface, facilitando a fiscalização presencial.

3.  **Gestão de Status:**
    A desativação (Inativo) é preferível à exclusão. O método UpdateStatusLavador gerencia essa transição, garantindo que o lavador pare de aparecer em novas escalas, mas permaneça nos registros de serviços já concluídos.

## 🛠 Snippets de Lógica Principal

### Consulta com Identificação de Fornecedor
Este código demonstra como o FrotiX mapeia o lavador através do contrato até chegar à empresa fornecedora:

`csharp
var result = (
    from l in _unitOfWork.Lavador.GetAll()
    join ct in _unitOfWork.Contrato.GetAll() on l.ContratoId equals ct.ContratoId into ctr
    from ctrResult in ctr.DefaultIfEmpty() 
    join f in _unitOfWork.Fornecedor.GetAll() on (ctrResult == null ? Guid.Empty : ctrResult.FornecedorId) equals f.FornecedorId into frd
    from frdResult in frd.DefaultIfEmpty()
    select new {
        l.Nome,
        ContratoLavador = ctrResult != null 
            ? $"{ctrResult.AnoContrato}/{ctrResult.NumeroContrato} - {frdResult.DescricaoFornecedor}"
            : "<b>(Sem Contrato)</b>"
    }
).ToList();
`

## 📝 Notas de Implementação (Padrão FrotiX)

- **Tratamento de Exceções:** Todas as Actions são protegidas por blocos 	ry-catch, utilizando a ferramenta global de logging Alerta.TratamentoErroComLinha para facilitar o debug em ambiente de produção.
- **Retornos Normalizados:** Em caso de erro em consultas, o controlador retorna uma View() padronizada ou um objeto JSON vazio, evitando que a interface do usuário (Syncfusion/DataTables) trave ou exiba erros técnicos brutos.
- **Performance de Imagens:** O processamento de fotos é feito de forma sob demanda, evitando que o carregamento da lista principal fique lento devido ao peso das imagens binárias.
