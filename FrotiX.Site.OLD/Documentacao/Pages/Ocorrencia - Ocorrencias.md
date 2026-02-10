# Gestão de Ocorrências e Eventos de Viagem

As **Ocorrências** no FrotiX representam as intercorrências registradas durante ou após as viagens, cobrindo desde sinistros leves até falhas operacionais críticas. O OcorrenciaController atua como o centralizador destes registros, fornecendo filtros avançados para que a gestão de frota possa tomar decisões rápidas sobre consertos e penalidades.

## ⚠️ Monitoramento e Triagem

O sistema processa ocorrências vindas de diversos canais (Mobile, Web, Vistoria). A inteligência do controlador reside em como ele filtra o mar de dados da ViewViagens para extrair apenas o que é relevante para o usuário:

### Pontos de Atenção na Implementação:

1.  **Parsing de Datas Multi-Cultura:** 
    O controlador implementa um motor de TryParse robusto que aceita formatos brasileiros (pt-BR) e internacionais (InvariantCulture). Isso garante que filtros de datas vindos de diferentes navegadores ou integrações de terceiros nunca quebrem a consulta.
    
2.  **Filtros Combinados (Veículo + Motorista + Status):**
    O método Get utiliza lógica incremental de filtros no SQL via IQueryable. Se nenhum filtro for aplicado, o sistema assume uma carga reduzida para manter a performance, filtrando apenas registros que contenham um ResumoOcorrencia válido.

3.  **Conversão de Conteúdo HTML:**
    O sistema armazena descrições ricas (Rich Text). O método Servicos.ConvertHtml é utilizado para limpar tags e fornecer um resumo legível e seguro para exibição em Grids e Tooltips frotistas.

## 🛠 Snippets de Lógica Principal

### Motor de Parsing de Datas Flexível
Este trecho exemplifica como o FrotiX lida com a entrada de datas de forma agnóstica à cultura do cliente:

`csharp
var formats = new[] { "dd/MM/yyyy", "dd/MM/yyyy HH:mm", "yyyy-MM-dd", "yyyy-MM-ddTHH:mm:ss" };
bool TryParse(string s, out DateTime dt) => 
    DateTime.TryParseExact(s.Trim(), formats, new CultureInfo("pt-BR"), DateTimeStyles.None, out dt) || 
    DateTime.TryParseExact(s.Trim(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);

if (!string.IsNullOrWhiteSpace(dataInicial) && TryParse(dataInicial, out var di)) dtIni = di;
`

## 📝 Notas de Implementação (Padrão FrotiX)

- **Modo Debug:** O controlador expõe um parâmetro debug=1 que retorna os metadados dos filtros aplicados, essencial para identificar por que uma ocorrência específica não está aparecendo em uma busca complexa.
- **Performance IQueryable:** Utiliza GetAllReducedIQueryable com sNoTracking: true, minimizando o consumo de memória do Entity Framework ao lidar com a gigantesca tabela de histórico de viagens.
- **Relação com Ficha de Vistoria:** Todas as ocorrências são vinculadas ao NoFichaVistoria, permitindo que o gestor clique no registro e seja levado diretamente ao laudo técnico do veículo.
