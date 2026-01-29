# 🤖 PROMPT PARA CONTINUAÇÃO - LOTE 19

## 📋 CONTEXTO DO PROJETO

Você está continuando um projeto de **documentação intra-código** para o sistema **FrotiX** (Sistema de Gestão de Frotas).

**O que foi feito até agora:**
- Lotes 11-18: 187 arquivos já documentados (Models, Pages, Views, Cadastros)
- Lote 19 (parcial): 10 de 14 arquivos documentados no ViagemController
- **Restam 4 arquivos para completar o Lote 19**

**Seu objetivo:**
Adicionar cabeçalhos de documentação ASCII (box-drawing) nos 4 arquivos pendentes do ViagemController.

---

## 📂 ARQUIVOS PENDENTES (4 arquivos)

### 1. ViagemController.HeatmapEconomildoPassageiros.cs
**Caminho:** `/mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Controllers/ViagemController.HeatmapEconomildoPassageiros.cs`

**Descrição:** Heatmap de PASSAGEIROS do Economildo. Retorna matriz 7x24 com SOMA de passageiros por dia da semana e hora.

**Endpoint:** `GET /api/Viagem/HeatmapEconomildoPassageiros`

**Ação:** Adicionar header ASCII antes dos `using` statements.

---

### 2. ViagemController.ListaEventos.cs
**Caminho:** `/mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Controllers/ViagemController.ListaEventos.cs`

**Descrição:** Lista eventos SUPER OTIMIZADO com paginação server-side (DataTables). Performance: < 2 segundos (vs 30+ segundos timeout da versão anterior).

**Endpoint:** `GET /api/Viagem/ListaEventos`

**Otimizações:**
- Paginação server-side (25 registros por vez)
- Agregação de custos apenas da página atual
- Queries com AsNoTracking

**Ação:** Adicionar header ASCII antes dos `using` statements.

---

### 3. ViagemController.MetodosEstatisticas.cs
**Caminho:** `/mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Controllers/ViagemController.MetodosEstatisticas.cs`

**Descrição:** Geração de estatísticas de viagens em background. Processa viagens em lotes e atualiza tabela ViagemEstatistica.

**Endpoints:**
- `POST /api/Viagem/GerarEstatisticasViagens` : Inicia geração em background
- `GET /api/Viagem/ObterProgressoEstatisticas` : Obtém progresso

**Classes Auxiliares:** `ProgressoEstatisticas` (controle de progresso)

**Ação:** Adicionar header ASCII antes dos `using` statements.

---

### 4. ViagemEventoController.UpdateStatus.cs
**Caminho:** `/mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Controllers/ViagemEventoController.UpdateStatus.cs`

**Descrição:** Partial class para atualização de status de eventos. Alterna entre Ativo ("1") e Inativo ("0").

**Endpoint:** `GET /api/ViagemEvento/UpdateStatusEvento?Id={guid}`

**Ação:** Adicionar header ASCII antes dos `using` statements.

---

## 📝 TEMPLATE DE DOCUMENTAÇÃO

Use este template para TODOS os 4 arquivos:

```csharp
/*
╔══════════════════════════════════════════════════════════════════════════════╗
║                    DOCUMENTACAO INTRA-CODIGO - FROTIX                        ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ Arquivo    : [NOME_DO_ARQUIVO.cs]                                            ║
║ Projeto    : FrotiX.Site                                                     ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ DESCRICAO                                                                    ║
║ [Descrição específica do arquivo - usar a descrição fornecida acima]         ║
║ [Incluir detalhes técnicos relevantes]                                       ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ ENDPOINTS                                                                    ║
║ - [MÉTODO] /rota : Descrição                                                 ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ [SEÇÕES ADICIONAIS conforme necessário]                                      ║
║ - CLASSES AUXILIARES                                                         ║
║ - METODOS AUXILIARES                                                         ║
║ - OTIMIZACOES                                                                ║
║ - DADOS RETORNADOS                                                           ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ Data Documentacao: 28/01/2026                              LOTE: 19          ║
╚══════════════════════════════════════════════════════════════════════════════╝
*/
```

---

## 🔧 INSTRUÇÕES DE EXECUÇÃO

### Passo 1: Ler o Arquivo
```
Read tool: file_path="/caminho/completo/arquivo.cs", limit=50
```

**Objetivo:** Ver as primeiras 50 linhas para entender a estrutura e verificar se já tem documentação.

---

### Passo 2: Preparar o Header
Com base nas informações fornecidas acima, crie o header ASCII completo seguindo o template.

**Seções importantes para cada arquivo:**

**Para HeatmapEconomildoPassageiros.cs:**
```
║ DESCRICAO                                                                    ║
║ Partial class do ViagemController para geração de Heatmap de PASSAGEIROS do  ║
║ Economildo. Retorna matriz 7x24 com SOMA de passageiros por dia/hora.        ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ ENDPOINTS                                                                    ║
║ - GET /api/Viagem/HeatmapEconomildoPassageiros : Matriz de passageiros       ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ DADOS RETORNADOS                                                             ║
║ - heatmap[7,24] : Matriz com soma de passageiros                             ║
║ - maxValor      : Valor máximo para escala                                   ║
```

**Para ListaEventos.cs:**
```
║ DESCRICAO                                                                    ║
║ Partial class do ViagemController com endpoint ListaEventos SUPER OTIMIZADO. ║
║ Implementa paginação server-side (DataTables), carregando apenas 25          ║
║ registros por vez. Performance: < 2 segundos (vs 30+ segundos timeout).      ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ ENDPOINTS                                                                    ║
║ - GET /api/Viagem/ListaEventos : Lista eventos com paginação server-side     ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ OTIMIZACOES                                                                  ║
║ - Paginação server-side (25 registros por vez)                               ║
║ - Agregação de custos apenas da página atual                                 ║
║ - Queries com AsNoTracking                                                   ║
```

