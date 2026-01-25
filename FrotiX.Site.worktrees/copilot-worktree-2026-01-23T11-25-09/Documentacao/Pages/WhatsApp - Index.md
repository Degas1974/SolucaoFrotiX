# Integração e Comunicação via WhatsApp

A comunicação em tempo real é vital para a logística de frotas. O módulo de **WhatsApp** do FrotiX permite que motoristas e gestores recebam alertas, comprovantes e ordens de serviço diretamente em seus dispositivos móveis através de uma API desacoplada e assíncrona.

## 📱 Hub de Comunicação

Ao contrário de sistemas de mensagem simples, o WhatsAppController gerencia sessões ativas e permite o envio de mídia complexa.

### Funcionalidades:
1.  **Gestão de Sessões (QR Code):** O controlador gera e fornece o QR Code em formato Base64 (data:image/png;base64,...) para que o pairing seja feito instantaneamente na tela do sistema.
2.  **Mensageria Multimodal:** Suporte para envio de texto simples e mídia (PDFs de multas, fotos de vistorias ou comprovantes de abastecimento).
3.  **Filas Assíncronas:** Todo o processamento utiliza CancellationToken e tarefas assíncronas para garantir que oscilações na rede do WhatsApp não travem a interface do usuário do FrotiX.

## 🛠 Snippets de Lógica Principal

### Captura de QR Code para Pareamento
A lógica de frontend solicita o QR Code, e o controlador garante que o prefixo Base64 esteja correto para renderização imediata em tags <img>:

`csharp
[HttpGet("qr")]
public async Task<IActionResult> Qr([FromQuery] string session, CancellationToken ct) {
    var b64 = await _wa.GetQrBase64Async(session, ct);
    if (string.IsNullOrWhiteSpace(b64)) return NotFound();
    
    // Garantia de prefixo data URI para renderização direta
    if (!b64.StartsWith("data:")) b64 = "data:image/png;base64," + b64;
    
    return Ok(new { success = true, qrcode = b64 });
}
`

## 📝 Notas de Implementação

- **Persistência de Sessão:** O sistema tenta manter a sessão ativa em background. Caso o motorista perca a conexão, o estado é atualizado no endpoint /status para que o gestor possa intervir.
- **Formatação de Telefone:** O sistema utiliza o padrão E.164 (Ex: +5511999999999) internamente, convertendo inputs do usuário para garantir que as mensagens cheguem ao destino globalmente.
- **Módulo Desacoplado:** A lógica de envio está em um serviço separado (IWhatsAppService), permitindo que as API Keys e segredos de conexão fiquem isolados do código-fonte principal.

---
*Documentação gerada para a Solução FrotiX 2026.*


---

# PARTE 2: LOG DE MODIFICAÃ‡Ã•ES/CORREÃ‡Ã•ES

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
