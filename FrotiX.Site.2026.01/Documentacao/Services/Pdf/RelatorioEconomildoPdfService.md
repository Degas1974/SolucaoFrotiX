# RelatorioEconomildoPdfService.cs

> **Última Atualização**: 14/01/2026
> **Versão Atual**: 1.1

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral
Serviço unificado para **geração de PDFs do Dashboard Economildo** usando QuestPDF. Gera relatórios PDF de diversos gráficos e análises do sistema Economildo (transporte público interno).

## Localização
`Services/Pdf/RelatorioEconomildoPdfService.cs`

## Dependências
- `QuestPDF` (`Document`, `PageSizes`, `Fluent API`)
- `FrotiX.Services.Pdf` (DTOs: `HeatmapDto`, `GraficoBarrasDto`, etc.)

## Características

### QuestPDF
- Biblioteca moderna para geração de PDFs
- API fluente e declarativa
- Licença Community (gratuita)

### Tipos de Relatórios Suportados
1. **Heatmap Viagens**: Mapa de calor de viagens por dia/hora
2. **Heatmap Passageiros**: Mapa de calor de passageiros por dia/hora
3. **Usuários por Mês**: Gráfico de barras vertical
4. **Usuários por Turno**: Gráfico de pizza
5. **Comparativo MOB**: Tabela comparativa (PGR, Rodoviária, Cefor)
6. **Usuários Dia da Semana**: Gráfico de barras horizontal
7. **Distribuição Horário**: Gráfico de barras vertical
8. **Top Veículos**: Ranking com barras

---

## Métodos Principais

### Heatmaps
- `GerarHeatmapViagens(HeatmapDto)`: Heatmap verde para viagens
- `GerarHeatmapPassageiros(HeatmapDto)`: Heatmap amarelo/laranja para passageiros
- `GerarHeatmapBase()`: Método base compartilhado

### Gráficos de Barras
- `GerarUsuariosMes(GraficoBarrasDto)`: Barras verticais
- `GerarUsuariosDiaSemana(GraficoBarrasDto)`: Barras horizontais
- `GerarDistribuicaoHorario(GraficoBarrasDto)`: Barras verticais
- `GerarTopVeiculos(GraficoBarrasDto)`: Ranking com barras

### Gráficos de Pizza
- `GerarUsuariosTurno(GraficoPizzaDto)`: Pizza com cores por turno

### Comparativos
- `GerarComparativoMob(GraficoComparativoDto)`: Tabela comparativa

---

## Componentes Compartilhados

### `HeaderPadrao()`
Cabeçalho padrão com:
- Ícone SVG do ônibus (sem fundo, apenas o ícone)
- Título e subtítulo
- Período do filtro (dinâmico conforme seleção)
- Branding "FrotiX Intelligence"

### `FooterPadrao()`
Rodapé com:
- "Câmara dos Deputados" + data/hora
- Numeração de páginas (Página X de Y)

### `BoxEstatistica()`
Caixa de estatística com:
- Ícone SVG
- Valor destacado
- Label descritivo

---

## Cores e Estilo

### Cores Padrão (Paleta Terracota Economildo)
- Primary: `#b45a3c` (terracota)
- Secondary: `#c96d4e` (terracota claro)
- Texto: `#1e293b` (cinza escuro)
- Texto Light: `#64748b` (cinza)

### Cores MOB
- PGR: `#3b82f6` (azul)
- Rodoviária: `#f97316` (laranja)
- Cefor: `#8b5cf6` (roxo)

### Cores Turno
- Manhã: `#3b82f6` (azul)
- Tarde: `#f97316` (laranja)
- Noite: `#8b5cf6` (violeta)

---

## Contribuição para o Sistema FrotiX

### 📄 Exportação de Relatórios
- Permite exportar análises do Dashboard Economildo para PDF
- Formato profissional e padronizado
- Fácil compartilhamento e impressão

### 🎨 Consistência Visual
- Design consistente entre todos os relatórios
- Cores e estilos padronizados
- Branding da Câmara dos Deputados

## Observações Importantes

1. **QuestPDF License**: Usa licença Community (gratuita). Para uso comercial avançado, considere licença comercial.

2. **SVG Icons**: Usa `SvgIcones` para ícones FontAwesome. Garanta que ícones estejam definidos.

3. **Performance**: Geração de PDF pode ser lenta para grandes volumes. Considere cache ou geração assíncrona.

4. **Formato**: Todos os PDFs são A4 (portrait ou landscape conforme necessário).

## Arquivos Relacionados
- `Services/Pdf/RelatorioEconomildoDto.cs`: DTOs para relatórios
- `Services/Pdf/SvgIcones.cs`: Ícones SVG
- `Controllers/RelatoriosController.cs`: Usa serviço para exportar PDFs

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [14/01/2026] - Remoção do fundo do ícone do ônibus

**Descrição**:
- Removido `Background(CorPrimary)` do ícone do ônibus no método `HeaderPadrao()`
- O ícone agora é exibido sem o quadrado terracota de fundo
- Mantém apenas o SVG do ícone para visual mais limpo

**Arquivos Afetados**:
- `Services/Pdf/RelatorioEconomildoPdfService.cs` (linha 464)

**Código Alterado**:
```csharp
// ANTES
row.ConstantItem(50).AlignMiddle().AlignCenter().Background(CorPrimary).Padding(8).Svg(SvgIcones.Bus);

// DEPOIS
row.ConstantItem(50).AlignMiddle().AlignCenter().Padding(8).Svg(SvgIcones.Bus);
```

**Impacto**: Header de todos os PDFs do Dashboard Economildo exibem ícone sem borda

**Status**: ✅ **Concluído**

**Versão**: 1.1

---

**Última atualização**: 14/01/2026
**Autor**: Sistema FrotiX
**Versão**: 1.1


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
