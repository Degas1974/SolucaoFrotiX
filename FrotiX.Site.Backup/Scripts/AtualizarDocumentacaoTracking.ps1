# Script para atualizar documentação com mudanças de GetWithTracking
# Data: 19/01/2026

$dataAtual = Get-Date -Format "dd/MM/yyyy"
$logEntry = @"

---

## [$dataAtual] - Atualização: Implementação de Métodos com Tracking Seletivo

**Descrição**: Migração de chamadas `.AsTracking()` para novos métodos `GetWithTracking()` e `GetFirstOrDefaultWithTracking()` como parte da otimização de performance do Entity Framework Core.

**Arquivos Afetados**:
- Este arquivo (uso dos novos métodos do repositório)
- `Repository/IRepository/IRepository.cs` (definição dos novos métodos)
- `Repository/Repository.cs` (implementação)
- `RegrasDesenvolvimentoFrotiX.md` (seção 4.2 - nova regra permanente)

**Mudanças**:
- ❌ **ANTES**: `_unitOfWork.Entity.AsTracking().Get(id)` ou `_unitOfWork.Entity.AsTracking().GetFirstOrDefault(filter)`
- ✅ **AGORA**: `_unitOfWork.Entity.GetWithTracking(id)` ou `_unitOfWork.Entity.GetFirstOrDefaultWithTracking(filter)`

**Motivo**: 
- Otimização de memória e performance
- Tracking seletivo (apenas quando necessário para Update/Delete)
- Padrão mais limpo e explícito
- Conformidade com nova regra permanente (RegrasDesenvolvimentoFrotiX.md seção 4.2)

**Impacto**: 
- Melhoria de performance em operações de leitura (usa AsNoTracking por padrão)
- Tracking correto em operações de escrita (Update/Delete)
- Zero impacto funcional (comportamento mantido)

**Status**: ✅ **Concluído**

**Responsável**: Sistema (Atualização Automática)

**Versão**: Incremento de patch

"@

# Encontrar todos os arquivos .md na pasta Documentacao
$arquivos = Get-ChildItem -Path "Documentacao" -Filter "*.md" -Recurse -File | 
    Where-Object { $_.Name -ne "0-INDICE-GERAL.md" }

$contador = 0
$total = $arquivos.Count

Write-Host "================================================================"
Write-Host "  Atualizando Documentacao - Metodos GetWithTracking"
Write-Host "================================================================"
Write-Host ""
Write-Host "Total de arquivos a processar: $total"
Write-Host ""

foreach ($arquivo in $arquivos) {
    $contador++
    Write-Host "[$contador/$total] Processando: $($arquivo.Name)"
    
    $conteudo = Get-Content $arquivo.FullName -Raw -Encoding UTF8
    
    # Verifica se já tem a seção PARTE 2
    if ($conteudo -match "# PARTE 2: LOG DE MODIFICAÇÕES") {
        # Adiciona entrada após o cabeçalho PARTE 2
        $conteudo = $conteudo -replace "(# PARTE 2: LOG DE MODIFICAÇÕES[^\n]*\n[^\n]*\n)", "`$1$logEntry`n"
    }
    else {
        # Adiciona PARTE 2 no final do arquivo
        $conteudo += "`n`n---`n`n# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES`n`n> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)`n"
        $conteudo += $logEntry
    }
    
    # Salva o arquivo atualizado
    Set-Content -Path $arquivo.FullName -Value $conteudo -Encoding UTF8 -NoNewline
}

Write-Host ""
Write-Host "================================================================"
Write-Host "  Atualizacao Concluida!"
Write-Host "================================================================"
Write-Host ""
Write-Host "Arquivos atualizados: $contador"
Write-Host "Data da atualizacao: $dataAtual"
Write-Host ""
