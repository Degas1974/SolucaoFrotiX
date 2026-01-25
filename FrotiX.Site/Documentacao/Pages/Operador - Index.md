# Gestão de Operadores de Máquinas e Equipamentos

Enquanto motoristas conduzem veículos de passeio, os **Operadores** são responsáveis pelos ativos pesados (retroescavadeiras, geradores, etc.) no ecossistema FrotiX. O OperadorController gerencia esses profissionais, garantindo que seu vínculo com fornecedores e contratos de locação seja mantido com precisão.

## 🏗 Especialização Operacional

O operador, assim como o encarregado, é um elo crítico no contrato de prestação de serviços. O FrotiX mantém um controle rigoroso sobre sua alocação pátio-contrato:

### Pontos de Atenção na Implementação:

1.  **Proteção de Chave Estrangeira Social:** 
    O sistema bloqueia a exclusão de um operador caso ele esteja vinculado a qualquer serviço ativo (OperadorContrato). Isso garante que os diários de bordo e registros de hora-máquina nunca percam a referência de quem estava no comando do equipamento.
    
2.  **Rastreabilidade de Alteração:**
    Cada registro de operador exibe quem foi o último gestor a alterar seus dados (UsuarioIdAlteracao -> NomeCompleto), criando uma camada de responsabilidade sobre os dados cadastrais.

3.  **Identificação Visual Obrigatória:**
    Através do PegaFotoModal, o sistema permite que supervisores de campo identifiquem o operador pela foto armazenada em banco, garantindo que a pessoa operando a máquina é de fato o profissional credenciado.

## 🛠 Snippets de Lógica Principal

### Captura de Foto com Conversão em Tempo Real
Este helper do controlador demonstra como os dados binários da foto são entregues à interface de forma limpa:

`csharp
[HttpGet("PegaFotoModal")]
public JsonResult PegaFotoModal(Guid id)
{
    var objFromDb = _unitOfWork.Operador.GetFirstOrDefault(u => u.OperadorId == id);
    if (objFromDb.Foto != null) {
        // Converte o byte[] para uma string Base64 consumível por tags <img>
        var base64 = this.GetImage(Convert.ToBase64String(objFromDb.Foto));
        return Json(base64);
    }
    return Json(false);
}
`

## 📝 Notas de Implementação (Padrão FrotiX)

- **Tratamento de Erros:** Todas as chamadas de banco e lógica de negócio são protegidas por 	ry-catch, registrando falhas via Alerta.TratamentoErroComLinha com metadados do arquivo OperadorController.cs.
- **Status Ativo/Inativo:** A troca de status gera uma mensagem amigável para o log do sistema, registrando o nome do operador e o novo estado (Ex: "Atualizado Status do Operador [Nome: João] (Ativo)").
- **Join de Fornecedor:** A listagem principal realiza um *Outer Join* com a tabela de Fornecedores, expondo claramente a empresa parceira responsável pelo profissional, facilitando a gestão de RH terceirizado.
