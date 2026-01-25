# Documentação: ViagemController.cs

> **Última Atualização**: 22/01/2026 16:55
> **Versão Atual**: 2.8

---

## PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O `ViagemController` é um dos controllers mais complexos e críticos do sistema FrotiX. Gerencia todas as operações relacionadas a viagens, incluindo CRUD, upload de fichas de vistoria, cálculos de custos, estatísticas, unificação de dados e integração com múltiplos serviços.

**Principais características:**

✅ **CRUD Completo**: Criação, leitura, atualização e exclusão de viagens  
✅ **Upload de Fichas**: Upload e recuperação de fichas de vistoria em Base64  
✅ **Cálculos de Custo**: Sistema complexo de cálculo de custos (combustível, motorista, operador, lavador, veículo)  
✅ **Estatísticas**: Integração com `ViagemEstatisticaService` e `VeiculoEstatisticaService`  
✅ **Unificação**: Sistema de unificação de origens/destinos  
✅ **Filtros Avançados**: Por veículo, motorista, data, status, evento  
✅ **Cache**: Usa `IMemoryCache` para otimização  
✅ **Batch Processing**: Cálculo de custos em lote com progresso

**⚠️ CRÍTICO**: Qualquer alteração afeta o core do sistema de gestão de viagens.

**Nota**: Controller implementado como partial class dividido em múltiplos arquivos:

- `ViagemController.cs` - Métodos principais
- `ViagemController.AtualizarDados.cs` - Atualização de dados
- `ViagemController.CalculoCustoBatch.cs` - Cálculo de custos em lote
- `ViagemController.CustosViagem.cs` - Gestão de custos
- `ViagemController.DashboardEconomildo.cs` - Dashboard Economildo
- `ViagemController.HeatmapEconomildo.cs` - Heatmap Economildo
- E outros arquivos parciais

---

## Endpoints API Principais

### POST `/api/Viagem/UploadFichaVistoria`

**Descrição**: Upload de ficha de vistoria (imagem) para uma viagem

**Request**: `multipart/form-data` com arquivo e `viagemId`

**Response**: Base64 da imagem salva

---

### GET `/api/Viagem/ObterFichaVistoria`

**Descrição**: Obtém ficha de vistoria de uma viagem

**Parâmetros**: `viagemId` (string GUID)

**Response**: Base64 da imagem ou `temImagem: false`

---

### GET `/api/Viagem/FotoMotorista`

**Descrição**: Obtém foto do motorista com cache HTTP (ETag)

**Parâmetros**: `id` (Guid) - ID do motorista

**Otimizações**:

- Usa ETag para cache HTTP
- Retorna `304 Not Modified` se não houver mudanças
- Cache-Control: `public,max-age=86400`

---

### GET `/api/Viagem/PegarStatusViagem`

**Descrição**: Verifica se viagem está aberta

**Parâmetros**: `viagemId` (Guid)

**Response**: `true` se status == "Aberta", senão `false`

---

### GET `/api/Viagem/ListaDistintos`

**Descrição**: Lista origens e destinos distintos de todas as viagens

**Response**:

```json
{
  "origens": ["Local A", "Local B"],
  "destinos": ["Local C", "Local D"]
}
```

---

### POST `/api/Viagem/Unificar`

**Descrição**: Unifica múltiplas origens/destinos em um único valor

**Request Body**: `UnificacaoRequest`

```json
{
  "novoValor": "Local Unificado",
  "origensSelecionadas": ["Local A", "Local B"],
  "destinosSelecionados": ["Local C"]
}
```

**Uso**: Normalização de dados de viagens

---

## Interconexões

### Quem Chama Este Controller

- **Pages**: `Pages/Viagem/*.cshtml` - Todas as páginas de viagens
- **Pages**: `Pages/Dashboard/*.cshtml` - Dashboards
- **JavaScript**: Múltiplos arquivos JS para operações de viagens

### O Que Este Controller Chama

- **`_unitOfWork.Viagem`**: CRUD de viagens
- **`_unitOfWork.ViewViagens`**: View otimizada
- **`_viagemEstatisticaService`**: Estatísticas de viagens
- **`_veiculoEstatisticaService`**: Estatísticas de veículos
- **`_fotoService`**: Serviço de fotos de motoristas
- **`_cache`**: Cache de memória
- **`_context`**: DbContext para operações complexas

---

## Notas Importantes

1. **Partial Class**: Controller dividido em múltiplos arquivos para organização
2. **Cálculos de Custo**: Sistema complexo com múltiplos serviços
3. **Estatísticas**: Atualização automática após mudanças
4. **Cache**: Usa cache HTTP e memória para otimização
5. **Batch Processing**: Suporta processamento em lote com progresso

---

## PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [22/01/2026 16:55] - FIX: Qualificação de Claims

