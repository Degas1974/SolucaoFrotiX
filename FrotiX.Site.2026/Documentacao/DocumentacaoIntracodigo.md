# 📂 Mapa de Documentação Intra-Código CSHTML

**Data de Criação:** 03/02/2026
**Objetivo:** Rastrear documentação adicionada dentro dos arquivos .cshtml (comentários visuais + cards)

---

## 📋 Convenção de Documentação Intra-Código

### Padrão de Comentário Visual (Card de Arquivo)

**Localização:** Linhas 1-15 de cada arquivo CSHTML

```html
@* ================================================================================================
 * ⚡ ARQUIVO: Pages/[Modulo]/[Arquivo].cshtml
 * ------------------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [Descrição do propósito - 1 linha]
 * 📥 ENTRADAS     : @Model, ViewData, filtros, parâmetros de query
 * 📤 SAÍDAS       : HTML renderizado, DataTable, Modais, respostas JSON
 * 🔗 CHAMADA POR  : Menu/Route/Link que a chama
 * 🔄 CHAMA        : Controllers, APIs, JavaScript handlers
 * 📦 DEPENDÊNCIAS : Syncfusion, DataTables, Bootstrap, etc
 * 📝 OBSERVAÇÕES  : [Notas técnicas importantes]
 * ================================================================================================ *@
```

### Regras Críticas

1. **NUNCA use `@` dentro de comentários HTML**
   ```html
   @* ❌ ERRADO: Este bloco @Model.Propriedade faz X *@
   @* ✅ CORRETO: Este bloco Model.Propriedade faz X *@
   ```

2. **Use caracteres visuais para clareza**
   ```
   ⚡ = Crítico/Status importante
   🎯 = Objetivo/Propósito
   📥 = Entradas/Parâmetros
   📤 = Saídas/Respostas
   🔗 = Relacionamento/Linking
   🔄 = Fluxo/Chamadas
   📦 = Dependências
   📝 = Observação
   ✅ = OK/Bom
   ⚠️ = Aviso
   🔴 = Crítico/Erro
   ```

3. **Documente scripts inline com comentários**
   ```javascript
   @section ScriptBlock {
       <script>
           // ⚡ FUNÇÃO: nomeFunc()
           // PROPÓSITO: [Breve descrição]
           // LINHAS: [XXX-YYY] (se >50 linhas, recomendar extração)
           // DEPENDÊNCIAS: alerta.js, jquery

           function nomeFunc() {
               // implementação
           }

           // ⚡ EVENT HANDLER: click do #btnSalvar
           // LINHAS: [ZZZ-AAA]
           // CHAMADAS AJAX: POST /api/[Controller]/[Action]

           $('#btnSalvar').on('click', function() {
               // handler
           });
       </script>
   }
   ```

---

## 📂 Seção: PÁGINAS (Pages/)

### Listagem Completa dos 30 Primeiros Arquivos

#### Grupo: Abastecimento (7 arquivos)

**1. Pages/Abastecimento/DashboardAbastecimento.cshtml**
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 2401
- **Scripts Inline:** 1200+ (CSS 400+)
- **Status Qualidade:** ⚠️ CRÍTICO - Arquivo gigante
- **Recomendação:** Extrair CSS para arquivo separado, modularizar JavaScript
- **Intra-Código:** ✅ Card visual presente (linhas 1-11)
- **Arquivo MD:** ✅ Documentacao/Pages/Abastecimento - DashboardAbastecimento.md (TODO)

**2. Pages/Abastecimento/Importacao.cshtml**
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 2850+
- **Scripts Inline:** 1500+ (lógica NPOI)
- **Status Qualidade:** 🔴 CRÍTICO - Refatoração urgente
- **Recomendação:** Mover para arquivo importacao-abastecimento.js
- **Intra-Código:** ⚠️ Card visual NECESSÁRIO
- **Arquivo MD:** ❌ TODO

**3. Pages/Abastecimento/Index.cshtml**
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 1340
- **Scripts Inline:** 800+ (DataTable, filtros, modais)
- **Status Qualidade:** ⚠️ REQUER EXTRAÇÃO
- **Recomendação:** Extrair para index-abastecimento.js
- **Intra-Código:** ✅ Card visual presente (linhas 1-43)
- **Arquivo MD:** ✅ Documentacao/Pages/Abastecimento - Index.md (PARCIAL)

