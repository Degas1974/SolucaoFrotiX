# Motor de Viagens e Operação Logística

A **Viagem** é a unidade de valor do FrotiX. É aqui que todos os dados (Veículos, Motoristas, Custos, Combustível) se encontram para gerar a operação logística. O ViagemController é disparado como o controlador mais complexo do sistema, lidando com milhares de registros e cálculos financeiros em tempo real.

## 🚀 Inteligência de Operação

O sistema de viagens não é apenas um registro de logs, mas um motor de inteligência que calcula o TCO (*Total Cost of Ownership*) de cada deslocamento.

### Funcionalidades Críticas:
1.  **Cálculo de Custo em Batch:** O FrotiX possui um algoritmo otimizado para recalcular os custos de milhares de viagens em segundos. Ele utiliza um cache em memória para evitar consultas repetitivas ao banco de dados sobre preços de combustíveis e salários.
2.  **Ficha de Vistoria Digital:** Acoplado à viagem, o sistema gerencia a imagem digitalizada da vistoria (yte[]), garantindo que qualquer avaria ou conformidade seja documentada visualmente e vinculada ao ID da viagem.
3.  **Filtros de Alta Performance:** Utiliza expressões Lambda/Linq dinâmicas (iagemsFilters) para permitir consultas simultâneas por data, placa, motorista e status sem perda de performance.

## 🛠 Snippets de Lógica Principal

### Otimização de Cálculo de Massa (Cache Singleton)
Para evitar que o cálculo de custo de 10.000 viagens faça 50.000 conexões ao banco, utilizamos o padrão de Cache de Dados Compartilhados:

`csharp
private class DadosCalculoCache {
    public Dictionary<Guid, double> ValoresCombustivel { get; set; } = new Dictionary<Guid, double>();
    public Dictionary<Guid, MotoristaInfo> InfoMotoristas { get; set; } = new Dictionary<Guid, MotoristaInfo>();
    // ... outros dados carregados UMA VEZ
}

// No Batch, carregamos tudo antes do Loop
var cache = await CarregarDadosCalculoCache();
foreach (var viagem in batch) {
    CalcularCustosViagem(viagem, cache); // Cálculo puramente em memória!
}
`

### Gestão Visual (Ficha de Vistoria)
O controlador lida com o upload e conversão de Base64 para garantir que a interface (Index.cshtml) possa mostrar a imagem sem precisar salvar em disco físico, mantendo tudo no banco para segurança e portabilidade.

## 📝 Notas de Implementação

- **Status "Realizada":** Apenas viagens marcadas como Realizadas entram no motor de cálculo de custos. Isso evita distorções financeiras em agendamentos futuros ou cancelados.
- **Integração com Eventos:** O ViagemEventoController permite anexar ocorrências (quebras, acidentes) diretamente à viagem, afetando os indicadores de disponibilidade do DashboardEventos.
- **Precisão de KM:** O sistema valida o KmInicial e KmFinal. Se a diferença for negativa ou excessiva (fora do padrão do veículo), um alerta é gerado no módulo de Auditoria.

---
*Documentação gerada para a Solução FrotiX 2026. Este controlador é central para a operação do sistema.*
