# Documentação: Servicos.cs

> **Última Atualização**: 23/01/2026 13:15  
> **Versão**: 1.0  
> **Documentação Intra-Código**: ✅ Completa

---

# PARTE 1: VISÃO GERAL

Biblioteca estática de utilitários e funções auxiliares do FrotiX (versão síncrona).

## Categorias de Funções

### 1. Cálculos de Viagem (Síncronos)
- CalculaCustoCombustivel
- CalculaCustoPedagio
- CalculaCustoManutencao
- CalculaCustoTotal

### 2. Formatações
- FormatarMoeda
- FormatarData
- FormatarCPF/CNPJ
- FormatarPlaca

### 3. Validações Rápidas
- ValidarEmail
- ValidarTelefone
- ValidarCEP

### 4. Conversões
- StringToDecimal
- StringToDateTime
- Base64ToImage

### 5. Utilitários
- GerarCodigoAleatorio
- CriptografarSenha
- CompararHashes

## Nota Importante
Este arquivo contém versões **síncronas** de funções também disponíveis em **ServicosAsync.cs**. Prefira versões async quando possível para melhor performance.

## Uso Típico

\\\csharp
// Formatação
var valorFormatado = Servicos.FormatarMoeda(1234.56); // "R$ 1.234,56"

// Validação
if (Servicos.ValidarEmail(email)) { ... }

// Conversão segura
var valor = Servicos.StringToDecimal("1.234,56"); // 1234.56
\\\

---

# PARTE 2: LOG DE MODIFICAÇÕES

## [23/01/2026 13:15] - Documentação Completa com Qualidade Máxima
**Descrição**: Biblioteca de utilitários completa e documentada  
**Status**: ✅ Concluído