**4. Pages/Abastecimento/PBI.cshtml**
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 2000+
- **Scripts Inline:** 1000+ (Power BI embedding)
- **Status Qualidade:** ⚠️ CRÍTICO
- **Recomendação:** Documentar integração Power BI
- **Intra-Código:** ⚠️ Card visual NECESSÁRIO
- **Arquivo MD:** ❌ TODO

**5. Pages/Abastecimento/Pendencias.cshtml**
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 2200+
- **Scripts Inline:** 1100+ (filtros, modais, relatórios)
- **Status Qualidade:** ⚠️ CRÍTICO
- **Intra-Código:** ⚠️ Card visual NECESSÁRIO
- **Arquivo MD:** ❌ TODO

**6. Pages/Abastecimento/RegistraCupons.cshtml**
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 1000+
- **Scripts Inline:** 500+ (DataTable simples)
- **Status Qualidade:** ✅ ACEITÁVEL
- **Intra-Código:** ⚠️ Card visual NECESSÁRIO
- **Arquivo MD:** ❌ TODO

**7. Pages/Abastecimento/UpsertCupons.cshtml**
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 600+
- **Scripts Inline:** 300+ (validação de formulário)
- **Status Qualidade:** ✅ ACEITÁVEL
- **Intra-Código:** ⚠️ Card visual NECESSÁRIO
- **Arquivo MD:** ❌ TODO

---

#### Grupo: Administracao (6 arquivos)

**8. Pages/Administracao/AjustaCustosViagem.cshtml**
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 654
- **Scripts Inline:** 50+ (mínimo, referencia JS externo)
- **Status Qualidade:** ✅ COM @section ScriptsBlock
- **Intra-Código:** ✅ Card visual presente (linhas 1-11)
- **Arquivo MD:** ✅ Documentacao/Pages/Administracao - AjustaCustosViagem.md (EXISTE)
- **Scripts Externos:** `~/js/cadastros/atualizacustosviagem.js` ✅ Documentado

**9. Pages/Administracao/CalculaCustoViagensTotal.cshtml**
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 700+
- **Scripts Inline:** 350+ (cálculos complexos)
- **Status Qualidade:** ⚠️ REQUER EXTRAÇÃO
- **Intra-Código:** ⚠️ Card visual NECESSÁRIO
- **Arquivo MD:** ❌ TODO

**10. Pages/Administracao/DashboardAdministracao.cshtml**
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 1200+
- **Scripts Inline:** 600+ (charts, abas)
- **Status Qualidade:** ⚠️ CRÍTICO
- **Intra-Código:** ⚠️ Card visual NECESSÁRIO
- **Arquivo MD:** ❌ TODO

**11. Pages/Administracao/DocGenerator.cshtml**
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 2250+
- **Scripts Inline:** 1100+ (geração de documentação)
- **Status Qualidade:** 🔴 CRÍTICO - Refatoração urgente
- **Intra-Código:** ⚠️ Card visual NECESSÁRIO
- **Arquivo MD:** ❌ TODO

**12. Pages/Administracao/GerarEstatisticasViagens.cshtml**
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 950+
- **Scripts Inline:** 400+ (processamento background)
- **Status Qualidade:** ⚠️ REQUER EXTRAÇÃO
- **Intra-Código:** ⚠️ Card visual NECESSÁRIO
- **Arquivo MD:** ❌ TODO

**13. Pages/Administracao/GestaoRecursosNavegacao.cshtml** 🔴 PRIORIDADE MÁXIMA
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 5600+ (ARQUIVO GIGANTE)
- **Scripts Inline:** 2800+ (SEM ORGANIZAÇÃO)
- **Status Qualidade:** 🔴 CRÍTICO - REFATORAÇÃO URGENTE
- **Recomendação:** Dividir em 5 arquivos menores
- **Timeline Refatoração:** 4-6 horas
- **Intra-Código:** ⚠️ Card visual NECESSÁRIO
- **Arquivo MD:** ❌ TODO (Será criado após refatoração)

**14. Pages/Administracao/HigienizarViagens.cshtml**
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 450+
- **Scripts Inline:** 150+ (background job trigger)
- **Status Qualidade:** ✅ ACEITÁVEL
- **Intra-Código:** ⚠️ Card visual NECESSÁRIO
- **Arquivo MD:** ❌ TODO

**15. Pages/Administracao/LogErros.cshtml**
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 2000+
- **Scripts Inline:** 900+ (filtros, modais, paginação)
- **Status Qualidade:** ⚠️ CRÍTICO
- **Intra-Código:** ⚠️ Card visual NECESSÁRIO
- **Arquivo MD:** ❌ TODO

