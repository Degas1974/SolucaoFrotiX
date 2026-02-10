# Gestão de Patrimônio e Movimentação de Ativos

A gestão de **Patrimônio** no FrotiX vai além do controle de veículos; ela abrange todo o inventário de equipamentos e bens móveis ligados à frota. O PatrimonioController é o cérebro por trás da rastreabilidade desses bens, gerenciando transferências entre setores, conferências físicas e o histórico completo de posse.

## 📦 Inventário e Rastreabilidade

O sistema utiliza a **ViewPatrimonioConferencia** para fornecer uma fotografia instantânea do estado real do patrimônio. Esta view consolida dados de diversos módulos para apresentar:
- **NPR (Número de Patrimônio):** A chave de identificação única.
- **Localização Atual:** Mapeamento exato entre Setores e Seções Patrimoniais.
- **Filtros Inteligentes:** O controlador suporta consultas multi-parâmetro (Marca, Modelo, Setor, Seção, Situação) para auditorias rápidas.

### O Fluxo de Movimentação (Asset Tracking):

1.  **Transferência de Posse:** 
    A funcionalidade CreateMovimentacao registra a saída de um bem de um setor/seção para outro. Este processo é atomicamente seguro, garantindo que o bem não "desapareça" do sistema durante a transferência.
    
2.  **Prevenção de Duplicidade:**
    Devido ao alto volume de operações simultâneas, o controlador implementa um mecanismo de **Lock e RequestKey**, impedindo que um clique duplo no botão de salvar gere duas movimentações idênticas para o mesmo bem no mesmo segundo.

3.  **Auditoria Operacional:**
    Cada movimentação armazena o usuário responsável (via ClaimsPrincipal), a data exata e os IDs de origem e destino, criando uma trilha de auditoria (Auditing Trail) inquebrável.

## 🛠 Snippets de Lógica Principal

### Proteção Contra Requisições Duplicadas (Concorrência)
Este padrão de design é aplicado em fluxos críticos de patrimônio para garantir a sanidade dos dados:

`csharp
private static readonly HashSet<string> _processandoRequests = new HashSet<string>();
private static readonly object _lockObject = new object();

[HttpPost("CreateMovimentacao")]
public IActionResult CreateMovimentacao([FromBody] MovimentacaoPatrimonioDto dto)
{
    var requestKey = $"{dto.PatrimonioId}_{dto.DataMovimentacao?.ToString("yyyyMMddHHmmss")}";
    
    lock (_lockObject) {
        if (_processandoRequests.Contains(requestKey)) return Json(new { success = false, message = "Requisição já processando" });
        _processandoRequests.Add(requestKey);
    }
    
    try {
        // Lógica de gravação no banco...
    } finally {
        lock (_lockObject) { _processandoRequests.Remove(requestKey); }
    }
}
`

## 📝 Notas de Implementação (Padrão FrotiX)

- **Desempenho de Carga (Eager Search):** As buscas por movimentações específicas (GetMovimentacao) realizam múltiplos carregamentos de entidades relacionadas (SetorOrigem, SecaoDestino, etc.) para entregar um objeto completo ao frontend, evitando o problema de N+1 consultas.
- **Tratamento de Exceções Verboso:** Em caso de erro, o controlador retorna mensagens detalhadas (x.Message), facilitando a identificação de problemas de integridade sem precisar acessar logs de servidor.
- **Segurança de Usuário:** O controlador extrai a identidade do usuário através de ClaimTypes.NameIdentifier, garantindo que o registro de "Quem fez" seja automático e impossível de burlar pelo frontend.