**Para MetodosEstatisticas.cs:**
```
║ DESCRICAO                                                                    ║
║ Partial class do ViagemController com métodos para geração de estatísticas   ║
║ de viagens em background. Processa viagens em lotes e atualiza tabela        ║
║ ViagemEstatistica com dados agregados.                                       ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ ENDPOINTS                                                                    ║
║ - POST /api/Viagem/GerarEstatisticasViagens : Inicia geração em background   ║
║ - GET  /api/Viagem/ObterProgressoEstatisticas : Obtém progresso              ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ CLASSES AUXILIARES                                                           ║
║ - ProgressoEstatisticas : Controle de progresso (total, processado, %)       ║
```

**Para UpdateStatus.cs:**
```
║ DESCRICAO                                                                    ║
║ Partial class do ViagemEventoController para atualização de status de        ║
║ eventos. Alterna entre Ativo ("1") e Inativo ("0").                          ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ ENDPOINTS                                                                    ║
║ - GET /api/ViagemEvento/UpdateStatusEvento?Id={guid} : Alterna status        ║
```

---

### Passo 3: Aplicar a Documentação
```
Edit tool:
  file_path: "/caminho/completo/arquivo.cs"
  old_string: "using FrotiX.Models;\nusing FrotiX.Repository...\n...\nnamespace FrotiX.Controllers\n{\n    public partial class..."
  new_string: "/*\n╔══════...\n[HEADER COMPLETO]\n╚══════╝\n*/\n\nusing FrotiX.Models;\nusing FrotiX.Repository...\n...\nnamespace FrotiX.Controllers\n{\n    public partial class..."
```

**ATENÇÃO:**
- Copiar EXATAMENTE o conteúdo original após o header
- Manter todos os `using` statements
- Preservar espaçamento e formatação
- O header vai ANTES dos `using` statements

---

### Passo 4: Verificar
Após editar cada arquivo, fazer um Read rápido para confirmar:
```
Read tool: file_path="/caminho/arquivo.cs", limit=50
```

Verificar se:
- ✅ Header ASCII está correto
- ✅ Data está como 28/01/2026
- ✅ LOTE está como 19
- ✅ Using statements estão preservados
- ✅ Código original não foi alterado

---

## ✅ CHECKLIST DE CONCLUSÃO

Após documentar os 4 arquivos, verificar:

- [ ] ViagemController.HeatmapEconomildoPassageiros.cs documentado
- [ ] ViagemController.ListaEventos.cs documentado
- [ ] ViagemController.MetodosEstatisticas.cs documentado
- [ ] ViagemEventoController.UpdateStatus.cs documentado
- [ ] Todos os headers têm a data 28/01/2026
- [ ] Todos os headers têm LOTE: 19
- [ ] Nenhum código funcional foi alterado
- [ ] Headers seguem o padrão ASCII box-drawing

---

## 📊 RESUMO PARA RELATÓRIO FINAL

**Lote 19 - Status Final:**
- ✅ 14/14 arquivos documentados (100%)
- ✅ ViagemController completo (11 arquivos parciais)
- ✅ ViagemEventoController completo (2 arquivos)
- ✅ ViagemLimpezaController completo (1 arquivo)

**Total Projeto:**
- Lotes 11-19: ~211 arquivos documentados

---

## 🎯 EXEMPLO COMPLETO (Arquivo 1)

**Arquivo:** ViagemController.HeatmapEconomildoPassageiros.cs

**ANTES:**
```csharp
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
    public partial class ViagemController
    {
        #region Heatmap Economildo Passageiros
```

**DEPOIS:**
```csharp
/*
╔══════════════════════════════════════════════════════════════════════════════╗
║                    DOCUMENTACAO INTRA-CODIGO - FROTIX                        ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ Arquivo    : ViagemController.HeatmapEconomildoPassageiros.cs                ║
║ Projeto    : FrotiX.Site                                                     ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ DESCRICAO                                                                    ║
║ Partial class do ViagemController para geração de Heatmap de PASSAGEIROS do  ║
║ Economildo. Retorna matriz 7x24 com SOMA de passageiros por dia/hora.        ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ ENDPOINTS                                                                    ║
║ - GET /api/Viagem/HeatmapEconomildoPassageiros : Matriz de passageiros       ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ Data Documentacao: 28/01/2026                              LOTE: 19          ║
╚══════════════════════════════════════════════════════════════════════════════╝
*/

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
    public partial class ViagemController
    {
        #region Heatmap Economildo Passageiros
```

---

## 🚀 COMANDO INICIAL

Comece com este comando:

```
Por favor, continuar a documentação do Lote 19 do projeto FrotiX.

Existem 4 arquivos pendentes que precisam receber headers de documentação ASCII.

Vou processar os arquivos na seguinte ordem:
1. ViagemController.HeatmapEconomildoPassageiros.cs
2. ViagemController.ListaEventos.cs
3. ViagemController.MetodosEstatisticas.cs
4. ViagemEventoController.UpdateStatus.cs

Começando pelo primeiro arquivo...
```

---

**BOA SORTE! 🎉**

Lembre-se:
- Usar Read tool primeiro para ver o arquivo
- Preparar o header seguindo o template
- Aplicar com Edit tool preservando o código original
- Verificar com Read tool após aplicar
- Marcar no checklist após cada arquivo

**FIM DO PROMPT**