**16. Pages/Administracao/LogErrosDashboard.cshtml** ⚠️ PRIORIDADE ALTA
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 2800+
- **Scripts Inline:** 1400+ (charts complexos, real-time updates)
- **Status Qualidade:** ⚠️ CRÍTICO
- **Recomendação:** Extrair lógica de charts para arquivo separado
- **Intra-Código:** ⚠️ Card visual NECESSÁRIO
- **Arquivo MD:** ❌ TODO

---

#### Grupo: Agenda (1 arquivo)

**17. Pages/Agenda/Index.cshtml** ⚠️ PRIORIDADE ALTA
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 2008 (COMPLEXO)
- **Scripts Inline:** 1000+ (FullCalendar v6, recorrência)
- **Scripts Externos:** `~/js/cadastros/modal_agenda.js` ✅ Referenciado
- **Status Qualidade:** ⚠️ REQUER EXTRAÇÃO (1000+ linhas)
- **Recomendação:** Melhor organizar modal_agenda.js (1099 linhas)
- **Intra-Código:** ✅ Card visual presente (linhas 1-73)
- **Arquivo MD:** ✅ Documentacao/Pages/Agenda - Index.md (NECESSITA ATUALIZAÇÃO)

---

#### Grupo: AlertasFrotiX (2 arquivos)

**18. Pages/AlertasFrotiX/AlertasFrotiX.cshtml**
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 900+
- **Scripts Inline:** 450+ (SignalR, Hubs)
- **Status Qualidade:** ⚠️ REQUER EXTRAÇÃO
- **Intra-Código:** ⚠️ Card visual NECESSÁRIO
- **Arquivo MD:** ❌ TODO

**19. Pages/AlertasFrotiX/Upsert.cshtml**
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 1100+
- **Scripts Inline:** 550+ (validação, modal)
- **Status Qualidade:** ⚠️ REQUER EXTRAÇÃO
- **Intra-Código:** ⚠️ Card visual NECESSÁRIO
- **Arquivo MD:** ❌ TODO

---

#### Grupo: AtaRegistroPrecos (2 arquivos)

**20. Pages/AtaRegistroPrecos/Index.cshtml**
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 1500+
- **Scripts Inline:** 700+ (DataTable, filtros avançados)
- **Status Qualidade:** ⚠️ REQUER EXTRAÇÃO
- **Intra-Código:** ⚠️ Card visual NECESSÁRIO
- **Arquivo MD:** ❌ TODO

**21. Pages/AtaRegistroPrecos/Upsert.cshtml**
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 1800+
- **Scripts Inline:** 900+ (validação de itens, cálculos)
- **Status Qualidade:** ⚠️ REQUER EXTRAÇÃO
- **Intra-Código:** ⚠️ Card visual NECESSÁRIO
- **Arquivo MD:** ❌ TODO

---

#### Grupo: Combustivel (2 arquivos)

**22. Pages/Combustivel/Index.cshtml**
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 1400+
- **Scripts Inline:** 650+ (DataTable, CRUD)
- **Status Qualidade:** ⚠️ REQUER EXTRAÇÃO
- **Intra-Código:** ⚠️ Card visual NECESSÁRIO
- **Arquivo MD:** ❌ TODO

**23. Pages/Combustivel/Upsert.cshtml**
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 1100+
- **Scripts Inline:** 500+ (validação, cálculos)
- **Status Qualidade:** ⚠️ REQUER EXTRAÇÃO
- **Intra-Código:** ⚠️ Card visual NECESSÁRIO
- **Arquivo MD:** ❌ TODO

---

#### Grupo: Contrato (4 arquivos)

**24. Pages/Contrato/Index.cshtml** ⚠️ PRIORIDADE ALTA
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 2500+
- **Scripts Inline:** 1200+ (DataTable complexa, múltiplos modais)
- **Status Qualidade:** ⚠️ CRÍTICO
- **Recomendação:** Refatorar para modular JavaScript
- **Intra-Código:** ⚠️ Card visual NECESSÁRIO
- **Arquivo MD:** ❌ TODO

**25. Pages/Contrato/ItensContrato.cshtml**
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 1800+
- **Scripts Inline:** 900+ (linha-a-linha edição)
- **Status Qualidade:** ⚠️ REQUER EXTRAÇÃO
- **Intra-Código:** ⚠️ Card visual NECESSÁRIO
- **Arquivo MD:** ❌ TODO