**Descrição**: Uso explícito de `System.Security.Claims.ClaimsPrincipal` e `System.Security.Claims.ClaimTypes` para evitar erros de resolução de namespace.

**Arquivos Afetados**:

- `Controllers/ViagemController.cs`

**Impacto**: Compilação consistente sem depender de diretivas `using`.

**Status**: ✅ **Concluído**

**Versão**: 2.8

---

## [22/01/2026 16:50] - DOC: Ajustes de formatação

**Descrição**: Ajustes de formatação Markdown (títulos) e remoção de seção duplicada com texto corrompido.

**Arquivos Afetados**:

- `Documentacao/Controllers/ViagemController.md`

**Impacto**: Documento válido para lint e sem duplicidades.

**Status**: ✅ **Concluído**

**Versão**: 2.7

---

## [22/01/2026 16:30] - FIX: Restauração de usings de segurança

**Descrição**: Reintroduzidos `using System.Security.Claims` e `using System.Security.Cryptography` após erros de compilação em `ClaimsPrincipal`, `ClaimTypes` e `SHA256`.

**Arquivos Afetados**:

- `Controllers/ViagemController.cs`

**Impacto**: Build volta a reconhecer tipos de segurança e criptografia sem impacto funcional.

**Status**: ✅ **Concluído**

**Versão**: 2.6

---

## [22/01/2026 16:10] - FIX: Restauração de Task namespace

**Descrição**: Reintroduzido `using System.Threading.Tasks` após identificar erros CS0246 para `Task`/`Task<>`.

**Arquivos Afetados**:

- `Controllers/ViagemController.cs`

**Impacto**: Build volta a reconhecer tipos assíncronos sem impactar funcionalidade.

**Status**: ✅ **Concluído**

**Versão**: 2.5

---

## [22/01/2026 15:40] - FIX: Ajuste final de usings (CS0105)

**Descrição**: Removidas diretivas `using` redundantes para `System.Security.*` e `System.Threading.Tasks` para eliminar avisos remanescentes de duplicidade.

**Arquivos Afetados**:

- `Controllers/ViagemController.cs`

**Impacto**: Redução adicional de avisos CS0105 sem alteração funcional.

**Status**: ✅ **Concluído**

**Versão**: 2.4

---

## [22/01/2026 15:00] - FIX: Remoção de using duplicados

**Descrição**: Removidas diretivas `using` duplicadas no topo do controller para eliminar avisos de compilação.

**Arquivos Afetados**:

- `Controllers/ViagemController.cs`

**Impacto**: Redução de avisos CS0105 sem alteração funcional.

**Status**: ✅ **Concluído**

**Versão**: 2.3

---

## [21/01/2026] - Preparação para Feature Ficha de Vistoria Real

**Descrição**: Adicionada lógica no método `UploadFichaVistoria` para detectar automaticamente se a ficha sendo enviada é a ficha padrão (amarelinha) ou uma ficha real cadastrada pelo usuário.

**Alterações**:

- Detecta nome do arquivo (`fichaamarelanova`, `ficha_amarela`, `fichapadrao`)
- Define campo `TemFichaVistoriaReal` como `true` para fichas reais, `false` para fichas padrão
- Prepara a base para botão de visualização de ficha no modal

**Arquivos Afetados**:

- `Controllers/ViagemController.cs` (linhas ~204-216)

**Impacto**: Permite distinguir viagens com fichas reais das que usam a ficha padrão

**Status**: ✅ **Concluído**

**Versão**: 2.2

---

## [14/01/2026] - Correção do cálculo de Litros Gastos no modal de custos

**Descrição**:

- Removida lógica incorreta que atribuía litros de abastecimentos do período à viagem
- **Problema**: Se um veículo era abastecido no dia da viagem, o sistema atribuía TODOS os litros abastecidos à viagem, mesmo que o combustível fosse para múltiplas viagens futuras
- **Solução**: Agora o cálculo usa apenas: `km percorrido / consumo médio do veículo (km/L)`
- Exemplo: Uma viagem de 11km com consumo médio de 10km/L agora mostra corretamente ~1,1L gastos (não 67,53L de um abastecimento)

**Arquivos Afetados**:

- `Controllers/ViagemController.CustosViagem.cs` (linhas 135-142)

**Impacto**: Modal de detalhamento de custos da viagem agora mostra valores corretos de litros gastos

**Status**: ✅ **Concluído**

**Versão**: 2.1

---

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação da documentação completa do ViagemController

**Arquivos Afetados**:

- `Controllers/ViagemController.cs`
- `Controllers/ViagemController.*.cs` (múltiplos arquivos parciais)

**Impacto**: Documentação de referência para operações de viagens

**Status**: ✅ **Concluído**

**Versão**: 2.0

---

**Última atualização**: 08/01/2026  
**Autor**: Sistema FrotiX  
**Versão**: 2.0

---