**26. Pages/Contrato/RepactuacaoContrato.cshtml**
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 1600+
- **Scripts Inline:** 800+ (cálculos de reajuste)
- **Status Qualidade:** ⚠️ REQUER EXTRAÇÃO
- **Intra-Código:** ⚠️ Card visual NECESSÁRIO
- **Arquivo MD:** ❌ TODO

**27. Pages/Contrato/Upsert.cshtml** ⚠️ PRIORIDADE ALTA
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 2200+
- **Scripts Inline:** 1100+ (validação complexa, múltiplos campos)
- **Status Qualidade:** ⚠️ CRÍTICO
- **Intra-Código:** ⚠️ Card visual NECESSÁRIO
- **Arquivo MD:** ❌ TODO

---

#### Grupo: Empenho (2 arquivos)

**28. Pages/Empenho/Index.cshtml**
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 1700+
- **Scripts Inline:** 800+ (DataTable, modal)
- **Status Qualidade:** ⚠️ REQUER EXTRAÇÃO
- **Intra-Código:** ⚠️ Card visual NECESSÁRIO
- **Arquivo MD:** ❌ TODO

**29. Pages/Empenho/Upsert.cshtml**
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 1400+
- **Scripts Inline:** 700+ (validação de empenho)
- **Status Qualidade:** ⚠️ REQUER EXTRAÇÃO
- **Intra-Código:** ⚠️ Card visual NECESSÁRIO
- **Arquivo MD:** ❌ TODO

---

#### Grupo: Encarregado (1 arquivo)

**30. Pages/Encarregado/Index.cshtml**
- **Status:** 📋 DOCUMENTADO (Relatório_Lote_PAGES-001)
- **Linhas Totais:** 1300+
- **Scripts Inline:** 600+ (DataTable, CRUD)
- **Status Qualidade:** ⚠️ REQUER EXTRAÇÃO
- **Intra-Código:** ⚠️ Card visual NECESSÁRIO
- **Arquivo MD:** ❌ TODO

---

## 📊 Resumo de Status

### Documentação Intra-Código
- ✅ **Com Card Visual:** 3 arquivos (AjustaCustosViagem, Agenda, Abastecimento/Index)
- ⚠️ **Necessita Card:** 27 arquivos

### Documentação Externa (Arquivos .md)
- ✅ **Documentados:** 2 arquivos (AjustaCustosViagem, Agenda - PARCIAL)
- ❌ **Não Iniciados:** 28 arquivos

### Prioridade de Criação de Documentação

🔴 **MÁXIMA (Refatoração Urgente + Documentação)**
1. Pages/Administracao/GestaoRecursosNavegacao.cshtml (5600+ linhas)
2. Pages/Administracao/DocGenerator.cshtml (2250+)
3. Pages/Administracao/LogErrosDashboard.cshtml (2800+)

⚠️ **ALTA (Apenas Documentação)**
1. Pages/Contrato/Index.cshtml (2500+)
2. Pages/Contrato/Upsert.cshtml (2200+)
3. Pages/Agenda/Index.cshtml (2008+)
4. Pages/Abastecimento/Importacao.cshtml (2850+)
5. Pages/Abastecimento/DashboardAbastecimento.cshtml (2401+)

✅ **NORMAL (Documentação Padrão)**
- Restante dos 21 arquivos

---

## 📌 Próximas Ações

### Fase 1: Adicionar Card Intra-Código (7-10 horas)
Para cada um dos 30 arquivos:
1. Adicionar comentário visual no início do arquivo (15 min por arquivo)
2. Documentar scripts inline com indicações de extração
3. Validar nenhum `@` dentro de comentários

### Fase 2: Criar Arquivos .md de Documentação (20-25 horas)
Para cada arquivo:
1. Usar template do GuiaEnriquecimento.md
2. Mapear scripts, APIs, dependências
3. Adicionar recomendações

### Fase 3: Refatoração de Arquivos Críticos (10-15 horas)
1. GestaoRecursosNavegacao.cshtml → 5 arquivos
2. DocGenerator.cshtml → Modularização
3. LogErrosDashboard.cshtml → Extração de charts

---

**Última Atualização:** 03/02/2026 10:15
**Mantido por:** Sistema de Documentação FrotiX
**Status Geral Lote PAGES-001:** ⚠️ ENRIQUECIMENTO 15% COMPLETO

